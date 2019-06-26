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
    /// 社保上传.契约
    /// </summary>
    [ServiceContract]
    public interface ItfUploadSb : IWcf, IDisposable
    {
        /// <summary>
        /// 获取病案首页&出院小结
        /// </summary>
        /// <param name="dicParm">1、病案首页 2、出院小结</param>
        /// <returns></returns>
        [OperationContract(Name = "GetPatList")]
        List<EntityPatUpload> GetPatList(List<EntityParm> dicParm, int flg);

        /// <summary>
        /// 获取病案首页其他信息
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetPatFirstInfo")]
        List<EntityPatUpload> GetPatFirstInfo(List<EntityPatUpload> lstUpVo);

        /// <summary>
        /// 保存上传的病案首页&出院小结
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "SavePatFirstPage")]
        int SavePatFirstPage(List<EntityPatUpload> lstVo);


        /// <summary>
        /// 获取门诊处方信息
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetPatMzchList")]
        List<EntityMzcf> GetPatMzchList(List<EntityParm> dicParm);

        /// <summary>
        /// 获取处方明细
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetPatMzcfMsgList")]
        List<EntityMzcfMsg> GetPatMzcfMsgList(string outpatrecipeid);

        /// <summary>
        /// 保存上传的门诊处方信息
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "SaveUpPatMzcf")]
        int SaveUpPatMzcf(List<EntityMzcf> lstVo);
        
    }
}
