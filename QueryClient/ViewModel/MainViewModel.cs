using GalaSoft.MvvmLight;
using QueryClient.Model;

using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace QueryClient.ViewModel
{

    public class MainViewModel : ViewModelBase
    {




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

            this.syncBlack = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SyacBlackNoteSpan"]);
            this.syncWhite = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SyncWhiteNoteSpan"]);

            //初始化重置timeSpan间隔
            this.clearSpan = GetMinGongBeiShu(new List<int> { this.keepSpan, this.checkCancelSpan, this.checkSafeSpan, this.syncWhite, this.syncBlack });

            #endregion

            #region init
            this.IsPhoneOnline = true;
            this.IsWebOnline = true;
            this.IsWeinxinOnline = true;
            this.IsSmOnline = true;

            this.TotalTime = new DateTime(0);
            #endregion
            //init the timer
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();

            #region register msg

            Messenger.Default.Register<GenericMessage<bool>>("CheckWebSatus", msg => this.IsWebOnline = msg.Content);
            Messenger.Default.Register<GenericMessage<bool>>("CheckWeixinSatus", msg => this.IsWeinxinOnline = msg.Content);
            Messenger.Default.Register<GenericMessage<bool>>("CheckPhoneSatus", msg => this.IsPhoneOnline = msg.Content);
            #endregion
        }

        #region 参数
        public static SystemMenagerService.LoginUser NowUser = new SystemMenagerService.LoginUser();
        private DispatcherTimer timer = new DispatcherTimer();
        private int keepSpan = 60;
        private int checkSafeSpan = 10;
        private int checkCancelSpan = 500;

        private int clearSpan = 999999;
        private int span = 0;

        private int syncBlack = 0;
        private int syncWhite = 0;

        Guard d = new Guard();
        Fliter f = new Fliter();

        private const string FormTitlePropertyName = "_formTitle";
        private string _formTitle;

        public LoginView lv;
        public SystemMenagerService.LoginUser _user;

        public bool _isEnable;

        private string iconPath;


        public const string IsWebOnlinePropertyName = "IsWebOnline";
        private bool _isWebOnline = false;

        public const string IsPhoneOnlinePropertyName = "IsPhoneOnline";
        private bool _isPhoneOnline = false;

        public const string IsWeinxinOnlinePropertyName = "IsWeinxinOnline";
        private bool _isWeixinOnline = false;

        public const string IsSmOnlinePropertyName = "IsSmOnline";
        private bool _isSmOnlien = false;
        public const string TotalTimePropertyName = "TotalTime";

        private DateTime _totalTime = new DateTime(0);

        public const string TotalTimeStringPropertyName = "TotalTimeString";
        private string _totalTimeString = string.Empty;

        public const string TodayQueryCountPropertyName = "TodayQueryCount";
        private int _todayQueryCount = 0;

        public const string LogListPropertyName = "LogList";
        private BindingList<QueryClient.LogService.QueryLog> _queryLog = new BindingList<LogService.QueryLog>();

        public const string BlackListPropertyName = "BlackList";
        private ObservableCollection<FliterService.BlackNote> _blackList = new ObservableCollection<FliterService.BlackNote>();

        public const string WhiteListPropertyName = "WhiteList";
        private ObservableCollection<FliterService.WhiteNote> _whiteList = new ObservableCollection<FliterService.WhiteNote>();

        public const string IsOldCodeOnlinePropertyName = "IsOldCodeOnline";
        private bool _isOldCodeOnline = false;

        public const string Is4d4cOnlinePropertyName = "Is4d4cOnline";
        private bool _is4d4cOnline = false;



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

        public bool IsWebOnline
        {
            get
            {
                return _isWebOnline;
            }

            set
            {
                if (_isWebOnline == value)
                {
                    return;
                }

                RaisePropertyChanging(IsWebOnlinePropertyName);
                _isWebOnline = value;
                RaisePropertyChanged(IsWebOnlinePropertyName);
            }
        }

        public bool IsPhoneOnline
        {
            get
            {
                return _isPhoneOnline;
            }

            set
            {
                if (_isPhoneOnline == value)
                {
                    return;
                }

                RaisePropertyChanging(IsPhoneOnlinePropertyName);
                _isPhoneOnline = value;
                RaisePropertyChanged(IsPhoneOnlinePropertyName);
            }
        }

        public bool IsWeinxinOnline
        {
            get
            {
                return _isWeixinOnline;
            }

            set
            {
                if (_isWeixinOnline == value)
                {
                    return;
                }

                RaisePropertyChanging(IsWeinxinOnlinePropertyName);
                _isWeixinOnline = value;
                RaisePropertyChanged(IsWeinxinOnlinePropertyName);
            }
        }

        public bool IsSmOnline
        {
            get
            {
                return _isSmOnlien;
            }

            set
            {
                if (_isSmOnlien == value)
                {
                    return;
                }

                RaisePropertyChanging(IsSmOnlinePropertyName);
                _isSmOnlien = value;
                RaisePropertyChanged(IsSmOnlinePropertyName);
            }
        }

        public DateTime TotalTime
        {
            get
            {
                return _totalTime;
            }

            set
            {
                if (_totalTime == value)
                {
                    return;
                }

                RaisePropertyChanging(TotalTimePropertyName);
                _totalTime = value;
                RaisePropertyChanged(TotalTimePropertyName);
                this.TotalTimeString = "运行时间： " + value.ToLongTimeString();
            }
        }

        public string TotalTimeString
        {
            get
            {
                return _totalTimeString;
            }

            set
            {
                if (_totalTimeString == value)
                {
                    return;
                }

                RaisePropertyChanging(TotalTimeStringPropertyName);
                _totalTimeString = value;
                RaisePropertyChanged(TotalTimeStringPropertyName);
            }
        }

        public int TodayQueryCount
        {
            get
            {
                return _todayQueryCount;
            }

            set
            {
                if (_todayQueryCount == value)
                {
                    return;
                }

                //  RaisePropertyChanging(TodayQueryCountPropertyName);
                _todayQueryCount = value;
                RaisePropertyChanged(TodayQueryCountString);
                RaisePropertyChanged(TodayQueryCountPropertyName);
            }
        }

        public string TodayQueryCountString
        {
            get
            {
                return string.Format("今日查询 {0} 条", this.LogList.Count());
            }
        }

        public BindingList<QueryClient.LogService.QueryLog> LogList
        {
            get
            {
                return _queryLog;
            }

            set
            {
                if (_queryLog == value)
                {
                    return;
                }
                // RaisePropertyChanging(LogListPropertyName);
                _queryLog = value;
                RaisePropertyChanged(LogListPropertyName);
                RaisePropertyChanged("TodayQueryCountString");
            }
        }

        public ObservableCollection<FliterService.WhiteNote> WhiteList
        {
            get
            {
                return _whiteList;
            }

            set
            {

                //   RaisePropertyChanging(WhiteListPropertyName);
                _whiteList = value;
                RaisePropertyChanged(WhiteListPropertyName);
            }
        }

        public ObservableCollection<FliterService.BlackNote> BlackList
        {
            get
            {
                return _blackList;
            }

            set
            {

                //RaisePropertyChanging(BlackListPropertyName);
                _blackList = value;
                RaisePropertyChanged(BlackListPropertyName);
            }
        }

        public bool Is4d4cOnline
        {
            get
            {
                return _is4d4cOnline;
            }

            set
            {
                if (_is4d4cOnline == value)
                {
                    return;
                }

                RaisePropertyChanging(Is4d4cOnlinePropertyName);
                _is4d4cOnline = value;
                RaisePropertyChanged(Is4d4cOnlinePropertyName);
            }
        }

        public bool IsOldCodeOnline
        {
            get
            {
                return _isOldCodeOnline;
            }

            set
            {
                if (_isOldCodeOnline == value)
                {
                    return;
                }

                RaisePropertyChanging(IsOldCodeOnlinePropertyName);
                _isOldCodeOnline = value;
                RaisePropertyChanged(IsOldCodeOnlinePropertyName);
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
            return u - 1;
        }


        #endregion

        #region 事件
        async private void timer_Tick(object sender, EventArgs e)
        {

            if (this.span % keepSpan == 0)
            {
                this.IsPhoneOnline = await d.CheckPhone();
                this.IsWebOnline = await d.CheckWeb();
                this.IsWeinxinOnline = await d.CheckWeixin();
                this.IsSmOnline = await d.CheckSm();
                this.Is4d4cOnline = await d.Check4d4c();
                this.IsOldCodeOnline = await d.CheckOldCode();
            }
            if (this.span % this.checkSafeSpan == 0)
            {
                //TODO: update the b/n notes
                await f.FliterBlackNote(this.span % this.syncWhite == 0, this.span % this.syncBlack == 0);
                this.BlackList = new ObservableCollection<FliterService.BlackNote>(f.BlackList);
                this.WhiteList = new ObservableCollection<FliterService.WhiteNote>(f.WhiteList);
                this.LogList = new BindingList<LogService.QueryLog>(f.TodayLogList.OrderByDescending(n => n.OptionDate).ToList());
            }
            if (this.span % this.checkCancelSpan == 0)
            {
                //TODO: cancel the codes 
            }

            this.TotalTime = this.TotalTime.AddSeconds(1d);
            this.span++;
            if (this.span % this.clearSpan == 0)
            {
                this.span = 0;
            }
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