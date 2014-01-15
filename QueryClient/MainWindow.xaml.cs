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
        private MainViewModel vm = new MainViewModel();

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = vm;
            Closing += (s, e) => { ViewModelLocator.Cleanup(); Application.Current.Shutdown(); };

            this.Loaded += (s, e) => vm.lv.ShowDialog();
            //this.vm = new MainViewModel();
            #region 消息注册

            //登录成功
            Messenger.Default.Register<GenericMessage<SystemMenagerService.LoginUser>>(this, "loginSuccess", msg =>
            {
                this.vm.User = msg.Content;
                this.vm.IsEnable = true;
            });


            #endregion
        }
    }
}