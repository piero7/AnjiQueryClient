using QueryClient.LogService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryClient.Helper
{
    class DubiousFliter
    {
        static public void GetDobious(IEnumerable<QueryLog> logList)
        {
            //读取增加的参数
            var addByMold = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DobiousByNewMold"]);
            var addByfeature = int.Parse(System.Configuration.ConfigurationManager.AppSettings["DobiousByNewFeature"]);

            //筛选日志
            var tmpUsefulList = logList.Where(l => l.Code.Length == 14 && l.Result == QueryResultMold.已被查询);
            var usefulList = new List<QueryLog>();
            foreach (var l in tmpUsefulList)
            {
                usefulList.Add(l);
            }
            var duboiousList = new List<DubiousList>();
            var lookup = usefulList.ToLookup(l => l.Code);

            //建立Dubious列表
            foreach (var l in lookup)
            {
                duboiousList.Add(new DubiousList(l.Key, l.ToList(), 5, 5));
            }

            //获取增加的可疑度并提交
            using (DubiousService.ServiceClient client = new DubiousService.ServiceClient())
            {
                client.Open();
                foreach (var d in duboiousList)
                {
                    var dubious = d.GetDubious();
                    client.AddDubiousValuesLog(d.Code, dubious);
                }
            }
        }
    }

    class DubiousList
    {
        public DubiousList(string Code, List<QueryLog> todayyList, int newMoldAdd, int newFeatureAdd)
        {
            this._addDobiousByNewMold = newMoldAdd;
            this._addDobipusByNewFeature = newFeatureAdd;

            this.TodayLogs = todayyList.OrderBy(l => l.OptionDate).ToList();
            this.IsFinishGetDuboius = false;
            this.IsFinishGetLog = false;
            this.Code = Code;
            try
            {
                var client = new LogServiceClient();
                this.OLogList = client.GetCodeQueryLogs(Code).OrderBy(l => l.OptionDate).ToList();
            }
            catch (Exception)
            {
                this.IsFinishGetLog = false;
            }
            this.IsFinishGetLog = true;
        }

        public string Code;
        public List<QueryLog> TodayLogs;
        public List<QueryLog> OLogList;
        public int DubiousCount
        {
            get;
            private set;
        }


        private int _addDobiousByNewMold;
        private int _addDobipusByNewFeature;

        public bool IsFinishGetLog
        {
            get;
            private set;
        }

        public bool IsFinishGetDuboius
        {
            get;
            private set;
        }

        public int GetDubious()
        {
            int ret = 0;
            foreach (var tLog in this.TodayLogs)
            {
                if (tLog.Result == QueryResultMold.已被查询)
                {
                    var usefulLogs = this.OLogList.Where(l => l.OptionDate < tLog.OptionDate);
                    var sameMoldList = usefulLogs.Where(l => l.Mold == tLog.Mold);

                    if (sameMoldList.Count() == 0)
                    {
                        ret += _addDobiousByNewMold;
                    }
                    else if (!sameMoldList.Contains(tLog, new QueryFeatureCompaper()))
                    {
                        ret += _addDobipusByNewFeature;
                    }
                }
            }
            this.DubiousCount = ret;
            return ret;
        }

    }

    public class QueryFeatureCompaper : IEqualityComparer<QueryLog>
    {

        public bool Equals(QueryLog x, QueryLog y)
        {
            return x.Mold == y.Mold && x.Feature == y.Feature;
        }

        public int GetHashCode(QueryLog obj)
        {
            return (obj.Mold.ToString() + obj.Feature).GetHashCode();
        }
    }


}
