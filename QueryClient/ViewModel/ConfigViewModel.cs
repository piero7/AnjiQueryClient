using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Configuration;
using System.Windows.Input;

namespace QueryClient.ViewModel
{
    public class ConfigViewModel : ViewModelBase
    {
        public const string InfoCountPropertyName = "InfoCount";
        private int _infoCount = 0;

        public const string LogCountPropertyName = "LogCount";
        private int _logCount = 0;

        public const string TokenPropertyName = "Token";
        private string _token = "";


        public const string TitlePropertyName = "Title";

        private string _title = "系统设置";


        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title == value)
                {
                    return;
                }

                RaisePropertyChanging(TitlePropertyName);
                _title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        public string Token
        {
            get
            {
                return _token;
            }

            set
            {
                if (_token == value)
                {
                    return;
                }

                RaisePropertyChanging(TokenPropertyName);
                _token = value;
                RaisePropertyChanged(TokenPropertyName);
            }
        }

        public int LogCount
        {
            get
            {
                return _logCount;
            }

            set
            {
                if (_logCount == value)
                {
                    return;
                }

                RaisePropertyChanging(LogCountPropertyName);
                _logCount = value;
                RaisePropertyChanged(LogCountPropertyName);
            }
        }

        public int InfoCount
        {
            get
            {
                return _infoCount;
            }

            set
            {
                if (_infoCount == value)
                {
                    return;
                }

                RaisePropertyChanging(InfoCountPropertyName);
                _infoCount = value;
                RaisePropertyChanged(InfoCountPropertyName);
            }
        }

        public ICommand Save
        {
            get
            {
                return new RelayCommand(SaveExec, CanSave);
            }
        }


        public ICommand Close
        {
            get
            {
                return new RelayCommand(CloseExec, CanClose);
            }
        }

        private void CloseExec()
        {
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("Close"), "ConfigView");
        }

        private bool CanClose()
        {
            return true;
        }

        private void SaveExec()
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //configuration.AppSettings.Settings["WriteFileName"].Value = this.txtWriteFileName.Text;

            configuration.AppSettings.Settings["infoCount"].Value = this.InfoCount.ToString();
            configuration.AppSettings.Settings["logCount"].Value = this.LogCount.ToString();
            configuration.AppSettings.Settings["localToken"].Value = this.Token;
            configuration.Save(System.Configuration.ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");


            //System.Configuration.ConfigurationManager.AppSettings.Set("infoCount", this.InfoCount.ToString());
            //System.Configuration.ConfigurationManager.AppSettings.Set("logCount", this.LogCount.ToString());
            //System.Configuration.ConfigurationManager.AppSettings.Set("localToken", this.Token);
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("Close"), "ConfigView");
        }

        private bool CanSave()
        {
            return true;
        }

        public ConfigViewModel()
        {
            Init();
        }
        private void Init()
        {
            this.LogCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["logCount"]);
            this.InfoCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["infoCount"]);
            this.Token = System.Configuration.ConfigurationManager.AppSettings["localToken"];
        }
    }
}