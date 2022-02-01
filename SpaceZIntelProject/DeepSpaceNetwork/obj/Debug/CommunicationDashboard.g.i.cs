﻿#pragma checksum "..\..\CommunicationDashboard.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "7DBB4E6C526BDC1190A358384D8FEE6F40B705E6A14B201DC907140544546E38"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DeepSpaceNetwork;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace DeepSpaceNetwork {
    
    
    /// <summary>
    /// CommunicationDashboard
    /// </summary>
    public partial class CommunicationDashboard : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\CommunicationDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label spaceCraftName;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\CommunicationDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button startTelemetry;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\CommunicationDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button stopTelemetry;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\CommunicationDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button deorbitSpacecraft;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\CommunicationDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button customPayloadBtn;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\CommunicationDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label payloadName;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\CommunicationDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label payloadType;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\CommunicationDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TelemetryBox;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\CommunicationDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox CommunicationBox;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\CommunicationDashboard.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label payloadWarning;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/DeepSpaceNetwork;component/communicationdashboard.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CommunicationDashboard.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 10 "..\..\CommunicationDashboard.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Go_Back);
            
            #line default
            #line hidden
            return;
            case 2:
            this.spaceCraftName = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.startTelemetry = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\CommunicationDashboard.xaml"
            this.startTelemetry.Click += new System.Windows.RoutedEventHandler(this.Start_Telemetry_Function);
            
            #line default
            #line hidden
            return;
            case 4:
            this.stopTelemetry = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\CommunicationDashboard.xaml"
            this.stopTelemetry.Click += new System.Windows.RoutedEventHandler(this.Stop_Telemetry_Function);
            
            #line default
            #line hidden
            return;
            case 5:
            this.deorbitSpacecraft = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\CommunicationDashboard.xaml"
            this.deorbitSpacecraft.Click += new System.Windows.RoutedEventHandler(this.DeOrbit_Spacecraft);
            
            #line default
            #line hidden
            return;
            case 6:
            this.customPayloadBtn = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\CommunicationDashboard.xaml"
            this.customPayloadBtn.Click += new System.Windows.RoutedEventHandler(this.Custom_Payload_Function);
            
            #line default
            #line hidden
            return;
            case 7:
            this.payloadName = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.payloadType = ((System.Windows.Controls.Label)(target));
            return;
            case 9:
            this.TelemetryBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 10:
            this.CommunicationBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.payloadWarning = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

