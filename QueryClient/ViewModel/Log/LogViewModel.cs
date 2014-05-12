using GalaSoft.MvvmLight;
using System;

using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;
using QueryClient.LogService;
using System.Collections.Generic;

namespace QueryClient.ViewModel
{
    public class LogViewModel : ViewModelBase
    {
        private int count = 0;
        /// <summary>
        /// 日志空间的viewmodel
        /// </summary>
        public LogViewModel()
        {

            //this.SystemLogForShow = new List<SystemLogForShow>{new SystemLogForShow
            //{
            //    e = "1",
            //    w = LogLevel.Debug,
            //    q = "12",
            //    r = 11
            //}};
            //this.QueryLogForShow = new List<QueryLogForShow>{new QueryLogForShow
            //{
            //    备注 = "123123",
            //    查询渠道 = QueryMold.Debug,
            //    查询时间 = DateTime.Now,
            //    防伪码 = "code",
            //    结果 = QueryResultMold.被拦截,
            //    特征 = "ffff"
            //}};
            this.count = int.Parse(System.Configuration.ConfigurationManager.AppSettings["logCount"]);
            this.queryLogArgs = new QueryLogQueryArgs
            {
                sDate = new System.DateTime(2008, 8, 8),
                eDate = System.DateTime.Now.AddDays(1)
            };

            Messenger.Default.Register<GenericMessage<string>>(this, "initLog", msg =>
            {
                if (msg.Content == "systemLog")
                {
                    this.systemLogArgs = new SystemLogQueryArgs { sDate = new DateTime(2008, 8, 8), eDate = DateTime.Now.AddDays(1) };
                    QuerySyslogExec();
                    return;
                }
                if (msg.Content == "queryLog")
                {
                    this.queryLogArgs = new QueryLogQueryArgs { sDate = new DateTime(2008, 8, 8), eDate = DateTime.Now.AddDays(1) };
                    QueryLogExec();
                    return;
                }
            });

            Messenger.Default.Register<GenericMessage<LogService.SystemLogQueryArgs>>(this, "querySystemLog", msg =>
            {
                this.SystemLogArgs = msg.Content;
                QuerySyslogExec();
            });

            Messenger.Default.Register<GenericMessage<LogService.QueryLogQueryArgs>>(this, "getQueryLog", msg =>
            {
                this.QueryLogArgs = msg.Content;
                QueryLogExec();
            });


            Messenger.Default.Register<GenericMessage<LogService.SystemLogQueryArgs>>(this, "SystemQueryArgsChanged", msg =>
            {
                this.SystemLogArgs = msg.Content;
            });
            Messenger.Default.Register<GenericMessage<LogService.QueryLogQueryArgs>>(this, "QueryArgsChanged", msg =>
            {
                this.QueryLogArgs = msg.Content;
            });

        }
        #region 参数
        /// <summary>
        /// 系统日志list
        /// </summary>
        private List<SystemLogInfo> systemLogList = new List<SystemLogInfo>();
        /// <summary>
        /// 系统日志查询参数
        /// </summary>
        private SystemLogQueryArgs systemLogArgs = new SystemLogQueryArgs();

        /// <summary>
        /// 查询日志list
        /// </summary>
        private List<QueryLog> queryLogList = new List<QueryLog>();
        /// <summary>
        /// Query日志查询参数
        /// </summary>
        private QueryLogQueryArgs queryLogArgs = new QueryLogQueryArgs();


        #endregion

        #region 属性
        public List<SystemLogInfo> SystemLogList
        {
            get
            {
                return this.systemLogList;
            }
            set
            {
                if (this.systemLogList == value)
                {
                    return;
                }
                this.systemLogList = value;
                //this.systemLogForShow = this.GetSystemLogForShow();
                RaisePropertyChanged("SystemLogList");
            }
        }

        public SystemLogQueryArgs SystemLogArgs
        {
            get
            {
                return this.systemLogArgs;
            }
            set
            {
                if (value == this.systemLogArgs)
                {
                    return;
                }
                this.systemLogArgs = value;
                RaisePropertyChanged("SystemLogArgs");
                //Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("changeSystemArgs"), this.systemLogArgs);
                //TODO Rigest the messenger in args flyout.
            }
        }


