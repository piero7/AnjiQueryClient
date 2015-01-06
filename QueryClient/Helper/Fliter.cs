using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QueryClient.FliterService;
using QueryClient.LogService;
using GalaSoft.MvvmLight.Messaging;
namespace QueryClient
{
    class Fliter
    {
        public List<BlackNote> BlackList;
        public List<WhiteNote> WhiteList;

        public List<QueryLog> TodayLogList;
        public List<QueryLog> LastHundreLogList;

        private int LastId = 0;


        private IEnumerable<QueryLog> NewLogList;
        private IEnumerable<BlackNote> OverDueBlackList;

        private int shortTimeSpan = 999;
        private int shortTimeQueryTimes = 999;
        private int timesForOneDay = 999;

        public Fliter()
        {
            #region init parms
            this.shortTimeSpan = int.Parse(System.Configuration.ConfigurationManager.AppSettings["shortTimeSpan"]);
            this.shortTimeQueryTimes = int.Parse(System.Configuration.ConfigurationManager.AppSettings["shortTimeQueryTimes"]);
            this.timesForOneDay = int.Parse(System.Configuration.ConfigurationManager.AppSettings["timesForOneDay"]);

            this.WhiteList = new List<WhiteNote>();
            this.BlackList = new List<BlackNote>();
            this.TodayLogList = new List<QueryLog>();
            this.LastHundreLogList = new List<QueryLog>();
            #endregion

            #region 同步黑白名单
            FliterServiceClient fClient = new FliterServiceClient();
            LogServiceClient lClient = new LogServiceClient();

            lClient.Open();
            fClient.Open();

            this.BlackList = fClient.GetBlackNoteList().Where(n => n.Flg).ToList();
            this.WhiteList = fClient.GetWhiteNoteList().Where(n => n.Flg).ToList();
            #endregion

        }



