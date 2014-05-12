﻿using GalaSoft.MvvmLight;
using System.Collections.Generic;

using GalaSoft.MvvmLight.Messaging;
namespace QueryClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class QueryLogViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the QueryLogViewModel class.
        /// </summary>
        public QueryLogViewModel()
        {
            this._isBusy = false;

            this._logs = new List<LogService.QueryLog>();

        }
        #region 参数
        private bool _isBusy;
        private List<LogService.QueryLog> _logs;
        #endregion
        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }
            set
            {
                if (this._isBusy == value)
                {
                    return;
                }
                this._isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public List<LogService.QueryLog> Logs
        {
            get
            {
                return this._logs;
            }
            set
            {
                if (this._logs == value)
                {
                    return;
                }
                this._logs = value;
                RaisePropertyChanged("Logs");
            }
        }
        #region 属性
        #endregion
        #region 命令
        #endregion
        #region 方法
        private void InitLogs()
        {
            
        }
        #endregion
    }
}