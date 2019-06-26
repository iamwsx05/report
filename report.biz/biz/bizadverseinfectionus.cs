using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using weCare.Core.Dac;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Entity;
using System.Data;

namespace Report.Biz
{
    public class bizAdverseInfectionus : IDisposable
    {
        #region
        /// <summary>
        /// 获取病人信息
        /// </summary>
        /// <param name="inpatId"></param>
        /// <returns></returns>

        internal List<EntityPatientInfo> GetPatInfo(string inpatId)
        {
            List<EntityPatientInfo> lstPat = new List<EntityPatientInfo>();
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;

                Sql = @"select  *
                          from (select t.registerid_chr,
                                       a.patientid_chr             as pid,
                                       a.lastname_vchr             as patname,
                                       a.sex_chr                   as sex,
                                       a.birth_dat                 as birthday,
                                       a.idcard_chr                as idcard,
                                       a.homephone_vchr            as contacttel,
                                       a.contactpersonaddress_vchr as contactaddr,
                                       b.patientcardid_chr         as cardno,
                                       r1.mzdiagnose_vchr          as ryzd,
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
                                       t.diagnose_vchr             as diagnose
                                  from t_bse_patient a
                                 inner join t_bse_patientcard b
                                    on a.patientid_chr = b.patientid_chr
                                 inner join t_opr_bih_register r1
                                    on a.patientid_chr = r1.patientid_chr
                                  left join t_opr_bih_leave r2
                                    on r1.registerid_chr = r2.registerid_chr
                                  left join t_bse_deptdesc d1
                                    on r1.areaid_chr = d1.deptid_chr
                                  left join t_bse_deptdesc d2
                                    on r2.outdeptid_chr = d2.deptid_chr
                                  left join t_bse_bed e
                                    on r1.bedid_chr = e.bedid_chr
                                  left join t_bse_employee e1
                                    on r1.casedoctor_chr = e1.empid_chr
                                  left join T_Opr_BIH_Register t
                                    on a.inpatientid_chr = t.inpatientid_chr and r1.inpatient_dat = t.inpatient_dat
                                 where r1.inpatientid_chr = ?
                                 order by r1.inpatientcount_int desc) where rownum < 2 ";

                parm = svc.CreateParm(1);
                parm[0].Value = inpatId;
                DataTable dt = svc.GetDataTable(Sql, parm);

                #region Sql2
                Sql = @"select a.preoperativediagnosis_chr,
                         c.cutlevel,b.operate_date,a.operationname_chr
                         from t_ana_requisition a 
                         inner join ana_collection_eventcontent b
                            on a.anaid_int = b.anaid_int
                         left join anaesthesia_event f
                            on b.event_id = f.event_id
                         left join inhospitalmainrecord_operation c
                         on a.inpatientid_chr = c.inpatientid and a.inpatientdate_dat = c.inpatientdate
                         where a.registerid_chr = ?
                         and b.status = 0
                         and f.event_id = '0054'
                         and rownum < 2
                         order by a.operationdate_dat desc ";

                #endregion

                if (dt != null && dt.Rows.Count > 0)
                {
                    EntityPatientInfo pat = null;
                    DataRow dr = dt.Rows[0];

                    pat = new EntityPatientInfo();
                    pat.nhId = dr["registerid_chr"].ToString();
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
                    pat.email = dr["ryzd"].ToString();
                   
                    if (string.IsNullOrEmpty(pat.ID)) pat.ID = pat.pid;

                    if (pat.nhId != string.Empty)
                    {
                        parm = svc.CreateParm(1);
                        parm[0].Value = pat.nhId;
                        DataTable dt2 = svc.GetDataTable(Sql, parm);
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            DataRow dr2 = dt2.Rows[0];
                            pat.upFlag = dr2["operationname_chr"].ToString();
                            pat.zip = dr2["operate_date"].ToString();

                            if (dr2["cutlevel"].ToString().Contains("I") || dr2["cutlevel"].ToString().Contains("一")
                                || dr2["cutlevel"].ToString().Contains("1"))
                                pat.tremType = "I";
                            else if (dr2["cutlevel"].ToString().Contains("II") || dr2["cutlevel"].ToString().Contains("2")
                                || dr2["cutlevel"].ToString().Contains("‖") || dr2["cutlevel"].ToString().Contains("11"))
                                pat.tremType = "II";
                            else if (dr2["cutlevel"].ToString().Contains("III") || dr2["cutlevel"].ToString().Contains("3"))
                                pat.tremType = "II";
                            else if (dr2["cutlevel"].ToString().Contains("III") || dr2["cutlevel"].ToString().Contains("3"))
                                pat.tremType = "III";
                            else if (dr2["cutlevel"].ToString().Contains("0"))
                                pat.tremType = "0";
                        }
                        else
                        {
                            pat.upFlag = "";
                            pat.zip = "";
                            pat.tremType = "";
                        }
                    }

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

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="vo"></param>
        /// <param name="lstPathogenyVo"></param>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal int SaveInfectionus(EntityInfectionus vo, List<EntityPathogeny> lstPathogenyVo)
        {
            int affectRows = 0;
            decimal  rptId = 0;
            decimal serNo = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                svc = new SqlHelper(EnumBiz.onlineDB);

                if (vo.rptId <= 0)  // new
                {
                    rptId = Function.Dec(GetNextID(svc, "rptinfectionus").ToString());
                     
                    vo.rptId = rptId;
                    lstParm.Add(svc.GetInsertParm(vo));

                    foreach (EntityPathogeny item in lstPathogenyVo)
                    {
                        serNo = Function.Dec(GetNextID(svc, "rptpathogeny").ToString());
                        item.serNo = serNo;
                        item.rptId = rptId;
                    }

                    if (lstPathogenyVo.Count > 0)
                    {
                        lstParm.Add(svc.GetInsertParm(lstPathogenyVo.ToArray()));
                    }

                    affectRows = svc.Commit(lstParm);
                }
                else                // edit
                {
                    #region Sql
                    svc = new SqlHelper(EnumBiz.onlineDB);
                    Sql = @"update rptinfectionus set 
                            reportTime = ?, inDiagnosis = ?, incisionType = ?,  infectionSite01 = ?, 
                            infectionSite02 = ?, infectionDate01 = ?, infectionDate02 = ?, infectionreason = ?,  
                            operationName = ?, operationDate = ?,recordDate = ?, status = ? where rptid = ?";

                    IDataParameter[] parm = svc.CreateParm(13);
                    parm[0].Value = vo.reportTime;
                    parm[1].Value = vo.inDiagnosis;
                    parm[2].Value = vo.incisionType;
                    parm[3].Value = vo.infectionSite01;
                    parm[4].Value = vo.infectionSite02;
                    parm[5].Value = vo.infectionDate01;
                    parm[6].Value = vo.infectionDate02;
                    parm[7].Value = vo.infectionReason;
                    parm[8].Value = vo.operationName;
                    parm[9].Value = vo.operationDate;
                    parm[10].Value = vo.recordDate;
                    parm[11].Value = vo.status;
                    parm[12].Value = vo.rptId;
                    #endregion
                    //lstParm.Add(svc.ExecSql(Sql, parm));
                    lstParm.Add(svc.GetDacParm(EnumExecType.ExecSql, Sql, parm));
                    Sql = @"delete from  rptPathogeny where rptId = ?";
                    parm = svc.CreateParm(1);
                    parm[0].Value = vo.rptId;
                    //lstParm.Add(svc.ExecSql(Sql, parm));
                    lstParm.Add(svc.GetDacParm(EnumExecType.ExecSql, Sql, parm));

                    foreach (EntityPathogeny item in lstPathogenyVo)
                    {
                        serNo = Function.Dec(GetNextID(svc, "rptpathogeny").ToString());
                        item.serNo = serNo;
                        item.rptId = vo.rptId;
                    }

                    if (lstPathogenyVo.Count > 0)
                    {
                        lstParm.Add(svc.GetInsertParm(lstPathogenyVo.ToArray()));
                    }

                    affectRows = svc.Commit(lstParm);
                }
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

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="vo"></param>
        /// <returns></returns>
        internal int ComfirmRpt(EntityInfectionus vo)
        {
            int affectRows = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"update rptinfectionus set status = ? where rptid = ?";

                IDataParameter[] parm = svc.CreateParm(2);
                parm[0].Value = vo.status;
                parm[1].Value = vo.rptId;

                #endregion
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

        #region 获取感染报告列表
        /// <summary>
        /// 获取感染报告列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        internal List<EntityInfectionus> GetInfectionusList(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            List<EntityInfectionus> data = new List<EntityInfectionus>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select t.rptid,
                               t.reporttime,
                               t.inpatno,
                               t.patname,
                               t.patsex,
                               t.patage,
                               t.bedno,
                               t.datein,
                               t.recorddate,
                               t.deptcode,
                               t.indiagnosis,
                               t.incisiontype,
                               t.infectionsite01,
                               t.infectionsite02,
                               t.infectiondate01,
                               t.infectiondate02,
                               t.infectionreason,
                               t.reportername,
                               t.reporterid,
                               t.status,
                               b.deptname_vchr as deptname,
                               e.lastname_vchr as doctname,
                               t.operationdate,
                               t.operationname
                          from rptinfectionus t
                          left join t_bse_deptdesc b
                            on t.deptcode = b.CODE_VCHR
                          left join t_bse_employee e
                            on e.empno_chr = t.doctid
                         where t.status in (0,1,2)";
                #endregion

                #region 条件

                string strSub = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();
                // 默认参数
                IDataParameter parm = null;

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
                            strSub += " and (t.reporttime between ? and ?)";
                            break;
                        case "deptCode":
                            if (keyValue.IndexOf("','") > 0)
                            {
                                strSub += " and (t.deptcode in (" + keyValue + "))";
                            }
                            else
                            {
                                parm.Value = parm.Value.ToString().Replace("'", "");
                                lstParm.Add(parm);
                                strSub += " and (t.deptcode = ?)";
                            }
                            break;
                        case "cardNo":
                            lstParm.Add(parm);
                            strSub += " and (t.inpatno = ?)";
                            break;
                        case "patName":
                            Sql = Sql1;
                            parm.Value = "%" + keyValue + "%";
                            lstParm.Add(parm);
                            strSub += " and (t.patname like ?)";
                            break;
                        case "areaStr":
                            strSub += " and (t.deptcode in (" + keyValue + ")";
                            break;
                        case "selfId":
                            strSub += " or t.reporterid = '" + keyValue + "')";
                            break;
                         case "limitId":
                            strSub += " and (t.reporterid in (" + keyValue + "))";
                            break;

                        default:
                            break;
                    }
                }

                #endregion

                #region 赋值

                // 组合条件
                Sql += strSub;
                Sql += " order by t.reporttime";
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());
                if (dt != null)
                {
                    EntityInfectionus vo = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vo = new EntityInfectionus();
                        //vo.rptIdStr = dr["rptId"].ToString();
                        vo.rptId = Function.Int(dr["rptid"].ToString());
                        vo.reportTime = dr["reporttime"].ToString();
                        vo.reporterName = dr["reportername"].ToString();
                        vo.inpatNo = dr["inpatno"].ToString();
                        vo.patName = dr["patname"].ToString();
                        //vo.patSex = dr["patsex"].ToString() == "1" ? "男" : "女";
                        if (dr["patSex"].ToString() == "1")
                            vo.patSex = "男";
                        else if (dr["patSex"].ToString() == "2")
                            vo.patSex = "女";
                        else
                            vo.patSex = "\\";
                        vo.patAge = dr["patage"].ToString();
                        vo.deptName = dr["deptname"].ToString();
                        vo.dateIn = Function.Datetime(dr["datein"].ToString());
                        vo.incisionType = Function.Int(dr["incisiontype"].ToString());
                        vo.infectionDate01 = dr["infectiondate01"].ToString();
                        vo.infectionDate02 = dr["infectiondate02"].ToString();
                        vo.infectionSite01 = dr["infectionsite01"].ToString();
                        vo.infectionSite02 = dr["infectionsite02"].ToString();
                        vo.infectionReason = dr["infectionreason"].ToString();
                        vo.operationName = dr["operationname"].ToString();
                        vo.operationDate = dr["operationdate"].ToString();
                        vo.doctName = dr["doctname"].ToString();
                        vo.reporterName = dr["reportername"].ToString();
                        vo.reporterId = dr["reporterid"].ToString();
                        vo.status = Function.Int(dr["status"].ToString());
                        vo.inDiagnosis = dr["indiagnosis"].ToString();
                        vo.operationName = dr["operationname"].ToString();
                        vo.operationDate = dr["operationdate"].ToString();
                        if (vo.status == 1)
                            vo.isPass = "审核通过";
                        else if (vo.status == 2)
                            vo.isPass = "审核未通过";
                        else
                            vo.isPass = "";
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

        #region 打印 数据源
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        public List<EntityInfectionus> GetXrDataSource(decimal rptId)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            List<EntityInfectionus> data = new List<EntityInfectionus>();
            SqlHelper svc = null;

            try
            {
                #region Sql
                Sql = @"select t.rptid,
                               t.reporttime,
                               t.inpatno,
                               t.patname,
                               t.patsex,
                               t.patage,
                               t.bedno,
                               t.datein,
                               t.recorddate,
                               t.deptcode,
                               t.indiagnosis,
                               t.incisiontype,
                               t.infectionsite01,
                               t.infectionsite02,
                               t.infectiondate01,
                               t.infectiondate02,
                               t.infectionreason,
                               t.reportername,
                               t.reporterid,
                               t.status,
                               b.deptname_vchr as deptname,
                               e.lastname_vchr as doctname,
                               t.operationdate,
                               t.operationname
                          from rptinfectionus t
                          left join t_bse_deptdesc b
                            on t.deptcode = b.CODE_VCHR
                          left join t_bse_employee e
                            on e.empno_chr = t.doctid
                         where t.rptid = ? ";


                Sql1 = @"select sampleName, checkDate, pathogenyName, drugName  
                          from rptpathogeny where rptid = ? ";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                IDataParameter[] parm1 = null;
                parm = svc.CreateParm(1);
                parm[0].Value = rptId;
                parm1 = svc.CreateParm(1);
                parm1[0].Value = rptId;
                DataTable dt = svc.GetDataTable(Sql, parm);
                DataTable dt1 = svc.GetDataTable(Sql1, parm1);
                EntityInfectionus vo = null;
                #endregion

                #region 赋值

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt1.Rows)
                        {
                            vo = new EntityInfectionus();

                            vo.rptId = Function.Int(dt.Rows[0]["rptid"].ToString());
                            vo.reportTime = dt.Rows[0]["reporttime"].ToString();
                            vo.reporterName = dt.Rows[0]["reportername"].ToString();
                            vo.inpatNo = dt.Rows[0]["inpatno"].ToString();
                            vo.patName = dt.Rows[0]["patname"].ToString();
                            vo.patSex = dt.Rows[0]["patsex"].ToString() == "1" ? "男" : "女";
                            vo.patAge = dt.Rows[0]["patage"].ToString();
                            vo.deptName = dt.Rows[0]["deptname"].ToString();
                            vo.dateIn = Function.Datetime(dt.Rows[0]["datein"].ToString());
                            vo.incisionType = Function.Int(dt.Rows[0]["incisiontype"].ToString());
                            if (vo.incisionType == 1)
                                vo.incisionType01 = 1;
                            if (vo.incisionType == 2)
                                vo.incisionType02 = 1;
                            if (vo.incisionType == 3)
                                vo.incisionType03 = 1;
                            if (vo.incisionType == 4)
                                vo.incisionType04 = 1;

                            vo.infectionDate01 = dt.Rows[0]["infectiondate01"].ToString();
                            vo.infectionDate02 = dt.Rows[0]["infectiondate02"].ToString();
                            vo.infectionSite01 = dt.Rows[0]["infectionsite01"].ToString();
                            vo.infectionSite02 = dt.Rows[0]["infectionsite02"].ToString();
                            vo.infectionReason = dt.Rows[0]["infectionreason"].ToString();
                            vo.operationName = dt.Rows[0]["operationname"].ToString();
                            vo.operationDate = dt.Rows[0]["operationdate"].ToString();
                            vo.doctName = dt.Rows[0]["doctname"].ToString();
                            vo.reporterName = dt.Rows[0]["reportername"].ToString();
                            vo.reporterId = dt.Rows[0]["reporterid"].ToString();
                            vo.status = Function.Int(dt.Rows[0]["status"].ToString());
                            vo.inDiagnosis = dt.Rows[0]["indiagnosis"].ToString();
                            vo.operationName = dt.Rows[0]["operationname"].ToString();
                            vo.operationDate = dt.Rows[0]["operationdate"].ToString();

                            vo.sampleName = dr["sampleName"].ToString();
                            vo.checkDate = dr["checkDate"].ToString();
                            vo.pathogenyName = dr["pathogenyName"].ToString();
                            vo.drugName = dr["drugName"].ToString();

                            data.Add(vo);
                        }
                    }
                    else
                    {
                        vo = new EntityInfectionus();

                        vo.rptId = Function.Int(dt.Rows[0]["rptid"].ToString());
                        vo.reportTime = dt.Rows[0]["reporttime"].ToString();
                        vo.reporterName = dt.Rows[0]["reportername"].ToString();
                        vo.inpatNo = dt.Rows[0]["inpatno"].ToString();
                        vo.patName = dt.Rows[0]["patname"].ToString();
                        //vo.patSex = dt.Rows[0]["patsex"].ToString() == "1" ? "男" : "女";
                        if (dt.Rows[0]["patsex"].ToString() == "1")
                            vo.patSex = "男";
                        else if (dt.Rows[0]["patsex"].ToString() == "2")
                            vo.patSex = "女";
                        else
                            vo.patSex = "\\";
                        vo.patAge = dt.Rows[0]["patage"].ToString();
                        vo.deptName = dt.Rows[0]["deptname"].ToString();
                        vo.dateIn = Function.Datetime(dt.Rows[0]["datein"].ToString());
                        vo.incisionType = Function.Int(dt.Rows[0]["incisiontype"].ToString());
                        if (vo.incisionType == 1)
                            vo.incisionType01 = 1;
                        if (vo.incisionType == 2)
                            vo.incisionType02 = 1;
                        if (vo.incisionType == 3)
                            vo.incisionType03 = 1;
                        if (vo.incisionType == 4)
                            vo.incisionType04 = 1;

                        vo.infectionDate01 = dt.Rows[0]["infectiondate01"].ToString();
                        vo.infectionDate02 = dt.Rows[0]["infectiondate02"].ToString();
                        vo.infectionSite01 = dt.Rows[0]["infectionsite01"].ToString();
                        vo.infectionSite02 = dt.Rows[0]["infectionsite02"].ToString();
                        vo.infectionReason = dt.Rows[0]["infectionreason"].ToString();
                        vo.operationName = dt.Rows[0]["operationname"].ToString();
                        vo.operationDate = dt.Rows[0]["operationdate"].ToString();
                        vo.doctName = dt.Rows[0]["doctname"].ToString();
                        vo.reporterName = dt.Rows[0]["reportername"].ToString();
                        vo.reporterId = dt.Rows[0]["reporterid"].ToString();
                        vo.status = Function.Int(dt.Rows[0]["status"].ToString());
                        vo.inDiagnosis = dt.Rows[0]["indiagnosis"].ToString();
                        vo.operationName = dt.Rows[0]["operationname"].ToString();
                        vo.operationDate = dt.Rows[0]["operationdate"].ToString();

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

        #region
        internal List<EntityPathogeny> GetPathogeny(decimal rptId)
        {
            string Sql = string.Empty;
            List<EntityPathogeny> data = new List<EntityPathogeny>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select sampleName, checkDate, pathogenyName, drugName  
                          from rptpathogeny where rptid = ? ";
                IDataParameter[] parm = null;
                int n = -1;
                parm = svc.CreateParm(1);
                parm[++n].Value = rptId;
                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        data.Add(new EntityPathogeny() { sampleName = dr["sampleName"].ToString(),
                                                         checkDate = dr["checkDate"].ToString(),
                                                         pathogenyName = dr["pathogenyName"].ToString(),
                                                         drugName = dr["drugName"].ToString()
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

        #region

        public List<string> GetInfectionSiteSource()
        {
            string Sql = string.Empty;
            List<string> data = new List<string>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select itemname from diccommon where classid = 20 and status = 1";
                #endregion

               DataTable dt = svc.GetDataTable(Sql);
               if (dt != null && dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       data.Add(dr["itemname"].ToString());
                   }
               }
            }
            catch (Exception e )
            {
                ExceptionLog.OutPutException(e);
            }

            return data;
        }

        #endregion

        #region
        public List<string> GetCausationSource()
        {
            string Sql = string.Empty;
            List<string> data = new List<string>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select itemname from diccommon where classid = 21 and status = 1";
                #endregion

               DataTable dt = svc.GetDataTable(Sql);
               if (dt != null && dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       data.Add(dr["itemname"].ToString());
                   }
               }
            }
            catch (Exception e )
            {
                ExceptionLog.OutPutException(e);
            }

            return data;
        }
        #endregion

