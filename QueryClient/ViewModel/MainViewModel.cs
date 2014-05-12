using GalaSoft.MvvmLight;
using QueryClient.Model;

using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;

namespace QueryClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        public static SystemMenagerService.LoginUser NowUser = new SystemMenagerService.LoginUser();
        private DispatcherTimer timer = new DispatcherTimer();
        private int keepSpan = 60;
        private int checkSafeSpan = 10;
        private int checkCancelSpan = 500;

        private int clearSpan = 999999;
        private int span = 0;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            User = new SystemMenagerService.LoginUser { RealName = "" };
            #region MyRegion
            //Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("登录"), "startToLogin");
            //System.Windows.MessageBox.Show("startToLogin");

            //this.IconPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"..\..\Resources\anjilogo.ico"; 
            #endregion
            this._formTitle = "查询平台管理客户端";

            lv = new LoginView();
            _user = new SystemMenagerService.LoginUser();
            _isEnable = false;

            #region init spans
            this.keepSpan = int.Parse(System.Configuration.ConfigurationManager.AppSettings["keepSpan"]);
            this.checkCancelSpan = int.Parse(System.Configuration.ConfigurationManager.AppSettings["checkCancelSpan"]);
            this.checkSafeSpan = int.Parse(System.Configuration.ConfigurationManager.AppSettings["checkSafeSpan"]);
            this.clearSpan = GetMinGongBeiShu(new List<int> { this.keepSpan, this.checkCancelSpan, this.checkSafeSpan });
            #endregion

            //init the timer
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.span++;
            if (this.span % keepSpan == 0)
            {
                //TODO: keep line
            }
            if (this.span % this.checkSafeSpan == 0)
            {
                //TODO: update the b/n notes
            }
            if (this.span % this.checkCancelSpan == 0)
            {
                //TODO: cancel the codes 
            }
            if (this.span % this.clearSpan == 0)
            {
                this.span = 0;
            }

        }

        #region 参数
        private const string FormTitlePropertyName = "_formTitle";
        private string _formTitle;

        public LoginView lv;
        public SystemMenagerService.LoginUser _user;

        public bool _isEnable;

        private string iconPath;
        #endregion

        #region 属性
        public string FormTitle
        {
            get
            {
                return this._formTitle;
            }
            set
            {
                if (this._formTitle == value)
                {
                    return;
                }
                this._formTitle = value;
                RaisePropertyChanged(FormTitlePropertyName);
            }
        }

        public string IconPath
        {
            get
            {
                return this.iconPath;
            }
            set
            {
                if (this.iconPath == value)
                {
                    return;
                }
                this.iconPath = value;
                RaisePropertyChanged("IconPath");
            }
        }

        public string RealName
        {
            get
            {
                return this.User.RealName;
            }
            private set
            {
                RaisePropertyChanged("RealName");
                RaisePropertyChanged("WelcomeTitle");
            }
        }

        public SystemMenagerService.LoginUser User
        {
            get
            {
                return this._user;
            }
            set
            {
                this._user = value;
                MainViewModel.NowUser = value;
                RaisePropertyChanged("User");
                RaisePropertyChanged("RealName");
                RaisePropertyChanged("WelcomeTitle");
            }
        }

        public bool IsEnable
        {
            get
            {
                return this._isEnable;
            }
            set
            {
                if (this._isEnable == value)
                {
                    return;
                }
                this._isEnable = value;
                RaisePropertyChanged("IsEnable");
            }
        }

        public string WelcomeTitleWithName
        {
            get
            {
                string sayHellow;
                if (System.DateTime.Now.Hour < 12)
                {
                    sayHellow = "上午好";
                }
                else if (DateTime.Now.Hour < 14)
                {
                    sayHellow = "中午好";
                }
                else if (DateTime.Now.Hour < 18)
                {
                    sayHellow = "下午好";
                }
                else
                {
                    sayHellow = "晚上好";
                }
                return string.Format("{0},{1}!", RealName, sayHellow);
            }
            set
            {
                RaisePropertyChanged("WelcomeTitleWithName");
            }
        }


        public string WelcomeTitle
        {
            get
            {
                string sayHellow;
                if (System.DateTime.Now.Hour < 12)
                {
                    sayHellow = "上午好";
                }
                else if (DateTime.Now.Hour < 14)
                {
                    sayHellow = "中午好";
                }
                else if (DateTime.Now.Hour < 18)
                {
                    sayHellow = "下午好";
                }
                else
                {
                    sayHellow = "晚上好";
                }
                return sayHellow;
            }
            set
            {
                RaisePropertyChanged("WelcomeTitle");
            }
        }
        #endregion

        #region 命令
        public ICommand ChangePassword
        {
            get
            {
                return new RelayCommand(ChangePasswordExec, CanChangePassword);
            }
        }

        public ICommand ShowConfig
        {
            get
            {
                return new RelayCommand(ShowConfigExec, CanShowConfig);
            }
        }

        private void ShowConfigExec()
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(new System.Action(
               () =>
               {
                   ConfigView cv = new ConfigView();
                   cv.ShowDialog();
               }
               ));
        }

        private bool CanShowConfig()
        {
            return true;
        }



        #endregion

        #region  方法
        private bool CanChangePassword()
        {
            return true;
        }

        private void ChangePasswordExec()
        {
            GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(new System.Action(
                () =>
                {
                    ChangePasswordView cpv = new ChangePasswordView();
                    cpv.ShowDialog();
                }
                ));

        }

        public static int GetMinGongBeiShu(List<int> ints)
        {
            int u = ints.Max() - 1;
            bool isReal = false;
            for (; !isReal; u++)
            {
                isReal = true;
                ints.ForEach(i => isReal = isReal && (u % i == 0));
            }
            return u;
        }


        #endregion

        #region test iteam
        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
        #endregion
    }
}