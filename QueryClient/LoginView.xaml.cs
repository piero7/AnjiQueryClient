using System.Windows;

using QueryClient.Model;
using MahApps;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;

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
            //开始检查服务器连接
            Messenger.Default.Register<NotificationMessageWithCallback>(this, "testConn", (msg) =>
                    DispatcherHelper.CheckBeginInvokeOnUI(new System.Action(() =>
                    {
                        UIHelper.DoEvents();
                        new System.Action(() => msg.Execute()).BeginInvoke(null, null);
                    }))
                );

            //连接失败
            Messenger.Default.Register<GenericMessage<string>>(this, "connFail", async (msg) =>
            {
                await ShowMessageAsync("连接错误", "无法连接服务器，请检查网络连接之后联系管理员。");
                this.CloseApplication();
            });

            //连接成功
            Messenger.Default.Register<GenericMessage<string>>(this, "connSuccess", (msg) => { ;});

            //关闭程序
            Messenger.Default.Register<GenericMessage<string>>(this, "close", (msg) => this.CloseApplication());

            //开始登陆
            Messenger.Default.Register<NotificationMessageWithCallback>(this, "start", msg =>
            {
                UIHelper.DoEvents();
                new System.Action(() => msg.Execute()).BeginInvoke(null, null);
            });

            //用户名错误
            Messenger.Default.Register<GenericMessage<string>>(this, "errorUserName",
                (msg) => DispatcherHelper.CheckBeginInvokeOnUI(new System.Action(
                    () => ErrorName(msg.Content))
                    ));

            //密码错误
            Messenger.Default.Register<GenericMessage<string>>(this, "errorPwd",
                (msg) => this.ErrorPwd(msg.Content));

            //登录成功
            Messenger.Default.Register<GenericMessage<SystemMenagerService.LoginUser>>(this, "success",
                (msg) => DispatcherHelper.CheckBeginInvokeOnUI(
                    new System.Action(
                       async () =>
                       {
                           await ShowMessageAsync("登录成功", "欢迎您 " + msg.Content.RealName);
                           Messenger.Default.Send<GenericMessage<SystemMenagerService.LoginUser>>(
    new GenericMessage<SystemMenagerService.LoginUser>(msg.Content), "loginSuccess");
                           this.Close();
                       })
                        ));

            //系统错误
            Messenger.Default.Register<GenericMessage<string>>(this, "systemError",
                (msg) => DispatcherHelper.CheckBeginInvokeOnUI(new System.Action(async () => await ShowMessageAsync("系统错误", msg.Content))));

            //登录失败
            Messenger.Default.Register<GenericMessage<string>>(this, "errorUser",
                (msg) => DispatcherHelper.CheckBeginInvokeOnUI(
                    new System.Action(() =>
                {
                    this.ErrorName("");
                    this.ErrorPwd(msg.Content);
                })));
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

            //验证连接
            this.Loaded += new System.Windows.RoutedEventHandler((o, args) =>
            {
                new System.Action(() => this.vm.TestConnWithChangeStatus()).BeginInvoke(null, null);
            });
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