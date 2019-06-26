using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using weCare.Core.Dac;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Entity;
using System.Xml;

namespace Report.Biz
{
    /// <summary>
    /// 手麻报表.biz
    /// </summary>
    public class bizAnaReport : IDisposable
    {
        #region 获取麻醉科医师
        /// <summary>
        /// 获取麻醉科医师
        /// </summary>
        /// <returns></returns>
        public List<EntityCodeOperator> GetAnaOperator()
        {
            string Sql = string.Empty;
            List<EntityCodeOperator> data = null;
            SqlHelper svc = null;
            try
            {
                data = new List<EntityCodeOperator>();
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select c.deptname_vchr, b.empno_chr, b.lastname_vchr, b.technicalrank_chr
                          from t_bse_deptemp a
                         inner join t_bse_employee b
                            on a.empid_chr = b.empid_chr
                         inner join t_bse_deptdesc c
                            on a.deptid_chr = c.deptid_chr
                         where a.deptid_chr = '0000245'
                           and b.status_int = 1 
                           and b.technicalrank_chr in ('主任医师', '副主任医师', '主治医师', '医师')
                         order by b.technicalrank_chr";
                DataTable dt = svc.GetDataTable(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    EntityCodeOperator vo = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vo = new EntityCodeOperator();
                        vo.operCode = dr["empno_chr"].ToString();
                        vo.operName = dr["lastname_vchr"].ToString();
                        vo.rankName = dr["technicalrank_chr"].ToString();
                        if (data.Any(t => t.operName == vo.operName)) continue;
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

        #region GetAnaRegister1
        /// <summary>
        /// GetAnaRegister1
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityAnaRegister1> GetAnaRegister1(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            List<EntityAnaRegister1> data = new List<EntityAnaRegister1>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                DataTable dt1 = null;
                DataTable dt2 = null;

                #region Sql1
                Sql = @"select distinct
                               a.anaid_int,
                               b.operate_date             AS RSSSRQSJ, --入手术室日期时间
                               h.officename                AS KS, -- 科室
                               a.bedno_vchr                AS CW, --床位
                               a.inpatientid_chr           AS ZYH, --住院号
                               c.ananumber_all             AS MZBH, --麻醉编号
                               a.patientname_vchr          AS XM, --姓名
                               a.sex_vchr                  AS XB, --性别
                               a.age_vchr                  AS NL, --年龄
                               a.preoperativediagnosis_chr AS SQZD, --术前诊断
                               a.operationname_chr         AS SSMC, --手术名称
                               c.asalevel_all              AS MZFJ, --麻醉分级
                               a.asalevel_chr              AS SSFJ, --手术分级
                               null                        AS SFMZQTL, --是否麻醉前讨论
                               c.anamode_all               AS MZFS, --麻醉方式
                               c.poseasepain               AS ZTFS, --镇痛方式
                               d.cpr_chr                   AS SFXFFS, --是否心肺复苏
                               null                        AS SFJRFSS, --是否进入复苏室
                               d.outpacusteward_chr        AS LSSF, --离室Steward ≥4分
                               nulL                        AS SFJICU, --是否入ICU
                               c.anaeffect_all             AS MZXG, --麻醉效果
                               null                        AS SFMZFYQXGSJ, --是否麻醉非预期相关事件
                               null                        AS MZYS, --麻醉用时
                               null                        AS SSYS, --手术用时
                               null                        AS ZDYS, --主刀医师
                               null                        AS YZYS, --一助医师
                               null                        AS QXHS, --器械护士
                               null                        AS XHHS, --巡回护士
                               null                        AS MZYS2, --麻醉医生
                               a.statXml1 
                          from t_ana_requisition a
                         inner join ana_collection_eventcontent b
                            on a.anaid_int = b.anaid_int
                          left join anaesthesia_event f
                            on b.event_id = f.event_id
                          left join ana_collection c
                            on a.anaid_int = c.anaesthesiaid_int
                          left join t_opr_ana_madicalrecord d
                            on a.INPATIENTID_CHR = d.INPATIENTID_VCHR
                         inner join ana_record_report h
                            on a.anaid_int = h.anaesthesiaid_int
                         where a.status_int >= 1
                           and b.status = 0
                           and f.event_id = '0054'
                           and b.operate_date between ? and ?
                         order by b.operate_date                  
                         ";

                parm = svc.CreateParm(2);
                parm[0].Value = Function.Datetime(beginDate + " 00:00:00");
                parm[1].Value = Function.Datetime(endDate + " 23:59:59");
                dt1 = svc.GetDataTable(Sql, parm);
                #endregion

                #region Sql2

                Sql = @" select a.anaid_int,
                                c.event_id,
                                c.operate_date,
                                d.event_desc,
                                s.tag_chr,
                                s.employeename_chr
                           from t_ana_requisition a
                           left join ana_collection_eventcontent c
                             on a.anaid_int = c.anaid_int
                           left join anaesthesia_event f
                             on c.event_id = f.event_id
                           left join anaesthesia_event d
                             on c.event_id = d.event_id
                           left join t_ana_sign s
                             on a.signsequence_int = s.sequenceid_int  
                          where a.status_int >= 1
                            and c.event_id in ('0010', '0030', '0035', '0043', '0049','0045')
                            and c.operate_date between ? and ?
                        ";

                parm = svc.CreateParm(2);
                parm[0].Value = Function.Datetime(beginDate + " 00:00:00");
                parm[1].Value = Function.Datetime(endDate + " 23:59:59");
                dt2 = svc.GetDataTable(Sql, parm);

                #endregion

                #region 赋值
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    int n = 0;
                    decimal anaId = 0;
                    string MZYS2 = "";
                    string xmlData = string.Empty;
                    Dictionary<string, string> dicData = new Dictionary<string, string>();
                    DataRow[] drr = null;
                    EntityAnaRegister1 vo = null;
                    foreach (DataRow dr in dt1.Rows)
                    {
                        #region vo
                        vo = new EntityAnaRegister1();
                        dicData = new Dictionary<string, string>();

                        /// AnaId
                        vo.AnaId = Function.Dec(dr["anaid_int"].ToString());
                        /// 序号
                        vo.XH = ++n;
                        /// 入手术室日期时间
                        vo.RSSSRQSJ = Function.Datetime(dr["RSSSRQSJ"].ToString()).ToString("yyyy-MM-dd HH:mm");
                        /// 科室
                        vo.KS = dr["KS"].ToString();
                        /// 床位
                        vo.CW = dr["CW"].ToString();
                        /// 住院号
                        vo.ZYH = dr["ZYH"].ToString();
                        /// 麻醉编号
                        vo.MZBH = dr["MZBH"].ToString();
                        /// 姓名
                        vo.XM = dr["XM"].ToString();
                        /// 性别
                        vo.XB = dr["XB"].ToString();
                        /// 年龄
                        vo.NL = dr["NL"].ToString();
                        /// 术前诊断
                        vo.SQZD = dr["SQZD"].ToString();
                        /// 手术名称
                        vo.SSMC = dr["SSMC"].ToString();
                        /// 麻醉分级
                        vo.MZFJ = dr["MZFJ"].ToString();
                        /// 手术分级
                        vo.SSFJ = dr["SSFJ"].ToString();
                        /// 是否麻醉前讨论
                        vo.SFMZQTL = dr["SFMZQTL"].ToString();
                        /// 麻醉方式
                        vo.MZFS = dr["MZFS"].ToString();
                        /// 镇痛方式
                        vo.ZTFS = dr["ZTFS"].ToString();
                        if (vo.ZTFS.Trim() == "无")
                            vo.ZTFS = "";
                        /// 是否心肺复苏
                        vo.SFXFFS = dr["SFXFFS"].ToString();
                        /// 是否进入复苏室
                        vo.SFJRFSS = dr["SFJRFSS"].ToString();
                        /// 离室Steward>=4分
                        vo.LSSF = dr["LSSF"].ToString();
                        /// 是否入ICU
                        vo.SFJICU = dr["SFJICU"].ToString();
                        /// 麻醉效果
                        vo.MZXG = dr["MZXG"].ToString();
                        if (vo.MZXG.Trim() == "优")
                            vo.MZXG = "I";
                        else if (vo.MZXG.Trim() == "良")
                            vo.MZXG = "II";
                        else if (vo.MZXG.Trim() == "中")
                            vo.MZXG = "III";
                        else if (vo.MZXG.Trim() == "差")
                            vo.MZXG = "IV";
                        /// 是否麻醉非预期相关事件
                        vo.SFMZFYQXGSJ = dr["SFMZFYQXGSJ"].ToString();
                        /// 麻醉用时
                        vo.MZYS = dr["MZYS"].ToString();
                        /// 手术用时
                        vo.SSYS = dr["SSYS"].ToString();

                        #endregion

                        #region 计算
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            anaId = Function.Dec(dr["anaid_int"].ToString());
                            drr = dt2.Select("anaid_int = " + anaId);
                            if (drr != null && drr.Length > 0)
                            {
                                foreach (DataRow dr2 in drr)
                                {
                                    if (dr2["event_id"] != DBNull.Value)
                                    {
                                        if (dr2["event_id"].ToString() == "0043" || dr2["event_id"].ToString() == "0045")
                                            vo.SFJRFSS = "是";
                                        else if (dr2["event_id"].ToString() == "0049")
                                            vo.SFJICU = "是";
                                        else if (dr2["event_id"].ToString() == "0010")
                                        {
                                            vo.MZJSSJ = Function.Datetimenull(dr2["operate_date"]);
                                            vo.SSJSSJ = vo.MZJSSJ;
                                        }
                                        else if (dr2["event_id"].ToString() == "0030")
                                            vo.MZKSSJ = Function.Datetimenull(dr2["operate_date"]);
                                        else if (dr2["event_id"].ToString() == "0035")
                                            vo.SSKSSJ = Function.Datetimenull(dr2["operate_date"]);
                                    }
                                    if (dr2["tag_chr"] != DBNull.Value)
                                    {
                                        if (dr2["tag_chr"].ToString().Trim() == "主刀医师")
                                            vo.ZDYS = dr2["employeename_chr"].ToString();
                                        else if (dr2["tag_chr"].ToString().Trim() == "一助")
                                            vo.YZYS = dr2["employeename_chr"].ToString();
                                        else if (dr2["tag_chr"].ToString().Trim() == "洗手护士")//器械护士
                                            vo.QXHS = dr2["employeename_chr"].ToString();
                                        else if (dr2["tag_chr"].ToString().Trim() == "巡回护士")
                                            vo.XHHS = dr2["employeename_chr"].ToString();
                                        else if (dr2["tag_chr"].ToString().Trim() == "麻醉医师" && MZYS2 == "") //麻醉医师
                                        {
                                            MZYS2 = dr2["employeename_chr"].ToString();
                                        }
                                        else if (dr2["tag_chr"].ToString().Trim() == "麻醉医师" && MZYS2 != "" )
                                        {
                                            if(MZYS2 != dr2["employeename_chr"].ToString())
                                                vo.MZYS2 = MZYS2+"," + dr2["employeename_chr"].ToString();
                                            else
                                                vo.MZYS2 = MZYS2;
                                            MZYS2 = "";
                                        }
                                    }
                                }
                            }
                            if (vo.MZJSSJ != null && vo.MZKSSJ != null)
                            {
                                TimeSpan ts = vo.MZJSSJ.Value - vo.MZKSSJ.Value;
                                vo.MZYS = ts.Hours + "时" + ts.Minutes + "分";    // Function.Int((vo.MZJSSJ.Value - vo.MZKSSJ.Value).TotalMinutes).ToString() + "分钟";
                            }
                            if (vo.SSJSSJ != null && vo.SSKSSJ != null)
                            {
                                TimeSpan ts = vo.SSJSSJ.Value - vo.SSKSSJ.Value;
                                vo.SSYS = ts.Hours + "时" + ts.Minutes + "分";     // Function.Int((vo.SSJSSJ.Value - vo.SSKSSJ.Value).TotalMinutes).ToString() + "分钟";
                            }
                        }
                        #endregion
                        xmlData = dr["statXml1"].ToString();
                        if (!string.IsNullOrEmpty(xmlData))
                        {
                            dicData = Function.ReadXmlNodes(xmlData, "XmlData");
                            if (dicData["F001"] == "1") vo.SFXFFS = "是";
                            if (dicData["F002"] == "1") vo.SFMZQTL = "是";
                            vo.LSSF = dicData["F031"];

                            #region 麻醉非预期事件
                            if (dicData.ContainsKey("F101") && dicData["F101"] == "1")
                                vo.SFMZFYQXGSJ += "口纯损伤" + ";";
                            if(dicData.ContainsKey("F102") && dicData["F102"] == "1")
                                vo.SFMZFYQXGSJ += "鼻腔出血" + ";";
                            if (dicData.ContainsKey("F103") && dicData["F103"] == "1")
                                vo.SFMZFYQXGSJ += "喉痛" + ";";
                            if (dicData.ContainsKey("F104") && dicData["F104"] == "1")
                                vo.SFMZFYQXGSJ += "声音嘶哑" + ";";
                            if (dicData.ContainsKey("F105") && dicData["F105"] == "1")
                                vo.SFMZFYQXGSJ += "颈部皮下气肿" + ";";
                            if (dicData.ContainsKey("F106") && dicData["F106"] == "1")
                                vo.SFMZFYQXGSJ += "牙齿松动或脱落" + ";";
                            if (dicData.ContainsKey("F107") && dicData["F107"] == "1")
                                vo.SFMZFYQXGSJ += "杓状软骨脱位" + ";";
                            if (dicData.ContainsKey("F108") && dicData["F108"] == "1")
                                vo.SFMZFYQXGSJ += "高血压" + ";";
                            if (dicData.ContainsKey("F109") && dicData["F109"] == "1")
                                vo.SFMZFYQXGSJ += "高颅压" + ";";
                            if (dicData.ContainsKey("F1010") && dicData["F1010"] == "1")
                                vo.SFMZFYQXGSJ += "缺氧和二氧化碳蓄积" + ";";
                            if (dicData.ContainsKey("F1011") && dicData["F1011"] == "1")
                                vo.SFMZFYQXGSJ += "气道阻塞" + ";";
                            if (dicData.ContainsKey("F1012") && dicData["F1012"] == "1")
                                vo.SFMZFYQXGSJ += "喉头水肿" + ";";
                            if (dicData.ContainsKey("F1013") && dicData["F1013"] == "1")
                                vo.SFMZFYQXGSJ += "插管困难" + ";";
                            if (dicData.ContainsKey("F1014") && dicData["F1014"] == "1")
                                vo.SFMZFYQXGSJ += "呕吐" + ";";
                            if (dicData.ContainsKey("F1015") && dicData["F1015"] == "1")
                                vo.SFMZFYQXGSJ += "返流误吸" + ";";
                            if (dicData.ContainsKey("F1016") && dicData["F1016"] == "1")
                                vo.SFMZFYQXGSJ += "喉痉挛" + ";";
                            if (dicData.ContainsKey("F1017") && dicData["F1017"] == "1")
                                vo.SFMZFYQXGSJ += "咳嗽和呛咳" + ";";
                            if (dicData.ContainsKey("F1018") && dicData["F1018"] == "1")
                                vo.SFMZFYQXGSJ += "呃逆" + ";";
                            if (dicData.ContainsKey("F1019") && dicData["F1019"] == "1")
                                vo.SFMZFYQXGSJ += "体温升高或降低" + ";";
                            if (dicData.ContainsKey("F1020") && dicData["F1020"] == "1")
                                vo.SFMZFYQXGSJ += "恶性高热" + ";";
                            if (dicData.ContainsKey("F1021") && dicData["F1021"] == "1")
                                vo.SFMZFYQXGSJ += "张力性气胸" + ";";
                            if (dicData.ContainsKey("F1022") && dicData["F1022"] == "1")
                                vo.SFMZFYQXGSJ += "支气管痉挛" + ";";
                            if (dicData.ContainsKey("F1023") && dicData["F1023"] == "1")
                                vo.SFMZFYQXGSJ += "支气管痉挛" + ";";
                            if (dicData.ContainsKey("F1024") && dicData["F1024"] == "1")
                                vo.SFMZFYQXGSJ += "药物变态反应" + ";";
                            if (dicData.ContainsKey("F1025") && dicData["F1025"] == "1")
                                vo.SFMZFYQXGSJ += "急性心肌梗塞" + ";";
                            if (dicData.ContainsKey("F1026") && dicData["F1026"] == "1")
                                vo.SFMZFYQXGSJ += "术中心律失常";
                            if (dicData.ContainsKey("F1027") && dicData["F1027"] == "1")
                                vo.SFMZFYQXGSJ += "心力衰竭" + ";";
                            if (dicData.ContainsKey("F1028") && dicData["F1028"] == "1")
                                vo.SFMZFYQXGSJ += "心跳停止" + ";";
                            if (dicData.ContainsKey("F1029") && dicData["F1029"] == "1")
                                vo.SFMZFYQXGSJ += "脑血管意外" + ";";
                            if (dicData.ContainsKey("F1030") && dicData["F1030"] == "1")
                                vo.SFMZFYQXGSJ += "呼吸恢复延迟" + ";";
                            if (dicData.ContainsKey("F1030") && dicData["F1030"] == "1")
                                vo.SFMZFYQXGSJ += "呼吸困难" + ";";
                            if (dicData.ContainsKey("F1031") && dicData["F1031"] == "1")
                                vo.SFMZFYQXGSJ += "部感染" + ";";
                            if (dicData.ContainsKey("F1032") && dicData["F1032"] == "1")
                                vo.SFMZFYQXGSJ += "肺不张肺" + ";";
                            if (dicData.ContainsKey("F1033") && dicData["F1033"] == "1")
                                vo.SFMZFYQXGSJ += "张力性气胸" + ";";
                            if (dicData.ContainsKey("F1034") && dicData["F1034"] == "1")
                                vo.SFMZFYQXGSJ += "肺不张" + ";";
                            if (dicData.ContainsKey("F1035") && dicData["F1035"] == "1")
                                vo.SFMZFYQXGSJ += "肺栓塞" + ";";
                            if (dicData.ContainsKey("F1036") && dicData["F1036"] == "1")
                                vo.SFMZFYQXGSJ += "呼吸衰竭" + ";";
                                  
                            if (dicData.ContainsKey("F201") && dicData["F201"] == "1")
                                vo.SFMZFYQXGSJ += "穿破硬脊膜" + ";";
                            if (dicData.ContainsKey("F202") && dicData["F202"] == "1")
                                vo.SFMZFYQXGSJ += "穿刺针或导管误入血管" + ";";
                            if (dicData.ContainsKey("F203") && dicData["F203"] == "1")
                                vo.SFMZFYQXGSJ += "导管折断" + ";";
                            if (dicData.ContainsKey("F204") && dicData["F204"] == "1")
                                vo.SFMZFYQXGSJ += "局麻药中毒性反应" + ";";
                            if (dicData.ContainsKey("F205") && dicData["F205"] == "1")
                                vo.SFMZFYQXGSJ += "严重低血压" + ";";
                            if (dicData.ContainsKey("F206") && dicData["F206"] == "1")
                                vo.SFMZFYQXGSJ += "异常广泛神经阻滞" + ";";
                            if (dicData.ContainsKey("F207") && dicData["F207"] == "1")
                                vo.SFMZFYQXGSJ += "神经根/脊髓损伤";
                            if (dicData.ContainsKey("F208") && dicData["F208"] == "1")
                                vo.SFMZFYQXGSJ += "硬膜外血肿" + ";";
                            if (dicData.ContainsKey("F209") && dicData["F209"] == "1")
                                vo.SFMZFYQXGSJ += "截瘫" + ";";
                            if (dicData.ContainsKey("F2010") && dicData["F2010"] == "1")
                                vo.SFMZFYQXGSJ += "硬膜外脓肿" + ";";
                            if (dicData.ContainsKey("F2011") && dicData["F2011"] == "1")
                                vo.SFMZFYQXGSJ += "呼吸麻痹" + ";";
                            if (dicData.ContainsKey("F2012") && dicData["F2012"] == "1")
                                vo.SFMZFYQXGSJ += "粘连性蛛网膜炎";
                            if (dicData.ContainsKey("F2013") && dicData["F2013"] == "1")
                                vo.SFMZFYQXGSJ += "脊髓前动脉综合症" + ";";
                            if (dicData.ContainsKey("F2014") && dicData["F2014"] == "1")
                                vo.SFMZFYQXGSJ += "全脊麻硬" + ";";
                            if (dicData.ContainsKey("F2015") && dicData["F2015"] == "1")
                                vo.SFMZFYQXGSJ += "腰背痛" + ";";
                            if (dicData.ContainsKey("F2016") && dicData["F2016"] == "1")
                                vo.SFMZFYQXGSJ += "空气栓塞" + ";";
                            if (dicData.ContainsKey("F2017") && dicData["F2017"] == "1")
                                vo.SFMZFYQXGSJ += "感染" + ";";
                            if (dicData.ContainsKey("F2018") && dicData["F2018"] == "1")
                                vo.SFMZFYQXGSJ += "头痛" + ";";
                             
                            if (dicData.ContainsKey("F301") && dicData["F301"] == "1")
                                vo.SFMZFYQXGSJ += "恶心呕吐" + ";";
                            if (dicData.ContainsKey("F302") && dicData["F302"] == "1")
                                vo.SFMZFYQXGSJ += "脊麻后头痛" + ";";
                            if (dicData.ContainsKey("F303") && dicData["F303"] == "1")
                                vo.SFMZFYQXGSJ += "尿潴留" + ";";
                            if (dicData.ContainsKey("F304") && dicData["F304"] == "1")
                                vo.SFMZFYQXGSJ += "脑脊膜炎(化脓性或无菌性)" + ";";
                            if (dicData.ContainsKey("F305") && dicData["F305"] == "1")
                                vo.SFMZFYQXGSJ += "脊痛(脊椎关节炎、脊椎骨髓炎)" + ";";
                            if (dicData.ContainsKey("F306") && dicData["F306"] == "1")
                                vo.SFMZFYQXGSJ += "脑神经麻痹" + ";";
                            if (dicData.ContainsKey("F307") && dicData["F307"] == "1")
                                vo.SFMZFYQXGSJ += "粘连性蛛网膜炎" + ";";
                            if (dicData.ContainsKey("F308") && dicData["F308"] == "1")
                                vo.SFMZFYQXGSJ += "高平面脊麻" + ";";

                            if (dicData.ContainsKey("F401") && dicData["F401"] == "1")
                                vo.SFMZFYQXGSJ += "局麻药品毒性反应" + ";";
                            if (dicData.ContainsKey("F402") && dicData["F402"] == "1")
                                vo.SFMZFYQXGSJ += "局部血肿" + ";";
                            if (dicData.ContainsKey("F403") && dicData["F403"] == "1")
                                vo.SFMZFYQXGSJ += "误注蛛网膜下腔" + ";";
                            if (dicData.ContainsKey("F404") && dicData["F404"] == "1")
                                vo.SFMZFYQXGSJ += "误注硬膜外间隙" + ";";
                            if (dicData.ContainsKey("F405") && dicData["F405"] == "1")
                                vo.SFMZFYQXGSJ += "霍纳氏综合症" + ";";
                            if (dicData.ContainsKey("F406") && dicData["F406"] == "1")
                                vo.SFMZFYQXGSJ += "膈神经阻滞" + ";";
                            if (dicData.ContainsKey("F407") && dicData["F407"] == "1")
                                vo.SFMZFYQXGSJ += "喉返神经阻滞" + ";";
                            if (dicData.ContainsKey("F408") && dicData["F408"] == "1")
                                vo.SFMZFYQXGSJ += "椎动脉刺伤出血" + ";";
                            if (dicData.ContainsKey("F409") && dicData["F409"] == "1")
                                vo.SFMZFYQXGSJ += "血胸" + ";";
                            if (dicData.ContainsKey("F4010") && dicData["F4010"] == "1")
                                vo.SFMZFYQXGSJ += "气胸" + ";";

                            if (dicData.ContainsKey("F501") && dicData["F501"] == "1")
                                vo.SFMZFYQXGSJ += "感染" + ";";
                            if (dicData.ContainsKey("F502") && dicData["F502"] == "1")
                                vo.SFMZFYQXGSJ += "出血" + ";";
                            if (dicData.ContainsKey("F503") && dicData["F503"] == "1")
                                vo.SFMZFYQXGSJ += "神经和淋巴管损伤" + ";";
                            if (dicData.ContainsKey("F504") && dicData["F504"] == "1")
                                vo.SFMZFYQXGSJ += "心律失常" + ";";
                            if (dicData.ContainsKey("F505") && dicData["F505"] == "1")
                                vo.SFMZFYQXGSJ += "栓塞" + ";";
                            if (dicData.ContainsKey("F506") && dicData["F506"] == "1")
                                vo.SFMZFYQXGSJ += "气胸" + ";";
                            if (dicData.ContainsKey("F507") && dicData["F507"] == "1")
                                vo.SFMZFYQXGSJ += "血胸" + ";";
                            if (dicData.ContainsKey("F508") && dicData["F508"] == "1")
                                vo.SFMZFYQXGSJ += "心包堵塞" + ";";

                            if (dicData.ContainsKey("F601") && dicData["F601"] == "1")
                                vo.SFMZFYQXGSJ += "血栓形成" + ";";
                            if (dicData.ContainsKey("F602") && dicData["F602"] == "1")
                                vo.SFMZFYQXGSJ += "假性动脉瘤" + ";";
                            if (dicData.ContainsKey("F603") && dicData["F603"] == "1")
                                vo.SFMZFYQXGSJ += "肢体缺血" + ";";
                            if (dicData.ContainsKey("F604") && dicData["F604"] == "1")
                                vo.SFMZFYQXGSJ += "感染" + ";";
                            if (dicData.ContainsKey("F605") && dicData["F605"] == "1")
                                vo.SFMZFYQXGSJ += "出血" + ";";

                            if (dicData.ContainsKey("F701") && dicData["F701"] == "1")
                                vo.SFMZFYQXGSJ += "意识障碍" + ";";
                            if (dicData.ContainsKey("F702") && dicData["F702"] == "1")
                                vo.SFMZFYQXGSJ += "氧饱和度降低" + ";";
                            if (dicData.ContainsKey("F703") && dicData["F703"] == "1")
                                vo.SFMZFYQXGSJ += "使用催醒药" + ";";
                            if (dicData.ContainsKey("F704") && dicData["F704"] == "1")
                                vo.SFMZFYQXGSJ += "低体温" + ";";
                            if (dicData.ContainsKey("F705") && dicData["F705"] == "1")
                                vo.SFMZFYQXGSJ += "入PACU>3h" + ";";
                            if (dicData.ContainsKey("F706") && dicData["F706"] == "1")
                                vo.SFMZFYQXGSJ += "非计划入ICU";
                            if (dicData.ContainsKey("F707") && dicData["F707"] == "1")
                                vo.SFMZFYQXGSJ += "非计划二次气管插管";
                            if (dicData.ContainsKey("F708") && dicData["F708"] == "1")
                                vo.SFMZFYQXGSJ += "过敏" + ";";
                            if (dicData.ContainsKey("F709") && dicData["F709"] == "1")
                                vo.SFMZFYQXGSJ += "昏迷" + ";";
                            if (dicData.ContainsKey("F7010") && dicData["F7010"] == "1")
                                vo.SFMZFYQXGSJ += "24h内死亡" + ";";
                            if (string.IsNullOrEmpty(dicData["F711"]) || dicData["F711"] != "")
                                vo.SFMZFYQXGSJ += dicData["F711"];
                            #endregion

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

        #region GetAnaRegister2
        /// <summary>
        /// GetAnaRegister2
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityAnaRegister2> GetAnaRegister2(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            List<EntityAnaRegister2> data = new List<EntityAnaRegister2>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                DataTable dt1 = null;
                DataTable dt2 = null;

                #region Sql1
                Sql = @"select a.anaid_int,
                               c.pcastarttime_dat  AS ZTKSSJ, --镇痛开始时间              
                               c.pcaendtime_dat    AS TBSJ, --ana_collection
                               h.officename        AS KS, --科室            
                               a.bedno_vchr        AS CW, --床位               
                               a.patientname_vchr  AS XM, --姓名  
                               a.operationname_chr AS SSMC, --手术名称         
                               c.poseasepain       AS ZTFS, --镇痛方式                
                               e.vsa_chr           AS VASPF, --VAS评分
                               e.sideeffect_vchr   AS SFBFZ, --是否并发症 
                               NULL                AS MZYS, --麻醉医师
                               a.statXml2 
                          from t_ana_requisition a
                          left join ana_collection c
                            on a.anaid_int = c.anaesthesiaid_int
                          left join t_opr_ana_madicalrecord d
                            on a.INPATIENTID_CHR = d.INPATIENTID_VCHR
                          left join t_opr_ana_postoperativerecord e
                            on a.anaid_int = e.anaid_int
                         inner join ana_record_report h
                            on a.anaid_int = h.anaesthesiaid_int
                         where a.status_int >= 1
                           and c.pcastarttime_dat between ? and ?
                         order by c.pcastarttime_dat asc                 
                         ";
                parm = svc.CreateParm(2);
                parm[0].Value = Function.Datetime(beginDate + " 00:00:00");
                parm[1].Value = Function.Datetime(endDate + " 23:59:59");
                dt1 = svc.GetDataTable(Sql, parm);
                #endregion

                #region Sql2

                Sql = @" select a.anaid_int, s.tag_chr, s.employeename_chr
                           from t_ana_requisition a
                          inner join t_opr_nursrecord b
                             on a.anaid_int = b.anaid_int
                           left join t_ana_sign s
                             on a.signsequence_int = s.sequenceid_int
                          where a.status_int >= 1
                            and b.status_int = 1
                            and b.inrometime_dat between ? and ?";

                parm = svc.CreateParm(2);
                parm[0].Value = Function.Datetime(beginDate + " 00:00:00");
                parm[1].Value = Function.Datetime(endDate + " 23:59:59");
                dt2 = svc.GetDataTable(Sql, parm);

                #endregion

                #region 赋值
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    int n = 0;
                    decimal anaId = 0;
                    string xmlData = string.Empty;
                    Dictionary<string, string> dicData = new Dictionary<string, string>();
                    DataRow[] drr = null;
                    EntityAnaRegister2 vo = null;
                    foreach (DataRow dr in dt1.Rows)
                    {
                        #region vo
                        if (dr["ZTFS"].ToString().Trim() == "无")
                            continue;
                        vo = new EntityAnaRegister2();
                        /// AnaId
                        vo.AnaId = Function.Dec(dr["anaid_int"].ToString());
                        /// 序号
                        vo.XH = ++n;
                        /// 镇痛开始时间
                        vo.ZTKSSJ = Convert.ToDateTime(dr["ZTKSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm");
                        /// 停泵时间
                        if (!string.IsNullOrEmpty(dr["TBSJ"].ToString()))
                            vo.TBSJ = Convert.ToDateTime(dr["TBSJ"].ToString()).ToString("yyyy-MM-dd HH:mm");
                        else
                            vo.TBSJ = "";
                        /// 科室
                        vo.KS = dr["KS"].ToString();
                        /// 床位
                        vo.CW = dr["CW"].ToString();
                        /// 姓名
                        vo.XM = dr["XM"].ToString();
                        /// 手术名称
                        vo.SSMC = dr["SSMC"].ToString();
                        /// 镇痛方式
                        vo.ZTFS = dr["ZTFS"].ToString();

                        /// VAS评分
                        //vo.VASPF = dr["VASPF"].ToString();
                        string VASPF = dr["VASPF"].ToString();
                        if (VASPF.Trim() == "0")
                            vo.VASPF = "";
                        else if (VASPF.Trim() == "1")
                            vo.VASPF = "1-3分";
                        else if (VASPF.Trim() == "2")
                            vo.VASPF = "4-6分";
                        else if (VASPF.Trim() == "3")
                            vo.VASPF = "7-10分";

                        /// 是否并发症
                        //vo.SFBFZ = dr["SFBFZ"].ToString();
                        //if (vo.SFBFZ == "1") vo.SFBFZ = "是";
                        /// 是否并发症
                        string SFBFZ = dr["SFBFZ"].ToString();
                        string[] strarr = SFBFZ.Trim().Split(',');
                        foreach (var i in strarr)
                        {
                            if (i.Trim() == "1")
                                vo.SFBFZ = "";
                            else if (i.Trim() == "2")
                                vo.SFBFZ += "恶心";
                            else if (i.Trim() == "3")
                                vo.SFBFZ += "呕吐";
                            else if (i.Trim() == "4")
                                vo.SFBFZ += "尿潴留";
                            else if (i.Trim() == "5")
                                vo.SFBFZ += "镇静过度";
                            else if (i.Trim() == "6")
                                vo.SFBFZ += "呼吸抑制";
                            else if (i.Trim() == "7")
                                vo.SFBFZ += "肢体乏力";
                            else
                            {
                                vo.SFBFZ += i;
                            }

                        }
                        /// 麻醉医师
                        vo.MZYS = dr["MZYS"].ToString();
                        #endregion

                        #region 计算
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            anaId = Function.Dec(dr["anaid_int"].ToString());
                            drr = dt2.Select("anaid_int = " + anaId);
                            if (drr != null && drr.Length > 0)
                            {
                                foreach (DataRow dr2 in drr)
                                {
                                    if (dr2["tag_chr"] != DBNull.Value)
                                    {
                                        if (dr2["tag_chr"].ToString().Trim() == "麻醉医师")
                                            vo.MZYS = dr2["employeename_chr"].ToString();
                                    }
                                }
                            }
                        }
                        #endregion

                        xmlData = dr["statXml2"].ToString();
                        if (!string.IsNullOrEmpty(xmlData))
                        {
                            dicData = Function.ReadXmlNodes(xmlData, "XmlData");
                            vo.SFBFZ = string.Empty;
                            if (dicData["F001"] == "1") vo.SFBFZ += "恶心呕吐、";
                            if (dicData["F002"] == "1") vo.SFBFZ += "排尿困难、";
                            if (dicData["F003"] == "1") vo.SFBFZ += "皮肤瘙痒、";
                            if (dicData["F004"] == "1") vo.SFBFZ += "镇痛不全、";
                            if (dicData["F005"] == "1") vo.SFBFZ += "硬膜外导管脱出、";
                            if (dicData["F006"] == "1") vo.SFBFZ += "感觉及运动异常、";
                            if (dicData["F007"] == "1") vo.SFBFZ += "镇静过度、";
                            if (dicData["F008"] == "1") vo.SFBFZ += "呼吸抑制、";
                            if (vo.SFBFZ != string.Empty) vo.SFBFZ = vo.SFBFZ.TrimEnd('、');
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

        #region GetAnaStat1
        /// <summary>
        /// GetAnaStat1
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityAnaStat1> GetAnaStat1(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string Sql1 = string.Empty;
            List<EntityAnaStat1> data = new List<EntityAnaStat1>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                IDataParameter[] parm1 = null;

                #region Sql
                Sql = @"select distinct a.anaid_int,
                               b.operate_date             AS RSSSRQSJ, --入手术室日期时间
                               a.bedno_vchr                AS CW, --床位
                               a.inpatientid_chr           AS ZYH, --住院号
                               c.ananumber_all             AS MZBH, --麻醉编号
                               a.patientname_vchr          AS XM, --姓名
                               a.sex_vchr                  AS XB, --性别
                               a.age_vchr                  AS NL, --年龄
                               a.preoperativediagnosis_chr AS SQZD, --术前诊断
                               a.operationname_chr         AS SSMC, --手术名称
                               c.asalevel_all              AS MZFJ, --麻醉分级
                               a.asalevel_chr              AS SSFJ, --手术分级
                               c.anamode_all               AS MZFS, --麻醉方式
                               c.poseasepain               AS ZTFS, --镇痛方式
                               d.cpr_chr                   AS SFXFFS, --是否心肺复苏
                               d.outpacusteward_chr        AS LSSF, --离室Steward ≥4分
                               c.anaeffect_all             AS MZXG, --麻醉效果
                               a.statXml1,
                               e1.event_id                 as ZXJM, -- 中心静脉
                               a.Emergency_Chr             as SSLB, -- 手术类别
                               e2.event_id                 as QGCG, -- 气管插管
                               e3.event_id                 as QGBG, -- 气管拔管
                               e4.event_id                 as JRFSS, -- 进入复苏室
                               e6.event_id                 as JRFSS2,--进入复苏室
                               e5.event_id                 as JRICU, -- 进入ICU 
                               e7.event_id                 as QGLG, -- 气管留管 
                               e8.event_id                 as DMCC,  -- 动脉穿刺
                               c.anasummary_all
                          from icare.t_ana_requisition a
                         inner join icare.ana_collection_eventcontent b
                            on a.anaid_int = b.anaid_int
                          left join anaesthesia_event f
                            on b.event_id = f.event_id
                          left join ana_collection c
                            on a.anaid_int = c.anaesthesiaid_int
                          left join icare.t_opr_ana_madicalrecord d
                            on a.INPATIENTID_CHR = d.INPATIENTID_VCHR
                          left join ana_collection_eventcontent e1
                            on a.anaid_int = e1.anaid_int
                           and e1.event_id = '0039'
                          left join ana_collection_eventcontent e2
                            on a.anaid_int = e2.anaid_int
                           and e2.event_id = '0002'
                          left join ana_collection_eventcontent e3
                            on a.anaid_int = e3.anaid_int
                           and e3.event_id = '0003'
                          left join ana_collection_eventcontent e4
                            on a.anaid_int = e4.anaid_int
                           and e4.event_id = '0043'  
                          left join ana_collection_eventcontent e5
                            on a.anaid_int = e5.anaid_int
                           and e5.event_id = '0049'
                          left join ana_collection_eventcontent e6
                            on a.anaid_int = e6.anaid_int
                           and e6.event_id = '0045' 
                            left join ana_collection_eventcontent e7
                            on a.anaid_int = e7.anaid_int
                           and e7.event_id = '0090' 
                           left join ana_collection_eventcontent e8
                            on a.anaid_int = e8.anaid_int
                           and e8.event_id = '0079' 

                         where a.status_int >= 1
                           and b.status = 0
                          and f.event_id = '0054'
                           and b.operate_date between ? and  ?
                         order by b.operate_date asc                  
                         ";

                parm = svc.CreateParm(2);
                parm[0].Value = Function.Datetime(beginDate + " 00:00:00");
                parm[1].Value = Function.Datetime(endDate + " 23:59:59");
                DataTable dt = svc.GetDataTable(Sql, parm);
                #endregion
                
                #region Sql1  
                Sql1 = @"select Fmonth, 
                                Field1,
                                Field2,
                                Field3,
                                Field4
                         from t_ana_stattemp
                         where Fmonth between ? and ? 
                         order by Fmonth ";

                parm1 = svc.CreateParm(2);
                parm1[0].Value = Function.Datetime(beginDate.ToString()).ToString("yyyy-MM");
                parm1[1].Value = Function.Datetime(endDate.ToString()).ToString("yyyy-MM");
                DataTable dt1 = svc.GetDataTable(Sql1, parm1);

                #endregion

                #region 赋值
                if (dt != null && dt.Rows.Count > 0)
                {
                    string month1 = string.Empty;
                    string month = string.Empty;
                    string xmlData = string.Empty;
                    Dictionary<string, string> dicData = new Dictionary<string, string>();

                    string MZXG = string.Empty;
                    string MZFS = string.Empty;
                    string ZTFS = string.Empty;
                    string ANALEVEL = string.Empty;
                    foreach (DataRow dr in dt.Rows)
                    {
                        month = Function.Datetime(dr["RSSSRQSJ"].ToString()).ToString("yyyy-MM");
                        xmlData = dr["statXml1"].ToString();
                        if (!string.IsNullOrEmpty(xmlData))
                        {
                            dicData = Function.ReadXmlNodes(xmlData, "XmlData");
                        }
                        else 
                        {
                            dicData = new Dictionary<string, string>();
                        }

                        if (data.Any(t => t.YF == month))
                        {
                            #region 累计

                            EntityAnaStat1 voClone = data.FirstOrDefault(t => t.YF == month);
                            if (dr["MZFS"] != DBNull.Value)
                            {
                                MZFS = dr["MZFS"].ToString().Trim();
                               if (MZFS.IndexOf("+") > 0 || MZFS.IndexOf("＋") > 0)
                                    /// 麻醉总例数   复合麻醉
                                    voClone.MZZLS_FHMZ += 1;
                                else if (MZFS.IndexOf("蛛网膜下腔")>=0 || MZFS.IndexOf("硬膜外麻醉")>=0 || MZFS.IndexOf("腰硬联合麻醉")>=0 
                                   || MZFS.IndexOf("骶管麻醉")>=0 || MZFS.IndexOf("骶麻")>=0 || MZFS.IndexOf("腰硬联合")>=0 || 
                                   MZFS.IndexOf("腰硬联合麻")>0 || MZFS.IndexOf("连续硬膜外腔阻滞")>=0 || MZFS.IndexOf("连续硬膜外麻醉")>=0)
                                    /// 麻醉总例数   椎管内麻醉
                                    voClone.MZZLS_ZGNMZ += 1;
                               else if (MZFS.IndexOf("气管插管") >= 0 || MZFS.IndexOf("喉罩插管") >= 0 
                                   || MZFS.IndexOf("静吸复合") >=0)
                                    /// 麻醉总例数   插管全麻
                                    voClone.MZZLS_CGMZ += 1;
                                else if (MZFS.IndexOf("静脉全麻")>=0 || MZFS.IndexOf("非气管插管") >= 0 )
                                    /// 麻醉总例数   非插管全麻
                                    voClone.MZZLS_FCGMZ += 1;
                               else if (MZFS != string.Empty)
                               {
                                   /// 麻醉总例数   其他
                                   voClone.MZZLS_QT += 1;
                               }
                            }
                            if (dr["ZTFS"] != DBNull.Value)
                            {
                                ZTFS = dr["ZTFS"].ToString().Trim().ToUpper();
                                if (ZTFS == "PCEA")
                                    /// 术后镇痛例数  硬膜外镇痛
                                    voClone.SHZTLS_YMWZT += 1;
                                else if (ZTFS == "PCIA")
                                    /// 术后镇痛例数  静脉镇痛
                                    voClone.SHZTLS_JMZT += 1;
                                else if (ZTFS == "PCNA")
                                    /// 术后镇痛例数  神经阻滞
                                    voClone.SHZTLS_SJZZ += 1;
                                else if (!string.IsNullOrEmpty(ZTFS.Trim()) && ZTFS.Trim() != "无")
                                    /// 术后镇痛例数  其他
                                    voClone.SHZTLS_QT += 1;
                            }
                            //if (dicData.ContainsKey("F011") && dicData["F011"] == "1")
                            //    /// 手术室外麻醉	分娩镇痛
                            //    voClone.SSSWMZ_FMZT += 1;
                            //if (dicData.ContainsKey("F012") && dicData["F012"] == "1")
                            //    /// 手术室外麻醉	无痛胃肠镜
                            //    voClone.SSSWMZ_WTWCJ += 1;
                            //if (dicData.ContainsKey("F013") && dicData["F013"] == "1")
                            //    /// 手术室外麻醉	无痛人流
                            //    voClone.SSSWMZ_WTRL += 1;
                            //if (dicData.ContainsKey("F014") && dicData["F014"] == "1")
                            //    /// 手术室外麻醉	其他
                            //    voClone.SSSWMZ_QT += 1;

                            if (dr["ZXJM"] != DBNull.Value && dr["ZXJM"].ToString() != string.Empty)
                                /// 血管穿刺例数	中心静脉
                                voClone.XGCCLS_ZXJM += 1;
                            if ((dicData.ContainsKey("F007") && dicData["F007"] == "1") || dr["DMCC"] != DBNull.Value)
                                /// 血管穿刺例数	动脉
                                voClone.XGCCLS_DM += 1;

                            if (dr["SSLB"] != DBNull.Value)
                            {
                                if (dr["SSLB"].ToString().IndexOf("择期") >= 0)
                                    /// 手术类别例数	择期
                                    voClone.SSLBLS_ZQ += 1;
                                else if (dr["SSLB"].ToString().IndexOf("急诊") >= 0)
                                    /// 手术类别例数	急诊
                                    voClone.SSLBLS_JZ += 1;
                            }

                            if (dicData.ContainsKey("F005") && dicData["F005"] == "1")
                                /// 手术类别例数	醉后手术取消
                                voClone.SSLBLS_ZHSSQX += 1;

                            if ( dicData.ContainsKey("F021") && dicData["F021"] == "1")
                                /// 输血例数	术中输血
                                voClone.SXLS_SZSX += 1;
                            if (dicData.ContainsKey("F022") && dicData["F022"] == "1")
                                /// 输血例数 <400ml
                                voClone.SXLS_X400 += 1;
                            if (dicData.ContainsKey("F023") && dicData["F023"] == "1")
                                /// 输血例数	>=400ml
                                voClone.SXLS_D400 += 1;
                            if (dr["QGCG"] != DBNull.Value && dr["QGLG"].ToString() != string.Empty)
                             //气管插管管理例数	带管出手术室
                            voClone.QGCGGLLS_DGCSSS += 1;
                            if (dr["QGBG"] != DBNull.Value && dr["QGBG"].ToString() != string.Empty)
                                /// 气管插管管理例数	拔管出手术室
                                voClone.QGCGGLLS_BGCSSS += 1;

                            if (dr["JRFSS"] != DBNull.Value && dr["JRFSS"].ToString() != string.Empty)
                                /// 麻醉复苏管理例数	进入复苏室
                                voClone.MZFSGLLS_JRFSS += 1;
                            if (dr["JRFSS2"] != DBNull.Value && dr["JRFSS2"].ToString() != string.Empty)
                                /// 麻醉复苏管理例数	进入复苏室
                                voClone.MZFSGLLS_JRFSS += 1;

                            if (dicData.ContainsKey("F001") && dicData["F001"] == "1")
                                /// 麻醉复苏管理例数	进入复苏室
                                voClone.MZFSGLLS_JRFSS += 1;
                            if (dicData.ContainsKey("F031") && dicData["F031"].Trim() != string.Empty)
                                /// 麻醉复苏管理例数	离室steward>=4分
                                voClone.MZFSGLLS_LSSF += 1;
                            if (dicData.ContainsKey("F006") && dicData["F006"] == "1")
                                /// 心肺复苏例数	h内心跳骤停
                                voClone.XFFSLS_HNXTZT += 1;
                            if (dicData.ContainsKey("F004") && dicData["F004"] == "1")
                                /// 心肺复苏例数	复苏成功
                                voClone.XFFSLS_FSCG += 1;

                            #region 麻醉非预期相关事件例数
                            if (dicData.ContainsKey("F701") && dicData["F701"] == "1")
                                /// 麻醉非预期相关事件例数	意识障碍
                                voClone.MZFYQSJ_YSZA += 1;
                            if (dicData.ContainsKey("F702") && dicData["F702"] == "1")
                                /// 麻醉非预期相关事件例数	氧饱和度降低
                                voClone.MZFYQSJ_YBHDJD += 1;
                            if (dicData.ContainsKey("F703") && dicData["F703"] == "1")
                                 /// 麻醉非预期相关事件例数	使用催醒药
                                voClone.MZFYQSJ_SYCXY += 1;
                            if (!string.IsNullOrEmpty(xmlData) && dicData.ContainsKey("F704") && dicData["F704"] == "1")
                                /// 麻醉非预期相关事件例数	低体温
                                voClone.MZFYQSJ_DTW += 1;
                            if (dicData.ContainsKey("F705") && dicData["F705"] == "1")
                                /// 麻醉非预期相关事件例数	入PACU>3h
                                voClone.MZFYQSJ_RPACU3H += 1;
                            if (dicData.ContainsKey("F706") && dicData["F706"] == "1")
                                /// 麻醉非预期相关事件例数	非计划入ICU
                                voClone.MZFYQSJ_FJHRICU += 1;
                            if (dicData.ContainsKey("F707") && dicData["F707"] == "1")
                                /// 麻醉非预期相关事件例数	气管插管非计划二次
                                voClone.MZFYQSJ_QGCGFJHEC += 1;
                            if (!string.IsNullOrEmpty(xmlData) && dicData.ContainsKey("F708") && dicData["F708"] == "1")
                                /// 麻醉非预期相关事件例数	过敏
                                voClone.MZFYQSJ_GM += 1;
                            if (dicData.ContainsKey("F1015") && dicData["F1015"] == "1")
                                /// 麻醉非预期相关事件例数	误吸
                                voClone.MZFYQSJ_WX += 1;
                            if (dicData.ContainsKey("F709") && dicData["F709"] == "1")
                                /// 麻醉非预期相关事件例数	昏迷
                                voClone.MZFYQSJ_HM += 1;
                            if (dicData.ContainsKey("F710") && dicData["F710"] == "1")
                                /// 麻醉非预期相关事件例数	24h内死亡
                                voClone.MZFYQSJ_24NSW += 1;
                            if (dicData.ContainsKey("F207") && dicData["F207"] == "1")
                                /// 麻醉非预期相关事件例数	神经损伤
                                voClone.MZFYQSJ_SJSS += 1;
                            if (dicData.ContainsKey("F104") && dicData["F104"] == "1")
                                /// 麻醉非预期相关事件例数	声音嘶哑
                                voClone.MZFYQSJ_SYSY += 1;

                            /// 麻醉非预期相关事件例数	中心静脉穿刺
                            if ((dicData.ContainsKey("F501") && dicData["F501"] == "1") ||
                                (dicData.ContainsKey("F502") && dicData["F502"] == "1") ||
                                (dicData.ContainsKey("F503") && dicData["F503"] == "1") ||
                                (dicData.ContainsKey("F504") && dicData["F504"] == "1") ||
                                (dicData.ContainsKey("F505") && dicData["F505"] == "1") ||
                                (dicData.ContainsKey("F506") && dicData["F506"] == "1") ||
                                (dicData.ContainsKey("F507") && dicData["F507"] == "1") ||
                                (dicData.ContainsKey("F508") && dicData["F508"] == "1"))
                            {
                                voClone.MZFYQSJ_ZXJMCC += 1;
                            }

                            /// 麻醉非预期相关事件例数	动脉穿刺
                            if ((dicData.ContainsKey("F601") && dicData["F601"] == "1") ||
                                (dicData.ContainsKey("F602") && dicData["F602"] == "1") ||
                                (dicData.ContainsKey("F603") && dicData["F603"] == "1") ||
                                (dicData.ContainsKey("F604") && dicData["F604"] == "1") ||
                                (dicData.ContainsKey("F605") && dicData["F605"] == "1") ) 
                            {
                                voClone.MZFYQSJ_JDMCC += 1;
                            }

                            #region 麻醉非预期事件 其他
                            if ((dicData.ContainsKey("F101") && dicData["F101"] == "1") || //口纯损伤
                                (dicData.ContainsKey("F102") && dicData["F102"] == "1") || //鼻腔出血
                                (dicData.ContainsKey("F103") && dicData["F103"] == "1") || //喉痛
                                (dicData.ContainsKey("F104") && dicData["F104"] == "1") || //声音嘶哑
                                (dicData.ContainsKey("F105") && dicData["F105"] == "1") ||  //颈部皮下气肿
                                (dicData.ContainsKey("F106") && dicData["F106"] == "1") ||//牙齿松动或脱落
                                (dicData.ContainsKey("F107") && dicData["F107"] == "1") || //杓状软骨脱位
                                (dicData.ContainsKey("F108") && dicData["F108"] == "1") || //高血压
                                (dicData.ContainsKey("F109") && dicData["F109"] == "1") ||  //高颅压
                                (dicData.ContainsKey("F1010") && dicData["F1010"] == "1") ||  //缺氧和二氧化碳蓄积
                                (dicData.ContainsKey("F1011") && dicData["F1011"] == "1") ||  //气道阻塞
                                (dicData.ContainsKey("F1012") && dicData["F1012"] == "1") || //喉头水肿
                                (dicData.ContainsKey("F1013") && dicData["F1013"] == "1") || //"插管困难
                                (dicData.ContainsKey("F1014") && dicData["F1014"] == "1") || //呕吐
                                //(dicData.ContainsKey("F1015") && dicData["F1015"] == "1") || //返流误吸
                                (dicData.ContainsKey("F1016") && dicData["F1016"] == "1") || //喉痉挛
                                (dicData.ContainsKey("F1017") && dicData["F1017"] == "1") || //咳嗽和呛咳
                                (dicData.ContainsKey("F1018") && dicData["F1018"] == "1") || //呃逆
                                (dicData.ContainsKey("F1019") && dicData["F1019"] == "1") || //体温升高或降低
                                (dicData.ContainsKey("F1020") && dicData["F1020"] == "1") || //恶性高热
                                (dicData.ContainsKey("F1021") && dicData["F1021"] == "1") || //张力性气胸
                                (dicData.ContainsKey("F1022") && dicData["F1022"] == "1") || //支气管痉挛
                                (dicData.ContainsKey("F1023") && dicData["F1023"] == "1") || //支气管痉挛
                                (dicData.ContainsKey("F1024") && dicData["F1024"] == "1") || //药物变态反应
                                (dicData.ContainsKey("F1025") && dicData["F1025"] == "1") || //急性心肌梗塞
                                (dicData.ContainsKey("F1026") && dicData["F1026"] == "1") || //术中心律失常
                                (dicData.ContainsKey("F1027") && dicData["F1027"] == "1") || //心力衰竭
                                (dicData.ContainsKey("F1028") && dicData["F1028"] == "1") || //心跳停止
                                (dicData.ContainsKey("F1029") && dicData["F1029"] == "1") || //脑血管意外
                                (dicData.ContainsKey("F1030") && dicData["F1030"] == "1") || //呼吸恢复延迟
                                (dicData.ContainsKey("F1030") && dicData["F1030"] == "1") || //呼吸困难
                                (dicData.ContainsKey("F1031") && dicData["F1031"] == "1") || //部感染
                                (dicData.ContainsKey("F1032") && dicData["F1032"] == "1") || //肺不张肺
                                (dicData.ContainsKey("F1033") && dicData["F1033"] == "1") || //张力性气胸
                                (dicData.ContainsKey("F1034") && dicData["F1034"] == "1") || //肺不张
                                (dicData.ContainsKey("F1035") && dicData["F1035"] == "1") || //肺栓塞
                                (dicData.ContainsKey("F1036") && dicData["F1036"] == "1") || //呼吸衰竭
                                (dicData.ContainsKey("F201") && dicData["F201"] == "1") || //穿破硬脊膜
                                (dicData.ContainsKey("F202") && dicData["F202"] == "1") || //穿刺针或导管误入血管
                                (dicData.ContainsKey("F203") && dicData["F203"] == "1") || //导管折断
                                (dicData.ContainsKey("F204") && dicData["F204"] == "1") || //局麻药中毒性反应
                                (dicData.ContainsKey("F205") && dicData["F205"] == "1") || //严重低血压
                                (dicData.ContainsKey("F206") && dicData["F206"] == "1") || //异常广泛神经阻滞
                                (dicData.ContainsKey("F207") && dicData["F207"] == "1") ||  //神经根/脊髓损伤
                                (dicData.ContainsKey("F208") && dicData["F208"] == "1") || //硬膜外血肿
                                (dicData.ContainsKey("F209") && dicData["F209"] == "1") || //截瘫
                                (dicData.ContainsKey("F2010") && dicData["F2010"] == "1") || //硬膜外脓肿
                                (dicData.ContainsKey("F2011") && dicData["F2011"] == "1") || //呼吸麻痹
                                (dicData.ContainsKey("F2012") && dicData["F2012"] == "1") || //粘连性蛛网膜炎
                                (dicData.ContainsKey("F2013") && dicData["F2013"] == "1") || //脊髓前动脉综合症
                                (dicData.ContainsKey("F2014") && dicData["F2014"] == "1") || //全脊麻硬
                                (dicData.ContainsKey("F2015") && dicData["F2015"] == "1") || //腰背痛
                                (dicData.ContainsKey("F2016") && dicData["F2016"] == "1") || //空气栓塞
                                (dicData.ContainsKey("F2017") && dicData["F2017"] == "1") || //感染
                                (dicData.ContainsKey("F2018") && dicData["F2018"] == "1") || //头痛
                                (dicData.ContainsKey("F301") && dicData["F301"] == "1") || //恶心呕吐
                                (dicData.ContainsKey("F302") && dicData["F302"] == "1") || //脊麻后头痛
                                (dicData.ContainsKey("F303") && dicData["F303"] == "1") || //尿潴留
                                (dicData.ContainsKey("F304") && dicData["F304"] == "1") || //脑脊膜炎(化脓性或无菌性)
                                (dicData.ContainsKey("F305") && dicData["F305"] == "1") || //脊痛(脊椎关节炎、脊椎骨髓炎
                                (dicData.ContainsKey("F306") && dicData["F306"] == "1") || //脑神经麻痹
                                (dicData.ContainsKey("F307") && dicData["F307"] == "1") || //粘连性蛛网膜炎
                                (dicData.ContainsKey("F308") && dicData["F308"] == "1") ||  //高平面脊麻
                                (dicData.ContainsKey("F401") && dicData["F401"] == "1") || //局麻药品毒性反应
                                (dicData.ContainsKey("F402") && dicData["F402"] == "1") || //局部血肿
                                (dicData.ContainsKey("F403") && dicData["F403"] == "1") || //误注蛛网膜下腔
                                (dicData.ContainsKey("F404") && dicData["F404"] == "1") || //误注硬膜外间隙
                                (dicData.ContainsKey("F405") && dicData["F405"] == "1") || //霍纳氏综合症
                                (dicData.ContainsKey("F406") && dicData["F406"] == "1") || //膈神经阻滞
                                (dicData.ContainsKey("F407") && dicData["F407"] == "1") || //喉返神经阻滞
                                (dicData.ContainsKey("F408") && dicData["F408"] == "1") || //椎动脉刺伤出血
                                (dicData.ContainsKey("F409") && dicData["F409"] == "1") || //血胸
                                (dicData.ContainsKey("F4010") && dicData["F4010"] == "1") ||
                                (dicData.ContainsKey("F711") && dicData["F711"].Trim() != string.Empty)) //其他
                            {
                                voClone.MZFYQSJ_QT += 1;
                            }
                               
                            #endregion
                            //if (dicData.ContainsKey("F007") && dicData["F007"] == "1")
                            //    /// 麻醉非预期相关事件例数	动静脉穿刺
                            //    voClone.MZFYQSJ_JDMCC += 1;
                            //if (dicData.ContainsKey("F711") && dicData["F711"].Trim() != string.Empty)
                                /// 麻醉非预期相关事件例数	其他
                               // voClone.MZFYQSJ_QT += 1;
                            #endregion
                            /* 暂时屏蔽
                            /// 麻醉效果管理例数	I
                            voClone.MZXGGLLS_I += 1;
                            /// 麻醉效果管理例数	II
                            voClone.MZXGGLLS_II += 1;
                            /// 麻醉效果管理例数	III
                            voClone.MZXGGLLS_III += 1;
                            /// 麻醉效果管理例数	IV
                            //voClone.MZXGGLLS_IV += 1;*/

                            if (dr["MZXG"] != DBNull.Value)
                            {
                                MZXG = dr["MZXG"].ToString().Trim();
                                if (MZXG == "优")
                                    /// 麻醉效果管理例数	I
                                    voClone.MZXGGLLS_I += 1;
                                else if (MZXG == "良")
                                    /// 麻醉效果管理例数	II
                                    voClone.MZXGGLLS_II += 1;
                                else if (MZXG == "中")
                                    /// 麻醉效果管理例数	III
                                    voClone.MZXGGLLS_III += 1;
                                else if (MZXG == "差")
                                    /// 麻醉效果管理例数	IV
                                    voClone.MZXGGLLS_IV += 1;
                            }

                            if (!string.IsNullOrEmpty(xmlData) && dicData.ContainsKey("F003") && dicData["F003"] == "1")
                                /// 麻醉效果管理例数	麻醉方式更改
                                voClone.MZXGGLLS_MZFSGG += 1;

                            if (dr["MZFJ"] != DBNull.Value)
                            {
                                ANALEVEL = dr["MZFJ"].ToString().Trim();
                                if (ANALEVEL == "I" || ANALEVEL.StartsWith("I("))
                                    /// 麻醉分级管理例数ASA	I
                                    voClone.MZFJGLLSASA_I += 1;
                                else if (ANALEVEL == "II" || ANALEVEL.StartsWith("II("))
                                    /// 麻醉分级管理例数ASA	II
                                    voClone.MZFJGLLSASA_II += 1;
                                else if (ANALEVEL == "III" || ANALEVEL.StartsWith("III("))
                                    /// 麻醉分级管理例数ASA	III
                                    voClone.MZFJGLLSASA_III += 1;
                                else if (ANALEVEL == "IV" || ANALEVEL.StartsWith("IV("))
                                    /// 麻醉分级管理例数ASA	IV
                                    voClone.MZFJGLLSASA_IV += 1;
                                else if (ANALEVEL == "V" || ANALEVEL.StartsWith("V("))
                                    /// 麻醉分级管理例数ASA	V
                                    voClone.MZFJGLLSASA_V += 1;
                            }

                            if (voClone.ZGYS_ZRYS == 0)
                            {
                                if (dicData.ContainsKey("F801") && dicData["F801"].Trim() != string.Empty)
                                {
                                    /// 在岗医师	主任医师
                                    voClone.ZGYS_ZRYS = dicData["F801"].Trim().Split(',').Length;
                                }
                            }

                            if (voClone.ZGYS_FZRYS == 0)
                            {
                                if (dicData.ContainsKey("F802") && dicData["F802"].Trim() != string.Empty)
                                {
                                    /// 在岗医师	副主任医师
                                    voClone.ZGYS_FZRYS = dicData["F802"].Trim().Split(',').Length;
                                }
                            }

                            if (voClone.ZGYS_ZZYS == 0)
                            {
                                if (dicData.ContainsKey("F803") && dicData["F803"].Trim() != string.Empty)
                                {
                                    /// 在岗医师	主治医师
                                    voClone.ZGYS_ZZYS = dicData["F803"].Trim().Split(',').Length;
                                }
                            }

                            if (voClone.ZGYS_ZYYS == 0)
                            {
                                if (dicData.ContainsKey("F804") && dicData["F804"].Trim() != string.Empty)
                                {
                                    /// 在岗医师	住院医师
                                    voClone.ZGYS_ZYYS = dicData["F804"].Trim().Split(',').Length;
                                }
                            }

                            if (dr["JRICU"] != DBNull.Value && dr["JRICU"].ToString() != string.Empty)
                                /// 进入ICU 合计
                                voClone.JRICU_HJ += 1;

                            foreach (DataRow dr1 in dt1.Rows)
                            {
                                month1 = Function.Datetime(dr1["Fmonth"].ToString()).ToString("yyyy-MM");
                                
                                if (month1 == month)
                                {
                                    //分娩镇痛
                                    voClone.SSSWMZ_FMZT = Function.Int(dr1["Field1"].ToString());
                                    //无痛胃肠镜
                                    voClone.SSSWMZ_WTWCJ = Function.Int(dr1["Field2"].ToString());
                                    //无痛人流
                                    voClone.SSSWMZ_WTRL = Function.Int(dr1["Field3"].ToString());
                                    //其他
                                    voClone.SSSWMZ_QT = Function.Int(dr1["Field4"].ToString());
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region vo
                            EntityAnaStat1 vo = new EntityAnaStat1();
                            /// 月份
                            vo.YF = month;
                            //if (dr["MZFS"] != DBNull.Value)
                            //{
                            //    MZFS = dr["MZFS"].ToString().Trim();
                            //    if (MZFS == "蛛网膜下腔" || MZFS == "硬膜外麻醉" || MZFS == "腰硬联合麻醉" || MZFS == "骶管麻醉" || MZFS == "骶麻")
                            //        /// 麻醉总例数   椎管内麻醉
                            //        vo.MZZLS_ZGNMZ = 1;
                            //    else if (MZFS == "气管插管全麻" || MZFS == "双腔支气管插管全麻" || MZFS == "喉罩插管全麻" || MZFS == "静吸复合全麻")
                            //        /// 麻醉总例数   插管全麻
                            //        vo.MZZLS_CGMZ = 1;
                            //    else if (MZFS == "气管插管静脉全麻" || MZFS == "静脉全麻")
                            //        /// 麻醉总例数   非插管全麻
                            //        vo.MZZLS_FCGMZ = 1;
                            //    else if (MZFS.IndexOf("+") > 0 || MZFS.IndexOf("＋") > 0)
                            //        /// 麻醉总例数   复合麻醉
                            //        vo.MZZLS_FHMZ = 1;
                            //    else if (MZFS != string.Empty)
                            //        /// 麻醉总例数   其他
                            //        vo.MZZLS_QT = 1;
                            //}
                            if (dr["MZFS"] != DBNull.Value)
                            {
                                MZFS = dr["MZFS"].ToString().Trim();
                                if (MZFS.IndexOf("+") > 0 || MZFS.IndexOf("＋") > 0)
                                    /// 麻醉总例数   复合麻醉
                                    vo.MZZLS_FHMZ = 1;
                                else if (MZFS.IndexOf("蛛网膜下腔") >= 0 || MZFS.IndexOf("硬膜外麻醉") >= 0 || MZFS.IndexOf("腰硬联合麻醉") >= 0
                                   || MZFS.IndexOf("骶管麻醉") >= 0 || MZFS.IndexOf("骶麻") >= 0 || MZFS.IndexOf("腰硬联合") >= 0 ||
                                   MZFS.IndexOf("腰硬联合麻") > 0 || MZFS.IndexOf("连续硬膜外腔阻滞") >= 0 || MZFS.IndexOf("连续硬膜外麻醉") >= 0)
                                    /// 麻醉总例数   椎管内麻醉
                                    vo.MZZLS_ZGNMZ = 1;
                                else if (MZFS.IndexOf("气管插管") >= 0 || MZFS.IndexOf("喉罩插管") >= 0
                                    || MZFS.IndexOf("静吸复合") >= 0)
                                    /// 麻醉总例数   插管全麻
                                    vo.MZZLS_CGMZ = 1;
                                else if (MZFS.IndexOf("静脉全麻") >= 0 || MZFS.IndexOf("非气管插管") >= 0)
                                    /// 麻醉总例数   非插管全麻
                                    vo.MZZLS_FCGMZ = 1;
                                else if (MZFS != string.Empty)
                                {
                                    /// 麻醉总例数   其他
                                    vo.MZZLS_QT = 1;
                                }
                            }
                            if (dr["ZTFS"] != DBNull.Value)
                            {
                                ZTFS = dr["ZTFS"].ToString().Trim().ToUpper();
                                if (ZTFS == "PCEA")
                                    /// 术后镇痛例数  硬膜外镇痛
                                    vo.SHZTLS_YMWZT = 1;
                                else if (ZTFS == "PCIA")
                                    /// 术后镇痛例数  静脉镇痛
                                    vo.SHZTLS_JMZT = 1;
                                else if (ZTFS == "PCNA")
                                    /// 术后镇痛例数  神经阻滞
                                    vo.SHZTLS_SJZZ = 1;
                                else if (!string.IsNullOrEmpty(ZTFS.Trim()) && ZTFS.Trim() != "无")
                                    /// 术后镇痛例数  其他
                                    vo.SHZTLS_QT = 1;
                            }

                            //if (dicData.ContainsKey("F011") && dicData["F011"] == "1")
                            //    /// 手术室外麻醉	分娩镇痛
                            //    vo.SSSWMZ_FMZT = 1;
                            //if (dicData.ContainsKey("F012") && dicData["F012"] == "1")
                            //    /// 手术室外麻醉	无痛胃肠镜
                            //    vo.SSSWMZ_WTWCJ = 1;
                            //if (dicData.ContainsKey("F013") && dicData["F013"] == "1")
                            //    /// 手术室外麻醉	无痛人流
                            //    vo.SSSWMZ_WTRL = 1;
                            //if (dicData.ContainsKey("F014") && dicData["F014"] == "1")
                            //    /// 手术室外麻醉	其他
                            //    vo.SSSWMZ_QT = 1;

                            if (dr["ZXJM"] != DBNull.Value && dr["ZXJM"].ToString() != string.Empty)
                                /// 血管穿刺例数	中心静脉
                                vo.XGCCLS_ZXJM = 1;
                            if ((dicData.ContainsKey("F007") && dicData["F007"] == "1") ||
                                dr["DMCC"] != DBNull.Value)
                                /// 血管穿刺例数	动脉
                                vo.XGCCLS_DM = 1;

                            if (dr["SSLB"] != DBNull.Value)
                            {
                                if (dr["SSLB"].ToString().IndexOf("择期") >= 0)
                                    /// 手术类别例数	择期
                                    vo.SSLBLS_ZQ = 1;
                                else if (dr["SSLB"].ToString().IndexOf("急诊") >= 0)
                                    /// 手术类别例数	急诊
                                    vo.SSLBLS_JZ = 1;
                            }

                            if (dicData.ContainsKey("F005") && dicData["F005"] == "1")
                                /// 手术类别例数	醉后手术取消
                                vo.SSLBLS_ZHSSQX = 1;

                            if (dicData.ContainsKey("F021") && dicData["F021"] == "1")
                                /// 输血例数	术中输血
                                vo.SXLS_SZSX = 1;
                            if (dicData.ContainsKey("F022") && dicData["F022"] == "1")
                                /// 输血例数 <400ml
                                vo.SXLS_X400 = 1;
                            if (dicData.ContainsKey("F023") && dicData["F023"] == "1")
                                /// 输血例数	>=400ml
                                vo.SXLS_D400 = 1;

                            if (dr["QGCG"] != DBNull.Value && dr["QGCG"].ToString() != string.Empty)
                                /// 气管插管管理例数	带管出手术室
                                vo.QGCGGLLS_DGCSSS = 1;
                            if (dr["QGBG"] != DBNull.Value && dr["QGBG"].ToString() != string.Empty)
                                /// 气管插管管理例数	拔管出手术室
                                vo.QGCGGLLS_BGCSSS = 1;

                            if (dicData.ContainsKey("F001") && dicData["F001"] == "1")
                                /// 麻醉复苏管理例数	进入复苏室
                                vo.MZFSGLLS_JRFSS = 1;
                            if (dicData.ContainsKey("F031") && dicData["F031"].Trim() != string.Empty)
                                /// 麻醉复苏管理例数	离室steward>=4分
                                vo.MZFSGLLS_LSSF = 1;
                            if (dicData.ContainsKey("F006") && dicData["F006"] == "1")
                                /// 心肺复苏例数	h内心跳骤停
                                vo.XFFSLS_HNXTZT = 1;
                            if (dicData.ContainsKey("F004") && dicData["F004"] == "1")
                                /// 心肺复苏例数	复苏成功
                                vo.XFFSLS_FSCG = 1;

                            if (dicData.ContainsKey("F701") && dicData["F701"] == "1")
                                /// 麻醉非预期相关事件例数	意识障碍
                                vo.MZFYQSJ_YSZA = 1;
                            if (dicData.ContainsKey("F702") && dicData["F702"] == "1")
                                /// 麻醉非预期相关事件例数	氧饱和度降低
                                vo.MZFYQSJ_YBHDJD = 1;
                            if (dicData.ContainsKey("F703") && dicData["F703"] == "1")
                                /// 麻醉非预期相关事件例数	使用催醒药
                                vo.MZFYQSJ_SYCXY = 1;
                            if (dicData.ContainsKey("F704") && dicData["F704"] == "1")
                                /// 麻醉非预期相关事件例数	低体温
                                vo.MZFYQSJ_DTW = 1;
                            if (dicData.ContainsKey("F705") && dicData["F705"] == "1")
                                /// 麻醉非预期相关事件例数	入PACU>3h
                                vo.MZFYQSJ_RPACU3H = 1;
                            if (dicData.ContainsKey("F706") && dicData["F706"] == "1")
                                /// 麻醉非预期相关事件例数	非计划入ICU
                                vo.MZFYQSJ_FJHRICU = 1;
                            if (dicData.ContainsKey("F707") && dicData["F707"] == "1")
                                /// 麻醉非预期相关事件例数	气管插管非计划二次
                                vo.MZFYQSJ_QGCGFJHEC = 1;
                            if (dicData.ContainsKey("F708") && dicData["F708"] == "1")
                                /// 麻醉非预期相关事件例数	过敏
                                vo.MZFYQSJ_GM = 1;
                            if (dicData.ContainsKey("F1015") && dicData["F1015"] == "1")
                                /// 麻醉非预期相关事件例数	误吸
                                vo.MZFYQSJ_WX = 1;
                            if (dicData.ContainsKey("F709") && dicData["F709"] == "1")
                                /// 麻醉非预期相关事件例数	昏迷
                                vo.MZFYQSJ_HM = 1;
                            if (dicData.ContainsKey("F710") && dicData["F710"] == "1")
                                /// 麻醉非预期相关事件例数	24h内死亡
                                vo.MZFYQSJ_24NSW = 1;
                            if (dicData.ContainsKey("F207") && dicData["F207"] == "1")
                                /// 麻醉非预期相关事件例数	神经损伤
                                vo.MZFYQSJ_SJSS = 1;
                            if (dicData.ContainsKey("F104") && dicData["F104"] == "1")
                                /// 麻醉非预期相关事件例数	声音嘶哑
                                vo.MZFYQSJ_SYSY = 1;

                            if ((dicData.ContainsKey("F501") && dicData["F501"] == "1") ||
                                (dicData.ContainsKey("F502") && dicData["F502"] == "1") ||
                                (dicData.ContainsKey("F503") && dicData["F503"] == "1") ||
                                (dicData.ContainsKey("F504") && dicData["F504"] == "1") ||
                                (dicData.ContainsKey("F505") && dicData["F505"] == "1") ||
                                (dicData.ContainsKey("F506") && dicData["F506"] == "1") ||
                                (dicData.ContainsKey("F507") && dicData["F507"] == "1") ||
                                (dicData.ContainsKey("F508") && dicData["F508"] == "1"))
                            {
                                vo.MZFYQSJ_ZXJMCC = 1;
                            }

                            if ((dicData.ContainsKey("F601") && dicData["F601"] == "1") ||
                                (dicData.ContainsKey("F602") && dicData["F602"] == "1") ||
                                (dicData.ContainsKey("F603") && dicData["F603"] == "1") ||
                                (dicData.ContainsKey("F604") && dicData["F604"] == "1") ||
                                (dicData.ContainsKey("F605") && dicData["F605"] == "1") ||
                                 dr["DMCC"] != DBNull.Value)
                            {
                                vo.MZFYQSJ_JDMCC = 1;
                            }

                            //if (dicData.ContainsKey("F007") && dicData["F007"] == "1")
                            //    /// 麻醉非预期相关事件例数	动静脉穿刺
                            //    vo.MZFYQSJ_JDMCC = 1;
                            if ((dicData.ContainsKey("F711") && dicData["F711"].Trim() != string.Empty) ||
                                (dicData.ContainsKey("F102") && dicData["F102"] == "1") ||
                                (dicData.ContainsKey("F103") && dicData["F103"] == "1" )||
                                (dicData.ContainsKey("F104") && dicData["F104"] == "1" )||
                                (dicData.ContainsKey("F105") && dicData["F105"] == "1" )||
                                (dicData.ContainsKey("F106") && dicData["F106"] == "1") ||
                                (dicData.ContainsKey("F107") && dicData["F107"] == "1") ||
                                (dicData.ContainsKey("F108") && dicData["F108"] == "1") ||
                                (dicData.ContainsKey("F109") && dicData["F109"] == "1") ||
                                (dicData.ContainsKey("F1010") && dicData["F1010"] == "1") ||
                                (dicData.ContainsKey("F1011") && dicData["F1011"] == "1") ||
                                (dicData.ContainsKey("F1012") && dicData["F1012"] == "1") ||
                                (dicData.ContainsKey("F1013") && dicData["F1013"] == "1") ||
                                (dicData.ContainsKey("F1014") && dicData["F1014"] == "1") ||
                                (dicData.ContainsKey("F1016") && dicData["F1016"] == "1") ||
                                (dicData.ContainsKey("F1017") && dicData["F1017"] == "1") ||
                                (dicData.ContainsKey("F1018") && dicData["F1018"] == "1") ||
                                (dicData.ContainsKey("F1019") && dicData["F1019"] == "1") ||
                                (dicData.ContainsKey("F1020") && dicData["F1020"] == "1") ||
                                (dicData.ContainsKey("F1021") && dicData["F1021"] == "1") ||
                                (dicData.ContainsKey("F1022") && dicData["F1022"] == "1") ||
                                (dicData.ContainsKey("F1023") && dicData["F1023"] == "1") ||
                                (dicData.ContainsKey("F1024") && dicData["F1024"] == "1") ||
                                (dicData.ContainsKey("F1025") && dicData["F1025"] == "1") ||
                                (dicData.ContainsKey("F1026") && dicData["F1026"] == "1") ||
                                (dicData.ContainsKey("F1027") && dicData["F1027"] == "1") ||
                                (dicData.ContainsKey("F1028") && dicData["F1028"] == "1") ||
                                (dicData.ContainsKey("F1029") && dicData["F1029"] == "1") ||
                                (dicData.ContainsKey("F1030") && dicData["F1030"] == "1") ||
                                (dicData.ContainsKey("F1030") && dicData["F1030"] == "1") ||
                                (dicData.ContainsKey("F1031") && dicData["F1031"] == "1") ||
                                (dicData.ContainsKey("F1032") && dicData["F1032"] == "1") ||
                                (dicData.ContainsKey("F1033") && dicData["F1033"] == "1") ||
                                (dicData.ContainsKey("F1034") && dicData["F1034"] == "1") ||
                                (dicData.ContainsKey("F1035") && dicData["F1035"] == "1") ||
                                (dicData.ContainsKey("F1036") && dicData["F1036"] == "1") ||
                                (dicData.ContainsKey("F201") && dicData["F201"] == "1") ||
                                (dicData.ContainsKey("F202") && dicData["F202"] == "1") ||
                                (dicData.ContainsKey("F203") && dicData["F203"] == "1") ||
                                (dicData.ContainsKey("F204") && dicData["F204"] == "1") ||
                                (dicData.ContainsKey("F205") && dicData["F205"] == "1") ||
                                (dicData.ContainsKey("F206") && dicData["F206"] == "1") ||
                                (dicData.ContainsKey("F207") && dicData["F207"] == "1") ||
                                (dicData.ContainsKey("F208") && dicData["F208"] == "1") ||
                                (dicData.ContainsKey("F209") && dicData["F209"] == "1") ||
                                (dicData.ContainsKey("F2010") && dicData["F2010"] == "1") ||
                                (dicData.ContainsKey("F2011") && dicData["F2011"] == "1") ||
                                (dicData.ContainsKey("F2012") && dicData["F2012"] == "1") ||
                                (dicData.ContainsKey("F2013") && dicData["F2013"] == "1") ||
                                (dicData.ContainsKey("F2014") && dicData["F2014"] == "1") ||
                                (dicData.ContainsKey("F2015") && dicData["F2015"] == "1") ||
                                (dicData.ContainsKey("F2016") && dicData["F2016"] == "1") ||
                                (dicData.ContainsKey("F2017") && dicData["F2017"] == "1") ||
                                (dicData.ContainsKey("F2018") && dicData["F2018"] == "1") ||
                                (dicData.ContainsKey("F407") && dicData["F407"] == "1") ||
                                (dicData.ContainsKey("F408") && dicData["F408"] == "1") ||
                                (dicData.ContainsKey("F409") && dicData["F409"] == "1") ||
                                (dicData.ContainsKey("F4010") && dicData["F4010"] == "1"))
                                /// 麻醉非预期相关事件例数	其他
                                vo.MZFYQSJ_QT = 1;

                            /* 暂时屏蔽
                            /// 麻醉效果管理例数	I
                            vo.MZXGGLLS_I = 1;
                            /// 麻醉效果管理例数	II
                            vo.MZXGGLLS_II = 1;
                            /// 麻醉效果管理例数	III
                            vo.MZXGGLLS_III = 1;
                            /// 麻醉效果管理例数	IV
                            vo.MZXGGLLS_IV = 1;
                            */

                            if (dr["MZXG"] != DBNull.Value)
                            {
                                MZXG = dr["MZXG"].ToString().Trim();
                                if (MZXG == "优")
                                    /// 麻醉效果管理例数	I
                                    vo.MZXGGLLS_I = 1;
                                else if (MZXG == "良")
                                    /// 麻醉效果管理例数	II
                                    vo.MZXGGLLS_II = 1;
                                else if (MZXG == "中")
                                    /// 麻醉效果管理例数	III
                                    vo.MZXGGLLS_III = 1;
                                else if (MZXG == "差")
                                    /// 麻醉效果管理例数	IV
                                    vo.MZXGGLLS_IV = 1;
                            }

                            if (dicData.ContainsKey("F003") && dicData["F003"] == "1")
                                /// 麻醉效果管理例数	麻醉方式更改
                                vo.MZXGGLLS_MZFSGG = 1;

                            if (dr["MZFJ"] != DBNull.Value)
                            {
                                ANALEVEL = dr["MZFJ"].ToString().Trim();
                                if (ANALEVEL == "I" || ANALEVEL.StartsWith("I("))
                                    /// 麻醉分级管理例数ASA	I
                                    vo.MZFJGLLSASA_I = 1;
                                else if (ANALEVEL == "II" || ANALEVEL.StartsWith("II("))
                                    /// 麻醉分级管理例数ASA	II
                                    vo.MZFJGLLSASA_II = 1;
                                else if (ANALEVEL == "III" || ANALEVEL.StartsWith("III("))
                                    /// 麻醉分级管理例数ASA	III
                                    vo.MZFJGLLSASA_III = 1;
                                else if (ANALEVEL == "IV" || ANALEVEL.StartsWith("IV("))
                                    /// 麻醉分级管理例数ASA	IV
                                    vo.MZFJGLLSASA_IV = 1;
                                else if (ANALEVEL == "V" || ANALEVEL.StartsWith("V("))
                                    /// 麻醉分级管理例数ASA	V
                                    vo.MZFJGLLSASA_V = 1;
                            }

                            if (dicData.ContainsKey("F801") && dicData["F801"].Trim() != string.Empty)
                            {
                                /// 在岗医师	主任医师
                                vo.ZGYS_ZRYS = dicData["F801"].Trim().Split(',').Length;
                            }
                            if (dicData.ContainsKey("F802") && dicData["F802"].Trim() != string.Empty)
                            {
                                /// 在岗医师	副主任医师
                                vo.ZGYS_FZRYS = dicData["F802"].Trim().Split(',').Length;
                            }
                            if (dicData.ContainsKey("F803") && dicData["F803"].Trim() != string.Empty)
                            {
                                /// 在岗医师	主治医师
                                vo.ZGYS_ZZYS = dicData["F803"].Trim().Split(',').Length;
                            }
                            if (dicData.ContainsKey("F804") && dicData["F804"].Trim() != string.Empty)
                            {
                                /// 在岗医师	住院医师
                                vo.ZGYS_ZYYS = dicData["F804"].Trim().Split(',').Length;
                            }

                            if (dr["JRICU"] != DBNull.Value && dr["JRICU"].ToString() != string.Empty)
                                /// 进入ICU 合计
                                vo.JRICU_HJ = 1;

                            foreach (DataRow dr1 in dt1.Rows)
                            {
                                month1 = Function.Datetime(dr1["Fmonth"].ToString()).ToString("yyyy-MM");
                                if (month1 == month)
                                {
                                    //分娩镇痛
                                    vo.SSSWMZ_FMZT = Function.Int(dr1["Field1"].ToString());
                                    //无痛胃肠镜
                                    vo.SSSWMZ_WTWCJ = Function.Int(dr1["Field2"].ToString());
                                    //无痛人流
                                    vo.SSSWMZ_WTRL = Function.Int(dr1["Field3"].ToString());
                                    //其他
                                    vo.SSSWMZ_QT = Function.Int(dr1["Field4"].ToString());
                                }
                            }

                            #endregion

                            data.Add(vo);
                        }
                    }
                }
                #endregion

                #region 合计

                foreach (EntityAnaStat1 item in data)
                {
                    item.MZZLS_ZGNMZ += item.SSSWMZ_FMZT;
                    item.SSLBLS_JZ += item.SSSWMZ_FMZT;
                    item.MZXGGLLS_II += item.SSSWMZ_FMZT;
                    item.MZFJGLLSASA_I += item.SSSWMZ_FMZT;
                    //带管出手术室 =插管全麻总数 - 拔管出手术室
                    item.QGCGGLLS_DGCSSS = item.MZZLS_CGMZ - item.QGCGGLLS_BGCSSS;

                    if (item.QGCGGLLS_DGCSSS < 0)
                    {
                        item.QGCGGLLS_DGCSSS = 0;
                        item.MZZLS_CGMZ = item.QGCGGLLS_BGCSSS;
                    }
                    
                    item.MZZLS_HJ = item.MZZLS_CGMZ + item.MZZLS_FCGMZ + item.MZZLS_FHMZ + item.MZZLS_QT + item.MZZLS_ZGNMZ;
                    item.SHZTLS_HJ = item.SHZTLS_JMZT + item.SHZTLS_QT + item.SHZTLS_SJZZ + item.SHZTLS_YMWZT;
                    item.SSSWMZ_HJ = item.SSSWMZ_FMZT + item.SSSWMZ_QT + item.SSSWMZ_WTRL + item.SSSWMZ_WTWCJ;
                    item.SXLS_ZTSX = item.SXLS_D400 + item.SXLS_X400;// +item.SXLS_SZSX;
                    item.MZFYQSJ_HJ = item.MZFYQSJ_24NSW + item.MZFYQSJ_DTW + item.MZFYQSJ_FJHRICU + item.MZFYQSJ_GM + item.MZFYQSJ_HM +
                                      item.MZFYQSJ_ZXJMCC + item.MZFYQSJ_JDMCC + item.MZFYQSJ_QGCGFJHEC + item.MZFYQSJ_QT + item.MZFYQSJ_RPACU3H + item.MZFYQSJ_SJSS +
                                      item.MZFYQSJ_SYCXY + item.MZFYQSJ_SYSY + item.MZFYQSJ_WX + item.MZFYQSJ_YBHDJD + item.MZFYQSJ_YSZA;
                    item.MZFJGLLSASA_HJ = item.MZFJGLLSASA_I + item.MZFJGLLSASA_II + item.MZFJGLLSASA_III + item.MZFJGLLSASA_IV + item.MZFJGLLSASA_V;
                    item.ZGYS_HJ = item.ZGYS_FZRYS + item.ZGYS_ZRYS + item.ZGYS_ZYYS + item.ZGYS_ZZYS;
                    item.SSLBLS_ZQ = item.MZZLS_HJ - item.SSLBLS_JZ;
                }

                //汇总
                if(data != null)            
                {
                    EntityAnaStat1 voSum = new EntityAnaStat1();
                    voSum.YF = "汇总";
                    foreach (EntityAnaStat1 item in data)
                    {
                        voSum.MZZLS_CGMZ += item.MZZLS_CGMZ;
                        voSum.MZZLS_FCGMZ += item.MZZLS_FCGMZ;
                        voSum.MZZLS_FHMZ += item.MZZLS_FHMZ;
                        voSum.MZZLS_QT += item.MZZLS_QT;
                        voSum.MZZLS_ZGNMZ += item.MZZLS_ZGNMZ;
                        voSum.MZZLS_HJ += item.MZZLS_HJ;
                        voSum.SHZTLS_JMZT += item.SHZTLS_JMZT;
                        voSum.SHZTLS_QT += item.SHZTLS_QT;
                        voSum.SHZTLS_SJZZ += item.SHZTLS_SJZZ;
                        voSum.SHZTLS_YMWZT += item.SHZTLS_YMWZT;
                        voSum.SHZTLS_HJ += item.SHZTLS_HJ;
                        voSum.SSSWMZ_FMZT += item.SSSWMZ_FMZT;
                        voSum.SSSWMZ_QT += item.SSSWMZ_QT;
                        voSum.SSSWMZ_WTRL += item.SSSWMZ_WTRL;
                        voSum.SSSWMZ_WTWCJ += item.SSSWMZ_WTWCJ;
                        voSum.SSSWMZ_HJ += item.SSSWMZ_HJ;
                        voSum.XGCCLS_DM += item.XGCCLS_DM;
                        voSum.XGCCLS_ZXJM += item.XGCCLS_ZXJM;
                        voSum.SXLS_D400 += item.SXLS_D400;
                        voSum.SXLS_X400 += item.SXLS_X400;
                        voSum.SXLS_ZTSX += item.SXLS_ZTSX;
                        voSum.SXLS_SZSX += item.SXLS_SZSX;
                        voSum.SSLBLS_JZ += item.SSLBLS_JZ;
                        voSum.SSLBLS_ZHSSQX += item.SSLBLS_ZHSSQX;
                        voSum.SSLBLS_ZQ += item.SSLBLS_ZQ;
                        voSum.QGCGGLLS_BGCSSS += item.QGCGGLLS_BGCSSS;
                        voSum.QGCGGLLS_DGCSSS += item.QGCGGLLS_DGCSSS;
                        voSum.MZFSGLLS_JRFSS += item.MZFSGLLS_JRFSS;
                        voSum.MZFSGLLS_LSSF += item.MZFSGLLS_LSSF;
                        voSum.XFFSLS_FSCG += item.XFFSLS_FSCG;
                        voSum.XFFSLS_HNXTZT += item.XFFSLS_HNXTZT;
                        voSum.MZFYQSJ_24NSW += item.MZFYQSJ_24NSW;
                        voSum.MZFYQSJ_DTW += item.MZFYQSJ_DTW;
                        voSum.MZFYQSJ_FJHRICU += item.MZFYQSJ_FJHRICU;
                        voSum.MZFYQSJ_GM += item.MZFYQSJ_GM;
                        voSum.MZFYQSJ_HM += item.MZFYQSJ_HM;
                        voSum.MZFYQSJ_ZXJMCC += item.MZFYQSJ_ZXJMCC;
                        voSum.MZFYQSJ_JDMCC += item.MZFYQSJ_JDMCC;
                        voSum.MZFYQSJ_QGCGFJHEC += item.MZFYQSJ_QGCGFJHEC;
                        voSum.MZFYQSJ_QT += item.MZFYQSJ_QT;
                        voSum.MZFYQSJ_RPACU3H += item.MZFYQSJ_RPACU3H;
                        voSum.MZFYQSJ_SJSS += item.MZFYQSJ_SJSS;
                        voSum.MZFYQSJ_SYCXY += item.MZFYQSJ_SYCXY;
                        voSum.MZFYQSJ_SYSY += item.MZFYQSJ_SYSY;
                        voSum.MZFYQSJ_WX += item.MZFYQSJ_WX;
                        voSum.MZFYQSJ_YBHDJD += item.MZFYQSJ_YBHDJD;
                        voSum.MZFYQSJ_YSZA += item.MZFYQSJ_YSZA;
                        voSum.MZFYQSJ_HJ += item.MZFYQSJ_HJ;
                        voSum.MZFJGLLSASA_I += item.MZFJGLLSASA_I;
                        voSum.MZFJGLLSASA_II += item.MZFJGLLSASA_II;
                        voSum.MZFJGLLSASA_III += item.MZFJGLLSASA_III;
                        voSum.MZFJGLLSASA_IV += item.MZFJGLLSASA_IV;
                        voSum.MZFJGLLSASA_V += item.MZFJGLLSASA_V;
                        voSum.MZFJGLLSASA_HJ += item.MZFJGLLSASA_HJ;
                        voSum.MZXGGLLS_I += item.MZXGGLLS_I;
                        voSum.MZXGGLLS_II += item.MZXGGLLS_II;
                        voSum.MZXGGLLS_III += item.MZXGGLLS_III;
                        voSum.MZXGGLLS_IV += item.MZXGGLLS_IV;
                        voSum.MZXGGLLS_MZFSGG += item.MZXGGLLS_MZFSGG;
                        //voSum.ZGYS_FZRYS += item.ZGYS_FZRYS;
                        //voSum.ZGYS_ZRYS += item.ZGYS_ZRYS;
                        //voSum.ZGYS_ZYYS += item.ZGYS_ZYYS;
                        //voSum.ZGYS_ZZYS += item.ZGYS_ZZYS;
                        //voSum.ZGYS_HJ += item.ZGYS_HJ;
                    }
                    data.Add(voSum);
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

        #region GetAnaStat2
        /// <summary>
        /// GetAnaStat2
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityAnaStat2> GetAnaStat2(string beginDate, string endDate)
        {
            List<EntityAnaStat2> data = new List<EntityAnaStat2>();
            List<EntityAnaStat1> lstAna = this.GetAnaStat1(beginDate, endDate);
            EntityAnaStat2 vo = null;
            foreach (EntityAnaStat1 item in lstAna)
            {
                vo = new EntityAnaStat2();
                /// 月份
                vo.YF = item.YF;
                if (item.MZZLS_HJ > 0)
                {
                    if (item.ZGYS_HJ > 0)
                        /// 麻醉科医患比(麻醉科固定在岗(本院)医师总数比麻醉总数)
                        vo.MZKYHB = Function.Round(((double)item.ZGYS_HJ / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.MZFJGLLSASA_I > 0)
                        /// 各ASA分级麻醉患者比例(ASA分级麻醉患者数比ASA分级麻醉患者总数)	I
                        vo.ASAFJMZHZBL_I = Function.Round(((double)item.MZFJGLLSASA_I / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.MZFJGLLSASA_II > 0)
                        /// 各ASA分级麻醉患者比例(ASA分级麻醉患者数比ASA分级麻醉患者总数)	II
                        vo.ASAFJMZHZBL_II = Function.Round(((double)item.MZFJGLLSASA_II / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.MZFJGLLSASA_III > 0)
                        /// 各ASA分级麻醉患者比例(ASA分级麻醉患者数比ASA分级麻醉患者总数)	III
                        vo.ASAFJMZHZBL_III = Function.Round(((double)item.MZFJGLLSASA_III / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.MZFJGLLSASA_IV > 0)
                        /// 各ASA分级麻醉患者比例(ASA分级麻醉患者数比ASA分级麻醉患者总数)	IV
                        vo.ASAFJMZHZBL_IV = Function.Round(((double)item.MZFJGLLSASA_IV / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.MZFJGLLSASA_V > 0)
                        /// 各ASA分级麻醉患者比例(ASA分级麻醉患者数比ASA分级麻醉患者总数)	V
                        vo.ASAFJMZHZBL_V = Function.Round(((double)item.MZFJGLLSASA_V / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.SSLBLS_JZ > 0)
                        /// 急诊非择期麻醉比例(急诊非择期手术所实施的麻醉数比麻醉总数)
                        vo.JZFZQMZBL = Function.Round(((double)item.SSLBLS_JZ / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.MZZLS_ZGNMZ > 0)
                        /// 各类麻醉方式比例(麻醉方式数比麻醉总数)	椎管内麻醉
                        vo.MZFSBL_ZGNMZ = Function.Round(((double)item.MZZLS_ZGNMZ / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.MZZLS_CGMZ > 0)
                        /// 各类麻醉方式比例(麻醉方式数比麻醉总数)	插管全麻
                        vo.MZFSBL_CGMZ = Function.Round(((double)item.MZZLS_CGMZ / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.MZZLS_FCGMZ > 0)
                        /// 各类麻醉方式比例(麻醉方式数比麻醉总数)	非插管全麻
                        vo.MZFSBL_FCGMZ = Function.Round(((double)item.MZZLS_FCGMZ / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.MZZLS_FHMZ > 0)
                        /// 各类麻醉方式比例(麻醉方式数比麻醉总数)	复合麻醉
                        vo.MZFSBL_FHMZ = Function.Round(((double)item.MZZLS_FHMZ / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.MZZLS_QT > 0)
                        /// 各类麻醉方式比例(麻醉方式数比麻醉总数)	其他
                        vo.MZFSBL_QT = Function.Round(((double)item.MZZLS_QT / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.SSLBLS_ZHSSQX > 0)
                        /// 麻醉开始后手术取消率(手术取消的数比麻醉总数)
                        vo.MZKSHSSQXL = Function.Round(((double)item.SSLBLS_ZHSSQX / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                }
                if (item.MZFSGLLS_JRFSS > 0)
                {
                    if (item.MZFYQSJ_RPACU3H > 0)
                        /// 麻醉后监测治疗室(PACU)转出延迟率(入PACU超过3小时的患者数比入PACU患者总数)
                        vo.MZHJKZLSZCYCL = Function.Round(((double)item.MZFYQSJ_RPACU3H / (double)item.MZFSGLLS_JRFSS) * 100, 1).ToString() + "%";
                    if (item.MZFYQSJ_DTW > 0)
                        /// PACU入室低体温率(PACU入室低体温患者数比入PACU患者总数)
                        vo.PACURSDTWL = Function.Round(((double)item.MZFYQSJ_DTW / (double)item.MZFSGLLS_JRFSS) * 100, 1).ToString() + "%";
                }
                if (item.JRICU_HJ > 0 && item.MZFYQSJ_FJHRICU > 0)
                {
                    /// 非计划转入ICU率(非计划转入ICU患者数比转入ICU患者总数)
                    vo.FJHZRICUL = Function.Round(((double)item.MZFYQSJ_FJHRICU / (double)item.JRICU_HJ) * 100, 1).ToString() + "%";
                }
                if (item.QGCGGLLS_BGCSSS > 0 && item.MZFYQSJ_QGCGFJHEC > 0)
                {
                    /// 非计划二次气管插管率(非计划计划二次气管插管患者数比术后气管插管拔除患者总数) 非计划二次气管插拔/拔管出手术室
                    //vo.FJHECQGCGL = Function.Round(((double)item.QGCGGLLS_DGCSSS / (double)(item.QGCGGLLS_BGCSSS + item.QGCGGLLS_DGCSSS)) * 100, 1).ToString() + "%";
                    vo.FJHECQGCGL = Function.Round(((double)item.MZFYQSJ_QGCGFJHEC / (double)(item.QGCGGLLS_BGCSSS)) * 100, 1).ToString() + "%";
                }
                if (item.MZZLS_HJ > 0)
                {
                    if (item.MZFYQSJ_24NSW > 0)
                        /// 麻醉开始后24小时内死亡率(麻醉开始后24小时内死亡患者数比麻醉患者总数)
                        vo.MZKSH24SWL = Function.Round(((double)item.MZFYQSJ_24NSW / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                    if (item.XFFSLS_HNXTZT > 0)
                        /// 麻醉开始后24小时内心跳骤停率(麻醉开始后24小时内心跳骤停患者数比麻醉患者总数)
                        vo.MZKSH24XTZTL = Function.Round(((double)item.XFFSLS_HNXTZT / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                }
                // ? 算法待确定
                if (item.SXLS_ZTSX > 0 && item.SXLS_D400 > 0)
                {
                    /// 术中自体血输注率(接受400ml及以上自体血(包括自体全血及自体血红细胞)输注患者数比接受400ml及以上输血治疗的患者总数
                    //vo.SZZTSXZL = Function.Round(((double)item.SXLS_D400 / (double)item.SXLS_HJ) * 100, 1).ToString() + "%";
                    vo.SZZTSXZL = Function.Round(((double)item.SXLS_D400 / (double)item.SXLS_ZTSX) * 100, 1).ToString() + "%";
                }
                if (item.MZZLS_HJ > 0 && item.MZFYQSJ_GM > 0)
                {
                    /// 麻醉期间严重过敏反应发生率(麻醉期间严重过敏反应发生例数比麻醉总例数)
                    vo.MZQJYZGMFYFSL = Function.Round(((double)item.MZFYQSJ_GM / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                }
                // ? 算法待确定
                if (item.MZZLS_ZGNMZ > 0 && item.MZFYQSJ_SJSS > 0)
                {
                    /// 椎管内麻醉后严重神经并发症发生率(椎管内麻醉后严重神经并发症发生例数比椎管内麻醉总例数)
                    vo.ZGNMZHYZSJBFZFSL = Function.Round(((double)item.MZFYQSJ_SJSS / (double)item.MZZLS_ZGNMZ) * 100, 1).ToString() + "%";
                }

                /* ? 算法待确定
                /// 中心静脉穿刺严重并发症发生率(中心静脉穿刺严重并发症发生例数比中心静脉穿刺总例数)
                vo.ZXJMCCYZBFZFSL = ;
                 * */
                if (item.SSSWMZ_HJ > 0 && item.MZFYQSJ_ZXJMCC > 0)
                {
                    //中心静脉穿刺严重并发症发生率(中心静脉穿刺严重并发症发生例数比中心静脉穿刺总例数)
                    vo.ZXJMCCYZBFZFSL = Function.Round(((double)item.MZFYQSJ_ZXJMCC / (double)item.XGCCLS_ZXJM) * 100, 1).ToString() + "%";
                }

                if (item.MZZLS_CGMZ > 0 && item.MZFYQSJ_SYSY > 0)
                {
                    /// 全麻气管插管拔管后声音嘶哑发生率(全麻气管插管拔管后声音嘶哑发生例数比全麻气管插管总例数)
                    vo.QMQGCGBGHSYSYFSL = Function.Round(((double)item.MZFYQSJ_SYSY / (double)item.MZZLS_CGMZ) * 100, 1).ToString() + "%";
                }
                if (item.MZZLS_HJ > 0 && item.MZFYQSJ_HM > 0)
                {
                    /// 麻醉后新发昏迷发生率(麻醉后新发昏迷发生例数比麻醉总例数)	
                    vo.MZHXFHMFSL = Function.Round(((double)item.MZFYQSJ_HM / (double)item.MZZLS_HJ) * 100, 1).ToString() + "%";
                }

                data.Add(vo);
            }
            return data;
        }
        #endregion

        #region GetRegister1Xml
        /// <summary>
        /// GetRegister1Xml
        /// </summary>
        /// <param name="anaId"></param>
        /// <returns></returns>
        public string GetRegister1Xml(decimal anaId)
        {
            string xmlData = string.Empty;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                Sql = @"select statxml1 from t_ana_requisition where anaid_int = ?";
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = svc.CreateParm(1);
                parm[0].Value = anaId;
                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                    xmlData = dt.Rows[0]["statxml1"].ToString();
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

        #region Register1Edit
        /// <summary>
        /// Register1Edit
        /// </summary>
        /// <param name="anaId"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public int Register1Edit(decimal anaId, string xmlData)
        {
            int affectRows = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                Sql = @"update t_ana_requisition set statxml1 = ? where anaid_int = ?";
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = svc.CreateParm(2);
                parm[0].Value = xmlData;
                parm[1].Value = anaId;
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

        #region GetRegister2Xml
        /// <summary>
        /// GetRegister2Xml
        /// </summary>
        /// <param name="anaId"></param>
        /// <returns></returns>
        public string GetRegister2Xml(decimal anaId)
        {
            string xmlData = string.Empty;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                Sql = @"select statxml2 from t_ana_requisition where anaid_int = ?";
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = svc.CreateParm(1);
                parm[0].Value = anaId;
                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                    xmlData = dt.Rows[0]["statxml2"].ToString();
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

        #region Register2Edit
        /// <summary>
        /// Register2Edit
        /// </summary>
        /// <param name="anaId"></param>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public int Register2Edit(decimal anaId, string xmlData)
        {
            int affectRows = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                Sql = @"update t_ana_requisition set statxml2 = ? where anaid_int = ?";
                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = svc.CreateParm(2);
                parm[0].Value = xmlData;
                parm[1].Value = anaId;
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

        #region GetAnaStatTemp
        /// <summary>
        /// GetAnaStatTemp
        /// </summary>
        /// <returns></returns>
        public List<EntityAnaStatTemp> GetAnaStatTemp()
        {
            string Sql = string.Empty;
            List<EntityAnaStatTemp> data = new List<EntityAnaStatTemp>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select fmonth, field1, field2, field3, field4
                          from t_ana_stattemp
                         order by fmonth ";
                DataTable dt = svc.GetDataTable(Sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        data.Add(new EntityAnaStatTemp() { Fmonth = dr["fmonth"].ToString(), Field1 = Function.Int(dr["field1"]), Field2 = Function.Int(dr["field2"]), Field3 = Function.Int(dr["field3"]), Field4 = Function.Int(dr["field4"]) });
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

        #region SaveAnaStatTemp
        /// <summary>
        /// SaveAnaStatTemp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int SaveAnaStatTemp(List<EntityAnaStatTemp> data)
        {
            int affectRows = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                List<DacParm> lstParm = new List<DacParm>();
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = "delete from t_ana_stattemp";
                lstParm.Add(svc.GetDacParm(EnumExecType.ExecSql, Sql));
                if (data != null && data.Count > 0)
                {
                    int n = 0;
                    IDataParameter[] parm = null;
                    Sql = @"insert into t_ana_stattemp values (?, ?, ?, ?, ?)";
                    foreach (EntityAnaStatTemp item in data)
                    {
                        n = -1;
                        parm = svc.CreateParm(5);
                        parm[++n].Value = item.Fmonth;
                        parm[++n].Value = item.Field1;
                        parm[++n].Value = item.Field2;
                        parm[++n].Value = item.Field3;
                        parm[++n].Value = item.Field4;
                        lstParm.Add(svc.GetDacParm(EnumExecType.ExecSql, Sql, parm));
                    }
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

        #region 术前排班表
        /// <summary>
        /// 术前排班表
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityOperationReport> GetOperationRegister(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            List<EntityOperationReport> data = new List<EntityOperationReport>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                List<IDataParameter> lstParm = new List<IDataParameter>();
                IDataParameter[] parm = null;
                DataTable dt1 = null;
                DataTable dt2 = null;
                int xh = 0;
                #region Sql1
                Sql = @"select t1.patientid_chr,
                               t1.patientcardid_chr,
                               t1.inpatientdate_dat,
                               t1.opendate_dat  AS TZSJ,
                               t1.operationdate_dat  as RQSJ,
                               t4.roomname_chr as FJH,
                               t11.deptname_vchr as BQ,
                               t1.bedno_vchr  as CH,
	                           t1.inpatientid_chr  as ZYH,
                               t1.patientname_vchr AS XM,
                               t1.sex_vchr AS XB,
                               t1.age_vchr AS NL,
                               t1.preoperativediagnosis_chr AS ZYZD,
                               t1.operationname_chr AS SSMC,
	                           t1.operationpart_chr AS SSBW,
                               t1.emergency_chr AS SSLX,
	                           t1.asalevel_chr AS SSFJ,
	                           '' AS ZDYS,
	                           '' AS ZC,
	                           '' AS ZS,
	                           t1.remark_chr AS BZ,
                               t1.diseasename_chr,
                               t1.isemergency_int,
                               t1.specialcase_chr,
                               t1.weight_chr,
                               t1.operationroomid_chr,
                               t1.isisolated_int,
                               t1.sequence_chr,
                               t1.iscontinuedoperation_int,
                               t1.isaxenic_int,
                               t1.visitor_chr,
                               t1.createuserid_chr,
                               t1.issignedicf,
                               t1.opendate_dat,
                               t1.registerid_chr,
                               t1.deptid_chr as appdeptid,
                               t1.signsequence_int,
                               t1.anaid_int,
                               t1.status_int,
                               t1.anamode_chr,
                               t1.iscomfim_int,
                               t1.continuedoperation_vchr,
                               t1.anadeptid_chr,
                               t1.from_int,
                               t1.ischecked_int,
                               t1.frozenChecked_chr,
                               t1.incisionLev_chr,
                               '' deptid_chr,
                               '' areaid_chr,
                               t3.lastname_vchr creator_name,
                               t1.sex_vchr,
                               t1.age_vchr,
                               '' dept_name,
                               '' area_name,
                               t1.bedno_vchr bed_name,
                               t1.approval_date,
                               t10.start_dat,
                               t10.complete_dat,
                               t10.status_int opstatus,
                               t10.recordid_int,
                               t1.isprint_int,
                               t1.print_mac_address,
                               t12.anadeptname_vchr
                          from t_ana_requisition   t1,
                               t_bse_employee      t3,
                               t_ana_operatingroom t4,
                               t_ana_operation     t10,
                               t_bse_deptdesc      t11,
                               t_aid_ana_anadept   t12
                         where t1.createuserid_chr = t3.empid_chr(+)
                           and t1.operationroomid_chr = t4.roomid_chr(+)
                           and t1.anaid_int = t10.aneid_int(+)
                           and t1.deptid_chr = t11.deptid_chr(+)
                           and t1.status_int >= -1
                           and t1.anadeptid_chr = t12.anadeptid_chr(+)
                           and (t10.status_int <> -1 or t10.status_int is null)
                           and t1.operationdate_dat between to_date(?,'yyyy-mm-dd hh24:mi:ss') and to_date(?,'yyyy-mm-dd hh24:mi:ss')
                           and t1.status_int>=1 and t1.from_int = 1 ";

                string strSub = string.Empty;

                foreach (EntityParm po in dicParm)
                {
                    string keyValue = po.value;

                    switch (po.key)
                    {
                        case "operateDate":
                           IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0] + " 00:00:00";
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            break;
                        case "sslx":
                            strSub += " and trim(t1.emergency_chr) = '" + keyValue + "'";
                            break;
                        default:
                            break;
                    }
                }
                strSub += " order by t1.operationdate_dat ";
                Sql += strSub;

                dt1 = svc.GetDataTable(Sql, lstParm);
                #endregion

                #region Sql2

                Sql = @" select t.sequenceid_int,
                               t.tag_chr,
                               t.additionalinfo_chr,
                               t.signdate_dat,
                               t.employeeid_chr,
                               t.employeename_chr,
                               a.technicalrank_chr
                          from t_ana_sign t
                          left join t_bse_employee a 
                          on t.employeeid_chr = a.empid_chr
                         where t.sequenceid_int = ? ";

                #endregion

                #region 赋值
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    EntityOperationReport vo = null;
                    foreach (DataRow dr in dt1.Rows)
                    {
                        #region vo
                        vo = new EntityOperationReport();
                        vo.XH = ++xh;
                        vo.sequenceid = Function.Int(dr["signsequence_int"]);
                        vo.RQSJ = Function.Datetime(dr["RQSJ"]).ToString("yyyy-MM-dd HH:mm");
                        vo.TZSJ = Function.Datetime(dr["TZSJ"]).ToString("yyyy-MM-dd HH:mm");
                        vo.FJH = dr["FJH"].ToString();
                        vo.BQ = dr["BQ"].ToString().Trim();
                        vo.CH = dr["CH"].ToString();
                        vo.ZYH = dr["ZYH"].ToString();
                        vo.XM = dr["XM"].ToString();
                        vo.XB = dr["XB"].ToString();
                        vo.NL = dr["NL"].ToString();
                        vo.ZYZD = dr["ZYZD"].ToString();
                        vo.SSMC = dr["SSMC"].ToString();
                        vo.SSBW = dr["SSBW"].ToString();
                        vo.SSLX = dr["SSLX"].ToString();
                        vo.SSFJ = dr["SSFJ"].ToString();
                        vo.ZDYS = dr["ZDYS"].ToString();
                        vo.ZC = dr["ZC"].ToString();
                        vo.ZS = dr["ZS"].ToString();
                        vo.BZ = dr["BZ"].ToString();
                        parm = svc.CreateParm(1);
                        parm[0].Value = vo.sequenceid;
                        dt2 = svc.GetDataTable(Sql, parm);

                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            foreach(DataRow dr2 in dt2.Rows)
                            {
                                if (dr2["tag_chr"].ToString().Trim() == "主刀医师")
                                {
                                    vo.ZDYS = dr2["employeename_chr"].ToString();
                                    vo.ZC = dr2["technicalrank_chr"].ToString().Trim();
                                }
                                if (dr2["tag_chr"].ToString().Trim() == "一助")
                                    vo.ZS += dr2["employeename_chr"].ToString() + "、";

                                if (dr2["tag_chr"].ToString().Trim() == "二助")
                                    vo.ZS += dr2["employeename_chr"].ToString() + "、";
                            }
                        }
                        vo.ZS = vo.ZS.TrimEnd('、');

                        data.Add(vo);
                        #endregion
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

        #region 院感报表

        #region 入院信息
        /// <summary>
        /// GetAccessRecord 
        /// </summary>
        /// <returns></returns>
        public List<EntityYgAccessRecord> GetAccessRecord(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string Sql2 = string.Empty;
            IDataParameter[] parm = null;
            DataRow[] drr = null;
            int n = 0;
            List<EntityYgAccessRecord> data = new List<EntityYgAccessRecord>();
            List<EntityYgAccessRecord> lstVo = new List<EntityYgAccessRecord>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select distinct a.registerid_chr,
                               b.lastname_vchr,
                               e.deptid_chr as ryks,
                               to_char(a.inpatient_dat, 'yyyy-mm-dd hh24:mi:ss') as rysj,
                               to_char(c.outhospital_dat, 'yyyy-mm-dd hh24:mi:ss') as cysj
                          from t_opr_bih_register a
                          left join t_opr_bih_registerdetail b
                          on a.registerid_chr = b.registerid_chr
                          left join t_opr_bih_leave c
                          on a.registerid_chr = c.registerid_chr  and c.status_int = 1
                          left join t_bse_deptdesc e 
                          on a.areaid_chr = e.deptid_chr   
                           left join t_opr_bih_order ord
                            on a.registerid_chr = ord.registerid_chr 
                         where  a.status_int = 1
                         and ord.status_int >= 2
                         and (a.inpatient_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss') )
                         order by a.registerid_chr ";

                Sql2 = @"select distinct trf.TRANSFERID_CHR,reg.registerid_chr,
                            pat.lastname_vchr as lastname,
                            pat.sex_chr as sex,
                            pat.birth_dat,
                            '' as age,
                            reg.inpatientid_chr as inpatientid,
                            reg.inpatient_dat as inpatientdate,
                            pty.paytypename_vchr as paytype,
                            (select deptid_chr
                               from t_bse_deptdesc
                              where deptid_chr = trf.sourceareaid_chr) as sourceareaname,
                            (select code_chr
                               from t_bse_bed
                              where bedid_chr = trf.sourcebedid_chr) as sourcebedno,
                            dep.deptid_chr as targetareaname,
                            bed.code_chr as targetbedno,
                            emp.lastname_vchr as operatorname,
                            trf.modify_dat as modify_dat
              from t_bse_deptdesc           dep,
                   t_bse_bed                bed,
                   t_bse_employee           emp,
                   t_bse_patientpaytype     pty,
                   t_opr_bih_transfer       trf,
                   t_opr_bih_register       reg,
                   t_opr_bih_leave          c,
                   t_opr_bih_registerdetail pat
             where reg.registerid_chr = pat.registerid_chr
               and reg.registerid_chr = trf.registerid_chr
               and reg.registerid_chr = c.registerid_chr(+)
               and trf.targetareaid_chr = dep.deptid_chr(+)
               and trf.targetbedid_chr = bed.bedid_chr(+)
               and trf.operatorid_chr = emp.empid_chr
               and pty.paytypeid_chr = reg.paytypeid_chr
               and trf.type_int = 3
               and reg.inpatient_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') 
               and to_date(?, 'yyyy-mm-dd hh24:mi:ss')
               order by trf.modify_dat asc";//order by trf.modify_dat desc

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt2 = svc.GetDataTable(Sql2, parm);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityYgAccessRecord vo = new EntityYgAccessRecord();
                        vo.HZDM = dr["registerid_chr"].ToString();
                        vo.XM = dr["lastname_vchr"].ToString();
                        vo.RKKS = dr["ryks"].ToString();
                        vo.RKSJ = Function.Datetime(dr["rysj"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        if (dr["cysj"].ToString() != "")
                            vo.CKSJ = Function.Datetime(dr["cysj"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        vo.XH = ++n;
                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            string registerId = dr["registerid_chr"].ToString();
                            drr = dt2.Select("registerid_chr = " + registerId, "modify_dat asc");
                            if (drr != null && drr.Length > 0)
                            {
                                for (int i = 0; i < drr.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        vo.RKKS = drr[i]["sourceareaname"].ToString();

                                        if (drr[i]["modify_dat"].ToString() != "")
                                                vo.CKSJ = Function.Datetime(drr[i]["modify_dat"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                        data.Add(vo);

                                        //if (drr[i]["targetareaname"].ToString() == vo.RKKS)
                                        //{
                                        //    continue;
                                       // }
                                    }

                                    EntityYgAccessRecord vo2 = new EntityYgAccessRecord();
                                    vo2.HZDM = drr[i]["registerid_chr"].ToString();
                                    vo2.XM = drr[i]["lastname"].ToString();
                                    vo2.RKSJ = Function.Datetime(drr[i]["modify_dat"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                    vo2.RKKS = drr[i]["targetareaname"].ToString();
                                    if (i < drr.Length - 1)
                                        vo2.CKSJ = Function.Datetime(drr[i + 1]["modify_dat"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                    else
                                    {
                                        if (dr["cysj"].ToString() != "")
                                            vo2.CKSJ = Function.Datetime(dr["cysj"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    vo2.XH = ++n;
                                    data.Add(vo2);
                                }
                            }
                            else
                                data.Add(vo);
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
            return data;
        }
        #endregion

        #region  患者转科记录
        /// <summary>
        /// 患者住院记录
        /// </summary>
        /// <returns></returns>
        public List<EntityYgInpatStat> GetYgAdversInpatStat(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string Sql2 = string.Empty;
            IDataParameter[] parm = null;
            int n = 0;
            DataRow[] drr = null;
            List<EntityYgInpatStat> data = new List<EntityYgInpatStat>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select distinct t1.registerid_chr,
                        t1.inpatientid_chr as ipno,
                        inpatientcount_int as rycs,
                        b.lastname_vchr as xm,
                        b.sex_chr as sex,
                        to_char(b.birth_dat, 'yyyy-mm-dd') as birth,
                        t2.deptid_chr as ryks,
                        to_char(t1.inpatient_dat, 'yyyy-mm-dd hh24:mi:ss')  as rysj,
                        t1.mzdiagnose_vchr as ryzd,
                        d.deptid_chr as cyks,
                        to_char(c.outhospital_dat, 'yyyy-mm-dd hh24:mi:ss') as cysj,
                        c.type_int  as cyfs,
                        t1.state_int,                                   
                        t1.des_vchr,
                        t1.pstatus_int
                        from t_opr_bih_register t1
                        left join t_bse_deptdesc     t2
                        on t1.areaid_chr = t2.deptid_chr
                        left join t_bse_bed          t4
                        on t1.bedid_chr = t4.bedid_chr
                        --left join t_bse_patient      t5
                        --on t1.patientid_chr = t5.patientid_chr
                        left join t_opr_bih_registerdetail b 
                        on t1.registerid_chr = b.registerid_chr
                        left join t_opr_bih_leave c
                        on t1.registerid_chr = c.registerid_chr and c.status_int = 1
                        left join t_bse_deptdesc d 
                        on c.outareaid_chr = d.deptid_chr
                        left join t_opr_bih_order f
                        on t1.registerid_chr = f.registerid_chr
                        where t1.status_int = 1
                        and f.status_int >= 2
                        --and t5.name_vchr is not null
                        and (t1.inpatient_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss') )
                        order by t1.registerid_chr  ";
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);


                Sql2 = @"select distinct trf.TRANSFERID_CHR,reg.registerid_chr,
                            pat.lastname_vchr as lastname,
                            pat.sex_chr as sex,
                            pat.birth_dat,
                            '' as age,
                            reg.inpatientid_chr as inpatientid,
                            reg.inpatient_dat as inpatientdate,
                            pty.paytypename_vchr as paytype,
                            (select deptid_chr
                               from t_bse_deptdesc
                              where deptid_chr = trf.sourceareaid_chr) as sourceareaname,
                            (select code_chr
                               from t_bse_bed
                              where bedid_chr = trf.sourcebedid_chr) as sourcebedno,
                            dep.deptid_chr as targetareaname,
                            bed.code_chr as targetbedno,
                            emp.lastname_vchr as operatorname,
                            trf.modify_dat as modify_dat
              from t_bse_deptdesc           dep,
                   t_bse_bed                bed,
                   t_bse_employee           emp,
                   t_bse_patientpaytype     pty,
                   t_opr_bih_transfer       trf,
                   t_opr_bih_register       reg,
                   t_opr_bih_leave          c,
                   t_opr_bih_registerdetail pat
             where reg.registerid_chr = pat.registerid_chr
               and reg.registerid_chr = trf.registerid_chr
               and reg.registerid_chr = c.registerid_chr(+)
               and trf.targetareaid_chr = dep.deptid_chr(+)
               and trf.targetbedid_chr = bed.bedid_chr(+)
               and trf.operatorid_chr = emp.empid_chr
               and pty.paytypeid_chr = reg.paytypeid_chr
               and trf.type_int >= 3
               and reg.inpatient_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') 
               and to_date(?, 'yyyy-mm-dd hh24:mi:ss')
               order by trf.modify_dat asc";//order by trf.modify_dat desc

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt2 = svc.GetDataTable(Sql2, parm);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["xm"].ToString().Contains("B") && dr["ryks"].ToString() == "0000225")
                            continue;
                        EntityYgInpatStat vo = new EntityYgInpatStat();
                        vo.HZDM = dr["registerid_chr"].ToString();
                        vo.ZYH = dr["ipno"].ToString();
                        vo.CSNY = dr["birth"].ToString();
                        vo.XM = dr["xm"].ToString();
                        vo.ZYCS = dr["rycs"].ToString();
                        vo.XB = dr["sex"].ToString();
                        vo.RYKS = dr["ryks"].ToString();

                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            string registerId = dr["registerid_chr"].ToString();
                            drr = dt2.Select("registerid_chr = " + registerId, "modify_dat asc");
                            if (drr != null && drr.Length > 0 && !string.IsNullOrEmpty(drr[0]["sourceareaname"].ToString())) 
                                 vo.RYKS = drr[0]["sourceareaname"].ToString();
                        }

                        if (dr["rysj"].ToString() != "")
                            vo.RYSJ = Function.Datetime(dr["rysj"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        vo.RYZD = dr["ryzd"].ToString();
                        vo.CYKS = dr["cyks"].ToString();
                        if (dr["cysj"].ToString() != "")
                            vo.CYSJ = Function.Datetime(dr["cysj"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        vo.CYFS = dr["cyfs"].ToString();
                        vo.XH = ++n;
                        //{1=治愈出院;2=转院;3=其它;4=死亡}
                        if (dr["cyfs"].ToString() == "4")
                            vo.CYFS = "死亡";
                        else if (vo.CYKS != "")
                            vo.CYFS = "正常";
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

        #region 多种耐药记录
        /// <summary>
        /// GetYgDrugStat
        /// </summary>
        /// <returns></returns>
        public List<EntityYgDrugStat> GetYgDrugStat(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string Sql2 = string.Empty;
            IDataParameter[] parm = null;
            IDataParameter[] parm2 = null;
            DataRow[] drr = null;
            int n = 0;
            List<EntityYgDrugStat> data = new List<EntityYgDrugStat>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                Sql2 = @"select distinct 
                                a.application_id_chr,
                                d.sample_id_chr,
                                a.patient_name_vchr  AS HZXM, --患者姓名
                                a.appl_deptid_chr    ,
                                e.deptid_chr     AS SJKS, -- 送检科室
                                a.application_id_chr,
                                a.application_form_no_chr AS BGBH, --报告编号
                                d.sampletype_vchr     AS BBMC, --标本名称
                                to_char(d.sampling_date_dat, 'yyyy-mm-dd hh24:mi:ss')  AS CYSJ , --采样时间
                                to_char(r.report_dat, 'yyyy-mm-dd hh24:mi:ss')      AS BGSJ, --报告时间
                                r.summary_vchr,
                                a.patientid_chr,
                                a.patient_inhospitalno_chr,
                                r1.result_vchr   AS XJMC,
                                a.pstatus_int,
                                d.modify_dat
                                from t_opr_lis_application a 
                                left join t_opr_lis_sample d 
                                on a.application_id_chr = d.application_id_chr 
                                left join t_opr_lis_app_report r
                                on a.application_id_chr = r.application_id_chr
                                left join t_bse_deptdesc e
                                on a.appl_deptid_chr = e.deptid_chr
                                left join t_opr_lis_check_result  r1
                                on d.sample_id_chr = r1.sample_id_chr 
                                where d.status_int >= 5
                                and a.pstatus_int  = 2
                                and d.modify_dat between to_date(?,'yyyy-mm-dd hh24:mi:ss') 
                                and to_date(?,'yyyy-mm-dd hh24:mi:ss') 
                                and r.report_dat is not null
                                and r.status_int > 1
                                and d.patient_type_chr = '1'
                                and r1.result_vchr <> '\'
                                and (r1.device_check_item_name_vchr = '结果' or r1.device_check_item_name_vchr = '培养结果')
                                and r1.status_int = 1 ";


                Sql = @"select distinct 
                                a.application_id_chr,
                                c.registerid_chr,
                                d.sample_id_chr,
                                a.patient_name_vchr  AS HZXM, --患者姓名
                                a.appl_deptid_chr    ,
                                e.deptid_chr     AS SJKS, -- 送检科室
                                a.application_id_chr,
                                a.application_form_no_chr AS BGBH, --报告编号
                                c.name_vchr    , 
                                d.sampletype_vchr     AS BBMC, --标本名称
                                to_char(d.sampling_date_dat, 'yyyy-mm-dd hh24:mi:ss')  AS CYSJ , --采样时间
                                to_char(r.report_dat, 'yyyy-mm-dd hh24:mi:ss')      AS BGSJ, --报告时间
                                r.summary_vchr,
                                a.patientid_chr,
                                a.patient_inhospitalno_chr,
                                r1.result_vchr   AS XJMC,
                                a.pstatus_int,
                                d.modify_dat
                                from t_opr_lis_application a 
                                left join t_opr_attachrelation b 
                                on a.application_id_chr = b.attachid_vchr 
                                left join t_opr_bih_order c 
                                on b.sourceitemid_vchr = c.orderid_chr
                                left join t_opr_lis_sample d 
                                on a.application_id_chr = d.application_id_chr 
                                left join t_opr_lis_app_report r
                                on a.application_id_chr = r.application_id_chr
                                left join t_bse_deptdesc e
                                on a.appl_deptid_chr = e.deptid_chr
                                left join t_opr_lis_check_result  r1
                                on d.sample_id_chr = r1.sample_id_chr 
                                where  c.registerid_chr is not null
                                and d.status_int >= 5
                                and a.pstatus_int  = 2
                                and d.modify_dat between to_date(?,'yyyy-mm-dd hh24:mi:ss') 
                                and to_date(?,'yyyy-mm-dd hh24:mi:ss')
                                and r.report_dat is not null
                                and r.status_int > 1
                                and d.patient_type_chr = '1'
                                and r1.result_vchr <> '\'
                                and (r1.device_check_item_name_vchr = '结果' or r1.device_check_item_name_vchr = '培养结果')
                                and r1.status_int = 1
                                order by c.registerid_chr ";

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                parm2 = svc.CreateParm(2);
                parm2[0].Value = beginDate + " 00:00:00";
                parm2[1].Value = endDate + " 23:59:59";
                DataTable dt2 = svc.GetDataTable(Sql2, parm2);

                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        string applicationId = dr2["application_id_chr"].ToString();
                        drr = dt.Select("application_id_chr = '" + applicationId + "'");
                        EntityYgDrugStat vo = new EntityYgDrugStat();
                        vo.XH = ++n;
                        //vo.HZDM = dr["registerid_chr"].ToString();
                        vo.XM = dr2["HZXM"].ToString();
                        vo.SJKS = dr2["SJKS"].ToString();
                        vo.BGBH = dr2["BGBH"].ToString();
                        vo.BBMC = dr2["BBMC"].ToString();
                        vo.XJMC = dr2["XJMC"].ToString();
                        if (dr2["CYSJ"].ToString() != "")
                            vo.CYSJ = Function.Datetime(dr2["CYSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        if (dr2["BGSJ"].ToString() != "")
                            vo.BGSJ = Function.Datetime(dr2["BGSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        vo.SFDZNYJ = dr2["summary_vchr"].ToString();
                        vo.DZNYJZL = "";
                        
                        if (drr != null && drr.Length > 0)
                        {
                            vo.HZDM = drr[0]["registerid_chr"].ToString();
                        }

                        if (string.IsNullOrEmpty(vo.HZDM))
                        {
                            string zyId = dr2["patient_inhospitalno_chr"].ToString();
                            drr = dt.Select("patient_inhospitalno_chr = '" + zyId + "'");

                            if (drr != null && drr.Length > 0)
                                vo.HZDM = drr[0]["registerid_chr"].ToString();
                        }
                        data.Add(vo);
                    }
                }

                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        EntityYgDrugStat vo = new EntityYgDrugStat();
                //        vo.XH = ++n;
                //        vo.HZDM = dr["registerid_chr"].ToString();
                //        vo.XM = dr["HZXM"].ToString();
                //        vo.SJKS = dr["SJKS"].ToString();
                //        vo.BGBH = dr["BGBH"].ToString();
                //        vo.BBMC = dr["BBMC"].ToString();
                //        vo.XJMC = dr["XJMC"].ToString();
                //        if (dr["CYSJ"].ToString() != "")
                //            vo.CYSJ = Function.Datetime(dr["CYSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                //        if (dr["BGSJ"].ToString() != "")
                //            vo.BGSJ = Function.Datetime(dr["BGSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                //        vo.SFDZNYJ = dr["summary_vchr"].ToString();
                //        vo.DZNYJZL = "";
                //        data.Add(vo);
                //    }
                //}
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

        #region 手术信息
        /// <summary>
        /// GetYgOperationStat
        /// </summary>
        /// <returns></returns>
        public List<EntityYgOperationStat> GetYgOperationStat(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string Sql2 = string.Empty;
            string Sql3 = string.Empty;
            string Sql4 = string.Empty;
            string Sql5 = string.Empty;
            IDataParameter[] parm = null;
            DataRow[] drr = null;
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            int n = 0;
            List<EntityYgOperationStat> data = new List<EntityYgOperationStat>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select distinct
                               a.anaid_int,
                               a.registerid_chr,
                               a.patientname_vchr          AS XM, --姓名
                               a.inpatientid_chr           AS IPNO,
                               hh.deptid_chr                AS SSKS, -- 科室
                               a.operationname_chr         AS SSMC, --手术名称
                               to_char(b.operate_date, 'yyyy-mm-dd hh24:mi:ss')              AS KSSJ, --开始时间
                               ''                          AS JSSJ,--结束时间
                               c.anamode_all               AS MZFS, --麻醉方式
                               ''                          AS SQYY, --术前用药
                               aa.asalevel_all             AS ASA , --ASA评分
                               ''                          AS SQ2HYY,--术前05-2H用药
                               ''                          AS QKDJ --切口等级
                          from t_ana_requisition a
                         inner join ana_collection_eventcontent b
                            on a.anaid_int = b.anaid_int
                          left join anaesthesia_event f
                            on b.event_id = f.event_id
                            inner join ana_collection_eventcontent b1
                            on a.anaid_int = b1.anaid_int
                         --left join anaesthesia_event f1
                            --on b1.event_id = f1.event_id
                         left join ana_collection c
                            on a.anaid_int = c.anaesthesiaid_int
                         left join ana_collection aa
                         on a.anaid_int = aa.anaesthesiaid_int
                         inner join ana_record_report h
                            on a.anaid_int = h.anaesthesiaid_int
                         left join t_bse_deptdesc hh
                         on h.officename = hh.deptname_vchr
                         left join t_opr_bih_register t
                         on a.registerid_chr = t.registerid_chr
                         where a.status_int >= 1
                           and b.status = 0
                           and b1.status = 0
                           and f.event_id = '0054'
                           --and f1.event_id = '0010'
                           and b.operate_date between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                         order by a.registerid_chr ";

                Sql2 = @"select a.anaid_int,
                                c.event_id,
                                c.operate_date,
                                d.event_desc,
                                s.tag_chr,
                                s.employeename_chr
                           from t_ana_requisition a
                           left join ana_collection_eventcontent c
                             on a.anaid_int = c.anaid_int
                           left join anaesthesia_event f
                             on c.event_id = f.event_id
                           left join anaesthesia_event d
                             on c.event_id = d.event_id
                           left join t_ana_sign s
                             on a.signsequence_int = s.sequenceid_int  
                          where a.status_int >= 1
                            and c.event_id in ('0010', '0030', '0035', '0043', '0049')
                            and c.operate_date between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss') ";

                Sql3 = @" select distinct a.opendate,
                            a.descxml,
                            b.drugname,
                            a.drug_flag,
                            a.anaid_int,
                            a.seq_drug
                            from ana_collection_drugcontent a
                            left join anadrug_desc b
                            on a.drug_id = b.drug_id
                            where a.drug_flag = 2
                            and a.status = 0
                            and a.opendate between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss') ";

                Sql4 = @"select t1.inpatientid_chr,
                       t1.registerid_chr,
                       t1.patientname_vchr  AS XM, --姓名
                       t1.inpatientid_chr   AS IPNO,
                       t1.deptid_chr        AS SSKS, -- 科室
                       t1.operationname_chr AS SSMC, --手术名称
                       t1.anamode_chr       AS MZFS, --麻醉方式
                       t1.asalevel_chr      AS ASA, --ASA评分
                       t1.inpatientdate_dat,
                       t1.operationdate_dat,
                       t1.anaid_int
                  from t_ana_requisition   t1,
                       t_bse_employee      t3,
                       t_ana_operatingroom t4,
                       t_ana_operation     t10,
                       t_bse_deptdesc      t11
                 where t1.createuserid_chr = t3.empid_chr(+)
                   and t1.anaid_int = t10.aneid_int(+)
                   and t1.deptid_chr = t11.deptid_chr(+)
                   and t1.operationroomid_chr = t4.roomid_chr(+)
                   and t1.status_int >= 1
                   and (t10.status_int <> -1 or t10.status_int is null)
                   and t1.operationdate_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss')";

                Sql5 = @"select a.anaid_int, t.sequenceid_int,
                         t.tag_chr,
                         t.additionalinfo_chr,
                         t.signdate_dat,
                         t.employeeid_chr,
                         t.employeename_chr
                      from t_ana_requisition a
                      left join t_ana_sign t
                      on a.signsequence_int = t.sequenceid_int  
                     where t.signdate_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss')";

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt4 = svc.GetDataTable(Sql4, parm);

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt2 = svc.GetDataTable(Sql2, parm);

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt3 = svc.GetDataTable(Sql3, parm);

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt5 = svc.GetDataTable(Sql5, parm);

                if (dt4 != null && dt4.Rows.Count > 0)
                {
                    foreach (DataRow dr4 in dt4.Rows)
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            decimal anaId = Function.Dec(dr4["anaid_int"].ToString());
                            drr = dt.Select("anaid_int = " + anaId);
                            if (drr != null && drr.Length > 0)
                            {
                                foreach (DataRow dr in drr)
                                {
                                    EntityYgOperationStat vo = new EntityYgOperationStat();
                                    vo.HZDM = dr["registerid_chr"].ToString();
                                    vo.XM = dr["XM"].ToString();
                                    vo.IPNO = dr["ipno"].ToString();
                                    vo.SSKS = dr["SSKS"].ToString();
                                    vo.SSMC = dr["SSMC"].ToString();
                                    if (dr["KSSJ"].ToString() != "")
                                        vo.KSSJ = Function.Datetime(dr["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                    if (dr["JSSJ"].ToString() != "")
                                        vo.JSSJ = Function.Datetime(dr["JSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                                    vo.ASAPF = dr["ASA"].ToString();
                                    if (vo.ASAPF.Trim() == "Ⅰ" || vo.ASAPF.Trim() == "I" || vo.ASAPF.Trim() == "Ⅰ(E)" || vo.ASAPF.Trim() == "I(E)")
                                        vo.ASAPF = "P1";
                                    if (vo.ASAPF.Trim() == "Ⅱ" || vo.ASAPF.Trim() == "II" || vo.ASAPF.Trim() == "Ⅱ(E)" || vo.ASAPF.Trim() == "II(E)")
                                        vo.ASAPF = "P2";
                                    if (vo.ASAPF.Trim() == "Ⅲ" || vo.ASAPF.Trim() == "III" || vo.ASAPF.Trim() == "Ⅲ(E)" || vo.ASAPF.Trim() == "III(E)")
                                        vo.ASAPF = "P3";
                                    if (vo.ASAPF.Trim() == "IV" || vo.ASAPF.Trim() == "IV(E)" || vo.ASAPF.Trim() == "Ⅳ(E)")
                                        vo.ASAPF = "P4";
                                    vo.MZFS = dr["MZFS"].ToString();
                                    vo.SQYFYY = dr["SQYY"].ToString();
                                    vo.QKTJ = dr["QKDJ"].ToString();
                                    vo.SQ2HYY = dr["SQ2HYY"].ToString();
                                    vo.XH = ++n;

                                    if (dt2 != null && dt2.Rows.Count > 0)
                                    {
                                        decimal anaId2 = Function.Dec(dr4["anaid_int"].ToString());
                                        drr = dt2.Select("anaid_int = " + anaId2);
                                        if (drr != null && drr.Length > 0)
                                        {
                                            foreach (DataRow dr2 in drr)
                                            {
                                                if (dr2["tag_chr"] != DBNull.Value)
                                                {
                                                    if (dr2["tag_chr"].ToString().Trim() == "主刀医师")
                                                        vo.SSYS = dr2["employeename_chr"].ToString();
                                                    else if (dr2["tag_chr"].ToString().Trim() == "一助")
                                                        vo.SSYS += "、" + dr2["employeename_chr"].ToString();
                                                }

                                                if (dr2["event_id"] != DBNull.Value)
                                                {
                                                    if (dr2["event_id"].ToString() == "0010")
                                                    {
                                                        if (dr2["operate_date"].ToString() != "")
                                                            vo.JSSJ = Function.Datetime(dr2["operate_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                    }
                                                    if (dr2["event_id"].ToString() == "0035")
                                                    {
                                                        if (dr2["operate_date"].ToString() != "")
                                                            vo.KSSJ = Function.Datetime(dr2["operate_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (dt3 != null && dt3.Rows.Count > 0)
                                    {
                                        decimal anaId3 = Function.Dec(dr4["anaid_int"].ToString());
                                        drr = dt3.Select("anaid_int = " + anaId3);

                                        if (drr != null && drr.Length > 0)
                                        {
                                            foreach (DataRow dr3 in drr)
                                            {
                                                string xmlData = dr3["descxml"].ToString();
                                                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                                doc.LoadXml(xmlData);
                                                XmlNodeList xnl = doc.SelectSingleNode("M").ChildNodes;
                                                foreach (XmlNode xn in xnl)
                                                {
                                                    XmlElement xe = (XmlElement)xn;
                                                    vo.SQYFYY += xe.GetAttribute("Name") + "、";
                                                }
                                            }
                                            vo.SQYFYY = vo.SQYFYY.TrimEnd('、');
                                        }
                                    }
                                    data.Add(vo);
                                }
                            }
                            else
                            {
                                EntityYgOperationStat vo1 = new EntityYgOperationStat();
                                vo1.HZDM = dr4["registerid_chr"].ToString();
                                vo1.XM = dr4["XM"].ToString();
                                vo1.IPNO = dr4["ipno"].ToString();
                                vo1.SSKS = dr4["SSKS"].ToString();
                                vo1.SSMC = dr4["SSMC"].ToString();
                                vo1.MZFS = dr4["MZFS"].ToString();
                                //vo.SQYFYY = dr4["SQYY"].ToString();
                                //vo.QKTJ = dr4["QKDJ"].ToString();
                                //vo.SQ2HYY = dr4["SQ2HYY"].ToString();
                                vo1.XH = ++n;

                                if (dt5 != null && dt5.Rows.Count > 0)
                                {
                                    decimal anaId5 = Function.Dec(dr4["anaid_int"].ToString());
                                    drr = dt5.Select("anaid_int = " + anaId5);

                                    if (drr != null && drr.Length > 0)
                                    {
                                        foreach (DataRow dr5 in drr)
                                        {
                                            if (dr5["tag_chr"] != DBNull.Value)
                                            {
                                                if (dr5["tag_chr"].ToString().Trim() == "主刀医师")
                                                    vo1.SSYS = dr5["employeename_chr"].ToString();
                                                else if (dr5["tag_chr"].ToString().Trim() == "一助")
                                                    vo1.SSYS += "、" + dr5["employeename_chr"].ToString();
                                            }
                                        }
                                    }
                                }
                                data.Add(vo1);
                            }
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
            return data;
        }
        #endregion

        #region 住院材料信息
        /// <summary>
        /// GetYgMachineStat
        /// </summary>
        /// <returns></returns>
        public List<EntityYgMachineStat> GetYgMachineStat(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            IDataParameter[] parm = null;
            int n = 0;
            List<EntityYgMachineStat> data = new List<EntityYgMachineStat>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select distinct a.registerid_chr,
                               a.inpatient_dat,
                               p.name_vchr   as XM,
                               e.deptid_chr as RYKS,
                               dd.deptid_chr as SYKS,
                               b.chargeitemid_chr,
                               b.chargeitemname_chr  as JXZL,
                               c.name_vchr            as YCMC,
                               to_char(c.startdate_dat, 'yyyy-mm-dd hh24:mi:ss') as KSSJ,
                               to_char(c.stopdate_dat, 'yyyy-mm-dd hh24:mi:ss') as JSSJ
                          from t_opr_bih_register a
                          left join t_bse_deptdesc e
                            on a.areaid_chr = e.deptid_chr
                          left join t_bse_patient p
                            on a.patientid_chr = p.patientid_chr
                          left join t_opr_bih_patientcharge b
                            on a.registerid_chr = b.registerid_chr
                          left join t_opr_bih_order c
                            on a.registerid_chr = c.registerid_chr
                           and c.orderid_chr = b.orderid_chr
                          left join t_bse_deptdesc dd
                            on c.curareaid_chr = dd.deptid_chr
                         where a.status_int = 1
                           and b.status_int = 1
                           and c.status_int >= 1
                           and c.executetype_int = 1
                           --and (a.inpatient_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss'))
                           and (c.startdate_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss'))
                           and b.chargeitemid_chr in ( '0000005105','0000006814', '0000006648')
                         order by a.registerid_chr  ";

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityYgMachineStat vo = new EntityYgMachineStat();
                        vo.HZDM = dr["registerid_chr"].ToString();
                        vo.XM = dr["XM"].ToString();
                        vo.SYKS = dr["SYKS"].ToString();
                        vo.JXZL = dr["JXZL"].ToString();
                        if ( dr["chargeitemid_chr"].ToString() == "0000006814" )
                            vo.JXZL = "泌尿道插管";
                        else if (dr["chargeitemid_chr"].ToString() == "0000005105")
                            vo.JXZL = "呼吸机";
                        //else if (dr["chargeitemid_chr"].ToString() == "0000006727")
                        //    vo.JXZL = "中央静脉导管";
                        else if (dr["chargeitemid_chr"].ToString() == "0000006648")
                            vo.JXZL = "中央静脉导管";
                        vo.YCMC = dr["YCMC"].ToString();
                        if (dr["KSSJ"].ToString() != "")
                            vo.KSSJ = Function.Datetime(dr["KSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        if (dr["JSSJ"].ToString() != "")
                            vo.JSSJ = Function.Datetime(dr["JSSJ"].ToString()).ToString("yyyy-MM-dd HH:mm:ss");
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

        #region  院感预警信息

        #region 检验信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetCheckLisInfo(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string result = string.Empty;
            string registerId = string.Empty;
            string inPatientId = string.Empty;
            string patName = string.Empty;
            string sex = string.Empty;
            string itemName = string.Empty;
            string itemResult = string.Empty;
            string aplicationId = string.Empty;
            DateTime applyDate;

            int affectRows = 0;
            List<EntityAlert> data = new List<EntityAlert>();
            List<DacParm> lstParm = new List<DacParm>();
            SqlHelper svc = null;

            try
            {
                Sql = @"select distinct c.registerid_chr           as registerId,
                                        tr.inpatient_dat,
                                        a.patient_inhospitalno_chr as inPatientId,
                                        a.application_id_chr       as aplicationId,
                                        d.appl_dat                 as applyDate,
                                        a.patient_name_vchr        AS patName, 
                                        a.sex_chr                  as sex,
                                        e.deptid_chr               AS SJKS, 
                                        a.application_form_no_chr AS BGBH, 
                                        d.sample_id_chr,
                                        tb.lisapplyunitid_chr,---项目ID
                                        c.name_vchr ,
                                        d.sampletype_vchr , --标本类型
                                        r1.check_item_id_chr, --申请单元ID
                                        r1.check_item_name_vchr as itemName, --项目名称
                                        r1.result_vchr as itemResult,--申请单元结果
                                        to_char(d.sampling_date_dat, 'yyyy-mm-dd hh24:mi:ss') AS CYSJ, --采样时间
                                        to_char(r.report_dat, 'yyyy-mm-dd hh24:mi:ss') AS BGSJ, --报告时间
                                        a.pstatus_int,
                                        d.modify_dat
                          from t_opr_lis_application a
                          left join t_opr_attachrelation b
                            on a.application_id_chr = b.attachid_vchr
                          left join t_opr_bih_order c
                            on b.sourceitemid_vchr = c.orderid_chr
                          left join t_opr_lis_sample d
                            on a.application_id_chr = d.application_id_chr
                          left join t_opr_lis_app_report r
                            on a.application_id_chr = r.application_id_chr
                          left join t_bse_deptdesc e
                            on a.appl_deptid_chr = e.deptid_chr
                          left join t_opr_lis_check_result r1
                            on d.sample_id_chr = r1.sample_id_chr
                          left join t_bse_bih_orderdic tb
                          on c.orderdicid_chr = tb.orderdicid_chr
                           left join t_opr_bih_register tr
                          on c.registerid_chr = tr.registerid_chr
                         where c.registerid_chr is not null
                           and d.status_int > 5
                           and a.pstatus_int = 2
                           and d.appl_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                           and r.report_dat is not null
                           and r.status_int > 1
                           and r1.status_int = 1
                         order by c.registerid_chr";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate;
                parm[1].Value = endDate;

                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DateTime appDate = Function.Datetime(dr["applyDate"]);
                        DateTime inDate = Function.Datetime(dr["inpatient_dat"]);
                        TimeSpan ts = appDate - inDate;
                        applyDate = Function.Datetime(dr["applyDate"]);

                        EntityAlert vo = new EntityAlert();
                        vo.registerId = dr["registerId"].ToString();
                        vo.inPatientId = dr["inPatientId"].ToString();
                        vo.patName = dr["patName"].ToString();
                        vo.sex = dr["sex"].ToString();
                        if (vo.sex.Trim() == "女")
                            vo.sex = "2";
                        else
                            vo.sex = "1";
                        vo.orderName = dr["name_vchr"].ToString();
                        vo.itemName = dr["itemName"].ToString();
                        vo.itemResult = dr["itemResult"].ToString();
                        vo.aplicationId = dr["aplicationId"].ToString();
                        vo.applyDate = Function.Datetime(dr["applyDate"]);
                        vo.recordDate = endDate;
                        vo.deptId = dr["SJKS"].ToString();
                        vo.OrderId = dr["lisapplyunitid_chr"].ToString();
                        vo.checkItemId = dr["check_item_id_chr"].ToString();
                        result = dr["itemResult"].ToString();
                        if (Function.Int(ts.TotalHours) >= 48)
                        {
                            //尿常规
                            if (dr["lisapplyunitid_chr"].ToString() == "001060" &&
                                dr["check_item_id_chr"].ToString() == "000043")
                            {
                                result = dr["itemResult"].ToString();
                                if (Function.Int(result) >= 24)
                                {
                                    vo.typeInt = 3;
                                    data.Add(vo);
                                }
                            }

                            //降钙素原（定量）（PCT）
                            if (dr["lisapplyunitid_chr"].ToString() == "001022" &&
                                dr["check_item_id_chr"].ToString() == "001623")
                            {
                                result = dr["itemResult"].ToString().Trim();
                                if (result.Contains("<"))
                                    continue;
                                else if (dr["itemResult"].ToString().Trim().Contains(">"))
                                {
                                    result = result.Replace(">","");
                                }

                                if (Function.Double(result) > 0.5)
                                {
                                    vo.typeInt = 4;
                                    data.Add(vo);
                                }
                            }
                            //涂片找菌
                            if (dr["lisapplyunitid_chr"].ToString() == "000687" ||
                                dr["lisapplyunitid_chr"].ToString() == "000455" ||
                                dr["lisapplyunitid_chr"].ToString() == "000807")
                            {
                                if (dr["itemResult"].ToString().Contains("未找到"))
                                    continue;
                                else if ((dr["itemResult"].ToString().Contains("找到")))
                                {
                                    vo.typeInt = 2;
                                    data.Add(vo);
                                }
                            }

                            //细菌培养+药敏 
                            if (dr["lisapplyunitid_chr"].ToString() == "000405" &&
                                dr["check_item_id_chr"].ToString() == "000860")
                            {
                                result = dr["itemResult"].ToString();
                                if (string.IsNullOrEmpty(result) && result != "\\")
                                {
                                    vo.typeInt = 1;
                                    data.Add(vo);
                                }
                            }

                            //轮状病毒
                            if (dr["lisapplyunitid_chr"].ToString() == "000498")
                            {
                                if (dr["itemResult"].ToString().Trim().Contains("阳性"))
                                {
                                    vo.typeInt = 5;
                                    data.Add(vo);
                                }
                            }

                            //快速C反应蛋白测定
                            if (dr["check_item_id_chr"].ToString() == "001526")
                            {
                                result = dr["itemResult"].ToString().Trim();
                                if (result.Contains("<"))
                                    continue;
                                else if (dr["itemResult"].ToString().Trim().Contains(">"))
                                {
                                    result = result.Replace(">", "");
                                }

                                if (Function.Double(result.Trim()) > 10)
                                {
                                    vo.typeInt = 9;
                                    data.Add(vo);
                                }
                            }
                        }

                        //送检类型：脓液 分泌物 
                        if (dr["sampletype_vchr"].ToString().Trim().Contains("脓液") ||
                            dr["sampletype_vchr"].ToString().Trim().Contains("分泌物"))
                        {
                            if (result.Trim().Contains("阴性") || result.Trim().Contains("阳性") || result.Trim().Contains("培养结果"))
                            {
                                vo.typeInt = 15;
                                data.Add(vo);
                            }
                        }

                        //脑脊液
                        if (dr["sampletype_vchr"].ToString().Trim().Contains("脑脊液") &&
                            (dr["check_item_id_chr"].ToString() == "000175"))
                        {
                            if (dr["itemResult"].ToString().Trim() != "\\")
                            {
                                result = dr["itemResult"].ToString().Trim();
                                if (Function.Int(result) >= 50)
                                {
                                    vo.typeInt = 17;
                                    data.Add(vo);
                                }
                            }
                        }
                    }
                    if (data.Count > 0)
                        lstParm.Add(svc.GetInsertParm(data.ToArray()));
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                Log.Output("GetCheckLisInfo-->e");
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }

            return affectRows;
        }
        #endregion

        #region 生命体征信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetVsInfo(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string result = string.Empty;
            string registerId = string.Empty;
            string inPatientId = string.Empty;
            string patName = string.Empty;
            string sex = string.Empty;
            string itemName = string.Empty;
            string itemResult = string.Empty;
            string aplicationId = string.Empty;

            int affectRows = 0;
            List<EntityAlert> data = new List<EntityAlert>();
            List<DacParm> lstParm = new List<DacParm>();
            SqlHelper svc = null;

            try
            {
                Sql = @"select distinct a.registerid_chr,
                                        b.inpatientid,
                                        e.name_vchr,
                                        e.sex_chr,
                                        '体温' as ordername,
                                        a.inareadate_dat,
                                        b.opendate,
                                        b.createdate,
                                        b.temperaturexml,
                                        b.status,
                                        a.areaid_chr
                          from t_opr_bih_register a
                          left join threemeasurerecord b
                            on a.inpatientid_chr = b.inpatientid and a.modify_dat = b.inpatientdate
                          left join threemeasurerecordcontent c
                            on a.inpatientid_chr = c.inpatientid and a.modify_dat = c.inpatientdate
                          left join t_bse_patient e
                            on a.patientid_chr = e.patientid_chr
                         where c.modifydate in
                               (select max(e.modifydate)
                                  from threemeasurerecordcontent e
                                 where e.opendate between
                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                 group by e.opendate)
                           and b.status = 0
                           and a.pstatus_int not in (3,4)
                           and b.opendate between
                               to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss')";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(4);
                parm[0].Value = beginDate;
                parm[1].Value = endDate;
                parm[2].Value = beginDate;
                parm[3].Value = endDate;

                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DateTime appDate = Function.Datetime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        DateTime inDate = Function.Datetime(dr["inareadate_dat"]);
                        TimeSpan ts = appDate - inDate;

                        if (Function.Int(ts.TotalHours) >= 48)
                        {
                            EntityAlert vo = new EntityAlert();
                            vo.registerId = dr["registerid_chr"].ToString();
                            vo.inPatientId = dr["inpatientid"].ToString();
                            vo.patName = dr["name_vchr"].ToString();
                            vo.sex = dr["sex_chr"].ToString();
                            if (vo.sex.Trim() == "女")
                                vo.sex = "2";
                            else
                                vo.sex = "1";
                            vo.orderName = dr["ordername"].ToString();
                            vo.itemName = Function.Datetime(dr["opendate"]).ToString("yyyy-MM-dd HH:mm");
                            vo.itemResult = GetTempreture(dr["temperaturexml"].ToString(), ref vo);
                            if (Function.Double(vo.itemResult) < 38)
                                continue;
                            vo.itemResult = vo.itemResult + "℃";

                            vo.aplicationId = dr["inpatientid"].ToString();
                            vo.applyDate = Function.Datetime(dr["opendate"]);
                            vo.recordDate = endDate;
                            vo.deptId = dr["areaid_chr"].ToString();
                            vo.OrderId = Function.Datetime(dr["createdate"]).ToString("yyyyMMddHHmmss");
                            vo.checkItemId = Function.Datetime(dr["opendate"]).ToString("yyyyMMddHHmmss");
                            vo.typeInt = 5;
                            data.Add(vo);
                        }
                    }
                    if (data.Count > 0)
                        lstParm.Add(svc.GetInsertParm(data.ToArray()));
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                Log.Output("GetVsInfo-->e");
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }

            return affectRows;
        }
        #endregion

        #region 生命体征信息2
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetVsInfo2(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string result = string.Empty;
            string registerId = string.Empty;
            string inPatientId = string.Empty;
            string patName = string.Empty;
            string sex = string.Empty;
            string itemName = string.Empty;
            string itemResult = string.Empty;
            string aplicationId = string.Empty;

            int affectRows = 0;
            List<EntityAlert> data = new List<EntityAlert>();
            List<DacParm> lstParm = new List<DacParm>();
            SqlHelper svc = null;

            try
            {
                Sql = @"select c.registerid_chr,
                                a.inpatientid,
                                c.inpatient_dat,
                                c.areaid_chr,
                                p.name_vchr,
                                p.sex_chr,
                                a.opendate,
                                a.diagnosis AS RYZD,
                                a.maindiagnosis AS CYZZD,
                                b.diagnosis AS CYQTZD,
                                b.seqid,
                                a.mainconditionseq
                                from inhospitalmainrecord_content a
                                left join inhospitalmainrecord_diagnosis b
                                on a.inpatientid = b.inpatientid and a.inpatientdate = b.inpatientdate
                                left join t_opr_bih_register c
                                on a.inpatientdate = c.modify_dat and a.inpatientid = c.inpatientid_chr
                                left join t_bse_patient p
                                on c.patientid_chr = p.patientid_chr
                                where  a.status =1 and b.status = 1
                                and a.opendate between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss')";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate;
                parm[1].Value = endDate;

                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityAlert vo = new EntityAlert();
                        vo.registerId = dr["registerid_chr"].ToString();
                        vo.inPatientId = dr["inpatientid"].ToString();
                        vo.patName = dr["name_vchr"].ToString();
                        vo.sex = dr["sex_chr"].ToString();
                        if (vo.sex.Trim() == "女")
                            vo.sex = "2";
                        else
                            vo.sex = "1";
                        vo.orderName = dr["RYZD"].ToString().Trim();
                        if (vo.orderName.Contains("褥疮") || vo.orderName.Contains("带状疮诊"))
                            continue;
                        vo.itemName = "出院诊断:";
                        vo.aplicationId = dr["inpatientid"].ToString();
                        vo.applyDate = Function.Datetime(dr["opendate"]);
                        vo.recordDate = endDate;
                        vo.deptId = dr["areaid_chr"].ToString();
                        vo.OrderId = Function.Datetime(dr["inpatientid"]).ToString("yyyyMMddHHmmss");
                        vo.checkItemId = Function.Datetime(dr["opendate"]).ToString("yyyyMMddHHmmss") + dr["seqid"].ToString();
                        vo.typeInt = 20;

                        vo.itemResult = dr["CYZZD"].ToString().Trim();
                        if (vo.itemResult.Contains("褥疮") || vo.orderName.Contains("带状疮诊"))
                        {
                            data.Add(vo);
                            break;
                        }
                        else
                        {
                            vo.itemResult = dr["CYQTZD"].ToString().Trim();
                            if (vo.itemResult.Contains("褥疮") || vo.orderName.Contains("带状疮诊"))
                                data.Add(vo);
                        }
                    }
                    if (data.Count > 0)
                        lstParm.Add(svc.GetInsertParm(data.ToArray()));
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                Log.Output("GetVsInfo2-->e");
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }

            return affectRows;
        }
        #endregion

        #region 生命体征信息3
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetVsInfo3(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string Sql2 = string.Empty;
            string inpatientId = string.Empty;
            DataRow[] drr = null;
            int affectRows = 0;
            List<EntityAlert> data = new List<EntityAlert>();
            List<DacParm> lstParm = new List<DacParm>();
            SqlHelper svc = null;

            try
            {
                Sql = @"select t1.registerid_chr,
                                t1.inpatientid_chr as ipno,
                                inpatientcount_int as rycs,
                                t3.name_vchr,
                                t3.sex_chr,
                                t2.deptid_chr as ryks,
                                to_char(t1.inpatient_dat, 'yyyy-mm-dd hh24:mi:ss')  as rysj,
                                t1.mzdiagnose_vchr as ryzd
                                from t_opr_bih_register t1
                                left join t_bse_deptdesc     t2
                                on t1.areaid_chr = t2.deptid_chr
                                left join t_bse_patient t3
                                on t1.patientid_chr = t3.patientid_chr
                                where t1.status_int = 1 and t1.pstatus_int in (0,1)
                                and (t1.modify_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') 
                                and to_date(?, 'yyyy-mm-dd hh24:mi:ss') )
                                order by t1.registerid_chr";

                Sql2 = @"select c.registerid_chr,
                                t1.inpatientid_chr,
                                t2.anaid_int,
                                c.outhospital_dat 
                                from t_opr_bih_leave c 
                                left join t_opr_bih_register t1
                                on c.registerid_chr = t1.registerid_chr
                                left join t_ana_requisition t2
                                on c.registerid_chr = t2.registerid_chr and t2.status_int >= 1
                                where  c.status_int  = 1 and c.outhospital_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') 
                                and to_date(?, 'yyyy-mm-dd hh24:mi:ss') ";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate;
                parm[1].Value = endDate;

                beginDate = Function.Datetime(beginDate).AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss");
                IDataParameter[] parm2 = null;
                parm2 = svc.CreateParm(2);
                parm2[0].Value = beginDate;
                parm2[1].Value = endDate;

                DataTable dt = svc.GetDataTable(Sql, parm);
                DataTable dt2 = svc.GetDataTable(Sql2, parm2);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityAlert vo = new EntityAlert();
                        vo.registerId = dr["registerid_chr"].ToString();
                        vo.inPatientId = dr["ipno"].ToString();
                        vo.patName = dr["name_vchr"].ToString();
                        vo.sex = dr["sex_chr"].ToString();
                        if (vo.sex.Trim() == "女")
                            vo.sex = "2";
                        else
                            vo.sex = "1";
                        vo.orderName = "";
                        vo.aplicationId = "";
                        vo.applyDate = Function.Datetime(dr["rysj"]);
                        vo.recordDate = endDate;
                        vo.deptId = dr["ryks"].ToString();
                        vo.OrderId = "";
                        vo.checkItemId = "";
                        vo.typeInt = 19;

                        inpatientId = dr["ipno"].ToString();
                        drr = dt2.Select("inpatientid_chr = '" + inpatientId + "'");
                        if (drr != null && drr.Length > 0)
                        {
                            foreach (DataRow dr2 in drr)
                            {
                                if (!string.IsNullOrEmpty(dr2["anaid_int"].ToString()))
                                {
                                    vo.itemName = "有手术患者 1个月再入院 出院时间:";
                                    vo.itemResult = Function.Datetime(dr2["outhospital_dat"]).ToString("yyyy-MM-dd HH:mm");
                                    data.Add(vo);
                                }
                                else
                                {
                                    DateTime outDate = Function.Datetime(dr2["outhospital_dat"]);
                                    DateTime inDate = Function.Datetime(dr["rysj"]);
                                    TimeSpan ts = inDate - outDate;
                                    if (ts.Days <= 3)
                                    {
                                        vo.itemName = "无手术患者3天再入院 出院时间:";
                                        vo.itemResult = Function.Datetime(dr2["outhospital_dat"]).ToString("yyyy-MM-dd HH:mm");
                                        data.Add(vo);
                                    }
                                }
                            }
                        }
                    }
                    if (data.Count > 0)
                        lstParm.Add(svc.GetInsertParm(data.ToArray()));
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                Log.Output("GetVsInfo3-->e");
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }

            return affectRows;
        }
        #endregion

        #region 医嘱信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetOrderAlertInfo(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            string result = string.Empty;
            string registerId = string.Empty;
            string mzzd  = string.Empty;
            string antiuse = string.Empty;
            string orderType = string.Empty;//长临嘱
            string putmedicineflg = string.Empty;
            int affectRows = 0;
            List<EntityAlert> data = new List<EntityAlert>();
            List<DacParm> lstParm = new List<DacParm>();
            SqlHelper svc = null;

            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select distinct a.registerid_chr,
                               a.inpatientid_chr,
                               a.inpatient_dat,
                               p.name_vchr   as XM,
                               p.sex_chr,
                               e.deptid_chr as RYKS,
                               dd.deptid_chr as SYKS,
                               b.chargeitemid_chr,
                               b.chargeitemname_chr  as JXZL,
                               b.putmedicineflag_int,
                               c.antiuse,
                               c.executetype_int,
                               c.name_vchr           as YCMC,
                               a.mzdiagnose_vchr,
                               to_char(c.startdate_dat, 'yyyy-mm-dd hh24:mi:ss') as KSSJ,
                               to_char(c.stopdate_dat, 'yyyy-mm-dd hh24:mi:ss') as JSSJ
                          from t_opr_bih_register a
                          left join t_bse_deptdesc e
                            on a.areaid_chr = e.deptid_chr
                          left join t_bse_patient p
                            on a.patientid_chr = p.patientid_chr
                          left join t_opr_bih_patientcharge b
                            on a.registerid_chr = b.registerid_chr
                          left join t_opr_bih_order c
                            on a.registerid_chr = c.registerid_chr
                           and c.orderid_chr = b.orderid_chr
                          left join t_bse_deptdesc dd
                            on c.curareaid_chr = dd.deptid_chr
                         where a.status_int = 1
                           and b.status_int = 1
                           and c.status_int >= 1
                           and (c.startdate_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') and to_date(?, 'yyyy-mm-dd hh24:mi:ss'))
                         order by a.registerid_chr  ";

                svc = new SqlHelper(EnumBiz.onlineDB);
                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate;
                parm[1].Value = endDate;

                DataTable dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityAlert vo = new EntityAlert();
                        vo.registerId = dr["registerid_chr"].ToString();
                        vo.inPatientId = dr["inpatientid_chr"].ToString();
                        vo.patName = dr["XM"].ToString();
                        vo.sex = dr["sex_chr"].ToString();
                        if (vo.sex.Trim() == "女")
                            vo.sex = "2";
                        else
                            vo.sex = "1";
                        vo.orderName = dr["JXZL"].ToString().Trim();
                        vo.itemName = Function.Datetime(dr["KSSJ"]).ToString("yyyy-MM-dd HH:mm");
                        vo.itemResult = dr["JXZL"].ToString().Trim();
                        mzzd = dr["mzdiagnose_vchr"].ToString();
                        antiuse = dr["antiuse"].ToString().Trim();
                        orderType = dr["executetype_int"].ToString();
                        putmedicineflg = dr["putmedicineflag_int"].ToString();
                        if ((dr["chargeitemid_chr"].ToString() == "0000006723" ||
                            dr["chargeitemid_chr"].ToString() == "0000006725") && orderType == "1")
                            vo.typeInt = 14;
                        else if (dr["chargeitemid_chr"].ToString() == "0000005105" && orderType == "1")
                            vo.typeInt = 12;
                        else if (dr["chargeitemid_chr"].ToString() == "0000006814" && orderType == "1")
                            vo.typeInt = 13;
                        else if (!string.IsNullOrEmpty(antiuse) && antiuse != "0" && putmedicineflg == "1" &&
                            (mzzd.Contains("炎") || mzzd.Contains("感染")) && orderType != "3")//新开抗菌药
                            vo.typeInt = 11;
                        else
                            continue;

                        vo.aplicationId = Function.Datetime(dr["KSSJ"]).ToString("yyyyMMddHHmmss");
                        vo.applyDate = Function.Datetime(dr["KSSJ"]);
                        vo.recordDate = endDate;
                        vo.deptId = dr["SYKS"].ToString();
                        vo.OrderId = Function.Datetime(dr["KSSJ"]).ToString("yyyyMMddHHmmss");
                        vo.checkItemId = dr["chargeitemid_chr"].ToString();
                        data.Add(vo);
                    }
                    if (data.Count > 0)
                        lstParm.Add(svc.GetInsertParm(data.ToArray()));
                    if (lstParm.Count > 0)
                        affectRows = svc.Commit(lstParm);
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                Log.Output("GetOrderAlertInfo-->e");
                affectRows = -1;
            }
            finally
            {
                svc = null;
            }

            return affectRows;
        }
        #endregion

        #region 获取预警信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityAlertDisplay> getAlertInfo(List<EntityParm> dicParm)
        {
            string Sql = string.Empty;
            int n = 0;
            List<int> lstLisType = new List<int> { 1, 2, 3, 4, 8, 9, 15, 16, 17, 18 };
            List<int> lstOrderType = new List<int> { 11,12, 13, 14 };
            List<int> lstVsType = new List<int> { 5, 6, 7, 19, 20 };

            List<EntityAlertDisplay> data = new List<EntityAlertDisplay>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select distinct a.inpatientcount_int,
                                        a.inpatient_dat,
                                        c.deptname_vchr,
                                        b.registerid,
                                        b.inpatientid,
                                        b.patname,
                                        b.sex,
                                        b.typeint,
                                        b.itemname,
                                        b.itemresult
                          from t_opr_bih_register a, t_alerted b, t_bse_deptdesc c
                         where a.registerid_chr = b.registerid
                           and b.deptid = c.deptid_chr ";

                string strSub = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();
                IDataParameter parm = svc.CreateParm();

                foreach (EntityParm po in dicParm)
                {
                    parm = svc.CreateParm();
                    string keyValue = po.value;
                    parm.Value = keyValue;
                    switch (po.key)
                    {
                        case "queryDate":
                             IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0] + " 00:00:00";
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            strSub += " and b.recorddate between ? and ? ";
                            break;
                        case "deptCode":
                            parm.Value = parm.Value.ToString().Replace("'", "");
                            lstParm.Add(parm);
                            strSub += " and (c.code_vchr = ?)";
                            break;
                        default:
                            break;
                    }
                }

                Sql += strSub;
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EntityAlertDisplay vo = new EntityAlertDisplay();
                        vo.registerId = dr["registerId"].ToString();
                        vo.inPatientId = dr["inpatientid"].ToString();
                        vo.inCount = dr["inpatientcount_int"].ToString();
                        vo.inPatientDate = Function.Datetime(dr["inpatient_dat"]).ToString("yyyy-MM-dd HH:mm");
                        vo.patName = dr["patname"].ToString();
                        vo.sex = dr["sex"].ToString();
                        //vo.recordDate = Function.Datetime(dr["recorddate"]).ToString("yyyy-MM-dd HH:mm");
                        vo.deptName = dr["deptname_vchr"].ToString();
                        if (vo.sex == "1")
                            vo.sex = "男";
                        else
                            vo.sex = "女";
                        int typeInt = Function.Int(dr["typeint"]);

                        if (lstLisType.Contains(typeInt))
                        {
                            if (typeInt == 1)
                                vo.lisAlertInfo += "细菌培养+药敏--" + dr["itemname"].ToString() + ": " + dr["itemresult"].ToString() + "\r\n";
                            if(typeInt == 2)
                                vo.lisAlertInfo += "涂片找菌--" + dr["itemname"].ToString() + ": " + dr["itemresult"].ToString() + "\r\n";
                            if (typeInt == 3)
                                vo.lisAlertInfo += "尿液常规--" + dr["itemname"].ToString() + ": " + dr["itemresult"].ToString() + "\r\n";
                            if (typeInt == 4)
                                vo.lisAlertInfo += "降钙素原(PCT)--" + dr["itemname"].ToString() + ": " + dr["itemresult"].ToString() + "\r\n";
                            if (typeInt == 8)
                                vo.lisAlertInfo += "轮状病毒--" + dr["itemname"].ToString() + ": " + dr["itemresult"].ToString() + "\r\n";
                            if (typeInt == 9)
                                vo.lisAlertInfo += "快速C反应--" + dr["itemname"].ToString() + ": " + dr["itemresult"].ToString() + "\r\n";
                            if (typeInt == 15)
                                vo.lisAlertInfo += "脓液、分泌物--" + dr["itemname"].ToString() + ": " + dr["itemresult"].ToString() + "\r\n";
                            if (typeInt == 16)
                                vo.lisAlertInfo += "胸水、腹水、关节腔积液--" + dr["itemname"].ToString() + ": " + dr["itemresult"].ToString() + "\r\n";
                            if (typeInt == 17 || typeInt == 18)
                                vo.lisAlertInfo += "脑脊液--" + dr["itemname"].ToString() + ": " + dr["itemresult"].ToString() + "\r\n";
                        }



                        if (lstVsType.Contains(typeInt))
                        {
                            if(typeInt == 5 || typeInt == 6)
                                vo.vsAlertInfo +="发热--" + dr["itemname"].ToString() + " " + dr["itemresult"].ToString() + "\r\n";
                            if(typeInt == 7 )
                                vo.vsAlertInfo +="疑似腹泻--" + dr["itemname"].ToString() + " " + dr["itemresult"].ToString() + "\r\n";
                            if(typeInt == 19 )
                                vo.vsAlertInfo +="再入院预警--" + dr["itemname"].ToString() + " " + dr["itemresult"].ToString() + "\r\n";
                            if(typeInt == 20 )
                                vo.vsAlertInfo +="出院诊断--" + dr["itemname"].ToString() + " " + dr["itemresult"].ToString() + "\r\n";
                        }


                        if (lstOrderType.Contains(typeInt))
                        {
                            if(typeInt == 11)
                                vo.orderAlertInfo +="新开抗菌药--" + dr["itemname"].ToString() + " " + dr["itemresult"].ToString().Trim() + "\r\n";
                            else
                                vo.orderAlertInfo += "医嘱--" + dr["itemname"].ToString() + " " + dr["itemresult"].ToString().Trim() + "\r\n";
                        }

                        if (data.Any(t => t.registerId == vo.registerId))
                        {
                            int index = data.FindIndex(t => t.registerId == vo.registerId);
                            if (lstLisType.Contains(typeInt))
                                data[index].lisAlertInfo += vo.lisAlertInfo;
                            if (lstVsType.Contains(typeInt))
                                data[index].vsAlertInfo += vo.vsAlertInfo;
                            if (lstOrderType.Contains(typeInt))
                                data[index].orderAlertInfo += vo.orderAlertInfo;
                        }
                        else
                        {
                            vo.XH = ++n;
                            data.Add(vo);
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
            return data;
        }
        #endregion

        #region 获取预警信息科室统计
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityAlertStat> getAlertStat(string beginDate, string endDate)
        {
            string Sql = string.Empty;
            IDataParameter[] parm = null;
            int n = 0;
            List<int> lstLisType = new List<int> { 1, 2, 3, 4, 8, 9, 15, 16, 17, 18 };
            List<int> lstOrderType = new List<int> { 12, 13, 14 };
            List<int> lstVsType = new List<int> { 5, 6, 7, 19, 20 };

            List<EntityAlertStat> data = new List<EntityAlertStat>();
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select distinct a.inpatientcount_int,
                                        a.inpatient_dat,
                                        c.deptname_vchr,
                                        c.deptid_chr,
                                        b.registerid,
                                        b.inpatientid,
                                        b.patname,
                                        b.sex,
                                        b.typeint
                          from t_opr_bih_register a, t_alerted b, t_bse_deptdesc c
                         where a.registerid_chr = b.registerid
                           and b.deptid = c.deptid_chr
                           and b.recorddate between ? and ? ";

                parm = svc.CreateParm(2);
                parm[0].Value = beginDate + " 00:00:00";
                parm[1].Value = endDate + " 23:59:59";
                DataTable dt = svc.GetDataTable(Sql, parm);

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        string deptName = dr["deptname_vchr"].ToString();
                        int typeInt = Function.Int(dr["typeint"]);
                        string deptId = dr["deptid_chr"].ToString();

                        if (data.Any(t => t.deptName == deptName))
                        {
                            int index = data.FindIndex(t => t.deptName == deptName);

                            if(data[index].lstType.Any(t => t.typeInt == typeInt))
                            {
                                int index2 = data[index].lstType.FindIndex(t => t.typeInt == typeInt);
                                data[index].lstType[index2].Count++;
                            }
                            else
                            {
                                EntityTypeStat vo2 = new EntityTypeStat();
                                vo2.typeInt = typeInt;
                                vo2.Count = 1;
                                data[index].lstType.Add(vo2);
                            }
                        }
                        else
                        {
                            EntityAlertStat vo = new EntityAlertStat();
                            vo.lstType = new List<EntityTypeStat>();
                            vo.deptName = dr["deptname_vchr"].ToString().Trim();
                            EntityTypeStat vo1 = new EntityTypeStat();
                            vo1.typeInt = typeInt;
                            vo1.Count = 1;
                            vo.lstType.Add(vo1);
                            vo.XH = ++n;

                            data.Add(vo);
                        }
                    }

                    for (int i = 0; i<data.Count; i++)
                    {
                        #region 检验
                        if (data[i].lstType.Any(t => t.typeInt == 1))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 1);
                            data[i].lisInfo += "细菌培养+药敏：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 2))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 2);
                            data[i].lisInfo += "涂片找菌：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 3))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 3);
                            data[i].lisInfo += "尿液常格：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 4))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 4);
                            data[i].lisInfo += "降钙素原(PCT)：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 8))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 8);
                            data[i].lisInfo += "轮状病毒：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 9))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 9);
                            data[i].lisInfo += "快速C反应：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 15))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 15);
                            data[i].lisInfo += "脓液、分泌物：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 16))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 16);
                            data[i].lisInfo += "胸水、腹水、关节腔积液：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 17))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 17);
                            data[i].lisInfo += "脑脊液常规检查：" + data[i].lstType[index].Count + "\r\n";
                        }
                        #endregion

                        #region 医嘱
                        if (data[i].lstType.Any(t => t.typeInt == 18))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 18);
                            data[i].lisInfo += "脑脊液生化：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 12))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 12);
                            data[i].orderInfo += "呼吸机辅助机：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 13))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 13);
                            data[i].orderInfo += "留置导尿：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 14))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 14);
                            data[i].orderInfo += "中心静脉穿刺管术：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 11))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 11);
                            data[i].orderInfo += "新开抗菌药：" + data[i].lstType[index].Count + "\r\n";
                        }
                        #endregion

                        #region 生命体征
                        if (data[i].lstType.Any(t => t.typeInt == 5))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 5);
                            data[i].orderInfo += "体温异常：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 7))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 7);
                            data[i].orderInfo += "疑似腹泻：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 19))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 19);
                            data[i].orderInfo += "再入院预警：" + data[i].lstType[index].Count + "\r\n";
                        }

                        if (data[i].lstType.Any(t => t.typeInt == 20))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 20);
                            data[i].orderInfo += "出院诊断预警：" + data[i].lstType[index].Count + "\r\n";
                        }
                        #endregion

                        #region 检查
                        if (data[i].lstType.Any(t => t.typeInt == 10))
                        {
                            int index = data[i].lstType.FindIndex(t => t.typeInt == 10);
                            data[i].orderInfo += "影象检查：" + data[i].lstType[index].Count + "\r\n";
                        }
                        #endregion
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

        #region 获取体温值
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetTempreture(string xmlIn, ref EntityAlert vo)
        {
            string value = null;
            string checkItemId = string.Empty;
            string strFile = xmlIn;

            try
            {
                System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                doc.LoadXml(strFile);

                XmlNodeList nodeList = doc.SelectSingleNode("Temperatures").ChildNodes;
                foreach (XmlNode xn in nodeList)
                {
                    XmlElement xe = (XmlElement)xn;

                    value = xe.Attributes["Value"].Value.ToString().Trim();
                    checkItemId = xe.Attributes["ModifyTime"].Value.ToString().Trim();
                    if (Function.Double(value) >= 38)
                    {
                        vo.itemResult = value;
                        vo.checkItemId = Function.Datetime(checkItemId).ToString("yyyyMMddHHmmss");
                    }
                }

                doc = null;
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                Log.Output("GetTempreture-->");
                Log.Output(strFile);
            }

            return value;
        }
        #endregion
        #endregion

        #region 手术风险评估统计报表
        /// <summary>
        /// 手术风险评估统计报表
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityRiskAna> getRiskAnaStat(List<EntityParm> dicParm)
        {
            List<EntityRiskAna> data = new List<EntityRiskAna>();
            int n = 0;
            string Sql = string.Empty;
            SqlHelper svc = null;
            try
            {
                #region Sql
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select 
                        extractvalue(b.xmldata, '/FormData/PatientDept') as KS,
                        extractvalue(b.xmldata, '/FormData/PatientName') as XM,
                        extractvalue(b.xmldata, '/FormData/PatientIpNo') as ZYH,
                        extractvalue(b.xmldata, '/FormData/X115') as SSMC,
                        extractvalue(b.xmldata, '/FormData/X116') as SSRQ,
                        extractvalue(b.xmldata, '/FormData/X016') as QKCD1, --I 类手术切口（清洁手术）
                        extractvalue(b.xmldata, '/FormData/X025') as QKCD2, --II 类手术切口（清洁-污染手术）
                        extractvalue(b.xmldata, '/FormData/X018') as QKCD3, --III 类手术切口（污染手术）
                        extractvalue(b.xmldata, '/FormData/X028') as QKCD4, --IV 手术切口（污秽-感染手术）
                        extractvalue(b.xmldata, '/FormData/X034') as SSLB1, --浅层组织手术
                        extractvalue(b.xmldata, '/FormData/X035') as SSLB2, --深部组织手术
                        extractvalue(b.xmldata, '/FormData/X036') as SSLB3, --器官手术
                        extractvalue(b.xmldata, '/FormData/X043') as MZFJ1, --P1
                        extractvalue(b.xmldata, '/FormData/X046') as MZFJ2, --P2
                        extractvalue(b.xmldata, '/FormData/X048') as MZFJ3, --P3
                        extractvalue(b.xmldata, '/FormData/X049') as MZFJ4, --P4
                        extractvalue(b.xmldata, '/FormData/X053') as MZFJ5, --P5
                        extractvalue(b.xmldata, '/FormData/X055') as MZFJ6, --P6
                        extractvalue(b.xmldata, '/FormData/X057') as SS3XSWC, --手术在3小时内完成 
                        extractvalue(b.xmldata, '/FormData/X058') as CG3XS, --超过3小时 
                        extractvalue(b.xmldata, '/FormData/B001') as JZSS, --急诊手术 
                        extractvalue(b.xmldata, '/FormData/X059') as SQYFYY, --术前30-60分钟预防用药 
                        extractvalue(b.xmldata, '/FormData/X060') as ZRW1, --植入物|是
                        extractvalue(b.xmldata, '/FormData/X062') as ZRW2, --植入物否
                        extractvalue(b.xmldata, '/FormData/X064') as KJYZL, --抗菌药物种类
                        extractvalue(b.xmldata, '/FormData/X068') as C3XSZJ1, --超3小时追加|是
                        extractvalue(b.xmldata, '/FormData/X069') as C3XSZJ2, --超3小时追加|否
                        extractvalue(b.xmldata, '/FormData/X071') as CXC1500_1, --出血超1500ml追加|是
                        extractvalue(b.xmldata, '/FormData/X072') as CXC1500_2, --出血超1500ml追加|否
                        extractvalue(b.xmldata, '/FormData/X087') as NNIS1, --NNIS分级|0
                        extractvalue(b.xmldata, '/FormData/X088') as NNIS2, --NNIS分级|1
                        extractvalue(b.xmldata, '/FormData/X089') as NNIS3, --NNIS分级|2
                        extractvalue(b.xmldata, '/FormData/X090') as NNIS4, --NNIS分级|3
                        extractvalue(b.xmldata, '/FormData/X098') as YH1, --愈合|甲
                        extractvalue(b.xmldata, '/FormData/X099') as YH2, --愈合|乙
                        extractvalue(b.xmldata, '/FormData/X100') as YH3, --愈合|丙
                        extractvalue(b.xmldata, '/FormData/X101') as YH4, --愈合|其他
                        extractvalue(b.xmldata, '/FormData/X103') as GR1, --感染|无
                        extractvalue(b.xmldata, '/FormData/X104') as GR2, --感染|切口浅部组织
                        extractvalue(b.xmldata, '/FormData/X105') as GR3, --感染|切口深部组织感染
                        extractvalue(b.xmldata, '/FormData/X106') as GR4 --感染|器官/腔隙感染
                        from icare.emrssfxpgd b 
                            inner join t_opr_bih_register c
                            on b.registerid = c.registerid_chr
                             and c.status_int = 1
                            inner join t_bse_deptdesc e
                            on c.areaid_chr = e.deptid_chr
                            where  ";
                #endregion

                #region 条件

                string strSub = string.Empty;
                List<IDataParameter> lstParm = new List<IDataParameter>();

                foreach (EntityParm po in dicParm)
                {
                    string keyValue = po.value;

                    switch (po.key)
                    {
                        case "reportDate":
                            IDataParameter parm1 = svc.CreateParm();
                            parm1.Value = keyValue.Split('|')[0] + "00:00:00";
                            lstParm.Add(parm1);
                            IDataParameter parm2 = svc.CreateParm();
                            parm2.Value = keyValue.Split('|')[1] + " 23:59:59";
                            lstParm.Add(parm2);
                            strSub += " b.recorddate between to_date(?,'yyyy-mm-dd hh24:mi:ss') and to_date(?,'yyyy-mm-dd hh24:mi:ss')";
                            break;
                        case "deptCode":
                            strSub += " and e.code_vchr = '" + keyValue + "'";
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                #region 赋值

                // 组合条件
                Sql += strSub;
                Sql += " order by b.recorddate";
                DataTable dt = svc.GetDataTable(Sql, lstParm.ToArray());
                if (dt != null)
                {
                    EntityRiskAna vo = null;
                    foreach (DataRow dr in dt.Rows)
                    {
                        vo = new EntityRiskAna();
                        vo.XH = ++n;
                        //科室
                        vo.KS = dr["KS"].ToString();
                        //姓名
                        vo.XM = dr["XM"].ToString();
                        //住院号
                        vo.ZYH = dr["ZYH"].ToString();
                        //手术名称
                        vo.SSMC = dr["SSMC"].ToString();
                        //手术日期
                        vo.SSRQ = dr["SSRQ"].ToString();
                        //切口清洁程度
                        if (dr["QKCD1"].ToString() == "1")
                            vo.QKQJCD = "I 类手术切口（清洁手术）";
                        else if (dr["QKCD1"].ToString() == "1")
                            vo.QKQJCD = "II 类手术切口（清洁-污染手术）";
                        else if (dr["QKCD1"].ToString() == "1")
                            vo.QKQJCD = "III 类手术切口（污染手术）";
                        else if (dr["QKCD4"].ToString() == "1")
                            vo.QKQJCD = "IV 手术切口（污秽-感染手术)";

                        //手术类别
                        if (dr["SSLB1"].ToString() == "1")
                            vo.SSLB = "浅层组织手术";
                        else if (dr["SSLB2"].ToString() == "1")
                            vo.SSLB = "深部组织手术";
                        else if (dr["SSLB3"].ToString() == "1")
                            vo.SSLB = "器官手术";

                        //麻醉分级
                        if (dr["MZFJ1"].ToString() == "1")
                            vo.MZFJ = "P1";
                        else if (dr["MZFJ2"].ToString() == "1")
                            vo.MZFJ = "P2";
                        else if (dr["MZFJ3"].ToString() == "1")
                            vo.MZFJ = "P3";
                        else if (dr["MZFJ4"].ToString() == "1")
                            vo.MZFJ = "P4";
                        else if (dr["MZFJ5"].ToString() == "1")
                            vo.MZFJ = "P5";
                        else if (dr["MZFJ6"].ToString() == "1")
                            vo.MZFJ = "P6";

                        //手术在3小时内完成
                        if (dr["SS3XSWC"].ToString() == "1")
                            vo.SS3XSWC = "是";
                        //超过3小时
                        if (dr["CG3XS"].ToString() == "1")
                            vo.CG3XS = "是";
                        //急诊手术
                        if (dr["JZSS"].ToString() == "1")
                            vo.JZSS = "是";
                        //术前30-60分钟预防用药
                        if (dr["SQYFYY"].ToString() == "1")
                            vo.SQYFYY = "是";
                        //植入物
                        if (dr["ZRW1"].ToString() == "1")
                            vo.ZRW = "是";
                        else if (dr["ZRW2"].ToString() == "1")
                            vo.ZRW = "否";
                        //抗菌药物种类
                        vo.KJYZL = dr["KJYZL"].ToString();
                        //超3小时追加
                        if (dr["C3XSZJ1"].ToString() == "1")
                            vo.C3XSZJ = "是";
                        else if (dr["C3XSZJ2"].ToString() == "1")
                            vo.C3XSZJ = "否";
                        //出血超1500ml追加
                        if (dr["CXC1500_1"].ToString() == "1")
                            vo.CXC1500 = "是";
                        else if (dr["CXC1500_1"].ToString() == "1")
                            vo.CXC1500 = "否";
                        //NNIS分级
                        if (dr["NNIS1"].ToString() == "1")
                            vo.NNIS = "0";
                        else if (dr["NNIS2"].ToString() == "1")
                            vo.NNIS = "1";
                        else if (dr["NNIS3"].ToString() == "1")
                            vo.NNIS = "2";
                        else if (dr["NNIS4"].ToString() == "1")
                            vo.NNIS = "3";

                        //愈合
                        if (dr["YH1"].ToString() == "1")
                            vo.YH = "甲";
                        else if (dr["YH2"].ToString() == "1")
                            vo.YH = "乙";
                        else if (dr["YH3"].ToString() == "1")
                            vo.YH = "丙";
                        else if (dr["YH4"].ToString() == "1")
                            vo.YH = "其他";

                        //感染
                        if (dr["GR1"].ToString() == "1")
                            vo.GR = "无";
                        else if (dr["GR2"].ToString() == "1")
                            vo.GR = "切口浅部组织";
                        else if (dr["GR3"].ToString() == "1")
                            vo.GR = "切口深部组织感染";
                        else if (dr["GR4"].ToString() == "1")
                            vo.GR = "器官/腔隙感染";
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

        #region 获取住院科室
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        #endregion

        #region  药占比报表

        #region  获取参数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetSysParamStr(string param)
        {
            string Sql = string.Empty;
            string sysParamStr = string.Empty;
            IDataParameter[] parm = null;
            SqlHelper svc = null;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                Sql = @"select parmvalue_vchr from t_bse_sysparm  where status_int = 1 and parmcode_chr = ? ";

                parm = svc.CreateParm(1);
                parm[0].Value = param;
                DataTable dt = svc.GetDataTable(Sql, parm);

                if (dt != null && dt.Rows.Count > 0)
                {
                    sysParamStr = dt.Rows[0]["parmvalue_vchr"].ToString();
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
            return sysParamStr;
        }
        #endregion
        
        #region 获取住院数据
        /// <summary>
        /// 获取住院数据
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetZyRptYzb(Dictionary<string, string> dicParm,int statFlg)
        {
            List<EntityRptYzb> data = new List<EntityRptYzb>();
            List<EntityRptYzb> xyData = new List<EntityRptYzb>();
            List<EntityRptYzb> kjData = new List<EntityRptYzb>();
            List<EntityRptYzb> clData = new List<EntityRptYzb>();
            List<EntityRptYzb> jyData = new List<EntityRptYzb>();
            int n = 0;
            string beginDate = string.Empty;
            string endDate = string.Empty;
            string parmKjStr = string.Empty;
            string parmJyStr = string.Empty;

            try 
            {
                if (dicParm.ContainsKey("reportDate"))
                {
                    beginDate = dicParm["reportDate"].Split('|')[0] + " 00:00:00";
                    endDate = dicParm["reportDate"].Split('|')[1] + " 23:59:59";
                }
                
                data = GetZyRptZsr(beginDate, endDate, statFlg);
                xyData = GetZyRptXy(beginDate, endDate, statFlg);
                clData = GetZyRptCl(beginDate, endDate, statFlg,ref data);
                if (dicParm.ContainsKey("JyStr"))
                {
                    parmKjStr = dicParm["JyStr"];
                    jyData = GetZyRptJy(parmKjStr, beginDate, endDate, statFlg);
                }

                #region 计算

                if (data.Count > 0)
                {
                    if(statFlg == 0)
                    {
                        for (int dataI = 0; dataI < data.Count; dataI++)
                        {
                            data[dataI].XH = ++n;
                            #region 西药
                            if (xyData.Count > 0)
                            {
                                for (int xyI = 0; xyI < xyData.Count; xyI++)
                                {
                                    if (data[dataI].deptCode == xyData[xyI].deptCode)
                                    {
                                        data[dataI].YBSR = xyData[xyI].YBSR;
                                    }
                                }
                            }
                            #endregion

                            #region 材料
                            if (clData.Count > 0)
                            {
                                for (int clI = 0; clI < clData.Count; clI++)
                                {
                                    if (data[dataI].deptCode == clData[clI].deptCode)
                                    {
                                        data[dataI].CLSR = clData[clI].CLSR;
                                    }
                                }
                            }
                            #endregion

                            #region 基本药
                            if (jyData.Count > 0)
                            {
                                for (int jyI = 0; jyI < jyData.Count; jyI++)
                                {
                                    if (data[dataI].deptCode == jyData[jyI].deptCode)
                                    {
                                        data[dataI].JBYWSR = jyData[jyI].JBYWSR;
                                    }
                                }
                            }
                            #endregion
                        }

                        #region 比例
                        for (int dataI = 0; dataI < data.Count; dataI++)
                        {
                            decimal zsr = Function.Dec(data[dataI].ZSR);
                            decimal xysr = data[dataI].YBSR;
                            if (data[dataI].YBSR > 0)
                            {
                                decimal ybsr = Function.Dec(data[dataI].YBSR);
                                data[dataI].YB = Function.Round(((double)ybsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].CLSR > 0)
                            {
                                decimal clsr = Function.Dec(data[dataI].CLSR);
                                data[dataI].CLZB = Function.Round(((double)clsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].KJYWSR > 0)
                            {
                                decimal kjywsr = Function.Dec(data[dataI].KJYWSR);
                                data[dataI].KJYWSYB = Function.Round(((double)kjywsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].JBYWSR > 0)
                            {
                                decimal jbywsr = Function.Dec(data[dataI].JBYWSR);
                                data[dataI].JBYWSRB = Function.Round(((double)jbywsr / (double)xysr) * 100, 1).ToString() + "%";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        for (int dataI = 0; dataI < data.Count; dataI++)
                        {
                            data[dataI].XH = ++n;
                            #region 西药
                            if (xyData.Count > 0)
                            {
                                for (int xyI = 0; xyI < xyData.Count; xyI++)
                                {
                                    if (data[dataI].GH == xyData[xyI].GH)
                                    {
                                        data[dataI].YBSR = xyData[xyI].YBSR;
                                    }
                                }
                            }
                            #endregion

                            #region 材料
                            if (clData.Count > 0)
                            {
                                for (int clI = 0; clI < clData.Count; clI++)
                                {
                                    if (data[dataI].GH.Trim() == clData[clI].GH.Trim())
                                    {
                                        data[dataI].CLSR = clData[clI].CLSR;
                                    }
                                }
                            }
                            #endregion

                            #region 基本药
                            if (jyData.Count > 0)
                            {
                                for (int jyI = 0; jyI < jyData.Count; jyI++)
                                {
                                    if (data[dataI].GH == jyData[jyI].GH)
                                    {
                                        data[dataI].JBYWSR = jyData[jyI].JBYWSR;
                                    }
                                }
                            }
                            #endregion
                        }

                        #region 比例
                        for (int dataI = 0; dataI < data.Count; dataI++)
                        {
                            decimal zsr = Function.Dec(data[dataI].ZSR);
                            decimal xysr = data[dataI].YBSR;
                            if (data[dataI].YBSR > 0)
                            {
                                decimal ybsr = Function.Dec(data[dataI].YBSR);
                                data[dataI].YB = Function.Round(((double)ybsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].CLSR > 0)
                            {
                                decimal clsr = Function.Dec(data[dataI].CLSR);
                                data[dataI].CLZB = Function.Round(((double)clsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].JBYWSR > 0)
                            {
                                decimal jbywsr = Function.Dec(data[dataI].JBYWSR);
                                data[dataI].JBYWSRB = Function.Round(((double)jbywsr / (double)xysr) * 100, 1).ToString() + "%";
                            }
                        }
                        #endregion
                    }
                }

                #endregion
            }
            catch(Exception e)
            {
                ExceptionLog.OutPutException(e);
            }
            finally
            {
            }
            
            return data;
        }

        #endregion

        #region 住院医生总收入
        /// <summary>
        /// 住院医生总收入
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetZyRptZsr(string beginDate, string endDate,int statFlg)
        {
            List<EntityRptYzb> data = new List<EntityRptYzb>();
            SqlHelper svc = null;

            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
              
                string Sql = @"select * from (select nvl(c.chargedoctorid_chr, 'a99999') as doctid,
                                    e.empno_chr,
                                    e.lastname_vchr,
                                    d.code_vchr,
                                    d.deptname_vchr,
                                    g.typeid_chr,
                                    g.typename_vchr,
                                    round(sum(round(c.unitprice_dec * c.amount_dec, 2) +
                                    nvl(c.totaldiffcostmoney_dec, 0)),2) as totalsum
                                    from t_opr_bih_register      a,
                                         t_opr_bih_patientcharge c,
                                         t_bse_chargeitem        f,
                                         t_bse_chargeitemextype  g,
                                         t_bse_employee          e,
                                         t_bse_deptdesc          d
                                  where a.registerid_chr = c.registerid_chr
                                        and c.chargedoctorid_chr = e.empid_chr
                                        and c.createarea_chr = d.deptid_chr
                                        and c.chargeitemid_chr = f.itemid_chr
                                        and f.itemipcalctype_chr = g.typeid_chr
                                        and a.status_int = 1
                                        and c.status_int = 1
                                        and c.pstatus_int <> 0
                                        and g.typeid_chr <> '2023'
                                        and (c.chargeactive_dat between
                                             to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                             to_date(?, 'yyyy-mm-dd hh24:mi:ss'))
                                      group by c.chargedoctorid_chr,e.empno_chr,e.lastname_vchr,d.code_vchr,d.deptname_vchr, g.typeid_chr,
                                            g.typename_vchr ) order by deptname_vchr";

                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate;
                parm[1].Value = endDate;
                DataTable dtZsr = svc.GetDataTable(Sql, parm);

                if (dtZsr != null && dtZsr.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtZsr.Rows)
                    {
                        if (statFlg == 0)
                        {
                            string deptCode = dr["code_vchr"].ToString();

                            if (data.Any(t => t.deptCode == deptCode))
                            {
                                #region 累计

                                EntityRptYzb voClone = data.FirstOrDefault(t => t.deptCode == deptCode);
                                voClone.ZSR += Function.Dec(dr["totalsum"]);

                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityRptYzb vo = new EntityRptYzb();
                                vo.deptCode = dr["code_vchr"].ToString();
                                vo.KS = dr["deptname_vchr"].ToString();
                                vo.ZSR = Function.Dec(dr["totalsum"]);

                                data.Add(vo);
                                #endregion
                            }
                        }
                        else
                        {
                            string gh = dr["empno_chr"].ToString();

                            if (string.IsNullOrEmpty(dr["lastname_vchr"].ToString().Trim()) || Function.Dec(dr["totalsum"]) <= 0)
                                continue;

                            if (data.Any(t => t.GH == gh))
                            {
                                #region 累计

                                EntityRptYzb voClone = data.FirstOrDefault(t => t.GH == gh);
                                voClone.ZSR += Function.Dec(dr["totalsum"]);

                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityRptYzb vo = new EntityRptYzb();
                                vo.GH = gh;
                                vo.XM = dr["lastname_vchr"].ToString();
                                vo.KS = dr["deptname_vchr"].ToString();
                                vo.ZSR = Function.Dec(dr["totalsum"]);

                                data.Add(vo);
                                #endregion
                            }
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

            return data;
        }

        #endregion

        #region 住院药品
        /// <summary>
        /// 药品
        /// </summary>
        /// <param name="parmStr"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetZyRptXy(string beginDate, string endDate,int statFlg)
        {
            List<EntityRptYzb> data = new List<EntityRptYzb>();
            DataTable dtXy = null;
            SqlHelper svc = null;

            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                
                string Sql = @" select nvl(c.chargedoctorid_chr, 'a99999') as doctid,
                                            e.empno_chr,
                                            e.lastname_vchr,
                                            d.code_vchr,
                                            d.deptname_vchr,
                                            round(sum(round(c.unitprice_dec * c.amount_dec, 2) +
                                                nvl(c.totaldiffcostmoney_dec, 0)),2) as totalsum
                                       from t_opr_bih_register      a,
                                            t_opr_bih_patientcharge c,
                                            t_bse_chargeitem        f,
                                            t_bse_chargeitemextype  g,
                                            t_bse_employee          e,
                                            t_bse_deptdesc          d
                                      where a.registerid_chr = c.registerid_chr
                                        and c.chargedoctorid_chr = e.empid_chr
                                        and c.createarea_chr = d.deptid_chr
                                        and c.chargeitemid_chr = f.itemid_chr
                                        and f.itemipcalctype_chr = g.typeid_chr
                                        and a.status_int = 1
                                        and c.status_int = 1
                                        and c.pstatus_int <> 0
                                        and g.typeid_chr in ('5034','2003','2022','3008','0001','1003','1005','5004','5036','2022',
                                    '3010','0002','1005','1007','2014','5006')
                                        and (c.chargeactive_dat between
                                            to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                            to_date(?, 'yyyy-mm-dd hh24:mi:ss'))
                                      group by c.chargedoctorid_chr,e.empno_chr,e.lastname_vchr,d.code_vchr,d.deptname_vchr ";

                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate;
                parm[1].Value = endDate;
                dtXy = svc.GetDataTable(Sql, parm);
                if (dtXy != null && dtXy.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtXy.Rows)
                    {
                        if(statFlg == 0)
                        {
                            string deptCode = dr["code_vchr"].ToString();

                            if (string.IsNullOrEmpty(dr["deptname_vchr"].ToString().Trim()) || Function.Dec(dr["totalsum"]) <= 0)
                                continue;

                            if (data.Any(t => t.deptCode == deptCode))
                            {
                                #region 累计

                                EntityRptYzb voClone = data.FirstOrDefault(t => t.deptCode == deptCode);
                                voClone.YBSR += Function.Dec(dr["totalsum"]);

                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityRptYzb vo = new EntityRptYzb();
                                vo.deptCode = deptCode;
                                vo.KS = dr["deptname_vchr"].ToString();
                                vo.YBSR = Function.Dec(dr["totalsum"]);

                                data.Add(vo);
                                #endregion
                            }
                        }
                        else
                        {
                            string gh = dr["empno_chr"].ToString();

                            if (string.IsNullOrEmpty(dr["lastname_vchr"].ToString().Trim()) || Function.Dec(dr["totalsum"]) <= 0)
                                continue;

                            if (data.Any(t => t.GH == gh))
                            {
                                #region 累计

                                EntityRptYzb voClone = data.FirstOrDefault(t => t.GH == gh);
                                voClone.YBSR += Function.Dec(dr["totalsum"]);

                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityRptYzb vo = new EntityRptYzb();
                                vo.GH = gh;
                                vo.XM = dr["lastname_vchr"].ToString();
                                vo.KS = dr["deptname_vchr"].ToString();
                                vo.YBSR = Function.Dec(dr["totalsum"]);

                                data.Add(vo);
                                #endregion
                            }
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

            return data;
        }
        #endregion

        #region 住院材料
        /// <summary>
        /// 材料
        /// </summary>
        /// <param name="parmStr"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetZyRptCl(string beginDate, string endDate,int statFlg,ref List<EntityRptYzb> zsr)
        {
            List<EntityRptYzb> data = new List<EntityRptYzb>();
            DataTable dtCl = null;
            SqlHelper svc = null;

            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                string Sql = @" select nvl(c.chargedoctorid_chr, 'a99999') as doctid,
                                e.empno_chr,
                                e.lastname_vchr,
                                d.code_vchr,
                                d.deptname_vchr,
                                f.code_vchr     as patDeptCode,
                                f.deptname_vchr as patDeptName,
                                tb.groupname_chr,
                                round(sum(round(c.unitprice_dec * c.amount_dec, 2) +
                                    nvl(c.totaldiffcostmoney_dec, 0)),2) as totalsum
                           from t_opr_bih_register      a,
                                t_opr_bih_patientcharge c,
                                (select b.groupid_chr, b.groupname_chr, c.typeid_chr
                                          from t_aid_rpt_def a, t_aid_rpt_gop_def b, t_aid_rpt_gop_rla c
                                         where a.rptid_chr = '0006'
                                           and a.rptid_chr = b.rptid_chr
                                           and b.rptid_chr = c.rptid_chr
                                           and b.groupid_chr = c.groupid_chr(+)) tb,
                                t_bse_employee          e,
                                t_bse_deptdesc          d,
                                t_bse_deptdesc          f
                          where a.registerid_chr = c.registerid_chr
                            and c.chargedoctorid_chr = e.empid_chr
                            and c.createarea_chr = d.deptid_chr
                            and c.CURAREAID_CHR = f.deptid_chr
                            and c.calccateid_chr = tb.typeid_chr(+)
                            and a.status_int = 1
                            and c.status_int = 1
                            and c.pstatus_int <> 0
                             and tb.groupid_chr = '0016'
                             and (c.chargeactive_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') 
                                   and to_date(?, 'yyyy-mm-dd hh24:mi:ss'))                                   
                          group by c.chargedoctorid_chr,
                                   e.empno_chr,
                                   e.lastname_vchr,
                                   d.code_vchr,
                                   d.deptname_vchr,f.code_vchr,f.deptname_vchr,tb.groupname_chr ";

                IDataParameter[] parm = null;
                parm = svc.CreateParm(2);
                parm[0].Value = beginDate;
                parm[1].Value = endDate;
                dtCl = svc.GetDataTable(Sql, parm);
                if (dtCl != null && dtCl.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCl.Rows)
                    {
                        if(statFlg == 0)
                        {
                            string deptCode = dr["code_vchr"].ToString();
                            string patDeptCode = dr["patDeptCode"].ToString();
                            if (string.IsNullOrEmpty(dr["deptname_vchr"].ToString().Trim()) || Function.Dec(dr["totalsum"]) <= 0)
                                continue;

                            if (data.Any(t => t.deptCode == deptCode))
                            {
                                #region 累计
                                if (deptCode == "2601")//开单是手术定
                                {
                                    deptCode = patDeptCode;
                                    EntityRptYzb voZsr = data.FirstOrDefault(t => t.deptCode == deptCode);
                                    voZsr.ZSR += Function.Dec(dr["totalsum"]);
                                }
                                EntityRptYzb voClone = data.FirstOrDefault(t => t.deptCode == deptCode);
                                voClone.CLSR += Function.Dec(dr["totalsum"]);

                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityRptYzb vo = new EntityRptYzb();
                                vo.deptCode = deptCode;
                                vo.KS = dr["deptname_vchr"].ToString();
                                vo.CLSR = Function.Dec(dr["totalsum"]);

                                data.Add(vo);
                                #endregion
                            }
                        }
                        else
                        {
                            string gh = dr["empno_chr"].ToString();

                            if (string.IsNullOrEmpty(dr["lastname_vchr"].ToString().Trim()) || Function.Dec(dr["totalsum"]) <= 0)
                                continue;

                            if (data.Any(t => t.GH == gh))
                            {
                                #region 累计

                                EntityRptYzb voClone = data.FirstOrDefault(t => t.GH == gh);
                                voClone.CLSR += Function.Dec(dr["totalsum"]);

                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityRptYzb vo = new EntityRptYzb();
                                vo.GH = gh;
                                vo.XM = dr["lastname_vchr"].ToString();
                                vo.KS = dr["deptname_vchr"].ToString();
                                vo.CLSR = Function.Dec(dr["totalsum"]);

                                data.Add(vo);
                                #endregion
                            }
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

            return data;
        }
        #endregion

        #region 住院基药收入
        /// <summary>
        /// 住院基药收入
        /// </summary>
        /// <param name="parmStr"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetZyRptJy(string parmStr, string beginDate, string endDate,int statFlg)
        {
            List<EntityRptYzb> data = new List<EntityRptYzb>();
            SqlHelper svc = null;

            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
                
                string Sql = @"select nvl(c.chargedoctorid_chr, 'a99999') as doctid,
                                e.empno_chr,
                                e.lastname_vchr,
                                d.code_vchr,
                                d.deptname_vchr,
                                round(sum(round(c.unitprice_dec * c.amount_dec, 2) +
                                    nvl(c.totaldiffcostmoney_dec, 0)),2) as totalsum
                           from t_opr_bih_register      a,
                                t_opr_bih_patientcharge c,
                                t_bse_medicine          f,
                                t_bse_chargeitem        g,
                                t_bse_medicine          b,
                                t_bse_employee          e,
                                t_bse_deptdesc          d
                          where a.registerid_chr = c.registerid_chr
                            and c.chargedoctorid_chr = e.empid_chr
                            and c.createarea_chr = d.deptid_chr
                            and c.chargeitemid_chr = g.itemid_chr
                            and g.itemsrcid_vchr = b.medicineid_chr
                            and g.itemsrcid_vchr = f.medicineid_chr
                            and f.inpinsurancetype_vchr in {0} 
                            and b.medicinetypeid_chr = 2
                            and a.status_int = 1
                            and c.status_int = 1
                            and c.pstatus_int <> 0
                             and (c.chargeactive_dat between to_date(?, 'yyyy-mm-dd hh24:mi:ss') 
                                   and to_date(?, 'yyyy-mm-dd hh24:mi:ss'))                                   
                          group by c.chargedoctorid_chr,
                                   e.empno_chr,
                                   e.lastname_vchr,
                                   d.code_vchr,
                                   d.deptname_vchr ";

                if (!string.IsNullOrEmpty(parmStr))
                {
                    Sql = string.Format(Sql, parmStr);
                    IDataParameter[] parm = null;
                    parm = svc.CreateParm(2);
                    parm[0].Value = beginDate;
                    parm[1].Value = endDate;
                    DataTable dtJy = svc.GetDataTable(Sql, parm);
                    if (dtJy != null && dtJy.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtJy.Rows)
                        {
                            if(statFlg == 0)
                            {
                                string deptCode = dr["code_vchr"].ToString();

                                if (data.Any(t => t.deptCode == deptCode))
                                {
                                    #region 累计

                                    EntityRptYzb voClone = data.FirstOrDefault(t => t.deptCode == deptCode);
                                    voClone.KS = dr["deptname_vchr"].ToString();
                                    voClone.JBYWSR += Function.Dec(dr["totalsum"]);

                                    #endregion
                                }
                                else
                                {
                                    #region vo
                                    EntityRptYzb vo = new EntityRptYzb();
                                    vo.deptCode = deptCode;
                                    vo.KS = dr["deptname_vchr"].ToString();
                                    vo.JBYWSR = Function.Dec(dr["totalsum"]);

                                    data.Add(vo);
                                    #endregion
                                }
                            }
                            else
                            {
                                string gh = dr["empno_chr"].ToString();

                                if (data.Any(t => t.GH == gh))
                                {
                                    #region 累计

                                    EntityRptYzb voClone = data.FirstOrDefault(t => t.GH == gh);
                                    voClone.KS = dr["deptname_vchr"].ToString();
                                    voClone.JBYWSR += Function.Dec(dr["totalsum"]);

                                    #endregion
                                }
                                else
                                {
                                    #region vo
                                    EntityRptYzb vo = new EntityRptYzb();
                                    vo.GH = gh;
                                    vo.XM = dr["lastname_vchr"].ToString();
                                    vo.KS = dr["deptname_vchr"].ToString();
                                    vo.JBYWSR = Function.Dec(dr["totalsum"]);

                                    data.Add(vo);
                                    #endregion
                                }
                            }
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

            return data;
        }
        #endregion

        #region 获取门诊数据
        /// <summary>
        /// 获取门诊数据
        /// </summary>
        /// <param name="dicParm"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetMzRptYzb(Dictionary<string, string> dicParm,int statFlg)
        {
            List<EntityRptYzb> data = new List<EntityRptYzb>();
            List<EntityRptYzb> xyData = new List<EntityRptYzb>();
            List<EntityRptYzb> kjData = new List<EntityRptYzb>();
            List<EntityRptYzb> clData = new List<EntityRptYzb>();
            List<EntityRptYzb> jyData = new List<EntityRptYzb>();
            string beginDate = string.Empty;
            string endDate = string.Empty;
            string parmKjStr = string.Empty;
            string parmJyStr = string.Empty;
            int n = 0;

            try
            {
                if (dicParm.ContainsKey("reportDate"))
                {
                    beginDate = dicParm["reportDate"].Split('|')[0] + " 00:00:00";
                    endDate = dicParm["reportDate"].Split('|')[1] + " 23:59:59";
                }

                data = GetMzRptZsr(beginDate, endDate, statFlg);

                //if (dicParm.ContainsKey("KangJunStr"))
                //{
                //    parmKjStr = dicParm["KangJunStr"];
                //    kjData = GetMzRptKj(parmKjStr, beginDate, endDate);
                //}

                xyData = GetMzRptXy(beginDate, endDate, statFlg);
                clData = GetMzRptCl(beginDate, endDate, statFlg);

                if (dicParm.ContainsKey("JyStr"))
                {
                    parmJyStr = dicParm["JyStr"];
                    jyData = GetMzRptJy(parmJyStr, beginDate, endDate, statFlg);
                }

                #region 计算

                if (data.Count > 0)
                {
                    if (statFlg == 0)
                    {
                        for (int dataI = 0; dataI < data.Count; dataI++)
                        {
                            data[dataI].XH = ++n;
                            #region 西药
                            if (xyData.Count > 0)
                            {
                                for (int xyI = 0; xyI < xyData.Count; xyI++)
                                {
                                    if (data[dataI].deptCode == xyData[xyI].deptCode && xyData[xyI].YBSR > 0)
                                    {
                                        data[dataI].YBSR = xyData[xyI].YBSR;
                                    }
                                }
                            }
                            #endregion

                            #region 材料
                            if (clData.Count > 0)
                            {
                                for (int clI = 0; clI < clData.Count; clI++)
                                {
                                    if (data[dataI].deptCode == clData[clI].deptCode && clData[clI].CLSR > 0)
                                    {
                                        data[dataI].CLSR = clData[clI].CLSR;
                                    }
                                }
                            }
                            #endregion

                            #region 抗菌药
                            if (kjData.Count > 0)
                            {
                                for (int kjI = 0; kjI < kjData.Count; kjI++)
                                {
                                    if (data[dataI].deptCode == kjData[kjI].deptCode && kjData[kjI].KJYWSR > 0)
                                    {
                                        data[dataI].KJYWSR = kjData[kjI].KJYWSR;
                                    }
                                }
                            }
                            #endregion

                            #region 基药
                            if (jyData.Count > 0)
                            {
                                for (int jyI = 0; jyI < jyData.Count; jyI++)
                                {
                                    if (data[dataI].deptCode == jyData[jyI].deptCode && jyData[jyI].JBYWSR > 0)
                                    {
                                        data[dataI].JBYWSR = jyData[jyI].JBYWSR;
                                    }
                                }
                            }
                            #endregion
                        }

                        #region  比例
                        for (int dataI = 0; dataI < data.Count; dataI++)
                        {
                            decimal zsr = Function.Dec(data[dataI].ZSR);
                            decimal ybsr = data[dataI].YBSR;

                            if (data[dataI].YBSR > 0)
                            {
                                data[dataI].YB = Function.Round(((double)ybsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].CLSR > 0)
                            {
                                decimal clsr = Function.Dec(data[dataI].CLSR);
                                data[dataI].CLZB = Function.Round(((double)clsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].KJYWSR > 0)
                            {
                                decimal kjywsr = Function.Dec(data[dataI].KJYWSR);
                                data[dataI].KJYWSYB = Function.Round(((double)kjywsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].JBYWSR > 0)
                            {
                                decimal jysr = Function.Dec(data[dataI].JBYWSR);
                                data[dataI].JBYWSRB = Function.Round(((double)jysr / (double)ybsr) * 100, 1).ToString() + "%";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        for (int dataI = 0; dataI < data.Count; dataI++)
                        {
                            #region 西药
                            if (xyData.Count > 0)
                            {
                                for (int xyI = 0; xyI < xyData.Count; xyI++)
                                {
                                    if (data[dataI].GH == xyData[xyI].GH && xyData[xyI].YBSR > 0)
                                    {
                                        data[dataI].YBSR = xyData[xyI].YBSR;
                                    }
                                }
                            }
                            #endregion

                            #region 材料
                            if (clData.Count > 0)
                            {
                                for (int clI = 0; clI < clData.Count; clI++)
                                {
                                    if (data[dataI].GH == clData[clI].GH && clData[clI].CLSR > 0)
                                    {
                                        data[dataI].CLSR = clData[clI].CLSR;
                                    }
                                }
                            }
                            #endregion

                            #region 抗菌药
                            if (kjData.Count > 0)
                            {
                                for (int kjI = 0; kjI < kjData.Count; kjI++)
                                {
                                    if (data[dataI].GH == kjData[kjI].GH && kjData[kjI].KJYWSR > 0)
                                    {
                                        data[dataI].KJYWSR = kjData[kjI].KJYWSR;
                                    }
                                }
                            }
                            #endregion

                            #region 基药
                            if (jyData.Count > 0)
                            {
                                for (int jyI = 0; jyI < jyData.Count; jyI++)
                                {
                                    if (data[dataI].GH == jyData[jyI].GH && jyData[jyI].JBYWSR > 0)
                                    {
                                        data[dataI].JBYWSR = jyData[jyI].JBYWSR;
                                    }
                                }
                            }
                            #endregion
                        }

                        #region 比例
                        data = data.OrderBy(t => t.KS).ToList();
                        for (int dataI = 0; dataI < data.Count; dataI++)
                        {
                            data[dataI].XH = ++n;
                            decimal zsr = data[dataI].ZSR;
                            decimal ybsr = data[dataI].YBSR;

                            if (data[dataI].YBSR > 0)
                            {
                                data[dataI].YB = Function.Round(((double)ybsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].CLSR > 0)
                            {
                                decimal clsr = data[dataI].CLSR;
                                data[dataI].CLZB = Function.Round(((double)clsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].KJYWSR > 0)
                            {
                                decimal kjywsr = data[dataI].KJYWSR;
                                data[dataI].KJYWSYB = Function.Round(((double)kjywsr / (double)zsr) * 100, 1).ToString() + "%";
                            }
                            if (data[dataI].JBYWSR > 0)
                            {
                                decimal jysr = data[dataI].JBYWSR;
                                data[dataI].JBYWSRB = Function.Round(((double)jysr / (double)ybsr) * 100, 1).ToString() + "%";
                            }
                        }
                        #endregion
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

            }

            return data;
        }

        #endregion

        #region 门诊药品
        /// <summary>
        /// 药品
        /// </summary>
        /// <param name="parmStr"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetMzRptXy(string beginDate, string endDate,int statFlg)
        {
            List<EntityRptYzb> data = new List<EntityRptYzb>();
            string beginDate1 = string.Empty;
            string endDate1 = string.Empty;
            DataTable dtXy = null;
            SqlHelper svc = null;

            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                string Sql = @"select mm.typeid_chr,
                                       mm.typename_vchr,
                                       mm.doctorid_chr,
                                       mm.empno_chr,
                                       mm.lastname_vchr,
                                       mm.code_vchr,
                                       mm.deptname_vchr,
                                       (mm.tolfee_mny + nvl(tt.diffSum, 0)) tolfee_mny,
                                       mm.jxywl,
                                       tt.diffSum
                                  from (select g.typeid_chr,
                                               g.typename_vchr,
                                               a.doctorid_chr,
                                               e.empno_chr,
                                               e.lastname_vchr,
                                               e.code_vchr,
                                               e.deptname_vchr,
                                               sum(b.tolfee_mny) tolfee_mny,
                                               sum(b.tolfee_mny * f.percentage) jxywl
                                          from t_opr_outpatientrecipeinv a,
                                               t_opr_outpatientrecipesumde b,
                                               (select e.empid_chr,
                                                       e.empno_chr,
                                                       e.lastname_vchr,
                                                       r.deptid_chr,
                                                       d.code_vchr,
                                                       d.deptname_vchr
                                                  from t_bse_employee e, t_bse_deptemp r, t_bse_deptdesc d
                                                 where r.deptid_chr = d.deptid_chr
                                                   and e.empid_chr = r.empid_chr
                                                   and r.default_dept_int = 1
                                                union all
                                                select e2.empid_chr,
                                                       e2.empno_chr,
                                                       e2.lastname_vchr,
                                                       '' deptid_chr,
                                                       '' code_vchr,
                                                       '' deptname_vchr
                                                  from t_bse_employee e2
                                                 where not exists (select ''
                                                          from t_bse_deptemp r2
                                                         where r2.empid_chr = e2.empid_chr
                                                           and r2.default_dept_int = 1)) e,
                                               t_opr_drachformula f,
                                               t_bse_chargeitemextype g
                                         where a.seqid_chr = b.seqid_chr(+)
                                           and b.itemcatid_chr = g.typeid_chr
                                           and b.itemcatid_chr = f.typeid_chr(+)
                                           and g.flag_int = 1
                                           and a.doctorid_chr = e.empid_chr
                                           and (a.isvouchers_int <> 2 or a.isvouchers_int is null)
                                           and a.recorddate_dat between timestamp {0} and timestamp {1}
                                           and a.recorddate_dat between
                                               to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                               to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                         group by g.typeid_chr,
                                                  g.typename_vchr,
                                                  a.doctorid_chr,
                                                  e.empno_chr,
                                                  e.lastname_vchr,
                                                  e.code_vchr,
                                                  e.deptname_vchr) mm,
                                       (select ss.itcatid,
                                               ss.doctorid_chr,
                                               ss.empno_chr,
                                               ss.lastname_vchr,
                                               ss.code_vchr,
                                               ss.deptname_vchr,
                                               sum(ss.diffSum) diffSum
                                          from (select '1003' itcatid,
                                                       m.doctorid_chr,
                                                       e.empno_chr,
                                                       e.lastname_vchr,
                                                       e.code_vchr,
                                                       e.deptname_vchr,
                                                       sum(decode(m.STATUS_INT,
                                                                  0,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  2,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  nvl(n.toldiffprice_mny, 0))) diffSum
                                                  from t_opr_outpatientrecipeinv m,
                                                       t_opr_outpatientpwmrecipede n,
                                                       (select e.empid_chr,
                                                               e.empno_chr,
                                                               e.lastname_vchr,
                                                               r.deptid_chr,
                                                               d.code_vchr,
                                                               d.deptname_vchr
                                                          from t_bse_employee e,
                                                               t_bse_deptemp  r,
                                                               t_bse_deptdesc d
                                                         where r.deptid_chr = d.deptid_chr
                                                           and e.empid_chr = r.empid_chr
                                                           and r.default_dept_int = 1
                                                        union all
                                                        select e2.empid_chr,
                                                               e2.empno_chr,
                                                               e2.lastname_vchr,
                                                               '' deptid_chr,
                                                               '' code_vchr,
                                                               '' deptname_vchr
                                                          from t_bse_employee e2
                                                         where not exists
                                                         (select ''
                                                                  from t_bse_deptemp r2
                                                                 where r2.empid_chr = e2.empid_chr
                                                                   and r2.default_dept_int = 1)) e
                                                 where m.outpatrecipeid_chr = n.outpatrecipeid_chr
                                                   and m.doctorid_chr = e.empid_chr
                                                   and (m.isvouchers_int <> 2 or m.isvouchers_int is null)
                                                   and m.recorddate_dat between timestamp {0} and timestamp {1}
                                                   and m.recorddate_dat between
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                                 group by m.outpatrecipeid_chr,
                                                          e.empno_chr,
                                                          e.lastname_vchr,
                                                          e.code_vchr,
                                                          e.deptname_vchr,
                                                          m.doctorid_chr
                                                union all
                                                select '1006' itcatid,
                                                       m.doctorid_chr,
                                                       e.empno_chr,
                                                       e.lastname_vchr,
                                                       e.code_vchr,
                                                       e.deptname_vchr,
                                                       sum(decode(m.status_int,
                                                                  0,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  2,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  nvl(n.toldiffprice_mny, 0))) diffSum
                                                  from t_opr_outpatientrecipeinv m,
                                                       t_opr_outpatientcmrecipede n,
                                                       (select e.empid_chr,
                                                               e.empno_chr,
                                                               e.lastname_vchr,
                                                               r.deptid_chr,
                                                               d.code_vchr,
                                                               d.deptname_vchr
                                                          from t_bse_employee e,
                                                               t_bse_deptemp  r,
                                                               t_bse_deptdesc d
                                                         where r.deptid_chr = d.deptid_chr
                                                           and e.empid_chr = r.empid_chr
                                                           and r.default_dept_int = 1
                                                        union all
                                                        select e2.empid_chr,
                                                               e2.empno_chr,
                                                               e2.lastname_vchr,
                                                               '' deptid_chr,
                                                               '' code_vchr,
                                                               '' deptname_vchr
                                                          from t_bse_employee e2
                                                         where not exists
                                                         (select ''
                                                                  from t_bse_deptemp r2
                                                                 where r2.empid_chr = e2.empid_chr
                                                                   and r2.default_dept_int = 1)) e
                                                 where m.outpatrecipeid_chr = n.outpatrecipeid_chr
                                                   and m.doctorid_chr = e.empid_chr
                                                   and (m.isvouchers_int <> 2 or m.isvouchers_int is null)
                                                   and m.recorddate_dat between timestamp {0} and timestamp {1}
                                                   and m.recorddate_dat between
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                                 group by m.outpatrecipeid_chr,
                                                          m.doctorid_chr,
                                                          e.empno_chr,
                                                          e.lastname_vchr,
                                                          e.code_vchr,
                                                          e.deptname_vchr) ss
                                         group by ss.itcatid,
                                                  ss.doctorid_chr,
                                                  ss.empno_chr,
                                                  ss.lastname_vchr,
                                                  ss.code_vchr,
                                                  ss.deptname_vchr) tt
                                 where mm.typeid_chr = tt.itcatid(+)
                                   and mm.doctorid_chr = tt.doctorid_chr(+)
                                   and mm.code_vchr = tt.code_vchr(+)
                                   and mm.typeid_chr = '1003' ";

                beginDate1 = "'" + beginDate +"'";
                endDate1 = "'" + endDate + "'";
                Sql = string.Format(Sql,beginDate1,endDate1);

                IDataParameter[] parm = null;
                parm = svc.CreateParm(6);
                parm[0].Value = beginDate;
                parm[1].Value = endDate;
                parm[2].Value = beginDate;
                parm[3].Value = endDate;
                parm[4].Value = beginDate;
                parm[5].Value = endDate;
                dtXy = svc.GetDataTable(Sql, parm);
                if (dtXy != null && dtXy.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtXy.Rows)
                    {
                        string deptCode = dr["code_vchr"].ToString();
                        string deptName = dr["deptname_vchr"].ToString();
                        string gh = dr["empno_chr"].ToString();
                        if (string.IsNullOrEmpty(deptCode))
                        {
                            GetDept(gh, ref deptName, ref deptCode);
                        }

                        if (statFlg == 0)
                        {
                            if (data.Any(t => t.deptCode == deptCode))
                            {
                                #region 累计

                                EntityRptYzb voClone = data.FirstOrDefault(t => t.deptCode == deptCode);
                                voClone.YBSR += Function.Dec(dr["tolfee_mny"]);

                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityRptYzb vo = new EntityRptYzb();
                                vo.deptCode = deptCode;
                                vo.KS = deptName;
                                vo.YBSR = Function.Dec(dr["tolfee_mny"]);

                                data.Add(vo);
                                #endregion
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(dr["lastname_vchr"].ToString().Trim()) || Function.Dec(dr["tolfee_mny"]) <= 0)
                                continue;
                            if (data.Any(t => t.GH == gh))
                            {
                                #region 累计

                                EntityRptYzb voClone = data.FirstOrDefault(t => t.GH == gh);
                                voClone.YBSR += Function.Dec(dr["tolfee_mny"]);

                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityRptYzb vo = new EntityRptYzb();
                                vo.GH = gh;
                                vo.XM = dr["lastname_vchr"].ToString();
                                vo.KS = dr["deptname_vchr"].ToString();
                                vo.YBSR = Function.Dec(dr["tolfee_mny"]);

                                data.Add(vo);
                                #endregion
                            }
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

            return data;
        }

        #endregion

        #region 门诊材料
        /// <summary>
        /// 门诊材料
        /// </summary>
        /// <param name="parmStr"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetMzRptCl(string beginDate, string endDate,int statFlg)
        {
            List<EntityRptYzb> data = new List<EntityRptYzb>();
            string beginDate1 = string.Empty;
            string endDate1 = string.Empty;

            DataTable dtCl = null;
            SqlHelper svc = null;

            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                string Sql = @"select mm.typeid_chr,
                                       mm.typename_vchr,
                                       mm.doctorid_chr,
                                       mm.empno_chr,
                                       mm.lastname_vchr,
                                       mm.code_vchr,
                                       mm.deptname_vchr,
                                       (mm.tolfee_mny + nvl(tt.diffSum, 0)) tolfee_mny,
                                       mm.jxywl,
                                       tt.diffSum
                                  from (select g.typeid_chr,
                                               g.typename_vchr,
                                               a.doctorid_chr,
                                               e.empno_chr,
                                               e.lastname_vchr,
                                               e.code_vchr,
                                               e.deptname_vchr,
                                               sum(b.tolfee_mny) tolfee_mny,
                                               sum(b.tolfee_mny * f.percentage) jxywl
                                          from t_opr_outpatientrecipeinv a,
                                               t_opr_outpatientrecipesumde b,
                                               (select e.empid_chr,
                                                       e.empno_chr,
                                                       e.lastname_vchr,
                                                       r.deptid_chr,
                                                       d.code_vchr,
                                                       d.deptname_vchr
                                                  from t_bse_employee e, t_bse_deptemp r, t_bse_deptdesc d
                                                 where r.deptid_chr = d.deptid_chr
                                                   and e.empid_chr = r.empid_chr
                                                   and r.default_dept_int = 1
                                                union all
                                                select e2.empid_chr,
                                                       e2.empno_chr,
                                                       e2.lastname_vchr,
                                                       '' deptid_chr,
                                                       '' code_vchr,
                                                       '' deptname_vchr
                                                  from t_bse_employee e2
                                                 where not exists (select ''
                                                          from t_bse_deptemp r2
                                                         where r2.empid_chr = e2.empid_chr
                                                           and r2.default_dept_int = 1)) e,
                                               t_opr_drachformula f,
                                               t_bse_chargeitemextype g
                                         where a.seqid_chr = b.seqid_chr(+)
                                           and b.itemcatid_chr = g.typeid_chr
                                           and b.itemcatid_chr = f.typeid_chr(+)
                                           and g.flag_int = 1
                                           and a.doctorid_chr = e.empid_chr
                                           and (a.isvouchers_int <> 2 or a.isvouchers_int is null)
                                           and a.recorddate_dat between timestamp {0} and timestamp {1}
                                           and a.recorddate_dat between
                                               to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                               to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                         group by g.typeid_chr,
                                                  g.typename_vchr,
                                                  a.doctorid_chr,
                                                  e.empno_chr,
                                                  e.lastname_vchr,
                                                  e.code_vchr,
                                                  e.deptname_vchr) mm,
                                       (select ss.itcatid,
                                               ss.doctorid_chr,
                                               ss.empno_chr,
                                               ss.lastname_vchr,
                                               ss.code_vchr,
                                               ss.deptname_vchr,
                                               sum(ss.diffSum) diffSum
                                          from (select '1003' itcatid,
                                                       m.doctorid_chr,
                                                       e.empno_chr,
                                                       e.lastname_vchr,
                                                       e.code_vchr,
                                                       e.deptname_vchr,
                                                       sum(decode(m.STATUS_INT,
                                                                  0,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  2,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  nvl(n.toldiffprice_mny, 0))) diffSum
                                                  from t_opr_outpatientrecipeinv m,
                                                       t_opr_outpatientpwmrecipede n,
                                                       (select e.empid_chr,
                                                               e.empno_chr,
                                                               e.lastname_vchr,
                                                               r.deptid_chr,
                                                               d.code_vchr,
                                                               d.deptname_vchr
                                                          from t_bse_employee e,
                                                               t_bse_deptemp  r,
                                                               t_bse_deptdesc d
                                                         where r.deptid_chr = d.deptid_chr
                                                           and e.empid_chr = r.empid_chr
                                                           and r.default_dept_int = 1
                                                        union all
                                                        select e2.empid_chr,
                                                               e2.empno_chr,
                                                               e2.lastname_vchr,
                                                               '' deptid_chr,
                                                               '' code_vchr,
                                                               '' deptname_vchr
                                                          from t_bse_employee e2
                                                         where not exists
                                                         (select ''
                                                                  from t_bse_deptemp r2
                                                                 where r2.empid_chr = e2.empid_chr
                                                                   and r2.default_dept_int = 1)) e
                                                 where m.outpatrecipeid_chr = n.outpatrecipeid_chr
                                                   and m.doctorid_chr = e.empid_chr
                                                   and (m.isvouchers_int <> 2 or m.isvouchers_int is null)
                                                   and m.recorddate_dat between timestamp {0} and timestamp {1}
                                                   and m.recorddate_dat between
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                                 group by m.outpatrecipeid_chr,
                                                          e.empno_chr,
                                                          e.lastname_vchr,
                                                          e.code_vchr,
                                                          e.deptname_vchr,
                                                          m.doctorid_chr
                                                union all
                                                select '1006' itcatid,
                                                       m.doctorid_chr,
                                                       e.empno_chr,
                                                       e.lastname_vchr,
                                                       e.code_vchr,
                                                       e.deptname_vchr,
                                                       sum(decode(m.status_int,
                                                                  0,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  2,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  nvl(n.toldiffprice_mny, 0))) diffSum
                                                  from t_opr_outpatientrecipeinv m,
                                                       t_opr_outpatientcmrecipede n,
                                                       (select e.empid_chr,
                                                               e.empno_chr,
                                                               e.lastname_vchr,
                                                               r.deptid_chr,
                                                               d.code_vchr,
                                                               d.deptname_vchr
                                                          from t_bse_employee e,
                                                               t_bse_deptemp  r,
                                                               t_bse_deptdesc d
                                                         where r.deptid_chr = d.deptid_chr
                                                           and e.empid_chr = r.empid_chr
                                                           and r.default_dept_int = 1
                                                        union all
                                                        select e2.empid_chr,
                                                               e2.empno_chr,
                                                               e2.lastname_vchr,
                                                               '' deptid_chr,
                                                               '' code_vchr,
                                                               '' deptname_vchr
                                                          from t_bse_employee e2
                                                         where not exists
                                                         (select ''
                                                                  from t_bse_deptemp r2
                                                                 where r2.empid_chr = e2.empid_chr
                                                                   and r2.default_dept_int = 1)) e
                                                 where m.outpatrecipeid_chr = n.outpatrecipeid_chr
                                                   and m.doctorid_chr = e.empid_chr
                                                   and (m.isvouchers_int <> 2 or m.isvouchers_int is null)
                                                   and m.recorddate_dat between timestamp {0} and timestamp {1}
                                                   and m.recorddate_dat between
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                                 group by m.outpatrecipeid_chr,
                                                          m.doctorid_chr,
                                                          e.empno_chr,
                                                          e.lastname_vchr,
                                                          e.code_vchr,
                                                          e.deptname_vchr) ss
                                         group by ss.itcatid,
                                                  ss.doctorid_chr,
                                                  ss.empno_chr,
                                                  ss.lastname_vchr,
                                                  ss.code_vchr,
                                                  ss.deptname_vchr) tt
                                 where mm.typeid_chr = tt.itcatid(+)
                                   and mm.doctorid_chr = tt.doctorid_chr(+)
                                   and mm.code_vchr = tt.code_vchr(+)
                                   and mm.typeid_chr = '1026' ";

                beginDate1 = "'" + beginDate + "'";
                endDate1 = "'" + endDate + "'";
                Sql = string.Format(Sql,beginDate1,endDate1);

                IDataParameter[] parm = null;
                parm = svc.CreateParm(6);
                parm[0].Value = beginDate;
                parm[1].Value = endDate;
                parm[2].Value = beginDate;
                parm[3].Value = endDate;
                parm[4].Value = beginDate;
                parm[5].Value = endDate;
                dtCl = svc.GetDataTable(Sql, parm);
                if (dtCl != null && dtCl.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCl.Rows)
                    {
                        string deptCode = dr["code_vchr"].ToString();
                        string deptName = dr["deptname_vchr"].ToString();
                        string gh = dr["empno_chr"].ToString();
                        if (string.IsNullOrEmpty(deptCode))
                        {
                            GetDept(gh, ref deptName, ref deptCode);
                        }

                       if(statFlg == 0)
                       {
                           if (data.Any(t => t.deptCode == deptCode))
                           {
                               #region 累计

                               EntityRptYzb voClone = data.FirstOrDefault(t => t.deptCode == deptCode);
                               voClone.CLSR += Function.Dec(dr["tolfee_mny"]);

                               #endregion
                           }
                           else
                           {
                               #region vo
                               EntityRptYzb vo = new EntityRptYzb();
                               vo.deptCode = deptCode;
                               vo.KS = deptName;
                               vo.CLSR = Function.Dec(dr["tolfee_mny"]);

                               data.Add(vo);
                               #endregion
                           }
                       }
                       else
                       {
                           if (string.IsNullOrEmpty(dr["lastname_vchr"].ToString().Trim()) || Function.Dec(dr["tolfee_mny"]) <= 0)
                               continue;

                           if (data.Any(t => t.GH == gh))
                           {
                               #region 累计

                               EntityRptYzb voClone = data.FirstOrDefault(t => t.GH == gh);
                               voClone.CLSR += Function.Dec(dr["tolfee_mny"]);

                               #endregion
                           }
                           else
                           {
                               #region vo
                               EntityRptYzb vo = new EntityRptYzb();
                               vo.GH = gh;
                               vo.XM = dr["lastname_vchr"].ToString();
                               vo.KS = dr["deptname_vchr"].ToString();
                               vo.CLSR = Function.Dec(dr["tolfee_mny"]);

                               data.Add(vo);
                               #endregion
                           }
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

            return data;
        }
        #endregion

        #region 门诊总收入
        /// <summary>
        /// 门诊总收入
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetMzRptZsr(string beginDate, string endDate,int statFlg)
        {
            List<EntityRptYzb> data = new List<EntityRptYzb>();
            string beginDate1 = string.Empty;
            string endDate1 = string.Empty;
            DataTable dtZsr = null;
            SqlHelper svc = null;

            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);
              
                string Sql = @"select mm.typeid_chr,
                                       mm.typename_vchr,
                                       mm.doctorid_chr,
                                       mm.empno_chr,
                                       mm.lastname_vchr,
                                       mm.code_vchr,
                                       mm.deptname_vchr,
                                       (mm.tolfee_mny + nvl(tt.diffSum, 0)) tolfee_mny,
                                       mm.jxywl,
                                       tt.diffSum
                                  from (select g.typeid_chr,
                                               g.typename_vchr,
                                               a.doctorid_chr,
                                               e.empno_chr,
                                               e.lastname_vchr,
                                               e.code_vchr,
                                               e.deptname_vchr,
                                               sum(b.tolfee_mny) tolfee_mny,
                                               sum(b.tolfee_mny * f.percentage) jxywl
                                          from t_opr_outpatientrecipeinv a,
                                               t_opr_outpatientrecipesumde b,
                                               (select e.empid_chr,
                                                       e.empno_chr,
                                                       e.lastname_vchr,
                                                       r.deptid_chr,
                                                       d.code_vchr,
                                                       d.deptname_vchr
                                                  from t_bse_employee e, t_bse_deptemp r, t_bse_deptdesc d
                                                 where r.deptid_chr = d.deptid_chr
                                                   and e.empid_chr = r.empid_chr
                                                   and r.default_dept_int = 1
                                                union all
                                                select e2.empid_chr,
                                                       e2.empno_chr,
                                                       e2.lastname_vchr,
                                                       '' deptid_chr,
                                                       '' code_vchr,
                                                       '' deptname_vchr
                                                  from t_bse_employee e2
                                                 where not exists (select ''
                                                          from t_bse_deptemp r2
                                                         where r2.empid_chr = e2.empid_chr
                                                        and r2.default_dept_int = 1
                                                        )) e,
                                               t_opr_drachformula f,
                                               t_bse_chargeitemextype g
                                         where a.seqid_chr = b.seqid_chr(+)
                                           and b.itemcatid_chr = g.typeid_chr
                                           and b.itemcatid_chr = f.typeid_chr(+)
                                           and g.flag_int = 1
                                           and a.doctorid_chr = e.empid_chr
                                           and (a.isvouchers_int <> 2 or a.isvouchers_int is null)
                                           and a.recorddate_dat between timestamp {0} and timestamp {1}
                                           and a.recorddate_dat between
                                               to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                               to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                         group by g.typeid_chr,
                                                  g.typename_vchr,
                                                  a.doctorid_chr,
                                                  e.empno_chr,
                                                  e.lastname_vchr,
                                                  e.code_vchr,
                                                  e.deptname_vchr) mm,
       
                                       (select ss.itcatid,
                                               ss.doctorid_chr,
                                               ss.empno_chr,
                                               ss.lastname_vchr,
                                               ss.code_vchr,
                                               ss.deptname_vchr,
                                               sum(ss.diffSum) diffSum
                                          from (select '1003' itcatid,
                                                       m.doctorid_chr,
                                                       e.empno_chr,
                                                       e.lastname_vchr,
                                                       e.code_vchr,
                                                       e.deptname_vchr,
                                                       sum(decode(m.STATUS_INT,
                                                                  0,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  2,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  nvl(n.toldiffprice_mny, 0))) diffSum
                                                  from t_opr_outpatientrecipeinv m,
                                                       t_opr_outpatientpwmrecipede n,
                                                       (select e.empid_chr,
                                                               e.empno_chr,
                                                               e.lastname_vchr,
                                                               r.deptid_chr,
                                                               d.code_vchr,
                                                               d.deptname_vchr
                                                          from t_bse_employee e,
                                                               t_bse_deptemp  r,
                                                               t_bse_deptdesc d
                                                         where r.deptid_chr = d.deptid_chr
                                                           and e.empid_chr = r.empid_chr
                                                           and r.default_dept_int = 1
                                                        union all
                                                        select e2.empid_chr,
                                                               e2.empno_chr,
                                                               e2.lastname_vchr,
                                                               '' deptid_chr,
                                                               '' code_vchr,
                                                               '' deptname_vchr
                                                          from t_bse_employee e2
                                                         where not exists (select ''
                                                                  from t_bse_deptemp r2
                                                                 where r2.empid_chr = e2.empid_chr
                                                                and r2.default_dept_int = 1
                                                                )) e
                                                 where m.outpatrecipeid_chr = n.outpatrecipeid_chr
                                                   and m.doctorid_chr = e.empid_chr
                                                   and (m.isvouchers_int <> 2 or m.isvouchers_int is null)
                                                   and m.recorddate_dat between timestamp {0} and timestamp {1}
                                                   and m.recorddate_dat between
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                                 group by m.outpatrecipeid_chr,
                                                          e.empno_chr,
                                                          e.lastname_vchr,
                                                          e.code_vchr,
                                                          e.deptname_vchr,
                                                          m.doctorid_chr
                                                union all
                                                select '1006' itcatid,
                                                       m.doctorid_chr,
                                                       e.empno_chr,
                                                       e.lastname_vchr,
                                                       e.code_vchr,
                                                       e.deptname_vchr,
                                                       sum(decode(m.status_int,
                                                                  0,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  2,
                                                                  (-1) * nvl(n.toldiffprice_mny, 0),
                                                                  nvl(n.toldiffprice_mny, 0))) diffSum
                                                  from t_opr_outpatientrecipeinv m,
                                                       t_opr_outpatientcmrecipede n,
                                                       (select e.empid_chr,
                                                               e.empno_chr,
                                                               e.lastname_vchr,
                                                               r.deptid_chr,
                                                               d.code_vchr,
                                                               d.deptname_vchr
                                                          from t_bse_employee e,
                                                               t_bse_deptemp  r,
                                                               t_bse_deptdesc d
                                                         where r.deptid_chr = d.deptid_chr
                                                           and e.empid_chr = r.empid_chr
                                                           and r.default_dept_int = 1
                                                        union all
                                                        select e2.empid_chr,
                                                               e2.empno_chr,
                                                               e2.lastname_vchr,
                                                               '' deptid_chr,
                                                               '' code_vchr,
                                                               '' deptname_vchr
                                                          from t_bse_employee e2
                                                         where not exists (select ''
                                                                  from t_bse_deptemp r2
                                                                 where r2.empid_chr = e2.empid_chr
                                                                and r2.default_dept_int = 1
                                                                )) e
                                                 where m.outpatrecipeid_chr = n.outpatrecipeid_chr
                                                   and m.doctorid_chr = e.empid_chr
                                                   and (m.isvouchers_int <> 2 or m.isvouchers_int is null)
                                                   and m.recorddate_dat between timestamp {0} and timestamp {1}
                                                   and m.recorddate_dat between
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                                 group by m.outpatrecipeid_chr,
                                                          m.doctorid_chr,
                                                          e.empno_chr,
                                                          e.lastname_vchr,
                                                          e.code_vchr,
                                                          e.deptname_vchr) ss
                                         group by ss.itcatid,
                                                  ss.doctorid_chr,
                                                  ss.empno_chr,
                                                  ss.lastname_vchr,
                                                  ss.code_vchr,
                                                  ss.deptname_vchr) tt
                                 where mm.typeid_chr = tt.itcatid(+)
                                   and mm.doctorid_chr = tt.doctorid_chr(+)
                                   and mm.code_vchr = tt.code_vchr(+)
                                   and mm.typeid_chr <> '1006'
                                ";
                beginDate1 = "'" + beginDate + "'";
                endDate1 = "'" + endDate + "'";
                Sql = string.Format(Sql, beginDate1,endDate1);

                IDataParameter[] parm = null;
                parm = svc.CreateParm(6);
                parm[0].Value =  beginDate ;
                parm[1].Value =  endDate ;
                parm[2].Value = beginDate;
                parm[3].Value = endDate;
                parm[4].Value = beginDate ;
                parm[5].Value = endDate ;

                dtZsr = svc.GetDataTable(Sql, parm);
                if (dtZsr != null && dtZsr.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtZsr.Rows)
                    {
                        string deptCode = dr["code_vchr"].ToString();
                        string deptName = dr["deptname_vchr"].ToString();
                        string gh = dr["empno_chr"].ToString();

                        if (string.IsNullOrEmpty(deptCode))
                            GetDept(gh, ref deptName, ref deptCode);

                        if(statFlg == 0)
                        {
                            if (string.IsNullOrEmpty(deptName) || Function.Dec(dr["tolfee_mny"]) <= 0)
                                continue;

                            if (data.Any(t => t.deptCode == deptCode))
                            {
                                #region 累计

                                EntityRptYzb voClone = data.FirstOrDefault(t => t.deptCode == deptCode);
                                voClone.KS = deptName;
                                voClone.ZSR += Function.Dec(dr["tolfee_mny"]);

                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityRptYzb vo = new EntityRptYzb();
                                vo.deptCode = deptCode;
                                vo.KS = dr["deptname_vchr"].ToString();
                                vo.ZSR = Function.Dec(dr["tolfee_mny"]);

                                data.Add(vo);
                                #endregion
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(deptName) || Function.Dec(dr["tolfee_mny"]) <= 0)
                                continue;

                            if (data.Any(t => t.GH == gh))
                            {
                                #region 累计

                                EntityRptYzb voClone = data.FirstOrDefault(t => t.GH == gh);
                                voClone.XM = dr["lastname_vchr"].ToString();
                                voClone.ZSR += Function.Dec(dr["tolfee_mny"]);

                                #endregion
                            }
                            else
                            {
                                #region vo
                                EntityRptYzb vo = new EntityRptYzb();
                                vo.GH = gh;
                                vo.XM = dr["lastname_vchr"].ToString();
                                vo.KS = deptName;
                                vo.ZSR = Function.Dec(dr["tolfee_mny"]);

                                data.Add(vo);
                                #endregion
                            }
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
            
            return data;
        }
        #endregion

        #region 门诊基药收入
        /// <summary>
        /// 门诊基药收入
        /// </summary>
        /// <param name="parmStr"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<EntityRptYzb> GetMzRptJy(string parmStr, string beginDate, string endDate,int statFlg)
        {
            List<EntityRptYzb> data = new List<EntityRptYzb>();
            string beginDate1 = string.Empty;
            string endDate1 = string.Empty;
            SqlHelper svc = null;

            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                string Sql = @"select g.lastname_vchr,
                                       c.diagdr_chr,
                                       g.empno_chr,
                                       e.code_vchr,
                                       e.deptname_vchr,
                                       round(sum(bb.tolprice_mny +
                                       decode(d.opchargeflg_int,
                                              0,
                                              cc.tradeprice_mny - cc.itemprice_mny,
                                              1,
                                              round((cc.tradeprice_mny - cc.itemprice_mny) /
                                                    d.packqty_dec,
                                                    4)) * bb.qty_dec),2) tolprice_mny
                                  from t_opr_outpatientrecipeinv a,
                                       t_opr_reciperelation b,
                                       t_opr_outpatientrecipe c,
                                       t_opr_oprecipeitemde bb,
                                       t_bse_chargeitem cc,
                                       t_bse_medicine d,
                                       t_bse_deptdesc f,
                                       t_bse_employee g,
                                       (select t1.empid_chr, t1.deptid_chr, t2.deptname_vchr,t2.code_vchr
                                          from t_bse_deptemp t1, t_bse_deptdesc t2
                                         where t1.default_dept_int = 1
                                           and t1.deptid_chr = t2.deptid_chr) e
                                 where a.outpatrecipeid_chr = b.seqid
                                   and b.outpatrecipeid_chr = c.outpatrecipeid_chr
                                   and c.outpatrecipeid_chr = bb.outpatrecipeid_chr
                                   and bb.itemid_chr = cc.itemid_chr
                                   and c.diagdr_chr = e.empid_chr(+)
                                   and c.diagdept_chr = f.deptid_chr
                                   and c.diagdr_chr = g.empid_chr
                                   and c.pstauts_int in (2, 3)
                                   and cc.itemsrcid_vchr = d.medicineid_chr
                                   and d.medicinetypeid_chr = 2
                                   and d.insurancetype_vchr in {0}
                                   and (a.isvouchers_int <> 2 or a.isvouchers_int is null)
                                   and a.recorddate_dat between timestamp {1} and timestamp {2}
                                   and a.recorddate_dat between
                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss') and
                                       to_date(?, 'yyyy-mm-dd hh24:mi:ss')
                                 group by g.lastname_vchr,g.empno_chr, c.diagdr_chr,e.code_vchr,e.deptname_vchr ";

                beginDate1 = "'" + beginDate + "'";
                endDate1 = "'" + endDate + "'";
                Sql = string.Format(Sql, parmStr,beginDate1, endDate1);

                if (!string.IsNullOrEmpty(parmStr))
                {
                    IDataParameter[] parm = null;
                    parm = svc.CreateParm(2);
                    parm[0].Value = beginDate;
                    parm[1].Value = endDate;
                    DataTable dtKjy = svc.GetDataTable(Sql, parm);
                    if (dtKjy != null && dtKjy.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtKjy.Rows)
                        {
                            string deptCode = dr["code_vchr"].ToString();
                            string deptName = dr["deptname_vchr"].ToString();
                            string gh = dr["empno_chr"].ToString();

                            if (string.IsNullOrEmpty(deptCode))
                                GetDept(gh, ref deptName, ref deptCode);

                            if(statFlg == 0)
                            {
                                if (data.Any(t => t.deptCode == deptCode))
                                {
                                    #region 累计

                                    EntityRptYzb voClone = data.FirstOrDefault(t => t.deptCode == deptCode);
                                    voClone.JBYWSR += Function.Dec(dr["tolprice_mny"]);

                                    #endregion
                                }
                                else
                                {
                                    #region vo
                                    EntityRptYzb vo = new EntityRptYzb();
                                    vo.deptCode = deptCode;
                                    vo.KS = deptName;
                                    vo.JBYWSR = Function.Dec(dr["tolprice_mny"]);

                                    data.Add(vo);
                                    #endregion
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(dr["lastname_vchr"].ToString().Trim()) || Function.Dec(dr["tolprice_mny"]) <= 0)
                                    continue;

                                if (data.Any(t => t.GH == gh))
                                {
                                    #region 累计

                                    EntityRptYzb voClone = data.FirstOrDefault(t => t.GH == gh);
                                    voClone.JBYWSR += Function.Dec(dr["tolprice_mny"]);

                                    #endregion
                                }
                                else
                                {
                                    #region vo
                                    EntityRptYzb vo = new EntityRptYzb();
                                    vo.GH = gh;
                                    vo.XM = dr["lastname_vchr"].ToString();
                                    vo.KS = dr["deptname_vchr"].ToString();
                                    vo.JBYWSR = Function.Dec(dr["tolprice_mny"]);

                                    data.Add(vo);
                                    #endregion
                                }
                            }
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

            return data;
        }
        #endregion

        #region 获取科室代码 名称
        /// <summary>
        /// 
        /// </summary>
        /// <param name="empno"></param>
        /// <param name="deptName"></param>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public int GetDept(string empno,ref string deptName,ref string deptCode)
        {
            DataTable dt = null;
            SqlHelper svc = null;
            int affect = 0;
            try
            {
                svc = new SqlHelper(EnumBiz.onlineDB);

                string Sql = @"select e.empid_chr,
                               e.empno_chr,
                               e.lastname_vchr,
                               r.deptid_chr,
                               d.code_vchr,
                               d.deptname_vchr
                          from t_bse_employee e,
                               t_bse_deptemp  r,
                               t_bse_deptdesc d
                         where r.deptid_chr = d.deptid_chr
                           and e.empid_chr = r.empid_chr
                           and e.empno_chr =  ?";

                IDataParameter[] parm = null;
                parm = svc.CreateParm(1);
                parm[0].Value = empno;

                dt = svc.GetDataTable(Sql, parm);
                if (dt != null && dt.Rows.Count > 0)
                {
                    deptName = dt.Rows[0]["deptname_vchr"].ToString();
                    deptCode = dt.Rows[0]["code_vchr"].ToString();
                }
            }
            catch (Exception e)
            {
                ExceptionLog.OutPutException(e);
                affect = -1;
            }
            finally
            {
                svc = null;
            }
            return affect;
        }

        #endregion

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
