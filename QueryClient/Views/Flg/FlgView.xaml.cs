using System.Windows;

using GalaSoft.MvvmLight.Messaging;

namespace QueryClient
{
    /// <summary>
    /// Description for FlgView.
    /// </summary>
    public partial class FlgView : MahApps.Metro.Controls.MetroWindow
    {
        private QueryClient.ViewModel.FlgViewModel vm;

        public FlgView(InfoManagerService.QueryTargetInfo info, InfoManagerService.FlgMold mold)
        {
            InitializeComponent();
            this.vm = new ViewModel.FlgViewModel(info, mold);
            this.DataContext = vm;
            #region Messenger
            Messenger.Default.Register<GenericMessage<string>>(this, "FlgFlyOut", msg =>
            {
                switch (msg.Content)
                {
                    case "Edit":
                        this.Edit_fo.IsOpen = !this.Edit_fo.IsOpen;
                        break;
                    case "Add":
                        this.Add_fo.IsOpen = !this.Add_fo.IsOpen;
                        break;
                }
            });
            #endregion
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}