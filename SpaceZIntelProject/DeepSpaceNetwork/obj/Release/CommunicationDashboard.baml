   M S B A M L   `   `   `  ���� K  GDeepSpaceNetwork, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:DeepSpaceNetworkDeepSpaceNetwork  -    'DeepSpaceNetwork.CommunicationDashboard   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:DeepSpaceNetwork     -     � Title$  CommunicationDashboard��$	��600��$	��800��.��� 
�������.+��� -   $��<��
�������$��10(�$
��Left=�$��	10,10,0,0q�$	��Top=�$��28��$��25���� -    ��Name$ SpaceCraftName��$�� ��$��36(�$��Center=�$	��Top=�$��Center=�$��0,24,0,0q�$	��Top=�$	��666��$��62���� -    ��Name$ StartTelemetry��
�������$��Start Telemetry��$��20(�$��Center=�$
��Left=�$��
35,161,0,0q�$	��Top=�$	��164��$��42���� -   $ StopTelemetry��
�������$��Stop Telemetry��$��20(�{�.  $��Center=�$
��Left=�$��204,161,0,0q�$	��Top=�$	��196��$��42���� -   $ DeorbitSpacecraft��
�������$��Deorbit��$��20(�$��Center=�$
��Left=�$��
35,101,0,0q�$	��Top=�$	��164��$��42���� -   $ CustomPayloadBtn��
�������$��Launch Payload��$��20(�$��Center=�$
��Left=�$��210,101,0,0q�$	��Top=�$	��190��$��42���� $��Payload Name:��$��20(�$��Center=�$
��Left=�$��432,130,0,0q�$	��Top=�$	��149��$��39���� $��Payload Type:��$��20(�$��Center=�$
��Left=�$��432,176,0,0q�$	��Top=�$	��149��$��39���� -   $ PayloadName��$�� ��$��20(�$��Center=�$
��Left=�$��598,130,0,0q�$	��Top=�$	��192��$��40�� J� RenderTransformOrigin$ 0.497,0.857&��� -	   $ PayloadType��$�� ��$��20(�$��Center=�$
��Left=�$��598,179,0,0q�$	��Top=�$	��192��$��40���� -
    ��Name$ TelemetryBox��
�������{�.  $
��Left=�$��
35,263,0,0q� �� TextWrapping$
 Wrap=�$�� ��$	��Top=�$	��328��$	��265���� -   $ CommunicationBox��
�������{�.  $
��Left=�$��432,263,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��333��$	��265���� $��Telemetry Information��$��20(�$
��Bold"�$��Center=�$
��Left=�$��
35,224,0,0q�$	��Top=�$	��328��$��39���� $��Spacecraft communication��$��20(�$
��Bold"�$��Center=�$
��Left=�$��432,224,0,0q�$	��Top=�$	��333��$��39���� -   $ PayloadWarning��$�� ��$��14(�$��Center=�$
��Left=�$��432,101,0,0q�$	��Top=�$	��333��$��29��