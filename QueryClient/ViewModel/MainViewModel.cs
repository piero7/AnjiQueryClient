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
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                    WelcomeTitle = item.Title;
                });

            lv.ShowDialog();
        }

        #region 参数

        private readonly IDataService _dataService;

        private const string FormTitlePropertyName = "_formTitle";
        private string _formTitle = "查询平台管理客户端";

        public LoginView lv = new LoginView();
        public SystemMenagerService.LoginUser _user = new SystemMenagerService.LoginUser();
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

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }

            set
            {
                if (_welcomeTitle == value)
                {
                    return;
                }

                _welcomeTitle = value;
                RaisePropertyChanged("WelcomeTitle");
            }
        }

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