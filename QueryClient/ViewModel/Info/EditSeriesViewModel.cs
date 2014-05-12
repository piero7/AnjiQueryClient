using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace QueryClient.ViewModel
{

    public class EditSeriesViewModel : EditInfoViewModel
    {

        public EditSeriesViewModel()
        {
            Messenger.Default.Register<GenericMessage<InfoManagerService.QueryTargetInfo>>(this, "EditSeries", msg =>
            {
                this.Info = msg.Content;
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
                client.UpdateTarget(GetModel());

                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("EditSeries"), "flyOut");
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send<GenericMessage<System.Exception>>(new GenericMessage<System.Exception>(ex), "InfoError");
                //TODO save the messenger!!
            }
            finally
            {
                client.Close();
            }
        }

        private InfoManagerService.Series GetModel()
        {
            var ret = new InfoManagerService.Series
            {
                Namek__BackingField = Info.Name,
                Infok__BackingField = Info.Info,
                ImgPathk__BackingField = Info.ImgPath,
                Idk__BackingField = Info.Id,
                VoicePathk__BackingField = Info.VoicePath,
                Remarkk__BackingField = Info.Remark,
                SimpleInfok__BackingField = Info.SimpleInfo,
            };
            return ret;
        }
    }
}