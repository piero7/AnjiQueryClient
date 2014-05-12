using GalaSoft.MvvmLight;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Input;

namespace QueryClient.ViewModel
{

    public class ChagePasswordViewModel : ViewModelBase
    {
        public const string PasswordPropertyName = "Password";
        private string _password;

        public const string NewPasswordPropertyName = "NewPassword";
        private string _newPassword;

        public const string NewPasswordAgPropertyName = "NewPasswordAg";
        private string _newPasswordAg;

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private string _title = "修改密码";

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title == value)
                {
                    return;
                }

                RaisePropertyChanging(TitlePropertyName);
                _title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        public string NewPasswordAg
        {
            get
            {
                return _newPasswordAg;
            }

            set
            {
                if (_newPasswordAg == value)
                {
                    return;
                }

                RaisePropertyChanging(NewPasswordAgPropertyName);
                _newPasswordAg = value;
                RaisePropertyChanged(NewPasswordAgPropertyName);
            }
        }


        public string NewPassword
        {
            get
            {
                return _newPassword;
            }

            set
            {
                if (_newPassword == value)
                {
                    return;
                }

                RaisePropertyChanging(NewPasswordPropertyName);
                _newPassword = value;
                RaisePropertyChanged(NewPasswordPropertyName);
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                if (_password == value)
                {
                    return;
                }

                RaisePropertyChanging(PasswordPropertyName);
                _password = value;
                RaisePropertyChanged(PasswordPropertyName);
            }
        }

        public ICommand Update
        {
            get
            {
                return new RelayCommand(UpdateExec, CanUpdate);
            }
        }

        private bool CanUpdate()
        {
            if (this.NewPassword == this.NewPasswordAg)
            {
                return true;
            }
            return false;
        }

        private void UpdateExec()
        {
            SystemMenagerService.SystemManagerServiceClient client = new SystemMenagerService.SystemManagerServiceClient();
            try
            {
                client.Open();
                var ret = client.ChangePwd(MainViewModel.NowUser.id,
                    this.Password == null ? null : this.Password.GetHashCode().ToString(),
                    this.NewPasswordAg == null ? null : this.NewPasswordAg.GetHashCode().ToString());
                if (ret == 1|| ret ==0)
                {
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("Success"), "ChangePasswordMsg");
                    return;
                }
                else if (ret == -1)
                {
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("密码输入错误"), "ChangePasswordMsg");
                }
                else
                    Messenger.Default.Send<GenericMessage<string>>(new GenericMessage<string>("密码修改失败"), "ChangePasswordMsg");
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send<GenericMessage<System.Exception>>(new GenericMessage<System.Exception>(ex), "ChangePasswordError");
            }
            finally
            {
                client.Close();
            }
        }

        public ChagePasswordViewModel()
        {
        }
    }
}