        public List<QueryLog> QueryLogList
        {
            get
            {
                return this.queryLogList;
            }
            set
            {
                if (this.queryLogList == value)
                {
                    return;
                }
                this.queryLogList = value;
                //this.QueryLogForShow = GetQueryLogForShow();
                RaisePropertyChanged("QueryLogList");
            }
        }

        public QueryLogQueryArgs QueryLogArgs
        {
            get
            {
                return this.queryLogArgs;
            }
            set
            {
                if (value == this.queryLogArgs)
                {
                    return;
                }
                this.queryLogArgs = value;
                RaisePropertyChanged("QueryLogArgs");
                //Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("changeQueryArgs"), this.queryLogArgs);
                //x TODO Rigest the messenger in args flyout.
            }
        }


        #endregion

        #region 命令
        /// <summary>
        /// 查询系统日志
        /// </summary>
        public ICommand QuerySystemLog
        {
            get
            {
                return new RelayCommand(QuerySyslogExec, CanQuerySysLog);
            }
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        public ICommand QueryQueryLog
        {
            get
            {
                return new RelayCommand(QueryLogExec, CanQueryLog);
            }
        }

        /// <summary>
        /// 清空系统日志参数
        /// </summary>
        public ICommand ClearSystemLogArgs
        {
            get
            {
                return new RelayCommand(ClearSysLogArgsExec, CanClearSysLogArgs);
            }
        }

        /// <summary>
        /// 清空查询日志参数
        /// </summary>
        public ICommand ClearQueryLogArgs
        {
            get
            {
                return new RelayCommand(ClearQueryLogExec, CanClearQueryLogArgs);
            }
        }

        #endregion

        #region 方法
        private bool CanQuerySysLog()
        {
            return true;
        }

        private async void QuerySyslogExec()
        {
            LogService.LogServiceClient lc = new LogServiceClient();
            this.count = int.Parse(System.Configuration.ConfigurationManager.AppSettings["logCount"]);
            try
            {
                lc.Open();
                //TODO Set the method async, set UI waiting.
                var queryTask = lc.SystemLogQueryTakeAsync(this.SystemLogArgs, 0, count);
                this.SystemLogList = (await queryTask).OrderByDescending(l => l.OptionDate).ToList();
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(ex.Message), "querySysLogError");
            }
            finally
            {
                lc.Close();
            }
            //TODO Set UI to be free.
        }

        private bool CanQueryLog()
        {
            return true;
        }

        private async void QueryLogExec()
        {
            LogService.LogServiceClient lc = new LogServiceClient();
            this.count = int.Parse(System.Configuration.ConfigurationManager.AppSettings["logCount"]);
            try
            {
                lc.Open();
                //TODO Set the method async, set UI waiting.
                var queryTask = lc.QueryLogQueryTakeAsync(this.QueryLogArgs, 0, count);
                this.QueryLogList = (await queryTask).OrderByDescending(l => l.OptionDate).ToList();
            }
            catch (Exception ex)
            {
                Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>(ex.Message), "querySysLogError");
            }
            finally
            {
                lc.Close();
            }
            //TODO Set UI to be free.
        }

        private bool CanClearQueryLogArgs()
        {
            return true;
        }

        private void ClearQueryLogExec()
        {
            this.QueryLogArgs = new QueryLogQueryArgs
            {
                sDate = new System.DateTime(2008, 8, 8),
                eDate = System.DateTime.Now.AddDays(1)
            };
            QueryLogExec();
        }

        private bool CanClearSysLogArgs()
        {
            return true;
        }

        private void ClearSysLogArgsExec()
        {
            this.SystemLogArgs = new SystemLogQueryArgs
            {
                sDate = new DateTime(2008, 8, 8),
                eDate = DateTime.Now.AddDays(1)
            };
            this.QuerySyslogExec();
        }

        #endregion
    }

}