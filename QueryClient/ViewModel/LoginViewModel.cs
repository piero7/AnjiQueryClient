﻿using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using QueryClient.Model;
using QueryClient.SystemMenagerService;
using GalaSoft.MvvmLight.Threading;
using System.Threading.Tasks;

namespace QueryClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            //  Messenger.Default.Register<GenericMessage<string>>(this, "startLogin", msg => LonginExec());


        }

        #region 参数
        private string _userName = "";
        private string _password = "";
        public bool _isLogin = false;
        public string _title;
        public bool _isRunning = false;
        public bool _isChecking = false;
        #endregion

        #region 属性
        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if (value == this._title)
                    return;
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }


        public string UserName
        {
            get
            {
                return this._userName;
            }
            set
            {
                if (value == this._userName)
                {
                    return;
                }
                this._userName = value;
                RaisePropertyChanged("UserName");
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                if (this._password == value)
                {
                    return;
                }
                this._password = value;
                RaisePropertyChanged("_password");
            }
        }

        public bool IsRunning
        {
            get
            {
                return this._isRunning;
            }
            set
            {
                if (value == this._isRunning)
                {
                    return;
                }
                this._isRunning = value;
                RaisePropertyChanged("IsRunning");
                RaisePropertyChanged("IsNotRunning");
            }
        }

        public bool IsChecking
        {
            get
            {
                return this._isChecking;
            }
            set
            {
                if (this._isChecking == value)
                {
                    return;
                }
                this._isChecking = value;
                this.IsRunning = value;
                RaisePropertyChanged("IsChecking");
            }
        }

        public bool IsNotRunning
        {
            get
            {
                return !this._isRunning;
            }
        }

        #endregion

        #region 命令


        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ICommand Login
        {
            get
            {
                this.Password = this.Password.Trim();
                this.UserName = this.UserName.Trim();
                return new RelayCommand(LoginWithChangeStatus, CanLogin);
            }
        }

        public ICommand Close
        {
            get
            {
                return new RelayCommand(CloseExec, CanClose);
            }
        }

        public ICommand TestConn
        {
            get
            {
                return new RelayCommand(TestConnWithChangeStatus, () => true);
            }
        }
        #endregion

        #region 方法

        #region 登录

        public void LonginExec()
        {
            //            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "start");
            //Messenger.Default.Send<NotificationMessageAction<MahApps.Metro.Controls.ProgressRing>>(
            //    new NotificationMessageAction<MahApps.Metro.Controls.ProgressRing>("start",
            //        res => res.IsActive = true));
            // System.Threading.Thread.Sleep(5000);
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("update text of textbox "), "update");

            if (string.IsNullOrEmpty(this.UserName))//用户名验证
            {
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("用户名不能为空！"), "errorUserName");
                this._isLogin = false;
            }

            if (!string.IsNullOrEmpty(this.Password))
            {
                this.Password = Password.GetHashCode().ToString();
            }
            else
            {
                this.Password = null;
            }
            SystemMenagerService.SystemManagerServiceClient smService =
                new SystemMenagerService.SystemManagerServiceClient();
            try
            {
                smService.Open();
                var user = smService.Login(this.UserName, this.Password, System.Net.Dns.GetHostName());

                if (user != null)//登录成功
                {
                    // Debug
                    // System.Windows.MessageBox.Show("Login seuccess!");

                    Messenger.Default.Send<GenericMessage<SystemMenagerService.LoginUser>>(new GenericMessage<SystemMenagerService.LoginUser>(user), "success");
                }
                else
                {
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("账户或者密码错误！"), "errorUser");
                }
            }
            catch (System.Exception ex)
            {
                if (ex.Message.Contains("NotFindUsetException"))
                {
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("账户或者密码错误！"), "errorUser");
                }
                else
                {
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("登录错误,请联系管理员！\r\n错误描述： " + ex.Message), "systemError");
                }
            }
            finally
            {
                smService.Close();
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("Finish login"), "finish");
            }

            #region Debug
            //System.Threading.Thread.Sleep(2000);
            //if (this.UserName == "admin")
            //    Messenger.Default.Send<GenericMessage<SystemMenagerService.LoginUser>>(new GenericMessage<SystemMenagerService.LoginUser>(new LoginUser { RealName = "success" }), "success");
            //else
            //    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("账户或者密码错误！"), "errorUser");
            //Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(""), "finish");
            #endregion
            IsRunning = false;
            this.Title = "登录";
            return;
        }

        private void LoginWithChangeStatus()
        {
            this.IsRunning = true;
            this.Title = "登录中...";
            Messenger.Default.Send<NotificationMessageWithCallback>(new NotificationMessageWithCallback("", new System.Action(LonginExec)), "start");
        }


        public bool CanLogin()
        {
            return true;
        }
        #endregion

        #region 退出
        public void CloseExec()
        {
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("User close in login form."), "close");
        }
        public bool CanClose()
        {
            return true;
        }
        #endregion

        #region 测试连接


        async private Task<bool> TestConnExec()
        {
            SystemMenagerService.SystemManagerServiceClient sm = new SystemMenagerService.SystemManagerServiceClient();
            LogService.LogServiceClient lv = new LogService.LogServiceClient();
            InfoManagerService.InfoManagerServiceClient im = new InfoManagerService.InfoManagerServiceClient();

            bool testConnRet = false;
            string lToken = System.Configuration.ConfigurationManager.AppSettings["localToken"];
            try
            {
                var ran = new System.Random();
                var token = ran.Next(int.MaxValue).ToString();

                sm.Open();
                Task<int> testsm = sm.TestConnAsync(token);
                lv.Open();
                Task<int> testLv = lv.TestConnAsync(token);
                im.Open();
                Task<int> testIm = im.TestConnAsync(token);

                int smRet = await testsm;
                int imRet = await testIm;
                int lvRet = await testLv;

                testConnRet = (smRet == (token+lToken).GetHashCode()) || (smRet == imRet) || (smRet == lvRet);
            }
            catch (System.Exception)
            {
                this.IsChecking = false;
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("fail"), "connFail");
                return false;
            }

            finally
            {
                sm.Close();
                lv.Close();
                im.Close();
            }
            this.IsChecking = false;
            if (testConnRet)
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("success"), "loginSuccess");
            else
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("fail"), "connFail");
            this.Title = "登录";
            return true;
        }

        public void TestConnWithChangeStatus()
        {
            this.Title = "正在连接服务器...";
            this.IsChecking = true;
            Messenger.Default.Send<NotificationMessageWithCallback>(
               new NotificationMessageWithCallback("正在连接服务器...",
               new System.Action(() => this.TestConnExec())),
               "testConn");
        }
        #endregion

        #endregion

    }
}