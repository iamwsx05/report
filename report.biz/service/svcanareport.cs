using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Biz;
using Report.Entity;

namespace Report.Svc
{
    /// <summary>
    /// 手麻报表.svc
    /// </summary>
    public class SvcAnaReport : Report.Itf.ItfAnaReport
    {
        #region 获取麻醉科医师
        /// <summary>
        /// 获取麻醉科医师
        /// </summary>
        /// <returns></returns>
        public List<EntityCodeOperator> GetAnaOperator()
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetAnaOperator();
            }
        }
        #endregion

        #region GetAnaRegister1
        /// <summary>
        /// GetAnaRegister1
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityAnaRegister1> GetAnaRegister1(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetAnaRegister1(beginDate, endDate);
            }
        }
        #endregion

        #region GetAnaRegister2
        /// <summary>
        /// GetAnaRegister2
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityAnaRegister2> GetAnaRegister2(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetAnaRegister2(beginDate, endDate);
            }
        }
        #endregion

        #region GetAnaStat1
        /// <summary>
        /// GetAnaStat1
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityAnaStat1> GetAnaStat1(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetAnaStat1(beginDate, endDate);
            }
        }
        #endregion

        #region GetAnaStat2
        /// <summary>
        /// GetAnaStat2
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityAnaStat2> GetAnaStat2(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetAnaStat2(beginDate, endDate);
            }
        }
        #endregion

        #region GetRegister1Xml
        /// <summary>
        /// GetRegister1Xml
        /// </summary>
        /// <param name="anaId"></param>
        /// <returns></returns>
        public string GetRegister1Xml(decimal anaId)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetRegister1Xml(anaId);
            }
        }
        #endregion

        #region Register1Edit
        /// <summary>
        /// Register1Edit
        /// </summary>
        /// <param name="anaId"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public int Register1Edit(decimal anaId, string xmlData)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.Register1Edit(anaId, xmlData);
            }
        }
        #endregion

        #region GetRegister2Xml
        /// <summary>
        /// GetRegister2Xml
        /// </summary>
        /// <param name="anaId"></param>
        /// <returns></returns>
        public string GetRegister2Xml(decimal anaId)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetRegister2Xml(anaId);
            }
        }
        #endregion

        #region Register2Edit
        /// <summary>
        /// Register2Edit
        /// </summary>
        /// <param name="anaId"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public int Register2Edit(decimal anaId, string xmlData)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.Register2Edit(anaId, xmlData);
            }
        }
        #endregion

        #region GetAnaStatTemp
        /// <summary>
        /// GetAnaStatTemp
        /// </summary>
        /// <returns></returns>
        public List<EntityAnaStatTemp> GetAnaStatTemp()
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetAnaStatTemp();
            }
        }
        #endregion

        #region SaveAnaStatTemp
        /// <summary>
        /// SaveAnaStatTemp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int SaveAnaStatTemp(List<EntityAnaStatTemp> data)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.SaveAnaStatTemp(data);
            }
        }
        #endregion

        #region 术前排班表
        /// <summary>
        /// 术前排班表
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityOperationReport> GetOperationRegister(List<EntityParm> dicParm)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetOperationRegister(dicParm);
            }
        }
        #endregion

        #region 院感报表
        #region
        /// <summary>
        /// GetAccessRecord
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityYgAccessRecord> GetAccessRecord(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetAccessRecord(beginDate, endDate);
            }
        }
        #endregion

        #region
        /// <summary>
        /// GetYgAdversInpatStat
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityYgInpatStat> GetYgAdversInpatStat(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetYgAdversInpatStat(beginDate, endDate);
            }
        }
        #endregion

        #region
        /// <summary>
        /// GetYgDrugStat
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityYgDrugStat> GetYgDrugStat(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetYgDrugStat(beginDate, endDate);
            }
        }
        #endregion

        #region
        /// <summary>
        /// GetYgOperationStat
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityYgOperationStat> GetYgOperationStat(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetYgOperationStat(beginDate, endDate);
            }
        }
        #endregion

        #region
        /// <summary>
        /// GetYgMachineStat
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityYgMachineStat> GetYgMachineStat(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetYgMachineStat(beginDate, endDate);
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityRiskAna> getRiskAnaStat(List<EntityParm> dicParm)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.getRiskAnaStat(dicParm);
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EntityDeptList> getDeptList()
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.getDeptList();
            }
        }
        #endregion

        #endregion

        #region 院感预警
        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetCheckLisInfo(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetCheckLisInfo(beginDate, endDate);
            }
        }
        #endregion

        #region
        public int GetVsInfo(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetVsInfo(beginDate, endDate);
            }
        }
        #endregion

        #region
        public List<EntityAlertDisplay> getAlertInfo(List<EntityParm> dicParm)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.getAlertInfo(dicParm);
            }
        }
        #endregion

        #region
        public int GetOrderAlertInfo(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetOrderAlertInfo(beginDate, endDate);
            }
        }
        #endregion

        #region
        public int GetVsInfo2(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetVsInfo2(beginDate, endDate);
            }
        }
        #endregion

        #region
        public int GetVsInfo3(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetVsInfo3(beginDate, endDate);
            }
        }
        #endregion

        #region
        public List<EntityAlertStat> getAlertStat(string beginDate, string endDate)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.getAlertStat(beginDate, endDate);
            }
        }
        #endregion

        #endregion

        #region 药占比报表

        #region 住院药占比报表
        /// <summary>
        /// 药占比报表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetZyRptYzb(Dictionary<string, string> dicParm,int statFlg)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetZyRptYzb(dicParm,statFlg);
            }
        }
        #endregion

        #region 门诊药占比报表
        /// <summary>
        /// 药占比报表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetMzRptYzb(Dictionary<string, string> dicParm,int statFlg)
        {
            using (bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetMzRptYzb(dicParm, statFlg);
            }
        }
        #endregion

        /// <summary>
        /// 获取药占比报表参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetSysParamStr(string param)
        {
            using(bizAnaReport biz = new bizAnaReport())
            {
                return biz.GetSysParamStr(param);
            }
        }

        #endregion

        #region Verify
        /// <summary>
        /// Verify
        /// </summary>
        /// <returns></returns>
        public bool Verify()
        { return true; }
        #endregion

        #region IDispose
        /// <summary>
        /// IDispose
        /// </summary>
        public void Dispose()
        { GC.SuppressFinalize(this); }
        #endregion

    }
}
