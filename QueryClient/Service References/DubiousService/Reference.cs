﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.0
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace QueryClient.DubiousService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="QueryParameters", Namespace="http://schemas.datacontract.org/2004/07/AnjiQueryWcf")]
    [System.SerializableAttribute()]
    public partial class QueryParameters : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CodeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IpField;
        
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
        public string Code {
            get {
                return this.CodeField;
            }
            set {
                if ((object.ReferenceEquals(this.CodeField, value) != true)) {
                    this.CodeField = value;
                    this.RaisePropertyChanged("Code");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Ip {
            get {
                return this.IpField;
            }
            set {
                if ((object.ReferenceEquals(this.IpField, value) != true)) {
                    this.IpField = value;
                    this.RaisePropertyChanged("Ip");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="ReturnParameters", Namespace="http://schemas.datacontract.org/2004/07/AnjiQueryWcf")]
    [System.SerializableAttribute()]
    public partial class ReturnParameters : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Code_InfoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ImageUrlField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string Logistics_BatchField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MSGField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int Product_IdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime Query_TimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private QueryClient.DubiousService.CodeType TypeField;
        
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
        public string Code_Info {
            get {
                return this.Code_InfoField;
            }
            set {
                if ((object.ReferenceEquals(this.Code_InfoField, value) != true)) {
                    this.Code_InfoField = value;
                    this.RaisePropertyChanged("Code_Info");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ImageUrl {
            get {
                return this.ImageUrlField;
            }
            set {
                if ((object.ReferenceEquals(this.ImageUrlField, value) != true)) {
                    this.ImageUrlField = value;
                    this.RaisePropertyChanged("ImageUrl");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Logistics_Batch {
            get {
                return this.Logistics_BatchField;
            }
            set {
                if ((object.ReferenceEquals(this.Logistics_BatchField, value) != true)) {
                    this.Logistics_BatchField = value;
                    this.RaisePropertyChanged("Logistics_Batch");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MSG {
            get {
                return this.MSGField;
            }
            set {
                if ((object.ReferenceEquals(this.MSGField, value) != true)) {
                    this.MSGField = value;
                    this.RaisePropertyChanged("MSG");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Product_Id {
            get {
                return this.Product_IdField;
            }
            set {
                if ((this.Product_IdField.Equals(value) != true)) {
                    this.Product_IdField = value;
                    this.RaisePropertyChanged("Product_Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Query_Time {
            get {
                return this.Query_TimeField;
            }
            set {
                if ((this.Query_TimeField.Equals(value) != true)) {
                    this.Query_TimeField = value;
                    this.RaisePropertyChanged("Query_Time");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public QueryClient.DubiousService.CodeType Type {
            get {
                return this.TypeField;
            }
            set {
                if ((this.TypeField.Equals(value) != true)) {
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CodeType", Namespace="http://schemas.datacontract.org/2004/07/AnjiQueryWcf")]
    public enum CodeType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        None = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Normal = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Colourful = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="DubiousService.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/Query", ReplyAction="http://tempuri.org/IService/QueryResponse")]
        string Query(QueryClient.DubiousService.QueryParameters parameters);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/Query", ReplyAction="http://tempuri.org/IService/QueryResponse")]
        System.Threading.Tasks.Task<string> QueryAsync(QueryClient.DubiousService.QueryParameters parameters);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/QueryParameters", ReplyAction="http://tempuri.org/IService/QueryParametersResponse")]
        QueryClient.DubiousService.ReturnParameters QueryParameters(QueryClient.DubiousService.QueryParameters parameters);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/QueryParameters", ReplyAction="http://tempuri.org/IService/QueryParametersResponse")]
        System.Threading.Tasks.Task<QueryClient.DubiousService.ReturnParameters> QueryParametersAsync(QueryClient.DubiousService.QueryParameters parameters);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/AddDubiousValuesLog", ReplyAction="http://tempuri.org/IService/AddDubiousValuesLogResponse")]
        int AddDubiousValuesLog(string code, int dubiousValues);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/AddDubiousValuesLog", ReplyAction="http://tempuri.org/IService/AddDubiousValuesLogResponse")]
        System.Threading.Tasks.Task<int> AddDubiousValuesLogAsync(string code, int dubiousValues);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/QueryDubiousValuesLog", ReplyAction="http://tempuri.org/IService/QueryDubiousValuesLogResponse")]
        int QueryDubiousValuesLog(string code);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/QueryDubiousValuesLog", ReplyAction="http://tempuri.org/IService/QueryDubiousValuesLogResponse")]
        System.Threading.Tasks.Task<int> QueryDubiousValuesLogAsync(string code);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : global::QueryClient.DubiousService.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<global::QueryClient.DubiousService.IService>, global::QueryClient.DubiousService.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Query(QueryClient.DubiousService.QueryParameters parameters) {
            return base.Channel.Query(parameters);
        }
        
        public System.Threading.Tasks.Task<string> QueryAsync(QueryClient.DubiousService.QueryParameters parameters) {
            return base.Channel.QueryAsync(parameters);
        }
        
        public QueryClient.DubiousService.ReturnParameters QueryParameters(QueryClient.DubiousService.QueryParameters parameters) {
            return base.Channel.QueryParameters(parameters);
        }
        
        public System.Threading.Tasks.Task<QueryClient.DubiousService.ReturnParameters> QueryParametersAsync(QueryClient.DubiousService.QueryParameters parameters) {
            return base.Channel.QueryParametersAsync(parameters);
        }
        
        public int AddDubiousValuesLog(string code, int dubiousValues) {
            return base.Channel.AddDubiousValuesLog(code, dubiousValues);
        }
        
        public System.Threading.Tasks.Task<int> AddDubiousValuesLogAsync(string code, int dubiousValues) {
            return base.Channel.AddDubiousValuesLogAsync(code, dubiousValues);
        }
        
        public int QueryDubiousValuesLog(string code) {
            return base.Channel.QueryDubiousValuesLog(code);
        }
        
        public System.Threading.Tasks.Task<int> QueryDubiousValuesLogAsync(string code) {
            return base.Channel.QueryDubiousValuesLogAsync(code);
        }
    }
}
