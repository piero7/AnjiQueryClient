using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace QueryClient.ViewModel
{

    /// <summary>
    /// 编辑info ViewModel基类
    /// </summary>
    public class EditInfoViewModel : ViewModelBase
    {
        protected InfoManagerService.QueryTargetInfo oInfo = new InfoManagerService.QueryTargetInfo();

        public const string CheckedPropertyName = "Checked";
        private bool _checked = false;

        public const string InfoPropertyName = "Info";
        private InfoManagerService.QueryTargetInfo _info;

        public InfoManagerService.QueryTargetInfo Info
        {
            get
            {
                return _info;
            }

            set
            {
                if (_info == value)
                {
                    return;
                }

                RaisePropertyChanging(InfoPropertyName);
                _info = value;
                RaisePropertyChanged(InfoPropertyName);
            }
        }

        public bool Checked
        {
            get
            {
                return _checked;
            }

            set
            {
                if (_checked == value)
                {
                    return;
                }

                RaisePropertyChanging(CheckedPropertyName);
                _checked = value;
                RaisePropertyChanged(CheckedPropertyName);
            }
        }
        public ICommand CleanArgs
        {
            get
            {
                return new RelayCommand(CleanArgsExec, CanCleaArgs);
            }
        }

        private void CleanArgsExec()
        {
            this.Info = new InfoManagerService.QueryTargetInfo();
            this.Info = oInfo;
        }

        private bool CanCleaArgs()
        {
            return true;
        }



        protected bool CanEdit()
        {
            return true;
        }
        public EditInfoViewModel()
        {
        }


    }
}
