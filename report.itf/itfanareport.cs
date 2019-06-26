using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using weCare.Core.Entity;
using weCare.Core.Itf;
using weCare.Core.Utils;
using Report.Entity;

namespace Report.Itf
{
    /// <summary>
    /// 手麻报表.契约
    /// </summary>
    [ServiceContract]
    public interface ItfAnaReport : IWcf, IDisposable
    {
        /// <summary>
        /// 获取麻醉科医师
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetAnaOperator")]
        List<EntityCodeOperator> GetAnaOperator();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetAnaRegister1")]
        List<EntityAnaRegister1> GetAnaRegister1(string beginDate, string endDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetAnaRegister2")]
        List<EntityAnaRegister2> GetAnaRegister2(string beginDate, string endDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetAnaStat1")]
        List<EntityAnaStat1> GetAnaStat1(string beginDate, string endDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetAnaStat2")]
        List<EntityAnaStat2> GetAnaStat2(string beginDate, string endDate);

        /// <summary>
        /// 术前排班表
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetOperationRegister")]
        List<EntityOperationReport> GetOperationRegister(List<EntityParm> dicParm);
        

        /// <summary>
        /// GetRegister1Xml
        /// </summary>
        /// <param name="anaId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetRegister1Xml")]
        string GetRegister1Xml(decimal anaId);

        /// <summary>
        /// Register1Edit
        /// </summary>
        /// <param name="anaId"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        [OperationContract(Name = "Register1Edit")]
        int Register1Edit(decimal anaId, string xmlData);

        /// <summary>
        /// GetRegister2Xml
        /// </summary>
        /// <param name="anaId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetRegister2Xml")]
        string GetRegister2Xml(decimal anaId);

        /// <summary>
        /// Register2Edit
        /// </summary>
        /// <param name="anaId"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        [OperationContract(Name = "Register2Edit")]
        int Register2Edit(decimal anaId, string xmlData);

        /// <summary>
        /// GetAnaStatTemp
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetAnaStatTemp")]
        List<EntityAnaStatTemp> GetAnaStatTemp();

        /// <summary>
        /// SaveAnaStatTemp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract(Name = "SaveAnaStatTemp")]
        int SaveAnaStatTemp(List<EntityAnaStatTemp> data);

        #region 院感报表
        /// <summary>
        /// GetAccessRecord
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetAccessRecord")]
        List<EntityYgAccessRecord> GetAccessRecord(string beginDate, string endDate);

        /// <summary>
        /// GetYgAdversInpatStat
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetYgAdversInpatStat")]
        List<EntityYgInpatStat> GetYgAdversInpatStat(string beginDate, string endDate);

        /// <summary>
        /// GetYgDrugStat
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetYgDrugStat")]
        List<EntityYgDrugStat> GetYgDrugStat(string beginDate, string endDate);

        /// <summary>
        /// GetYgOperationStat
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetYgOperationStat")]
        List<EntityYgOperationStat> GetYgOperationStat(string beginDate, string endDate);


        /// <summary>
        /// GetYgOperationStat
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetYgMachineStat")]
        List<EntityYgMachineStat> GetYgMachineStat(string beginDate, string endDate);

        /// <summary>
        /// getRiskAnaStat
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "getRiskAnaStat")]
        List<EntityRiskAna> getRiskAnaStat(List<EntityParm> dicParm);

        /// <summary>
        /// getDeptList
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "getDeptList")]
        List<EntityDeptList> getDeptList();

        #endregion

        #region  院感预警信息

        [OperationContract(Name = "GetCheckLisInfo")]
        int GetCheckLisInfo(string beginDate, string endDate);

        [OperationContract(Name = "getAlertInfo")]
        List<EntityAlertDisplay> getAlertInfo(List<EntityParm> dicParm);

        [OperationContract(Name = "GetVsInfo")]
        int GetVsInfo(string beginDate, string endDate);

        [OperationContract(Name = "GetOrderAlertInfo")]
        int GetOrderAlertInfo(string beginDate, string endDate);

        [OperationContract(Name = "GetVsInfo2")]
        int GetVsInfo2(string beginDate, string endDate);

        [OperationContract(Name = "GetVsInfo3")]
        int GetVsInfo3(string beginDate, string endDate);

        [OperationContract(Name = "getAlertStat")]
        List<EntityAlertStat> getAlertStat(string beginDate, string endDate);
        
        #endregion

        #region 药占比报表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetZyRptYzb")]
        List<EntityRptYzb> GetZyRptYzb(Dictionary<string, string> dicParm,int statFlg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetMzRptYzb")]
        List<EntityRptYzb> GetMzRptYzb(Dictionary<string, string> dicParm,int statFlg);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetSysParamStr")]
        string GetSysParamStr(string param);
        #endregion

    }
}
