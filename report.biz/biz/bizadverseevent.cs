using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using weCare.Core.Dac;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Entity;

namespace Report.Biz
{
    /// <summary>
    /// 不良事件 .biz
    /// </summary>
    public class bizAdverseEvent : IDisposable
    {
        #region 查找病人
        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="cardNo">1 门诊卡号; 2 住院号; 3 职工 ; 4 无</param>
        /// <param name="flag">1 门诊； 2 住院； 3 职工；4 无</param>
        /// <returns></returns>
        internal List<EntityPatientInfo> GetPatient(string cardNo, int flag, string deptCode)
        {
            string Sql = string.Empty;
            deptCode = string.Empty;
            List<EntityPatientInfo> lstPat = new List<EntityPatientInfo>();
            SqlHelper svc = null;
            try
            {
                if (flag != 4)
                {
                    svc = new SqlHelper(EnumBiz.onlineDB);
                    IDataParameter[] parm = null;
                    if (flag == 1)
                    {
                        Sql = @"select a.patientid_chr              as pid,
                                   a.lastname_vchr              as patname,
                                   a.sex_chr                    as sex,
                                   a.birth_dat                  as birthday,
                                   a.race_vchr                  as race,
                                   a.idcard_chr                 as idcard,
                                   a.homephone_vchr             as contacttel,
                                   a.contactpersonaddress_vchr  as contactaddr,
                                   b.patientcardid_chr          as cardno,
                                   ''                           as ipNo,
                                   ''                           as ipTimes,
                                   null                         as inDate,
                                   d.code_vchr                  as deptcode,
                                   d.deptname_vchr              as deptname,
                                   null                         as outDate,
                                   null                         as deptcode2,
                                   null                         as deptname2,
                                   ''                           as bedNo,
                                   e1.empno_chr                 as doctCode,
                                   e1.lastname_vchr             as doctName
                              from t_bse_patient a
                             inner join t_bse_patientcard b
                                on a.patientid_chr = b.patientid_chr
                              left join t_opr_outpatientrecipe r
                                on a.patientid_chr = r.patientid_chr
                               and r.recorddate_dat >= ?
                              left join t_bse_deptdesc d
                                on r.diagdept_chr = d.deptid_chr
                              left join t_bse_employee e1
                                on r.diagdr_chr = e1.empid_chr
                             where b.patientcardid_chr = ? 
                               {0}
                             order by r.recorddate_dat desc
                            ";

                        TimeSpan ts = new TimeSpan(365, 0, 0, 0);
                        parm = svc.CreateParm(2);
                        parm[0].Value = DateTime.Now.Subtract(ts);
                        parm[1].Value = cardNo;
                        Sql = string.Format(Sql, (deptCode == string.Empty ? "" : ("and d.code_vchr in (" + deptCode + ")")));
                    }
                    else if (flag == 2)
                    {
                        Sql = @"select a.patientid_chr             as pid,
                                   a.lastname_vchr             as patname,
                                   a.sex_chr                   as sex,
                                   a.birth_dat                 as birthday,
                                   a.race_vchr                  as race,
                                   a.idcard_chr                as idcard,
                                   a.homephone_vchr            as contacttel,
                                   a.contactpersonaddress_vchr as contactaddr,
                                   b.patientcardid_chr         as cardno,
                                   r1.inpatientid_chr          as ipNo,
                                   r1.inpatientcount_int       as ipTimes,
                                   r1.inareadate_dat           as inDate,
                                   d1.code_vchr                as deptcode,
                                   d1.deptname_vchr            as deptname,
                                   r2.outhospital_dat          as outDate,
                                   d2.code_vchr                as deptcode2,
                                   d2.deptname_vchr            as deptname2,
                                   e.bed_no                    as bedNo,
                                   e1.empno_chr                as doctCode,
                                   e1.lastname_vchr            as doctName
                              from t_bse_patient a
                             inner join t_bse_patientcard b
                                on a.patientid_chr = b.patientid_chr
                             inner join t_opr_bih_register r1
                                on a.patientid_chr = r1.patientid_chr
                              left join t_opr_bih_leave r2
                                on r1.registerid_chr = r2.registerid_chr
                              left join t_bse_deptdesc d1
                                on r1.areaid_chr = d1.deptid_chr            --deptid_chr areaid_chr
                              left join t_bse_deptdesc d2
                                on r2.outareaid_chr = d2.deptid_chr   -- outdeptid_chr
                              left join t_bse_bed e
                                on r1.bedid_chr = e.bedid_chr
                              left join t_bse_employee e1
                                on r1.casedoctor_chr = e1.empid_chr
                             where r1.inpatientid_chr = ? 
                               {0}
                             order by r1.inpatientcount_int desc
                            ";

                        parm = svc.CreateParm(1);
                        parm[0].Value = cardNo;
                        Sql = string.Format(Sql, (deptCode == string.Empty ? "" : ("and (d1.code_vchr in (" + deptCode + ") and d2.code_vchr in (" + deptCode + "))")));
                    }
                    else if (flag == 3)
                    {
                        Sql = @"select a.empid_chr as pid,
                                   a.lastname_vchr as patname,
                                   a.sex_chr as sex,
                                   a.birthdate_dat as birthday,
                                   '' as race,
                                   '' as idcard,
                                   a.contactphone_vchr as contacttel,
                                   a.ancestoraddr_vchar as contactaddr,
                                   a.empno_chr as cardno,
                                   a.empno_chr as ipNo,
                                   0 as ipTimes,
                                   null as inDate,
                                   b.code_vchr as deptcode,
                                   b.deptname_vchr as deptname,
                                   null as outDate,
                                   b.code_vchr as deptcode2,
                                   b.deptname_vchr as deptname2,
                                   '' as bedNo,
                                   '' as doctCode,
                                   '' as doctName
                              from t_bse_employee a
                             inner join (select distinct tt.empid_chr,
                                                         tt.deptid_chr,
                                                         tt.code_vchr,
                                                         tt.deptname_vchr
                                           from (select t1.empid_chr,
                                                        t1.deptid_chr,
                                                        t2.code_vchr,
                                                        t2.deptname_vchr
                                                   from t_bse_deptemp t1
                                                  inner join t_bse_deptdesc t2
                                                     on t1.deptid_chr = t2.deptid_chr
                                                  where t1.default_inpatient_dept_int = 1
                                                 union all
                                                 select t1.empid_chr,
                                                        t1.deptid_chr,
                                                        t2.code_vchr,
                                                        t2.deptname_vchr
                                                   from t_bse_deptemp t1
                                                  inner join t_bse_deptdesc t2
                                                     on t1.deptid_chr = t2.deptid_chr
                                                  where t1.default_dept_int = 1
                                                 union all
                                                 select t1.empid_chr,
                                                        t1.deptid_chr,
                                                        t2.code_vchr,
                                                        t2.deptname_vchr
                                                   from t_bse_deptemp t1
                                                  inner join t_bse_deptdesc t2
                                                     on t1.deptid_chr = t2.deptid_chr) tt) b
                                on a.empid_chr = b.empid_chr
                             where a.status_int = 1 and a.empno_chr = ?
                            ";
                        parm = svc.CreateParm(1);
                        parm[0].Value = cardNo;
                    }
                    DataTable dt = svc.GetDataTable(Sql, parm);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        EntityPatientInfo pat = null;
                        foreach (DataRow dr in dt.Rows)
                        {
                            pat = new EntityPatientInfo();
                            pat.pid = dr["pid"].ToString();
                            pat.cardNo = dr["cardno"].ToString();
                            pat.name = dr["patname"].ToString();
                            pat.sex = (dr["sex"].ToString().Trim() == "男" ? "1" : "2");
                            if (dr["birthday"] != DBNull.Value) pat.birth = dr["birthday"].ToString();
                            pat.ID = dr["idcard"].ToString();
                            pat.contAddr = dr["contactaddr"].ToString();
                            pat.contTel = dr["contacttel"].ToString();
                            if (dr["deptcode2"] != DBNull.Value)
                            {
                                pat.deptCode = dr["deptcode2"].ToString();
                                pat.deptName = dr["deptname2"].ToString();
                            }
                            else
                            {
                                pat.deptCode = dr["deptcode"].ToString();
                                pat.deptName = dr["deptname"].ToString();
                            }
                            pat.ipNo = dr["ipNo"].ToString();
                            pat.ipTimes = Function.Int(dr["ipTimes"]);
                            pat.bedNo = dr["bedNo"].ToString();
                            if (dr["inDate"] != DBNull.Value) pat.inDate = Function.Datetime(dr["inDate"]);
                            if (dr["outDate"] != DBNull.Value) pat.outDate = Function.Datetime(dr["outDate"]);
                            pat.doctCode = dr["doctCode"].ToString();
                            pat.doctName = dr["doctName"].ToString();
                            if (string.IsNullOrEmpty(pat.ID)) pat.ID = pat.pid;
                            pat.corp = dr["race"].ToString();
                            lstPat.Add(pat);
                        }
                    }
                }
                else
                {
                    EntityPatientInfo pat = new EntityPatientInfo();
                    pat.pid = "\\";
                    pat.cardNo = "\\";
                    pat.name = "\\";
                    pat.sex = "\\";
                    pat.birth = string.Empty;
                    pat.ID = "\\";
                    pat.contAddr = "\\";
                    pat.contTel = "\\";
                    pat.deptCode = "\\";
                    pat.deptName = "\\";
                    pat.ipNo = "\\";
                    pat.ipTimes = 0;
                    pat.bedNo = "\\";
                    pat.inDate = null;
                    pat.outDate = null;
                    pat.doctCode = "\\";
                    pat.doctName = "\\";
                    pat.ID = pat.pid;
                    lstPat.Add(pat);
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return lstPat;
        }
        #endregion

        #region 不良事件

        #region 获取不良事件列表
        /// <summary>
        /// 获取不良事件列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        internal List<EntityEventDisplay> GetEventList(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            List<EntityEventDisplay> data = new List<EntityEventDisplay>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select t.rptId,
                               t.eventId,
                               t.reportTime,
                               t.reportOperCode,
                               t.reportOperName,
                               t.reportDeptCode,
                               c.deptname_vchr as reportDeptName,
                               t.reportType,
                               t.eventcode,
                               t.eventname,
                               t.patno,
                               t.patname,
                               t.patsex,
                               t.birthday,
                               t.contacttel,
                               t.deptcode,
                               extractvalue(d.xmldata, '/FormData/X236') as XZQM,
                               extractvalue(d.xmldata, '/FormData/X223') as HCQM,
                               extractvalue(d.xmldata, '/FormData/X238') as HLQM,
                               b.deptname_vchr as deptName
                          from icare.rptEvent t
                          left join t_bse_deptdesc b
                            on t.deptcode = b.CODE_VCHR
                          left join t_bse_deptdesc c 
                           on t.reportdeptcode = c.code_vchr
                           left join icare.rpteventdata d
                           on t.rptid = d.rptid
                         where t.status = 1 
                           and t.eventid = ?
                           ";

                Sql1 = @"select t.rptId,
                               t.eventId,
                               t.reportTime,
                               t.reportOperCode,
                               t.reportOperName,
                               t.reportDeptCode,
                               c.deptname_vchr as reportDeptName,
                               t.reportType,
                               t.eventcode,
                               t.eventname,
                               t.patno,
                               t.patname,
                               t.patsex,
                               t.birthday,
                               t.contacttel,
                               t.deptcode,
                               b.deptname_vchr as deptName,
                               extractvalue(d.xmldata, '/FormData/X236') as XZQM,
                               extractvalue(d.xmldata, '/FormData/X223') as HCQM,
                               extractvalue(d.xmldata, '/FormData/X238') as HLQM
                          from rptEvent t
                         inner join t_bse_patientcard a
                            on t.patno = a.patientcardid_chr
                         inner join t_bse_patient p
                            on a.patientid_chr = p.patientid_chr
                          left join t_bse_deptdesc b
                            on t.deptcode = b.CODE_VCHR
                          left join t_bse_deptdesc c 
                           on t.reportdeptcode = c.code_vchr
                          left join icare.rpteventdata d
                           on t.rptid = d.rptid
                         where t.status = 1 
                           and t.eventid = ?
                           ";
                #endregion

                #region 条件

                string strSub = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();
                // 默认参数
                IDataParameter parm = svc.CreateParm();
                parm.Value = dicParm.FirstOrDefault(t => t.key == "eventId").value;
                lstParm.Add(parm);

                foreach (EntityParm po in dicParm)
                {
                    parm = svc.CreateParm();
                    string keyValue = po.value;
                    parm.Value = keyValue;
                    switch (po.key)
                    {
                        case "reportDate":
                            IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0] + " 00:00:00";
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            strSub += " and (t.reportTime between ? and ?)";
                            break;
                        case "deptCode":
                            if (keyValue.IndexOf("','") > 0)
                            {
                                strSub += " and (t.reportDeptCode in (" + keyValue + "))";//and (t.deptCode in (" + keyValue + "))
                            }
                            else
                            {
                                parm.Value = parm.Value.ToString().Replace("'", "");
                                lstParm.Add(parm);
                                strSub += " and (t.reportDeptCode = ?)";//and (t.deptCode = ?)
                            }
                            break;
                        case "cardNo":
                            lstParm.Add(parm);
                            strSub += " and (t.patno= ?)";
                            break;
                        case "patName":
                            Sql = Sql1;
                            parm.Value = "%" + keyValue + "%";
                            lstParm.Add(parm);
                            strSub += " and (p.lastname_vchr like ?)";
                            break;
                        case "areaStr":
                            strSub += " and (t.reportDeptCode in (" + keyValue + ") or t.reportDeptCode is null";//" and (t.deptcode in (" + keyValue + ") or t.deptcode is null";
                            break;
                        case "selfId":
                            strSub += " or t.reportopercode = '" + keyValue + "')";
                            break;
                        case "limitId":
                            strSub += " and (t.reportopercode = '" + keyValue + "'  or t.reportopercode is null )";
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                #region 赋值

                // 组合条件
                Sql += strSub;
                Sql += " order by t.reportTime";
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());
                if (dt != null)
                {
                    EntityEventDisplay vo = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vo = new EntityEventDisplay();
                        vo.rptId = dr["rptId"].ToString();
                        vo.reportTime = dr["reportTime"].ToString();
                        vo.reportOperCode = dr["reportOperCode"].ToString();
                        vo.reportOperName = dr["reportOperName"].ToString();
                        vo.reportDeptName = dr["reportDeptName"].ToString();
                        vo.reportType = dr["reportType"].ToString();
                        vo.eventCode = dr["eventCode"].ToString();
                        vo.eventName = dr["eventName"].ToString();
                        vo.patNo = dr["patNo"].ToString();
                        vo.patName = dr["patName"].ToString();
                        if (dr["patSex"].ToString() == "1")
                            vo.patSex = "男";
                        else if (dr["patSex"].ToString() == "2")
                            vo.patSex = "女";
                        else
                            vo.patSex = "\\";

                        //vo.patSex = dr["patSex"].ToString() == "1" ? "男" : "女";
                        if (dr["birthday"] != DBNull.Value)
                        {
                            vo.patBirthDay = dr["birthday"].ToString();
                            vo.patAge = CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                        }
                        vo.contactTel = dr["contactTel"].ToString();
                        vo.deptName = dr["deptName"].ToString();
                        vo.HCQM = dr["HCQM"].ToString();
                        vo.HLQM = dr["HLQM"].ToString();
                        vo.XZQM = dr["XZQM"].ToString();
                        data.Add(vo);
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return data;
        }
        #endregion

        #region 获取不良事件实例(vo)
        /// <summary>
        /// 获取不良事件实例(vo)
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal EntityRptEvent GetEvent(decimal rptId)
        {
            EntityRptEvent vo = new EntityRptEvent();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                vo.rptId = rptId;
                vo = EntityTools.ConvertToEntity<EntityRptEvent>(svc.SelectPk(vo));

                EntityRptEventData dataVo = new EntityRptEventData();
                dataVo.rptId = rptId;
                dataVo = EntityTools.ConvertToEntity<EntityRptEventData>(svc.SelectPk(dataVo));
                vo.xmlData = dataVo.xmlData;
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return vo;
        }
        #endregion

        #region 获取不良事件统计列表
        /// <summary>
        /// 获取不良事件列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        internal List<EntityEventDisplay> GetEventListAll(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            List<EntityEventDisplay> data = new List<EntityEventDisplay>();
            SqlHelper svc = null;
            string SJDJ = string.Empty;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select t.rptId,
                               t.eventId,
                               t.reportTime,
                               t.reportOperCode,
                               t.reportOperName,
                               t.reportDeptCode,
                               c.deptname_vchr as reportDeptName,
                               t.reportType,
                               t.eventcode,
                               t.eventname,
                               t.patno,
                               t.patname,
                               t.patsex,
                               t.birthday,
                               t.contacttel,
                               t.deptcode,
                               extractvalue(d.xmldata, '/FormData/X236') as XZQM,
                               extractvalue(d.xmldata, '/FormData/X223') as HCQM,
                               extractvalue(d.xmldata, '/FormData/X238') as HLQM,
                               extractvalue(d.xmldata, '/FormData/X007') as HLSJDJ,
                               extractvalue(d.xmldata, '/FormData/A100') as ZYBLSJDJ,
                               extractvalue(d.xmldata, '/FormData/X052') as YLSJDJ_I,
                               extractvalue(d.xmldata, '/FormData/X053') as YLSJDJ_II,
                               extractvalue(d.xmldata, '/FormData/X054') as YLSJDJ_III,
                               extractvalue(d.xmldata, '/FormData/X055') as YLSJDJ_IV,

                               extractvalue(d.xmldata, '/FormData/X005') as eventTimeHl,
                               extractvalue(d.xmldata, '/FormData/X004') as eventTimePf,
                               extractvalue(d.xmldata, '/FormData/A002') as eventTimeSxjl,
                               extractvalue(d.xmldata, '/FormData/A002') as eventTimeSxhb,
                               extractvalue(d.xmldata, '/FormData/X091') as eventTimeYp,
                               extractvalue(d.xmldata, '/FormData/X004') as eventTimePfyw,
                               extractvalue(d.xmldata, '/FormData/X002') as eventTimeYlaq,
                               extractvalue(d.xmldata, '/FormData/X028') as eventTimeKyjx,
                               extractvalue(d.xmldata, '/FormData/X037') as eventTimeZybl,
                               b.deptname_vchr as deptName
                          from icare.rptEvent t
                          left join t_bse_deptdesc b
                            on t.deptcode = b.CODE_VCHR
                          left join t_bse_deptdesc c 
                           on t.reportdeptcode = c.code_vchr
                           left join icare.rpteventdata d
                           on t.rptid = d.rptid
                         where t.status = 1 and t.eventid <> '21'
                           
                           ";

                Sql1 = @"select t.rptId,
                               t.eventId,
                               t.reportTime,
                               t.reportOperCode,
                               t.reportOperName,
                               t.reportDeptCode,
                               c.deptname_vchr as reportDeptName,
                               t.reportType,
                               t.eventcode,
                               t.eventname,
                               t.patno,
                               t.patname,
                               t.patsex,
                               t.birthday,
                               t.contacttel,
                               t.deptcode,
                               b.deptname_vchr as deptName,
                               extractvalue(d.xmldata, '/FormData/X236') as XZQM,
                               extractvalue(d.xmldata, '/FormData/X223') as HCQM,
                               extractvalue(d.xmldata, '/FormData/X238') as HLQM,
                               extractvalue(d.xmldata, '/FormData/X007') as HLSJDJ,
                               extractvalue(d.xmldata, '/FormData/A100') as ZYBLSJDJ,
                               extractvalue(d.xmldata, '/FormData/X052') as YLSJDJ_I,
                               extractvalue(d.xmldata, '/FormData/X053') as YLSJDJ_II,
                               extractvalue(d.xmldata, '/FormData/X054') as YLSJDJ_III,
                               extractvalue(d.xmldata, '/FormData/X055') as YLSJDJ_IV,
                               extractvalue(d.xmldata, '/FormData/X005') as eventTimeHl,
                               extractvalue(d.xmldata, '/FormData/X004') as eventTimePf,
                               extractvalue(d.xmldata, '/FormData/A002') as eventTimeSxjl,
                               extractvalue(d.xmldata, '/FormData/A002') as eventTimeSxhb,
                               extractvalue(d.xmldata, '/FormData/X091') as eventTimeYp,
                               extractvalue(d.xmldata, '/FormData/X004') as eventTimePfyw,
                               extractvalue(d.xmldata, '/FormData/X002') as eventTimeYlaq,
                               extractvalue(d.xmldata, '/FormData/X028') as eventTimeKyjx,
                               extractvalue(d.xmldata, '/FormData/X037') as eventTimeZybl
                          from rptEvent t
                         inner join t_bse_patientcard a
                            on t.patno = a.patientcardid_chr
                         inner join t_bse_patient p
                            on a.patientid_chr = p.patientid_chr
                          left join t_bse_deptdesc b
                            on t.deptcode = b.CODE_VCHR
                          left join t_bse_deptdesc c 
                           on t.reportdeptcode = c.code_vchr
                          left join icare.rpteventdata d
                           on t.rptid = d.rptid
                         where t.status = 1 and t.eventid <> '21'
                           ";
                #endregion

                #region 条件

                string strSub = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();
                // 默认参数
                IDataParameter parm = svc.CreateParm();

                foreach (EntityParm po in dicParm)
                {
                    parm = svc.CreateParm();
                    string keyValue = po.value;
                    parm.Value = keyValue;
                    switch (po.key)
                    {
                        case "reportDate":
                            IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0] + " 00:00:00";
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            strSub += " and (t.reportTime between ? and ? )";
                            break;
                        case "deptCode":
                            if (keyValue.IndexOf("','") > 0)
                            {
                                strSub += " and (t.reportDeptCode in (" + keyValue + "))";//and (t.deptCode in (" + keyValue + "))
                            }
                            else
                            {
                                parm.Value = parm.Value.ToString().Replace("'", "");
                                lstParm.Add(parm);
                                strSub += " and (t.reportDeptCode = ?)";//and (t.deptCode = ?)
                            }
                            break;
                        case "reporter":
                            strSub += " and t.reportopername like '%" + keyValue + "%'";
                            break;
                        case "eventId":
                            strSub += " and t.eventid = " + keyValue;
                            break;
                        case "level":
                            SJDJ = keyValue;
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                #region 赋值

                // 组合条件
                Sql += strSub;
                Sql += " order by t.reportTime";
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());
                if (dt != null)
                {
                    EntityEventDisplay vo = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vo = new EntityEventDisplay();
                        vo.eventId = dr["eventId"].ToString();
                        vo.rptId = dr["rptId"].ToString();
                        if (dr["reportTime"] != DBNull.Value)
                            vo.reportTime = Function.Datetime(dr["reportTime"]).ToString("yyyy-MM-dd HH:mm");
                        vo.reportOperCode = dr["reportOperCode"].ToString();
                        vo.reportOperName = dr["reportOperName"].ToString();
                        vo.reportDeptName = dr["reportDeptName"].ToString();
                        vo.reportType = dr["reportType"].ToString();
                        vo.eventCode = dr["eventCode"].ToString();
                        vo.eventName = dr["eventName"].ToString();
                        vo.patNo = dr["patNo"].ToString();
                        vo.patName = dr["patName"].ToString();
                        if (dr["patSex"].ToString() == "1")
                            vo.patSex = "男";
                        else if (dr["patSex"].ToString() == "2")
                            vo.patSex = "女";
                        else
                            vo.patSex = "\\";

                        if (dr["birthday"] != DBNull.Value)
                        {
                            vo.patBirthDay = dr["birthday"].ToString();
                            vo.patAge = CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                        }
                        vo.contactTel = dr["contactTel"].ToString();
                        vo.deptName = dr["deptName"].ToString();
                        vo.HCQM = dr["HCQM"].ToString();
                        vo.HLQM = dr["HLQM"].ToString();
                        vo.XZQM = dr["XZQM"].ToString();

                        #region 事件等级
                        if (vo.eventId == "19")
                        {
                            vo.eventLevel = dr["hlsjdj"].ToString();
                        }

                        if (vo.eventId == "17")
                        {
                            vo.eventLevel = dr["ZYBLSJDJ"].ToString();
                        }

                        if (vo.eventId == "11")
                        {
                            if (dr["YLSJDJ_I"].ToString() == "1")
                                vo.eventLevel = "I";
                            if (dr["YLSJDJ_II"].ToString() == "1")
                                vo.eventLevel = "II";
                            if (dr["YLSJDJ_III"].ToString() == "1")
                                vo.eventLevel = "III";
                            if (dr["YLSJDJ_IV"].ToString() == "1")
                                vo.eventLevel = "IV";
                        }

                        if (!string.IsNullOrEmpty(SJDJ))
                        {
                            if (vo.eventId == "19")
                            {
                                if (SJDJ != dr["hlsjdj"].ToString())
                                    continue;
                            }
                            else if (vo.eventId == "11")
                            {
                                if (SJDJ == "I" && dr["YLSJDJ_I"].ToString() != "1")
                                    continue;
                                if (SJDJ == "II" && dr["YLSJDJ_II"].ToString() != "1")
                                    continue;
                                if (SJDJ == "III" && dr["YLSJDJ_III"].ToString() != "1")
                                    continue;
                                if (SJDJ == "IV" && dr["YLSJDJ_IV"].ToString() != "1")
                                    continue;
                            }
                            else if (vo.eventId == "17")
                            {
                                if (SJDJ != dr["ZYBLSJDJ"].ToString())
                                    continue;
                            }
                            else
                                continue;
                        }
                        #endregion

                        #region 事件发生时间
                        if (vo.eventId == "19")  //护理事件
                        {
                            vo.eventTime = dr["eventTimeHl"].ToString();
                        }
                        if (vo.eventId == "20")  //皮肤事件院内
                        {
                            vo.eventTime = dr["eventTimePf"].ToString();
                        }
                        if (vo.eventId == "15")  //输血不良反应记录表
                        {
                            vo.eventTime = dr["eventTimeSxjl"].ToString();
                        }
                        if (vo.eventId == "16")  //输血不良反应回报单
                        {
                            vo.eventTime = dr["eventTimeSxhb"].ToString();
                        }
                        if (vo.eventId == "13")  //药品不良反应
                        {
                            vo.eventTime = dr["eventTimeYp"].ToString();
                        }
                        if (vo.eventId == "21")  //皮肤事件（院外）
                        {
                            vo.eventTime = dr["eventTimePfyw"].ToString();
                        }
                        if (vo.eventId == "11")  //医疗安全
                        {
                            vo.eventTime = dr["eventTimeYlaq"].ToString();
                        }
                        if (vo.eventId == "12")  //可疑医疗器械
                        {
                            vo.eventTime = dr["eventTimeKyjx"].ToString();
                        }
                        if (vo.eventId == "17")  //可疑医疗器械
                        {
                            vo.eventTime = dr["eventTimeZybl"].ToString();
                        }
                        #endregion
                        data.Add(vo);
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return data;
        }
        #endregion

        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="vo"></param>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal int SaveEvent(EntityRptEvent vo, out decimal rptId)
        {
            int affectRows = 0;
            rptId = 0;
            SqlHelper svc = null;
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                svc = new SqlHelper(EnumBiz.onlineDB);
                EntityRptEventData voData = new EntityRptEventData();
                if (vo.rptId <= 0)  // new
                {
                    rptId = svc.GetNextID(EntityTools.GetTableName(vo), EntityTools.GetFieldName(vo, EntityRptEvent.Columns.rptId));
                    vo.rptId = rptId;
                    lstParm.Add(svc.GetInsertParm(vo));

                    voData.rptId = rptId;
                    voData.xmlData = vo.xmlData;
                    lstParm.Add(svc.GetInsertParm(voData));
                }
                else                // edit
                {
                    lstParm.Add(svc.GetUpdateParm(vo, new List<string>() {EntityRptEvent.Columns.reportTime, EntityRptEvent.Columns.reportOperCode, EntityRptEvent.Columns.reportOperName, 
                                                                          EntityRptEvent.Columns.eventCode, EntityRptEvent.Columns.eventName, EntityRptEvent.Columns.patType, EntityRptEvent.Columns.patNo,  
                                                                          EntityRptEvent.Columns.patName, EntityRptEvent.Columns.patSex, EntityRptEvent.Columns.birthday, EntityRptEvent.Columns.idCard,  
                                                                          EntityRptEvent.Columns.contactAddr, EntityRptEvent.Columns.contactTel, EntityRptEvent.Columns.deptCode, EntityRptEvent.Columns.formId,  
                                                                          EntityRptEvent.Columns.operCode, EntityRptEvent.Columns.recordDate, EntityRptEvent.Columns.status,EntityRptEvent.Columns.reportDeptCode},
                                                      new List<string>() { EntityRptEvent.Columns.rptId }));

                    voData.rptId = vo.rptId;
                    voData.xmlData = vo.xmlData;
                    lstParm.Add(svc.GetUpdateParm(voData, new List<string>() { EntityRptEventData.Columns.xmlData }, new List<string>() { EntityRptEventData.Columns.rptId }));

                    rptId = vo.rptId;
                }
                affectRows = svc.Commit(lstParm);
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }
            return affectRows;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal int DelEvent(decimal rptId)
        {
            int affectRows = 0;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                EntityRptEvent vo = new EntityRptEvent();
                vo.rptId = rptId;
                vo.status = 0;
                affectRows = svc.Commit(svc.GetUpdateParm(vo, new List<string>() { EntityRptEvent.Columns.status }, new List<string>() { EntityRptEvent.Columns.rptId }));
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }
            return affectRows;
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
            string Sql = string.Empty;
            List<EntityStatInstrument> data = new List<EntityStatInstrument>();
            SqlHelper svc = null;
            try
            {
                Sql = @"select a.reportTime,
                               a.reportopername,
                               a.patname,
                               a.deptcode,
                               d.deptname_vchr as deptName,
                               extractvalue(b.xmldata, '/FormData/X028') as eventTime,
                               extractvalue(b.xmldata, '/FormData/X027') as eventContent
                          from rptevent a
                         inner join rpteventdata b
                            on a.rptid = b.rptid
                         inner join t_bse_deptdesc d
                            on a.deptcode = d.deptid_chr
                         where a.eventid = 12
                           and a.status = 1 
                           and (a.reportTime between ? and ?) 
                           {0}
                         order by a.deptcode, a.reporttime";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                if (string.IsNullOrEmpty(deptCode))
                {
                    Sql = string.Format(Sql, "");
                    parm = svc.CreateParm(2);
                    parm[0].Value = beginTime;
                    parm[1].Value = endTime;
                }
                else
                {
                    Sql = string.Format(Sql, " and a.deptCode = ? ");
                    parm = svc.CreateParm(3);
                    parm[0].Value = beginTime;
                    parm[1].Value = endTime;
                    parm[2].Value = deptCode;
                }
                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int n = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        data.Add(new EntityStatInstrument()
                            {
                                sortNo = ++n,
                                eventTime = dr["eventTime"].ToString(),
                                eventContent = dr["eventContent"].ToString(),
                                reportTime = dr["reportTime"].ToString(),
                                reportOperName = dr["reportopername"].ToString(),
                                deptName = dr["deptName"].ToString(),
                                patientName = dr["patname"].ToString()
                            });
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return data;
        }
        #endregion

        #endregion

        #region  事件参数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetEventDicparm(decimal classid)
        {
            string Sql = string.Empty;
            Dictionary<string, string> data = new Dictionary<string, string>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select itemcode, itemname from diccommon where classid = ? and status = 1";

                IDataParameter[] parm = null;
                parm = svc.CreateParm(1);
                parm[0].Value = classid;
                DataTable dt = svc.GetDataTable(Sql, parm);

                #endregion

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        data.Add(dr["itemcode"].ToString(), dr["itemname"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return data;
        }
        #endregion

        #region 医疗安全（不良）事件统计表

        #region 按级别
        /// <summary>
        /// 医疗安全（不良）事件统计表（按级别）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<EntityEventStat> GetStatEventLevel(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            List<EntityEventStat> data = new List<EntityEventStat>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                Sql = @"select a.rptid,
                        a.reporttime,
                        extractvalue(b.xmldata, '/FormData/X052') as I,
                        extractvalue(b.xmldata, '/FormData/X053') as II,
                        extractvalue(b.xmldata, '/FormData/X054') as III,
                        extractvalue(b.xmldata, '/FormData/X055') as IV
                        from rptEvent a 
                        left join  rpteventdata b 
                        on a.rptid = b.rptid
                        where a.formid = 12
                        and a.status = 1
                        and a.reporttime between ? and ?
                        order by a.rptid ";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);
                
                #endregion
                
                #region 赋值
                string month = string.Empty;
                string xmlData = string.Empty;
                Dictionary<string, string> dicData = new Dictionary<string, string>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        month = Function.Datetime(dr["reporttime"].ToString()).ToString("yyyy-MM");

                        if (data.Any(t => t.TJYF == month))
                        {
                            #region 累计
                            EntityEventStat voClone = data.FirstOrDefault(t => t.TJYF == month);
                            voClone.TJYF = month;
                            //医疗安全不良事件 I级事件
                            if (dr["I"].ToString()=="1")
                                voClone.IJSJ += 1;
                            //医疗安全不良事件 II级事件
                            if (dr["II"].ToString() == "1")
                                voClone.IIJSJ += 1;
                            //医疗安全不良事件 III级事件
                            if (dr["III"].ToString() == "1")
                                voClone.IIIJSJ += 1;
                            //医疗安全不良事件 IV级事件
                            if (dr["IV"].ToString() == "1")
                                voClone.IVJSJ += 1;
                            #endregion
                        }
                        else
                        {
                            #region vo
                            EntityEventStat vo = new EntityEventStat();
                            /// 月份
                            vo.TJYF = month;
                            //医疗安全不良事件 I级事件
                            if (dr["I"].ToString() == "1")
                                vo.IJSJ = 1;
                            //医疗安全不良事件 II级事件
                            if (dr["II"].ToString() == "1")
                                vo.IIJSJ = 1;
                            //医疗安全不良事件 III级事件
                            if (dr["III"].ToString() == "1")
                                vo.IIIJSJ = 1;
                            //医疗安全不良事件 IV级事件
                            if (dr["IV"].ToString() == "1")
                                vo.IVJSJ = 1;

                            #endregion
                            data.Add(vo);
                        }
                    }
                }
                #endregion

                #region 合计

                foreach (EntityEventStat item in data)
                {
                    item.XJ = item.IJSJ + item.IIJSJ + item.IIIJSJ + item.IVJSJ;
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }
        #endregion

        #region 按类别
        /// <summary>
        /// 医疗安全（不良）事件统计表（按级别）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// 
        public List<EntityEventStat> GetStatEventClass(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            List<EntityEventStat> data = new List<EntityEventStat>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                Sql = @"select a.rptid,
                        a.reporttime,
                        extractvalue(b.xmldata, '/FormData/X033') as XXZDCW,
                        extractvalue(b.xmldata, '/FormData/X034') as ZLCW,
                        extractvalue(b.xmldata, '/FormData/X035') as FFJSCW,
                        extractvalue(b.xmldata, '/FormData/X036') as YWTJFFCW,
                        extractvalue(b.xmldata, '/FormData/X037') as SXSJ,
                        extractvalue(b.xmldata, '/FormData/X038') as SBQXSYSJ,
                        extractvalue(b.xmldata, '/FormData/X039') as DGCZ,
                        extractvalue(b.xmldata, '/FormData/X040') as YLJSJCSJ,
                        extractvalue(b.xmldata, '/FormData/X041') as JCHLSJ,
                        extractvalue(b.xmldata, '/FormData/X042') as YYYYSSJ,
                        extractvalue(b.xmldata, '/FormData/X043') as WPYSSJ,
                        extractvalue(b.xmldata, '/FormData/X044') as FSAQSJ,
                        extractvalue(b.xmldata, '/FormData/X045') as ZLJLSJ,
                        extractvalue(b.xmldata, '/FormData/X046') as ZQTYSJ,
                        extractvalue(b.xmldata, '/FormData/X047') as FYQSJ,
                        extractvalue(b.xmldata, '/FormData/X048') as YHAQSJ,
                        extractvalue(b.xmldata, '/FormData/X049') as BZWSJ,
                        extractvalue(b.xmldata, '/FormData/X050') as BFZ,
                        extractvalue(b.xmldata, '/FormData/X051') as QTSJ
                        from rptEvent a 
                        left join  rpteventdata b 
                        on a.rptid = b.rptid
                        where a.formid = 12
                        and a.status = 1
                        and a.reporttime between ? and ?
                        order by a.rptid ";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                #endregion

                #region 赋值
                string month = string.Empty;
                string xmlData = string.Empty;
                Dictionary<string, string> dicData = new Dictionary<string, string>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        month = Function.Datetime(dr["reporttime"].ToString()).ToString("yyyy-MM");

                        if (data.Any(t => t.TJYF == month))
                        {
                            #region 累计
                            EntityEventStat voClone = data.FirstOrDefault(t => t.TJYF == month);
                            voClone.TJYF = month;
                            //医疗安全不良事件 消息传递错误 
                            if (dr["XXZDCW"].ToString() == "1")
                                voClone.XXZDCW += 1;
                            //医疗安全不良事件 治疗错误
                            if (dr["ZLCW"].ToString() == "1")
                                voClone.ZLCW += 1;
                            //医疗安全不良事件 方法/技术错误
                            if (dr["FFJSCW"].ToString() == "1")
                                voClone.FFJSCW += 1;
                            //医疗安全不良事件 药物调剂分发错误 
                            if (dr["YWTJFFCW"].ToString() == "1")
                                voClone.YWTJFFCW += 1;
                            //医疗安全不良事件 输血事件 
                            if (dr["SXSJ"].ToString() == "1")
                                voClone.SXSJ += 1;
                            //医疗安全不良事件 设备器械使用事件
                            if (dr["SBQXSYSJ"].ToString() == "1")
                                voClone.SBQXSYSJ += 1;
                            //医疗安全不良事件 导管操作事件
                            if (dr["DGCZ"].ToString() == "1")
                                voClone.DGCZ += 1;
                            //医疗安全不良事件 医疗技术检查事件
                            if (dr["YLJSJCSJ"].ToString() == "1")
                                voClone.YLJSJCSJ += 1;
                            //医疗安全不良事件 基础护理事件
                            if (dr["JCHLSJ"].ToString() == "1")
                                voClone.JCHLSJ += 1;
                            //医疗安全不良事件 营养与饮食事件
                            if (dr["YYYYSSJ"].ToString() == "1")
                                voClone.YYYYSSJ += 1;
                            //医疗安全不良事件 物品运送事件 
                            if (dr["WPYSSJ"].ToString() == "1")
                                voClone.WPYSSJ += 1;
                            //医疗安全不良事件 放射安全事件
                            if (dr["FSAQSJ"].ToString() == "1")
                                voClone.FSAQSJ += 1;
                            //医疗安全不良事件 诊疗记录事件
                            if (dr["ZLJLSJ"].ToString() == "1")
                                voClone.ZLJLSJ += 1;
                            //医疗安全不良事件 知情同意事件
                            if (dr["ZQTYSJ"].ToString() == "1")
                                voClone.ZQTYSJ += 1;
                            //医疗安全不良事件 非预期事件 
                            if (dr["FYQSJ"].ToString() == "1")
                                voClone.FYQSJ += 1;
                            //医疗安全不良事件 医护安全事件
                            if (dr["YHAQSJ"].ToString() == "1")
                                voClone.YHAQSJ += 1;
                            //医疗安全不良事件 不作为事件 
                            if (dr["BZWSJ"].ToString() == "1")
                                voClone.BZWSJ += 1;
                            //医疗安全不良事件 不作为事件 
                            if (dr["BFZ"].ToString() == "1")
                                voClone.BFZ += 1;
                            //医疗安全不良事件 其他事件
                            if (dr["QTSJ"].ToString() == "1")
                                voClone.QTSJ += 1;
                            #endregion
                        }
                        else
                        {
                            #region vo
                            EntityEventStat vo = new EntityEventStat();
                            /// 月份
                            vo.TJYF = month;
                            //医疗安全不良事件 消息传递错误 
                            if (dr["XXZDCW"].ToString() == "1")
                                vo.XXZDCW = 1;
                            //医疗安全不良事件 治疗错误
                            if (dr["FFJSCW"].ToString() == "1")
                                vo.ZLCW = 1;
                            //医疗安全不良事件 方法/技术错误
                            if (dr["FFJSCW"].ToString() == "1")
                                vo.FFJSCW = 1;
                            //医疗安全不良事件 药物调剂分发错误 
                            if (dr["YWTJFFCW"].ToString() == "1")
                                vo.YWTJFFCW = 1;
                            //医疗安全不良事件 输血事件 
                            if (dr["SXSJ"].ToString() == "1")
                                vo.SXSJ = 1;
                            //医疗安全不良事件 设备器械使用事件
                            if (dr["SBQXSYSJ"].ToString() == "1")
                                vo.SBQXSYSJ = 1;
                            //医疗安全不良事件 导管操作事件
                            if (dr["DGCZ"].ToString() == "1")
                                vo.DGCZ = 1;
                            //医疗安全不良事件 医疗技术检查事件
                            if (dr["YLJSJCSJ"].ToString() == "1")
                                vo.YLJSJCSJ = 1;
                            //医疗安全不良事件 基础护理事件
                            if (dr["JCHLSJ"].ToString() == "1")
                                vo.JCHLSJ = 1;
                            //医疗安全不良事件 营养与饮食事件
                            if (dr["YYYYSSJ"].ToString() == "1")
                                vo.YYYYSSJ = 1;
                            //医疗安全不良事件 物品运送事件 
                            if (dr["WPYSSJ"].ToString() == "1")
                                vo.WPYSSJ = 1;
                            //医疗安全不良事件 放射安全事件
                            if (dr["FSAQSJ"].ToString() == "1")
                                vo.FSAQSJ = 1;
                            //医疗安全不良事件 诊疗记录事件
                            if (dr["ZLJLSJ"].ToString() == "1")
                                vo.ZLJLSJ = 1;
                            //医疗安全不良事件 知情同意事件
                            if (dr["ZQTYSJ"].ToString() == "1")
                                vo.ZQTYSJ = 1;
                            //医疗安全不良事件 非预期事件 
                            if (dr["FYQSJ"].ToString() == "1")
                                vo.FYQSJ = 1;
                            //医疗安全不良事件 医护安全事件
                            if (dr["YHAQSJ"].ToString() == "1")
                                vo.YHAQSJ = 1;
                            //医疗安全不良事件 不作为事件 
                            if (dr["BZWSJ"].ToString() == "1")
                                vo.BZWSJ = 1;
                            //医疗安全不良事件 不作为事件 
                            if (dr["BFZ"].ToString() == "1")
                                vo.BFZ = 1;
                            //医疗安全不良事件 其他事件
                            if (dr["QTSJ"].ToString() == "1")
                                vo.QTSJ = 1;

                            #endregion
                            data.Add(vo);
                        }
                    }
                }
                #endregion

