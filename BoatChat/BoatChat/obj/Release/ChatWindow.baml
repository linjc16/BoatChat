   M S B A M L   `   `   `  ���� C  ?BoatChat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:BoatChatBoatChat      BoatChat.ChatWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     (localclr-namespace:BoatChat     V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    � Title$  
ChatWindow��$	��600��$	��850��$	��792��$	��464�� � 
ResizeMode$ CanMinimize=�   MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=� � WindowStartupLocation$ CenterScreen=�$X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)� � WindowStyle$
 None=�.��)  #MaterialDesignThemes.Wpf.DialogHost  -     Name$ DlgHost��$��Center=�   DialogContent �� -    ��Name$ sp��$��16q�.+��� .+��� -   	 ��Name$	 SearchFriend��Search Contacts�� -   
 ��Name$
 
SearchIDTB��)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 
Student ID�� % !MaterialDesignFloatingHintTextBox#��[ �� -    ��Name$ 
CheckLabel�����	#DDF31212$��12(��� -   $ BtnSp�� �� Orientation$ 
Horizontal=�$��Right=�.+���   MaterialDesignFlatButton#��[  �� IsCancel .    CloseDialogCommand#��Z $��Center=���O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem ��3system&clr-namespace:System;assembly=mscorlib  True��.��ACCEPT�� #��[  . #��Z $��Center=���1&clr-namespace:System;assembly=mscorlibSystem ��3system&clr-namespace:System;assembly=mscorlib  	False��.��CANCEL.��� $��	0,0,0,510q�$	��850��.+�(  "MaterialDesignThemes.Wpf.ColorZone  -      Mode$ 
PrimaryMid=�$��16q����	#FF7A7878
���   �$��Stretch=�$	��Top=�$��81��.��]� .+�'  !MaterialDesignThemes.Wpf.PopupBox  -     Name$ MenuPB��$��Right=�   PlacementMode$ BottomAndAlignRightEdges=�   PopupContent. �� .+��� -	    ��Name$ SettingsBtb��$��Settings���� -
   $ QuitBtn��$
��Quit���� $ 
Horizontal=�$��52��.+��� -   $ 
addContact�� 0 ,MaterialDesignFloatingActionMiniAccentButton#��[ $��0,0,15,7q�$��Center=�$	��Top=����	#FFDAC19D��Z� .��'  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ Contacts=��� -   $ addGroup��#��[ $��0,0,15,7q�$��Center=�$	��Top=����	#FFB1DED0��Z� .��  $ 	UserGroup=��� -   $ findContacts��#��[ $��Center=�$	��Top=����	#FF8686AA$��0,0,15,7q���Z� .��  $ 
UserSearch=�]� -    ]�Name$ Contacts��$
��Left=�$	��520����.  $��0,81,0,-508q�$	��Top=�$	��287��.+�R� -    R�Name$ ContactsTreeView��$	��220��$	��282��
�������$	��520��$	��Top=�$	��Top=�.@�Q� -    Q�Name$ 	Contacts_��$��Contacts��Q� -   $ Group_Chats_��$��Group Chats��.@�Q� $��
2016011498��Q� $��Official Accounts��.@�Q� $��Faker��]� -   $
 Chat��$
��Left=�$	��321����.  $��303,76,0,-307q�$	��Top=�$	��531����. .+�� -    �Name$ ChatGrid��$	��530��$��0,34,0,0q�$	��298��$	��Top=� � ColumnDefinitions �� .+��� -    ��Name$ ChatList��.@��� .��#  MaterialDesignThemes.Wpf.Card  ���	#FF7C7A7A ! PrimaryHueDarkForegroundBrush#���  $	��150��$	��100��$��8q�   UniformCornerRadius$ 6Q�$
��Left=�.��r� -    r� Source$ file://E:/111g.gif?��� $��Right=�.��  ���	#FF8490C3#���  $	��200��$��8q�$ 6Q�$��Right=�.���� $
��Wrap=�
�������(&I am beginning to feel like a rap god.]� -   $ Message��$	��190����.  $��287,411,16,-511q�$	��Top=�$	��547��$��Center=�.+��� -   $
 SendBox�� * &MaterialDesignOutlinedTextFieldTextBox#��[ $��	32,0,0,24q�$��Bottom=�$	��142�� �� TextWrapping$
 Wrap=�" �� VerticalScrollBarVisibility$
 Auto=�$ 	Text here��$	��499��$
��Left=�$
��Left=�]� $
��Left=�$��59����.  $��512,102.5,0,-102.5q�$	��Top=�$	��100���� -   $ 
StickerBtn��$
��Left=�$��33��$��322,401,0,-344q�$	��Top=�#��[ $��34����Z� ��Z� .��  $ StickerEmoji=��� -   $ FilesendBtn��$
��Left=�$��33��$��361,401,0,-344q�$	��Top=�$��34��#��[ ��Z� ��Z� .��  $
 File=��� -   $ VoiceBtn��$
��Left=�#��[ $��33��$��400,401,0,-344q�$	��Top=�$��34����Z� ��Z� .��  $ SettingsVoice=��� -   $ MessBtn��$
��Left=�$��33��$��438,401,0,-344q�$	��Top=�$��34��#��[ ��Z� ��Z� .��  $ MessageTextOutline=��� -   $ sendBtn��$
��Send��$��26��$��0,546,45,-482q�$	��Top=�$��64��$��Right=����	#FF85BBA8��Z� �� -   $
 ChatId��$
��Left=�$��35��$��
478,20,0,0q�$
 Wrap=�$��
2016011498��$	��Top=�$	��165��$��Bottom=�$��Center=�$��16(����	#DDFFFFFF��Z� ��Z� �� -   $ 
StateLight��#��[ $��10��$��10��$��484,35,356,0q�$	��Top=���Z� '	  !MaterialDesignThemes.Wpf.Snackbar	  -     	 Name$  SnackbarMess��! 	  IsActive! .  $��-6,0,568,-511q�$	��282��$��Center=�$��Center=�" 	  Message." .
  (MaterialDesignThemes.Wpf.SnackbarMessage
  -    # 
 Name$# 	SnackMess��$��
2016011498��$ 
  ActionContent$$ New Message��