using QueryClient.LogService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryClient.Helper
{
    class SuspiciousList
    {
        public SuspiciousList(string Code, List<QueryLog> todayyList, int newMoldAdd, int newFeatureAdd)
        {
            this._addDobiousByNewMold = newMoldAdd;
            this._addDobipusByNewFeature = newFeatureAdd;

            this.TodayLogs = todayyList.OrderBy(l => l.OptionDate).ToList();
            this.IsFinishGetSuspicious = false;
            this.IsFinishGetLog = false;
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

        private int _addDobiousByNewMold;
        private int _addDobipusByNewFeature;

        public bool IsFinishGetLog
        {
            get;
            private set;
        }

        public bool IsFinishGetSuspicious
        {
            get;
            private set;
        }

        public int GetSuspicous()
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
                        return _addDobiousByNewMold;
                    }
                    if (!sameMoldList.Contains(tLog, new QueryFeatureCompaper()))
                    {
                        return _addDobipusByNewFeature;
                    }
                }
            }

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
