using System.Windows;
using GalaSoft.MvvmLight.Messaging;

namespace QueryClient
{
    /// <summary>
    /// Description for ConfigView.
    /// </summary>
    public partial class ConfigView : MahApps.Metro.Controls.MetroWindow
    {
        ViewModel.ConfigViewModel VM = new ViewModel.ConfigViewModel();

        public ConfigView()
        {
            InitializeComponent();
            this.DataContext = VM;

            Messenger.Default.Register<GenericMessage<string>>(this, "ConfigView", msg =>
            {
                if (msg.Content == "Close")
                {
                    GalaSoft.MvvmLight.Threading.DispatcherHelper.CheckBeginInvokeOnUI(new System.Action(() => {
                        this.Close();
                    }));
                }
            });
        }

    }

    class IntRule : System.Windows.Controls.ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int ret = 0;
            if (!int.TryParse((string)value, out ret))
            {
                return new System.Windows.Controls.ValidationResult(false, "请输入数字");
            }
            return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}