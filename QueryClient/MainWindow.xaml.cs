using System.Windows;
using QueryClient.ViewModel;
using MahApps.Metro.Controls;

using QueryClient.Model;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro;
using System.Windows.Threading;
using System.Collections.Generic;

namespace QueryClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private MainViewModel vm = new MainViewModel();
        private DispatcherTimer timer = new DispatcherTimer();

        private int keepSpan = 60;
        private int checkSafeSpan = 10;
        private int checkCancelSpan = 500;

        private int clearSpan = 999999;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = vm;
            Closing += (s, e) => { ViewModelLocator.Cleanup(); Application.Current.Shutdown(); };

            this.Loaded += (s, e) => vm.lv.ShowDialog();
            //MahApps.Metro.Behaviours.ReloadBehavior.SetOnSelectedTabChanged(this.mainControl, false);
            //this.vm = new MainViewModel();
            #region 消息注册

            //登录成功
            Messenger.Default.Register<GenericMessage<SystemMenagerService.LoginUser>>(this, "loginSuccess", msg =>
            {
                this.vm.User = msg.Content;
                this.vm.IsEnable = true;
            });

            #region flyout

            Messenger.Default.Register<GenericMessage<string>>(this, "flyOut", (msg) =>
            {
                switch (msg.Content)
                {
                    case "QueryArgs":
                        this.QueryLogArgs_fo.IsOpen = !this.QueryLogArgs_fo.IsOpen;
                        break;
                    case "SystemArgs":
                        this.SystemLogArgs_fo.IsOpen = !this.SystemLogArgs_fo.IsOpen;
                        break;
                    case "ProductArgs":
                        this.ProducrArgs_fo.IsOpen = !this.ProducrArgs_fo.IsOpen;
                        break;
                    case "SeriesArgs":
                        this.SeriesArgs_fo.IsOpen = !this.SeriesArgs_fo.IsOpen;
                        break;
                    case "FirmArgs":
                        this.FirmArgs_fo.IsOpen = !this.FirmArgs_fo.IsOpen;
                        break;
                    case "AddProduct":
                        this.AddPro_fo.IsOpen = !this.AddPro_fo.IsOpen;
                        break;
                    case "AddSeries":
                        this.AddSer_fo.IsOpen = !this.AddSer_fo.IsOpen;
                        break;
                    case "AddFirm":
                        this.AddFirm_fo.IsOpen = !this.AddFirm_fo.IsOpen;
                        break;
                    case "EditFirm":
                        this.EditFirm_fo.IsOpen = !this.EditFirm_fo.IsOpen;
                        break;
                    case "EditSeries":
                        this.EditSeries_fo.IsOpen = !this.EditSeries_fo.IsOpen;
                        break;
                    case "EditProduct":
                        this.EditProduct_fo.IsOpen = !this.EditProduct_fo.IsOpen;
                        break;
                }
            });
            #endregion

            #region MessageDialog
            Messenger.Default.Register<DialogMessage>(this, "showMsg", async msg =>
         {
             if (msg.Button == MessageBoxButton.YesNo)
             {
                 var showTask = this.ShowMessageAsync("提示", msg.Content, MessageDialogStyle.AffirmativeAndNegative);
                 if (await showTask == MessageDialogResult.Affirmative)
                 {
                     msg.Callback.BeginInvoke(MessageBoxResult.Yes, null, null);
                 }
                 else
                 {
                     msg.Callback.BeginInvoke(MessageBoxResult.No, null, null);
                 }
             }
             else
             {
                 var showTask = this.ShowMessageAsync("提示", msg.Content, MessageDialogStyle.Affirmative);
                 await showTask;
                 if (msg.Callback != null)
                 {
                     msg.Callback.BeginInvoke(MessageBoxResult.OK, null, null);
                 }
             }
         });
            #endregion

            #endregion

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new System.TimeSpan(1000);
            timer.Start();
            timer.Tick += timer_Tick;

        }

        private void timer_Tick(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public static int GetMinGongBeiShu(List<int> ints)
        {
            ints.Sort();
            int u = ints[ints.Count - 1] - 1;
            bool isReal = false;
            for (; !isReal; u++)
            {
                isReal = true;
                ints.ForEach(i => isReal = isReal && (u % i == 0));
            }
            return u;
        }



    }
}