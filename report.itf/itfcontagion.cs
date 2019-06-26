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
    /// 传染病.契约
    /// </summary>
    [ServiceContract]
    public interface ItfContagion : IWcf, IDisposable
    {
        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="flag">1 门诊； 2 住院</param>
        /// <returns></returns>
        [OperationContract(Name = "GetPatient")]
        List<EntityPatientInfo> GetPatient(string cardNo, int flag);

         /// <summary>
        /// 查找病人检验信息
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="flag">1 门诊； 2 住院</param>
        /// <returns></returns>
        [OperationContract(Name = "GetPatLisInfo")]
        List<EntityAidsCheck> GetPatLisInfo(string cardNo, int flag,decimal formId);

        /// <summary>
        /// 获取传染病列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetContagionList")]
        List<EntityContagionDisplay> GetContagionList(List<EntityParm> dicParm);

        /// <summary>
        /// 获取传染病实例(vo)
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetContagion")]
        EntityRptContagion GetContagion(decimal rptId);

        /// <summary>
        /// 保存传染病
        /// </summary>
        /// <param name="eventVo"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [OperationContract(Name = "SaveContagion")]
        int SaveContagion(EntityRptContagion eventVo,string reqNo, out decimal rptId);

        /// <summary>
        ///  删除传染病(伪删)
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "DelContagion")]
        int DelContagion(decimal rptId);

        /// <summary>
        ///  
        /// </summary>
        /// <param name="empNo"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetContagionRole")]
        string GetContagionRole(string empNo);
    }
}
