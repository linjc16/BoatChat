using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls.Primitives;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Data.SQLite;
using System.Data;
using System.IO;
using ESBasic;
using Oraycn.MCapture;
using Oraycn.MFile;
namespace BoatChat
{
    /// <summary>
    /// ChatWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChatWindow : Window
    {
        /******************************
        *  聊天记录类
        ******************************/

        //单条聊天记录纪录
        public class chatHistorySingle
        {
            public string Speaker;     //谁说的话
            public int chatid;
            public string Mess;
            public string Date = "2019/1/1";
            public string ModeFile = "0"; 
            public chatHistorySingle(string speaker, int Chatid, string mess,string modefile,string date)
            {
                Speaker = speaker;
                chatid = Chatid;
                Mess = mess;
                ModeFile = modefile;
                Date = date;
            }
        }

        //整个聊天记录
        public class chatHistory
        {
            public string Monitor;     //主机（谁登录的）
            public string Contact;       //给谁发
            public List<chatHistorySingle> History = new List<chatHistorySingle>();
            public int preLoadHistoryID = 1;
            public chatHistory(string monitor, string contact)
            {
                Monitor = monitor;
                Contact = contact;
            }
            public void AddHistory(chatHistorySingle chs)
            {
                History.Add(chs);
            }
        }

        public string myUsername;
        public ServerConnct serverConnct;
        P2PModule P2PModule;
        public List<string> ContactsName = new List<string>();
        public List<string> ContactsIP = new List<string>();
        public List<string> GroupNmae = new List<string>();
        public List<chatHistory> chatHistoryList = new List<chatHistory>();

        private int DialogMode = 0; // 0 添加好友 1 添加群聊 2 查询好友 3 退出
        private int MessMode = 0; // 0 为正常消息，1为图片，2为文件，3位表情包
        private int lenStdMess = 14;
        Thread ListeningTread; //侦听线程
        Thread MonitorNewMessThread; //有无新消息
        bool islistening = false;
        string CurrContact;//记录当前联系人
        public ChatWindow(ServerConnct sc)
        {
            InitializeComponent();
            //ChatId.IsEnabled = false;
            serverConnct = sc;
            myUsername = MainWindow.LOGINWINDOW.username;
            ContactsName.Add(myUsername);
            ContactsIP.Add(serverConnct.ServerQuery("q" + myUsername));

            chatHistory cHtmp = new chatHistory(myUsername, myUsername);
            chatHistoryList.Add(cHtmp);
            P2PModule = new P2PModule(ContactsIP[0]);

            TextBlock tb = new TextBlock();
            TreeViewItem NewContact = new TreeViewItem();
            NewContact.Header = myUsername+"(Me)";
            NewContact.PreviewMouseLeftButtonDown += TreeviewItem_PreviewMouseLeftButtonDown;

            /*Button bt = new Button();
            bt.Height = 10;
            bt.Width = 10;
            bt.SetResourceReference(StyleProperty, "MaterialDesignFloatingActionMiniAccentButton");
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            sp.Children.Add(bt);
            sp.Children.Add(NewContact);*/
            //更新List
            Contacts_.Items.Add(NewContact);
            ChatId.Text = myUsername + "(Me)";
            CurrContact = myUsername;
            LoadfromDataBase();
            StartListening();
            //SavetoDataBase();
        }



        /************************************
         * 数据库操作
         ***********************************/
        public void test()
        {
            string dbFilename = "test.db3";
            SQLiteConnection dbCore = new SQLiteConnection("data source=" + dbFilename);
            dbCore.Open();
            string commandText = "CREATE TABLE IF NOT EXISTS t1(ID INT NOT NULL, NAME NTEXT NOT NULL);";
            SQLiteCommand cmd = new SQLiteCommand(commandText, dbCore);
            int result = cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO t1(ID,NAME) VALUES(@ID, @NAME)";
            cmd.Parameters.Add("ID", DbType.Int32).Value = 125;
            cmd.Parameters.Add("NAME", DbType.String).Value = "Test";
            result = cmd.ExecuteNonQuery();
            Console.WriteLine("Result = {0}", result);
            dbCore.Close();
        }
         public void SavetoDataBase()
        {
            string dbFilename = "ChatHistory.db3";
            SQLiteConnection dbCore = new SQLiteConnection("data source=" + dbFilename);
            dbCore.Open();
            string commandText = "CREATE TABLE IF NOT EXISTS t1(MONITOR NTEXT NOT NULL, SPEAKER NTEXT NOT NULL, " +
                "PEER NTEXT NOT NULL, CONTENT NTEXT NOT NULL, DATE NTEXT NOT NULL, KIND NTEXT NOT NULL, ID INT NOT NULL);";
            SQLiteCommand cmd = new SQLiteCommand(commandText, dbCore);
            int result = cmd.ExecuteNonQuery();
            Console.WriteLine(chatHistoryList.Count);
            for (int i = 0; i < chatHistoryList.Count(); i++)
            {
                for (int j = chatHistoryList[i].preLoadHistoryID; j < chatHistoryList[i].History.Count; j++) 
                {
                    cmd.CommandText = "INSERT INTO t1(MONITOR,SPEAKER,PEER,CONTENT,DATE,KIND,ID)" +
                        " VALUES(@MONITOR, @SPEAKER, @PEER, @CONTENT, @DATE, @KIND, @ID)";
                    cmd.Parameters.Add("MONITOR", DbType.String).Value = myUsername;
                    cmd.Parameters.Add("SPEAKER", DbType.String).Value = chatHistoryList[i].History[j].Speaker;
                    cmd.Parameters.Add("PEER", DbType.String).Value = chatHistoryList[i].Contact;
                    cmd.Parameters.Add("CONTENT", DbType.String).Value = chatHistoryList[i].History[j].Mess;
                    cmd.Parameters.Add("DATE", DbType.String).Value = chatHistoryList[i].History[j].Date;
                    cmd.Parameters.Add("KIND", DbType.String).Value = chatHistoryList[i].History[j].ModeFile;
                    cmd.Parameters.Add("ID", DbType.String).Value = chatHistoryList[i].History[j].chatid;
                    result = cmd.ExecuteNonQuery();
                }

            }
            dbCore.Close();
        }

        public void LoadfromDataBase()
        {
            string dbFilename = "ChatHistory.db3";
            SQLiteConnection dbCore = new SQLiteConnection("data source=" + dbFilename);
            dbCore.Open();
            string commandText = "CREATE TABLE IF NOT EXISTS t1(MONITOR NTEXT NOT NULL, SPEAKER NTEXT NOT NULL, " +
                "PEER NTEXT NOT NULL, CONTENT NTEXT NOT NULL, DATE NTEXT NOT NULL, KIND NTEXT NOT NULL, ID INT NOT NULL);";
            SQLiteCommand cmdCreate = new SQLiteCommand(commandText, dbCore);
            cmdCreate.ExecuteNonQuery();
            string commandText2 = "select * from t1;";
            SQLiteCommand cmd = new SQLiteCommand(commandText2, dbCore);
            int result = cmd.ExecuteNonQuery();
            SQLiteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string monitor = (string)reader["MONITOR"];       //主机
                if (monitor != myUsername)
                    continue;
                string speaker = (string)reader["SPEAKER"];       //谁说的消息 History[].username
                string peer = (string)reader["PEER"];           //对方是谁Contact
                string content = (string)reader["CONTENT"];         
                string data = (string)reader["DATE"];
                string kind = (string)reader["KIND"];
                int id = (int)reader["ID"];
                int Contactindex = chatHistoryList.FindIndex(s => s.Contact == peer);
                //不存在则新建
                if(Contactindex == -1)
                {
                    chatHistory chTmp = new chatHistory(monitor, peer);
                    chatHistorySingle chsTmp = new chatHistorySingle(speaker, id, content, kind, "2019/1/1");
                    chTmp.AddHistory(chsTmp);
                    chatHistoryList.Add(chTmp);
                }
                else
                {
                    chatHistorySingle chsTmp = new chatHistorySingle(speaker, id, content, kind, "2019/1/1");
                    chatHistoryList[Contactindex].AddHistory(chsTmp);
                }
            }
            //重构界面
            for (int i = 0; i < chatHistoryList.Count; i++)
            {
                chatHistoryList[i].preLoadHistoryID = chatHistoryList[i].History.Count;
                //自己给自己发的记录不恢复
                if (chatHistoryList[i].Contact == myUsername)
                    continue;
                //菜单栏加一个
                TextBlock tb = new TextBlock();
                TreeViewItem NewContact = new TreeViewItem();
                NewContact.Header = chatHistoryList[i].Contact;
                NewContact.PreviewMouseLeftButtonDown += TreeviewItem_PreviewMouseLeftButtonDown;
                /*Button bt = new Button();
                bt.Height = 10;
                bt.Width = 10;
                bt.SetResourceReference(StyleProperty, "MaterialDesignFloatingActionMiniAccentButton");
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.Children.Add(bt);
                sp.Children.Add(NewContact);*/
                //更新List
                Contacts_.Items.Add(NewContact);
                //将信息存入内存
                ContactsName.Add(chatHistoryList[i].Contact);
                ContactsIP.Add(serverConnct.ServerQuery("q" + chatHistoryList[i].Contact));
            }
            dbCore.Close();
        }

