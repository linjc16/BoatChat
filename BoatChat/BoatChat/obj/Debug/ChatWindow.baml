   M S B A M L   `   `   `  ����C  ?BoatChat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:BoatChatBoatChat  5   	       BoatChat.ChatWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:BoatChat     5   	   V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  
ChatWindow��5
   	   $	��600��6   $	��850��6)   $	��792��65   $	��464��6D    � 
ResizeMode$ CanMinimize=�6T      MaterialDesignBody#���   5   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	    � WindowStartupLocation$ CenterScreen=�6.   $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�5   	    � WindowStyle$
 None=�5   	   .��5      )  #MaterialDesignThemes.Wpf.DialogHost  -   5          Name$ DlgHost��5       $��Center=�61      DialogContent 5   
   �� 5      -   5         ��Name$ sp��5   %   $��16q�6   .+�5      �� .+�5      �� -   5        	 ��Name$	 SearchFriend��5       Search Contacts5      �� 5      -   5        
 ��Name$
 
SearchIDTB��5      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 
Student ID��5       % !MaterialDesignFloatingHintTextBox#��[ 5      6T   �� 5       -   5         ��Name$ 
CheckLabel��5       ���	#DDF31212$��12(�6I   5!      5"      �� 5%      -   5        $ BtnSp��5%       �� Orientation$ 
Horizontal=�6,   $��Right=�6E   .+�5&      ��   MaterialDesignFlatButton#��[ 5'       �� IsCancel .    CloseDialogCommand#��Z 5)      $��Center=�5*      ��5+      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5,   -   ��6   3system&clr-namespace:System;assembly=mscorlib  6-   True��5.      .��5/   3   ACCEPT51      �� 52      #��[ 53       . #��Z 55      $��Center=�56      ��57      1&clr-namespace:System;assembly=mscorlibSystem 58   -   ��6   3system&clr-namespace:System;assembly=mscorlib  6-   	False��5:      .��5;   3   CANCEL5=      5?      5A      .��5D   
   � $��	0,0,0,510q�6   $	��850��6"   .+�5H      (  "MaterialDesignThemes.Wpf.ColorZone  -   5           Mode$ 
PrimaryMid=�5H   '   $��16q�5I   %   ���	#FF7A7878
���   �$��Stretch=�5J   #   $	��Top=�6H   $��81��6g   .��5K      ]� .+�5L      '  !MaterialDesignThemes.Wpf.PopupBox  -   5          Name$ MenuPB��5L   .   $��Right=�5M         PlacementMode$ BottomAndAlignRightEdges=�5N         PopupContent. 5O      �� .+�5P      �� -	   5         ��Name$ SettingsBtb��5P   %   $��Settings��6:   �� 5Q      -
   5        $ QuitBtn��5Q   %   $
��Quit��66   6E   5R      5S      �� 5T      $ 
Horizontal=�6!   $��52��6:   .+�5U      �� -   5        $ 
addContact��5U   !    0 ,MaterialDesignFloatingActionMiniAccentButton#��[ 65   $��0,0,15,7q�5V   !   $��Center=�63   $	��Top=�6U   ���	#FFDAC19D��6�   Z� .��5W      '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ Contacts=�5X      5Y      �� 5Z      -   5        $ addGroup��5Z   !   #��[ 63   $��0,0,15,7q�5[   !   $��Center=�63   $	��Top=�6U   ���	#FFB1DED0��6m   Z� .��5]        $ 	UserGroup=�5^      5_      �� 5`      -   5        $ findContacts��5`   !   #��[ 67   $��Center=�5a   !   $	��Top=�6C   ���	#FF8686AA$��0,0,15,7q�6�   ��6[   Z� .��5b        $ 
UserSearch=�5c   !   5d      5e      5f      5g      ]� 5h      -   5         ]�Name$ Contacts��5h      $
��Left=�6*   $	��520��6E   ��.  $��0,81,0,-508q�6h   $	��Top=�6}   $	��287��6�   .+�5i      R� -   5         R�Name$ ContactsTreeView��5i      $	��220��5j      $	��282��6    
�������$	��520��6C   $	��Top=�6P   $	��Top=�6h   .@�5k      Q� -   5         Q�Name$ 	Contacts_��5k   #   $��Contacts��5l      5m      Q� 5n      -   5        $ Group_Chats_��5n   #   $��Group Chats��5o      .@�5p      Q� $��
2016011498��5q      5r      5w      Q� 5x      $��Official Accounts��5y      .@�5z      Q� $��Faker��5{      5|      5}      5~      ]� 5      -   5        $
 Chat��5      $
��Left=�6&   $	��321��6A   ��.  $��303,76,0,-307q�6d   $	��Top=�6{   $	��531��6�   ��. .+�5�      � -   5         �Name$ ChatGrid��5�      $	��530��6)   $��0,34,0,0q�65   $	��298��6G   $	��Top=�6T    � ColumnDefinitions 5�      �� 5�      .+�5�      �� -   5         ��Name$ ChatList��5�      .@�5�      �� .��5�      #  MaterialDesignThemes.Wpf.Card  ���	#FF7C7A7A ! PrimaryHueDarkForegroundBrush#���  5�   !   $	��150��5�   !   $	��100��60   $��8q�5�   !      UniformCornerRadius$ 6Q�5�   !   $
��Left=�69   .��5�   "   r� -   5         r� Source$ file://E:/111g.gif?�5�   /   5�   #   5�      5�      �� 5�      $��Right=�6'   .��5�        ���	#FF8490C3#���  5�   "   $	��200��5�   "   $��8q�5�   "   $ 6Q�5�   "   $��Right=�6:   .��5�   "   �� $
��Wrap=�5�   !   
�������(&I am beginning to feel like a rap god.5�   #   5�      5�      5�      5�      5�      ]� 5�      -   5        $ Message��5�      $	��190��6)   ��.  $��287,411,16,-511q�6L   $	��Top=�6e   $	��547��6}   $��Center=�6�   .+�5�      �� -   5        $
 SendBox��5�       * &MaterialDesignOutlinedTextFieldTextBox#��[ 5�      $��	32,0,0,24q�5�      $��Bottom=�5�      $	��142��5�       �� TextWrapping$
 Wrap=�5�      " �� VerticalScrollBarVisibility$
 Auto=�5�      $ 	Text here��5�      $	��499��69   $
��Left=�6E   $
��Left=�6`   6�   5�      ]� 5�      $
��Left=�6   $��59��63   ��.  $��512,102.5,0,-102.5q�6U   $	��Top=�6q   $	��100��6�   �� 5�      -   5        $ 
StickerBtn��5�      $
��Left=�6*   $��33��5�      $��322,401,0,-344q�6!   $	��Top=�69   #��[ 6Q   $��34��5�      ��6    Z� ��66   Z� .��5�        $ StickerEmoji=�6*   5�      �� 5�      -   5        $ FilesendBtn��5�      $
��Left=�6+   $��33��5�      $��361,401,0,-344q�6!   $	��Top=�5�      $��34��6-   #��[ 68   ��5�   9   Z� ��6O   Z� .��5�        $
 File=�5�      5�      �� 5�      -   5        $ VoiceBtn��5�      $
