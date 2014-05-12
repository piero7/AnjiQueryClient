using System.Windows;

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
namespace QueryClient
{
    /// <summary>
    /// Description for ChangePasswordView.
    /// </summary>
    public partial class ChangePasswordView : MahApps.Metro.Controls.MetroWindow
    {
        private ViewModel.ChagePasswordViewModel vm = new ViewModel.ChagePasswordViewModel();

        public ChangePasswordView()
        {
            InitializeComponent();
            this.DataContext = vm;

            Messenger.Default.Register<GenericMessage<string>>(this, "ChangePasswordMsg", msg =>
            {
                switch (msg.Content)
                {
                    case "Success":
                        DispatcherHelper.CheckBeginInvokeOnUI(new System.Action(async () =>
                        {
                            await ShowMessageAsync("提示", "已经成功修改密码！");
                            this.Close();
                        }));
                        return;
                    default:
                        DispatcherHelper.CheckBeginInvokeOnUI(new System.Action(async () =>
                       {
                           await ShowMessageAsync("提示", msg.Content);
                       }));
                        return;
                }
            });

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}