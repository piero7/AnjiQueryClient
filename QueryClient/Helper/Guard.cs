using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryClient.SystemMenagerService;

using GalaSoft.MvvmLight.Messaging;

namespace QueryClient
{
    class Guard
    {
        //private List<LogInfo> LastHundredLogs = new List<LogInfo>();
        //private List<LogInfo> TodayLogs = new List<LogInfo>();
        //private int nowId = 0;
        //private List<BlackNote> BlaclNoteList = new List<BlackNote>();

        delegate bool keepLineHandle();

        public bool KeepLine()
        {
            return false;

            double a = 1.1;
            int b = 123;
            a = b;

        }

        private bool CheckWeb()
        {
            var clinet = new WebServiceReference.WebServiceClient();

            try
            {
                clinet.Open();

                var ret = clinet.Query("Keep Line", "38657844541667");
                if (ret.InfoList.Count() != 1 || ret.InfoList[0].Info != "u")
                {
                    return false;
                }
                clinet.Close();
            }
            catch (Exception ex)
            {
                ShowErrMsg("checking web", ex);
                return false;
            }
            finally
            {
                clinet.Abort();
            }
            return true;
        }

        private bool CheckPhone()
        {
            var client = new PhoneServiceReference.PhoneServiceClient();
            try
            {

                client.Open();
                var ret = client.Query("38657844541667", "Keep Line");
                if (ret != "code:j")
                {
                    return false;
                }

                client.Close();
            }
            catch (Exception ex)
            {
                ShowErrMsg("checking phone", ex);
                return false;
            }
            finally
            {
                client.Abort();
            }
            return true;
        }

        private bool CheckWeixin()
        {
            string postString = string.Format("code={0}&feature={1}", "12345678901234", "Keep Line");
            string url = "http://10.0.0.7:7770/WeixinService.asmx/Query";

            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url + "?" + postString);
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Credentials = System.Net.CredentialCache.DefaultCredentials;
            try
            {
                var str = req.GetRequestStream();

                System.IO.StreamReader sr = new System.IO.StreamReader(str, Encoding.UTF8);
                string ret = sr.ReadToEnd();
                string retstring = ret.Remove(0, ret.IndexOf("http://anjismart.com/\">")).Replace("</string>", "").Replace("http://anjismart.com/\">", "");
                if (retstring != "对不起！您所查验的号码，未在本平台注册。")
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ShowErrMsg("checking weixin", ex);
                return false;
            }
            finally
            {
                req.Abort();
            }
            return true;
        }

        public static void ShowErrMsg(string option, string msg)
        {
            Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Format("An error was happend on {0}:\r\n{1}", option, msg), null));
            //TODO: show in form
        }
        public static void ShowErrMsg(string option, Exception ex)
        {
            ShowErrMsg(option, ex.Message);
        }
    }
}