        public Task FliterBlackNote(bool SyncWhiteNotes, bool SyncBlackNotes)
        {

            return Task.Run(() =>
            {
                FliterServiceClient fClient = new FliterServiceClient();

                LogServiceClient lClient = new LogServiceClient();
                try
                {
                    lClient.Open();
                    fClient.Open();
                    if (SyncBlackNotes)
                    {
                        //定时同步黑名单
                        this.BlackList = fClient.GetBlackNoteList().ToList();
                    }
                    if (SyncWhiteNotes)
                    {
                        //定时同步白名单
                        this.WhiteList = fClient.GetWhiteNoteList().ToList();
                    }

                    //获取日志
                    NewLogList = lClient.LastQueryLogQuery(this.LastId, false);
                    if (NewLogList.Count() > 0)
                    {
                        this.LastId = NewLogList.Max(n => n.Id);
                    }

                    //添加至对应的列表中
                    this.TodayLogList.AddRange(NewLogList);
                    this.LastHundreLogList.AddRange(NewLogList);



                    //删除过期黑名单
                    foreach (var n in BlackList)
                    {
                        if (n.Reason == "当日查询过多！" && DateTime.Now.Subtract(n.CreateDate).TotalDays > 1)
                        {
                            fClient.DelBlackNote(n);
                            continue;
                        }
                        if (n.Reason == "短时间内频繁查询。" && DateTime.Now.Subtract(n.CreateDate).TotalSeconds > this.shortTimeSpan)
                        {
                            fClient.DelBlackNote(n);
                        }
                    }

                    //删除本地日志中的过期项
                    //if (this.LastHundreLogList.Count() > 100)
                    //{
                    //    this.LastHundreLogList.RemoveRange(0, this.LastHundreLogList.Count() - 100);
                    //}
                    this.LastHundreLogList.RemoveAll(n => DateTime.Now.Subtract(n.OptionDate).TotalSeconds > this.shortTimeSpan);
                    this.TodayLogList.RemoveAll(n => DateTime.Now.Subtract(n.OptionDate).TotalDays > 1);
                    //while (this.TodayLogList.Count() > 0 && DateTime.Now.Subtract(this.TodayLogList[0].OptionDate).TotalDays > 1)
                    //{
                    //    this.TodayLogList.RemoveAt(0);
                    //}
                    BlackList.RemoveAll(n => n.Reason == "当日查询过多！" && DateTime.Now.Subtract(n.CreateDate).TotalDays > 1);
                    BlackList.RemoveAll(n => n.Reason == "短时间内频繁查询。" && DateTime.Now.Subtract(n.CreateDate).TotalSeconds > this.shortTimeSpan);

                    //筛选黑名单
                    var todayDic = GetCountDic(TodayLogList);
                    var lastDic = GetCountDic(LastHundreLogList);

                    //检测是否在白名单内
                    foreach (var k in todayDic.Keys)
                    {
                        if (this.WhiteList.Any(n => n.Feature == k.Feature && n.Mold == k.Mold))
                        {
                            //todayDic.Remove(k);
                            todayDic[k] = 0;
                        }
                    }
                    foreach (var k in lastDic.Keys)
                    {
                        if (this.WhiteList.Any(n => n.Feature == k.Feature && n.Mold == k.Mold))
                        {
                            //lastDic.Remove(k);
                            lastDic[k] = 0;
                        }
                    }
                    //添加至黑名单中(封装新的方法)
                    foreach (var note in todayDic)
                    {
                        if (note.Value > this.timesForOneDay)
                        {
                            // AddToBlackNote(note.Key);
                            var newNote = new BlackNote
                            {
                                CreateDate = DateTime.Now,
                                Feature = note.Key.Feature,
                                Mold = note.Key.Mold,
                                Flg = true,
                                Reason = "当日查询过多！",
                            };
                            if (fClient.AddBlackNote(newNote) == 1)
                            {
                                this.BlackList.Add(newNote);
                            }
                        }

                    }
                    foreach (var note in lastDic)
                    {
                        if (note.Value > this.shortTimeQueryTimes)
                        {
                            var newNote = new BlackNote
                            {
                                CreateDate = DateTime.Now,
                                Reason = "短时间内频繁查询。",
                                Flg = true,
                                Feature = note.Key.Feature,
                                Mold = note.Key.Mold,
                                UpdateDate = DateTime.Now,
                                Id = 0
                            };
                            var ret = fClient.AddBlackNote(newNote);
                            if (ret == 1)
                            //if (fClient.AddBlackNote(newNote) == 1)
                            {
                                this.BlackList.Add(newNote);
                            }
                        }
                    }
                    lClient.Close();
                    fClient.Close();
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send<DialogMessage>(new DialogMessage(string.Format("An error was happend on {0}:\r\n{1}", "add black note", ex.Message), null), "showMsg");
                }
                finally
                {
                    lClient.Abort();
                    fClient.Abort();
                }
            }
             );
        }

        private void AddToBlackNote(NoteFeature noteFeature)
        {
            throw new NotImplementedException();
        }

        private Dictionary<NoteFeature, int> GetCountDic(IEnumerable<QueryLog> logList)
        {
            Dictionary<NoteFeature, int> retDic = new Dictionary<NoteFeature, int>();
            foreach (var l in logList)
            {
                var fea = new NoteFeature
                {
                    Feature = l.Feature,
                    Mold = (QueryClient.FliterService.QueryMold)l.Mold
                };
                if (retDic.Keys.Contains(fea))
                {
                    retDic[fea]++;
                }
                else
                {
                    retDic.Add(fea, 1);
                }
            }
            return retDic;
        }

        private bool IsInBlackList(BlackNote note)
        {
            return this.BlackList.Any(n => n.Feature == note.Feature && n.Mold == note.Mold);
        }
    }


    struct NoteFeature
    {
        public string Feature;
        public QueryClient.FliterService.QueryMold Mold;
    }
}
