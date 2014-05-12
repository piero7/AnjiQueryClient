using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

using GalaSoft.MvvmLight.Messaging;
namespace QueryClient.ViewModel
{
    public class AddFlgViewModel : ViewModelBase
    {
        public const string FlgPropertyName = "Flg";
        private InfoManagerService.Flg _myProperty;


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

        public ICommand Add
        {
            get
            {
                return new RelayCommand(AddExec, CanAdd);
            }
        }

        private bool CanAdd()
        {
            return true;
        }

        private async void AddExec()
        {
            InfoManagerService.InfoManagerServiceClient client = new InfoManagerService.InfoManagerServiceClient();
            try
            {
                var addTask = client.AddFlgAsync(this.Flg);
                await addTask;
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("Add"), "FlgFlyOut");
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("RefreshFlg"), "RefreshFlg");
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

        public AddFlgViewModel()
        {
            Messenger.Default.Register<GenericMessage<InfoManagerService.Flg>>(this, "AddFlg", msg =>
            {
                this.Flg = msg.Content;
            });
        }
    }
}