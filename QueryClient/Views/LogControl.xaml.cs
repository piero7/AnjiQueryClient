using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QueryClient
{
    /// <summary>
    /// LogControl.xaml 的交互逻辑
    /// </summary>
    public partial class LogControl : UserControl
    {
        private ViewModel.LogViewModel vm = new ViewModel.LogViewModel();

        public LogControl()
        {
            InitializeComponent();
            this.DataContext = vm;

        }

        /// <summary>
        /// 弹出查询日志筛选参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowQueryArgsForm(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("QueryArgs"), "flyOut");
            //Messenger.Default.Send<GenericMessage<LogService.QueryLogQueryArgs>>(
            //    new GenericMessage<LogService.QueryLogQueryArgs>(this.vm.QueryLogArgs), "updateQueryLogArgs");
        }

        private void ShowSystemArgsForm(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("SystemArgs"), "flyOut");
            //Messenger.Default.Send<GenericMessage<LogService.SystemLogQueryArgs>>(
            //    new GenericMessage<LogService.SystemLogQueryArgs>(vm.SystemLogArgs), "updateSystemLogArgs");
            //x TODO Resist the messenger in main form.
        }

        private void InitQueryLogs(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("InitSystemLogs");
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("queryLog"), "initLog");
        }
        private void InitSystemLogs(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("InitSystemLogs");
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("systemLog"), "initLog");
        }
    }
}
