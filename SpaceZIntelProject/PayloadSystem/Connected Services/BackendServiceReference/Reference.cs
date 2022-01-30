﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PayloadSystem.BackendServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Vehicle", Namespace="http://schemas.datacontract.org/2004/07/Backend")]
    [System.SerializableAttribute()]
    public partial class Vehicle : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private double OrbitRadiusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private PayloadSystem.BackendServiceReference.Payload PayloadField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SpacecraftStatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public double OrbitRadius {
            get {
                return this.OrbitRadiusField;
            }
            set {
                if ((this.OrbitRadiusField.Equals(value) != true)) {
                    this.OrbitRadiusField = value;
                    this.RaisePropertyChanged("OrbitRadius");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public PayloadSystem.BackendServiceReference.Payload Payload {
            get {
                return this.PayloadField;
            }
            set {
                if ((object.ReferenceEquals(this.PayloadField, value) != true)) {
                    this.PayloadField = value;
                    this.RaisePropertyChanged("Payload");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SpacecraftStatus {
            get {
                return this.SpacecraftStatusField;
            }
            set {
                if ((object.ReferenceEquals(this.SpacecraftStatusField, value) != true)) {
                    this.SpacecraftStatusField = value;
                    this.RaisePropertyChanged("SpacecraftStatus");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Status {
            get {
                return this.StatusField;
            }
            set {
                if ((object.ReferenceEquals(this.StatusField, value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Payload", Namespace="http://schemas.datacontract.org/2004/07/Backend")]
    [System.SerializableAttribute()]
    public partial class Payload : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PayloadStatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PayloadStatus {
            get {
                return this.PayloadStatusField;
            }
            set {
                if ((object.ReferenceEquals(this.PayloadStatusField, value) != true)) {
                    this.PayloadStatusField = value;
                    this.RaisePropertyChanged("PayloadStatus");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Status {
            get {
                return this.StatusField;
            }
            set {
                if ((object.ReferenceEquals(this.StatusField, value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Type {
            get {
                return this.TypeField;
            }
            set {
                if ((object.ReferenceEquals(this.TypeField, value) != true)) {
                    this.TypeField = value;
                    this.RaisePropertyChanged("Type");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BackendServiceReference.BackendServices")]
    public interface BackendServices {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/AddSpaceCraft", ReplyAction="http://tempuri.org/BackendServices/AddSpaceCraftResponse")]
        string AddSpaceCraft(PayloadSystem.BackendServiceReference.Vehicle vehicle);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/AddSpaceCraft", ReplyAction="http://tempuri.org/BackendServices/AddSpaceCraftResponse")]
        System.Threading.Tasks.Task<string> AddSpaceCraftAsync(PayloadSystem.BackendServiceReference.Vehicle vehicle);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/GetAllSpacecraft", ReplyAction="http://tempuri.org/BackendServices/GetAllSpacecraftResponse")]
        PayloadSystem.BackendServiceReference.Vehicle[] GetAllSpacecraft();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/GetAllSpacecraft", ReplyAction="http://tempuri.org/BackendServices/GetAllSpacecraftResponse")]
        System.Threading.Tasks.Task<PayloadSystem.BackendServiceReference.Vehicle[]> GetAllSpacecraftAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/CheckSpacecraftExists", ReplyAction="http://tempuri.org/BackendServices/CheckSpacecraftExistsResponse")]
        long CheckSpacecraftExists(string vehicleName, string payloadName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/CheckSpacecraftExists", ReplyAction="http://tempuri.org/BackendServices/CheckSpacecraftExistsResponse")]
        System.Threading.Tasks.Task<long> CheckSpacecraftExistsAsync(string vehicleName, string payloadName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/GetAddedSpacecraft", ReplyAction="http://tempuri.org/BackendServices/GetAddedSpacecraftResponse")]
        PayloadSystem.BackendServiceReference.Vehicle[] GetAddedSpacecraft();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/GetAddedSpacecraft", ReplyAction="http://tempuri.org/BackendServices/GetAddedSpacecraftResponse")]
        System.Threading.Tasks.Task<PayloadSystem.BackendServiceReference.Vehicle[]> GetAddedSpacecraftAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/UpdateSpacecraft", ReplyAction="http://tempuri.org/BackendServices/UpdateSpacecraftResponse")]
        void UpdateSpacecraft(string vehicleName, string column, string status);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/UpdateSpacecraft", ReplyAction="http://tempuri.org/BackendServices/UpdateSpacecraftResponse")]
        System.Threading.Tasks.Task UpdateSpacecraftAsync(string vehicleName, string column, string status);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/GetAllOnlineSpacecraft", ReplyAction="http://tempuri.org/BackendServices/GetAllOnlineSpacecraftResponse")]
        PayloadSystem.BackendServiceReference.Vehicle[] GetAllOnlineSpacecraft();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/GetAllOnlineSpacecraft", ReplyAction="http://tempuri.org/BackendServices/GetAllOnlineSpacecraftResponse")]
        System.Threading.Tasks.Task<PayloadSystem.BackendServiceReference.Vehicle[]> GetAllOnlineSpacecraftAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/GetSpacecraft", ReplyAction="http://tempuri.org/BackendServices/GetSpacecraftResponse")]
        PayloadSystem.BackendServiceReference.Vehicle GetSpacecraft(string vehicleName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/GetSpacecraft", ReplyAction="http://tempuri.org/BackendServices/GetSpacecraftResponse")]
        System.Threading.Tasks.Task<PayloadSystem.BackendServiceReference.Vehicle> GetSpacecraftAsync(string vehicleName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/GetAllOnlinePayload", ReplyAction="http://tempuri.org/BackendServices/GetAllOnlinePayloadResponse")]
        PayloadSystem.BackendServiceReference.Vehicle[] GetAllOnlinePayload();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/BackendServices/GetAllOnlinePayload", ReplyAction="http://tempuri.org/BackendServices/GetAllOnlinePayloadResponse")]
        System.Threading.Tasks.Task<PayloadSystem.BackendServiceReference.Vehicle[]> GetAllOnlinePayloadAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface BackendServicesChannel : PayloadSystem.BackendServiceReference.BackendServices, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BackendServicesClient : System.ServiceModel.ClientBase<PayloadSystem.BackendServiceReference.BackendServices>, PayloadSystem.BackendServiceReference.BackendServices {
        
        public BackendServicesClient() {
        }
        
        public BackendServicesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BackendServicesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BackendServicesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BackendServicesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string AddSpaceCraft(PayloadSystem.BackendServiceReference.Vehicle vehicle) {
            return base.Channel.AddSpaceCraft(vehicle);
        }
        
        public System.Threading.Tasks.Task<string> AddSpaceCraftAsync(PayloadSystem.BackendServiceReference.Vehicle vehicle) {
            return base.Channel.AddSpaceCraftAsync(vehicle);
        }
        
        public PayloadSystem.BackendServiceReference.Vehicle[] GetAllSpacecraft() {
            return base.Channel.GetAllSpacecraft();
        }
        
        public System.Threading.Tasks.Task<PayloadSystem.BackendServiceReference.Vehicle[]> GetAllSpacecraftAsync() {
            return base.Channel.GetAllSpacecraftAsync();
        }
        
        public long CheckSpacecraftExists(string vehicleName, string payloadName) {
            return base.Channel.CheckSpacecraftExists(vehicleName, payloadName);
        }
        
        public System.Threading.Tasks.Task<long> CheckSpacecraftExistsAsync(string vehicleName, string payloadName) {
            return base.Channel.CheckSpacecraftExistsAsync(vehicleName, payloadName);
        }
        
        public PayloadSystem.BackendServiceReference.Vehicle[] GetAddedSpacecraft() {
            return base.Channel.GetAddedSpacecraft();
        }
        
        public System.Threading.Tasks.Task<PayloadSystem.BackendServiceReference.Vehicle[]> GetAddedSpacecraftAsync() {
            return base.Channel.GetAddedSpacecraftAsync();
        }
        
        public void UpdateSpacecraft(string vehicleName, string column, string status) {
            base.Channel.UpdateSpacecraft(vehicleName, column, status);
        }
        
        public System.Threading.Tasks.Task UpdateSpacecraftAsync(string vehicleName, string column, string status) {
            return base.Channel.UpdateSpacecraftAsync(vehicleName, column, status);
        }
        
        public PayloadSystem.BackendServiceReference.Vehicle[] GetAllOnlineSpacecraft() {
            return base.Channel.GetAllOnlineSpacecraft();
        }
        
        public System.Threading.Tasks.Task<PayloadSystem.BackendServiceReference.Vehicle[]> GetAllOnlineSpacecraftAsync() {
            return base.Channel.GetAllOnlineSpacecraftAsync();
        }
        
        public PayloadSystem.BackendServiceReference.Vehicle GetSpacecraft(string vehicleName) {
            return base.Channel.GetSpacecraft(vehicleName);
        }
        
        public System.Threading.Tasks.Task<PayloadSystem.BackendServiceReference.Vehicle> GetSpacecraftAsync(string vehicleName) {
            return base.Channel.GetSpacecraftAsync(vehicleName);
        }
        
        public PayloadSystem.BackendServiceReference.Vehicle[] GetAllOnlinePayload() {
            return base.Channel.GetAllOnlinePayload();
        }
        
        public System.Threading.Tasks.Task<PayloadSystem.BackendServiceReference.Vehicle[]> GetAllOnlinePayloadAsync() {
            return base.Channel.GetAllOnlinePayloadAsync();
        }
    }
}