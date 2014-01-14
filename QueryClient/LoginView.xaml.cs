﻿using System.Windows;

using QueryClient.Model;
using MahApps;
using GalaSoft.MvvmLight.Messaging;
namespace QueryClient
{
    /// <summary>
    /// Description for LoginView.
    /// </summary>
    public partial class LoginView : MahApps.Metro.Controls.MetroWindow
    {
        private QueryClient.ViewModel.LoginViewModel vm = new QueryClient.ViewModel.LoginViewModel();

        /// <summary>
        /// Initializes a new instance of the LoginView class.
        /// </summary>
        public LoginView()
        {
            InitializeComponent();

            this.DataContext = vm;

            #region 消息注册
            //关闭程序
            Messenger.Default.Register<GenericMessage<string>>(this, "close", (msg) => this.CloseApplication());

            //开始登陆
            Messenger.Default.Register<NotificationMessageWithCallback>(this,"start", new System.Action<NotificationMessageWithCallback>(msg =>
            {
                Dispatcher.InvokeAsync(new System.Action(()=>msg.Execute()));
            }));

            Messenger.Default.Register<GenericMessage<string>>(this, "errorUserName",
                (msg) => this.ErrorName(msg.Content));

            Messenger.Default.Register<GenericMessage<string>>(this, "errorPwd",
                (msg) => this.ErrorPwd(msg.Content));

            Messenger.Default.Register<GenericMessage<SystemMenagerService.LoginUser>>(this, "success",
                (msg) => ShowMessageAsync("登录成功", "欢迎您 " + msg.Content.RealName));

            Messenger.Default.Register<GenericMessage<string>>(this, "systemError",
                (msg) => ShowMessageAsync("系统错误", msg.Content));

            Messenger.Default.Register<GenericMessage<string>>(this, "errorUser",
                (msg) =>
                {
                    this.ErrorName("");
                    this.ErrorPwd(msg.Content);
                });
            #endregion

            #region Event
            //密码纠正
            this.password_tb.TextChanged += new System.Windows.Controls.TextChangedEventHandler((o, args) =>
            {

                MahApps.Metro.Controls.TextboxHelper.SetIsWaitingForData(this.password_tb, false);
                this.info_lb.Content = "";
            });
            //用户名纠正
            this.userName_tb.TextChanged += new System.Windows.Controls.TextChangedEventHandler((o, args) =>
            {

                MahApps.Metro.Controls.TextboxHelper.SetIsWaitingForData(this.userName_tb, false);
                this.info_lb.Content = "";
            });
            //this.login_btn.PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(async (o, args) =>
            //{
            //    this.SetRunning();
            //    await this.Dispatcher.InvokeAsync(new System.Action(() => { this.vm.LonginExec(); }));
            //    //this.vm.LonginExec();
            //    this.SetRunning(false);
            //});
            #endregion
        }

        #region 方法
        private void CloseForm()
        {
            this.Close();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        private void ErrorName(string info)
        {
            MahApps.Metro.Controls.TextboxHelper.SetIsWaitingForData(this.userName_tb, true);
            this.info_lb.Content = info;
        }

        private void ErrorPwd(string info)
        {
            MahApps.Metro.Controls.TextboxHelper.SetIsWaitingForData(this.password_tb, true);
            this.info_lb.Content = info;
        }


        private void SetRunning(bool isrunning)
        {
            if (isrunning)
            {
                this.isRunning_pb.Visibility = Visibility.Visible;
                this.main_gd.IsEnabled = false;

            }
            else
            {
                this.isRunning_pb.Visibility = Visibility.Hidden;
                this.main_gd.IsEnabled = true;
            }
        }
        private void SetRunning()
        {
            SetRunning(true);
        }
        #endregion
    }
}