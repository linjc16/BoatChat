   M S B A M L   `   `   `  ����C  ?BoatChat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:BoatChatBoatChat  5   	       BoatChat.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   "localclr-namespace:BoatChat  5   	   -   5          �Name$  LoginWindow��5   R      MaterialDesignBody#���   5	   	     
LightBrush#���  5
   	   $��Regular"�5   	   $��13(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	   $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�5   	    � Title$ 
MainWindow��5   	   $	��400��6   $	��600��6)    � 
ResizeMode$ CanMinimize=�65    � WindowStyle$
 None=�6N    � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�6�   .��5      � .+�5   
   #  MaterialDesignThemes.Wpf.Card  $��32q�6   $��
16,27,16,0q�6+   .��5      ��    MaterialDesignTitleTextBlock#���  6   $��Center=�6O   	BoatChat �� 5       ! PrimaryHueDarkForegroundBrush#���  6$   5      5      �� 5   
   -   5         ��Name$ LoginBtn__Login_��5      $��Login��6+   $
��Left=�6;   $��38��6V   $��208.5,255,0,0q�6b   $	��166��6y   $	��Top=�6�   6�   �� 5   
   -   5        	 ��Name$	 IDInput��5      $
��Left=�6#   $��34��5      $��208.5,164,0,0q�6   
 �� TextWrapping$

 Wrap=�65   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Username��5      $	��Top=�6<   $	��166��5      $
��Left=�6   $	��Top=�5       % !MaterialDesignFloatingHintTextBox#��[ 5       .��5!      5#      G� 5$   
   -   5         G�Name$ PsdInput��5$      $
��Left=�6(   $��37��5%      $��208.5,203,0,0q�6"   $	��Top=�69   $	��166��6Q   $ Password��5&      $
��Left=�6@   $	��Top=�6b    ) %MaterialDesignFloatingHintPasswordBox#��[ 5'      '  !MaterialDesignThemes.Wpf.PackIcon  5)   
   -   5          Name$ 	CloseIcon��5)   "   �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ WindowClose=�65   $��	571,5,0,0q�6I     5*   
   -   5        $ MinIcon��5*   "   $ Minus=�63   $��	547,5,0,0q�6@   �� 5+   
   -   5        $ MinBtn��5+      $��Button��6!   $
��Left=�5,      $��20��6,   $��	545,3,0,0q�68   $	��Top=�6K   $��20��6c    # MaterialDesignRaisedLightButton#��[ 5/       � ToolTip$ Minimize��50      ��5-      Z� ��6'   Z� ��6=   Z� 50      �� 52   
   -   5        $ CloseBtn��52      $��Button��6#   $
��Left=�64   $��20��6O   $��	569,3,0,0q�6[   $	��Top=�53      $��20��6)   #��[ 55      $
 Quit��56      ��53   4   Z� ��54      Z� 56      58      59      