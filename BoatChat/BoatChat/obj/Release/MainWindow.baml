   M S B A M L   `   `   `  ���� C  ?BoatChat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:BoatChatBoatChat      BoatChat.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  "localclr-namespace:BoatChat  -     �Name$  LoginWindow��   MaterialDesignBody#���     
LightBrush#���  $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=�$X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)� � Title$ 
MainWindow��$	��400��$	��600�� � 
ResizeMode$ CanMinimize=� � WindowStyle$
 None=� � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�.��� .+�#  MaterialDesignThemes.Wpf.Card  $��32q�$��
16,27,16,0q�.����    MaterialDesignTitleTextBlock#���  $��Center=�	BoatChat ��  ! PrimaryHueDarkForegroundBrush#���  �� -    ��Name$ LoginBtn__Login_��$��Login��$
��Left=�$��38��$��208.5,255,0,0q�$	��166��$	��Top=��� -   	 ��Name$	 IDInput��$
��Left=�$��34��$��208.5,164,0,0q�
 �� TextWrapping$

 Wrap=�)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Username��$	��Top=�$	��166��$
��Left=�$	��Top=� % !MaterialDesignFloatingHintTextBox#��[ .��G� -    G�Name$ PsdInput��$
��Left=�$��37��$��208.5,203,0,0q�$	��Top=�$	��166��$ Password��$
��Left=�$	��Top=� ) %MaterialDesignFloatingHintPasswordBox#��[ '  !MaterialDesignThemes.Wpf.PackIcon  -     Name$ 	CloseIcon���  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ WindowClose=�$��	571,5,0,0q�  -   $ MinIcon��$ Minus=�$��	547,5,0,0q��� -   $ MinBtn��$��Button��$
��Left=�$��20��$��	545,3,0,0q�$	��Top=�$��20�� # MaterialDesignRaisedLightButton#��[  � ToolTip$ Minimize����Z� ��Z� ��Z� �� -   $ CloseBtn��$��Button��$
��Left=�$��20��$��	569,3,0,0q�$	��Top=�$��20��#��[ $
 Quit����Z� ��Z� 