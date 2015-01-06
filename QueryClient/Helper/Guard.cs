using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryClient.SystemMenagerService;

using GalaSoft.MvvmLight.Messaging;
using System.Net;

namespace QueryClient
{
    class Guard
    {
        //private List<LogInfo> LastHundredLogs = new List<LogInfo>();
        //private List<LogInfo> TodayLogs = new List<LogInfo>();
        //private int nowId = 0;
        //private List<BlackNote> BlaclNoteList = new List<BlackNote>();

        delegate bool keepLineHandle();

        private int _maxErrorCount = 0;
        private int _weixinErrorCount = 0;
        private int _phoneErrorCount = 0;
        private int _smErrorCount = 0;
        private int _webErrorCount = 0;
        private int _oldCodeErrorCount = 0;
        private int _4d4cErrorCount = 0;

        /// <summary>
        /// 网页错误字典
        /// key：网址
        /// value：错误列表及信息
        /// </summary>
        private Dictionary<string, WebError> WebErrorList;

        public Guard()
        {
            this._maxErrorCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["MaxErrorCount"]);
        }

        public Task<bool> CheckSm()
        {
            var code = System.Configuration.ConfigurationManager.AppSettings["code"];
            var path = System.Configuration.ConfigurationManager.AppSettings["smPath"];

            return Task.Run<bool>(() =>
            {
                string postString = string.Format("<?xml version=\"1.0\" encoding=\"gb2312\"?><SmsMessage><MessageID></MessageID><Mobile>{1}</Mobile><Message>{0}</Message><MobaddrName></MobaddrName><MobaddrCode></MobaddrCode><ReceiveDate></ReceiveDate><ServiceRequireType></ServiceRequireType><ServiceRequireinfo></ServiceRequireinfo><SmsServiceCode></SmsServiceCode><SmsSubServiceCode></SmsSubServiceCode><Telcom></Telcom><Protocolname></Protocolname><MobileAttachId></MobileAttachId><MobileAttach></MobileAttach><LinkID></LinkID></SmsMessage>", code, "Keep Line");

                byte[] parms = Encoding.GetEncoding("GB2312").GetBytes(postString);
                System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(path);
                req.ContentLength = parms.Length;
                req.Method = "POST";

                req.ContentType = "application/x-www-form-urlencoded";
                req.Credentials = System.Net.CredentialCache.DefaultCredentials;
                using (System.IO.Stream s = req.GetRequestStream())
                {
                    s.Write(parms, 0, parms.Length);
                    s.Close();
                }

                try
                {
                    var retMsg = System.Configuration.ConfigurationManager.AppSettings["smReturnMsg"];

                    var reqS = req.GetResponse() as System.Net.HttpWebResponse;
                    var str = reqS.GetResponseStream();

                    System.IO.StreamReader sr = new System.IO.StreamReader(str, Encoding.GetEncoding("GB2312"));
                    string ret = sr.ReadToEnd();
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    doc.LoadXml(ret);
                    var retstring = doc.SelectSingleNode("/SmsMessage/Message").InnerText;

                    // string retstring = ret.Remove(0, ret.IndexOf("http://anjismart.com/\">")).Replace("</string>", "").Replace("http://anjismart.com/\">", "");
                    if (retstring != retMsg)
                    {
                        ShowErrMsg("checking smsg", "Error return !\r\n" + retstring);
                        this._smErrorCount++;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    ShowErrMsg("checking weixin", ex);
                    Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(false), "CheckWeixinSatus");
                    this._smErrorCount++;
                    return false;
                }
                finally
                {
                    if (this._smErrorCount >= _maxErrorCount)
                    {
                        SendWarningMessage("防伪码查询平台异常报告：请检查短信接口状态！", SendMold.ShotMessage);
                    }
                    req.Abort();
                }
                Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true), "CheckWeixinSatus");
                this._smErrorCount = 0;

                return true;
            });
        }

        public Task<bool> CheckWeb()
        {
            return Task.Run<bool>(() =>
             {
                 var clinet = new WebServiceReference.WebServiceClient();
                 var retCount = int.Parse(System.Configuration.ConfigurationManager.AppSettings["webReturnCount"]);
                 var code = System.Configuration.ConfigurationManager.AppSettings["code"];
                 try
                 {
                     clinet.Open();

                     var ret = clinet.Query("Keep Line", code);
                     if (ret.InfoList.Count() != retCount || ret.InfoList[0].Info != "j")
                     {
                         ShowErrMsg("checking web", "Error return !\r\n" + ret.InfoList.Count());
                         Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(false), "CheckWebSatus");
                         this._webErrorCount++;
                         return false;
                     }
                     clinet.Close();
                 }
                 catch (Exception ex)
                 {
                     ShowErrMsg("checking web", ex);
                     Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(false), "CheckWebSatus");
                     this._webErrorCount++;
                     return false;
                 }
                 finally
                 {
                     clinet.Abort();
                     if (this._smErrorCount >= _maxErrorCount)
                     {
                         SendWarningMessage("防伪码查询平台异常报告：请检查网页接口状态！", SendMold.ShotMessage);
                     }
                 }
                 Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true), "CheckWebSatus");
                 this._webErrorCount = 0;
                 return true;
             });
        }

        public Task<bool> CheckPhone()
        {
            var client = new PhoneServiceReference.PhoneServiceClient();
            var retMsg = System.Configuration.ConfigurationManager.AppSettings["phoneReturnMsg"];
            var code = System.Configuration.ConfigurationManager.AppSettings["code"];
            return Task.Run<bool>(() =>
            {
                try
                {

                    client.Open();
                    var ret = client.Query(code, "Keep Line");
                    if (ret != retMsg)
                    {
                        ShowErrMsg("checking phone", "Error return !\r\n" + ret);
                        Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(false), "CheckPhoneSatus");
                        this._phoneErrorCount++;
                        return false;
                    }

                    client.Close();
                }
                catch (Exception ex)
                {
                    ShowErrMsg("checking phone", ex);
                    Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(false), "CheckPhoneSatus");
                    this._phoneErrorCount++;
                    return false;
                }
                finally
                {
                    if (this._smErrorCount >= _maxErrorCount)
                    {
                        SendWarningMessage("防伪码查询平台异常报告：请检查电话接口状态！", SendMold.ShotMessage);
                    }
                    client.Abort();
                }
                Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true), "CheckPhoneSatus");
                this._phoneErrorCount = 0;
                return true;
            });
        }

        public Task<bool> CheckWeixin()
        {
            var code = System.Configuration.ConfigurationManager.AppSettings["code"];


            string postString = string.Format("code={0}&feature={1}", "11111111111111", "Keep Line");
            string url = GetWeixinServiceAddress();

            return Task.Run<bool>(() =>
             {
                 System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url + "?" + postString);
                 req.Method = "GET";
                 req.ContentType = "application/x-www-form-urlencoded";
                 req.Credentials = System.Net.CredentialCache.DefaultCredentials;
                 try
                 {
                     var retMsg = System.Configuration.ConfigurationManager.AppSettings["weixinReturnMsg"];

                     var reqS = req.GetResponse() as System.Net.HttpWebResponse;
                     var str = reqS.GetResponseStream();

                     System.IO.StreamReader sr = new System.IO.StreamReader(str, Encoding.UTF8);
                     string ret = sr.ReadToEnd();
                     string retstring = ret.Remove(0, ret.IndexOf("http://anjismart.com/\">")).Replace("</string>", "").Replace("http://anjismart.com/\">", "");
                     if (retstring != retMsg)
                     {
                         ShowErrMsg("checking weixin", "Error return !\r\n" + retstring);
                         Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(false), "CheckWeixinSatus");
                         this._weixinErrorCount++;
                         return false;
                     }
                 }
                 catch (Exception ex)
                 {
                     ShowErrMsg("checking weixin", ex);
                     Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(false), "CheckWeixinSatus");
                     this._weixinErrorCount++;
                     return false;
                 }
                 finally
                 {
                     req.Abort();
                     if (this._smErrorCount >= _maxErrorCount)
                     {
                         SendWarningMessage("防伪码查询平台异常报告：请检查微信接口状态！", SendMold.ShotMessage);
                     }
                 }
                 Messenger.Default.Send<GenericMessage<bool>>(new GenericMessage<bool>(true), "CheckWeixinSatus");
                 this._webErrorCount = 0;
                 return true;
             });
        }

        public Task<bool> CheckOldCode()
        {
            var code = System.Configuration.ConfigurationManager.AppSettings["oldCode"];
            var url = System.Configuration.ConfigurationManager.AppSettings["oldCodeAddress"];
            var retMsg = System.Configuration.ConfigurationManager.AppSettings["oldCodeReturnString"];


            return Task.Run<bool>(() =>
             {
                 WebClient webClient = new WebClient();

                 try
                 {
                     string postString = "fwm=" + code;//这里即为传递的参数，可以用工具抓包分析，也可以自己分析，主要是form里面每一个name都要加进来  
                     byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式  
                     webClient.Dispose();
                     webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  

                     byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
                     string srcString = Encoding.Default.GetString(responseData);//解码
                     srcString = srcString.Remove(srcString.IndexOf("& -->"));
                     srcString = srcString.Replace("<!-- response:", "");
                     if (srcString == retMsg)
                     {
                         this._oldCodeErrorCount = 0;
                         return true;
                     }
                     else
                     {
                         ShowErrMsg("checkong old code", srcString);
                         this._oldCodeErrorCount++;
                         return false;
                     }
                 }
                 catch (Exception ex)
                 {
                     ShowErrMsg("checking old code", ex);
                     this._oldCodeErrorCount++;
                     return false;
                 }
                 finally
                 {
                     webClient.Dispose();
                     if (this._smErrorCount >= _maxErrorCount)
                     {
                         SendWarningMessage("防伪码查询平台异常报告：请检查15位码接口状态！", SendMold.ShotMessage);
                     }
                 }
             });
        }

        public Task<bool> Check4d4c()
        {
            WebClient webClient = new WebClient();
            string code = System.Configuration.ConfigurationManager.AppSettings["colorfulCode"];
            string url = System.Configuration.ConfigurationManager.AppSettings["colorfulCodeAddress"];
            string retString = code + "：" + System.Configuration.ConfigurationManager.AppSettings["colorfulCodeReturnString"];


            return Task.Run<bool>(() =>
             {
                 try
                 {
                     string postString = "f=" + code;//这里即为传递的参数，可以用工具抓包分析，也可以自己分析，主要是form里面每一个name都要加进来  
                     byte[] postData = Encoding.UTF8.GetBytes(postString);//编码，尤其是汉字，事先要看下抓取网页的编码方式   
                     // string url = "http://2d.4d4c.com/s.php";
                     webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的话就去掉这句话即可  

                     byte[] responseData = webClient.UploadData(url, "POST", postData);//得到返回字符流  
                     string srcString = Encoding.Default.GetString(responseData);//解码
                     string[] strs = new string[2];
                     var t = new HtmlParser(srcString); //  
                     // t.KeepTag(new string[] { "img" }); //设置br标签不过虑  
                     srcString = t.Text();
                     if (srcString.Contains(retString))
                     {
                         this._4d4cErrorCount = 0;
                         return true;
                     }
                     else
                     {
                         ShowErrMsg("checking 4d4c code", srcString);
                         this._4d4cErrorCount++;
                         return false;
                     }


                     //strs[0] = temp3.Split(new char[] { '<' })[0];//返回信息
                     //strs[1] = "http://2d.4d4c.com/" + temp3.Split(new char[] { '\'' })[1];//图片地址

                     //return strs == 
                 }
                 catch (Exception ex)
                 {
                     ShowErrMsg("checking 4d4c code ", ex);
                     this._4d4cErrorCount++;
                     return false;
                 }
                 finally
                 {
                     webClient.Dispose();
                     if (this._smErrorCount >= _maxErrorCount)
                     {
                         SendWarningMessage("防伪码查询平台异常报告：请检查4d4c接口状态！", SendMold.ShotMessage);
                     }
                 }
             });


        }

        /// <summary>
        /// 读取微信中的地址
        /// </summary>
        /// <returns></returns>
        private string GetWeixinServiceAddress()
        {
            return System.Configuration.ConfigurationManager.AppSettings["weixinServiceAddress"];
        }

        async public static void ShowErrMsg(string option, string msg)
        {
            Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Format("An error was happend on {0}:\r\n{1}", option, msg), null), "showMsg");
            LogService.LogServiceClient client = new LogService.LogServiceClient();
            try
            {
                client.Open();
                var insertTask = client.InsertSystemLogAsync(new LogService.SystemLog
                {
                    Level = LogService.LogLevel.Error,
                    OptionDate = DateTime.Now,
                    Info = string.Format("Operation:{0} Error:{1}", option, msg)
                });
                await insertTask;
                client.Close();
            }
            finally
            {
                client.Abort();
            }

        }
        public static void ShowErrMsg(string option, Exception ex)
        {
            ShowErrMsg(option, ex.Message);
        }


        public void SendWarningMessage(string msg, SendMold mold)
        {
            //throw new NotImplementedException();
        }


        async public void CheckWebAlive()
        {
            var congfigString = System.Configuration.ConfigurationManager.AppSettings["CheckWeb"];
            var configArr = congfigString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            List<CheckWebInfo> InfoList = new List<CheckWebInfo>();
            foreach (var con in configArr)
            {
                var tmpArr = con.Split(new char[] { '^' });
                if (tmpArr.Length > 1)
                {
                    InfoList.Add(new CheckWebInfo
                    {
                        keyWord = tmpArr[1],
                        Url = tmpArr[0]
                    });
                }
            }

            //检测网页中是否存在关键字
            foreach (var info in InfoList)
            {
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(info.Url);
                req.Method = "Get";
                req.Timeout = 4000;

                var resTask = req.GetResponseAsync();

                try
                {
                    var res = (HttpWebResponse)await resTask;
                }
                catch (Exception ex)
                {
                    SendWebCheckError(ex, info.Url, info.Url);
                }
                finally
                {
                    req.Abort();
                }


            }
        }

        private void SendWebCheckError(Exception ex, string url, string keyword)
        {
            if (this.WebErrorList.Keys.Contains(url))
            {
                WebErrorList[url].ErrorDic.Add(DateTime.Now, ex);
                //是否是24小时之内的
                if (DateTime.Now.Subtract(WebErrorList[url].OccurDate).TotalHours > 24)
                {
                    ShowErrMsg("Check web\r\n" + url, ex);
                    SendWarningMessage(string.Format("平台网页异常： {0} 无法检测到指定关键字，请尽快解决。", url), SendMold.ShotMessage);
                }
            }
            else
            {
                WebErrorList.Add(url, new WebError
                {
                    ErrorDic = new Dictionary<DateTime, Exception>(),
                    keyword = keyword,
                    OccurDate = DateTime.Now,
                    url = url
                });
                WebErrorList[url].ErrorDic.Add(DateTime.Now, ex);
                ShowErrMsg("Check web\r\n" + url, ex);
                SendWarningMessage(string.Format("平台网页异常： {0} 无法检测到指定关键字，请尽快解决。", url), SendMold.ShotMessage);
            }
        }
    }
    public enum SendMold
    {
        Email,
        ShotMessage
    }

    public struct CheckWebInfo
    {
        public string Url;
        public string keyWord;
    }

    public class NotFindKeywordInWebException : Exception
    {
        public string Url;
        public string Keyword;

        NotFindKeywordInWebException(string url, string keyword)
            : base()
        {

        }

        public override string Message
        {
            get
            {
                string msg = "";
                msg = string.Format("未能在 {0} 中找到既定关键字 {1}。请注意检查。");
                return msg;
            }
        }
    }

    public struct WebError
    {
        public string url;
        public DateTime OccurDate;
        public string keyword;
        //发生时间 、 发生错误
        public Dictionary<DateTime, Exception> ErrorDic;
    }

}