        #region
        public List<string> GetSampleSource()
        {
            string Sql = string.Empty;
            List<string> data = new List<string>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select itemname from diccommon where classid = 23 and status = 1";
                #endregion

               DataTable dt = svc.GetDataTable(Sql);
               if (dt != null && dt.Rows.Count > 0)
               {
                   foreach (DataRow dr in dt.Rows)
                   {
                       data.Add(dr["itemname"].ToString());
                   }
               }
            }
            catch (Exception e )
            {
                ExceptionLog.OutPutException(e);
            }

            return data;
        }
        #endregion

        #region
        public List<string> GetDrugSource()
        {
            string Sql = string.Empty;
            List<string> data = new List<string>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select itemname from diccommon where classid = 23 and status = 1";
                #endregion

                DataTable dt = svc.GetDataTable(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        data.Add(dr["itemname"].ToString());
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

        #region
        public string GetAreaRankLimt(string empNo)
        {
            string Sql1 = string.Empty;
            string Sql2 = string.Empty;
            string DeptcodeStr = string.Empty;
            List<string> data = new List<string>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                #region
                Sql1 = @"select distinct
                        e.empid_chr,
                        t.code_vchr,
                               t.deptname_vchr,
                               e.lastname_vchr,
                               e.technicalrank_chr,
                               e.status_int,
                               e.deptcode_chr,
                               e.technicallevel_chr,
                               e.empno_chr from  t_bse_deptemp m,
                               t_bse_employee e ,
                               t_bse_deptdesc t
                               where m.empid_chr = e.empid_chr 
                               and t.deptid_chr = m.deptid_chr
                               and e.status_int = 1";
                #endregion
                Sql1 += " and e.empno_chr = '" + empNo + "'";
                DataTable dt = svc.GetDataTable(Sql1);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["technicalrank_chr"].ToString().Trim() == "主任医师" || 
                            dr["technicalrank_chr"].ToString().Trim() == "副主任医师")
                            DeptcodeStr += "'" + dr["code_vchr"].ToString().Trim() + "',";
                    }
                }

