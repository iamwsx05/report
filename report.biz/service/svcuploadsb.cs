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
    /// 社保上传.WCF
    /// </summary>
    public class SvcUploadSb : Report.Itf.ItfUploadSb
    {
        #region 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityPatUpload> GetPatList(List<EntityParm> dicParm,int flg)
        {
            using (bizUploadSb biz = new bizUploadSb())
            {
                return biz.GetPatList(dicParm,flg);
            }
        }
        #endregion


        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityPatUpload> GetPatList2(List<EntityParm> dicParm)
        {
            using (bizUploadSb biz = new bizUploadSb())
            {
                return biz.GetPatList2(dicParm);
            }
        }
        #endregion

        
        #region 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityPatUpload> GetPatFirstInfo(List<EntityPatUpload> lstUpVo)
        {
            using (bizUploadSb biz = new bizUploadSb())
            {
                return biz.GetPatFirstInfo(lstUpVo);
            }
        }
        #endregion

        #region 保存上传记录
        /// <summary>
        /// 保存上传记录
        /// </summary>
        /// <param name="eventVo"></param>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public int SavePatFirstPage(List<EntityPatUpload> lstVo)
        {
            using (bizUploadSb biz = new bizUploadSb())
            {
                return biz.SavePatFirstPage(lstVo);
            }
        }
        
       
        #endregion

         #region 处方项目信息
        /// <summary>
        /// 处方项目信息
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityMzcf> GetPatMzchList(List<EntityParm> dicParm)
        {
            using (bizUploadSb biz = new bizUploadSb())
            {
                return biz.GetPatMzchList(dicParm);
            }
        }
       
        #endregion

        #region 处方项目明细信息
        /// <summary>
        /// 处方项目明细信息
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityMzcfMsg> GetPatMzcfMsgList(string registerid)
        {
            using (bizUploadSb biz = new bizUploadSb())
            {
                return biz.GetPatMzcfMsgList(registerid);
            }
        }
        #endregion

        #region 保存处方上传信息
        /// <summary>
        /// 保存处方上传信息
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public int SaveUpPatMzcf(List<EntityMzcf> lstVo)
        {
            using (bizUploadSb biz = new bizUploadSb())
            {
                return biz.SaveUpPatMzcf(lstVo);
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
