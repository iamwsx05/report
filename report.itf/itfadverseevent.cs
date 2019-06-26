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
    /// 不良事件.契约
    /// </summary>
    [ServiceContract]
    public interface ItfAdverseEvent : IWcf, IDisposable
    {
        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="cardNo"></param>
        /// <param name="flag">1 门诊； 2 住院</param>
        /// <returns></returns>
        [OperationContract(Name = "GetPatient")]
        List<EntityPatientInfo> GetPatient(string cardNo, int flag, string deptCode);

        /// <summary>
        /// 获取不良事件列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetEventList")]
        List<EntityEventDisplay> GetEventList(List<EntityParm> dicParm);

        /// <summary>
        /// 获取不良事件实例(vo)
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetAdverseEvent")]
        EntityRptEvent GetEvent(decimal rptId);

        /// <summary>
        /// 保存不良事件
        /// </summary>
        /// <param name="eventVo"></param>
        /// <param name="reportId"></param>
        /// <returns></returns>
        [OperationContract(Name = "SaveEvent")]
        int SaveEvent(EntityRptEvent eventVo, out decimal rptId);

        /// <summary>
        ///  删除不良事件(伪删)
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "DelEvent")]
        int DelEvent(decimal rptId);

        /// <summary>
        /// 医疗器械不良事件汇总
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatInstrument")]
        List<EntityStatInstrument> GetStatInstrument(string beginTime, string endTime, string deptCode);

        /// <summary>
        /// 不良事件统计列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetEventListAll")]
        List<EntityEventDisplay> GetEventListAll(List<EntityParm> dicParm);

        /// <summary>
        /// 事件参数
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetEventDicparm")]
        Dictionary<string, string> GetEventDicparm(decimal classid);

        #region 医疗安全（不良）事件统计表
        /// <summary>
        /// 医疗安全（不良）事件统计表（按级别）
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatEventLevel")]
        List<EntityEventStat> GetStatEventLevel(string beginDate, string endDate);

        /// <summary>
        /// 医疗安全（不良）事件统计表（按类别）
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatEventClass")]
        List<EntityEventStat> GetStatEventClass(string beginDate, string endDate);


        /// <summary>
        /// 医疗安全（不良）事件统计表（按科室）
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatEventDept")]
        List<EntityEventStat> GetStatEventDept(string beginDate, string endDate);

        #endregion

        #region 护理安全不良事件统计表
        /// <summary>
        /// 事件统计表（按类别）
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatNursEventClass")]
        List<EntityNursEventClass> GetStatNursEventClass(string beginDate, string endDate);

        /// <summary>
        /// 事件统计表（细分类别）
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatNursEventSubClass")]
        List<EntityNursEventSubClass> GetStatNursEventSubClass(string beginDate, string endDate);

        /// <summary>
        /// 事件统计表（上报类型）
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatNursEventLevel")]
        List<EntityNursEventRptClass> GetStatNursEventLevel(string beginDate, string endDate);

        /// <summary>
        /// 护理质量与安全（不良事件）上报科室统计
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatNursEventDept")]
        List<EntityNursEventDept> GetStatNursEventDept(string beginDate, string endDate);

        /// <summary>
        ///全院护理上报安全事件汇总表
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatNursEventInstrument")]
        List<EntityNursEventStrument> GetStatNursEventInstrument(List<EntityParm> dicParm);

        /// <summary>
        ///护理皮肤损害安全（不良）事件统计
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatNursEventSkin")]
        List<EntityNursEventSkin> GetStatNursEventSkin(string beginDate, string endDate);

        /// <summary>
        /// 护理皮肤损害安全（不良）事件汇总
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatSkinEventInstrument")]
        List<EntitySkinEventStrument> GetStatSkinEventInstrument(List<EntityParm> dicParm);

        /// <summary>
        /// 职业暴露汇总
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetStatOccupationexp")]
        List<EntityOccupationexp> GetStatOccupationexp(List<EntityParm> dicParm);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetAdversDept")]
        Dictionary<string, string> GetAdversDept();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetAdversType")]
        Dictionary<string, string> GetAdversType();

         /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetAdversSkinType")]
        Dictionary<string, string> GetAdversSkinType();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract(Name = "GetSpanTime")]
        Dictionary<string, string> GetSpanTime();

        #endregion

        /// <summary>
        /// 具有编辑权限可查看自己所在科室事件
        /// </summary>
        /// <param name="empNo"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetEventRoleEdit")]
        string GetEventRoleEdit(string empNo);

        /// <summary>
        /// 具有查看职业暴露权限可以看所有职业暴露
        /// </summary>
        /// <param name="empNo"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetZYBLEventRoleLimt")]
        string GetZYBLEventRoleLimt(string empNo);

        /// <summary>
        /// 具有查看删除权限可以看所有职业暴露
        /// </summary>
        /// <param name="empNo"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetEventRoleDel")]
        string GetEventRoleDel(string empNo);

        /// <summary>
        /// 具有查看权限可以看所有不良事件
        /// </summary>
        /// <param name="empNo"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetEventRoleQuery")]
        string GetEventRoleQuery(string empNo);
        

        /// <summary>
        /// 获取科室代码
        /// </summary>
        /// <param name="deptName"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetDeptCode")]
        string GetDeptCode(string deptName);

        #region 感染病例报告
        /// <summary>
        /// 获取病人信息
        /// </summary>
        /// <param name="inpatId">住院号</param>
        /// <returns></returns>
        [OperationContract(Name = "GetPatInfo")]
        List<EntityPatientInfo> GetPatInfo(string inpatId);


        /// <summary>
        /// 获取多耐药菌信息
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetPathogeny")]
        List<EntityPathogeny> GetPathogeny(decimal rptId);


        /// <summary>
        /// 获取感染部位信息
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetInfectionSite")]
        List<string> GetInfectionSiteSource();

        /// <summary>
        /// 获取易感因素信息
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetCausationSource")]
        List<string> GetCausationSource();

        /// <summary>
        /// 获取多种耐药菌信息
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetDrugSource")]
        List<string> GetDrugSource();

        
        /// <summary>
        /// 获取标本信息
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetSampleSource")]
        List<string> GetSampleSource();


        /// <summary>
        /// 保存感染病例
        /// </summary>
        /// <param name="EntityInfectionus">病例信息</param>
        /// <param name="lstPathogenyVo">病原送检</param>
        /// <returns></returns>
        [OperationContract(Name = "SaveInfectionus")]
        int SaveInfectionus(EntityInfectionus infectionusVo, List<EntityPathogeny> lstPathogenyVo);


        /// <summary>
        /// 获取感染病例列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetInfectionusList")]
        List<EntityInfectionus> GetInfectionusList(List<EntityParm> dicParm);

        /// <summary>
        /// 获取感染病例数据源 用于打印
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetXrDataSource")]
        List<EntityInfectionus> GetXrDataSource(decimal rptId);

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="EntityInfectionus"></param>
        /// <returns></returns>
        [OperationContract(Name = "ComfirmRpt")]
        int ComfirmRpt(EntityInfectionus vo);

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [OperationContract(Name = "getDeptList")]
        List<EntityDeptList> getDeptList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reporterId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetInfectReportRoleQuery")]
        string GetInfectReportRoleQuery(string empNo);

        #endregion

        #region 传染病报告 

        #region

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        [OperationContract(Name = "SaveZrbbg")]
        int SaveZrbbg(EntityRptZrbbg vo, out decimal rptId);
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetZrbbgList")]
        List<EntityZrbbgDisplay> GetZrbbgList(List<EntityParm> dicParm);
        #endregion

         #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetZrbbg")]
        EntityRptZrbbg GetZrbbg(decimal rptId);

        #endregion

        /// <summary>
        ///  传染病报告(伪删)
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "DelZrbbg")]
        int DelZrbbg(decimal rptId);

        /// <summary>
        ///  更新打印标志
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "UpdateZrbbgPrintFlg")]
        int UpdateZrbbgPrintFlg(decimal rptId);
        
        /// <summary>
        ///  获取传染病报告参数
        /// </summary>
        [OperationContract(Name = "EntityRptZrbbgParm")]
        List<EntityRptZrbbgParm> GetZrbbgParm();

        /// <summary>
        /// RegisterZrbygfk
        /// </summary>
        /// <param name="rptId"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        [OperationContract(Name = "RegisterZrbygfk")]
        int RegisterZrbygfk(decimal rptId, string xmlData);

        /// <summary>
        /// GetRegisterZrbygfk
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetRegisterZrbygfk")]
        string GetRegisterZrbygfk(decimal rptId);
          

        #endregion

        #region 获取所属科室
        /// <summary>
        /// 获取所属科室
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetOwerDeptCode")]
        string GetOwerDeptCode(string empno);
        #endregion

        #region 出院患者随访记录
        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetInterviewVo")]
        EntityOutpatientInterview GetInterviewVo(decimal rptId);
        #endregion

        #region

        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetInterviewPatient")]
        List<EntityPatientInfo> GetInterviewPatient(string cardNo, string deptCode);
        #endregion

        #region

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        [OperationContract(Name = "SaveInterview")]
        int SaveInterview(ref EntityOutpatientInterview vo);
        #endregion


        #region

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetPatInterviewInfo")]
        List<EntityOutpatientInterview> GetPatInterviewInfo(List<EntityParm> dicParm);
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetInterviewList")]
        List<EntityOutpatientInterview> GetInterviewList(List<EntityParm> dicParm);
        #endregion

        [OperationContract(Name = "DelInterview")]
        int DelInterview(decimal rptId);

        [OperationContract(Name = "GetInterviewParm")]
        List<EntityRptInterviewParm> GetInterviewParm();

        #endregion
    }
}