                if (DeptcodeStr != string.Empty) DeptcodeStr = DeptcodeStr.TrimEnd(',');
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
            }

            return DeptcodeStr;
        }
        #endregion

        #region 具有感染病例查看权限可查看所有报告
        public string GetInfectReportRoleQuery(string empNo)
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
                        where (c.role_code = '33') ";
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

        #region 获取住院科室
        public List<EntityDeptList> getDeptList()
        {
            string Sql = string.Empty;
            List<EntityDeptList> data = new List<EntityDeptList>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select deptid_chr, deptname_vchr, category_int,
                               inpatientoroutpatient_int, operatorid_chr, address_vchr, pycode_chr,
                               attributeid, parentid, createdate_dat, status_int, deactivate_dat,
                               wbcode_chr, code_vchr, extendid_vchr, shortno_chr, stdbed_count_int,
                               putmed_int
                          from t_bse_deptdesc 
                         where status_int = 1 
                           and (inpatientoroutpatient_int = 1)
                      order by code_vchr";
                #endregion

                DataTable dt = svc.GetDataTable(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    EntityDeptList vo = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vo = new EntityDeptList();
                        vo.deptCode = dr["code_vchr"].ToString();
                        vo.deptName = dr["deptname_vchr"].ToString();
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

        #region 获取下一个ID
        /// <summary>
        /// 获取下一个ID
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns>获取下一个ID</returns>
        public int GetNextID(SqlHelper svc, string tabName)
        {
            int intMinID = 0;
            string Sql = string.Empty;
            tabName = tabName.ToLower();
            try
            {
                if (this.CheckSequence(svc, tabName) >= 0)
                {
                    Sql = @"update sysSequenceid  set curid = curid + 1 where tabname = ?";
                    IDataParameter[] parm = svc.CreateParm(1);
                    parm[0].Value = tabName;
                    if (svc.ExecSql(Sql, parm) > 0)
                    {
                        Sql = @"select curid from sysSequenceid  where tabname = ?";
                        parm = svc.CreateParm(1);
                        parm[0].Value = tabName;

                        DataTable dt = svc.GetDataTable(Sql, parm);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            intMinID = (dt.Rows[0]["curid"] == System.DBNull.Value) ? 1 : Function.Int(dt.Rows[0]["curid"]);
                        }
                        else
                        {
                            intMinID = 1;
                        }
                    }
                    else
                    {
                        intMinID = -1;
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                intMinID = 1;
            }
            return intMinID;
        }
        #endregion

        #region 检查
        /// <summary>
        /// 检查
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        private int CheckSequence(SqlHelper svc, string tabName)
        {
            string Sql = @"select 1 from sysSequenceid  t where t.tabname = ?";
            IDataParameter[] parm = svc.CreateParm(1);
            parm[0].Value = tabName;
            DataTable dt = svc.GetDataTable(Sql, parm);
            if (dt == null || dt.Rows.Count == 0)
            {
                Sql = @"insert into sysSequenceid  (tabname, curid) values (?, ?)";
                parm = svc.CreateParm(2);
                parm[0].Value = tabName;
                parm[1].Value = 0;
                parm[1].DbType = DbType.Int32;
                return svc.ExecSql(Sql, parm);
            }
            return 1;
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