                #region 合计

                foreach (EntityEventStat item in data)
                {
                    item.XJ = item.XXZDCW + item.ZLCW + item.FFJSCW + item.YWTJFFCW +
                        item.SXSJ + item.SBQXSYSJ + item.DGCZ + item.YLJSJCSJ +
                        item.JCHLSJ + item.YYYYSSJ + item.WPYSSJ + item.FSAQSJ +
                        item.ZLJLSJ + item.ZQTYSJ + item.FYQSJ + item.YHAQSJ +
                        item.BZWSJ + item.QTSJ + item.BFZ ;
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }
        #endregion

        #region 按科室
        /// <summary>
        /// 医疗安全（不良）事件统计表（按科室）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// 
        public List<EntityEventStat> GetStatEventDept(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            List<EntityEventStat> data = new List<EntityEventStat>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                Sql = @"select a.rptid,
                        a.reporttime,
                        d.deptid_chr,
                        d.code_vchr,
                        d.deptname_vchr
                        from rptEvent a 
                        left join  rpteventdata b 
                        on a.rptid = b.rptid
                        inner join t_bse_deptdesc d
                        on a.reportdeptcode = d.code_vchr
                        where a.formid = 12
                        and a.status = 1
                        and a.reporttime between ? and ?
                        order by a.rptid ";// 

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                #endregion

                #region 赋值
                string month = string.Empty;
                string xmlData = string.Empty;
                Dictionary<string, string> dicData = new Dictionary<string, string>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        month = Function.Datetime(dr["reporttime"].ToString()).ToString("yyyy-MM");

                        if (data.Any(t => t.TJYF == month))
                        {
                            #region 累计
                            EntityEventStat voClone = data.FirstOrDefault(t => t.TJYF == month);
                            voClone.TJYF = month;
                            string deptcode = dr["code_vchr"].ToString().Trim();
                            string deptArea = JudgeDeptArea(deptcode.Trim());
                            //内一区
                            if(deptArea == "NYQ")
                                voClone.NYQ += 1;
                            //内二区
                            else if (deptArea == "NEQ")
                                voClone.NEQ += 1;
                            //普外科
                            else if (deptArea == "PWK")
                                voClone.PWK += 1;
                            //泌尿外科
                            else if (deptArea == "MNWK")
                                voClone.MNWK += 1;
                            //手外科
                            else if (deptArea == "SWK")
                                voClone.SWK += 1;
                            //骨外科
                            else if (deptArea == "GWK")
                                voClone.GWK += 1;
                            //妇产科
                            else if (deptArea == "FCK")
                                voClone.FCK += 1;
                            //儿科
                            else if (deptArea == "EK")
                                voClone.EK += 1;
                            //重症医学科
                            else if (deptArea == "CZYXK")
                                voClone.CZYXK += 1;
                            //急诊科
                            else if (deptArea == "JZK")
                                voClone.JZK += 1;
                            //麻醉科
                            else if (deptArea == "MZK")
                                voClone.MZK += 1;
                            //中医康复科
                            else if (deptArea == "ZYKFK")
                                voClone.ZYKFK += 1;
                            //口腔科
                            else if (deptArea == "KJK")
                                voClone.KJK += 1;
                            //检验科
                            else if (deptArea == "JYK")
                                voClone.JYK += 1;
                            //病理科
                            else if (deptArea == "BLK")
                                voClone.BLK += 1;
                            //放射科
                            else if (deptArea == "FSK")
                                voClone.FSK += 1;
                            //超声科
                            else if (deptArea == "CSK")
                                voClone.CSK += 1;
                            //药剂科
                            else if (deptArea == "YJK")
                                voClone.YJK += 1;
                            //眼科
                            else if (deptArea == "YK")
                                voClone.YK += 1;
                            //眼科
                            else if (deptArea == "EBHK")
                                voClone.EBHK += 1;
                            //第二门部
                            else if (deptArea == "DEMZ")
                                voClone.DEMZ += 1;
                            //第三门部
                            else if (deptArea == "DSMZ")
                                voClone.DSMZ += 1;
                            //门诊部
                            else if (deptArea == "MZ")
                                voClone.MZ += 1;
                            else voClone.MZ += 1;

                            #endregion
                        }
                        else
                        {
                            #region vo
                            EntityEventStat vo = new EntityEventStat();
                            /// 月份
                            vo.TJYF = month;
                            string deptcode = dr["code_vchr"].ToString();
                            string deptArea = JudgeDeptArea(deptcode);
                            //内一区
                            if (deptArea == "NYQ")
                                vo.NYQ = 1;
                            //内二区
                            else if (deptArea == "NEQ")
                                vo.NEQ = 1;
                            //普外科
                            else if (deptArea == "PWK")
                                vo.PWK = 1;
                            //泌尿外科
                            else if (deptArea == "MNWK")
                                vo.MNWK = 1;
                            //手外科
                            else if (deptArea == "SWK")
                                vo.SWK = 1;
                            //骨外科
                            else if (deptArea == "GWK")
                                vo.GWK = 1;
                            //妇产科
                            else if (deptArea == "FCK")
                                vo.FCK = 1;
                            //儿科
                            else if (deptArea == "EK")
                                vo.EK = 1;
                            //重症医学科
                            else if (deptArea == "CZYXK")
                                vo.CZYXK = 1;
                            //急诊科
                            else if (deptArea == "JZK")
                                vo.JZK = 1;
                            //麻醉科
                            else if (deptArea == "MZK")
                                vo.MZK = 1;
                            //中医康复科
                            else if (deptArea == "ZYKFK")
                                vo.ZYKFK = 1;
                            //口腔科
                            else if (deptArea == "KJK")
                                vo.KJK = 1;
                            //检验科
                            else if (deptArea == "JYK")
                                vo.JYK = 1;
                            //病理科
                            else if (deptArea == "BLK")
                                vo.BLK = 1;
                            //放射科
                            else if (deptArea == "FSK")
                                vo.FSK = 1;
                            //超声科
                            else if (deptArea == "CSK")
                                vo.CSK = 1;
                            //药剂科
                            else if (deptArea == "YJK")
                                vo.YJK = 1;
                            //眼科
                            else if (deptArea == "YK")
                                vo.YK = 1;
                            //眼科
                            else if (deptArea == "EBHK")
                                vo.EBHK = 1;
                            //第二门部
                            else if (deptArea == "DEMZ")
                                vo.DEMZ = 1;
                            //第三门部
                            else if (deptArea == "DSMZ")
                                vo.DSMZ = 1;
                            //门诊部
                            else if (deptArea == "MZ")
                                vo.MZ = 1;
                            else vo.MZ = 1;


                            #endregion
                            data.Add(vo);
                        }
                    }
                }
                #endregion

                #region 合计

                foreach (EntityEventStat item in data)
                {
                    item.XJ = item.NYQ + item.NEQ + item.PWK + item.MNWK +
                        item.SWK + item.GWK + item.SJWK + item.FCK +
                        item.EK + item.CZYXK + item.JZK + item.MZK +
                        item.ZYKFK + item.KJK + item.BLK + item.FSK +
                        item.CSK + item.YJK + item.YK + item.EBHK +
                        item.DEMZ + item.DSMZ + item.MZ;
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }

        private string JudgeDeptArea(string deptcode)
        {
            string deptArea = string.Empty;
            //内一区
            List<string> NY = new List<string>() { "02", "0201", "0301", "03011", "030111", "03012", "030121", 
                "03013", "03014", "16", "16012", "16022" ,"16032", "161", "162"};
            //内二区
            List<string> NE = new List<string>() { "0302", "03021", "030211", "03022", "030221", "03023", "03024", 
                "03025", "792"};
            //普外科
            List<string> PW = new List<string>() { "0401", "04011", "04012" };
            //泌尿外科
            List<string> MW = new List<string>() { "0405", "04051", "04013" };
            //手外科
            List<string> SW = new List<string>() { "0408", "04081", "04082" };
            //骨外科
            List<string> GW = new List<string>() { "0403", "04031", "04032" };
            //神经外科
            List<string> SJW = new List<string>() { "0402", "04021", "04022" };
            //妇产科
            List<string> FCK = new List<string>() { "06", "0504", "0503", "05", "0501", "05011", "05012", "0502", "05021", "05022" };
            //儿科
            List<string> EK = new List<string>() { "07", "0701", "0702", "071", "0711", "0712", "072" };
            //重症医学科
            List<string> ICU = new List<string>() { "02", "0201" };
            //急诊科
            List<string> JZ = new List<string>() { "20", "823" };
            //麻醉科
            List<string> MZK = new List<string>() { "2601", "26" };
            //中医康复科
            List<string> ZYKF = new List<string>() { "0404", "04041", "15011", "50", "50152", "502" };
            //口腔科
            List<string> KJ = new List<string>() { "12" };
            //检验科
            List<string> JYK = new List<string>() { "30" };
            //病理科
            List<string> BLK = new List<string>() { "31" };
            //放射科
            List<string> FSK = new List<string>() { "32", "3201", "3202" };
            //超声科
            List<string> CSK = new List<string>() { "3205" };
            //药剂科
            List<string> YJK = new List<string>() { "78", "781", "782", "783", "784", "785", "7851", "7852",
                                                    "7853","7854","7855","7856","7857","787","788"};
            //眼科
            List<string> YK = new List<string>() { "10" };
            //耳鼻喉科
            List<string> EBHK = new List<string>() { "11" };
            //第二门诊
            List<string> DEMZ = new List<string>() { "8802", "88002" };
            //第三门诊
            List<string> DSMZ = new List<string>() { "8803", "88003" };
            //门诊部
            List<string> MZ = new List<string>() { "032", "82", "821", "821", "822", "13", "824", "825" };

            if (NY.Contains(deptcode))
                deptArea = "NYQ"; //内一区
            if (NE.Contains(deptcode))
                deptArea = "NEQ"; //内二区
            if (PW.Contains(deptcode))
                deptArea = "PWK"; //普外科
            if (MW.Contains(deptcode))
                deptArea = "MNWK"; //泌尿外科
            if (SW.Contains(deptcode))
                deptArea = "SWK"; //手外科
            if (GW.Contains(deptcode))
                deptArea = "GWK"; //骨外
            if (SJW.Contains(deptcode))
                deptArea = "SJWK"; //神经外科
            if (FCK.Contains(deptcode))
                deptArea = "FCK"; //妇产科
            if (EK.Contains(deptcode))
                deptArea = "EK"; //儿科
            if (ICU.Contains(deptcode))
                deptArea = "CZYXK"; //重症医学科
            if (JZ.Contains(deptcode))
                deptArea = "JZK"; //急诊
            if (MZK.Contains(deptcode))
                deptArea = "MZK"; //麻醉科
            if (ZYKF.Contains(deptcode))
                deptArea = "ZYKF"; //中医康复科
            if (KJ.Contains(deptcode))
                deptArea = "KJK"; //口腔科
            if (JYK.Contains(deptcode))
                deptArea = "JYK"; //检验科
            if (BLK.Contains(deptcode))
                deptArea = "BLK"; //病理科
            if (FSK.Contains(deptcode))
                deptArea = "FSK"; //放射科
            if (CSK.Contains(deptcode))
                deptArea = "CSK"; //超声科
            if (YJK.Contains(deptcode))
                deptArea = "YJK"; //药剂科
            if (YK.Contains(deptcode))
                deptArea = "YK"; //眼科
            if (EBHK.Contains(deptcode))
                deptArea = "EBHK"; //耳鼻喉科
            if (DEMZ.Contains(deptcode))
                deptArea = "DEMZ"; //第二门诊
            if (DSMZ.Contains(deptcode))
                deptArea = "DSMZ"; //第三门诊
            if (MZ.Contains(deptcode))
                deptArea = "MZ"; //门诊
            return deptArea;
        }

       
        #endregion

