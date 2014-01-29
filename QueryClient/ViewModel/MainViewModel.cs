using GalaSoft.MvvmLight;
using QueryClient.Model;

using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System;
using GalaSoft.MvvmLight.Messaging;

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

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            this.User = new SystemMenagerService.LoginUser { RealName = "" };
            //Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("登录"), "startToLogin");
            //System.Windows.MessageBox.Show("startToLogin");
        }

        #region 参数


        private const string FormTitlePropertyName = "_formTitle";
        private string _formTitle = "查询平台管理客户端";

        public LoginView lv = new LoginView();
        public SystemMenagerService.LoginUser _user = new SystemMenagerService.LoginUser();

        public bool _isEnable = false;

        private string iconPath = "";
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
                return string.Format("{0},{1}!", RealName, sayHellow);
            }
            set
            {
                RaisePropertyChanged("WelcomeTitle");
            }
        }
        #endregion

        #region 命令
        #endregion

        #region  方法

        #endregion

        #region test itme
        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";
        public const string LabelStr = "I am a label ";
        public string _lableCon = "I am a lable";

        private string _welcomeTitle = string.Empty;
        private int _count = 0;


        public string LabelCon
        {
            get
            {
                return _lableCon;
            }
            set
            {
                if (_lableCon == value)
                {
                    return;
                }
                this._lableCon = value;
                RaisePropertyChanged<string>("LabelCon");
            }
        }



        public ICommand GetNewLabelCon
        {
            get
            {
                return new RelayCommand(
                   new Action(() =>
                   {
                       this.LabelCon = LabelStr + _count++;
                       this.WelcomeTitle = this.WelcomeTitle + _count;
                       Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("Now, the count is " + _count), "count");
                       //MessageBox.Show(LabelCon+ WelcomeTitle);
                   }),
                    new Func<bool>(() => true));
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
        #endregion
    }
}