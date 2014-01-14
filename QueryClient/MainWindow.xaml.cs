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
        private MainViewModel model = new MainViewModel(new DataService());

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = model;
            Closing += (s, e) => ViewModelLocator.Cleanup();


            Messenger.Default.Register<GenericMessage<string>>(
               this,
               "count",
             async msg =>
             {
                 MessageDialogResult ret = await ShowMessageAsync("message", msg.Content, MessageDialogStyle.AffirmativeAndNegative);
                 //ret.Wait();
                 await ShowMessageAsync("Request", ret.ToString());
             });

        }
    }
}