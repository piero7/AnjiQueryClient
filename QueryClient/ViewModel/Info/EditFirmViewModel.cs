using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;

namespace QueryClient.ViewModel
{
    public class EditFirmViewModel : EditInfoViewModel
    {

        public ICommand Edit
        {
            get
            {
                return new RelayCommand(EditExec, CanEdit);
            }
        }

        private void EditExec()
        {
            InfoManagerService.InfoManagerServiceClient client = new InfoManagerService.InfoManagerServiceClient();
            try
            {
                client.Open();
                client.UpdateTarget(GetFirmModel());

                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("EditFirm"), "flyOut");
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send<GenericMessage<System.Exception>>(new GenericMessage<System.Exception>(ex), "InfoError");
                //TODO save the messenge
            }
            finally
            {
                client.Close();
            }
        }

        //private bool CanEdit()
        //{
        //    return true;
        //}
        public EditFirmViewModel()
        {
            Messenger.Default.Register<GenericMessage<InfoManagerService.QueryTargetInfo>>(this, "EditFirm", msg =>
            {
                Info = msg.Content;
                this.oInfo = new InfoManagerService.QueryTargetInfo
                {
                    Id = msg.Content.Id,
                    ImgPath = msg.Content.ImgPath,
                    Info = msg.Content.Info,
                    SimpleInfo = msg.Content.SimpleInfo,
                    Name = msg.Content.Name,
                    Remark = msg.Content.Remark,
                    VoicePath = msg.Content.VoicePath
                };
            });
        }

        private InfoManagerService.Firm GetFirmModel()
        {
            var ret = new InfoManagerService.Firm
            {
                Idk__BackingField = this.Info.Id,
                Namek__BackingField = this.Info.Name,
                ImgPathk__BackingField = this.Info.ImgPath,
                Remarkk__BackingField = this.Info.Remark,
                Infok__BackingField = this.Info.Info,
                SimpleInfok__BackingField = this.Info.SimpleInfo,
                VoicePathk__BackingField = this.Info.VoicePath,
            };
            return ret;
        }
    }
}