using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;

namespace QueryClient.ViewModel
{
    public class FlgViewModel : ViewModelBase
    {
        #region 参数
        public const string InfoPropertyName = "Info";
        public const string InfoNamePropertyName = "InfoName";
        private InfoManagerService.QueryTargetInfo _info;


        public const string MoldPropertyName = "Mold";
        private InfoManagerService.FlgMold _mold;

        public const string FlgListPropertyName = "FlgList";
        private List<InfoManagerService.Flg> _flgList;



        #endregion
        #region 属性
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
                RaisePropertyChanging(InfoNamePropertyName);
                _info = value;
                RaisePropertyChanged(InfoPropertyName);
                RaisePropertyChanged(InfoNamePropertyName);
            }
        }

        public string InfoName
        {
            get
            {
                return this.Info.Name;
            }
        }

        public string Mold
        {
            get
            {
                switch (this._mold)
                {
                    case InfoManagerService.FlgMold.Firm:
                        return "厂家";
                    case InfoManagerService.FlgMold.Series:
                        return "系列";
                    case InfoManagerService.FlgMold.Product:
                        return "商品";
                    default:
                        return "";
                }
            }

            set
            {
                if (_mold.ToString() == value)
                {
                    return;
                }
                var mo = (InfoManagerService.FlgMold)System.Enum.Parse(typeof(InfoManagerService.FlgMold), value);
                RaisePropertyChanging(MoldPropertyName);
                _mold = mo;
                RaisePropertyChanged(MoldPropertyName);
            }
        }

        public List<InfoManagerService.Flg> FlgList
        {
            get
            {
                return _flgList;
            }

            set
            {
                if (_flgList == value)
                {
                    return;
                }

                RaisePropertyChanging(FlgListPropertyName);
                _flgList = value;
                RaisePropertyChanged(FlgListPropertyName);
            }
        }

        #endregion
        public ICommand Edit
        {

            get
            {
                return new RelayCommand<InfoManagerService.Flg>(EditExec, CanEdit);
            }
        }

        public ICommand Add
        {
            get
            {
                return new RelayCommand(AddExec, CanAdd);
            }
        }

        public ICommand Refresh
        {
            get
            {
                return new RelayCommand(RefreshExec, CanRefresh);
            }
        }

        public ICommand SelectionChanged
        {
            get
            {
                return new RelayCommand<InfoManagerService.QueryMold>(SelectedChangedExec);
            }
        }



        #region 命令

        #endregion

        #region 方法
        private void SelectedChangedExec(InfoManagerService.QueryMold obj)
        {

        }

        private async void RefreshExec()
        {
            InfoManagerService.InfoManagerServiceClient client = new InfoManagerService.InfoManagerServiceClient();
            try
            {
                client.Open();
                var queryTask = client.GetFlgs1Async(this.Info.Id, _mold);
                this.FlgList = new List<InfoManagerService.Flg>(await queryTask);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send<GenericMessage<System.Exception>>(new GenericMessage<System.Exception>(ex), "FlgError");
            }
            finally
            {
                client.Close();
            }
        }

        private bool CanRefresh()
        {
            return true;
        }
        private void AddExec()
        {
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("Add"), "FlgFlyOut");
            Messenger.Default.Send<GenericMessage<InfoManagerService.Flg>>(new GenericMessage<InfoManagerService.Flg>(
                new InfoManagerService.Flg
                {
                    IsEnable = true,
                    QueryTable = _mold,
                    QueryTarget = Info.Id,
                    FlgNumber = InfoManagerService.ReturnMold.Text,
                    QueryType = InfoManagerService.QueryMold.Weixin
                }
                ), "AddFlg");
        }

        private bool CanAdd()
        {
            return true;
        }

        private void EditExec(InfoManagerService.Flg obj)
        {
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("Edit"), "FlgFlyOut");
            Messenger.Default.Send<GenericMessage<InfoManagerService.Flg>>(new GenericMessage<InfoManagerService.Flg>(obj), "EditFlg");
        }

        private bool CanEdit(InfoManagerService.Flg arg)
        {
            if (arg != null)
            {
                return true;
            }
            return false;
        }

        #endregion



        public FlgViewModel()
        {
            if (this.Info != null)
            {
                RefreshExec();
            }
        }

        public FlgViewModel(InfoManagerService.QueryTargetInfo info, InfoManagerService.FlgMold mold)
        {
            // TODO: Complete member initialization
            Messenger.Default.Register<GenericMessage<string>>(this, "RefreshFlg", (msg) =>
            {
                this.RefreshExec();
            });

            this.Info = info;
            this._mold = mold;
            if (this.Info != null)
            {
                RefreshExec();
            }
        }
    }
}