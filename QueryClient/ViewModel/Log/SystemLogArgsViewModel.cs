using GalaSoft.MvvmLight;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;
namespace QueryClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SystemLogArgsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the SystemLogArgsMvvmViewModel class.
        /// </summary>
        public SystemLogArgsViewModel()
        {
            this.args = new LogService.SystemLogQueryArgs
            {
                sDate = new System.DateTime(2008, 8, 8),
                eDate = System.DateTime.Now.AddDays(1),
            };
        }

        #region 参数
        private LogService.SystemLogQueryArgs args;
        #endregion
        #region 属性
        public LogService.SystemLogQueryArgs Args
        {
            get
            {
                return this.args;
            }
            set
            {
                if (value == this.args)
                {
                    return;
                }
                this.args = value;
                Messenger.Default.Send<GenericMessage<LogService.SystemLogQueryArgs>>(new GenericMessage<LogService.SystemLogQueryArgs>(this.args), "SystemQueryArgsChanged");
                RaisePropertyChanged("Args");
            }
        }
        #endregion

        #region 命令
        public ICommand Query
        {
            get
            {
                return new RelayCommand(QueryExec, CanQuery);
            }
        }

        public ICommand CleanArgs
        {
            get
            {
                return new RelayCommand(CleanArgsExec, CanCleanArgs);
            }
        }

        #endregion

        #region 方法
        private bool CanQuery()
        {
            return true;
        }

        private void QueryExec()
        {
            Messenger.Default.Send<GenericMessage<LogService.SystemLogQueryArgs>>(new GenericMessage<LogService.SystemLogQueryArgs>(this.Args), "querySystemLog");
        }
        private bool CanCleanArgs()
        {
            return true;
        }

        private void CleanArgsExec()
        {
            this.Args = new LogService.SystemLogQueryArgs
            {
                sDate = new System.DateTime(2008, 8, 8),
                eDate = System.DateTime.Now.AddDays(1),
                info = "",
                remarks = "",
            };

        }
        #endregion
    }
}