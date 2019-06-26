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
    /// 不良事件.WCF
    /// </summary>
    public class SvcAdverseEvent : Report.Itf.ItfAdverseEvent
    {
        #region 查找病人
        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="flag">1 门诊； 2 住院</param>
        /// <returns></returns>
        public List<EntityPatientInfo> GetPatient(string cardNo, int flag, string deptCode)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetPatient(cardNo, flag, deptCode);
            }
        }
        #endregion

        #region 获取不良事件列表
        /// <summary>
        /// 获取不良事件列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityEventDisplay> GetEventList(List<EntityParm> dicParm)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetEventList(dicParm);
            }
        }
        #endregion

        #region 获取不良事件实例(vo)
        /// <summary>
        /// 获取不良事件实例(vo)
        /// </summary>
        /// <param name="serNo"></param>
        /// <returns></returns>
        public EntityRptEvent GetEvent(decimal rptId)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetEvent(rptId);
            }
        }
        #endregion

        #region 保存不良事件
        /// <summary>
        /// 保存不良事件
        /// </summary>
        /// <param name="eventVo"></param>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public int SaveEvent(EntityRptEvent eventVo, out decimal rptId)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.SaveEvent(eventVo, out rptId);
            }
        }
        #endregion

        #region 删除不良事件(伪删)
        /// <summary>
        ///  删除不良事件(伪删)
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public int DelEvent(decimal rptId)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.DelEvent(rptId);
            }
        }
        #endregion

        #region 统计

        #region 医疗器械不良事件汇总
        /// <summary>
        /// 医疗器械不良事件汇总
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public List<EntityStatInstrument> GetStatInstrument(string beginTime, string endTime, string deptCode)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatInstrument(beginTime, endTime, deptCode);
            }
        }
        #endregion

        #endregion

        #region 不良事件统计列表
        /// <summary>
        /// 不良事件统计列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityEventDisplay> GetEventListAll(List<EntityParm> dicParm)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetEventListAll(dicParm);
            }
        }
        #endregion

        #region 获取事件参数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetEventDicparm(decimal classid)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetEventDicparm(classid);
            }
        }

        #endregion

        #region 医疗安全（不良）事件统计表

        #region 按级别
        public List<EntityEventStat> GetStatEventLevel(string beginDate, string endDate)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatEventLevel(beginDate, endDate);
            }
        }
        #endregion

        #region 按类别
        public List<EntityEventStat> GetStatEventClass(string beginDate, string endDate)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatEventClass(beginDate, endDate);
            }
        }
        #endregion

        #region 按科室
        public List<EntityEventStat> GetStatEventDept(string beginDate, string endDate)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatEventDept(beginDate, endDate);
            }
        }
        #endregion

        #endregion

        #region 护理安全不良事件统计

        #region 护理质量与安全（不良事件）（类型统计）
        public List<EntityNursEventClass> GetStatNursEventClass(string beginDate, string endDate)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatNursEventClass(beginDate, endDate);
            }
        }
        #endregion

        #region 护理质量与安全（不良事件）（细分类型统计）
        public List<EntityNursEventSubClass> GetStatNursEventSubClass(string beginDate, string endDate)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatNursEventSubClass(beginDate, endDate);
            }
        }
        #endregion

        #region 护理质量与安全（不良事件）（上报类型统计）
        public List<EntityNursEventRptClass> GetStatNursEventLevel(string beginDate, string endDate)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatNursEventLevel(beginDate, endDate);
            }
        }
        #endregion

        #region 护理质量与安全（不良事件）上报科室统计
        public List<EntityNursEventDept> GetStatNursEventDept(string beginDate, string endDate)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatNursEventDept(beginDate, endDate);
            }
        }
        #endregion

        #region 全院护理上报安全事件汇总表
        public List<EntityNursEventStrument> GetStatNursEventInstrument(List<EntityParm> dicParm)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatNursEventInstrument(dicParm);
            }
        }
        #endregion

        #region 护理皮肤损害安全（不良）事件统计
        public List<EntityNursEventSkin> GetStatNursEventSkin(string beginDate, string endDate)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatNursEventSkin(beginDate, endDate);
            }
        }
        #endregion

        #region 护理皮肤损害安全（不良）事件汇总
        public List<EntitySkinEventStrument> GetStatSkinEventInstrument(List<EntityParm> dicParm)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatSkinEventInstrument(dicParm);
            }
        }
        #endregion

        #region 职业暴露汇总
        public List<EntityOccupationexp> GetStatOccupationexp(List<EntityParm> dicParm)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetStatOccupationexp(dicParm);
            }
        }
        #endregion

        #region
        public Dictionary<string,string> GetAdversDept()
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetAdversDept();
            }
        }
        #endregion

        #region
        /// <summary>
        /// 事件类型
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAdversType()
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetAdversType();
            }
        }
        #endregion
        
        #region
        /// <summary>
        /// 皮肤事件类型
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAdversSkinType()
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetAdversSkinType();
            }
        }
        #endregion

        #region 护理事件发生时间段
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetSpanTime() 
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetSpanTime();
            }
        }
        #endregion

        #endregion

        #region 具有编辑事件权限可查看自己所在科室事件
        public string GetEventRoleEdit(string empNo)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetEventRoleEdit(empNo);
            }
        }
        #endregion

        #region 具有查看职业暴露权限可以看所有职业暴露
        public string GetZYBLEventRoleLimt(string empNo)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetZYBLEventRoleLimt(empNo);
            }
        }
        #endregion

        #region 具有删除权限可以看所有不良事件
        public string GetEventRoleDel(string empNo)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetEventRoleDel(empNo);
            }
        }
        #endregion

        #region 具有查看权限可以看所有不良事件
        public string GetEventRoleQuery(string empNo)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetEventRoleQuery(empNo);
            }
        }
        #endregion

        #region  获取科室代码
        public string GetDeptCode(string deptName)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetDeptCode(deptName);
            }
        }
        #endregion

        #region 感染病例报告
        #region 获取病人信息
        /// <summary>
        /// 获取病人信息
        /// </summary>
        /// <param name="inpatId"></param>
        /// <returns></returns>
        public List<EntityPatientInfo> GetPatInfo(string inpatId)
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.GetPatInfo(inpatId);
            }
        }
        #endregion

        #region 保存病例报告
        /// <summary>
        /// 保存不良事件
        /// </summary>
        /// <param name="EntityInfectionus"></param>
        /// <param name="EntityPathogeny"></param>
        /// <returns></returns>
        public int SaveInfectionus(EntityInfectionus infectionusVo, List<EntityPathogeny> lstPathogenyVo)
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.SaveInfectionus(infectionusVo, lstPathogenyVo);
            }
        }
        #endregion

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="EntityInfectionus"></param>
        /// <returns></returns>
        public int ComfirmRpt(EntityInfectionus vo)
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.ComfirmRpt(vo);
            }
        }
        #endregion

        #region 获取感染病例列表
        /// <summary>
        /// 获取感染病例列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityInfectionus> GetInfectionusList(List<EntityParm> dicParm)
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.GetInfectionusList(dicParm);
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public List<EntityInfectionus> GetXrDataSource(decimal rptId)
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.GetXrDataSource(rptId);
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public List<EntityPathogeny> GetPathogeny(decimal rptId)
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.GetPathogeny(rptId);
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetInfectionSiteSource()
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.GetInfectionSiteSource();
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetCausationSource()
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.GetCausationSource();
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetSampleSource()
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.GetSampleSource();
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<string> GetDrugSource()
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.GetDrugSource();
            }
        }
        #endregion
        
        #region
        /// <summary>
        /// 获取住院科室列表
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<EntityDeptList> getDeptList()
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.getDeptList();
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public string GetInfectReportRoleQuery(string empNo)
        {
            using (bizAdverseInfectionus biz = new bizAdverseInfectionus())
            {
                return biz.GetInfectReportRoleQuery(empNo);
            }
        }
        #endregion


        #endregion

        #region 传染病报告

        #region 获取传染病报告列表
        /// <summary>
        /// 获取传染病报告列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityZrbbgDisplay> GetZrbbgList(List<EntityParm> dicParm)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetZrbbgList(dicParm);
            }
        }
        #endregion

        #region 获取传染病报告实例(vo)
        /// <summary>
        /// 获取传染病报告实例(vo)
        /// </summary>
        /// <param name="serNo"></param>
        /// <returns></returns>
        public EntityRptZrbbg GetZrbbg(decimal rptId)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetZrbbg(rptId);
            }
        }
        #endregion

        #region 保存传染病报告
        /// <summary>
        /// 保存传染病报告
        /// </summary>
        /// <param name="eventVo"></param>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public int SaveZrbbg(EntityRptZrbbg eventVo, out decimal rptId)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.SaveZrbbg(eventVo, out rptId);
            }
        }
        #endregion

        #region 删除传染病报告(伪删)
        /// <summary>
        ///  删除传染病报告(伪删)
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public int DelZrbbg(decimal rptId)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.DelZrbbg(rptId);
            }
        }
        #endregion

        #region 更新打印标志
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public int UpdateZrbbgPrintFlg(decimal rptId)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.UpdateZrbbgPrintFlg(rptId);
            }
        }
        #endregion

        #region 获取传染病报告参数
        /// <summary>
        ///  获取传染病报告参数
        /// </summary>
        public List<EntityRptZrbbgParm> GetZrbbgParm()
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetZrbbgParm();
            }
        }
        #endregion

        #region RegisterZrbygfk
        /// <summary>
        /// RegisterZrbygfk
        /// </summary>
        /// <param name="rptId"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public int RegisterZrbygfk(decimal rptId, string xmlData)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.RegisterZrbygfk(rptId, xmlData);
            }
        }

        #endregion

        #region GetRegisterZrbygfk
        /// <summary>
        /// GetRegisterZrbygfk
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public string GetRegisterZrbygfk(decimal rptId)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetRegisterZrbygfk(rptId);
            }
        }
        #endregion

        #endregion

        #region 出院患者随访记录

        #region 查找病人
        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="flag">1 门诊； 2 住院</param>
        /// <returns></returns>
        public List<EntityPatientInfo> GetInterviewPatient(string cardNo, string deptCode)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetInterviewPatient(cardNo, deptCode);
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public EntityOutpatientInterview GetInterviewVo(decimal rptId)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetInterviewVo(rptId);
            }
        }
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public int SaveInterview(ref EntityOutpatientInterview vo)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.SaveInterview(ref vo);
            }
        }
        #endregion

        public List<EntityOutpatientInterview> GetInterviewList(List<EntityParm> dicParm)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetInterviewList(dicParm);
            }
        }

        public List<EntityRptInterviewParm> GetInterviewParm()
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetInterviewParm();
            }
        }

        public int DelInterview(decimal rptId)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.DelInterview(rptId);
            }
        }

        public List<EntityOutpatientInterview> GetPatInterviewInfo(List<EntityParm> dicParm)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetPatInterviewInfo(dicParm);
            }
        }

        #endregion

        #region 获取所属科室
        /// <summary>
        /// 获取所属科室
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        public string GetOwerDeptCode(string empno)
        {
            using (bizAdverseEvent biz = new bizAdverseEvent())
            {
                return biz.GetOwerDeptCode(empno);
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
