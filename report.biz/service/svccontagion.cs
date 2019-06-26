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
    /// 传染病.WCF
    /// </summary>
    public class SvcContagion : Report.Itf.ItfContagion
    {
        #region 查找病人
        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="flag">1 门诊； 2 住院</param>
        /// <returns></returns>
        public List<EntityPatientInfo> GetPatient(string cardNo, int flag)
        {
            using (bizContagion biz = new bizContagion())
            {
                return biz.GetPatient(cardNo, flag);
            }
        }
        #endregion

        #region 查找病人检验信息
        /// <summary>
        /// 查找病人检验信息
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="flag">1 门诊； 2 住院</param>
        /// <returns></returns>
        public List<EntityAidsCheck> GetPatLisInfo(string cardNo, int flag,decimal formId)
        {
            using (bizContagion biz = new bizContagion())
            {
                return biz.GetPatLisInfo(cardNo, flag, formId);
            }
        }
        #endregion

        #region 获取传染病列表
        /// <summary>
        /// 获取传染病列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityContagionDisplay> GetContagionList(List<EntityParm> dicParm)
        {
            using (bizContagion biz = new bizContagion())
            {
                return biz.GetContagionList(dicParm);
            }
        }
        #endregion

        #region 获取传染病实例(vo)
        /// <summary>
        /// 获取传染病实例(vo)
        /// </summary>
        /// <param name="serNo"></param>
        /// <returns></returns>
        public EntityRptContagion GetContagion(decimal rptId)
        {
            using (bizContagion biz = new bizContagion())
            {
                return biz.GetContagion(rptId);
            }
        }
        #endregion

        #region 保存传染病
        /// <summary>
        /// 保存传染病
        /// </summary>
        /// <param name="eventVo"></param>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public int SaveContagion(EntityRptContagion eventVo,string reqNo, out decimal rptId)
        {
            using (bizContagion biz = new bizContagion())
            {
                return biz.SaveContagion(eventVo,reqNo,out rptId);
            }
        }
        #endregion

        #region 删除传染病(伪删)
        /// <summary>
        ///  删除传染病(伪删)
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public int DelContagion(decimal rptId)
        {
            using (bizContagion biz = new bizContagion())
            {
                return biz.DelContagion(rptId);
            }
        }
        #endregion


        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="empNo"></param>
        /// <returns></returns>
        public string GetContagionRole(string empNo)
        {
            using (bizContagion biz = new bizContagion())
            {
                return biz.GetContagionRole(empNo);
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
