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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using GalaSoft.MvvmLight.Messaging;

namespace QueryClient
{
    /// <summary>
    /// ProductInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ProductInfo : UserControl
    {
        private QueryClient.ViewModel.InfoViewModel vm = new ViewModel.InfoViewModel();

        public ProductInfo()
        {
            InitializeComponent();
            this.DataContext = vm;
            #region Chage Tab
            Messenger.Default.Register<GenericMessage<string>>(this, "ChangeInfoTab", msg =>
            {
                switch (msg.Content)
                {
                    case "Product":
                        this.product_tab.IsSelected = true;
                        break;
                    case "Series":
                        this.series_tab.IsSelected = true;
                        break;
                    case "Firm":
                        this.firm_tab.IsSelected = true;
                        break;
                }
            });
            #endregion
        }


        #region init
        private void InitProduct(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("init"), "initProduct");
        }

        private void InitSeries(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("init"), "initSeries");
        }

        private void InitFirm(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("init"), "initFirm");
        }
        #endregion



    }
}