��Left=�6(   #��[ 5�      $��33��5�      $��400,401,0,-344q�6!   $	��Top=�69   $��34��6Q   ��6\   Z� ��6r   Z� .��5�        $ SettingsVoice=�5�      5�      �� 5�      -   5        $ MessBtn��5�      $
��Left=�6'   $��33��6B   $��438,401,0,-344q�6N   $	��Top=�5�      $��34��6-   #��[ 6N   ��68   Z� ��5�      Z� .��5�        $ MessageTextOutline=�6*   5�      �� 5�      -   5        $ sendBtn��5�      $
��Send��6&   $��26��65   $��0,546,45,-482q�6A   $	��Top=�6X   $��64��6p   $��Right=�6{   ���	#FF85BBA8��6�   Z� 6�   �� 5�      -   5        $
 ChatId��5�      $
��Left=�6&   $��35��6A   $��
478,20,0,0q�5�      $
 Wrap=�6*   $��
2016011498��6>   $	��Top=�6P   $	��165��6h   $��Bottom=�6�   $��Center=�6�   $��16(�6�   ���	#DDFFFFFF��6t   Z� ��6�   Z� 6�   �� 5�      -   5        $ 
StateLight��5�      #��[ 5�      $��10��5�   %   $��10��61   $��484,35,356,0q�5�      $	��Top=�6'   ��5�   =   Z� 5�   '   '	  !MaterialDesignThemes.Wpf.Snackbar	  5�      -   5          	 Name$  SnackbarMess��5�   &   ! 	  IsActive! .  $��-6,0,568,-511q�5�      $	��282��65   $��Center=�6A   $��Center=�6^   " 	  Message." 5�      .
  (MaterialDesignThemes.Wpf.SnackbarMessage
  -    5        # 
 Name$# 	SnackMess��5�   1   $��
2016011498��5�      $ 
  ActionContent$$ New Message��5�      6)   5�      5�      5�      5�      