   M S B A M L   `   `   `  ���� K  GDeepSpaceNetwork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:DeepSpaceNetworkDeepSpaceNetwork  4    .DeepSpaceNetwork.PayloadCommunicationDashboard   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:DeepSpaceNetwork     -     � Title$#  PayloadCommunicationDashboard��$	��600��$
��1200��.��� 
�������.+�� 
�������.+��� -   $��<��
�������$��10(�$
��Left=�$��	10,10,0,0q�$	��Top=�$��28��$��25���� -    ��Name$% PayloadCommunicationSystemLabel��$�� ��$��36(�$��Center=�$	��Top=�$��Center=�$��0,24,0,0q�$	��Top=�$	��960��$��62���� -    ��Name$ StartTelemetryBtn��
�������$��Start Telemetry��{�.  $��20(�$��Center=�$
��Left=�$��
35,177,0,0q�$	��Top=�$	��164��$��42���� -   $ StopTelemetryBtn��
�������$��Stop Telemetry��{�.  $��20(�$��Center=�$
��Left=�$��204,177,0,0q�$	��Top=�$	��164��$��42���� -   $ DecommisionBtn��
�������$��Decommision��$��20(�$��Center=�$
��Left=�$��432,104,0,0q�$	��Top=�$	��333��$��42���� -   $ StartDataBtn��
�������$��
Start Data��$��20(�{�.  $��Center=�$
��Left=�$��432,177,0,0q�$	��Top=�$	��164��$��42���� -   $ StopDataBtn��
�������$��	Stop Data��$��20(�{�.  $��Center=�$
��Left=�$��605,177,0,0q�$	��Top=�$	��160��$��42���� $��Payload Type:��$��20(�$��Center=�$
��Left=�$��791,108,0,0q�$	��Top=�$	��133��$��39���� -	   $ PayloadType��$�� ��$��20(�$��Center=�$
��Left=�$��929,107,0,0q�$	��Top=�$	��244��$��40���� $��Telemetry Information��$
��Bold"�$��20(�$��Center=�$
��Left=�$��
35,219,0,0q�$	��Top=�$	��328��$��39���� -
    ��Name$ TelemetryBox��
�������{�.  $
��Left=�$��
35,263,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��328��$	��265���� $��Payload communication��$
��Bold"�$��20(�$��Center=�$
��Left=�$��840,220,0,0q�$	��Top=�$	��333��$��38���� -   $ CommunicationBox��
�������{�.  $
��Left=�$��840,263,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��333��$	��265���� $��Payload Data��$
��Bold"�$��20(�$��Center=�$
��Left=�$��432,219,0,0q�$	��Top=�$	��328��$��39���� -   $ DataBox��
�������{�.  $
��Left=�$��432,258,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��333��$	��265��