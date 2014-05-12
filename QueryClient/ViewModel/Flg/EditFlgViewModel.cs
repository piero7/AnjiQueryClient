using GalaSoft.MvvmLight;

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Collections.Generic;

namespace QueryClient.ViewModel
{
    public class EditFlgViewModel : ViewModelBase
    {

        public const string FlgPropertyName = "Flg";
        private InfoManagerService.Flg _myProperty;

        private InfoManagerService.Flg _oFlg;

        private Dictionary<InfoManagerService.ReturnMold, string> ReturnMold;
        private Dictionary<InfoManagerService.QueryMold, string> QueryMold;


        #region 属性

        public InfoManagerService.Flg Flg
        {
            get
            {
                return _myProperty;
            }

            set
            {
                if (_myProperty == value)
                {
                    return;
                }

                RaisePropertyChanging(FlgPropertyName);
                _myProperty = value;
                RaisePropertyChanged(FlgPropertyName);
            }
        }
        #endregion

        public ICommand Edit
        {
            get
            {
                return new RelayCommand(EditExec, CanEdit);
            }
        }

        public ICommand CleanArgs
        {
            get
            {
                return new RelayCommand(CleanArgsExex, CanCleanArgs);
            }
        }

        private void CleanArgsExex()
        {
            this.Flg = _oFlg;
            Flg.FlgNumber = InfoManagerService.ReturnMold.FilePath;
            Flg.QueryType = InfoManagerService.QueryMold.Weixin;
        }

        private bool CanCleanArgs()
        {
            return true;
        }

        private bool CanEdit()
        {
            return true;
        }

        private async void EditExec()
        {
            InfoManagerService.InfoManagerServiceClient client = new InfoManagerService.InfoManagerServiceClient();
            try
            {
                client.Open();
                var editTask = client.UpdateFlgAsync(this.Flg);
                await editTask;

                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("Edit"), "FlgFlyOut");
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send<GenericMessage<System.Exception>>(new GenericMessage<System.Exception>(ex), "FlgQueryError");
            }
            finally
            {
                client.Close();
            }
        }

        public EditFlgViewModel()
        {
            this.QueryMold = new Dictionary<InfoManagerService.QueryMold, string>();
            this.QueryMold.Add(InfoManagerService.QueryMold.Weixin, "微信");
            this.QueryMold.Add(InfoManagerService.QueryMold.Web, "网页");
            this.QueryMold.Add(InfoManagerService.QueryMold.ShotMsg, "短信");
            this.QueryMold.Add(InfoManagerService.QueryMold.Phone, "电话");
            this.QueryMold.Add(InfoManagerService.QueryMold.Innner, "内部查询");
            this.QueryMold.Add(InfoManagerService.QueryMold.Debug, "调试");

            this.ReturnMold = new Dictionary<InfoManagerService.ReturnMold, string>();

            Messenger.Default.Register<GenericMessage<InfoManagerService.Flg>>(this, "EditFlg", msg =>
            {
                this.Flg = msg.Content;
                this._oFlg = msg.Content;
            });
        }
    }
}