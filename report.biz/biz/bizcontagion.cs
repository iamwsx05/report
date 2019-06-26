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
    /// 传染病上报
    /// </summary>
    public class bizContagion : IDisposable
    {
        #region 查找病人
        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="cardNo">1 门诊卡号； 2 住院号</param>
        /// <param name="flag">1 门诊； 2 住院</param>
        /// <returns></returns>
        internal List<EntityPatientInfo> GetPatient(string cardNo, int flag)
        {
            string Sql = string.Empty;
            string Sql2 = string.Empty;

            List<EntityPatientInfo> lstPat = new List<EntityPatientInfo>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.interfaceDB);
                IDataParameter[] parm = null;
                if (flag == 1)
                {
                    Sql = @"select a.PID       as pid,
                                   a.NAME      as patname,
                                   a.SEX       as sex, -- 性别
                                   a.BIRTH     as birthday, -- 出生日期
                                   a.ID        as idcard, -- 身份证号
                                   a.CONT_TEL  as contacttel, -- 联系电话
                                   a.CONT_ADDR as contactaddr, -- 联系地址
                                   a.CARD_NO   as cardno, -- 诊疗卡号
                                   null        as ipNo, -- 住院号
                                   null        as ipTimes, -- 住院次数
                                   null        as inDate, -- 入院时间
                                   b.DEPT_CODE as deptcode, -- 住院科室编码
                                   c.DEPT_NAME as deptname, -- 住院科室名称
                                   null        as outDate, -- 出院时间
                                   null        as deptcode2, -- 出院科室编码
                                   null        as deptname2, -- 出院科室名称
                                   null        as bedNo, -- 床位号
                                   b.DR_CODE   as doctCode, -- 主治医生编码
                                   d.OPER_NAME as doctName -- 主治医生姓名
                              from PATIENTINFO a, CL_REGISTER b, CODE_DEPARTMENT c, CODE_OPERATOR d
                             where a.PID = b.PID
                               and b.DEPT_CODE = c.DEPT_CODE
                               and b.DR_CODE = d.OPER_CODE
                               and a.CARD_NO = ?
                            ";

                    TimeSpan ts = new TimeSpan(30, 0, 0, 0);
                    parm = svc.CreateParm(1);
                    //parm[0].Value = DateTime.Now.Subtract(ts);
                    parm[0].Value = cardNo;
                }
                else if (flag == 2)
                {
                    Sql = @"select a.PID       as pid,
                                   a.NAME      as patname,
                                   a.SEX       as sex, -- 性别
                                   a.BIRTH     as birthday, -- 出生日期
                                   a.ID        as idcard, -- 身份证号
                                   a.CONT_TEL  as contacttel, -- 联系电话
                                   a.CONT_ADDR as contactaddr, -- 联系地址
                                   a.CARD_NO   as cardno, -- 诊疗卡号
                                   b.IP_NO     as ipNo, -- 住院号
                                   b.IP_CNT    as ipTimes, -- 住院次数
                                   b.IP_DATE   as inDate, -- 入院时间
                                   b.IP_DEPT   as deptcode, -- 住院科室编码
                                   c.DEPT_NAME as deptname, -- 住院科室名称
                                   b.OP_DATE   as outDate, -- 出院时间
                                   b.OP_DEPT   as deptcode2, -- 出院科室编码
                                   d.DEPT_NAME as deptname2, -- 出院科室名称
                                   e.BED_CODE  as bedNo, -- 床位号
                                   e.MAN_DR    as doctCode, -- 主治医生编码
                                   f.OPER_NAME as doctName -- 主治医生姓名
                              from PATIENTINFO a
                             inner join IP_REGISTER b
                                on a.PID = b.PID
                              left join CODE_DEPARTMENT c
                                on b.IP_DEPT = c.DEPT_CODE
                              left join CODE_DEPARTMENT d
                                on b.OP_DEPT = d.DEPT_CODE
                              left join IP_PATIENT e
                                on b.REG_NO = e.REG_NO
                               and e.CURR_FLAG = 'T'
                              left join CODE_OPERATOR f
                                on e.MAN_DR = f.OPER_CODE
                             where b.IP_NO = ?
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
                        pat.sex = (dr["sex"].ToString() == "男" ? "1" : "2");
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

        #region 查找病人检验信息
        /// <summary>
        /// 查找病人
        /// </summary>
        /// <param name="cardNo">1 门诊卡号； 2 住院号</param>
        /// <param name="flag">1 门诊； 2 住院</param>
        /// <returns></returns>
        internal List<EntityAidsCheck> GetPatLisInfo(string cardNo, int flag,decimal formId)
        {
            string Sql = string.Empty;
            string Sql2 = string.Empty;
            string reqNo = string.Empty;
            string yz = string.Empty;
            int count = 0;
            List<EntityAidsCheck> lstVo = new List<EntityAidsCheck>();
            SqlHelper svc = null;
            SqlHelper svcOn = null;
            SqlHelper lisSvc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.interfaceDB);
                svcOn = new SqlHelper(EnumBiz.onlineDB);
                lisSvc = new SqlHelper(EnumBiz.lisDB);
                IDataParameter[] parm = null;
                IDataParameter[] parm2 = null;
                IDataParameter[] parm3 = null;

                if (flag == 1)
                {
                    Sql = @"select a.card_no, a.reques_no ,a.hiv1,a.hiv2,a.hiv3,a.yzs, a.zx_time from FRE_CKECK a 
                            where a.card_no = ? order by a.zx_time desc";
                    parm = svc.CreateParm(1);
                    parm[0].Value = cardNo;
                }
                else if (flag == 2)
                {
                    Sql = @"select a.card_no,a.yzs,a.hiv1,a.hiv2,a.hiv3, a.reques_no , a.zx_time 
                            from FRE_CKECK a 
                            left join IP_REGISTER b
                            on a.pid = b.PID
                            where b.IP_NO = ?
                            order by a.zx_time desc";

                    parm = svc.CreateParm(1);
                    parm[0].Value = cardNo;
                }

                DataTable dt = svc.GetDataTable(Sql, parm);

                if (dt != null && dt.Rows.Count > 0)
                {
                    reqNo = dt.Rows[0]["reques_no"].ToString();
                    yz = dt.Rows[0]["yzs"].ToString();

                    foreach(DataRow dr in dt.Rows)
                    {
                        if(dr["hiv1"]!= DBNull.Value || dr["hiv2"]!= DBNull.Value ||dr["hiv3"]!= DBNull.Value)
                        {
                            count++;
                        }
                    }

                    Sql2 = @"select reques_no from rptContagion where reques_no = ? and status = 1 ";
                    parm2 = svcOn.CreateParm(1);
                    parm2[0].Value = reqNo;
                    DataTable dt2 = svcOn.GetDataTable(Sql2, parm2);

                    if(dt2 != null && dt2.Rows.Count > 0)
                        return null;

                    Sql = @"SELECT  samp_name = '标本类型：' + DMB_BB.SAMP_NAME,
                                     NOTE = (CASE WHEN DMB_BBZT.NAME IS NULL THEN '' ELSE  '标本状态:' + DMB_BBZT.NAME END)+ ' ' + ISNULL(SJB_BGD.NOTE,''),
                                     SJB_BGDMXB.ITEM_CODE ,
                                              item_name = '  ' + DMB_XM.ITEM_NAME,
                                     DMB_XM.SEQ_NO1,	
                                     DMB_XM.TYPE,	
                                      isnull(DMB_XM.unit,'') as unit ,
                                              isnull(SJB_BGDMXB.UPBOUND ,'') as UPBOUND ,
                                              isnull(SJB_BGDMXB.DOWNBOUND ,'') as DOWNBOUND ,
                                              SJB_BGDMXB.NORMAL ,
                                              SJB_BGDMXB.RESULT     
                                            FROM SJB_BGDMXB,
                                                 SJB_BGD,
                                                 DMB_XM ,
                                     DMB_BB,
                                     DMB_BBZT
                                            WHERE (SJB_BGD.REP_NO = SJB_BGDMXB.REP_NO) AND
                                                  (DMB_XM.ROOM_CODE = SJB_BGD.ROOM_CODE) AND
                                                  (DMB_XM.ITEM_CODE = SJB_BGDMXB.ITEM_CODE) AND
                                                  ( SJB_BGDMXB.REP_NO = ? )   AND
                                      ( ltrim(SJB_BGDMXB.result) <> '') AND
                                      ( DMB_XM.P_FLAG <> 'F' ) AND
                                      ( SJB_BGD.SAMP_CODE = DMB_BB.SAMP_CODE) AND
                                      ( SJB_BGD.SAMP_STATUS *= DMB_BBZT.STATUS)";
                    parm3 = lisSvc.CreateParm(1);
                    parm3[0].Value = reqNo;
                    dt = svcOn.GetDataTable(Sql, parm3);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            #region //"3-II.艾滋病病毒感染孕产妇妊娠及所生婴儿登记卡"
                            if (formId == 11)
                            {
                                EntityAidsCheck vo = new EntityAidsCheck();
                                vo.REQNO = reqNo;//申请单号
                                vo.YZ = yz;      //孕周
                                vo.count = count;

                                //白细胞计数
                                if (dr["item_code"].ToString() == "1221" ||
                                    dr["item_code"].ToString() == "1055" ||
                                    dr["item_code"].ToString() == "1351")
                                    vo.BXBJS = dr["result"].ToString();
                                //总淋巴细胞计数
                                if (dr["item_code"].ToString() == "1068")
                                    vo.ZLBXBJS = dr["result"].ToString();
                                //血小板计数
                                if (dr["item_code"].ToString() == "1369")
                                    vo.XXBJS = dr["result"].ToString();
                                //血红蛋白
                                if (dr["item_code"].ToString() == "1363")
                                    vo.XHDB = dr["result"].ToString();
                                //血糖
                                if (dr["item_code"].ToString() == "2713")
                                    vo.XT = dr["result"].ToString();
                                //谷丙转氨酶
                                if (dr["item_code"].ToString() == "3045")
                                    vo.GBZAM = dr["result"].ToString();
                                //谷草转氨酶
                                if (dr["item_code"].ToString() == "2002")
                                    vo.GCZAM = dr["result"].ToString();
                                //总胆红素
                                if (dr["item_code"].ToString() == "2008")
                                    vo.ZDHS = dr["result"].ToString();
                                //肌酐
                                if (dr["item_code"].ToString() == "2021")
                                    vo.XJG = dr["result"].ToString();
                                //尿素氮
                                if (dr["item_code"].ToString() == "2020")
                                    vo.XNST = dr["result"].ToString();

                                //梅毒螺旋体抗原血清学实验（TPPA/ELISA）
                                if (dr["item_code"].ToString() == "3012")
                                    vo.MD_TPPA_ELISA = dr["result"].ToString();
                                //非梅毒螺旋体抗原血清学实验（RPR、TRUST）
                                if (dr["item_code"].ToString() == "2339")
                                    vo.MD_RPR_TRUST = dr["result"].ToString();
                                //梅毒滴度
                                vo.iDD = -1;
                                if (dr["item_code"].ToString() == "3206")
                                {
                                    string result = dr["result"].ToString().Trim();
                                    if (result.Contains(":"))
                                        vo.iDD = Function.Int(result.Split(':')[1]);
                                }

                                //乙肝表面抗原（HBsAg）
                                if (dr["item_code"].ToString() == "2401" || dr["item_code"].ToString() == "3001" ||
                                    dr["item_code"].ToString() == "2721")
                                    vo.HBsAg = dr["result"].ToString();
                                //乙肝e抗原(HBeAg)
                                if (dr["item_code"].ToString() == "2723" || dr["item_code"].ToString() == "3003")
                                    vo.HBeAg = dr["result"].ToString();

                                // 丙肝抗体检测(HCV)-IgG
                                if (dr["item_code"].ToString() == "3008")
                                    vo.HCV_IGG = dr["result"].ToString();

                                lstVo.Add(vo);
                            }
                            else if (formId == 15) //4-I梅毒感染孕产妇登记卡
                            {
                                EntityAidsCheck vo = new EntityAidsCheck();
                                vo.REQNO = reqNo;//申请单号
                                vo.YZ = yz;      //孕周
                                #region 4-I
                                //        4——I
                                //快速血浆反应素环状片试验（RPR）X133 
                                //阴性X134 
                                //阳性X135
                                //检测时间X137
                                vo.I_RPR = "";
                                vo.I_RPRTIME = "";
                                //甲苯胺红不加热血清试验（TRUST）X138
                                //阴性X139
                                //阳性X140
                                //检测时间X142
                                if(dr["item_code"].ToString() == "2399")
                                {
                                    vo.I_TRUST = dr["result"].ToString();
                                }
                                vo.I_TRUSTTIME = "";
                                //梅毒螺旋体颗粒凝集试验（TPPA）X145
                                //阴性X146
                                //阳性X147
                                //检测时间X149
                                if (dr["item_code"].ToString() == "3012")
                                {
                                    vo.I_TPPA = dr["result"].ToString();
                                }
                                vo.I_TPPATIME = "";
                                //酶联免疫吸附试验（ELISA）X150
                                //阴性X151
                                //阳性X152
                                //检测时间X154
                                vo.I_ELISA = "";
                                vo.I_ELISATIME = "";
                                //免疫层析法-快速体测（RT）X155
                                //阴性X156
                                //阳性X157
                                //检测时间X159
                                vo.I_RT = "";
                                vo.I_RTTIME = "";
                                //梅毒螺旋体IgM抗体检测：
                                //未检测X166
                                //检测阳性X167
                                //检测阴性X168
                                //检测时间X169
                                vo.I_IGM = "";
                                vo.I_IGMTIME = "";
                                //暗视野显微镜梅毒螺旋体检测：
                                //未检测X170
                                //检测X171
                                //（检测到梅毒螺旋体：
                                //否X172
                                //是X173
                                //检测时间X174
                                vo.I_MD = "";
                                vo.I_MDTIME = "";
                                #endregion
                            }
                            #endregion
                        }
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
            return lstVo;
        }
        #endregion

        #region 查询

        #region 获取传染病列表
        /// <summary>
        /// 获取传染病列表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        internal List<EntityContagionDisplay> GetContagionList(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            string xmlData = string.Empty;
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            List<EntityContagionDisplay> data = new List<EntityContagionDisplay>();
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select t.rptId,
                               t.reportId,
                               t.reportTime,
                               t.reportOperCode,
                               t.reportOperName,
                               t.registerCode,
                               t.patno,
                               t.patname,
                               t.patsex,
                               t.birthday,
                               t.contacttel,
                               t.deptcode,
                               b.DEPT_NAME as deptName,
                               c.xmlData
                          from rptContagion t
                          left join code_department b
                            on t.deptcode = b.DEPT_CODE
                          left join rptContagionData c 
                            on t.rptId = c.rptId
                         where t.status = 1
                           and t.reportId = ?
                           ";

                Sql1 = @"select t.rptId,
                               t.reportId,
                               t.reportTime,
                               t.reportOperCode,
                               t.reportOperName,
                               t.registerCode,
                               t.patno,
                               t.patname,
                               t.patsex,
                               t.birthday,
                               t.contacttel,
                               t.deptcode,
                               b.DEPT_NAME as deptName,
                               c.xmlData
                          from rptContagion t
                         inner join patientinfo a
                            on t.patno = a.CARD_NO
                          left join code_department b
                            on t.deptcode = b.DEPT_CODE
                         left join rptContagionData c 
                            on t.rptId = c.rptId
                         where t.status = 1
                           and t.reportId = ?
                           ";
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
                                strSub += " and (t.deptCode in (" + keyValue + "))";
                            }
                            else
                            {
                                parm.Value = parm.Value.ToString().Replace("'", "");
                                lstParm.Add(parm);
                                strSub += " and (t.deptCode = ?)";
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
                            strSub += " and (a.name like ?)";
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                #region 赋值

                // 组合条件
                Sql += strSub;
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());
                if (dt != null)
                {
                    EntityContagionDisplay vo = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vo = new EntityContagionDisplay();
                        vo.rptId = dr["rptId"].ToString();
                        vo.reportTime = dr["reportTime"].ToString();
                        vo.reportOperCode = dr["reportOperCode"].ToString();
                        vo.reportOperName = dr["reportOperName"].ToString();
                        vo.registerCode = dr["registerCode"].ToString();
                        vo.patNo = dr["patNo"].ToString();
                        vo.patName = dr["patName"].ToString();
                        vo.patSex = dr["patSex"].ToString() == "1" ? "男" : "女";
                        if (dr["birthday"] != DBNull.Value)
                        {
                            vo.patBirthDay = dr["birthday"].ToString();
                            vo.patAge = CalcAge.GetAge(Function.Datetime(dr["birthday"]));
                        }
                        vo.contactTel = dr["contactTel"].ToString();
                        vo.deptName = dr["deptName"].ToString();

                        xmlData = dr["xmlData"].ToString();
                        if (!string.IsNullOrEmpty(xmlData))
                        {
                            dicData = Function.ReadXmlNodes(xmlData, "FormData");
                            if (dicData.ContainsKey("XSHR") && dicData["XSHR"] != "")
                            {
                                vo.SH = "已审核";
                                vo.SHR = dicData["XSHR"];
                            }
                            if (dicData.ContainsKey("XSHD") && dicData["XSHD"] != "")
                                vo.SHSJ = dicData["XSHD"];
                            if (dicData.ContainsKey("XBKS") && dicData["XBKS"] != "")
                                vo.reportDept = dicData["XBKS"];
                        }
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

        #region 获取传染病实例(vo)
        /// <summary>
        /// 获取传染病实例(vo)
        /// </summary>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal EntityRptContagion GetContagion(decimal rptId)
        {
            EntityRptContagion vo = new EntityRptContagion();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                vo.rptId = rptId;
                vo = EntityTools.ConvertToEntity<EntityRptContagion>(svc.SelectPk(vo));

                EntityRptContagionData dataVo = new EntityRptContagionData();
                dataVo.rptId = rptId;
                dataVo = EntityTools.ConvertToEntity<EntityRptContagionData>(svc.SelectPk(dataVo));
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

        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="vo"></param>
        /// <param name="rptId"></param>
        /// <returns></returns>
        internal int SaveContagion(EntityRptContagion vo, string reqNo, out decimal rptId)
        {
            int affectRows = 0;
            rptId = 0;
            string Sql = string.Empty;
            IDataParameter[] parm = null;
            SqlHelper svc = null;
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                svc = new SqlHelper(EnumBiz.onlineDB);
                EntityRptContagionData voData = new EntityRptContagionData();
                if (vo.rptId <= 0)  // new
                {
                    rptId = svc.GetNextID(EntityTools.GetTableName(vo), EntityTools.GetFieldName(vo, EntityRptContagion.Columns.rptId));
                    vo.rptId = rptId;
                    lstParm.Add(svc.GetInsertParm(vo));

                    voData.rptId = rptId;
                    voData.xmlData = vo.xmlData;

                    lstParm.Add(svc.GetInsertParm(voData));

                    if (!string.IsNullOrEmpty(reqNo))
                    {
                        Sql = @"update  rptContagion set reques_no = ? where rptId = ?";
                        parm = svc.CreateParm(2);
                        parm[0].Value = reqNo;
                        parm[1].Value = rptId;
                        lstParm.Add(svc.GetDacParm(EnumExecType.ExecSql, Sql, parm));
                    }
                }
                else                // edit
                {
                    lstParm.Add(svc.GetUpdateParm(vo, new List<string>() {EntityRptContagion.Columns.reportTime, EntityRptContagion.Columns.reportOperCode, EntityRptContagion.Columns.reportOperName, EntityRptContagion.Columns.reportId, 
                                                                          EntityRptContagion.Columns.registerCode, EntityRptContagion.Columns.patType, EntityRptContagion.Columns.patNo, EntityRptContagion.Columns.patName,  
                                                                          EntityRptContagion.Columns.patSex, EntityRptContagion.Columns.birthday, EntityRptContagion.Columns.idCard, EntityRptContagion.Columns.contactAddr, 
                                                                          EntityRptContagion.Columns.contactTel, EntityRptContagion.Columns.deptCode, EntityRptContagion.Columns.formId, EntityRptContagion.Columns.operCode, 
                                                                          EntityRptContagion.Columns.recordDate, EntityRptContagion.Columns.status},
                                                      new List<string>() { EntityRptContagion.Columns.rptId }));

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
        internal int DelContagion(decimal rptId)
        {
            int affectRows = 0;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                EntityRptContagion vo = new EntityRptContagion();
                vo.rptId = rptId;
                vo.status = 0;
                affectRows = svc.Commit(svc.GetUpdateParm(vo, new List<string>() { EntityRptContagion.Columns.status }, new List<string>() { EntityRptContagion.Columns.rptId }));
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

        #region
        public string GetContagionRole(string empNo)
        {
            string Sql1 = string.Empty;
            string roleCode = string.Empty;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                #region
                Sql1 = @"select  a.OPER_CODE,a.ROLE_CODE ,b.ROLE_NAME from def_operator_role a
                        left join code_role b
                        on a.ROLE_CODE = b.ROLE_CODE 
                        where (b.role_code = '30') ";
                #endregion


                Sql1 += " and a.oper_code = '" + empNo + "'";
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
