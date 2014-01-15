using System.Windows;
using QueryClient.ViewModel;
using MahApps.Metro.Controls;

using QueryClient.Model;
using GalaSoft.MvvmLight.Messaging;

namespace QueryClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainViewModel mv = new MainViewModel(new DataService());

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = mv;
            Closing += (s, e) => ViewModelLocator.Cleanup();

            #region 消息注册
            Messenger.Default.Register<GenericMessage<SystemMenagerService.LoginUser>>(this, "loginSuccess", msg => this.mv.User = msg.Content);
            #endregion
        }
    }
}