        private void findContacts_Click(object sender, RoutedEventArgs e)
        {
            SearchFriend.Text = null;
            DialogMode = 2;
            CheckLabel.Visibility = Visibility.Visible;
            SearchFriend.Text = "Find Contact";
            SearchIDTB.Visibility = Visibility.Visible;
            DlgHost.IsOpen = true;
        }
        private  void MenuPopupButton_OnClick(object sender, RoutedEventArgs e)
        {
            SearchFriend.Text = null;
            SearchFriend.Text = "Quit Now?";
            CheckLabel.Visibility = Visibility.Hidden;
            SearchIDTB.Visibility = Visibility.Hidden;
            DialogMode = 3;
            DlgHost.IsOpen = true;
        }

        private void Quit_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, true)) return;

            switch (DialogMode)
            {
                //查询/加好友
                case 0:
                    string strReceive = serverConnct.ServerQuery("q" + SearchIDTB.Text);
                    if(strReceive==null || strReceive == "Please send the correct message." || strReceive == "Incorrect No.")
                    {
                        //失败
                        MessageBox.Show("Wrong Format.");
                    }
                    else if(strReceive == "n")
                    {
                        MessageBox.Show("Not Online.");
                    }
                    
                    else if (!IsExist(SearchIDTB.Text))
                    {
                        TextBlock tb = new TextBlock();
                        TreeViewItem NewContact = new TreeViewItem();
                        NewContact.Header = SearchIDTB.Text;
                        NewContact.PreviewMouseLeftButtonDown += TreeviewItem_PreviewMouseLeftButtonDown;
                        /*Button bt = new Button();
                        bt.Height = 10;
                        bt.Width = 10;
                        bt.SetResourceReference(StyleProperty, "MaterialDesignFloatingActionMiniAccentButton");
                        StackPanel sp = new StackPanel();
                        sp.Orientation = Orientation.Horizontal;
                        sp.Children.Add(bt);
                        sp.Children.Add(NewContact);*/
                        //tb.Text = SearchIDTB.Text;
                        //tb.PreviewMouseLeftButtonDown += TextBlock_PreviewMouseLeftButtonDown;
                        //更新List
                        // Console.WriteLine(ContactsName.FindIndex(s => s == SearchIDTB.Text));
                        Contacts_.Items.Add(NewContact);
                        //将信息存入内存
                        ContactsName.Add(SearchIDTB.Text);
                        ContactsIP.Add(strReceive);
                        chatHistory cHtmp = new chatHistory(myUsername, SearchIDTB.Text);
                        chatHistoryList.Add(cHtmp);
                    }
                    SearchIDTB.Text = null;
                    break;
                case 1:
                    break;
                case 2:
                    //查询好友
                    int ContactID = ContactsName.FindIndex(s => s == SearchIDTB.Text);
                    if (ContactID == -1)
                        MessageBox.Show("Not your friend.");
                    else //查到了就跳转并更新界面
                    {
                        string LastContact = CurrContact;
                        CurrContact = SearchIDTB.Text;
                        ChatId.Text = CurrContact;
                        if (serverConnct.ServerQuery("q" + CurrContact) == "n")
                        {
                            BrushConverter brushConvert = new BrushConverter();
                            StateLight.Background = (Brush)brushConvert.ConvertFromString("#FF838971");
                        }
                        else
                        {
                            BrushConverter brushConvert = new BrushConverter();
                            StateLight.Background = (Brush)brushConvert.ConvertFromString("#FFAEEA00");
                        }
                        if (LastContact != CurrContact && CurrContact == myUsername)
                            ChatList.Items.Clear();

                        if (LastContact != CurrContact && CurrContact != myUsername)
                        {
                            ChatList.Items.Clear();
                            int Contactindex = chatHistoryList.FindIndex(s => s.Contact == CurrContact);
                            //恢复每一个消息
                            for (int i = 0; i < chatHistoryList[Contactindex].History.Count; i++)
                            {
                                if (chatHistoryList[Contactindex].History[i].Speaker == myUsername)
                                {
                                    if (chatHistoryList[Contactindex].History[i].ModeFile == "0")
                                    {
                                        Card myNewMessCard = new Card();
                                        //myNewMessCard.SetResourceReference(BackgroundProperty, "PrimaryHueDarkBrush");
                                        BrushConverter brushConvert = new BrushConverter();
                                        myNewMessCard.Background = (Brush)brushConvert.ConvertFromString("#FF8490C3");
                                        myNewMessCard.FontFamily = new FontFamily("Roboto");
                                        myNewMessCard.MaxWidth = 200;
                                        int padding = 8;
                                        myNewMessCard.Padding = new System.Windows.Thickness((double)padding);
                                        myNewMessCard.UniformCornerRadius = 6;
                                        myNewMessCard.HorizontalAlignment = HorizontalAlignment.Right;
                                        TextBlock myNewMessTB = new TextBlock();
                                        myNewMessTB.TextWrapping = TextWrapping.Wrap;
                                        myNewMessTB.Text = chatHistoryList[Contactindex].History[i].Mess;
                                        myNewMessTB.Foreground = Brushes.White;
                                        myNewMessCard.Content = myNewMessTB;
                                        ListViewItem myNewMessLVI = new ListViewItem();
                                        myNewMessLVI.HorizontalAlignment = HorizontalAlignment.Right;
                                        myNewMessLVI.Content = myNewMessCard;
                                        ChatList.Items.Add(myNewMessLVI);
                                        ChatList.ScrollIntoView(myNewMessLVI);
                                    }
                                    else if (chatHistoryList[Contactindex].History[i].ModeFile == "1")
                                    {
                                        //更新UI
                                        Card myNewMessCardGif = new Card();
                                        BrushConverter brushConvertGif = new BrushConverter();
                                        myNewMessCardGif.Background = (Brush)brushConvertGif.ConvertFromString("#FF8490C3");
                                        //myNewMessCard.SetResourceReference(BackgroundProperty, "SystemColors.GrayTextBrushKey");
                                        myNewMessCardGif.FontFamily = new FontFamily("Roboto");
                                        myNewMessCardGif.MaxWidth = 150;
                                        myNewMessCardGif.MaxHeight = 100;
                                        int paddingGif = 8;
                                        myNewMessCardGif.Padding = new System.Windows.Thickness((double)paddingGif);
                                        myNewMessCardGif.UniformCornerRadius = 6;
                                        myNewMessCardGif.HorizontalAlignment = HorizontalAlignment.Right;
                                        MediaElement MEGif = new MediaElement();
                                        //filePathGif.Remove(0, 1);
                                        MEGif.Source = new Uri(chatHistoryList[Contactindex].History[i].Mess);
                                        Console.WriteLine(MEGif.Source);
                                        MEGif.MediaEnded += MediaElement_MediaEnded;
                                        myNewMessCardGif.Content = MEGif;
                                        ListViewItem myNewMessLVIGif = new ListViewItem();
                                        myNewMessLVIGif.HorizontalAlignment = HorizontalAlignment.Right;
                                        myNewMessLVIGif.Content = myNewMessCardGif;
                                        ChatList.Items.Add(myNewMessLVIGif);
                                        ChatList.ScrollIntoView(myNewMessLVIGif);
                                    }

                                }
                                else if (chatHistoryList[Contactindex].History[i].Speaker == CurrContact)
                                {
                                    if (chatHistoryList[Contactindex].History[i].ModeFile == "0")
                                    {
                                        Card myNewMessCard = new Card();
                                        BrushConverter brushConvert = new BrushConverter();
                                        myNewMessCard.Background = (Brush)brushConvert.ConvertFromString("#FF7C7A7A");
                                        //myNewMessCard.SetResourceReference(BackgroundProperty, "SystemColors.GrayTextBrushKey");
                                        myNewMessCard.FontFamily = new FontFamily("Roboto");
                                        myNewMessCard.MaxWidth = 200;
                                        int padding = 8;
                                        myNewMessCard.Padding = new System.Windows.Thickness((double)padding);
                                        myNewMessCard.UniformCornerRadius = 6;
                                        myNewMessCard.HorizontalAlignment = HorizontalAlignment.Left;
                                        TextBlock myNewMessTB = new TextBlock();
                                        myNewMessTB.TextWrapping = TextWrapping.Wrap;
                                        myNewMessTB.Text = chatHistoryList[Contactindex].History[i].Mess;
                                        myNewMessTB.Foreground = Brushes.White;
                                        myNewMessCard.Content = myNewMessTB;
                                        ListViewItem myNewMessLVI = new ListViewItem();
                                        myNewMessLVI.HorizontalAlignment = HorizontalAlignment.Left;
                                        myNewMessLVI.Content = myNewMessCard;
                                        ChatList.Items.Add(myNewMessLVI);
                                        ChatList.ScrollIntoView(myNewMessLVI);
                                    }
                                    else if (chatHistoryList[Contactindex].History[i].ModeFile == "1")
                                    {
                                        //更新UI
                                        Card myNewMessCardGif = new Card();
                                        BrushConverter brushConvertGif = new BrushConverter();
                                        myNewMessCardGif.Background = (Brush)brushConvertGif.ConvertFromString("#FF7C7A7A");
                                        //myNewMessCard.SetResourceReference(BackgroundProperty, "SystemColors.GrayTextBrushKey");
                                        myNewMessCardGif.FontFamily = new FontFamily("Roboto");
                                        myNewMessCardGif.MaxWidth = 150;
                                        myNewMessCardGif.MaxHeight = 100;
                                        int paddingGif = 8;
                                        myNewMessCardGif.Padding = new System.Windows.Thickness((double)paddingGif);
                                        myNewMessCardGif.UniformCornerRadius = 6;
                                        myNewMessCardGif.HorizontalAlignment = HorizontalAlignment.Left;
                                        MediaElement MEGif = new MediaElement();
                                        //filePathGif.Remove(0, 1);
                                        MEGif.Source = new Uri(chatHistoryList[Contactindex].History[i].Mess);
                                        Console.WriteLine(MEGif.Source);
                                        MEGif.MediaEnded += MediaElement_MediaEnded;
                                        myNewMessCardGif.Content = MEGif;
                                        ListViewItem myNewMessLVIGif = new ListViewItem();
                                        myNewMessLVIGif.HorizontalAlignment = HorizontalAlignment.Left;
                                        myNewMessLVIGif.Content = myNewMessCardGif;
                                        ChatList.Items.Add(myNewMessLVIGif);
                                        ChatList.ScrollIntoView(myNewMessLVIGif);
                                    }

                                }
                            }
                        }
                    }
                    SearchIDTB.Text = null;
                    break;
                case 3:
                    myUsername = MainWindow.LOGINWINDOW.username;
                    if (serverConnct.ServerQuery("logout" + myUsername) == "loo")
                    {
                        StopListening();
                        SavetoDataBase();
                        Close();
                    }
                    break;
            }

        }

        private void addContact_Click(object sender, RoutedEventArgs e)
        {
            DialogMode = 0;
            SearchFriend.Text = null;
            SearchFriend.Text = "Add Contact";
            CheckLabel.Visibility = Visibility.Visible;
            SearchIDTB.Visibility = Visibility.Visible;
            DlgHost.IsOpen = true;
        }

        private void ColorZone_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        
        private bool IsExist(string newID)
        {
            for(int i = 0; i < ContactsName.Count(); i++)
            {
                if (newID == ContactsName[i])
                    return true;
            }
            return false;
        }

        private void TreeviewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem tvi = (TreeViewItem)sender;
            ChatId.Text = (string)tvi.Header;
            string LastContact =CurrContact;
            if(ChatId.Text == myUsername + "(Me)")
                CurrContact = myUsername;
            else
                CurrContact = ChatId.Text;
            //更新聊天状态
            int Currindex = ContactsName.FindIndex(s => s == CurrContact);
            string recivestr = serverConnct.ServerQuery("q" + ContactsName[Currindex]);
            if (recivestr == "n")
            {
                BrushConverter brushConvert = new BrushConverter();
                StateLight.Background = (Brush)brushConvert.ConvertFromString("#FF838971");
                ContactsIP[Currindex] = "n";
            }
            else
            {
               BrushConverter brushConvert = new BrushConverter();
                StateLight.Background = (Brush)brushConvert.ConvertFromString("#FFAEEA00");
                ContactsIP[Currindex] = recivestr;
            }


            //自己对自己的聊天记录不保存
            if (LastContact != CurrContact && CurrContact == myUsername)
                ChatList.Items.Clear();

            if(LastContact != CurrContact && CurrContact != myUsername)
            {
                ChatList.Items.Clear();
                int Contactindex = chatHistoryList.FindIndex(s => s.Contact == CurrContact);
                //恢复每一个消息
                for (int i = 0; i < chatHistoryList[Contactindex].History.Count; i++) 
                {
                    if(chatHistoryList[Contactindex].History[i].Speaker == myUsername)
                    {
                        if(chatHistoryList[Contactindex].History[i].ModeFile == "0")
                        {
                            Card myNewMessCard = new Card();
                            //myNewMessCard.SetResourceReference(BackgroundProperty, "PrimaryHueDarkBrush");
                            BrushConverter brushConvert = new BrushConverter();
                            myNewMessCard.Background = (Brush)brushConvert.ConvertFromString("#FF8490C3");
                            myNewMessCard.FontFamily = new FontFamily("Roboto");
                            myNewMessCard.MaxWidth = 200;
                            int padding = 8;
                            myNewMessCard.Padding = new System.Windows.Thickness((double)padding);
                            myNewMessCard.UniformCornerRadius = 6;
                            myNewMessCard.HorizontalAlignment = HorizontalAlignment.Right;
                            TextBlock myNewMessTB = new TextBlock();
                            myNewMessTB.TextWrapping = TextWrapping.Wrap;
                            myNewMessTB.Text = chatHistoryList[Contactindex].History[i].Mess;
                            myNewMessTB.Foreground = Brushes.White;
                            myNewMessCard.Content = myNewMessTB;
                            ListViewItem myNewMessLVI = new ListViewItem();
                            myNewMessLVI.HorizontalAlignment = HorizontalAlignment.Right;
                            myNewMessLVI.Content = myNewMessCard;
                            ChatList.Items.Add(myNewMessLVI);
                            ChatList.ScrollIntoView(myNewMessLVI);
                        }
                        else if(chatHistoryList[Contactindex].History[i].ModeFile == "1")
                        {
                            //更新UI
                            Card myNewMessCardGif = new Card();
                            BrushConverter brushConvertGif = new BrushConverter();
                            myNewMessCardGif.Background = (Brush)brushConvertGif.ConvertFromString("#FF8490C3");
                            //myNewMessCard.SetResourceReference(BackgroundProperty, "SystemColors.GrayTextBrushKey");
                            myNewMessCardGif.FontFamily = new FontFamily("Roboto");
                            myNewMessCardGif.MaxWidth = 150;
                            myNewMessCardGif.MaxHeight = 100;
                            int paddingGif = 8;
                            myNewMessCardGif.Padding = new System.Windows.Thickness((double)paddingGif);
                            myNewMessCardGif.UniformCornerRadius = 6;
                            myNewMessCardGif.HorizontalAlignment = HorizontalAlignment.Right;
                            MediaElement MEGif = new MediaElement();
                            //filePathGif.Remove(0, 1);
                            MEGif.Source = new Uri(chatHistoryList[Contactindex].History[i].Mess);
                            Console.WriteLine(MEGif.Source);
                            MEGif.MediaEnded += MediaElement_MediaEnded;
                            myNewMessCardGif.Content = MEGif;
                            ListViewItem myNewMessLVIGif = new ListViewItem();
                            myNewMessLVIGif.HorizontalAlignment = HorizontalAlignment.Right;
                            myNewMessLVIGif.Content = myNewMessCardGif;
                            ChatList.Items.Add(myNewMessLVIGif);
                            ChatList.ScrollIntoView(myNewMessLVIGif);
                        }

                    }
                    else if(chatHistoryList[Contactindex].History[i].Speaker == CurrContact)
                    {
                        if(chatHistoryList[Contactindex].History[i].ModeFile == "0")
                        {
                            Card myNewMessCard = new Card();
                            BrushConverter brushConvert = new BrushConverter();
                            myNewMessCard.Background = (Brush)brushConvert.ConvertFromString("#FF7C7A7A");
                            //myNewMessCard.SetResourceReference(BackgroundProperty, "SystemColors.GrayTextBrushKey");
                            myNewMessCard.FontFamily = new FontFamily("Roboto");
                            myNewMessCard.MaxWidth = 200;
                            int padding = 8;
                            myNewMessCard.Padding = new System.Windows.Thickness((double)padding);
                            myNewMessCard.UniformCornerRadius = 6;
                            myNewMessCard.HorizontalAlignment = HorizontalAlignment.Left;
                            TextBlock myNewMessTB = new TextBlock();
                            myNewMessTB.TextWrapping = TextWrapping.Wrap;
                            myNewMessTB.Text = chatHistoryList[Contactindex].History[i].Mess;
                            myNewMessTB.Foreground = Brushes.White;
                            myNewMessCard.Content = myNewMessTB;
                            ListViewItem myNewMessLVI = new ListViewItem();
                            myNewMessLVI.HorizontalAlignment = HorizontalAlignment.Left;
                            myNewMessLVI.Content = myNewMessCard;
                            ChatList.Items.Add(myNewMessLVI);
                            ChatList.ScrollIntoView(myNewMessLVI);
                        }
                        else if(chatHistoryList[Contactindex].History[i].ModeFile == "1")
                        {
                            //更新UI
                            Card myNewMessCardGif = new Card();
                            BrushConverter brushConvertGif = new BrushConverter();
                            myNewMessCardGif.Background = (Brush)brushConvertGif.ConvertFromString("#FF7C7A7A");
                            //myNewMessCard.SetResourceReference(BackgroundProperty, "SystemColors.GrayTextBrushKey");
                            myNewMessCardGif.FontFamily = new FontFamily("Roboto");
                            myNewMessCardGif.MaxWidth = 150;
                            myNewMessCardGif.MaxHeight = 100;
                            int paddingGif = 8;
                            myNewMessCardGif.Padding = new System.Windows.Thickness((double)paddingGif);
                            myNewMessCardGif.UniformCornerRadius = 6;
                            myNewMessCardGif.HorizontalAlignment = HorizontalAlignment.Left;
                            MediaElement MEGif = new MediaElement();
                            //filePathGif.Remove(0, 1);
                            MEGif.Source = new Uri(chatHistoryList[Contactindex].History[i].Mess);
                            Console.WriteLine(MEGif.Source);
                            MEGif.MediaEnded += MediaElement_MediaEnded;
                            myNewMessCardGif.Content = MEGif;
                            ListViewItem myNewMessLVIGif = new ListViewItem();
                            myNewMessLVIGif.HorizontalAlignment = HorizontalAlignment.Left;
                            myNewMessLVIGif.Content = myNewMessCardGif;
                            ChatList.Items.Add(myNewMessLVIGif);
                            ChatList.ScrollIntoView(myNewMessLVIGif);
                        }

                    }
                }
            }

        }

        //发送消息
        private void sendBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SendBox.Text == "")
                return;
            

            //发送过去
            int IPindex = ContactsName.FindIndex(s => s == CurrContact);
            if(ContactsIP[IPindex] != "n")
            {
                Card myNewMessCard = new Card();
                //myNewMessCard.SetResourceReference(BackgroundProperty, "PrimaryHueDarkBrush");
                BrushConverter brushConvert = new BrushConverter();
                myNewMessCard.Background = (Brush)brushConvert.ConvertFromString("#FF8490C3");
                myNewMessCard.FontFamily = new FontFamily("Roboto");
                myNewMessCard.MaxWidth = 200;
                int padding = 8;
                myNewMessCard.Padding = new System.Windows.Thickness((double)padding);
                myNewMessCard.UniformCornerRadius = 6;
                myNewMessCard.HorizontalAlignment = HorizontalAlignment.Right;
                TextBlock myNewMessTB = new TextBlock();
                myNewMessTB.TextWrapping = TextWrapping.Wrap;
                myNewMessTB.Text = SendBox.Text;
                myNewMessTB.Foreground = Brushes.White;
                myNewMessCard.Content = myNewMessTB;
                ListViewItem myNewMessLVI = new ListViewItem();
                myNewMessLVI.HorizontalAlignment = HorizontalAlignment.Right;
                myNewMessLVI.Content = myNewMessCard;
                ChatList.Items.Add(myNewMessLVI);
                ChatList.ScrollIntoView(myNewMessLVI);
                IPAddress DstIP = IPAddress.Parse(ContactsIP[IPindex]);
                P2PModule.P2PSendMess(DstIP, Convert2Byte(0, SendBox.Text));
                int Contactindex = chatHistoryList.FindIndex(s => s.Contact == CurrContact);
                //  if (Contactindex == -1)
                //      Contactindex = chatHistoryList.FindIndex(s => s.userFrom == CurrContact);
                //chatHistory cHtmp = chatHistoryList.Find(s => s.userTo == CurrContact);
                chatHistorySingle cHStmp = new chatHistorySingle(myUsername, chatHistoryList[Contactindex].History.Count(),
                    SendBox.Text, "0", "2019/1/1");
                chatHistoryList[Contactindex].AddHistory(cHStmp);
                //清空输入框
                SendBox.Clear();
            }
            else
            {
                MessageBox.Show("对方不在线，不能发送消息");
            }

        }

        //回车发送
        private void SendBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sendBtn_Click(sender, e);
            }
        }

        /**************************************
        //发送消息
        ****************************************/
        

        //转换为发送的格式
        // lenStdMess 为协议字节数 StdByte为要发的字节
        public byte[] Convert2Byte(int ModeMess,string Mess)
        {
            byte[] MessByte = Encoding.UTF8.GetBytes(Mess);
            byte[] StdByte;
            switch(ModeMess)
            {
                //发消息
                case 0:
                    StdByte = new byte[MessByte.Length + lenStdMess];
                    Encoding.UTF8.GetBytes(myUsername).CopyTo(StdByte, 0);
                    Encoding.UTF8.GetBytes("0000").CopyTo(StdByte, lenStdMess - 4);
                    MessByte.CopyTo(StdByte, lenStdMess);
                    return StdByte;
                //发gif
                case 1:
                    string fileNameGif = Path.GetFileName(Mess);
                    byte[] fileNameByteGif = Encoding.UTF8.GetBytes(fileNameGif);
                    byte[] fileByteGif = File.ReadAllBytes(Mess); //Mess为路径

                    StdByte = new byte[lenStdMess + 1 + fileNameByteGif.Length + fileByteGif.Length];
                    Encoding.UTF8.GetBytes(myUsername).CopyTo(StdByte, 0);
                    Encoding.UTF8.GetBytes("1100").CopyTo(StdByte, lenStdMess - 4);
                    StdByte[lenStdMess] = (byte)fileNameByteGif.Length;
                    fileNameByteGif.CopyTo(StdByte, lenStdMess + 1);
                    fileByteGif.CopyTo(StdByte, lenStdMess + 1 + fileNameByteGif.Length);
                    return StdByte;
                //发文件（参数传过来Path)
                case 2:
                    string fileName = Path.GetFileName(Mess);
                    byte[] fileNameByte = Encoding.UTF8.GetBytes(fileName);
                    byte[] fileByte = File.ReadAllBytes(Mess); //Mess为路径

                    StdByte = new byte[lenStdMess + 1 + fileNameByte.Length +  fileByte.Length];
                    Encoding.UTF8.GetBytes(myUsername).CopyTo(StdByte, 0);
                    Encoding.UTF8.GetBytes("1000").CopyTo(StdByte, lenStdMess - 4);//协议位
                    StdByte[lenStdMess] = (byte)fileNameByte.Length;
                    fileNameByte.CopyTo(StdByte, lenStdMess + 1);
                    fileByte.CopyTo(StdByte, lenStdMess + 1 + fileNameByte.Length);
                    return StdByte;
                case 3:
                    return null;
                default:
                    throw new IndexOutOfRangeException();
            }
            //把发送的消息复制入StdByte


            
        }

        /***********************************************
         * 接收消息（单独一个线程）
         * ********************************************/
        
        // 开始监听
        public void StartListening()
        {
            //开启侦听
            ListeningTread = new Thread(new ThreadStart(P2PModule.P2PListen));
            ListeningTread.Start();
            islistening = true;
            //开启监听新消息
            MonitorNewMessThread = new Thread(MonitorNewMess);
            MonitorNewMessThread.Start();
        }

        // 监控新消息
        public void MonitorNewMess()
        {
            while(islistening)
            {
                if (P2PModule.newMessage)
                    this.Dispatcher.Invoke(new delegateUpdataUI(UpdateUI));
            }
        }

        //更新UI
        public delegate void delegateUpdataUI();

        public void UpdateUI()
        {
            string username = Encoding.UTF8.GetString(P2PModule.receiveByteAll, 0, lenStdMess - 4);
            P2PModule.newMessage = false;
            int MessMode = 0; //0为发消息，1为文件名字，2为文件
            string messR = Encoding.UTF8.GetString(P2PModule.receiveByteAll
                        , lenStdMess, P2PModule.lenReceiveAll - lenStdMess); ; //记录接收到的消息
            string MessKind = Encoding.UTF8.GetString(P2PModule.receiveByteAll, lenStdMess - 4, 4);
            string pathforgif = null;
            switch(MessKind)
            {
                case "0000":    //发消息
                    MessMode = 0;
                    break;      //发文件名字
                case "1100":
                    MessMode = 1;
                    break;
                case "1000":   //发文件
                    MessMode = 2;
                    break;
                default:
                    return;
            }
            SnackbarMess.IsActive = false;
            SnackMess.Content = username;
            SnackbarMess.IsActive = true;
            //只有窗口为当前联系人时，对话记录加载
            if(username == CurrContact)
            {
                switch(MessMode)
                {
                    case 0:
                        Card myNewMessCard = new Card();
                        BrushConverter brushConvert = new BrushConverter();
                        myNewMessCard.Background = (Brush)brushConvert.ConvertFromString("#FF7C7A7A");
                        //myNewMessCard.SetResourceReference(BackgroundProperty, "SystemColors.GrayTextBrushKey");
                        myNewMessCard.FontFamily = new FontFamily("Roboto");
                        myNewMessCard.MaxWidth = 200;
                        int padding = 8;
                        myNewMessCard.Padding = new System.Windows.Thickness((double)padding);
                        myNewMessCard.UniformCornerRadius = 6;
                        myNewMessCard.HorizontalAlignment = HorizontalAlignment.Left;
                        TextBlock myNewMessTB = new TextBlock();
                        myNewMessTB.TextWrapping = TextWrapping.Wrap;
                        myNewMessTB.Text = messR;
                        myNewMessTB.Foreground = Brushes.White;
                        myNewMessCard.Content = myNewMessTB;
                        ListViewItem myNewMessLVI = new ListViewItem();
                        myNewMessLVI.HorizontalAlignment = HorizontalAlignment.Left;
                        myNewMessLVI.Content = myNewMessCard;
                        ChatList.Items.Add(myNewMessLVI);
                        ChatList.ScrollIntoView(myNewMessLVI);
                        break;
                    case 1: //传gif
                        byte fileNameByteLengthGif = P2PModule.receiveByteAll[lenStdMess];
                        byte[] fileNameByteGif = new byte[fileNameByteLengthGif];
                        byte[] fileByteGif = new byte[P2PModule.lenReceiveAll - lenStdMess - 1 - fileNameByteLengthGif];
                        Array.Copy(P2PModule.receiveByteAll, lenStdMess + 1, fileNameByteGif, 0, fileNameByteLengthGif);
                        Array.Copy(P2PModule.receiveByteAll, lenStdMess + 1 + fileNameByteLengthGif,
                            fileByteGif, 0, fileByteGif.Length);
                        string Workpath = System.IO.Directory.GetCurrentDirectory();
                        string filePathGif = Workpath +"\\Image" + @"\" + Encoding.UTF8.GetString(fileNameByteGif);
                        Console.WriteLine(filePathGif);
                        pathforgif = filePathGif;
                        File.WriteAllBytes(filePathGif, fileByteGif);
                        //更新UI
                        Card myNewMessCardGif = new Card();
                        BrushConverter brushConvertGif = new BrushConverter();
                        myNewMessCardGif.Background = (Brush)brushConvertGif.ConvertFromString("#FF7C7A7A");
                        //myNewMessCard.SetResourceReference(BackgroundProperty, "SystemColors.GrayTextBrushKey");
                        myNewMessCardGif.FontFamily = new FontFamily("Roboto");
                        myNewMessCardGif.MaxWidth = 150;
                        myNewMessCardGif.MaxHeight = 100;
                        int paddingGif = 8;
                        myNewMessCardGif.Padding = new System.Windows.Thickness((double)paddingGif);
                        myNewMessCardGif.UniformCornerRadius = 6;
                        myNewMessCardGif.HorizontalAlignment = HorizontalAlignment.Left;
                        MediaElement MEGif = new MediaElement();
                        //filePathGif.Remove(0, 1);
                        MEGif.Source = new Uri(filePathGif);
                        Console.WriteLine(MEGif.Source);
                        MEGif.MediaEnded += MediaElement_MediaEnded;
                        myNewMessCardGif.Content = MEGif;
                        ListViewItem myNewMessLVIGif = new ListViewItem();
                        myNewMessLVIGif.HorizontalAlignment = HorizontalAlignment.Left;
                        myNewMessLVIGif.Content = myNewMessCardGif;
                        ChatList.Items.Add(myNewMessLVIGif);
                        ChatList.ScrollIntoView(myNewMessLVIGif);

                        break;
                    case 2: //传入文件
                        byte fileNameByteLength = P2PModule.receiveByteAll[lenStdMess];
                        byte[] fileNameByte = new byte[fileNameByteLength];
                        byte[] fileByte = new byte[P2PModule.lenReceiveAll - lenStdMess - 1 - fileNameByteLength];
                        Array.Copy(P2PModule.receiveByteAll, lenStdMess + 1, fileNameByte, 0, fileNameByteLength);
                        Array.Copy(P2PModule.receiveByteAll, lenStdMess + 1 + fileNameByteLength,
                            fileByte, 0, fileByte.Length);
                        var saveFileDialog = new Microsoft.Win32.SaveFileDialog()
                        {
                            Filter = "所有文件(*.*)|*.*"
                        };
                        saveFileDialog.FileName = Encoding.UTF8.GetString(fileNameByte);
                        if (saveFileDialog.ShowDialog() == true)
                        {
                            string filePath = saveFileDialog.FileName.ToString();
                            Console.WriteLine(filePath);

                            File.WriteAllBytes(filePath, fileByte);
                        }

                        break;
                }

            }
            if (MessMode == 0)
            {
                //记录与界面
                int Contactindex = chatHistoryList.FindIndex(s => s.Contact == username);
                if (Contactindex == -1)
                {
                    //新建一个
                    chatHistory CHLtmp = new chatHistory(myUsername, username);
                    chatHistorySingle CHStmp = new chatHistorySingle(username, 1, messR, "0", "2019/1/1");
                    CHLtmp.AddHistory(CHStmp);
                    chatHistoryList.Add(CHLtmp);
                    //菜单栏加一个
                    TextBlock tb = new TextBlock();
                    TreeViewItem NewContact = new TreeViewItem();
                    NewContact.Header = username;
                    NewContact.PreviewMouseLeftButtonDown += TreeviewItem_PreviewMouseLeftButtonDown;

                    /*Button bt = new Button();
                    bt.Height = 10;
                    bt.Width = 10;
                    bt.SetResourceReference(StyleProperty, "MaterialDesignFloatingActionMiniAccentButton");
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    sp.Children.Add(bt);
                    sp.Children.Add(NewContact);*/
                    //更新List
                    // Console.WriteLine(ContactsName.FindIndex(s => s == SearchIDTB.Text));
                    Contacts_.Items.Add(NewContact);
                    //将信息存入内存
                    ContactsName.Add(username);
                    ContactsIP.Add(serverConnct.ServerQuery("q" + username));
                }
                else
                {
                    chatHistorySingle cHStmp = new chatHistorySingle(username, chatHistoryList[Contactindex].History.Count(),
                                 messR, "0", "2019/1/1");
                    chatHistoryList[Contactindex].AddHistory(cHStmp);
                }
            }
            else if (MessMode == 1) //保留gif
            {
                //记录与界面
                int Contactindex = chatHistoryList.FindIndex(s => s.Contact == username);
                if (Contactindex == -1)
                {
                    //新建一个
                    chatHistory CHLtmp = new chatHistory(myUsername, username);
                    chatHistorySingle CHStmp = new chatHistorySingle(username, 1, pathforgif, "1", "2019/1/1");
                    CHLtmp.AddHistory(CHStmp);
                    chatHistoryList.Add(CHLtmp);
                    //菜单栏加一个
                    TextBlock tb = new TextBlock();
                    TreeViewItem NewContact = new TreeViewItem();
                    NewContact.Header = username;
                    NewContact.PreviewMouseLeftButtonDown += TreeviewItem_PreviewMouseLeftButtonDown;

                    /*Button bt = new Button();
                    bt.Height = 10;
                    bt.Width = 10;
                    bt.SetResourceReference(StyleProperty, "MaterialDesignFloatingActionMiniAccentButton");
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    sp.Children.Add(bt);
                    sp.Children.Add(NewContact);*/
                    //更新List
                    // Console.WriteLine(ContactsName.FindIndex(s => s == SearchIDTB.Text));
                    Contacts_.Items.Add(NewContact);
                    //将信息存入内存
                    ContactsName.Add(username);
                    ContactsIP.Add(serverConnct.ServerQuery("q" + username));
                }
                else
                {
                    chatHistorySingle cHStmp = new chatHistorySingle(username, chatHistoryList[Contactindex].History.Count(),
                                 pathforgif, "1", "2019/1/1");
                    chatHistoryList[Contactindex].AddHistory(cHStmp);
                }
            }




        }

        //停止监听（给自己发一个0的字节）
        public void StopListening()
        {
            islistening = false;
            byte[] Bytetmp = new byte[1];
            Bytetmp[0] = 0;
            P2PModule.P2PSendMess(IPAddress.Parse(ContactsIP[0]), Bytetmp);
        }


        //文件发送
        private void FilesendBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "所有文件(*.*)|*.*"
            };

            openFileDialog.RestoreDirectory = true;
            if(openFileDialog.ShowDialog() == true)
            {
                string localFilePath = openFileDialog.FileName.ToString();
                //string fileName = Path.GetFileName(localFilePath);
                int IPindex = ContactsName.FindIndex(s => s == CurrContact);
                if (ContactsIP[IPindex] != "n")
                {
                    IPAddress peerIp = IPAddress.Parse(ContactsIP[IPindex]);
                    Console.WriteLine(localFilePath);
                    P2PModule.P2PSendMess(peerIp, Convert2Byte(2, localFilePath));
                }
                else
                {
                    MessageBox.Show("对方不在线，不能发送消息。");
                }
                    
            }
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1));
        }

        private void StickerBtn_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "动态表情(*.gif*)|*.gif*"
            };

            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                string localFilePath = openFileDialog.FileName.ToString();
                //string fileName = Path.GetFileName(localFilePath);
                int IPindex = ContactsName.FindIndex(s => s == CurrContact);
                if (ContactsIP[IPindex] != "n")
                {
                    IPAddress peerIp = IPAddress.Parse(ContactsIP[IPindex]);
                    Console.WriteLine(localFilePath);
                    P2PModule.P2PSendMess(peerIp, Convert2Byte(1, localFilePath));
                    //更新UI
                    Card myNewMessCardGif = new Card();
                    BrushConverter brushConvertGif = new BrushConverter();
                    myNewMessCardGif.Background = (Brush)brushConvertGif.ConvertFromString("#FF8490C3");
                    //myNewMessCard.SetResourceReference(BackgroundProperty, "SystemColors.GrayTextBrushKey");
                    myNewMessCardGif.FontFamily = new FontFamily("Roboto");
                    myNewMessCardGif.MaxWidth = 150;
                    myNewMessCardGif.MaxHeight = 100;
                    int paddingGif = 8;
                    myNewMessCardGif.Padding = new System.Windows.Thickness((double)paddingGif);
                    myNewMessCardGif.UniformCornerRadius = 6;
                    myNewMessCardGif.HorizontalAlignment = HorizontalAlignment.Right;
                    MediaElement MEGif = new MediaElement();
                    //filePathGif.Remove(0, 1);
                    MEGif.Source = new Uri(localFilePath);
                    Console.WriteLine(MEGif.Source);
                    MEGif.MediaEnded += MediaElement_MediaEnded;
                    myNewMessCardGif.Content = MEGif;
                    ListViewItem myNewMessLVIGif = new ListViewItem();
                    myNewMessLVIGif.HorizontalAlignment = HorizontalAlignment.Right;
                    myNewMessLVIGif.Content = myNewMessCardGif;
                    ChatList.Items.Add(myNewMessLVIGif);
                    ChatList.ScrollIntoView(myNewMessLVIGif);
                    int Contactindex = chatHistoryList.FindIndex(s => s.Contact == CurrContact);
                    chatHistorySingle cHStmp = new chatHistorySingle(myUsername, chatHistoryList[Contactindex].History.Count(),
                      localFilePath, "1", "2019/1/1");
                    chatHistoryList[Contactindex].AddHistory(cHStmp);

                }
                else
                {
                    MessageBox.Show("对方不在线，不能发送消息。");
                }

            }
        }

        private void SnackMess_ActionClick(object sender, RoutedEventArgs e)
        {
            SnackbarMess.IsActive = false;
        }

        private void SearchIDTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!isCorrectFormat(SearchIDTB.Text))
            {
                BrushConverter brushConvert = new BrushConverter();
                CheckLabel.Foreground = (Brush)brushConvert.ConvertFromString("#DDF31212");
                CheckLabel.Content = "Please input the right format.";
                return;
            }
            CheckLabel.Content = null;
            switch(DialogMode)
            {
                case 0:
                    string strReceive = serverConnct.ServerQuery("q" + SearchIDTB.Text);
                    if (strReceive == null || strReceive == "Please send the correct message." || strReceive == "Incorrect No.")
                    {
                        //失败
                        BrushConverter brushConvert = new BrushConverter();
                        CheckLabel.Foreground = (Brush)brushConvert.ConvertFromString("#DDF31212"); 
                        CheckLabel.Content = "Please input the right format.";
                    }
                    else if (strReceive == "n")
                    {
                        BrushConverter brushConvert = new BrushConverter();
                        CheckLabel.Foreground = (Brush)brushConvert.ConvertFromString("#DDF31212");
                        CheckLabel.Content = "Not Online.";
                    }
                    else
                    {
                        BrushConverter brushConvert = new BrushConverter();
                        CheckLabel.Foreground = (Brush)brushConvert.ConvertFromString("#DD48F108");
                        CheckLabel.Content = "Online";
                    }
                    break;
                case 1:
                    break;
                case 2:
                    int ContactID = ContactsName.FindIndex(s => s == SearchIDTB.Text);
                    if (ContactID == -1)
                    {
                        BrushConverter brushConvert = new BrushConverter();
                        CheckLabel.Foreground = (Brush)brushConvert.ConvertFromString("#DDF31212");
                        CheckLabel.Content = "Not your friend.";
                    }
                    break;
                case 3:
                    break;
            }
        }
        private bool isCorrectFormat(string ID)
        {
            if (ID == null)
                return false;
            if (ID.Length != 10)
                return false;
            if (ID[0] != '2' || ID[1] != '0'|| ID[2] != '1')
                return false;
            return true;
        }
    }
}
