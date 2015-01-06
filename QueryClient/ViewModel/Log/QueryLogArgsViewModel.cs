using GalaSoft.MvvmLight;

using QueryClient.LogService;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.Generic;

namespace QueryClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class QueryLogArgsViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the QueryLogArgsViewModel class.
        /// </summary>
        public QueryLogArgsViewModel()
        {
            this._args = new QueryLogQueryArgs();
            this._args.sDate = new System.DateTime(2008, 8, 8);
            this._args.eDate = System.DateTime.Now.AddDays(1d);
            this._args.queryMold = LogService.QueryMold.All;
            this._args.resultMold = QueryResultMold.All;
            Messenger.Default.Register<LogService.QueryLogQueryArgs>(this, "updateQueryLogArgs", msg => this.Args = msg);


        }
        #region 参数
        private QueryLogQueryArgs _args;

        /// <summary>
        /// 查询日志是否自动筛选Keep Line
        /// </summary>
        public const string IsWithKeepLinePropertyName = "IsWithKeepLine";
        private bool _isWithKeepLine = false;
        #endregion

        #region 属性
        public QueryLogQueryArgs Args
        {
            get
            {
                return this._args;
            }
            set
            {
                if (value == this._args)
                {
                    return;
                }
                this._args = value;
                RaisePropertyChanged("Args");
            }
        }

        public string Test
        {
            get
            {
                return "test";
            }
        }

        public bool IsWithKeepLine
        {
            get
            {
                return _isWithKeepLine;
            }

            set
            {
                if (_isWithKeepLine == value)
                {
                    return;
                }

                RaisePropertyChanging(IsWithKeepLinePropertyName);
                _isWithKeepLine = value;
                RaisePropertyChanged(IsWithKeepLinePropertyName);
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

        public ICommand ClearArgs
        {
            get
            {
                return new RelayCommand(ClearArgsExec, CanClearArgs);
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
            Messenger.Default.Send<GenericMessage<LogService.QueryLogQueryArgs>>(
                new GenericMessage<LogService.QueryLogQueryArgs>(this.Args),
                "getQueryLog");
        }


        private bool CanClearArgs()
        {
            return true;
        }

        private void ClearArgsExec()
        {
            this.Args = new QueryLogQueryArgs
            {
                sDate = new System.DateTime(2008, 8, 8),
                eDate = System.DateTime.Now.AddDays(1),
                queryMold = LogService.QueryMold.All,
                resultMold = QueryResultMold.All,
                code = "",
                feature = "",
                remarks = ""
            };
        }

        #endregion
    }
}