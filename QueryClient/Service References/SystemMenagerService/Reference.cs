﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18052
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace QueryClient.SystemMenagerService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LoginUser", Namespace="http://schemas.datacontract.org/2004/07/AnjiSmart.Query.LogService")]
    [System.SerializableAttribute()]
    public partial class LoginUser : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AuthoryField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RealNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RoleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int idField;
        
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
        public string Authory {
            get {
                return this.AuthoryField;
            }
            set {
                if ((object.ReferenceEquals(this.AuthoryField, value) != true)) {
                    this.AuthoryField = value;
                    this.RaisePropertyChanged("Authory");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string RealName {
            get {
                return this.RealNameField;
            }
            set {
                if ((object.ReferenceEquals(this.RealNameField, value) != true)) {
                    this.RealNameField = value;
                    this.RaisePropertyChanged("RealName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Role {
            get {
                return this.RoleField;
            }
            set {
                if ((object.ReferenceEquals(this.RoleField, value) != true)) {
                    this.RoleField = value;
                    this.RaisePropertyChanged("Role");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserName {
            get {
                return this.UserNameField;
            }
            set {
                if ((object.ReferenceEquals(this.UserNameField, value) != true)) {
                    this.UserNameField = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int id {
            get {
                return this.idField;
            }
            set {
                if ((this.idField.Equals(value) != true)) {
                    this.idField = value;
                    this.RaisePropertyChanged("id");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SystemMenagerService.ISystemManagerService")]
    public interface ISystemManagerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/Login", ReplyAction="http://tempuri.org/ISystemManagerService/LoginResponse")]
        QueryClient.SystemMenagerService.LoginUser Login(string userName, string pwd, string feature);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/Login", ReplyAction="http://tempuri.org/ISystemManagerService/LoginResponse")]
        System.Threading.Tasks.Task<QueryClient.SystemMenagerService.LoginUser> LoginAsync(string userName, string pwd, string feature);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/AddUser", ReplyAction="http://tempuri.org/ISystemManagerService/AddUserResponse")]
        int AddUser(string name, string pwd, string roleName, string realName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/AddUser", ReplyAction="http://tempuri.org/ISystemManagerService/AddUserResponse")]
        System.Threading.Tasks.Task<int> AddUserAsync(string name, string pwd, string roleName, string realName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/CheckUserNameExist", ReplyAction="http://tempuri.org/ISystemManagerService/CheckUserNameExistResponse")]
        bool CheckUserNameExist(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/CheckUserNameExist", ReplyAction="http://tempuri.org/ISystemManagerService/CheckUserNameExistResponse")]
        System.Threading.Tasks.Task<bool> CheckUserNameExistAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/AddRole", ReplyAction="http://tempuri.org/ISystemManagerService/AddRoleResponse")]
        int AddRole(string name, string authory);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/AddRole", ReplyAction="http://tempuri.org/ISystemManagerService/AddRoleResponse")]
        System.Threading.Tasks.Task<int> AddRoleAsync(string name, string authory);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/ChangeUserRoles", ReplyAction="http://tempuri.org/ISystemManagerService/ChangeUserRolesResponse")]
        int ChangeUserRoles(string name, string roles);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/ChangeUserRoles", ReplyAction="http://tempuri.org/ISystemManagerService/ChangeUserRolesResponse")]
        System.Threading.Tasks.Task<int> ChangeUserRolesAsync(string name, string roles);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/GetUsetRoles", ReplyAction="http://tempuri.org/ISystemManagerService/GetUsetRolesResponse")]
        string[] GetUsetRoles(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/GetUsetRoles", ReplyAction="http://tempuri.org/ISystemManagerService/GetUsetRolesResponse")]
        System.Threading.Tasks.Task<string[]> GetUsetRolesAsync(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/GetUsetRolesByName", ReplyAction="http://tempuri.org/ISystemManagerService/GetUsetRolesByNameResponse")]
        string[] GetUsetRolesByName(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/GetUsetRolesByName", ReplyAction="http://tempuri.org/ISystemManagerService/GetUsetRolesByNameResponse")]
        System.Threading.Tasks.Task<string[]> GetUsetRolesByNameAsync(string userName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/GetUsersOfRole", ReplyAction="http://tempuri.org/ISystemManagerService/GetUsersOfRoleResponse")]
        System.Collections.Generic.Dictionary<int, string> GetUsersOfRole(string roleName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/GetUsersOfRole", ReplyAction="http://tempuri.org/ISystemManagerService/GetUsersOfRoleResponse")]
        System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int, string>> GetUsersOfRoleAsync(string roleName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/ChangeRoleAuthory", ReplyAction="http://tempuri.org/ISystemManagerService/ChangeRoleAuthoryResponse")]
        int ChangeRoleAuthory(string name, string authory);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/ChangeRoleAuthory", ReplyAction="http://tempuri.org/ISystemManagerService/ChangeRoleAuthoryResponse")]
        System.Threading.Tasks.Task<int> ChangeRoleAuthoryAsync(string name, string authory);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/GetRolse", ReplyAction="http://tempuri.org/ISystemManagerService/GetRolseResponse")]
        string[] GetRolse();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISystemManagerService/GetRolse", ReplyAction="http://tempuri.org/ISystemManagerService/GetRolseResponse")]
        System.Threading.Tasks.Task<string[]> GetRolseAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISystemManagerServiceChannel : QueryClient.SystemMenagerService.ISystemManagerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SystemManagerServiceClient : System.ServiceModel.ClientBase<QueryClient.SystemMenagerService.ISystemManagerService>, QueryClient.SystemMenagerService.ISystemManagerService {
        
        public SystemManagerServiceClient() {
        }
        
        public SystemManagerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SystemManagerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SystemManagerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SystemManagerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public QueryClient.SystemMenagerService.LoginUser Login(string userName, string pwd, string feature) {
            return base.Channel.Login(userName, pwd, feature);
        }
        
        public System.Threading.Tasks.Task<QueryClient.SystemMenagerService.LoginUser> LoginAsync(string userName, string pwd, string feature) {
            return base.Channel.LoginAsync(userName, pwd, feature);
        }
        
        public int AddUser(string name, string pwd, string roleName, string realName) {
            return base.Channel.AddUser(name, pwd, roleName, realName);
        }
        
        public System.Threading.Tasks.Task<int> AddUserAsync(string name, string pwd, string roleName, string realName) {
            return base.Channel.AddUserAsync(name, pwd, roleName, realName);
        }
        
        public bool CheckUserNameExist(string name) {
            return base.Channel.CheckUserNameExist(name);
        }
        
        public System.Threading.Tasks.Task<bool> CheckUserNameExistAsync(string name) {
            return base.Channel.CheckUserNameExistAsync(name);
        }
        
        public int AddRole(string name, string authory) {
            return base.Channel.AddRole(name, authory);
        }
        
        public System.Threading.Tasks.Task<int> AddRoleAsync(string name, string authory) {
            return base.Channel.AddRoleAsync(name, authory);
        }
        
        public int ChangeUserRoles(string name, string roles) {
            return base.Channel.ChangeUserRoles(name, roles);
        }
        
        public System.Threading.Tasks.Task<int> ChangeUserRolesAsync(string name, string roles) {
            return base.Channel.ChangeUserRolesAsync(name, roles);
        }
        
        public string[] GetUsetRoles(int userId) {
            return base.Channel.GetUsetRoles(userId);
        }
        
        public System.Threading.Tasks.Task<string[]> GetUsetRolesAsync(int userId) {
            return base.Channel.GetUsetRolesAsync(userId);
        }
        
        public string[] GetUsetRolesByName(string userName) {
            return base.Channel.GetUsetRolesByName(userName);
        }
        
        public System.Threading.Tasks.Task<string[]> GetUsetRolesByNameAsync(string userName) {
            return base.Channel.GetUsetRolesByNameAsync(userName);
        }
        
        public System.Collections.Generic.Dictionary<int, string> GetUsersOfRole(string roleName) {
            return base.Channel.GetUsersOfRole(roleName);
        }
        
        public System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<int, string>> GetUsersOfRoleAsync(string roleName) {
            return base.Channel.GetUsersOfRoleAsync(roleName);
        }
        
        public int ChangeRoleAuthory(string name, string authory) {
            return base.Channel.ChangeRoleAuthory(name, authory);
        }
        
        public System.Threading.Tasks.Task<int> ChangeRoleAuthoryAsync(string name, string authory) {
            return base.Channel.ChangeRoleAuthoryAsync(name, authory);
        }
        
        public string[] GetRolse() {
            return base.Channel.GetRolse();
        }
        
        public System.Threading.Tasks.Task<string[]> GetRolseAsync() {
            return base.Channel.GetRolseAsync();
        }
    }
}