        #endregion

        #region 护理质量与安全（不良事件）统计表
        
        #region 类型统计
        /// <summary>
        /// 护理质量与安全（不良事件）（类型统计）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// 
        public List<EntityNursEventClass> GetStatNursEventClass(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            List<EntityNursEventClass> data = new List<EntityNursEventClass>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                Sql = @"select a.rptid,
                        a.reporttime,
                        extractvalue(b.xmldata, '/FormData/X017') as CDBHG,
                        extractvalue(b.xmldata, '/FormData/X020') as SFSBCW,
                        extractvalue(b.xmldata, '/FormData/X021') as SYYWCW,
                        extractvalue(b.xmldata, '/FormData/X019') as BBDSSH,
                        extractvalue(b.xmldata, '/FormData/X018') as JQSBJCYPBHG,
                        extractvalue(b.xmldata, '/FormData/X022') as WJWPBHG,
                        extractvalue(b.xmldata, '/FormData/X023') as FFBHGXDHMJWP,
                        extractvalue(b.xmldata, '/FormData/X026') as GCYLJCSH,
                        extractvalue(b.xmldata, '/FormData/X024') as FSZHMJWPSJ,
                        extractvalue(b.xmldata, '/FormData/X027') as BNJXWPPZCW,
                        extractvalue(b.xmldata, '/FormData/X025') as FSYMJJXXDSJ,
                        extractvalue(b.xmldata, '/FormData/X029') as YWWS,
                        extractvalue(b.xmldata, '/FormData/X035') as YWSC,
                        extractvalue(b.xmldata, '/FormData/X033') as SXFY,
                        extractvalue(b.xmldata, '/FormData/X034') as SXFY_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X039') as SYFY,
                        extractvalue(b.xmldata, '/FormData/X040') as SYFY_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X031') as FJHXBG,
                        extractvalue(b.xmldata, '/FormData/X032') as FJHXBG_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X041') as TD,
                        extractvalue(b.xmldata, '/FormData/X042') as ZC,
                        extractvalue(b.xmldata, '/FormData/X043') as ZS,
                        extractvalue(b.xmldata, '/FormData/X037') as WX,
                        extractvalue(b.xmldata, '/FormData/X038') as WX_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X044') as CXZGJJYJRWS,
                        extractvalue(b.xmldata, '/FormData/X046') as DVTPET,
                        extractvalue(b.xmldata, '/FormData/X048') as XSESS,
                        extractvalue(b.xmldata, '/FormData/X049') as XSEBZGYS,
                        extractvalue(b.xmldata, '/FormData/X051') as CHCX,
                        extractvalue(b.xmldata, '/FormData/X052') as CHCX_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X053') as YDFMXSECS,
                        extractvalue(b.xmldata, '/FormData/X054') as YDFMXSECS_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X055') as YDFMCFNCL,
                        extractvalue(b.xmldata, '/FormData/X056') as SYCCSBFZ,
                        extractvalue(b.xmldata, '/FormData/X050') as HYLS,
                        extractvalue(b.xmldata, '/FormData/X059') as SSCD,
                        extractvalue(b.xmldata, '/FormData/X060') as SSCD_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X061') as SSGCYWYL,
                        extractvalue(b.xmldata, '/FormData/X063') as SSBBCL,
                        extractvalue(b.xmldata, '/FormData/X243') as SSBBCL_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X057') as ZXJMDGXLGR,
                        extractvalue(b.xmldata, '/FormData/X058') as SYHXJWWBZQ,
                        extractvalue(b.xmldata, '/FormData/X064') as JZFZBHG,
                        extractvalue(b.xmldata, '/FormData/X066') as YSZBQBH,
                        extractvalue(b.xmldata, '/FormData/X067') as SZLY,
                        extractvalue(b.xmldata, '/FormData/X068') as ZC_1,
                        extractvalue(b.xmldata, '/FormData/X069') as ZS_1,
                        extractvalue(b.xmldata, '/FormData/X070') as CS,
                        extractvalue(b.xmldata, '/FormData/X071') as SQ,
                        extractvalue(b.xmldata, '/FormData/X072') as DSJF,
                        extractvalue(b.xmldata, '/FormData/X073') as BLXW,
                        extractvalue(b.xmldata, '/FormData/X028') as QTSJ_1,
                        extractvalue(b.xmldata, '/FormData/X065') as QTSJ_2,
                        extractvalue(b.xmldata, '/FormData/X074') as QTSJ_3,
                        extractvalue(b.xmldata, '/FormData/X075') as QTSJ_4,
                        extractvalue(b.xmldata, '/FormData/X076') as QTSJ_5,
                        extractvalue(b.xmldata, '/FormData/X077') as QTSJ_6,
                        extractvalue(b.xmldata, '/FormData/X079') as QTSJ_7,
                        extractvalue(b.xmldata, '/FormData/X080') as QTSJ_8,
                        extractvalue(b.xmldata, '/FormData/B003') as YWSH,
                        extractvalue(b.xmldata, '/FormData/C001') as BFZ
                        from rptEvent a 
                        left join  rpteventdata b 
                        on a.rptid = b.rptid
                        where a.formid = 26      
                        and a.status = 1
                        and a.reporttime between ? and ?
                        order by a.rptid ";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                #endregion

                #region 赋值
                string month = string.Empty;
                string xmlData = string.Empty;
                Dictionary<string, string> dicData = new Dictionary<string, string>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        month = Function.Datetime(dr["reporttime"].ToString()).ToString("yyyy-MM");

                        if (data.Any(t => t.TJYF == month))
                        {
                            #region 累计
                            EntityNursEventClass voClone = data.FirstOrDefault(t => t.TJYF == month);
                            voClone.TJYF = month;
                            //查对不合格
                            if (dr["CDBHG"].ToString() == "1")
                                voClone.CDBHG += 1;
                            //身份识别错误（患者身份查对）
                            if (dr["SFSBCW"].ToString() == "1")
                                voClone.SFSBCW += 1;
                            //使用药物错误（发生在患者身上）
                            if (dr["SYYWCW"].ToString() == "1")
                                voClone.SYYWCW += 1;
                            //标本丢失、损毁 
                            if (dr["BBDSSH"].ToString() == "1")
                                voClone.BBDSSH += 1;
                            //急救设备器材药品不合格
                            if (dr["JQSBJCYPBHG"].ToString() == "1")
                                voClone.JQSBJCYPBHG += 1;
                            //无菌物品不合格
                            if (dr["WJWPBHG"].ToString() == "1")
                                voClone.WJWPBHG += 1;
                            //发放不合格的消毒或灭菌物品
                            if (dr["FFBHGXDHMJWP"].ToString() == "1")
                                voClone.FFBHGXDHMJWP += 1;
                            //贵重医疗器材损毁或丢失
                            if (dr["GCYLJCSH"].ToString() == "1")
                                voClone.GCYLJCSH += 1;
                            //发生召回灭菌物品事件
                            if (dr["FSZHMJWPSJ"].ToString() == "1")
                                voClone.FSZHMJWPSJ += 1;
                            //包内器械物品配置错误影响手术进程
                            if (dr["BNJXWPPZCW"].ToString() == "1")
                                voClone.BNJXWPPZCW += 1;
                            //发生与灭菌器械相关的感染事件
                            if (dr["FSYMJJXXDSJ"].ToString() == "1")
                                voClone.FSYMJJXXDSJ += 1;
                            //药物外渗
                            if (dr["YWWS"].ToString() == "1")
                                voClone.YWWS += 1;
                            //药物渗出
                            if (dr["YWSC"].ToString() == "1")
                                voClone.YWSC += 1;
                            //输血反应
                            if (dr["SXFY"].ToString() == "1" || dr["SXFY_DETAIL"].ToString() != "")
                                voClone.SXFY += 1;
                            //输液反应
                            if (dr["SYFY"].ToString() == "1" || dr["SYFY_DETAIL"].ToString() != "")
                                voClone.SYFY += 1;
                            //非计划性拔管
                            if (dr["FJHXBG"].ToString() == "1" || dr["FJHXBG_DETAIL"].ToString() != "")
                                voClone.FJHXBG += 1;
                            //跌倒
                            if (dr["TD"].ToString() == "1")
                                voClone.TD += 1;
                            //坠床 
                            if (dr["ZC"].ToString() == "1")
                                voClone.ZC += 1;
                            //走失
                            if (dr["ZS"].ToString() == "1")
                                voClone.ZS += 1;
                            //误吸
                            if (dr["WX"].ToString() == "1" || dr["WX_DETAIL"].ToString() != "")
                                voClone.WX += 1;
                            //足下垂/关节僵硬/肌肉萎缩
                            if (dr["CXZGJJYJRWS"].ToString() == "1")
                                voClone.CXZGJJYJRWS += 1;
                            //DVT/PET 
                            if (dr["DVTPET"].ToString() == "1")
                                voClone.DVTPET += 1;
                            //新生儿烧伤、烫伤
                            if (dr["XSESS"].ToString() == "1")
                                voClone.XSESS += 1;
                            //新生儿鼻中隔压伤
                            if (dr["CHCX"].ToString() == "1" || dr["CHCX_DETAIL"].ToString() != "")
                                voClone.CHCX += 1;
                            //阴道分娩新生儿产伤
                            if (dr["YDFMXSECS"].ToString() == "1" || dr["YDFMXSECS_DETAIL"].ToString() != "")
                                voClone.YDFMXSECS += 1;
                            //阴道分娩产妇尿潴留
                            if (dr["YDFMCFNCL"].ToString() == "1")
                                voClone.YDFMCFNCL += 1;
                            //使用催产素并发症
                            if (dr["SYCCSBFZ"].ToString() == "1")
                                voClone.SYCCSBFZ += 1;
                            //会阴裂伤
                            if (dr["HYLS"].ToString() == "1")
                                voClone.HYLS += 1;
                            //手术查对不合格
                            if (dr["SSCD"].ToString() == "1" || dr["SSCD_DETAIL"].ToString() != "")
                                voClone.SSCD += 1;
                            //手术过程异物遗留
                            if (dr["SSGCYWYL"].ToString() == "1")
                                voClone.SSGCYWYL += 1;
                            //手术标本处理不合格
                            if (dr["SSBBCL"].ToString() == "1" || dr["SSBBCL_DETAIL"].ToString() != "")
                                voClone.SSBBCL += 1;
                            //中心静脉导管相关血流感染
                            if (dr["ZXJMDGXLGR"].ToString() == "1")
                                voClone.ZXJMDGXLGR += 1;
                            //使用呼吸机卧位不正确
                            if (dr["SYHXJWWBZQ"].ToString() == "1")
                                voClone.SYHXJWWBZQ += 1;
                            //急诊分诊不合格
                            if (dr["JZFZBHG"].ToString() == "1")
                                voClone.JZFZBHG += 1;
                            //运送中病情变化    
                            if (dr["YSZBQBH"].ToString() == "1")
                                voClone.YSZBQBH += 1;
                            //擅自离院 
                            if (dr["SZLY"].ToString() == "1")
                                voClone.SZLY += 1;
                            //自残   
                            if (dr["ZC_1"].ToString() == "1")
                                voClone.ZC_1 += 1;
                            //自杀   
                            if (dr["ZS_1"].ToString() == "1")
                                voClone.ZS_1 += 1;
                            //猝死  
                            if (dr["CS"].ToString() == "1")
                                voClone.CS += 1;
                            //失窃    
                            if (dr["SQ"].ToString() == "1")
                                voClone.SQ += 1;
                            //投诉纠纷 
                            if (dr["DSJF"].ToString() == "1")
                                voClone.DSJF += 1;
                            //暴力行为      
                            if (dr["BLXW"].ToString() == "1")
                                voClone.BLXW += 1;
                            //意外伤害
                            if (dr["YWSH"].ToString() == "1")
                                voClone.YWSH += 1;
                            //意外伤害
                            if (dr["BFZ"].ToString() == "1")
                                voClone.BFZ += 1;
                            //其他事件    
                            if (dr["QTSJ_1"].ToString() == "1")
                                voClone.QTSJ += 1;
                            if (dr["QTSJ_2"].ToString() == "1")
                                voClone.QTSJ += 1;
                            if (dr["QTSJ_3"].ToString() == "1")
                                voClone.QTSJ += 1;
                            if (dr["QTSJ_4"].ToString() == "1")
                                voClone.QTSJ += 1;
                            if (dr["QTSJ_5"].ToString() == "1")
                                voClone.QTSJ += 1;
                            if (dr["QTSJ_6"].ToString() == "1")
                                voClone.QTSJ += 1;
                            if (dr["QTSJ_7"].ToString() == "1")
                                voClone.QTSJ += 1;
                            if (dr["QTSJ_8"].ToString() == "1")
                                voClone.QTSJ += 1;


                            #endregion
                        }
                        else
                        {
                            #region vo
                            EntityNursEventClass vo = new EntityNursEventClass();
                            /// 月份
                            vo.TJYF = month;
                            //查对不合格
                            if (dr["CDBHG"].ToString() == "1")
                                vo.CDBHG = 1;
                            //身份识别错误（患者身份查对）
                            if (dr["SFSBCW"].ToString() == "1")
                                vo.SFSBCW = 1;
                            //使用药物错误（发生在患者身上）
                            if (dr["SYYWCW"].ToString() == "1")
                                vo.SYYWCW = 1;
                            //标本丢失、损毁 
                            if (dr["BBDSSH"].ToString() == "1")
                                vo.BBDSSH = 1;
                            //急救设备器材药品不合格
                            if (dr["JQSBJCYPBHG"].ToString() == "1")
                                vo.JQSBJCYPBHG = 1;
                            //无菌物品不合格
                            if (dr["WJWPBHG"].ToString() == "1")
                                vo.WJWPBHG = 1;
                            //发放不合格的消毒或灭菌物品
                            if (dr["FFBHGXDHMJWP"].ToString() == "1")
                                vo.FFBHGXDHMJWP = 1;
                            //贵重医疗器材损毁或丢失
                            if (dr["GCYLJCSH"].ToString() == "1")
                                vo.GCYLJCSH = 1;
                            //发生召回灭菌物品事件
                            if (dr["FSZHMJWPSJ"].ToString() == "1")
                                vo.FSZHMJWPSJ = 1;
                            //包内器械物品配置错误影响手术进程
                            if (dr["BNJXWPPZCW"].ToString() == "1")
                                vo.BNJXWPPZCW = 1;
                            //发生与灭菌器械相关的感染事件
                            if (dr["FSYMJJXXDSJ"].ToString() == "1")
                                vo.FSYMJJXXDSJ = 1;
                            //药物外渗
                            if (dr["YWWS"].ToString() == "1")
                                vo.YWWS = 1;
                            //药物渗出
                            if (dr["YWSC"].ToString() == "1")
                                vo.YWSC = 1;
                            //输血反应
                            if (dr["SXFY"].ToString() == "1" || dr["SXFY_DETAIL"].ToString() != "")
                                vo.SXFY = 1;
                            //输液反应
                            if (dr["SYFY"].ToString() == "1" || dr["SYFY_DETAIL"].ToString() != "")
                                vo.SYFY = 1;
                            //非计划性拔管
                            if (dr["FJHXBG"].ToString() == "1" || dr["FJHXBG_DETAIL"].ToString() != "")
                                vo.FJHXBG = 1;
                            //跌倒
                            if (dr["TD"].ToString() == "1")
                                vo.TD = 1;
                            //坠床 
                            if (dr["ZC"].ToString() == "1")
                                vo.ZC = 1;
                            //走失
                            if (dr["ZS"].ToString() == "1")
                                vo.ZS = 1;
                            //误吸
                            if (dr["WX"].ToString() == "1" || dr["WX_DETAIL"].ToString() != "")
                                vo.WX = 1;
                            //足下垂/关节僵硬/肌肉萎缩
                            if (dr["CXZGJJYJRWS"].ToString() == "1")
                                vo.CXZGJJYJRWS = 1;
                            //DVT/PET 
                            if (dr["DVTPET"].ToString() == "1")
                                vo.DVTPET = 1;
                            //新生儿烧伤、烫伤
                            if (dr["XSESS"].ToString() == "1")
                                vo.XSESS = 1;
                            //新生儿鼻中隔压伤
                            if (dr["XSEBZGYS"].ToString() == "1")
                                vo.XSEBZGYS = 1;
                            //产后出血
                            if (dr["CHCX"].ToString() == "1" || dr["CHCX_DETAIL"].ToString() != "")
                                vo.CHCX = 1;
                            //阴道分娩新生儿产伤
                            if (dr["YDFMXSECS"].ToString() == "1" || dr["YDFMXSECS_DETAIL"].ToString() != "")
                                vo.YDFMXSECS = 1;
                            //阴道分娩产妇尿潴留
                            if (dr["YDFMCFNCL"].ToString() == "1")
                                vo.YDFMCFNCL = 1;
                            //使用催产素并发症
                            if (dr["SYCCSBFZ"].ToString() == "1")
                                vo.SYCCSBFZ = 1;
                            //会阴裂伤
                            if (dr["HYLS"].ToString() == "1")
                                vo.HYLS = 1;
                            //手术查对不合格
                            if (dr["SSCD"].ToString() == "1")
                                vo.SSCD = 1;
                            //手术过程异物遗留
                            if (dr["SSGCYWYL"].ToString() == "1")
                                vo.SSGCYWYL = 1;
                            //手术标本处理不合格
                            if (dr["SSBBCL"].ToString() == "1")
                                vo.SSBBCL = 1;
                            //中心静脉导管相关血流感染
                            if (dr["ZXJMDGXLGR"].ToString() == "1")
                                vo.ZXJMDGXLGR = 1;
                            //使用呼吸机卧位不正确
                            if (dr["SYHXJWWBZQ"].ToString() == "1")
                                vo.SYHXJWWBZQ = 1;
                            //急诊分诊不合格
                            if (dr["JZFZBHG"].ToString() == "1")
                                vo.JZFZBHG = 1;
                            //运送中病情变化    
                            if (dr["YSZBQBH"].ToString() == "1")
                                vo.YSZBQBH = 1;
                            //擅自离院 
                            if (dr["SZLY"].ToString() == "1")
                                vo.SZLY = 1;
                            //自残   
                            if (dr["ZC_1"].ToString() == "1")
                                vo.ZC_1 = 1;
                            //自杀   
                            if (dr["ZS_1"].ToString() == "1")
                                vo.ZS_1 = 1;
                            //猝死  
                            if (dr["CS"].ToString() == "1")
                                vo.CS = 1;
                            //失窃    
                            if (dr["SQ"].ToString() == "1")
                                vo.SQ = 1;
                            //投诉纠纷 
                            if (dr["DSJF"].ToString() == "1")
                                vo.DSJF = 1;
                            //暴力行为      
                            if (dr["BLXW"].ToString() == "1")
                                vo.BLXW = 1;
                            //意外伤害
                            if (dr["YWSH"].ToString() == "1")
                                vo.YWSH = 1;
                            //意外伤害
                            if (dr["BFZ"].ToString() == "1")
                                vo.BFZ = 1;
                            //其他事件      
                            if (dr["QTSJ_1"].ToString() == "1")
                                vo.QTSJ += 1;
                            if (dr["QTSJ_2"].ToString() == "1")
                                vo.QTSJ += 1;
                            if (dr["QTSJ_3"].ToString() == "1")
                                vo.QTSJ += 1;
                            if (dr["QTSJ_4"].ToString() == "1")
                                vo.QTSJ += 1;
                            if (dr["QTSJ_5"].ToString() == "1")
                                vo.QTSJ += 1;
                            if (dr["QTSJ_6"].ToString() == "1")
                                vo.QTSJ += 1;
                            if (dr["QTSJ_7"].ToString() == "1")
                                vo.QTSJ += 1;
                            if (dr["QTSJ_8"].ToString() == "1")
                                vo.QTSJ += 1;

                            #endregion
                            data.Add(vo);
                        }
                    }
                }
                #endregion

                #region 合计

                foreach (EntityNursEventClass item in data)
                {
                    item.HJ = item.CDBHG + item.SFSBCW + item.SYYWCW + item.BBDSSH +
                        item.JQSBJCYPBHG + item.WJWPBHG + item.FFBHGXDHMJWP + item.GCYLJCSH +
                        item.FSZHMJWPSJ + item.BNJXWPPZCW + item.FSYMJJXXDSJ + item.YWWS +
                        item.YWSC + item.SXFY + item.SXFY + item.SYFY +
                        item.FJHXBG + item.QTSJ + item.TD + item.ZC + item.ZS + item.WX + item.CXZGJJYJRWS +
                        item.DVTPET + item.XSESS + item.XSEBZGYS + item.CHCX +
                        item.YDFMXSECS + item.YDFMCFNCL + item.SYCCSBFZ + item.HYLS +
                        item.SSCD + item.SSGCYWYL + item.SSBBCL + item.ZXJMDGXLGR +
                        item.SYHXJWWBZQ + item.JZFZBHG + item.YSZBQBH +
                        item.SZLY + item.ZC_1 + item.ZS_1 + item.CS +
                        item.SQ + item.DSJF + item.BLXW+item.BFZ+item.YWSH ;
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }
        #endregion

        #region 细分类型统计
        /// <summary>
        /// 护理质量与安全（不良事件）（细分类型统计）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// 
        public List<EntityNursEventSubClass> GetStatNursEventSubClass(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            List<EntityNursEventSubClass> data = new List<EntityNursEventSubClass>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                Sql = @"select a.rptid,
                        a.reporttime,
                        extractvalue(b.xmldata, '/FormData/X034') as SXFY_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X040') as SYFY_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X032') as FJHXBG_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X038') as WX_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X052') as CHCX_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X054') as YDFMXSECS_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X060') as SSCD_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X243') as SSBBCL_DETAIL
                        from rptEvent a 
                        left join  rpteventdata b 
                        on a.rptid = b.rptid
                        where a.formid = 26      
                        and a.status = 1
                        and a.reporttime between ? and ?
                        order by a.rptid  ";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                #endregion

                #region 赋值
                string month = string.Empty;
                string xmlData = string.Empty;
                Dictionary<string, string> dicData = new Dictionary<string, string>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        month = Function.Datetime(dr["reporttime"].ToString()).ToString("yyyy-MM");

                        if (data.Any(t => t.TJYF == month))
                        {
                            #region 累计
                            EntityNursEventSubClass voClone = data.FirstOrDefault(t => t.TJYF == month);
                            voClone.TJYF = month;
                            //输血反应
                            if (dr["SXFY_DETAIL"].ToString().Trim() == "非溶血反应")
                                voClone.SXFY_FRXFY += 1;
                            if (dr["SXFY_DETAIL"].ToString().Trim() == "溶血反应")
                                voClone.SXFY_RXFY += 1;
                            //输液反应
                            if (dr["SYFY_DETAIL"].ToString().Trim() == "发热")
                                voClone.SYFY_FR += 1;
                            if (dr["SYFY_DETAIL"].ToString().Trim() == "过敏")
                                voClone.SYFY_GM += 1;
                            if (dr["SYFY_DETAIL"].ToString().Trim() == "静脉炎")
                                voClone.SYFY_JMY += 1;
                            if (dr["SYFY_DETAIL"].ToString().Trim() == "空气栓塞")
                                voClone.SYFY_XS += 1;
                            //非计划性拔管
                            if (dr["FJHXBG_DETAIL"].ToString().Trim() == "尿管")
                                voClone.FJHXBG_NG += 1;
                            if (dr["FJHXBG_DETAIL"].ToString().Trim() == "气管插管")
                                voClone.FJHXBG_QGCG += 1;
                            if (dr["FJHXBG_DETAIL"].ToString().Trim() == "胃管")
                                voClone.FJHXBG_WG += 1;
                            if (dr["FJHXBG_DETAIL"].ToString().Trim() == "引流管")
                                voClone.FJHXBG_YLG += 1;
                            if (dr["FJHXBG_DETAIL"].ToString().Trim() == "中心静脉导管")
                                voClone.FJHXBG_ZXJMDG += 1;
                            //误吸
                            if (dr["WX_DETAIL"].ToString().Trim() == "窒息")
                                voClone.WX_ZX += 1;
                            if (dr["WX_DETAIL"].ToString().Trim() == "肺炎")
                                voClone.WX_FY += 1;
                            //产后出血
                            if (dr["CHCX_DETAIL"].ToString().Trim() == "产房出血")
                                voClone.CHCX_CFCX += 1;
                            if (dr["CHCX_DETAIL"].ToString().Trim() == "病房产后出血")
                                voClone.CHCX_BFCX += 1;
                            //阴道分娩新生儿产伤
                            if (dr["YDFMXSECS_DETAIL"].ToString().Trim() == "骨折")
                                voClone.YDFMXSECS_GZ += 1;
                            //阴道分娩新生儿产伤
                            if (dr["YDFMXSECS_DETAIL"].ToString().Trim() == "重度窒息")
                                voClone.YDFMXSECS_CDZX += 1;
                            //阴道分娩新生儿产伤
                            if (dr["YDFMXSECS_DETAIL"].ToString().Trim() == "臂丛神经损伤")
                                voClone.YDFMXSECS_BCSJSS += 1;
                            //手术标本处理不合格
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "遗失")
                                voClone.SSBBCL_YS += 1;
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "留置不合格")
                                voClone.SSBBCL_LZ += 1;
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "漏送")
                                voClone.SSBBCL_LS += 1;
                            //手术查对
                            if (dr["SSCD_DETAIL"].ToString().Trim() == "手术患者身份")
                                voClone.SSCD_HZSF += 1;
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "手术部位标识")
                                voClone.SSCD_SSBW += 1;
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "手术同意书内容")
                                voClone.SSCD_SSTYS += 1;
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "TIME OUT")
                                voClone.SSCD_TIMEOUT += 1;
                            #endregion
                        }
                        else
                        {
                            #region vo
                            EntityNursEventSubClass vo = new EntityNursEventSubClass();
                            /// 月份
                            vo.TJYF = month;
                            //输血反应
                            if (dr["SXFY_DETAIL"].ToString().Trim() == "非溶血反应")
                                vo.SXFY_FRXFY = 1;
                            if (dr["SXFY_DETAIL"].ToString().Trim() == "溶血反应")
                                vo.SXFY_RXFY = 1;
                            //输液反应
                            if (dr["SYFY_DETAIL"].ToString().Trim() == "发热")
                                vo.SYFY_FR = 1;
                            if (dr["SYFY_DETAIL"].ToString().Trim() == "过敏")
                                vo.SYFY_GM = 1;
                            if (dr["SYFY_DETAIL"].ToString().Trim() == "静脉炎")
                                vo.SYFY_JMY = 1;
                            if (dr["SYFY_DETAIL"].ToString().Trim() == "空气栓塞")
                                vo.SYFY_XS = 1;
                            //非计划性拔管
                            if (dr["FJHXBG_DETAIL"].ToString().Trim() == "尿管")
                                vo.FJHXBG_NG = 1;
                            if (dr["FJHXBG_DETAIL"].ToString().Trim() == "气管插管")
                                vo.FJHXBG_QGCG = 1;
                            if (dr["FJHXBG_DETAIL"].ToString().Trim() == "胃管")
                                vo.FJHXBG_WG = 1;
                            if (dr["FJHXBG_DETAIL"].ToString().Trim() == "引流管")
                                vo.FJHXBG_YLG = 1;
                            if (dr["FJHXBG_DETAIL"].ToString().Trim() == "中心静脉导管")
                                vo.FJHXBG_ZXJMDG = 1;
                            //误吸
                            if (dr["WX_DETAIL"].ToString().Trim() == "窒息")
                                vo.WX_ZX = 1;
                            if (dr["WX_DETAIL"].ToString().Trim() == "肺炎")
                                vo.WX_FY = 1;
                            //产后出血
                            if (dr["CHCX_DETAIL"].ToString().Trim() == "产房出血")
                                vo.CHCX_CFCX = 1;
                            if (dr["CHCX_DETAIL"].ToString().Trim() == "病房产后出血")
                                vo.CHCX_BFCX = 1;
                            //阴道分娩新生儿产伤
                            if (dr["YDFMXSECS_DETAIL"].ToString().Trim() == "骨折")
                                vo.YDFMXSECS_GZ = 1;
                            //阴道分娩新生儿产伤
                            if (dr["YDFMXSECS_DETAIL"].ToString().Trim() == "重度窒息")
                                vo.YDFMXSECS_CDZX = 1;
                            //阴道分娩新生儿产伤
                            if (dr["YDFMXSECS_DETAIL"].ToString().Trim() == "臂丛神经损伤")
                                vo.YDFMXSECS_BCSJSS = 1;
                            //手术标本处理不合格
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "遗失")
                                vo.SSBBCL_YS = 1;
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "留置不合格")
                                vo.SSBBCL_LZ = 1;
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "漏送")
                                vo.SSBBCL_LS = 1;
                            //手术查对
                            if (dr["SSCD_DETAIL"].ToString().Trim() == "手术患者身份")
                                vo.SSCD_HZSF = 1;
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "手术部位标识")
                                vo.SSCD_SSBW = 1;
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "手术同意书内容")
                                vo.SSCD_SSTYS = 1;
                            if (dr["SSBBCL_DETAIL"].ToString().Trim() == "TIME OUT")
                                vo.SSCD_TIMEOUT = 1;

                            #endregion
                            data.Add(vo);
                        }
                    }
                }
                #endregion

                #region 合计

                foreach (EntityNursEventSubClass item in data)
                {
                    item.HJ = item.SXFY_FRXFY + item.SXFY_RXFY + item.SYFY_FR + item.SYFY_GM +
                        item.SYFY_JMY + item.SYFY_XS + item.FJHXBG_NG + item.FJHXBG_QGCG +
                        item.FJHXBG_WG + item.FJHXBG_YLG + item.FJHXBG_ZXJMDG + item.CHCX_CFCX +
                        item.CHCX_BFCX + item.YDFMXSECS_GZ + item.YDFMXSECS_CDZX + item.YDFMXSECS_BCSJSS +
                        item.SSBBCL_YS + item.SSBBCL_LZ + item.SSBBCL_LS + item.SSCD_HZSF + item.SSCD_SSBW +
                        item.SSCD_SSTYS + item.SSCD_TIMEOUT + item.WX_FY + item.WX_ZX ;
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }
        #endregion

        #region 按级别
        /// <summary>
        /// 医疗安全（不良）事件统计表（按级别）
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public List<EntityNursEventRptClass> GetStatNursEventLevel(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            List<EntityNursEventRptClass> data = new List<EntityNursEventRptClass>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                Sql = @"select a.rptid,
                        a.reporttime,
                        extractvalue(b.xmldata, '/FormData/X007') as SJLX
                        from rptEvent a 
                        left join  rpteventdata b 
                        on a.rptid = b.rptid
                        where a.formid = 26
                        and a.status = 1
                        and a.reporttime between ? and ?
                        order by a.rptid ";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                #endregion

                #region 赋值
                string month = string.Empty;
                string xmlData = string.Empty;
                Dictionary<string, string> dicData = new Dictionary<string, string>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        month = Function.Datetime(dr["reporttime"].ToString()).ToString("yyyy-MM");
                       
                        if (data.Any(t => t.TJYF == month))
                        {
                            #region 累计
                            EntityNursEventRptClass voClone = data.FirstOrDefault(t => t.TJYF == month);
                            voClone.TJYF = month;

                            // I型
                            if (dr["SJLX"].ToString().Trim() == "I")
                                voClone.IJSJ += 1;
                            // II型
                            if (dr["SJLX"].ToString().Trim() == "II")
                                voClone.IIJSJ += 1;
                            // III型
                            if (dr["SJLX"].ToString().Trim() == "III")
                                voClone.IIIJSJ += 1;
                            // IV型
                            if (dr["SJLX"].ToString().Trim() == "IV")
                                voClone.IVJSJ += 1;
                            #endregion
                        }
                        else
                        {
                            #region vo
                            EntityNursEventRptClass vo = new EntityNursEventRptClass();
                            /// 月份
                            vo.TJYF = month;
                            // I型
                            if (dr["SJLX"].ToString().Trim() == "I")
                                vo.IJSJ = 1;
                            // II型
                            if (dr["SJLX"].ToString().Trim() == "II")
                                vo.IIJSJ = 1;
                            // III型
                            if (dr["SJLX"].ToString().Trim() == "III")
                                vo.IIIJSJ = 1;
                            // IV型
                            if (dr["SJLX"].ToString().Trim() == "IV")
                                vo.IVJSJ = 1;

                            #endregion
                            data.Add(vo);
                        }
                    }
                }
                #endregion

                #region 合计

                foreach (EntityNursEventRptClass item in data)
                {
                    item.HJ = item.IJSJ + item.IIJSJ + item.IIIJSJ + item.IVJSJ;
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }
        #endregion

        #region 上报科室统计
        public List<EntityNursEventDept> GetStatNursEventDept(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            List<EntityNursEventDept> data = new List<EntityNursEventDept>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                Sql = @"select a.rptid,
                        a.reporttime,
                        d.deptid_chr,
                        d.code_vchr,
                        d.deptname_vchr
                        from rptEvent a 
                        left join  rpteventdata b 
                        on a.rptid = b.rptid
                        inner join t_bse_deptdesc d
                        on a.reportdeptcode = d.code_vchr
                        where a.formid = 26
                        and a.status = 1
                        and a.reporttime between ? and ?
                        order by a.rptid ";// 

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                #endregion

                #region 赋值
                string month = string.Empty;
                string xmlData = string.Empty;
                Dictionary<string, string> dicData = new Dictionary<string, string>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        month = Function.Datetime(dr["reporttime"].ToString()).ToString("yyyy-MM");

                        if (data.Any(t => t.TJYF == month))
                        {
                            #region 累计
                            EntityNursEventDept voClone = data.FirstOrDefault(t => t.TJYF == month);
                            voClone.TJYF = month;
                            string deptcode = dr["code_vchr"].ToString().Trim();
                            string deptArea = NursEventJudgeDept(deptcode.Trim());
                            //内一区
                            if (deptArea == "NYQ")
                                voClone.NYQ += 1;
                            //内二区
                            else if (deptArea == "NEQ")
                                voClone.NEQ += 1;
                            //神经外科
                            else if (deptArea == "SJWK")
                                voClone.SJWK += 1;
                            //骨科
                            else if (deptArea == "GWK")
                                voClone.GK += 1;
                            //耳鼻喉科
                            else if (deptArea == "EBHK")
                                voClone.EBHK += 1;
                            //眼科
                            else if (deptArea == "YK")
                                voClone.YK += 1;
                            //中医科
                            else if (deptArea == "ZYK")
                                voClone.ZYK += 1;
                            //普外科
                            else if (deptArea == "PWK")
                                voClone.PWK += 1;
                            //泌尿外科
                            else if (deptArea == "MNWK")
                                voClone.MNWK += 1;
                            //手外科
                            else if (deptArea == "SWK")
                                voClone.SWK += 1;
                            //妇科
                            else if (deptArea == "FK")
                                voClone.FK += 1;
                            //产科
                            else if (deptArea == "CK")
                                voClone.CK += 1;
                            //普儿科
                            else if (deptArea == "PEK")
                                voClone.PEK += 1;
                            //新生儿科
                            else if (deptArea == "XSEK")
                                voClone.XSEK += 1;
                            //ICU
                            else if (deptArea == "ICU")
                                voClone.ICU += 1;
                            //手术室
                            else if (deptArea == "SSS")
                                voClone.SSS += 1;
                            //急诊科
                            else if (deptArea == "JZK")
                                voClone.JZK += 1;
                            //第二门诊
                            else if (deptArea == "DEMZ")
                                voClone.DEMZ += 1;
                            //第三门诊
                            else if (deptArea == "DSMZ")
                                voClone.DSMZ += 1;
                            //消毒供应室
                            else if (deptArea == "XDGYS")
                                voClone.XDGYS += 1;
                            //多功能科
                            else if (deptArea == "DGNK")
                                voClone.DGNK += 1;
                            //体检科
                            else if (deptArea == "TJK")
                                voClone.TJK += 1;
                            //门诊部
                            else if (deptArea == "MZ")
                                voClone.MZB += 1;

                            #endregion
                        }
                        else
                        {
                            #region vo
                            EntityNursEventDept vo = new EntityNursEventDept();
                            /// 月份
                            vo.TJYF = month;
                            string deptcode = dr["code_vchr"].ToString();
                            string deptArea = JudgeDeptArea(deptcode);

                            //内一区
                            if (deptArea == "NYQ")
                                vo.NYQ = 1;
                            //内二区
                            else if (deptArea == "NEQ")
                                vo.NEQ = 1;
                            //神经外科
                            else if (deptArea == "SJWK")
                                vo.SJWK = 1;
                            //骨科
                            else if (deptArea == "GK")
                                vo.GK = 1;
                            //耳鼻喉科
                            else if (deptArea == "EBHK")
                                vo.EBHK = 1;
                            //眼科
                            else if (deptArea == "YK")
                                vo.YK = 1;
                            //中医科
                            else if (deptArea == "ZYK")
                                vo.ZYK = 1;
                            //普外科
                            else if (deptArea == "PWK")
                                vo.PWK = 1;
                            //泌尿外科
                            else if (deptArea == "MNWK")
                                vo.MNWK = 1;
                            //手外科
                            else if (deptArea == "SWK")
                                vo.SWK = 1;
                            //妇科
                            else if (deptArea == "FK")
                                vo.FK = 1;
                            //产科
                            else if (deptArea == "CK")
                                vo.CK = 1;
                            //普儿科
                            else if (deptArea == "PEK")
                                vo.PEK = 1;
                            //新生儿科
                            else if (deptArea == "XSEK")
                                vo.XSEK = 1;
                            //ICU
                            else if (deptArea == "ICU")
                                vo.ICU = 1;
                            //手术室
                            else if (deptArea == "SSS")
                                vo.SSS = 1;
                            //急诊科
                            else if (deptArea == "JZK")
                                vo.JZK = 1;
                            //第二门诊
                            else if (deptArea == "DEMZ")
                                vo.DEMZ = 1;
                            //第三门诊
                            else if (deptArea == "DSMZ")
                                vo.DSMZ = 1;
                            //消毒供应室
                            else if (deptArea == "XDGYS")
                                vo.XDGYS = 1;
                            //多功能科
                            else if (deptArea == "DGNK")
                                vo.DGNK = 1;
                            //体检科
                            else if (deptArea == "TJK")
                                vo.TJK = 1;
                            //门诊部
                            else if (deptArea == "MZB")
                                vo.MZB = 1;

                            #endregion
                            data.Add(vo);
                        }
                    }
                }
                #endregion

                #region 合计

                foreach (EntityNursEventDept item in data)
                {
                    item.HJ = item.NYQ + item.NEQ + item.SJWK + item.GK + item.EBHK +
                        item.YK + item.ZYK + item.PWK + item.MNWK + item.SWK + item.FK + 
                        item.CK + item.PEK + item.XSEK + item.ICU + item.SSS + item.JZK + item.TJK;
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }

        private string NursEventJudgeDept(string deptcode)
        {
            string deptArea = string.Empty;
            //内一区
            List<string> NYQ = new List<string>() { "02", "0201", "0301", "03011", "030111", "03012", "030121", 
                "03013", "03014", "16", "16012", "16022" ,"16032", "161", "162"};
            //内二区
            List<string> NEQ = new List<string>() { "0302", "03021", "030211", "03022", "030221", "03023", "03024", 
                "03025", "792"};
            //神经外科
            List<string> SJWK = new List<string>() { "0402", "04021", "04022" };
            //骨科
            List<string> GK = new List<string>() { "0403", "04031", "04032" };
            //耳鼻喉科
            List<string> EBHK = new List<string>() { "11" };
            //眼科
            List<string> YK = new List<string>() { "10" };
            //中医科
            List<string> ZYK = new List<string>() { "0404", "04041", "15011", "50", "50152", "502" };
            //普外科
            List<string> PWK = new List<string>() { "0401", "04011", "04012" };
            //泌尿外科
            List<string> MW = new List<string>() { "0405", "04051", "04013" };
            //手外科
            List<string> SW = new List<string>() { "0408", "04081", "04082" };
            //妇科
            List<string> FK = new List<string>() { "05","0501", "05011", "05012", "06" };
            //产科
            List<string> CK = new List<string>() { "0502", "05021", "05022"};
            //普儿科
            List<string> PEK = new List<string>() { "07", "0701", "071", "0711", "072" };
            //新生儿科
            List<string> XSEK = new List<string>() { "0702", "0712" };
            //ICU
            List<string> ICU = new List<string>() { "02", "0201" };
            //手术室
            List<string> SSS = new List<string>() { "2601", "26" };
            //门诊部（包括放射、B超、口腔、分诊、专科门诊）
            List<string> MZB = new List<string>() { "032", "82", "821", "821", "822", "13", "824", "825", 
                                             "31","32","3201","3202","3205","3206","12"};
            //急诊科
            List<string> JZK = new List<string>() { "20", "823" };  
            //第二门诊
            List<string> DEMZ = new List<string>() { "8802", "88002" };
            //第三门诊
            List<string> DSMZ = new List<string>() { "8803", "88003" };
            //消毒供应室
            List<string> XDGYS = new List<string>() { "791" };
            //多功能科
            List<string> DGNK = new List<string>() { "40" };
            //体检科
            List<string> TJK = new List<string>() { "0103" };

            if (NYQ.Contains(deptcode))
                deptArea = "NYQ"; //内一区
            if (NEQ.Contains(deptcode))
                deptArea = "NEQ"; //内二区
            if (SJWK.Contains(deptcode))
                deptArea = "SJWK"; //神经外科
            if (GK.Contains(deptcode))
                deptArea = "GK"; //骨科
            if (EBHK.Contains(deptcode))
                deptArea = "EBHK"; //耳鼻喉科
            if (YK.Contains(deptcode))
                deptArea = "YK"; //眼科
            if (ZYK.Contains(deptcode))
                deptArea = "ZYK"; //中医科
            if (MW.Contains(deptcode))
                deptArea = "MNWK"; //泌尿外科
            if (SW.Contains(deptcode))
                deptArea = "SWK"; //手外科
            if (GK.Contains(deptcode))
                deptArea = "GK"; //骨科
            if (FK.Contains(deptcode))
                deptArea = "FK"; //妇科
            if (CK.Contains(deptcode))
                deptArea = "CK"; //产科
            if (PEK.Contains(deptcode))
                deptArea = "PEK"; //普儿科
            if (XSEK.Contains(deptcode))
                deptArea = "XSEK"; //新生儿科
            if (ICU.Contains(deptcode))
                deptArea = "ICU"; //重症医学科
            if (SSS.Contains(deptcode))
                deptArea = "SSS"; //手术室
            if (JZK.Contains(deptcode))
                deptArea = "JZK"; //急诊科
            if (MZB.Contains(deptcode))
                deptArea = "MZB"; //门诊部
            if (DEMZ.Contains(deptcode))
                deptArea = "DEMZ"; //第二门诊
            if (DSMZ.Contains(deptcode))
                deptArea = "DSMZ"; //第三门诊
            if (XDGYS.Contains(deptcode))
                deptArea = "XDGYS"; //消毒供应室
            if (DGNK.Contains(deptcode))
                deptArea = "DGNK"; //多功能科
            if (TJK.Contains(deptcode))
                deptArea = "TJK"; //病理科

            return deptArea;
        }
        #endregion

        #region 汇总表
        /// <summary>
        /// 全院护理上报安全事件汇总表
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// 
        public List<EntityNursEventStrument> GetStatNursEventInstrument(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            List<EntityNursEventStrument> data = new List<EntityNursEventStrument>();
            string startTime = string.Empty;
            string endTime = string.Empty;
            string ShcdFlg = string.Empty;

            SqlHelper svc = null;
            try
            {
                #region Sql
                Sql = @"select a.rptid,
                        a.reporttime,
                        extractvalue(b.xmldata, '/FormData/X242') as KS,
                        extractvalue(b.xmldata, '/FormData/X005') as FSRQSJ,
                        a.patno as ZYHZLH,
                        extractvalue(b.xmldata, '/FormData/X004') as CH,
                        a.patname as XM,
                        extractvalue(b.xmldata, '/FormData/PatientAge') as NL,
                        a.patsex as XB,
                        extractvalue(b.xmldata, '/FormData/X007') as AQSJDJ,
                        extract(b.xmldata, '/FormData/X081') as AQSJJG,
                        extractvalue(b.xmldata, '/FormData/X002') as SBZ,
                        extractvalue(b.xmldata, '/FormData/X174') as YHSJ,
                        extractvalue(b.xmldata, '/FormData/X175') as WSH,
                        extractvalue(b.xmldata, '/FormData/X240') as JDSH,
                        extractvalue(b.xmldata, '/FormData/X176') as ZDSH,
                        extractvalue(b.xmldata, '/FormData/X177') as CDSH,
                        extractvalue(b.xmldata, '/FormData/X178') as JCDSH,
                        extractvalue(b.xmldata, '/FormData/X179') as SW,
                        extractvalue(b.xmldata, '/FormData/X017') as CDBHG,
						extractvalue(b.xmldata, '/FormData/X018') as JQSBJCYPBHG,
						extractvalue(b.xmldata, '/FormData/X019') as BBDSSH,
                        extractvalue(b.xmldata, '/FormData/X020') as SFSBCW,
                        extractvalue(b.xmldata, '/FormData/X021') as SYYWCW,
                        extractvalue(b.xmldata, '/FormData/X022') as WJWPBHG,
                        extractvalue(b.xmldata, '/FormData/X023') as FFBHGXDHMJWP,
						extractvalue(b.xmldata, '/FormData/X024') as FSZHMJWPSJ,
						extractvalue(b.xmldata, '/FormData/X025') as FSYMJJXXDSJ,
                        extractvalue(b.xmldata, '/FormData/X026') as GCYLJCSH,
                        extractvalue(b.xmldata, '/FormData/X027') as BNJXWPPZCW,
                        extractvalue(b.xmldata, '/FormData/X029') as YWWS,
						extractvalue(b.xmldata, '/FormData/X031') as FJHXBG,
						extractvalue(b.xmldata, '/FormData/X032') as FJHXBG_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X035') as YWSC,
						extractvalue(b.xmldata, '/FormData/X037') as WX,
						extractvalue(b.xmldata, '/FormData/X038') as WX_DETAIL,
						extractvalue(b.xmldata, '/FormData/B002') as SYFY,
						extractvalue(b.xmldata, '/FormData/B001') as SYFY_DETAIL,
						extractvalue(b.xmldata, '/FormData/X041') as TD,
						extractvalue(b.xmldata, '/FormData/X042') as ZC,
						extractvalue(b.xmldata, '/FormData/X043') as ZS,
						extractvalue(b.xmldata, '/FormData/X044') as CXZGJJYJRWS,
						extractvalue(b.xmldata, '/FormData/X046') as DVTPET,
						extractvalue(b.xmldata, '/FormData/X048') as XSESS,
						extractvalue(b.xmldata, '/FormData/X049') as XSEBZGYS,
                        extractvalue(b.xmldata, '/FormData/X051') as CHCX,
						extractvalue(b.xmldata, '/FormData/X052') as CHCX_DETAIL,
						extractvalue(b.xmldata, '/FormData/X053') as YDFMXSECS,
                        extractvalue(b.xmldata, '/FormData/X054') as YDFMXSECS_DETAIL,
						extractvalue(b.xmldata, '/FormData/X055') as YDFMCFNCL,
						extractvalue(b.xmldata, '/FormData/X056') as SYCCSBFZ,
                        extractvalue(b.xmldata, '/FormData/X050') as HYLS,
                        extractvalue(b.xmldata, '/FormData/X059') as SSCD,
                        extractvalue(b.xmldata, '/FormData/X060') as SSCD_DETAIL,
                        extractvalue(b.xmldata, '/FormData/X061') as SSGCYWYL,
                        extractvalue(b.xmldata, '/FormData/X063') as SSBBCL,
                        extractvalue(b.xmldata, '/FormData/X243') as SSBBCL_DETAIL,
						extractvalue(b.xmldata, '/FormData/X057') as ZXJMDGXLGR,
                        extractvalue(b.xmldata, '/FormData/X058') as SYHXJWWBZQ,
                        extractvalue(b.xmldata, '/FormData/X064') as JZFZBHG,
                        extractvalue(b.xmldata, '/FormData/X066') as YSZBQBH,
                        extractvalue(b.xmldata, '/FormData/X067') as SZLY,
                        extractvalue(b.xmldata, '/FormData/X068') as ZC_1,
                        extractvalue(b.xmldata, '/FormData/X069') as ZS_1,
                        extractvalue(b.xmldata, '/FormData/X070') as CS,
                        extractvalue(b.xmldata, '/FormData/X071') as SQ,
                        extractvalue(b.xmldata, '/FormData/X072') as DSJF,
                        extractvalue(b.xmldata, '/FormData/X073') as BLXW,
                        extractvalue(b.xmldata, '/FormData/X080') as QTSJ_8,
						extractvalue(b.xmldata, '/FormData/B003') as YWSH,
						extractvalue(b.xmldata, '/FormData/C001') as BFZ,
                        extractvalue(b.xmldata, '/FormData/X003') as RANK,
                        extractvalue(b.xmldata, '/FormData/X180') as SBYY_1,
                        extractvalue(b.xmldata, '/FormData/X193') as SBYY_2,
                        extractvalue(b.xmldata, '/FormData/X197') as SBYY_3,
                        extractvalue(b.xmldata, '/FormData/X200') as SBYY_4,
                        extractvalue(b.xmldata, '/FormData/X205') as SBYY_5,
                        extractvalue(b.xmldata, '/FormData/X210') as SBYY_6,
                        extractvalue(b.xmldata, '/FormData/X211') as SBYY_7
                        from icare.rptEvent a 
                        left join  icare.rpteventdata b 
                        on a.rptid = b.rptid
                        left join t_bse_deptdesc c 
                        on a.deptcode = c.code_vchr
                        where a.formid = 26      
                        and a.status = 1 ";

                svc = new SqlHelper(EnumBiz.onlineDB);
                string strSub = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();
               
                foreach (EntityParm po in dicParm)
                {
                    string keyValue = po.value;

                    switch (po.key)
                    {
                        case "reportDate":
                            IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0] + " 00:00:00";
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            strSub += " and (a.reporttime between ? and ?)";
                            break;
                        case "deptCode":
                            strSub += " and (a.reportdeptcode = '" + keyValue + "')";
                            break;
                        case "level":
                            strSub += " and extractvalue(b.xmldata, '/FormData/X007') = '" + keyValue + "'";
                            break;
                        case "TypeCode":
                            strSub += " and extractvalue(b.xmldata, '/FormData/" + keyValue + "') = '1'";
                            break;
                        case "tspan":
                            startTime = keyValue.Split('-')[0];
                            endTime = keyValue.Split('-')[1];
                            break;
                        case "Rank":
                            strSub += " and extractvalue(b.xmldata, '/FormData/X003') = '" + keyValue + "'";
                            break;
                        case "Shcd":
                            strSub += " and extractvalue(b.xmldata, '/FormData/" + keyValue + "') = '1'";
                            break;
                        case "Sbyy":
                            strSub += " and extractvalue(b.xmldata, '/FormData/" + keyValue + "') = '1'";
                            break;
                        default:
                            break;
                    }
                }

                Sql += strSub;
                Sql += " order by a.reporttime";
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());

                #region 赋值
                string month = string.Empty;
                string xmlData = string.Empty;
                Dictionary<string, string> dicData = new Dictionary<string, string>();
                int i = 1;

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityNursEventStrument voClone = new EntityNursEventStrument();
                        voClone.XH = i;
                        i++;
                        voClone.KS = dr["KS"].ToString();
                        voClone.FSRQSJ = Function.Datetime(dr["FSRQSJ"]).ToString("yyyy-MM-dd HH:mm");

                        if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(voClone.FSRQSJ))
                        {
                            TimeSpan tStart = DateTime.Parse(startTime+":00").TimeOfDay;
                            TimeSpan tEnd = DateTime.Parse(endTime+":59").TimeOfDay;

                            DateTime t1 = Convert.ToDateTime(voClone.FSRQSJ);
                            TimeSpan dspNow = t1.TimeOfDay;

                            if (dspNow > tStart && dspNow < tEnd)
                            {

                            }
                            else
                                continue;
                        }

                        voClone.ZYHZLH = dr["ZYHZLH"].ToString();
                        voClone.CH = dr["CH"].ToString();
                        voClone.XM = dr["XM"].ToString();
                        voClone.NL = dr["NL"].ToString();
                        //voClone.XB = dr["XB"].ToString();
                        if (dr["XB"].ToString() == "1")
                            voClone.XB = "男";
                        else if (dr["XB"].ToString() == "2")
                            voClone.XB = "女";
                        else
                            voClone.XB = dr["XB"].ToString();
                        voClone.AQSJDJ = dr["AQSJDJ"].ToString();
                        voClone.AQSJJG = dr["AQSJJG"].ToString();
                        voClone.AQSJJG = voClone.AQSJJG.Replace("<X081><![CDATA[", "");
                        voClone.AQSJJG = voClone.AQSJJG.Replace("]]></X081>", "");
                        voClone.SBZ = dr["SBZ"].ToString();

                        //隐患事件
                        if (dr["YHSJ"].ToString() == "1")
                            voClone.AQSJSHCD += "隐患事件";
                        //无伤害事件
                        if (dr["WSH"].ToString() == "1")
                            voClone.AQSJSHCD += "无伤害事件";
                        //轻度伤害事件
                        if (dr["JDSH"].ToString() == "1")
                            voClone.AQSJSHCD += "轻度伤害事件";
                        //中度伤害事件
                        if (dr["ZDSH"].ToString() == "1")
                            voClone.AQSJSHCD += "中度伤害事件";
                        //重度伤害事件
                        if (dr["CDSH"].ToString() == "1")
                            voClone.AQSJSHCD += "重度伤害事件";
                        //极重度伤害事件
                        if (dr["JCDSH"].ToString() == "1")
                            voClone.AQSJSHCD += "极重度伤害事件";
                        //死亡
                        if (dr["SW"].ToString() == "1")
                            voClone.AQSJSHCD += "死亡";

                        #region 安全事件类型
                        //查对不合格
                        if (dr["CDBHG"].ToString() == "1")
                            voClone.AQSJLX += "查对不合格；";
                        //身份识别错误（患者身份查对）
                        if (dr["SFSBCW"].ToString() == "1")
                            voClone.AQSJLX += "身份识别错误（患者身份查对）；";
                        //使用药物错误（发生在患者身上）
                        if (dr["SYYWCW"].ToString() == "1")
                            voClone.AQSJLX += "使用药物错误（发生在患者身上）；";
                        //标本丢失、损毁 
                        if (dr["BBDSSH"].ToString() == "1")
                            voClone.AQSJLX += "标本丢失、损毁； ";
                        //急救设备器材药品不合格
                        if (dr["JQSBJCYPBHG"].ToString() == "1")
                            voClone.AQSJLX += "急救设备器材药品不合格；";
                        //无菌物品不合格
                        if (dr["WJWPBHG"].ToString() == "1")
                            voClone.AQSJLX += "无菌物品不合格；";
                        //发放不合格的消毒或灭菌物品
                        if (dr["FFBHGXDHMJWP"].ToString() == "1")
                            voClone.AQSJLX += "发放不合格的消毒或灭菌物品；";
                        //贵重医疗器材损毁或丢失
                        if (dr["GCYLJCSH"].ToString() == "1")
                            voClone.AQSJLX += "贵重医疗器材损毁或丢失；";
                        //发生召回灭菌物品事件
                        if (dr["FSZHMJWPSJ"].ToString() == "1")
                            voClone.AQSJLX += "发生召回灭菌物品事件；";
                        //包内器械物品配置错误影响手术进程
                        if (dr["BNJXWPPZCW"].ToString() == "1")
                            voClone.AQSJLX += "包内器械物品配置错误影响手术进程；";
                        //发生与灭菌器械相关的感染事件
                        if (dr["FSYMJJXXDSJ"].ToString() == "1")
                            voClone.AQSJLX += "发生与灭菌器械相关的感染事件；";
                        //药物外渗
                        if (dr["YWWS"].ToString() == "1")
                            voClone.AQSJLX += "药物外渗；";
                        //药物渗出
                        if (dr["YWSC"].ToString() == "1")
                            voClone.AQSJLX += "药物渗出；";
                        //输液反应
                        if (dr["SYFY"].ToString() == "1" || dr["SYFY_DETAIL"].ToString() != "")
                            voClone.AQSJLX += "输液反应；";
                        //非计划性拔管
                        if (dr["FJHXBG"].ToString() == "1" || dr["FJHXBG_DETAIL"].ToString() != "")
                            voClone.AQSJLX += "非计划性拔管；";
                        //跌倒
                        if (dr["TD"].ToString() == "1")
                            voClone.AQSJLX += "跌倒；";
                        //坠床 
                        if (dr["ZC"].ToString() == "1")
                            voClone.AQSJLX += "坠床；";
                        //走失
                        if (dr["ZS"].ToString() == "1")
                            voClone.AQSJLX += "走失；";
                        //误吸
                        if (dr["WX"].ToString() == "1" || dr["WX_DETAIL"].ToString() != "")
                            voClone.AQSJLX += "误吸；";
                        //足下垂/关节僵硬/肌肉萎缩
                        if (dr["CXZGJJYJRWS"].ToString() == "1")
                            voClone.AQSJLX += "足下垂/关节僵硬/肌肉萎缩；";
                        //DVT/PET 
                        if (dr["DVTPET"].ToString() == "1")
                            voClone.AQSJLX += "DVT/PET；";
                        //新生儿烧伤、烫伤
                        if (dr["XSESS"].ToString() == "1")
                            voClone.AQSJLX += "新生儿烧伤、烫伤；";
                        //新生儿鼻中隔压伤
                        if (dr["CHCX"].ToString() == "1" || dr["CHCX_DETAIL"].ToString() != "")
                            voClone.AQSJLX += "新生儿鼻中隔压伤；";
                        //阴道分娩新生儿产伤
                        if (dr["YDFMXSECS"].ToString() == "1" || dr["YDFMXSECS_DETAIL"].ToString() != "")
                            voClone.AQSJLX += "阴道分娩新生儿产伤；";
                        //阴道分娩产妇尿潴留
                        if (dr["YDFMCFNCL"].ToString() == "1")
                            voClone.AQSJLX += "阴道分娩产妇尿潴留；";
                        //使用催产素并发症
                       if (dr["SYCCSBFZ"].ToString() == "1")
                           voClone.AQSJLX += "使用催产素并发症；";
                       //会阴裂伤
                       if (dr["HYLS"].ToString() == "1")
                           voClone.AQSJLX += "会阴裂伤；";
                       //手术查对不合格
                       if (dr["SSCD"].ToString() == "1" || dr["SSCD_DETAIL"].ToString() != "")
                           voClone.AQSJLX += "手术查对不合格；";
                       //手术过程异物遗留
                       if (dr["SSGCYWYL"].ToString() == "1")
                           voClone.AQSJLX += "手术过程异物遗留；";
                       //手术标本处理不合格
                       if (dr["SSBBCL"].ToString() == "1" || dr["SSBBCL_DETAIL"].ToString() != "")
                           voClone.AQSJLX += "手术标本处理不合格；";
                       //中心静脉导管相关血流感染
                       if (dr["ZXJMDGXLGR"].ToString() == "1")
                           voClone.AQSJLX += "中心静脉导管相关血流感染；";
                       //使用呼吸机卧位不正确
                       if (dr["SYHXJWWBZQ"].ToString() == "1")
                           voClone.AQSJLX += "使用呼吸机卧位不正确；";
                       //急诊分诊不合格
                       if (dr["JZFZBHG"].ToString() == "1")
                           voClone.AQSJLX += "急诊分诊不合格；";
                        //运送中病情变化    
                       if (dr["YSZBQBH"].ToString() == "1")
                           voClone.AQSJLX += "运送中病情变化；";
                       //擅自离院 
                       if (dr["SZLY"].ToString() == "1")
                           voClone.AQSJLX += "擅自离院 ；";
                       //自残   
                       if (dr["ZC_1"].ToString() == "1")
                           voClone.AQSJLX += "自残 ；";
                        //自杀   
                        if (dr["ZS_1"].ToString() == "1")
                            voClone.AQSJLX += "自杀；";
                        //猝死  
                        if (dr["CS"].ToString() == "1")
                            voClone.AQSJLX += "猝死；";
                        //失窃    
                        if (dr["SQ"].ToString() == "1")
                            voClone.AQSJLX += "失窃；";
                        //投诉纠纷 
                        if (dr["DSJF"].ToString() == "1")
                            voClone.AQSJLX += "投诉纠纷；";
                        //暴力行为      
                        if (dr["BLXW"].ToString() == "1")
                            voClone.AQSJLX += "暴力行为；";
                        //意外伤害    
                        if (dr["YWSH"].ToString() == "1")
                            voClone.AQSJLX += "意外伤害";
                        //意外伤害    
                        if (dr["BFZ"].ToString() == "1")
                            voClone.AQSJLX += "并发症";
                        //其他事件    
                        if (dr["QTSJ_8"].ToString() == "1" )
                            voClone.AQSJLX += "其他事件";

                        #endregion
                #endregion

                        data.Add(voClone);
                    }
                }
                #endregion

            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }

        #endregion

        #endregion

        #region  发生时间段
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetSpanTime()
        {
            string Sql = string.Empty;
            Dictionary<string, string> data = new Dictionary<string, string>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select itemcode, itemname from diccommon where classid = 28 and status = 1";
                #endregion

                DataTable dt = svc.GetDataTable(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        data.Add(dr["itemcode"].ToString(), dr["itemname"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return data;
        }
        #endregion

        #region  护理皮肤损害安全事件

        #region 护理皮肤损害安全（不良）事件统计
        /// <summary>
        /// 护理皮肤损害安全（不良）事件统计
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// 
        public List<EntityNursEventSkin> GetStatNursEventSkin(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            string Sql2 = string.Empty;
            List<EntityNursEventSkin> data = new List<EntityNursEventSkin>();
            List<string> lstBw = new List<string> { "骶尾椎骨处" ,"坐骨处","股骨粗隆处","跟骨处","肘部" ,"其它部位" , "多部位"};
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                #region Sql
                Sql1 = @"select a.rptid,
                           a.reporttime,
                           extractvalue(b.xmldata, '/FormData/X190') as KS,
                           extractvalue(b.xmldata, '/FormData/X004') as FSRQSJ,
                           a.patno as ZYHZLH,
                           extractvalue(b.xmldata, '/FormData/X002') as CH,
                           a.patname as XM,
                           extractvalue(b.xmldata, '/FormData/PatientAge') as NL,
                           a.patsex as XB,
                           extract(b.xmldata, '/FormData/X032') as AQSJJG,
                           extractvalue(b.xmldata, '/FormData/X184') as SBZ,
       
                           --院内事件类型
                           extractvalue(b.xmldata, '/FormData/X018') as YNYLXPFSS, --院内压力性皮肤损伤
                           extractvalue(b.xmldata, '/FormData/X028') as YNSJXPY, --院内失禁性皮炎
                           extractvalue(b.xmldata, '/FormData/X029') as YYXPFSS, --医源性皮肤损伤
                           extractvalue(b.xmldata, '/FormData/X030') as YYXPFSS_DETAIL, --医源性皮肤损伤列表
                           --院内部位
                           trim(extractvalue(b.xmldata, '/FormData/X022')) as YNBW1,
                           trim(extractvalue(b.xmldata, '/FormData/X025')) as YNBW2,
                           --院内分期
                           trim(extractvalue(b.xmldata, '/FormData/X023')) as YNFQ1,
                           trim(extractvalue(b.xmldata, '/FormData/X026')) as YNFQ2,
       
                           --院外事件类型
                           '' as YWYLXSS, --院外压力性损伤
                           '' as YWSJXPY, --院外失禁性皮炎（小便/大便/大小便）
                           '' as FYYXYWPFSS, --（非医源性）意外皮肤损伤
                           --院外部位
                           '' as YWBW1,
                           '' as YWBW2,
                           '' as YWBW3,
                           '' as YWBW4,
                           '' as YWBW5,
                           '' as YWBW6,
                           --院外分期
                           '' as YWFQ1,
                           '' as YWFQ2,
                           '' as YWFQ3,
                           '' as YWFQ4,
                           '' as YWFQ5,
                           '' as YWFQ6,
                           '' as YWHZLY,
                           trim(extractvalue(b.xmldata, '/FormData/A001')) as KZLG_ZY,
                           trim(extractvalue(b.xmldata, '/FormData/A002')) as KZLG_HZ,
                           trim(extractvalue(b.xmldata, '/FormData/A003')) as KZLG_WY,
                           trim(extractvalue(b.xmldata, '/FormData/A004')) as KZLG_JZ,
                           trim(extractvalue(b.xmldata, '/FormData/A120')) as KZLG_CY,
                           trim(extractvalue(b.xmldata, '/FormData/A121')) as KZLG_ZY1,
                           trim(extractvalue(b.xmldata, '/FormData/A122')) as KZLG_SW
                      from icare.rptEvent a
                      left join icare.rpteventdata b
                        on a.rptid = b.rptid
                      left join t_bse_deptdesc c
                        on a.deptcode = c.code_vchr
                     where a.eventid = 20
                       and a.status = 1 
                       and (a.reporttime between ? and ? ) ";

                Sql2 = @"select a.rptid,
                           a.reporttime,
                           extractvalue(d.xmldata, '/FormData/X190') as KS,
                           extractvalue(d.xmldata, '/FormData/X004') as FSRQSJ,
                           a.patno as ZYHZLH,
                           extractvalue(d.xmldata, '/FormData/X002') as CH,
                           a.patname as XM,
                           extractvalue(d.xmldata, '/FormData/PatientAge') as NL,
                           a.patsex as XB,
                           extract(d.xmldata, '/FormData/X032') as AQSJJG,
                           extractvalue(d.xmldata, '/FormData/X184') as SBZ,
       
                           --院内事件类型
                           '' as YNYLXPFSS, --院内压力性皮肤损伤
                           '' as YNSJXPY, --院内失禁性皮炎
                           '' as YYXPFSS, --医源性皮肤损伤
                           '' as YYXPFSS_DETAIL, --医源性皮肤损伤列表
                           --院内部位
                           '' as YNBW1,
                           '' as YNBW2,
                           --院内分期
                           '' as YNFQ1,
                           '' as YNFQ2,
                           --院外事件类型
                           extractvalue(d.xmldata, '/FormData/X007') as YWYLXSS, --院外压力性损伤
                           extractvalue(d.xmldata, '/FormData/X017') as YWSJXPY, --院外失禁性皮炎（小便/大便/大小便）
                           extractvalue(d.xmldata, '/FormData/X031') as FYYXYWPFSS, --（非医源性）意外皮肤损伤
                           --院外部位
                           trim(extractvalue(d.xmldata, '/FormData/X008')) as YWBW1,
                           trim(extractvalue(d.xmldata, '/FormData/X011')) as YWBW2,
                           trim(extractvalue(d.xmldata, '/FormData/X014')) as YWBW3,
                           trim(extractvalue(d.xmldata, '/FormData/A104')) as YWBW4,
                           trim(extractvalue(d.xmldata, '/FormData/A105')) as YWBW5,
                           trim(extractvalue(d.xmldata, '/FormData/A106')) as YWBW6,
                           --院外分期
                           trim(extractvalue(d.xmldata, '/FormData/X009')) as YWFQ1,
                           trim(extractvalue(d.xmldata, '/FormData/X012')) as YWFQ2,
                           trim(extractvalue(d.xmldata, '/FormData/X015')) as YWFQ3,
                           trim(extractvalue(d.xmldata, '/FormData/A107')) as YWFQ4,
                           trim(extractvalue(d.xmldata, '/FormData/A108')) as YWFQ5,
                           trim(extractvalue(d.xmldata, '/FormData/A109')) as YWFQ6,
                           trim(extractvalue(d.xmldata, '/FormData/A125')) as YWHZLY,
                           trim(extractvalue(d.xmldata, '/FormData/A001')) as KZLG_ZY,
                           trim(extractvalue(d.xmldata, '/FormData/A002')) as KZLG_HZ,
                           trim(extractvalue(d.xmldata, '/FormData/A003')) as KZLG_WY,
                           trim(extractvalue(d.xmldata, '/FormData/A004')) as KZLG_JZ,
                           trim(extractvalue(d.xmldata, '/FormData/A120')) as KZLG_CY,
                           trim(extractvalue(d.xmldata, '/FormData/A121')) as KZLG_ZY1,
                           trim(extractvalue(d.xmldata, '/FormData/A122')) as KZLG_SW
                      from icare.rptEvent a
                      left join icare.rpteventdata d
                        on a.rptid = d.rptid
                      left join t_bse_deptdesc c
                        on a.deptcode = c.code_vchr
                     where a.eventid = 21
                       and a.status = 1 
                       and (a.reporttime between ? and ? )";
                #endregion

                IDataParameter [] parm = svc.CreateParm(4);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                parm[2].Value = beginDate + " 00:00:00";
                parm[3].Value = endDate + " 23:59:59";

                Sql = @"select * from (" + Sql1 + Environment.NewLine + "union all" + Environment.NewLine + Sql2 + ")" ;
                DataTable dt = svc.GetDataTable(Sql,parm);
                #region 赋值
                string month = string.Empty;
                string xmlData = string.Empty;
                Dictionary<string, string> dicData = new Dictionary<string, string>();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        month = Function.Datetime(dr["reporttime"].ToString()).ToString("yyyy-MM");

                        if (data.Any(t => t.TJYF == month))
                        {
                            #region 累计
                            EntityNursEventSkin voClone = data.FirstOrDefault(t => t.TJYF == month);
                            voClone.TJYF = month;
                            #region 事件类型
                            //院外压力性损伤
                            if (dr["YWYLXSS"].ToString().Trim() == "1")
                                voClone.YWYLXSS += 1;
                            //院外失禁性皮炎（选择小便/大便/大小便）
                            if (dr["YWSJXPY"].ToString().Trim() == "1")
                                voClone.YWSJXPY += 1;

                            //院内压力性皮肤损伤（选择）
                            if (dr["YNYLXPFSS"].ToString().Trim() == "1")
                                voClone.YNYLXPFSS += 1;
                            //院内失禁性皮炎（选择小便/大便/大小便）
                            if (dr["YNSJXPY"].ToString().Trim() == "1")
                                voClone.YNSJXPY += 1;
                            //医源性皮肤损伤（选择）
                            if (dr["YYXPFSS"].ToString() == "1" || dr["YYXPFSS_DETAIL"].ToString() != "")
                                voClone.YYXPFSS += 1;
                            //（非医源性）意外皮肤损伤
                            if (dr["FYYXYWPFSS"].ToString().Trim() == "1")
                                voClone.FYYXYWPFSS += 1;
                            #endregion 

                            #region 院外分期
                            if (dr["YWFQ1"].ToString().Trim() == "1期")
                                voClone.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "1期")
                                voClone.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "1期")
                                voClone.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "1期")
                                voClone.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "1期")
                                voClone.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "1期")
                                voClone.YWYLXSSFQ_I += 1;

                            if (dr["YWFQ1"].ToString().Trim() == "2期")
                                voClone.YWYLXSSFQ_II += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "2期")
                                voClone.YWYLXSSFQ_II += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "2期")
                                voClone.YWYLXSSFQ_II += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "2期")
                                voClone.YWYLXSSFQ_II += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "2期")
                                voClone.YWYLXSSFQ_II += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "2期")
                                voClone.YWYLXSSFQ_II += 1;

                            if (dr["YWFQ1"].ToString().Trim() == "3期")
                                voClone.YWYLXSSFQ_III += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "3期")
                                voClone.YWYLXSSFQ_III += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "3期")
                                voClone.YWYLXSSFQ_III += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "3期")
                                voClone.YWYLXSSFQ_III += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "3期")
                                voClone.YWYLXSSFQ_III += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "3期")
                                voClone.YWYLXSSFQ_III += 1;

                            if (dr["YWFQ1"].ToString().Trim() == "4期")
                                voClone.YWYLXSSFQ_IV += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "4期")
                                voClone.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "4期")
                                voClone.YWYLXSSFQ_IV += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "4期")
                                voClone.YWYLXSSFQ_IV += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "4期")
                                voClone.YWYLXSSFQ_IV += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "4期")
                                voClone.YWYLXSSFQ_IV += 1;

                            if (dr["YWFQ1"].ToString().Trim() == "无法分期")
                                voClone.YWYLXSSFQ_WFFQ += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "无法分期")
                                voClone.YWYLXSSFQ_WFFQ += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "无法分期")
                                voClone.YWYLXSSFQ_WFFQ += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "无法分期")
                                voClone.YWYLXSSFQ_WFFQ += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "无法分期")
                                voClone.YWYLXSSFQ_WFFQ += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "无法分期")
                                voClone.YWYLXSSFQ_WFFQ += 1;

                            if (dr["YWFQ1"].ToString().Trim() == "深部组织损伤")
                                voClone.YWYLXSSFQ_SBZZSS += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "深部组织损伤")
                                voClone.YWYLXSSFQ_SBZZSS += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "深部组织损伤")
                                voClone.YWYLXSSFQ_SBZZSS += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "深部组织损伤")
                                voClone.YWYLXSSFQ_SBZZSS += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "深部组织损伤")
                                voClone.YWYLXSSFQ_SBZZSS += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "深部组织损伤")
                                voClone.YWYLXSSFQ_SBZZSS += 1;
                            #endregion

                            #region //院内分期
                            if (dr["YNFQ1"].ToString().Trim() == "1期")
                                voClone.YNYLXSSFQ_I += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "1期")
                                voClone.YNYLXSSFQ_I += 1;

                            if (dr["YNFQ1"].ToString().Trim() == "2期")
                                voClone.YNYLXSSFQ_II += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "2期")
                                voClone.YNYLXSSFQ_II += 1;

                            if (dr["YNFQ1"].ToString().Trim() == "3期")
                                voClone.YNYLXSSFQ_III += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "3期")
                                voClone.YNYLXSSFQ_III += 1;

                            if (dr["YNFQ1"].ToString().Trim() == "4期")
                                voClone.YNYLXSSFQ_IV += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "4期")
                                voClone.YNYLXSSFQ_IV += 1;

                            if (dr["YNFQ1"].ToString().Trim() == "无法分期")
                                voClone.YNYLXSSFQ_WFFQ += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "无法分期")
                                voClone.YNYLXSSFQ_WFFQ += 1;
                            #endregion

                            #region //院外压力性损伤部位
                            
                            if (dr["YWBW1"].ToString().Trim() == "骶尾椎骨处")
                                voClone.YWYLXSSBW_DWZGC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "骶尾椎骨处")
                                voClone.YWYLXSSBW_DWZGC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "骶尾椎骨处")
                                voClone.YWYLXSSBW_DWZGC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "骶尾椎骨处")
                                voClone.YWYLXSSBW_DWZGC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "骶尾椎骨处")
                                voClone.YWYLXSSBW_DWZGC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "骶尾椎骨处")
                                voClone.YWYLXSSBW_DWZGC += 1;


                            if (dr["YWBW1"].ToString().Trim() == "坐骨处")
                                voClone.YWYLXSSBW_ZGC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "坐骨处")
                                voClone.YWYLXSSBW_ZGC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "坐骨处")
                                voClone.YWYLXSSBW_ZGC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "坐骨处")
                                voClone.YWYLXSSBW_ZGC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "坐骨处")
                                voClone.YWYLXSSBW_ZGC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "坐骨处")
                                voClone.YWYLXSSBW_ZGC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "股骨粗隆处")
                                voClone.YWYLXSSBW_GGZLC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "股骨粗隆处")
                                voClone.YWYLXSSBW_GGZLC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "股骨粗隆处")
                                voClone.YWYLXSSBW_GGZLC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "股骨粗隆处")
                                voClone.YWYLXSSBW_GGZLC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "股骨粗隆处")
                                voClone.YWYLXSSBW_GGZLC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "股骨粗隆处")
                                voClone.YWYLXSSBW_GGZLC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "跟骨处")
                                voClone.YWYLXSSBW_KGC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "跟骨处")
                                voClone.YWYLXSSBW_KGC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "跟骨处")
                                voClone.YWYLXSSBW_KGC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "跟骨处")
                                voClone.YWYLXSSBW_KGC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "跟骨处")
                                voClone.YWYLXSSBW_KGC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "跟骨处")
                                voClone.YWYLXSSBW_KGC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "足踝处")
                                voClone.YWYLXSSBW_ZWC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "足踝处")
                                voClone.YWYLXSSBW_ZWC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "足踝处")
                                voClone.YWYLXSSBW_ZWC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "足踝处")
                                voClone.YWYLXSSBW_ZWC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "足踝处")
                                voClone.YWYLXSSBW_ZWC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "足踝处")
                                voClone.YWYLXSSBW_ZWC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "肩胛处")
                                voClone.YWYLXSSBW_JJC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "肩胛处")
                                voClone.YWYLXSSBW_JJC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "肩胛处")
                                voClone.YWYLXSSBW_JJC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "肩胛处")
                                voClone.YWYLXSSBW_JJC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "肩胛处")
                                voClone.YWYLXSSBW_JJC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "肩胛处")
                                voClone.YWYLXSSBW_JJC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "枕骨处")
                                voClone.YWYLXSSBW_CGC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "枕骨处")
                                voClone.YWYLXSSBW_CGC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "枕骨处")
                                voClone.YWYLXSSBW_CGC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "枕骨处")
                                voClone.YWYLXSSBW_CGC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "枕骨处")
                                voClone.YWYLXSSBW_CGC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "枕骨处")
                                voClone.YWYLXSSBW_CGC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "肘部")
                                voClone.YWYLXSSBW_CB += 1;
                            if (dr["YWBW2"].ToString().Trim() == "肘部")
                                voClone.YWYLXSSBW_CB += 1;
                            if (dr["YWBW3"].ToString().Trim() == "肘部")
                                voClone.YWYLXSSBW_CB += 1;
                            if (dr["YWBW4"].ToString().Trim() == "肘部")
                                voClone.YWYLXSSBW_CB += 1;
                            if (dr["YWBW5"].ToString().Trim() == "肘部")
                                voClone.YWYLXSSBW_CB += 1;
                            if (dr["YWBW6"].ToString().Trim() == "肘部")
                                voClone.YWYLXSSBW_CB += 1;

                            if (dr["YWBW1"].ToString().Trim() == "其它部位")
                                voClone.YWYLXSSBW_QTBW += 1;
                            if (dr["YWBW2"].ToString().Trim() == "其它部位")
                                voClone.YWYLXSSBW_QTBW += 1;
                            if (dr["YWBW3"].ToString().Trim() == "其它部位")
                                voClone.YWYLXSSBW_QTBW += 1;
                            if (dr["YWBW4"].ToString().Trim() == "其它部位")
                                voClone.YWYLXSSBW_QTBW += 1;
                            if (dr["YWBW5"].ToString().Trim() == "其它部位")
                                voClone.YWYLXSSBW_QTBW += 1;
                            if (dr["YWBW6"].ToString().Trim() == "其它部位")
                                voClone.YWYLXSSBW_QTBW += 1;

                            if (dr["YWBW1"].ToString().Trim() == "多部位")
                                voClone.YWYLXSSBW_DBW += 1;
                            if (dr["YWBW2"].ToString().Trim() == "多部位")
                                voClone.YWYLXSSBW_DBW += 1;
                            if (dr["YWBW3"].ToString().Trim() == "多部位")
                                voClone.YWYLXSSBW_DBW += 1;
                            if (dr["YWBW4"].ToString().Trim() == "多部位")
                                voClone.YWYLXSSBW_DBW += 1;
                            if (dr["YWBW5"].ToString().Trim() == "多部位")
                                voClone.YWYLXSSBW_DBW += 1;
                            if (dr["YWBW6"].ToString().Trim() == "多部位")
                                voClone.YWYLXSSBW_DBW += 1;

                            if (!lstBw.Contains(dr["YWBW6"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW6"].ToString().Trim()))
                                voClone.YWYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YWBW5"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW5"].ToString().Trim()))
                                voClone.YWYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YWBW4"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW4"].ToString().Trim()))
                                voClone.YWYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YWBW3"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW3"].ToString().Trim()))
                                voClone.YWYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YWBW2"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW2"].ToString().Trim()))
                                voClone.YWYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YWBW1"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW1"].ToString().Trim()))
                                voClone.YWYLXSSBW_QTBW += 1;

                            #endregion

                            #region //院内压力性损伤部位
                            
                            if (dr["YNBW1"].ToString().Trim() == "骶尾椎骨处")
                                voClone.YNYLXSSBW_DWZGC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "骶尾椎骨处")
                                voClone.YNYLXSSBW_DWZGC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "坐骨处")
                                voClone.YNYLXSSBW_ZGC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "坐骨处")
                                voClone.YNYLXSSBW_ZGC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "股骨粗隆处")
                                voClone.YNYLXSSBW_GGZLC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "股骨粗隆处")
                                voClone.YNYLXSSBW_GGZLC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "跟骨处")
                                voClone.YNYLXSSBW_KGC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "跟骨处")
                                voClone.YNYLXSSBW_KGC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "足踝处")
                                voClone.YNYLXSSBW_ZWC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "足踝处")
                                voClone.YNYLXSSBW_ZWC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "肩胛处")
                                voClone.YNYLXSSBW_JJC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "肩胛处")
                                voClone.YNYLXSSBW_JJC += 1;


                            if (dr["YNBW1"].ToString().Trim() == "枕骨处")
                                voClone.YNYLXSSBW_CGC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "枕骨处")
                                voClone.YNYLXSSBW_CGC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "肘部")
                                voClone.YNYLXSSBW_CB += 1;
                            if (dr["YNBW2"].ToString().Trim() == "肘部")
                                voClone.YNYLXSSBW_CB += 1;

                            if (dr["YNBW1"].ToString().Trim() == "其它部位")
                                voClone.YNYLXSSBW_QTBW += 1;
                            if (dr["YNBW2"].ToString().Trim() == "其它部位")
                                voClone.YNYLXSSBW_QTBW += 1;

                            if (dr["YNBW1"].ToString().Trim() == "多部位")
                                voClone.YNYLXSSBW_DBW += 1;
                            if (dr["YNBW2"].ToString().Trim() == "多部位")
                                voClone.YNYLXSSBW_DBW += 1;
                            if (!lstBw.Contains(dr["YNBW1"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YNBW1"].ToString().Trim()))
                                voClone.YNYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YNBW2"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YNBW2"].ToString().Trim()))
                                voClone.YNYLXSSBW_QTBW += 1;
                            #endregion

                            if (dr["YWHZLY"].ToString().Trim() == "自家庭入住时有压疮的患者")
                                voClone.YWHZLY_JT += 1;
                            if (dr["YWHZLY"].ToString().Trim() == "自养老院入住时有压疮的患者")
                                voClone.YWHZLY_YLY += 1;
                            if (dr["YWHZLY"].ToString().Trim() == "自其他医院转入住时有压疮的患者")
                                voClone.YWHZLY_QTYY += 1;
                            if (dr["YWHZLY"].ToString().Trim() == "自其他来源入住时有压疮的患者")
                                voClone.YWHZLY_QTLY += 1;

                            //跟踪转归
                            if (dr["KZLG_ZY"].ToString().Trim() == "1")
                                voClone.KZLG_ZY += 1;
                            if (dr["KZLG_HZ"].ToString().Trim() == "1")
                                voClone.KZLG_HZ += 1;
                            if (dr["KZLG_WY"].ToString().Trim() == "1")
                                voClone.KZLG_WY += 1;
                            if (dr["KZLG_JZ"].ToString().Trim() == "1")
                                voClone.KZLG_JZ += 1;
                            if (dr["KZLG_CY"].ToString().Trim() == "1")
                                voClone.KZLG_CY += 1;
                            if (dr["KZLG_ZY1"].ToString().Trim() == "1")
                                voClone.KZLG_ZY1 += 1;
                            if (dr["KZLG_SW"].ToString().Trim() == "1")
                                voClone.KZLG_SW += 1;
                           
                            #endregion
                        }
                        else
                        {
                            #region vo
                            EntityNursEventSkin vo = new EntityNursEventSkin();
                            /// 月份
                            vo.TJYF = month;

                            #region 事件类型
                            //院外压力性损伤
                            if (dr["YWYLXSS"].ToString().Trim() == "1")
                                vo.YWYLXSS += 1;
                            //院外失禁性皮炎（选择小便/大便/大小便）
                            if (dr["YWSJXPY"].ToString().Trim() == "1")
                                vo.YWSJXPY += 1;

                            //院内压力性皮肤损伤（选择）
                            if (dr["YNYLXPFSS"].ToString().Trim() == "1")
                                vo.YNYLXPFSS += 1;
                            //院内失禁性皮炎（选择小便/大便/大小便）
                            if (dr["YNSJXPY"].ToString().Trim() == "1")
                                vo.YNSJXPY += 1;
                            //医源性皮肤损伤（选择）
                            if (dr["YYXPFSS"].ToString() == "1" || dr["YYXPFSS_DETAIL"].ToString() != "")
                                vo.YYXPFSS += 1;
                            //（非医源性）意外皮肤损伤
                            if (dr["FYYXYWPFSS"].ToString().Trim() == "1")
                                vo.FYYXYWPFSS += 1;
                            #endregion

                            #region 院外分期
                            if (dr["YWFQ1"].ToString().Trim() == "1期")
                                vo.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "1期")
                                vo.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "1期")
                                vo.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "1期")
                                vo.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "1期")
                                vo.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "1期")
                                vo.YWYLXSSFQ_I += 1;

                            if (dr["YWFQ1"].ToString().Trim() == "2期")
                                vo.YWYLXSSFQ_II += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "2期")
                                vo.YWYLXSSFQ_II += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "2期")
                                vo.YWYLXSSFQ_II += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "2期")
                                vo.YWYLXSSFQ_II += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "2期")
                                vo.YWYLXSSFQ_II += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "2期")
                                vo.YWYLXSSFQ_II += 1;

                            if (dr["YWFQ1"].ToString().Trim() == "3期")
                                vo.YWYLXSSFQ_III += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "3期")
                                vo.YWYLXSSFQ_III += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "3期")
                                vo.YWYLXSSFQ_III += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "3期")
                                vo.YWYLXSSFQ_III += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "3期")
                                vo.YWYLXSSFQ_III += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "3期")
                                vo.YWYLXSSFQ_III += 1;

                            if (dr["YWFQ1"].ToString().Trim() == "4期")
                                vo.YWYLXSSFQ_IV += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "4期")
                                vo.YWYLXSSFQ_I += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "4期")
                                vo.YWYLXSSFQ_IV += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "4期")
                                vo.YWYLXSSFQ_IV += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "4期")
                                vo.YWYLXSSFQ_IV += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "4期")
                                vo.YWYLXSSFQ_IV += 1;

                            if (dr["YWFQ1"].ToString().Trim() == "无法分期")
                                vo.YWYLXSSFQ_WFFQ += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "无法分期")
                                vo.YWYLXSSFQ_WFFQ += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "无法分期")
                                vo.YWYLXSSFQ_WFFQ += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "无法分期")
                                vo.YWYLXSSFQ_WFFQ += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "无法分期")
                                vo.YWYLXSSFQ_WFFQ += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "无法分期")
                                vo.YWYLXSSFQ_WFFQ += 1;

                            if (dr["YWFQ1"].ToString().Trim() == "深部组织损伤")
                                vo.YWYLXSSFQ_SBZZSS += 1;
                            if (dr["YWFQ2"].ToString().Trim() == "深部组织损伤")
                                vo.YWYLXSSFQ_SBZZSS += 1;
                            if (dr["YWFQ3"].ToString().Trim() == "深部组织损伤")
                                vo.YWYLXSSFQ_SBZZSS += 1;
                            if (dr["YWFQ4"].ToString().Trim() == "深部组织损伤")
                                vo.YWYLXSSFQ_SBZZSS += 1;
                            if (dr["YWFQ5"].ToString().Trim() == "深部组织损伤")
                                vo.YWYLXSSFQ_SBZZSS += 1;
                            if (dr["YWFQ6"].ToString().Trim() == "深部组织损伤")
                                vo.YWYLXSSFQ_SBZZSS += 1;
                            #endregion

                            #region //院内分期
                            if (dr["YNFQ1"].ToString().Trim() == "1期")
                                vo.YNYLXSSFQ_I += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "1期")
                                vo.YNYLXSSFQ_I += 1;

                            if (dr["YNFQ1"].ToString().Trim() == "2期")
                                vo.YNYLXSSFQ_II += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "2期")
                                vo.YNYLXSSFQ_II += 1;

                            if (dr["YNFQ1"].ToString().Trim() == "3期")
                                vo.YNYLXSSFQ_III += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "3期")
                                vo.YNYLXSSFQ_III += 1;

                            if (dr["YNFQ1"].ToString().Trim() == "4期")
                                vo.YNYLXSSFQ_IV += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "4期")
                                vo.YNYLXSSFQ_IV += 1;

                            if (dr["YNFQ1"].ToString().Trim() == "无法分期")
                                vo.YNYLXSSFQ_WFFQ += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "无法分期")
                                vo.YNYLXSSFQ_WFFQ += 1;
                            if (dr["YNFQ1"].ToString().Trim() == "深部组织损伤")
                                vo.YNYLXSSFQ_SBZZSS += 1;
                            if (dr["YNFQ2"].ToString().Trim() == "深部组织损伤")
                                vo.YNYLXSSFQ_SBZZSS += 1;
                            #endregion

                            #region //院外压力性损伤部位
                            
                            if (dr["YWBW1"].ToString().Trim() == "骶尾椎骨处")
                                vo.YWYLXSSBW_DWZGC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "骶尾椎骨处")
                                vo.YWYLXSSBW_DWZGC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "骶尾椎骨处")
                                vo.YWYLXSSBW_DWZGC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "骶尾椎骨处")
                                vo.YWYLXSSBW_DWZGC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "骶尾椎骨处")
                                vo.YWYLXSSBW_DWZGC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "骶尾椎骨处")
                                vo.YWYLXSSBW_DWZGC += 1;


                            if (dr["YWBW1"].ToString().Trim() == "坐骨处")
                                vo.YWYLXSSBW_ZGC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "坐骨处")
                                vo.YWYLXSSBW_ZGC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "坐骨处")
                                vo.YWYLXSSBW_ZGC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "坐骨处")
                                vo.YWYLXSSBW_ZGC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "坐骨处")
                                vo.YWYLXSSBW_ZGC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "坐骨处")
                                vo.YWYLXSSBW_ZGC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "股骨粗隆处")
                                vo.YWYLXSSBW_GGZLC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "股骨粗隆处")
                                vo.YWYLXSSBW_GGZLC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "股骨粗隆处")
                                vo.YWYLXSSBW_GGZLC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "股骨粗隆处")
                                vo.YWYLXSSBW_GGZLC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "股骨粗隆处")
                                vo.YWYLXSSBW_GGZLC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "股骨粗隆处")
                                vo.YWYLXSSBW_GGZLC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "跟骨处")
                                vo.YWYLXSSBW_KGC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "跟骨处")
                                vo.YWYLXSSBW_KGC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "跟骨处")
                                vo.YWYLXSSBW_KGC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "跟骨处")
                                vo.YWYLXSSBW_KGC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "跟骨处")
                                vo.YWYLXSSBW_KGC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "跟骨处")
                                vo.YWYLXSSBW_KGC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "足踝处")
                                vo.YWYLXSSBW_ZWC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "足踝处")
                                vo.YWYLXSSBW_ZWC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "足踝处")
                                vo.YWYLXSSBW_ZWC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "足踝处")
                                vo.YWYLXSSBW_ZWC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "足踝处")
                                vo.YWYLXSSBW_ZWC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "足踝处")
                                vo.YWYLXSSBW_ZWC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "肩胛处")
                                vo.YWYLXSSBW_JJC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "肩胛处")
                                vo.YWYLXSSBW_JJC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "肩胛处")
                                vo.YWYLXSSBW_JJC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "肩胛处")
                                vo.YWYLXSSBW_JJC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "肩胛处")
                                vo.YWYLXSSBW_JJC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "肩胛处")
                                vo.YWYLXSSBW_JJC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "枕骨处")
                                vo.YWYLXSSBW_CGC += 1;
                            if (dr["YWBW2"].ToString().Trim() == "枕骨处")
                                vo.YWYLXSSBW_CGC += 1;
                            if (dr["YWBW3"].ToString().Trim() == "枕骨处")
                                vo.YWYLXSSBW_CGC += 1;
                            if (dr["YWBW4"].ToString().Trim() == "枕骨处")
                                vo.YWYLXSSBW_CGC += 1;
                            if (dr["YWBW5"].ToString().Trim() == "枕骨处")
                                vo.YWYLXSSBW_CGC += 1;
                            if (dr["YWBW6"].ToString().Trim() == "枕骨处")
                                vo.YWYLXSSBW_CGC += 1;

                            if (dr["YWBW1"].ToString().Trim() == "肘部")
                                vo.YWYLXSSBW_CB += 1;
                            if (dr["YWBW2"].ToString().Trim() == "肘部")
                                vo.YWYLXSSBW_CB += 1;
                            if (dr["YWBW3"].ToString().Trim() == "肘部")
                                vo.YWYLXSSBW_CB += 1;
                            if (dr["YWBW4"].ToString().Trim() == "肘部")
                                vo.YWYLXSSBW_CB += 1;
                            if (dr["YWBW5"].ToString().Trim() == "肘部")
                                vo.YWYLXSSBW_CB += 1;
                            if (dr["YWBW6"].ToString().Trim() == "肘部")
                                vo.YWYLXSSBW_CB += 1;

                            if (dr["YWBW1"].ToString().Trim() == "其它部位")
                                vo.YWYLXSSBW_QTBW += 1;
                            if (dr["YWBW2"].ToString().Trim() == "其它部位")
                                vo.YWYLXSSBW_QTBW += 1;
                            if (dr["YWBW3"].ToString().Trim() == "其它部位")
                                vo.YWYLXSSBW_QTBW += 1;
                            if (dr["YWBW4"].ToString().Trim() == "其它部位")
                                vo.YWYLXSSBW_QTBW += 1;
                            if (dr["YWBW5"].ToString().Trim() == "其它部位")
                                vo.YWYLXSSBW_QTBW += 1;
                            if (dr["YWBW6"].ToString().Trim() == "其它部位")
                                vo.YWYLXSSBW_QTBW += 1;

                            if (dr["YWBW1"].ToString().Trim() == "多部位")
                                vo.YWYLXSSBW_DBW += 1;
                            if (dr["YWBW2"].ToString().Trim() == "多部位")
                                vo.YWYLXSSBW_DBW += 1;
                            if (dr["YWBW3"].ToString().Trim() == "多部位")
                                vo.YWYLXSSBW_DBW += 1;
                            if (dr["YWBW4"].ToString().Trim() == "多部位")
                                vo.YWYLXSSBW_DBW += 1;
                            if (dr["YWBW5"].ToString().Trim() == "多部位")
                                vo.YWYLXSSBW_DBW += 1;
                            if (dr["YWBW6"].ToString().Trim() == "多部位")
                                vo.YWYLXSSBW_DBW += 1;

                            if (!lstBw.Contains(dr["YWBW6"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW6"].ToString().Trim()))
                                vo.YWYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YWBW5"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW5"].ToString().Trim()))
                                vo.YWYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YWBW4"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW4"].ToString().Trim()))
                                vo.YWYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YWBW3"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW3"].ToString().Trim()))
                                vo.YWYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YWBW2"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW2"].ToString().Trim()))
                                vo.YWYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YWBW1"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YWBW1"].ToString().Trim()))
                                vo.YWYLXSSBW_QTBW += 1;

                            #endregion

                            #region //院内压力性损伤部位
                            

                            if (dr["YNBW1"].ToString().Trim() == "骶尾椎骨处")
                                vo.YNYLXSSBW_DWZGC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "骶尾椎骨处")
                                vo.YNYLXSSBW_DWZGC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "坐骨处")
                                vo.YNYLXSSBW_ZGC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "坐骨处")
                                vo.YNYLXSSBW_ZGC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "股骨粗隆处")
                                vo.YNYLXSSBW_GGZLC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "股骨粗隆处")
                                vo.YNYLXSSBW_GGZLC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "跟骨处")
                                vo.YNYLXSSBW_KGC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "跟骨处")
                                vo.YNYLXSSBW_KGC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "足踝处")
                                vo.YNYLXSSBW_ZWC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "足踝处")
                                vo.YNYLXSSBW_ZWC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "肩胛处")
                                vo.YNYLXSSBW_JJC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "肩胛处")
                                vo.YNYLXSSBW_JJC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "枕骨处")
                                vo.YNYLXSSBW_CGC += 1;
                            if (dr["YNBW2"].ToString().Trim() == "枕骨处")
                                vo.YNYLXSSBW_CGC += 1;

                            if (dr["YNBW1"].ToString().Trim() == "肘部")
                                vo.YNYLXSSBW_CB += 1;
                            if (dr["YNBW2"].ToString().Trim() == "肘部")
                                vo.YNYLXSSBW_CB += 1;

                            if (dr["YNBW1"].ToString().Trim() == "其它部位")
                                vo.YNYLXSSBW_QTBW += 1;
                            if (dr["YNBW2"].ToString().Trim() == "其它部位")
                                vo.YNYLXSSBW_QTBW += 1;

                            if (dr["YNBW1"].ToString().Trim() == "多部位")
                                vo.YNYLXSSBW_DBW += 1;
                            if (dr["YNBW2"].ToString().Trim() == "多部位")
                                vo.YNYLXSSBW_DBW += 1;

                            if (!lstBw.Contains(dr["YNBW1"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YNBW1"].ToString().Trim()))
                                vo.YNYLXSSBW_QTBW += 1;
                            if (!lstBw.Contains(dr["YNBW2"].ToString().Trim()) && !string.IsNullOrEmpty(dr["YNBW2"].ToString().Trim()))
                                vo.YNYLXSSBW_QTBW += 1;
                            #endregion

                            if (dr["YWHZLY"].ToString().Trim() == "自家庭入住时有压疮的患者")
                                vo.YWHZLY_JT += 1;
                            if (dr["YWHZLY"].ToString().Trim() == "自养老院入住时有压疮的患者")
                                vo.YWHZLY_YLY += 1;
                            if (dr["YWHZLY"].ToString().Trim() == "自其他医院转入住时有压疮的患者")
                                vo.YWHZLY_QTYY += 1;
                            if (dr["YWHZLY"].ToString().Trim() == "自其他来源入住时有压疮的患者")
                                vo.YWHZLY_QTLY += 1;

                            //跟踪转归
                            if (dr["KZLG_ZY"].ToString().Trim() == "1")
                                vo.KZLG_ZY += 1;
                            if (dr["KZLG_HZ"].ToString().Trim() == "1")
                                vo.KZLG_HZ += 1;
                            if (dr["KZLG_WY"].ToString().Trim() == "1")
                                vo.KZLG_WY += 1;
                            if (dr["KZLG_JZ"].ToString().Trim() == "1")
                                vo.KZLG_JZ += 1;
                            if (dr["KZLG_CY"].ToString().Trim() == "1")
                                vo.KZLG_CY += 1;
                            if (dr["KZLG_ZY1"].ToString().Trim() == "1")
                                vo.KZLG_ZY1 += 1;
                            if (dr["KZLG_SW"].ToString().Trim() == "1")
                                vo.KZLG_SW += 1;

                            #endregion

                            data.Add(vo);
                        }
                    }
                }
                #endregion

                #region 合计

                foreach (EntityNursEventSkin item in data)
                {

                    item.YWYLXSSFQ_HJ = item.YWYLXSSFQ_I + item.YWYLXSSFQ_II + item.YWYLXSSFQ_III + item.YWYLXSSFQ_IV +
                    item.YWYLXSSFQ_WFFQ + item.YWYLXSSFQ_SBZZSS;

                    item.YNYLXSSFQ_HJ = item.YNYLXSSFQ_I + item.YNYLXSSFQ_II + item.YNYLXSSFQ_III + item.YNYLXSSFQ_IV +
                        item.YNYLXSSFQ_WFFQ + item.YNYLXSSFQ_SBZZSS;

                    item.YWYLXSSBW_HJ = item.YWYLXSSBW_DWZGC + item.YWYLXSSBW_ZGC +
                        item.YWYLXSSBW_GGZLC + item.YWYLXSSBW_KGC + item.YWYLXSSBW_ZWC + item.YWYLXSSBW_JJC +
                        item.YWYLXSSBW_CGC + item.YWYLXSSBW_CB + item.YWYLXSSBW_QTBW + item.YWYLXSSBW_DBW;

                    item.YNYLXSSBW_HJ = item.YNYLXSSBW_DWZGC + item.YNYLXSSBW_ZGC + item.YNYLXSSBW_GGZLC +
                        item.YNYLXSSBW_KGC + item.YNYLXSSBW_ZWC + item.YNYLXSSBW_JJC + item.YNYLXSSBW_CGC +
                        item.YNYLXSSBW_CB + item.YNYLXSSBW_QTBW + item.YNYLXSSBW_DBW;

                    item.YWHZLY_HJ = item.YWHZLY_JT + item.YWHZLY_QTLY  + item.YWHZLY_QTYY + item.YWHZLY_YLY;
                    item.KZLG_HJ = item.KZLG_HZ + item.KZLG_JZ + item.KZLG_SW + item.KZLG_WY + item.KZLG_ZY + item.KZLG_ZY1 + item.KZLG_CY;
                    
                }
                if (data.Count > 0)
                {
                    EntityNursEventSkin vo = new EntityNursEventSkin();
                    vo.TJYF = "合计";

                    foreach (EntityNursEventSkin item in data)
                    {
                        vo.YWYLXSS += item.YWYLXSS;
                        vo.YWSJXPY += item.YWSJXPY;
                        vo.YNYLXPFSS += item.YNYLXPFSS;
                        vo.YNSJXPY += item.YNSJXPY;
                        vo.YYXPFSS += item.YYXPFSS;
                        vo.FYYXYWPFSS += item.FYYXYWPFSS;
                        vo.YWYLXSSFQ_I += item.YWYLXSSFQ_I;
                        vo.YWYLXSSFQ_II += item.YWYLXSSFQ_II;
                        vo.YWYLXSSFQ_III += item.YWYLXSSFQ_III;
                        vo.YWYLXSSFQ_IV += item.YWYLXSSFQ_IV;
                        vo.YWYLXSSFQ_WFFQ += item.YWYLXSSFQ_WFFQ;
                        vo.YWYLXSSFQ_SBZZSS += item.YWYLXSSFQ_SBZZSS;
                        vo.YWYLXSSFQ_HJ += item.YWYLXSSFQ_HJ;
                        vo.YNYLXSSFQ_I += item.YNYLXSSFQ_I;
                        vo.YNYLXSSFQ_II += item.YNYLXSSFQ_II;
                        vo.YNYLXSSFQ_III += item.YNYLXSSFQ_III;
                        vo.YNYLXSSFQ_IV += item.YNYLXSSFQ_IV;
                        vo.YNYLXSSFQ_WFFQ += item.YNYLXSSFQ_WFFQ;
                        vo.YNYLXSSFQ_SBZZSS += item.YNYLXSSFQ_SBZZSS;
                        vo.YNYLXSSFQ_HJ += item.YNYLXSSFQ_HJ;
                        vo.YWYLXSSBW_DWZGC += item.YWYLXSSBW_DWZGC;
                        vo.YWYLXSSBW_ZGC += item.YWYLXSSBW_ZGC;
                        vo.YWYLXSSBW_GGZLC += item.YWYLXSSBW_GGZLC;
                        vo.YWYLXSSBW_KGC += item.YWYLXSSBW_KGC;
                        vo.YWYLXSSBW_ZWC += item.YWYLXSSBW_ZWC;
                        vo.YWYLXSSBW_JJC += item.YWYLXSSBW_JJC;
                        vo.YWYLXSSBW_CGC += item.YWYLXSSBW_CGC;
                        vo.YWYLXSSBW_CB += item.YWYLXSSBW_CB;
                        vo.YWYLXSSBW_QTBW += item.YWYLXSSBW_QTBW;
                        vo.YWYLXSSBW_DBW += item.YWYLXSSBW_DBW;
                        vo.YWYLXSSBW_HJ += item.YWYLXSSBW_HJ;
                        vo.YNYLXSSBW_DWZGC += item.YNYLXSSBW_DWZGC;
                        vo.YNYLXSSBW_ZGC += item.YNYLXSSBW_ZGC;
                        vo.YNYLXSSBW_GGZLC += item.YNYLXSSBW_GGZLC;
                        vo.YNYLXSSBW_KGC += item.YNYLXSSBW_KGC;
                        vo.YNYLXSSBW_ZWC += item.YNYLXSSBW_ZWC;
                        vo.YNYLXSSBW_JJC += item.YNYLXSSBW_JJC;
                        vo.YNYLXSSBW_CGC += item.YNYLXSSBW_CGC;
                        vo.YNYLXSSBW_CB += item.YNYLXSSBW_CB;
                        vo.YNYLXSSBW_QTBW += item.YNYLXSSBW_QTBW;
                        vo.YNYLXSSBW_DBW += item.YNYLXSSBW_DBW;
                        vo.YNYLXSSBW_HJ += item.YNYLXSSBW_HJ;
                        vo.YWHZLY_HJ += item.YWHZLY_HJ;
                        vo.KZLG_HJ += item.KZLG_HJ;
                        vo.KZLG_ZY = item.KZLG_ZY;
                        vo.KZLG_HZ = item.KZLG_HZ;
                        vo.KZLG_WY = item.KZLG_WY;
                        vo.KZLG_JZ = item.KZLG_JZ;
                        vo.KZLG_CY = item.KZLG_CY;
                        vo.KZLG_ZY1 = item.KZLG_ZY1;
                        vo.KZLG_SW = item.KZLG_SW;
                        vo.YWHZLY_JT = item.YWHZLY_JT;
                        vo.YWHZLY_YLY = item.YWHZLY_YLY;
                        vo.YWHZLY_QTYY = item.YWHZLY_QTYY;
                        vo.YWHZLY_QTLY = item.YWHZLY_QTLY;
                    }
                    data.Add(vo);

                }

                data.OrderBy(a => a.TJYF).ToList();//降序
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }
        #endregion

        #region 汇总表
        /// <summary>
        /// 护理皮肤损害安全事件汇总表
        /// </summary>
        /// <returns></returns>
        /// 
        public List<EntitySkinEventStrument> GetStatSkinEventInstrument(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            string Sql2 = string.Empty;
            string bw = string.Empty;
            string fq = string.Empty;
            List<EntitySkinEventStrument> data = new List<EntitySkinEventStrument>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                Sql1 = @"select a.rptid,
                           a.reporttime,
                           extractvalue(b.xmldata, '/FormData/X190') as KS,
                           extractvalue(b.xmldata, '/FormData/X004') as FSRQSJ,
                           a.patno as ZYHZLH,
                           extractvalue(b.xmldata, '/FormData/X002') as CH,
                           a.patname as XM,
                           extractvalue(b.xmldata, '/FormData/PatientAge') as NL,
                           a.patsex as XB,
                           extract(b.xmldata, '/FormData/X032') as AQSJJG,
                           extractvalue(b.xmldata, '/FormData/X184') as SBZ,
       
                           --院内事件类型
                           extractvalue(b.xmldata, '/FormData/X018') as YNYLXPFSS, --院内压力性皮肤损伤
                           extractvalue(b.xmldata, '/FormData/X028') as YNSJXPY, --院内失禁性皮炎
                           extractvalue(b.xmldata, '/FormData/X029') as YYXPFSS, --医源性皮肤损伤
                           extractvalue(b.xmldata, '/FormData/X030') as YYXPFSS_DETAIL, --医源性皮肤损伤列表
                           --院内部位
                           trim(extractvalue(b.xmldata, '/FormData/X022')) as YNBW1,
                           trim(extractvalue(b.xmldata, '/FormData/X025')) as YNBW2,
                           --院内分期
                           trim(extractvalue(b.xmldata, '/FormData/X023')) as YNFQ1,
                           trim(extractvalue(b.xmldata, '/FormData/X026')) as YNFQ2,
       
                           --院外事件类型
                           '' as YWYLXSS, --院外压力性损伤
                           '' as YWSJXPY, --院外失禁性皮炎（小便/大便/大小便）
                           '' as FYYXYWSJXPY, --（非医源性）意外皮肤损伤
                           --院外部位
                           '' as YWBW1,
                           '' as YWBW2,
                           '' as YWBW3,
                           '' as YWBW4,
                           '' as YWBW5,
                           '' as YWBW6,
                           --院外分期
                           '' as YWFQ1,
                           '' as YWFQ2,
                           '' as YWFQ3,
                           '' as YWFQ4,
                           '' as YWFQ5,
                           '' as YWFQ6
                      from icare.rptEvent a
                      left join icare.rpteventdata b
                        on a.rptid = b.rptid
                      left join icare.rpteventdata d
                        on a.rptid = d.rptid
                      left join t_bse_deptdesc c
                        on a.deptcode = c.code_vchr
                     where a.eventid = 20
                       and a.status = 1 ";
                    Sql2 = @"select a.rptid,
                           a.reporttime,
                           extractvalue(d.xmldata, '/FormData/X190') as KS,
                           extractvalue(d.xmldata, '/FormData/X004') as FSRQSJ,
                           a.patno as ZYHZLH,
                           extractvalue(d.xmldata, '/FormData/X002') as CH,
                           a.patname as XM,
                           extractvalue(d.xmldata, '/FormData/PatientAge') as NL,
                           a.patsex as XB,
                           extract(d.xmldata, '/FormData/X032') as AQSJJG,
                           extractvalue(d.xmldata, '/FormData/X184') as SBZ,
       
                           --院内事件类型
                           '' as YNYLXPFSS, --院内压力性皮肤损伤
                           '' as YNSJXPY, --院内失禁性皮炎
                           '' as YYXPFSS, --医源性皮肤损伤
                           '' as YYXPFSS_DETAIL, --医源性皮肤损伤列表
                           --院内部位
                           '' as YNBW1,
                           '' as YNBW2,
                           --院内分期
                           '' as YNFQ1,
                           '' as YNFQ2,
       
                           --院外事件类型
                           extractvalue(d.xmldata, '/FormData/X007') as YWYLXSS, --院外压力性损伤
                           extractvalue(d.xmldata, '/FormData/X017') as YWSJXPY, --院外失禁性皮炎（小便/大便/大小便）
                           extractvalue(d.xmldata, '/FormData/X031') as FYYXYWSJXPY, --（非医源性）意外皮肤损伤
                           --院外部位
                           trim(extractvalue(d.xmldata, '/FormData/X008')) as YWBW1,
                           trim(extractvalue(d.xmldata, '/FormData/X011')) as YWBW2,
                           trim(extractvalue(d.xmldata, '/FormData/X014')) as YWBW3,
                           trim(extractvalue(d.xmldata, '/FormData/A104')) as YWBW4,
                           trim(extractvalue(d.xmldata, '/FormData/A105')) as YWBW5,
                           trim(extractvalue(d.xmldata, '/FormData/A106')) as YWBW6,
                           --院外分期
                           trim(extractvalue(d.xmldata, '/FormData/X009')) as YWFQ1,
                           trim(extractvalue(d.xmldata, '/FormData/X012')) as YWFQ2,
                           trim(extractvalue(d.xmldata, '/FormData/X015')) as YWFQ3,
                           trim(extractvalue(d.xmldata, '/FormData/A107')) as YWFQ4,
                           trim(extractvalue(d.xmldata, '/FormData/A108')) as YWFQ5,
                           trim(extractvalue(d.xmldata, '/FormData/A109')) as YWFQ6
                      from icare.rptEvent a
                      left join icare.rpteventdata b
                        on a.rptid = b.rptid
                      left join icare.rpteventdata d
                        on a.rptid = d.rptid
                      left join t_bse_deptdesc c
                        on a.deptcode = c.code_vchr
                     where a.eventid = 21
                       and a.status = 1 ";
                #endregion

                svc = new SqlHelper(EnumBiz.onlineDB);
                string strSub = string.Empty;
                string strSub2 = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();

                foreach (EntityParm po in dicParm)
                {
                    string keyValue = po.value;

                    switch (po.key)
                    {
                        case "reportDate":
                            IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0] + " 00:00:00";
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            IDataParameter parm3 = svc.CreateParm();
                            parm3.Value = keyValue.Split('|')[0] + " 00:00:00";
                            lstParm.Add(parm3);
                            IDataParameter parm4 = svc.CreateParm();
                            parm4.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm4);
                            strSub += " and (a.reporttime between ? and ?)";
                            break;
                        case "deptCode":
                            strSub += " and (a.reportdeptcode = '" + keyValue + "')";
                            break;
                        case "TypeCode":
                            strSub += " and extractvalue(b.xmldata, '/FormData/" + keyValue + "') = '1'";
                            strSub2 += " and extractvalue(d.xmldata, '/FormData/" + keyValue + "') = '1'";
                            break;
                        case "PartCode":
                            bw = keyValue;
                            break;
                        case "FqCode":
                            fq = keyValue;
                            break;
                        default:
                            break;
                    }
                }

                Sql1 += strSub ;
                Sql1 += Environment.NewLine + "union all" + Environment.NewLine ;
                Sql2 += (strSub + strSub2);
                Sql = @"select * from (" + Sql1 + Sql2 + ")";
                Sql += " order by rptid";
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());

                #region 赋值
                string month = string.Empty;
                string xmlData = string.Empty;
                Dictionary<string, string> dicData = new Dictionary<string, string>();
                int i = 1;

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntitySkinEventStrument voClone = new EntitySkinEventStrument();
                       
                        voClone.KS = dr["KS"].ToString();
                        voClone.FSRQSJ = Function.Datetime(dr["FSRQSJ"]).ToString("yyyy-MM-dd HH:mm");
                        voClone.ZYHZLH = dr["ZYHZLH"].ToString();
                        voClone.CH = dr["CH"].ToString();
                        voClone.XM = dr["XM"].ToString();
                        voClone.NL = dr["NL"].ToString();

                        if (dr["XB"].ToString() == "1")
                            voClone.XB = "男";
                        else if (dr["XB"].ToString() == "2")
                            voClone.XB = "女";
                        else
                            voClone.XB = dr["XB"].ToString();
                        voClone.AQSJJG = dr["AQSJJG"].ToString();
                        voClone.AQSJJG = voClone.AQSJJG.Replace("<X032/>", "").Replace("<X032>", "").Replace("<![CDATA[", "").Replace("</X032>", "").Replace("]]>", "");
                        voClone.SBZ = dr["SBZ"].ToString();

                        #region 安全事件类型
                        //院外压力性损伤
                        if (dr["YWYLXSS"].ToString() == "1")
                            voClone.AQSJLX += "院外压力性损伤、";
                        //院外失禁性皮炎（小便/大便/大小便）
                        if (dr["YWSJXPY"].ToString() == "1")
                            voClone.AQSJLX += "院外失禁性皮炎（小便/大便/大小便）、";
                        if (dr["FYYXYWSJXPY"].ToString() == "1")
                            voClone.AQSJLX += "（非医源性）意外皮肤损伤、";

                        //院内压力性皮肤损伤
                        if (dr["YNYLXPFSS"].ToString() == "1")
                            voClone.AQSJLX += "院内压力性皮肤损伤、";
                        if (dr["YNSJXPY"].ToString() == "1")
                            voClone.AQSJLX += "院内失禁性皮炎、";
                        //医源性皮肤损伤
                        if (dr["YYXPFSS"].ToString() == "1")
                            voClone.AQSJLX += "医源性皮肤损伤、";
                        //医源性皮肤损伤
                        if (dr["YYXPFSS"].ToString() == "0" && !string.IsNullOrEmpty(dr["YYXPFSS_DETAIL"].ToString()))
                            voClone.AQSJLX += "医源性皮肤损伤、";

                        if (!string.IsNullOrEmpty(voClone.AQSJLX))
                            voClone.AQSJLX = voClone.AQSJLX.TrimEnd('、');
                        #endregion

                        #region 院内部位
                        if (!string.IsNullOrEmpty(dr["ynbw1"].ToString()))
                            voClone.BW += dr["ynbw1"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["ynbw2"].ToString()))
                            voClone.BW += dr["ynbw2"].ToString() + "、";
                        #endregion

                        #region 院内分期
                        if (!string.IsNullOrEmpty(dr["YNFQ1"].ToString()))
                            voClone.FQ += dr["YNFQ1"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["YNFQ2"].ToString()))
                            voClone.FQ += dr["YNFQ2"].ToString() + "、";
                        #endregion

                        #region 院外部位
                        if (!string.IsNullOrEmpty(dr["ywbw1"].ToString()))
                            voClone.BW += dr["ywbw1"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["ywbw2"].ToString()))
                            voClone.BW += dr["ywbw2"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["ywbw3"].ToString()))
                            voClone.BW += dr["ywbw3"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["ywbw4"].ToString()))
                            voClone.BW += dr["ywbw4"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["ywbw5"].ToString()))
                            voClone.BW += dr["ywbw5"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["ywbw6"].ToString()))
                            voClone.BW += dr["ywbw6"].ToString() + "、";
                       
                        #endregion

                        #region 院外分期
                        if (!string.IsNullOrEmpty(dr["YWFQ1"].ToString()))
                            voClone.FQ += dr["YWFQ1"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["YWFQ2"].ToString()))
                            voClone.FQ += dr["YWFQ2"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["YWFQ3"].ToString()))
                            voClone.FQ += dr["YWFQ3"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["YWFQ4"].ToString()))
                            voClone.FQ += dr["YWFQ4"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["YWFQ5"].ToString()))
                            voClone.FQ += dr["YWFQ5"].ToString() + "、";
                        if (!string.IsNullOrEmpty(dr["YWFQ6"].ToString()))
                            voClone.FQ += dr["YWFQ6"].ToString() + "、";
                        #endregion

                        if (!string.IsNullOrEmpty(voClone.BW))
                            voClone.BW = voClone.BW.TrimEnd('、');

                        if (!string.IsNullOrEmpty(voClone.FQ))
                            voClone.FQ = voClone.FQ.TrimEnd('、');

                        if (!string.IsNullOrEmpty(bw) )
                        {
                            if (!voClone.BW.Contains(bw) && !string.IsNullOrEmpty(voClone.BW))
                                continue;
                        }

                        if (!string.IsNullOrEmpty(fq) )
                        {
                            if (!voClone.FQ.Contains(fq) && !string.IsNullOrEmpty(voClone.FQ))
                                continue;
                        }

                        voClone.XH = i++;
                        data.Add(voClone);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }

        #endregion

        #endregion

        #region  上报科室列表

        public Dictionary<string,string> GetAdversDept()
        {
            string Sql = string.Empty;
            Dictionary<string,string> data = new Dictionary<string,string>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select itemcode, itemname from diccommon where classid = 24 and status = 1";
                #endregion

                DataTable dt = svc.GetDataTable(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        data.Add(dr["itemcode"].ToString(),dr["itemname"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return data;
        }
        #endregion

        #region  上报事件类型

        public Dictionary<string, string> GetAdversType()
        {
            string Sql = string.Empty;
            Dictionary<string, string> data = new Dictionary<string, string>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select itemcode, itemname from diccommon where classid = 25 and status = 1";
                #endregion

                DataTable dt = svc.GetDataTable(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        data.Add(dr["itemcode"].ToString(), dr["itemname"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return data;
        }
        #endregion

        #region  皮肤事件类型

        public Dictionary<string, string> GetAdversSkinType()
        {
            string Sql = string.Empty;
            Dictionary<string, string> data = new Dictionary<string, string>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select itemcode, itemname from diccommon where classid = 26 and status = 1";
                #endregion

                DataTable dt = svc.GetDataTable(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        data.Add(dr["itemcode"].ToString(), dr["itemname"].ToString());
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return data;
        }
        #endregion

        #endregion

        #region 职业暴露汇总统计
        /// <summary>
        /// 职业暴露汇总统计
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityOccupationexp> GetStatOccupationexp(List<EntityParm> dicParm)
        {
            List<EntityOccupationexp> data = new List<EntityOccupationexp>();
            string Sql = string.Empty;
            int n = 0;
            SqlHelper svc = null;
            try
            {
                #region Sql
                Sql = @"select a.rptid,
                               a.reporttime,
                               a.eventcode as regCode,
                               extractvalue(b.xmldata, '/FormData/PatientName') as NAME,
                               extractvalue(b.xmldata, '/FormData/PatientSex') as Sex,
                               extractvalue(b.xmldata, '/FormData/PatientAge') as NL,
                               e.deptname_vchr as deptName,
                               extractvalue(b.xmldata, '/FormData/X001') as GL1,--'5年以下',
                               extractvalue(b.xmldata, '/FormData/X002') as GL2,--'10年以下',
                               extractvalue(b.xmldata, '/FormData/X003') as GL3,--'15年以下',
                               extractvalue(b.xmldata, '/FormData/X004') as GL4,--15年及以上,
                               extractvalue(b.xmldata, '/FormData/X201') as GZLB1,--医生, 
                               extractvalue(b.xmldata, '/FormData/X202') as GZLB2,--护士, 
                               extractvalue(b.xmldata, '/FormData/X203') as GZLB3,--检验人员, 
                               extractvalue(b.xmldata, '/FormData/X204') as GZLB4,--护工, 
                               extractvalue(b.xmldata, '/FormData/X205') as GZLB5,--保洁人员, 
                               extractvalue(b.xmldata, '/FormData/X206')  as GZLB6,--废物管理员,
                               extractvalue(b.xmldata, '/FormData/X207')  as GZLB7,--实习生,
                               extractvalue(b.xmldata, '/FormData/X006')  as BLFF_JZ1,--接触
                               extractvalue(b.xmldata, '/FormData/X007')  as BLFF_JZ2,
                               extractvalue(b.xmldata, '/FormData/X008')  as BLFF_JZ3,
                               extractvalue(b.xmldata, '/FormData/X009')  as BLFF_JZ4,
                               extractvalue(b.xmldata, '/FormData/X015')  as BLFF_JZ5,
                               extractvalue(b.xmldata, '/FormData/X016')  as BLFF_JZ6,
                               extractvalue(b.xmldata, '/FormData/X017')  as BLFF_JZ7,
                               extractvalue(b.xmldata, '/FormData/X018')  as BLFF_ZCS1,--针刺伤
                               extractvalue(b.xmldata, '/FormData/X019')  as BLFF_ZCS2,
                               extractvalue(b.xmldata, '/FormData/X020')  as BLFF_ZCS3,
                               extractvalue(b.xmldata, '/FormData/X022')  as BLFF_ZCS4,
                               extractvalue(b.xmldata, '/FormData/X031')  as BLFF_QTFS1,--抓伤
                               extractvalue(b.xmldata, '/FormData/X032')  as BLFF_QTFS2,--咬伤
                               extractvalue(b.xmldata, '/FormData/X034')  as BLFF_QTFS3,--其他
                               extractvalue(b.xmldata, '/FormData/X040')  as HZCZ1,--重新盖帽
                               extractvalue(b.xmldata, '/FormData/X041')  as HZCZ2,--采血
                               extractvalue(b.xmldata, '/FormData/X042')  as HZCZ3,--穿刺
                               extractvalue(b.xmldata, '/FormData/X043')  as HZCZ4,--注射
                               extractvalue(b.xmldata, '/FormData/X044')  as HZCZ5,--处理废物
                               extractvalue(b.xmldata, '/FormData/X045')  as HZCZ6,--利器盒
                               extractvalue(b.xmldata, '/FormData/X046')  as HZCZ7,--处理针
                               extractvalue(b.xmldata, '/FormData/X047')  as HZCZ8,--处理织物
                               extractvalue(b.xmldata, '/FormData/X048')  as HZCZ9,--皮试
                               extractvalue(b.xmldata, '/FormData/X049')  as HZCZ10,--缝合
                               extractvalue(b.xmldata, '/FormData/X050')  as HZCZ11,--包扎
                               extractvalue(b.xmldata, '/FormData/X051')  as HZCZ12,--实验操作
                               extractvalue(b.xmldata, '/FormData/X052')  as HZCZ13,--排尿管
                               extractvalue(b.xmldata, '/FormData/X054')  as HZCZ14,--清洁设备
                               extractvalue(b.xmldata, '/FormData/X055')  as HZCZ15,--采集导尿管尿
                               extractvalue(b.xmldata, '/FormData/X056')  as HZCZ16,--剃毛
                               extractvalue(b.xmldata, '/FormData/X058')  as HZCZ17,
                               extractvalue(b.xmldata, '/FormData/X059')  as BLZC1,--自己造成
                               extractvalue(b.xmldata, '/FormData/X060')  as BLZC2,--医护配合时刺伤或割伤
                               extractvalue(b.xmldata, '/FormData/X061')  as BLZC3,--因患者躁动时被刺伤
                               extractvalue(b.xmldata, '/FormData/X062')  as BLZC4,--被其他工作人员不慎刺伤
                               extractvalue(b.xmldata, '/FormData/X065')  as BLZC5,
                               extractvalue(b.xmldata, '/FormData/X037')  as FSSJ,
                               extractvalue(b.xmldata, '/FormData/X038')  as FSDT,
                               extractvalue(b.xmldata, '/FormData/X066')  as BLDSD1,--戴了
                               extractvalue(b.xmldata, '/FormData/X067')  as BLDSD2,--未戴
                               extractvalue(b.xmldata, '/FormData/X083')  as XYHZ--血源患者
                          from icare.rptEvent a
                          left join icare.rpteventdata b
                            on a.rptid = b.rptid
                          left join t_bse_deptdesc c
                            on a.deptcode = c.code_vchr
                          left join t_bse_deptdesc e
                          on a.reportdeptcode = e.code_vchr
                         where a.formid = 15
                           and a.status = 1 ";
                #endregion

                svc = new SqlHelper(EnumBiz.onlineDB);
                string strSub = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();

                foreach (EntityParm po in dicParm)
                {
                    string keyValue = po.value;

                    switch (po.key)
                    {
                        case "reportDate":
                            IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0] + " 00:00:00";
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            strSub += " and (a.reporttime between ? and ?)";
                            break;
                        case "deptCode":
                            strSub += " and (a.reportdeptcode = '" + keyValue + "')";
                            break;
                        default:
                            break;
                    }
                }

                Sql += strSub;
                Sql += " order by a.reporttime";
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());

                #region 赋值
                string month = string.Empty;

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityOccupationexp vo = new EntityOccupationexp();
                        vo.regCode = dr["regCode"].ToString();
                        vo.Name = dr["name"].ToString();
                        vo.Sex = dr["sex"].ToString();
                        vo.Age = dr["NL"].ToString();
                        vo.deptName = dr["deptName"].ToString();

                        if (dr["GL1"].ToString() == "1")
                            vo.GL = "5年以下";
                        else if (dr["GL2"].ToString() == "1")
                            vo.GL = "10年以下";
                        else if (dr["GL3"].ToString() == "1")
                            vo.GL = "15年以下";
                        else if (dr["GL4"].ToString() == "1")
                            vo.GL = "15年及以上";

                        if (dr["GZLB1"].ToString() == "1")
                            vo.GZLB = "医生";
                        else if (dr["GZLB2"].ToString() == "1")
                            vo.GZLB = "护士";
                        else if (dr["GZLB3"].ToString() == "1")
                            vo.GZLB = "检验人员";
                        else if (dr["GZLB4"].ToString() == "1")
                            vo.GZLB = "护工";
                        else if (dr["GZLB5"].ToString() == "1")
                            vo.GZLB = "保洁人员";
                        else if (dr["GZLB6"].ToString() == "1")
                            vo.GZLB = "废物管理员";
                        else if (dr["GZLB7"].ToString() == "1")
                            vo.GZLB = "实习生";

                        if (dr["BLFF_JZ1"].ToString() == "1" ||
                            dr["BLFF_JZ2"].ToString() == "1" ||
                            dr["BLFF_JZ3"].ToString() == "1" ||
                            dr["BLFF_JZ4"].ToString() == "1" ||
                            dr["BLFF_JZ5"].ToString() == "1" ||
                            dr["BLFF_JZ6"].ToString() == "1" ||
                            !string.IsNullOrEmpty(dr["BLFF_JZ7"].ToString()))
                            vo.BLFF = "接触、";
                        else if (dr["BLFF_ZCS1"].ToString() == "1" ||
                            dr["BLFF_ZCS2"].ToString() == "1" ||
                            dr["BLFF_ZCS3"].ToString() == "1" ||
                            dr["BLFF_ZCS4"].ToString() == "1" )
                            vo.BLFF += "针刺伤、";
                        else if(dr["BLFF_QTFS1"].ToString() == "1")
                            vo.BLFF += "抓伤、" ;
                        else if(dr["BLFF_QTFS2"].ToString() == "1")
                            vo.BLFF += "咬伤、";
                        else if(dr["BLFF_QTFS3"].ToString() == "1")
                            vo.BLFF += "其他、";
                        if(!string.IsNullOrEmpty(vo.BLFF))
                            vo.BLFF = vo.BLFF.TrimEnd('、');

                        if (dr["HZCZ1"].ToString() == "1") 
                            vo.FSSCZ = "重新盖帽、";
                        else if (dr["HZCZ2"].ToString() == "1") 
                            vo.FSSCZ = "采血、";
                        else if (dr["HZCZ3"].ToString() == "1") 
                            vo.FSSCZ = "穿刺、";
                        else if (dr["HZCZ4"].ToString() == "1") 
                            vo.FSSCZ = "注射、";
                        else if (dr["HZCZ5"].ToString() == "1") 
                            vo.FSSCZ = "处理废物、";
                        else if (dr["HZCZ6"].ToString() == "1") 
                            vo.FSSCZ = "利器盒、";
                        else if (dr["HZCZ7"].ToString() == "1") 
                            vo.FSSCZ = "处理针、";
                        else if (dr["HZCZ8"].ToString() == "1") 
                            vo.FSSCZ = "处理织物、";
                        else if (dr["HZCZ9"].ToString() == "1") 
                            vo.FSSCZ = "皮试、";
                        else if (dr["HZCZ10"].ToString() == "1") 
                            vo.FSSCZ = "缝合、";
                        else if (dr["HZCZ11"].ToString() == "1") 
                            vo.FSSCZ = "包扎、";
                        else if (dr["HZCZ12"].ToString() == "1") 
                            vo.FSSCZ = "实验操作、";
                        else if (dr["HZCZ13"].ToString() == "1") 
                            vo.FSSCZ = "排尿管、";
                        else if (dr["HZCZ14"].ToString() == "1") 
                            vo.FSSCZ = "清洁设备、";
                        else if (dr["HZCZ14"].ToString() == "1") 
                            vo.FSSCZ = "采集导尿管尿、";
                        else if (dr["HZCZ14"].ToString() == "1") 
                            vo.FSSCZ = "剃毛、";
                        else if(!string.IsNullOrEmpty(dr["HZCZ17"].ToString()))
                            vo.FSSCZ += dr["HZCZ17"].ToString() +"、";
                        if (!string.IsNullOrEmpty(vo.FSSCZ))
                            vo.FSSCZ = vo.FSSCZ.TrimEnd('、');

                        if (dr["BLZC1"].ToString() == "1") 
                            vo.BLZC = "自己造成、";
                        else if (dr["BLZC2"].ToString() == "1")
                            vo.BLZC = "医护配合时刺伤或割伤、";
                        else if (dr["BLZC3"].ToString() == "1")
                            vo.BLZC = "因患者躁动时被刺伤、";
                        else if (dr["BLZC4"].ToString() == "1")
                            vo.BLZC = "被其他工作人员不慎刺伤、";
                        else if(!string.IsNullOrEmpty(dr["BLZC5"].ToString()))
                            vo.BLZC += dr["BLZC5"].ToString() + "、";
                        if (!string.IsNullOrEmpty(vo.BLZC))
                            vo.BLZC = vo.BLZC.TrimEnd('、');

                        if (dr["BLDSD2"].ToString() == "1")
                            vo.BLDSD = "未戴"; 
                        else if (dr["BLDSD1"].ToString() == "1")
                            vo.BLDSD = "戴了";
                        vo.XYHZ = dr["XYHZ"].ToString();      

                        vo.FSSJ = dr["FSSJ"].ToString();
                        vo.FSDT = dr["FSDT"].ToString();
                        vo.XH = ++n;
                        data.Add(vo);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
        }
        #endregion

        #region 权限
        #region 具有编辑权限可查看自己所在科室事件
        public string GetEventRoleEdit(string empNo)
        {
            string Sql1 = string.Empty;
            string Sql2 = string.Empty;
            string roleCode = string.Empty;
            List<string> data = new List<string>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                #region
                Sql1 = @"select a.OPER_CODE,  a.RANK_CODE, a.DEPT_CODE,b.rank_name,c.role_code
                        from icare.PLUS_OPERATOR a
                        left join icare.code_rank b 
                        on a.rank_code = b.rank_code
                        left join icare.def_operator_role c 
                        on a.oper_code = c.oper_code
                        where (c.role_code =  '14') ";
                #endregion


                Sql1 += " and c.oper_code = '" + empNo + "'";
                DataTable dt = svc.GetDataTable(Sql1);
                if (dt != null && dt.Rows.Count > 0)
                {
                    roleCode = "Y";
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return roleCode;
        }
        #endregion

        #region  具有这个权限可查看职业暴露所有事件 
        public string GetZYBLEventRoleLimt(string empNo)
        {
            string Sql1 = string.Empty;
            string Sql2 = string.Empty;
            string roleCode = string.Empty;
            List<string> data = new List<string>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                #region
                Sql1 = @"select a.OPER_CODE,  a.RANK_CODE, a.DEPT_CODE,b.rank_name,c.role_code
                        from icare.PLUS_OPERATOR a
                        left join icare.code_rank b 
                        on a.rank_code = b.rank_code
                        left join icare.def_operator_role c 
                        on a.oper_code = c.oper_code
                        where c.role_code =  '17' ";
                #endregion


                Sql1 += " and c.oper_code = '" + empNo + "'";
                DataTable dt = svc.GetDataTable(Sql1);
                if (dt != null && dt.Rows.Count > 0)
                {
                    roleCode = "Y";
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return roleCode;
        }
        #endregion

        #region 具有删除权限可查看所有不良事件
        public string GetEventRoleDel(string empNo)
        {
            string Sql1 = string.Empty;
            string Sql2 = string.Empty;
            string roleCode = string.Empty;
            List<string> data = new List<string>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                #region
                Sql1 = @"select a.OPER_CODE,  a.RANK_CODE, a.DEPT_CODE,b.rank_name,c.role_code
                        from icare.PLUS_OPERATOR a
                        left join icare.code_rank b 
                        on a.rank_code = b.rank_code
                        left join icare.def_operator_role c 
                        on a.oper_code = c.oper_code
                        where (c.role_code = '16') ";
                #endregion


                Sql1 += " and c.oper_code = '" + empNo + "'";
                DataTable dt = svc.GetDataTable(Sql1);
                if (dt != null && dt.Rows.Count > 0)
                {
                    roleCode = "Y";
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return roleCode;
        }
        #endregion

        #region 具有查看权限可查看所有不良事件
        public string GetEventRoleQuery(string empNo)
        {
            string Sql1 = string.Empty;
            string Sql2 = string.Empty;
            string roleCode = string.Empty;
            List<string> data = new List<string>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                #region
                Sql1 = @"select a.OPER_CODE,  a.RANK_CODE, a.DEPT_CODE,b.rank_name,c.role_code
                        from icare.PLUS_OPERATOR a
                        left join icare.code_rank b 
                        on a.rank_code = b.rank_code
                        left join icare.def_operator_role c 
                        on a.oper_code = c.oper_code
                        where (c.role_code = '18') ";
                #endregion


                Sql1 += " and c.oper_code = '" + empNo + "'";
                DataTable dt = svc.GetDataTable(Sql1);
                if (dt != null && dt.Rows.Count > 0)
                {
                    roleCode = "Y";
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return roleCode;
        }
        #endregion

        #region  科室名称获取科室代码
        public string GetDeptCode(string deptName)
        {
            string Sql = string.Empty;
            string deptCode = string.Empty;
            List<string> data = new List<string>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                #region
                Sql = @"select code_vchr from t_bse_deptdesc a where a.deptname_vchr = ? ";
                #endregion

                IDataParameter[] parm = null;
                parm = svc.CreateParm(1);
                parm[0].Value = deptName;
                DataTable dt = svc.GetDataTable(Sql, parm);

                if (dt != null && dt.Rows.Count > 0)
                {
                    deptCode = dt.Rows[0]["code_vchr"].ToString();
                }
                else
                    deptCode = "";
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return deptCode;
        }
        #endregion
        #endregion 
        #endregion

        #region 传染病报告卡

        #region 获取传染病报告列表
        /// <summary>
        /// 获取传染病报告列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityZrbbgDisplay> GetZrbbgList(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            List<EntityZrbbgDisplay> data = new List<EntityZrbbgDisplay>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select t.* from icare.rptZrbbg t
                         where t.status = 1  and reportid = ? ";
                #endregion

                #region 条件

                string strSub = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();
                // 默认参数
                IDataParameter parm = svc.CreateParm();
                parm.Value = dicParm.FirstOrDefault(t => t.key == "reportId").value;
                lstParm.Add(parm);

                foreach (EntityParm po in dicParm)
                {
                    parm = svc.CreateParm();
                    string keyValue = po.value;
                    parm.Value = keyValue;

                    switch (po.key)
                    {
                        case "reportDate":
                            IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0];
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            strSub += " and (t.reportDate between ? and ?)";
                            break;
                        case "cardNo":
                            lstParm.Add(parm);
                            strSub += " and (t.patno= ?)";
                            break;
                        case "patName":
                            parm.Value = "%" + keyValue + "%";
                            lstParm.Add(parm);
                            strSub += " and (t.patname like ?)";
                            break;
                        case "regCode":
                            lstParm.Add(parm);
                            strSub += " and (t.registercode= ?)";
                            break;
                        case "areaStr":
                            strSub += " and (t.reportDeptCode in (" + keyValue + ") ";//"or t.reportDeptCode is null";
                            break;
                        case "selfId":
                            strSub += " or t.reportopercode = '" + keyValue + "')";
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                #region 赋值

                // 组合条件
                Sql += strSub;
                Sql += " order by t.reportDate";
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());
                if (dt != null)
                {
                    EntityZrbbgDisplay vo = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vo = new EntityZrbbgDisplay();
                        vo.rptId = Function.Dec(dr["rptId"].ToString());
                        vo.patName = dr["patName"].ToString();
                        vo.parentName = dr["parentName"].ToString();
                        vo.registerCode = dr["registerCode"].ToString();
                        vo.patSex = dr["patSex"].ToString();
                        if (vo.patSex == "1")
                            vo.patSex = "男";
                        else
                            vo.patSex = "女";
                        vo.patNo = dr["patNo"].ToString();
                        vo.familyAddr = dr["familyAddr"].ToString();
                        vo.contactAddr = dr["contactAddr"].ToString();
                        if (dr["birthday"] != DBNull.Value)
                        {
                            vo.birthday = dr["birthday"].ToString();
                            vo.patAge = CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                        }
                        vo.contactTel = dr["contactTel"].ToString();
                        vo.diagnoseName = dr["diagnoseName"].ToString();
                        vo.diagnoseDate = dr["diagnoseDate"].ToString();
                        vo.infectiveDate = dr["infectiveDate"].ToString();
                        vo.reportDate = dr["reportDate"].ToString();
                        vo.skDate = dr["skDate"].ToString();
                        vo.bcDate = dr["bcDate"].ToString();
                        vo.reportOperName = dr["reportOperName"].ToString();
                        vo.idCard = dr["idCard"].ToString();
                        vo.RQFL = dr["RQFL"].ToString();
                        vo.BZ = dr["bz"].ToString();
                        vo.skDate = dr["skdate"].ToString();
                        vo.bcDate = dr["bcDate"].ToString();
                        vo.printFlg = Function.Dec(dr["printflg"]);
                        data.Add(vo);
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }

            return data;
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
            EntityRptZrbbg vo = new EntityRptZrbbg();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                vo.rptId = rptId;
                vo = EntityTools.ConvertToEntity<EntityRptZrbbg>(svc.SelectPk(vo));

                EntityRptZrbbgData dataVo = new EntityRptZrbbgData();
                dataVo.rptId = rptId;
                dataVo = EntityTools.ConvertToEntity<EntityRptZrbbgData>(svc.SelectPk(dataVo));
                vo.xmlData = dataVo.xmlData;
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return vo;
        }
        #endregion

        #region 保存传染病报告
        /// <summary>
        /// 保存传染病报告
        /// </summary>
        /// <param name="eventVo"></param>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public int SaveZrbbg(EntityRptZrbbg vo, out decimal rptId)
        {
            int affectRows = 0;
            rptId = 0;
            SqlHelper svc = null;
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                svc = new SqlHelper(EnumBiz.onlineDB);
                EntityRptZrbbgData voData = new EntityRptZrbbgData();
                if (vo.rptId <= 0)  // new
                {
                    rptId = svc.GetNextID(EntityTools.GetTableName(vo), EntityTools.GetFieldName(vo, EntityRptZrbbg.Columns.rptId));
                    vo.rptId = rptId;
                    lstParm.Add(svc.GetInsertParm(vo));

                    voData.rptId = rptId;
                    voData.xmlData = vo.xmlData;
                    lstParm.Add(svc.GetInsertParm(voData));
                }
                else                // edit
                {
                    lstParm.Add(svc.GetUpdateParm(vo, new List<string>() {EntityRptZrbbg.Columns.registerCode, 
                        EntityRptZrbbg.Columns.patName, 
                        EntityRptZrbbg.Columns.parentName, 
                        EntityRptZrbbg.Columns.patSex, 
                        EntityRptZrbbg.Columns.birthday, 
                        EntityRptZrbbg.Columns.familyAddr, 
                        EntityRptZrbbg.Columns.contactAddr,  
                        EntityRptZrbbg.Columns.contactTel, 
                        EntityRptZrbbg.Columns.diagnoseName, 
                        EntityRptZrbbg.Columns.diagnoseDate, 
                        EntityRptZrbbg.Columns.infectiveDate,  
                        EntityRptZrbbg.Columns.reportDate, 
                        EntityRptZrbbg.Columns.skDate, 
                        EntityRptZrbbg.Columns.bcDate,  
                        EntityRptZrbbg.Columns.reportOperCode,
                        EntityRptZrbbg.Columns.reportOperName,
                        EntityRptZrbbg.Columns.reportDeptCode,
                        EntityRptZrbbg.Columns.patNo,
                        EntityRptZrbbg.Columns.idCard,
                        EntityRptZrbbg.Columns.patDeptCode,
                        EntityRptZrbbg.Columns.status,
                        EntityRptZrbbg.Columns.RQFL,
                        EntityRptZrbbg.Columns.BZ },
                                                      new List<string>() { EntityRptZrbbg.Columns.rptId }));

                    voData.rptId = vo.rptId;
                    voData.xmlData = vo.xmlData;
                    lstParm.Add(svc.GetUpdateParm(voData, new List<string>() { EntityRptZrbbgData.Columns.xmlData }, new List<string>() { EntityRptZrbbgData.Columns.rptId }));

                    rptId = vo.rptId;
                }
                affectRows = svc.Commit(lstParm);
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }
            return affectRows;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal int DelZrbbg(decimal rptId)
        {
            int affectRows = 0;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                EntityRptZrbbg vo = new EntityRptZrbbg();
                vo.rptId = rptId;
                vo.status = 0;
                affectRows = svc.Commit(svc.GetUpdateParm(vo, new List<string>() { EntityRptZrbbg.Columns.status }, new List<string>() { EntityRptZrbbg.Columns.rptId }));
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }
            return affectRows;
        }
        #endregion

        #region 更新传染病报告卡数据
        /// <summary>
        /// RegisterZrbygfk
        /// </summary>
        /// <param name="rptId"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public int RegisterZrbygfk(decimal rptId, string xmlData)
        {
            int affectRows = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                Sql = @"update rptzrbbgdata set ygfkxml = ? where rptid = ?";
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = svc.CreateParm(2);
                parm[0].Value = xmlData;
                parm[1].Value = rptId;
                affectRows = svc.ExecSql(Sql, parm);
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }
            return affectRows;
        }
        #endregion

        #region 获取传染病报告卡数据
        /// <summary>
        /// GetRegisterZrbygfk
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public string GetRegisterZrbygfk(decimal rptId)
        {
            string xmlData = string.Empty;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                Sql = @"select ygfkxml from rptzrbbgdata where rptid = ?";
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = svc.CreateParm(1);
                parm[0].Value = rptId;
                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                    xmlData = dt.Rows[0]["ygfkxml"].ToString();
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return xmlData;
        }
        #endregion

        #region 更新打印标志
        /// <summary>
        /// 更新打印标志
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal int UpdateZrbbgPrintFlg(decimal rptId)
        {
            int affectRows = 0;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                EntityRptZrbbg vo = new EntityRptZrbbg();
                vo.rptId = rptId;
                vo.printFlg = 1;
                affectRows = svc.Commit(svc.GetUpdateParm(vo, new List<string>() { EntityRptZrbbg.Columns.printFlg }, new List<string>() { EntityRptZrbbg.Columns.rptId }));
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }
            return affectRows;
        }
        #endregion

        #region 获取传染病报告参数
        /// <summary>
        /// 获取传染病报告参数
        /// </summary>
        /// <param name="serNo"></param>
        /// <returns></returns>
        public List<EntityRptZrbbgParm> GetZrbbgParm()
        {
            List<EntityRptZrbbgParm> data = new List<EntityRptZrbbgParm>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                data = EntityTools.ConvertToEntityList<EntityRptZrbbgParm>(svc.Select(new EntityRptZrbbgParm()));
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return data;
        }
        #endregion

        #endregion

        #region 出院患者随访记录

        #region 查找病人
        /// <summary>
        /// 查找病人
        /// </summary>
        /// <returns></returns>
        internal List<EntityPatientInfo> GetInterviewPatient(string cardNo, string deptCode)
        {
            string Sql = string.Empty;
            deptCode = string.Empty;
            List<EntityPatientInfo> lstPat = new List<EntityPatientInfo>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                    
                Sql = @"select a.patientid_chr             as pid,
                            a.lastname_vchr             as patname,
                            a.sex_chr                   as sex,
                            a.birth_dat                 as birthday,
                            a.idcard_chr                as idcard,
                            a.homephone_vchr            as contacttel,
                            a.contactpersonaddress_vchr as contactaddr,
                            b.patientcardid_chr         as cardno,
                            r1.inpatientid_chr          as ipNo,
                            r1.inpatientcount_int       as ipTimes,
                            r1.inareadate_dat           as inDate,
                            d1.code_vchr                as deptcode,
                            d1.deptname_vchr            as deptname,
                            r2.outhospital_dat          as outDate,
                            d2.code_vchr                as deptcode2,
                            d2.deptname_vchr            as deptname2,
                            e.bed_no                    as bedNo,
                            e1.empno_chr                as doctCode,
                            e1.lastname_vchr            as doctName
                        from t_bse_patient a
                        inner join t_bse_patientcard b
                        on a.patientid_chr = b.patientid_chr
                        inner join t_opr_bih_register r1
                        on a.patientid_chr = r1.patientid_chr
                        left join t_opr_bih_leave r2
                        on r1.registerid_chr = r2.registerid_chr
                        left join t_bse_deptdesc d1
                        on r1.areaid_chr = d1.deptid_chr            --deptid_chr areaid_chr
                        left join t_bse_deptdesc d2
                        on r2.outareaid_chr = d2.deptid_chr   -- outdeptid_chr
                        left join t_bse_bed e
                        on r1.bedid_chr = e.bedid_chr
                        left join t_bse_employee e1
                        on r1.casedoctor_chr = e1.empid_chr
                        where r1.inpatientid_chr = ? 
                        {0}
                        order by r1.inpatientcount_int desc ";

                parm = svc.CreateParm(1);
                parm[0].Value = cardNo;
                Sql = string.Format(Sql, (deptCode == string.Empty ? "" : ("and (d1.code_vchr in (" + deptCode + ") and d2.code_vchr in (" + deptCode + "))")));
                    
                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    EntityPatientInfo pat = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        pat = new EntityPatientInfo();
                        pat.pid = dr["pid"].ToString();
                        pat.cardNo = dr["cardno"].ToString();
                        pat.name = dr["patname"].ToString();
                        pat.sex = (dr["sex"].ToString().Trim() == "男" ? "1" : "2");
                        if (dr["birthday"] != DBNull.Value) pat.birth = dr["birthday"].ToString();
                        pat.ID = dr["idcard"].ToString();
                        pat.contAddr = dr["contactaddr"].ToString();
                        pat.contTel = dr["contacttel"].ToString();
                        if (dr["deptcode2"] != DBNull.Value)
                        {
                            pat.deptCode = dr["deptcode2"].ToString();
                            pat.deptName = dr["deptname2"].ToString();
                        }
                        else
                        {
                            pat.deptCode = dr["deptcode"].ToString();
                            pat.deptName = dr["deptname"].ToString();
                        }
                        pat.ipNo = dr["ipNo"].ToString();
                        pat.ipTimes = Function.Int(dr["ipTimes"]);
                        pat.bedNo = dr["bedNo"].ToString();
                        if (dr["inDate"] != DBNull.Value) pat.inDate = Function.Datetime(dr["inDate"]);
                        if (dr["outDate"] != DBNull.Value) pat.outDate = Function.Datetime(dr["outDate"]);
                        pat.doctCode = dr["doctCode"].ToString();
                        pat.doctName = dr["doctName"].ToString();
                        if (string.IsNullOrEmpty(pat.ID)) pat.ID = pat.pid;
                        lstPat.Add(pat);
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return lstPat;
        }
        #endregion


        #region 获取出院病人列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        internal List<EntityOutpatientInterview> GetPatInterviewInfo(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            List<EntityOutpatientInterview> data = new List<EntityOutpatientInterview>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select a.patientid_chr             as pid,
                                   r1.registerid_chr,
                                   a.lastname_vchr             as patname,
                                   a.sex_chr                   as sex,
                                   a.birth_dat                 as birthday,
                                   a.idcard_chr                as idcard,
                                   a.homephone_vchr            as contacttel,
                                   a.contactpersonaddress_vchr as contactaddr,
                                   b.patientcardid_chr         as cardno,
                                   r1.inpatientid_chr          as ipNo,
                                   r1.inpatientcount_int       as ipTimes,
                                   r1.inareadate_dat           as inDate,
                                   d1.code_vchr                as deptcode,
                                   d1.deptname_vchr            as deptname,
                                   r2.outhospital_dat          as outDate,
                                   d2.code_vchr                as deptcode2,
                                   d2.deptname_vchr            as deptname2,
                                   e.bed_no                    as bedNo,
                                   e1.empno_chr                as doctCode,
                                   e1.lastname_vchr            as doctName,
                                   rpt.*
                              from t_bse_patient a
                             inner join t_bse_patientcard b
                                on a.patientid_chr = b.patientid_chr
                             inner join t_opr_bih_register r1
                                on a.patientid_chr = r1.patientid_chr
                              left join t_opr_bih_leave r2
                                on r1.registerid_chr = r2.registerid_chr and r2.status_int = 1
                              left join t_bse_deptdesc d1
                                on r1.areaid_chr = d1.deptid_chr            --deptid_chr areaid_chr
                              left join t_bse_deptdesc d2
                                on r2.outareaid_chr = d2.deptid_chr        -- outdeptid_chr
                              left join t_bse_bed e
                                on r1.bedid_chr = e.bedid_chr
                              left join t_bse_employee e1
                                on r1.casedoctor_chr = e1.empid_chr
                                left join rptinterview rpt
                                on r1.registerid_chr = rpt.registerid and rpt.status = 1
                             where r1.pstatus_int = 3 ";
                #endregion

                #region 条件

                string strSub = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();
                // 默认参数
                IDataParameter parm = svc.CreateParm();

                foreach (EntityParm po in dicParm)
                {
                    parm = svc.CreateParm();
                    string keyValue = po.value;
                    parm.Value = keyValue;

                    switch (po.key)
                    {
                        case "outDate":
                            IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0] + " 00:00:00";
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            strSub += " and (r2.outhospital_dat between to_date(?,'yyyy-mm-dd hh24:mi:ss') and to_date(?,'yyyy-mm-dd hh24:mi:ss'))";
                            break;
                        case "cardNo":
                            lstParm.Add(parm);
                            strSub += " and (b.patientcardid_chr = ?)";
                            break;
                        case "patName":
                            parm.Value = "%" + keyValue + "%";
                            lstParm.Add(parm);
                            strSub += " and (a.lastname_vchr like ?)";
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region 赋值

                // 组合条件
                Sql += strSub;
                Sql += " order by r1.inpatientcount_int desc ";
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());
                if (dt != null)
                {
                    EntityOutpatientInterview vo = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vo = new EntityOutpatientInterview();
                        vo.rptId = Function.Dec(dr["rptId"].ToString());
                        vo.interviewTime = dr["interviewTime"].ToString();
                        //vo.interviewCode = dr["interviewCode"].ToString();
                        //vo.interviewName = dr["lastname_vchr"].ToString();
                        vo.patName = dr["patName"].ToString();
                        vo.patNo = dr["ipNo"].ToString();
                        vo.outDeptName = dr["deptname2"].ToString();
                        vo.outHospitalTime = Function.Datetime(dr["outDate"]).ToString("yyyy-MM-dd HH:mm");
                        vo.patSex = dr["sex"].ToString();

                        if (dr["birthday"] != DBNull.Value)
                        {
                            vo.birthday = dr["birthday"].ToString();
                            vo.patAge = CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                        }
                        vo.contactTel = dr["contactTel"].ToString();
                        vo.contactAddr = dr["contactAddr"].ToString();
                        vo.inDeptName = dr["deptname"].ToString();
                        vo.inDeptTime = Function.Datetime(dr["inDate"]).ToString("yyyy-MM-dd HH:mm");
                        vo.inCount = dr["ipTimes"].ToString();
                        vo.doctName = dr["doctName"].ToString();
                        vo.registerid = dr["registerid_chr"].ToString();
                        data.Add(vo);
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return data;
        }
        #endregion

        #region 获取随访列表
        /// <summary>
        /// 获取不良事件列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        internal List<EntityOutpatientInterview> GetInterviewList(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            List<EntityOutpatientInterview> data = new List<EntityOutpatientInterview>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select t.*,b.deptname_vchr,e.lastname_vchr
                          from icare.rptinterview t
                          left join t_bse_deptdesc b
                            on t.outdeptcode = b.CODE_VCHR
                          left join t_bse_deptdesc c 
                           on t.outdeptcode = c.code_vchr
                           left join t_bse_employee e 
                           on t.interviewcode = e.empno_chr
                         where t.status = 1  ";
                #endregion

                #region 条件

                string strSub = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();
                // 默认参数
                IDataParameter parm = svc.CreateParm();
                
                foreach (EntityParm po in dicParm)
                {
                    parm = svc.CreateParm();
                    string keyValue = po.value;
                    parm.Value = keyValue;

                    switch (po.key)
                    {
                        case "interviewDate":
                            IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0] + " 00:00:00";
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            strSub += " and (t.interviewtime between ? and ?)";
                            break;
                        case "cardNo":
                            lstParm.Add(parm);
                            strSub += " and (t.patno= ?)";
                            break;
                        case "patName":
                            parm.Value = "%" + keyValue + "%";
                            lstParm.Add(parm);
                            strSub += " and (t.patname like ?)";
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                #region 赋值

                // 组合条件
                Sql += strSub;
                Sql += " order by t.interviewtime";
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());
                if (dt != null)
                {
                    EntityOutpatientInterview vo = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vo = new EntityOutpatientInterview();
                        vo.rptId = Function.Dec(dr["rptId"].ToString()) ;
                        vo.interviewTime = dr["interviewTime"].ToString();
                        vo.interviewCode = dr["interviewCode"].ToString();
                        vo.interviewName = dr["lastname_vchr"].ToString();
                        vo.patName = dr["patName"].ToString();
                        vo.patNo = dr["patNo"].ToString();
                        vo.outDeptName = dr["deptname_vchr"].ToString();
                        vo.outHospitalTime = dr["outHospitalTime"].ToString();
                        if (dr["patSex"].ToString() == "1")
                            vo.patSex = "男";
                        else if (dr["patSex"].ToString() == "2")
                            vo.patSex = "女";

                        if (dr["birthday"] != DBNull.Value)
                        {
                            vo.birthday = dr["birthday"].ToString();
                            vo.patAge = CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                        }
                        vo.contactTel = dr["contactTel"].ToString();
                        vo.contactAddr = dr["contactAddr"].ToString();

                        data.Add(vo);
                    }
                }
                #endregion
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return data;
        }
        #endregion

        #region 出院患者随访记录实例(vo)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal EntityOutpatientInterview GetInterviewVo(decimal rptId)
        {
            EntityOutpatientInterview vo = new EntityOutpatientInterview();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                vo.rptId = rptId;
                vo = EntityTools.ConvertToEntity<EntityOutpatientInterview>(svc.SelectPk(vo));
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return vo;
        }
        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="vo"></param>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal int SaveInterview(ref EntityOutpatientInterview vo)
        {
            int affectRows = 0;
            decimal rptId = 0;
            SqlHelper svc = null;
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                svc = new SqlHelper(EnumBiz.onlineDB);
                if (vo.rptId <= 0)  // new
                {
                    rptId = svc.GetNextID(EntityTools.GetTableName(vo), "rptId");
                    vo.rptId = rptId;
                    lstParm.Add(svc.GetInsertParm(vo));
                }
                else                // edit
                {
                    rptId = vo.rptId;
                    string Sql = @" update rptinterview set interviewTime = ? ,interviewCode = ?,
                                    patNo = ?, patName = ? ,patSex = ?,idCard = ?,BIRTHDAY = ?,
                                    CONTACTADDR = ? ,CONTACTTEL = ?, OUTDEPTCODE = ? ,OUTHOSPITALTIME = ?,
                                    RECORDDATE = ?,STATUS = ?, XMLDATA = ? where RPTID = ?";

                    IDataParameter [] parm = svc.CreateParm(15);
                    parm[0].Value = vo.interviewTime;
                    parm[1].Value = vo.interviewCode;
                    parm[2].Value = vo.patNo;
                    parm[3].Value = vo.patName;
                    parm[4].Value = vo.patSex;
                    parm[5].Value = vo.idCard;
                    parm[6].Value = vo.birthday;
                    parm[7].Value = vo.contactAddr;
                    parm[8].Value = vo.contactTel;
                    parm[9].Value = vo.outDeptCode;
                    parm[10].Value = vo.outHospitalTime;
                    parm[11].Value = vo.recordDate;
                    parm[12].Value = vo.status;
                    parm[13].Value = vo.xmlData;
                    parm[14].Value = vo.rptId;
                    rptId = vo.rptId;
                    lstParm.Add(svc.GetDacParm(EnumExecType.ExecSql,Sql, parm));
                }
                affectRows = svc.Commit(lstParm);
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }
            return affectRows;
        }
        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal int DelInterview(decimal rptId)
        {
            int affectRows = 0;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                EntityOutpatientInterview vo = new EntityOutpatientInterview();
                vo.rptId = rptId;
                vo.status = 0;
                affectRows = svc.Commit(svc.GetUpdateParm(vo, new List<string>() { EntityOutpatientInterview.Columns.status }, new List<string>() { EntityRptZrbbg.Columns.rptId }));
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }
            return affectRows;
        }
        #endregion

        #region 获取参数
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="serNo"></param>
        /// <returns></returns>
        internal List<EntityRptInterviewParm> GetInterviewParm()
        {
            List<EntityRptInterviewParm> data = new List<EntityRptInterviewParm>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                data = EntityTools.ConvertToEntityList<EntityRptInterviewParm>(svc.Select(new EntityRptInterviewParm()));
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return data;
        }
        #endregion

        #endregion


        #region 获取所属科室
        /// <summary>
        /// 获取所属科室
        /// </summary>
        /// <param name="empno"></param>
        /// <returns></returns>
        public string GetOwerDeptCode(string empno)
        {
            string OwerDeptCode = string.Empty;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                string sql = @"select d.deptid_chr,
                                       d.deptname_vchr,
                                       d.status_int,
                                       d.shortno_chr
                                       from  t_bse_deptemp m,t_bse_deptdesc d ,
                                       t_bse_employee e
                                       where m.deptid_chr = d.deptid_chr 
                                       and m.empid_chr = e.empid_chr 
                                       and e.empno_chr= ? ";

                IDataParameter parm = svc.CreateParm();
                parm.Value = empno;

                DataTable data = svc.GetDataTable(sql, parm);

                if (data != null && data.Rows.Count > 0)
                {
                    foreach (DataRow dr in data.Rows)
                    {
                        OwerDeptCode +=  "'" + dr["shortno_chr"].ToString().Trim() + "',";
                    }

                    if (!string.IsNullOrEmpty(OwerDeptCode))
                       OwerDeptCode = OwerDeptCode.TrimEnd(',');
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
                svc = null;
            }
            return OwerDeptCode;
        }
        #endregion

        #region Dispose
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
