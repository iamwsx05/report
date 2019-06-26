using Common.Controls;
using Common.Entity;
using Common.Utils;
using DevExpress.XtraReports.UI;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Windows.Forms;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Entity;

namespace Report.Ui
{
    /// <summary>
    /// 传染病控制类
    /// </summary>
    public class ctlContagionEdit : BaseController
    {
        #region Override

        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmContagionEdit Viewer = null;

        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmContagionEdit)child;
        }
        #endregion

        #region 变量.属性

        /// <summary>
        /// 显示VO
        /// </summary>
        internal EntityContagionDisplay ContagionDisplayVo { get; set; }

        /// <summary>
        /// 表单设计VO
        /// </summary>
        EntityFormDesign FormDesignVo = null;

        /// <summary>
        /// 表单ID
        /// </summary>
        decimal formId { get; set; }

        /// <summary>
        /// 参数
        /// </summary>
        List<EntityRptContagionParm> ContagionParmData { get; set; }

        #endregion

        #region 方法

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        internal void Init()
        {
            try
            {
                Viewer.Location = new Point(Viewer.Location.X, 0);
                Viewer.Height = Screen.PrimaryScreen.WorkingArea.Height;
                uiHelper.BeginLoading(Viewer);

                #region 参数
                using (ProxyEntityFactory proxy = new ProxyEntityFactory())
                {
                    ContagionParmData = EntityTools.ConvertToEntityList<EntityRptContagionParm>(proxy.Service.SelectFullTable(new EntityRptContagionParm()));
                }
                #endregion

                // 传染病报表
                if (ContagionParmData != null)
                {
                    if (ContagionParmData.Any(t => t.reportId == this.ContagionDisplayVo.reportId && t.keyId == "templateId"))
                    {
                        this.formId = Function.Dec(ContagionParmData.FirstOrDefault(t => t.reportId == this.ContagionDisplayVo.reportId && t.keyId == "templateId").keyValue);
                    }
                }
                if (this.formId > 0)
                {
                    using (ProxyFormDesign proxy = new ProxyFormDesign())
                    {
                        proxy.Service.GetForm((int)this.formId, out FormDesignVo);
                    }
                }
                if (FormDesignVo == null) FormDesignVo = new EntityFormDesign();
                if (Function.Dec(this.ContagionDisplayVo.rptId) > 0)
                {
                    using (ProxyContagion proxy = new ProxyContagion())
                    {
                        EntityRptContagion vo = proxy.Service.GetContagion(Function.Dec(this.ContagionDisplayVo.rptId));

                        #region patientInfo
                        Viewer.rdoFlag.SelectedIndex = vo.patType - 1;
                        Viewer.rdoFlag.Properties.ReadOnly = true;
                        Viewer.txtCardNo.Text = vo.patNo;
                        #endregion

                        LoadForm(FormDesignVo.Layout, vo.xmlData);
                        GetPatient();
                    }
                }
                else
                {
                    LoadForm(FormDesignVo.Layout, null);
                }
            }
            finally
            {
                uiHelper.CloseLoading(Viewer);
            }
        }
        #endregion

        #region GetPatient
        /// <summary>
        /// GetPatient
        /// </summary>
        internal void GetPatient()
        {
            string cardNo = Viewer.txtCardNo.Text.Trim();
            if (cardNo != string.Empty)
            {
                List<EntityPatientInfo> lstPat = null;
                List<EntityAidsCheck> lstPatLis = null;

                using (ProxyContagion proxy = new ProxyContagion())
                {
                    lstPat = proxy.Service.GetPatient(cardNo, Viewer.rdoFlag.SelectedIndex + 1);
                    lstPatLis = proxy.Service.GetPatLisInfo(cardNo, Viewer.rdoFlag.SelectedIndex + 1,this.formId);
                }
                if (lstPat == null || lstPat.Count == 0)
                {
                    DialogBox.Msg("查无此人.");
                    return;
                }
                if (lstPat.Count == 1)
                {
                    SetPatValue(lstPat[0]);
                }
                else if (lstPat.Count > 1)
                {
                    SetPatValue(lstPat[0]);
                }
                if (lstPatLis != null && lstPatLis.Count > 0)
                {
                    lstPat[0].clNo = lstPatLis[0].REQNO;
                    SetLisValue(lstPatLis[0]);
                }
            }
        }

        #region SetPatValue
        /// <summary>
        /// SetPatValue
        /// </summary>
        /// <param name="pat"></param>
        void SetPatValue(EntityPatientInfo pat)
        {
            Viewer.txtPatName.Tag = pat;
            Viewer.txtPatName.Text = pat.name;
            Viewer.txtSex.Text = pat.sexCH;
            Viewer.txtBirthday.Text = string.IsNullOrEmpty(pat.birth) ? "" : Function.Datetime(pat.birth).ToString("yyyy-MM-dd");
            Viewer.txtContactAddr.Text = pat.contAddr;

            #region 全局Patient.赋值

            GlobalPatient.currPatient = new EntityPatient();
            GlobalPatient.currPatient.PatientName = pat.name;
            GlobalPatient.currPatient.Sex = pat.sex;
            if (!string.IsNullOrEmpty(pat.birth)) GlobalPatient.currPatient.Birthday = Function.Datetime(pat.birth);
            GlobalPatient.currPatient.PatientIpNo = pat.ipNo;
            GlobalPatient.currPatient.IpTimes = pat.ipTimes;
            GlobalPatient.currPatient.Indate = pat.inDate;
            if (pat.inDate != null) GlobalPatient.currPatient.RegisterDate = pat.inDate.Value;
            GlobalPatient.currPatient.DeptName = pat.deptName;
            GlobalPatient.currPatient.DoctName = pat.doctName;
            GlobalPatient.currPatient.OutDate = pat.outDate;
            //GlobalPatient.currPatient.BirthPlace = Viewer.PatVo.birthPlace;
            GlobalPatient.currPatient.HomeAddr = pat.contAddr;
            GlobalPatient.currPatient.HomeTel = pat.contTel;
            //GlobalPatient.currPatient.ContactName = Viewer.PatVo.contactPerson;
            GlobalPatient.currPatient.IdCard = pat.ID;
            //GlobalPatient.currPatient.patHkadr = Viewer.PatVo.registerAddr;
            GlobalPatient.currPatient.PatientOpNo = pat.cardNo;

            GlobalPatient.currPatient.RegisterID = pat.pid;
            GlobalCase.caseInfo = new EntityCaseInfo();
            GlobalCase.caseInfo.CaseCode = this.ContagionDisplayVo.reportId;

            //Viewer.showPanelForm.RefreshPatInfo();

            #endregion
        }
        #endregion

        void SetLisValue(EntityAidsCheck vo)
        {
            #region //"3-II.艾滋病病毒感染孕产妇妊娠及所生婴儿登记卡"
            if (this.formId == 11 && vo != null)
            {
                if (vo.count == 2)
                {
                    if (!string.IsNullOrEmpty(vo.YZ))
                        Viewer.showPanelForm.SetFieldValue("A060", vo.YZ);
                    if (!string.IsNullOrEmpty(vo.BXBJS))
                        Viewer.showPanelForm.SetFieldValue("A061", vo.BXBJS);
                    if (!string.IsNullOrEmpty(vo.ZLBXBJS))
                        Viewer.showPanelForm.SetFieldValue("A062", vo.ZLBXBJS);
                    if (!string.IsNullOrEmpty(vo.XXBJS))
                        Viewer.showPanelForm.SetFieldValue("A063", vo.XXBJS);
                    if (!string.IsNullOrEmpty(vo.XHDB))
                        Viewer.showPanelForm.SetFieldValue("A064", vo.XHDB);
                    if (!string.IsNullOrEmpty(vo.XT))
                        Viewer.showPanelForm.SetFieldValue("A065", vo.XT);
                    if (!string.IsNullOrEmpty(vo.GBZAM))
                        Viewer.showPanelForm.SetFieldValue("A066", vo.GBZAM);
                    if (!string.IsNullOrEmpty(vo.GCZAM))
                        Viewer.showPanelForm.SetFieldValue("A067", vo.GCZAM);
                    if (!string.IsNullOrEmpty(vo.ZDHS))
                        Viewer.showPanelForm.SetFieldValue("A068", vo.ZDHS);
                    if (!string.IsNullOrEmpty(vo.XJG))
                        Viewer.showPanelForm.SetFieldValue("A069", vo.XJG);
                    if (!string.IsNullOrEmpty(vo.XNST))
                        Viewer.showPanelForm.SetFieldValue("A070", vo.XNST);
                    if (!string.IsNullOrEmpty(vo.CD4XBJS))
                        Viewer.showPanelForm.SetFieldValue("A071", vo.CD4XBJS);
                    if (!string.IsNullOrEmpty(vo.CD8XBJS))
                        Viewer.showPanelForm.SetFieldValue("A072", vo.CD8XBJS);
                    if (!string.IsNullOrEmpty(vo.BDZL))
                        Viewer.showPanelForm.SetFieldValue("A073", vo.BDZL);

                }
                else if (vo.count == 3)
                {
                    if (!string.IsNullOrEmpty(vo.YZ))
                        Viewer.showPanelForm.SetFieldValue("A080", vo.YZ);
                    if (!string.IsNullOrEmpty(vo.BXBJS))
                        Viewer.showPanelForm.SetFieldValue("A081", vo.BXBJS);
                    if (!string.IsNullOrEmpty(vo.ZLBXBJS))
                        Viewer.showPanelForm.SetFieldValue("A082", vo.ZLBXBJS);
                    if (!string.IsNullOrEmpty(vo.XXBJS))
                        Viewer.showPanelForm.SetFieldValue("A083", vo.XXBJS);
                    if (!string.IsNullOrEmpty(vo.XHDB))
                        Viewer.showPanelForm.SetFieldValue("A084", vo.XHDB);
                    if (!string.IsNullOrEmpty(vo.XT))
                        Viewer.showPanelForm.SetFieldValue("A085", vo.XT);
                    if (!string.IsNullOrEmpty(vo.GBZAM))
                        Viewer.showPanelForm.SetFieldValue("A086", vo.GBZAM);
                    if (!string.IsNullOrEmpty(vo.GCZAM))
                        Viewer.showPanelForm.SetFieldValue("A087", vo.GCZAM);
                    if (!string.IsNullOrEmpty(vo.ZDHS))
                        Viewer.showPanelForm.SetFieldValue("A088", vo.ZDHS);
                    if (!string.IsNullOrEmpty(vo.XJG))
                        Viewer.showPanelForm.SetFieldValue("A089", vo.XJG);
                    if (!string.IsNullOrEmpty(vo.XNST))
                        Viewer.showPanelForm.SetFieldValue("A090", vo.XNST);
                    if (!string.IsNullOrEmpty(vo.CD4XBJS))
                        Viewer.showPanelForm.SetFieldValue("A091", vo.CD4XBJS);
                    if (!string.IsNullOrEmpty(vo.CD8XBJS))
                        Viewer.showPanelForm.SetFieldValue("A092", vo.CD8XBJS);
                    if (!string.IsNullOrEmpty(vo.BDZL))
                        Viewer.showPanelForm.SetFieldValue("A093", vo.BDZL);
                }
                else if (vo.count == 4)
                {
                    if (!string.IsNullOrEmpty(vo.YZ))
                        Viewer.showPanelForm.SetFieldValue("A100", vo.YZ);
                    if (!string.IsNullOrEmpty(vo.BXBJS))
                        Viewer.showPanelForm.SetFieldValue("A101", vo.BXBJS);
                    if (!string.IsNullOrEmpty(vo.ZLBXBJS))
                        Viewer.showPanelForm.SetFieldValue("A102", vo.ZLBXBJS);
                    if (!string.IsNullOrEmpty(vo.XXBJS))
                        Viewer.showPanelForm.SetFieldValue("A103", vo.XXBJS);
                    if (!string.IsNullOrEmpty(vo.XHDB))
                        Viewer.showPanelForm.SetFieldValue("A104", vo.XHDB);
                    if (!string.IsNullOrEmpty(vo.XT))
                        Viewer.showPanelForm.SetFieldValue("A105", vo.XT);
                    if (!string.IsNullOrEmpty(vo.GBZAM))
                        Viewer.showPanelForm.SetFieldValue("A106", vo.GBZAM);
                    if (!string.IsNullOrEmpty(vo.GCZAM))
                        Viewer.showPanelForm.SetFieldValue("A107", vo.GCZAM);
                    if (!string.IsNullOrEmpty(vo.ZDHS))
                        Viewer.showPanelForm.SetFieldValue("A108", vo.ZDHS);
                    if (!string.IsNullOrEmpty(vo.XJG))
                        Viewer.showPanelForm.SetFieldValue("A109", vo.XJG);
                    if (!string.IsNullOrEmpty(vo.XNST))
                        Viewer.showPanelForm.SetFieldValue("A110", vo.XNST);
                    if (!string.IsNullOrEmpty(vo.CD4XBJS))
                        Viewer.showPanelForm.SetFieldValue("A111", vo.CD4XBJS);
                    if (!string.IsNullOrEmpty(vo.CD8XBJS))
                        Viewer.showPanelForm.SetFieldValue("A112", vo.CD8XBJS);
                    if (!string.IsNullOrEmpty(vo.BDZL))
                        Viewer.showPanelForm.SetFieldValue("A113", vo.BDZL);
                }
                //梅毒检测
                if (!string.IsNullOrEmpty(vo.MD_TPPA_ELISA))
                {
                    Viewer.showPanelForm.SetFieldValue("X940", "1");
                    if (vo.MD_TPPA_ELISA.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X945", "1");
                    else if (vo.MD_TPPA_ELISA.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X944", "1");
                }

                if (!string.IsNullOrEmpty(vo.MD_RPR_TRUST))
                {
                    Viewer.showPanelForm.SetFieldValue("X941", "1");
                    if (vo.MD_RPR_TRUST.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X949", "1");
                    else if (vo.MD_RPR_TRUST.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X948", "1");
                }

                if (!string.IsNullOrEmpty(vo.HBsAg))
                {
                    Viewer.showPanelForm.SetFieldValue("X963", "1");
                    if (vo.HBsAg.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X967", "1");
                    else if (vo.HBsAg.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X966", "1");
                }

                if (!string.IsNullOrEmpty(vo.HBeAg))
                {
                    Viewer.showPanelForm.SetFieldValue("X971", "1");
                    if (vo.HBeAg.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X975", "1");
                    else if (vo.HBeAg.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X974", "1");
                }

                if (!string.IsNullOrEmpty(vo.HCV_IGG))
                {
                    Viewer.showPanelForm.SetFieldValue("X979", "1");
                    if (vo.HCV_IGG.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X983", "1");
                    else if (vo.HCV_IGG.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X982", "1");
                }
                if (!string.IsNullOrEmpty(vo.HCV_IGM))
                {
                    Viewer.showPanelForm.SetFieldValue("X987", "1");
                    if (vo.HCV_IGM.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X991", "1");
                    else if (vo.HCV_IGM.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X990", "1");
                }

                if (vo.iDD > 0 && vo.iDD <= 8)
                    Viewer.showPanelForm.SetFieldValue("X953", "1");
                else if (vo.iDD > 8 && vo.iDD < 64)
                    Viewer.showPanelForm.SetFieldValue("X954", "1");
                else if (vo.iDD >= 64 && vo.iDD < 128)
                    Viewer.showPanelForm.SetFieldValue("X955", "1");
                else if (vo.iDD >= 128 && vo.iDD < 256)
                    Viewer.showPanelForm.SetFieldValue("X956", "1");
                else if (vo.iDD >= 256)
                    Viewer.showPanelForm.SetFieldValue("X957", "1");
            }
            else
            {
                if (!string.IsNullOrEmpty(vo.YZ))
                    Viewer.showPanelForm.SetFieldValue("A040", vo.YZ);
                if (!string.IsNullOrEmpty(vo.BXBJS))
                    Viewer.showPanelForm.SetFieldValue("A041", vo.BXBJS);
                if (!string.IsNullOrEmpty(vo.ZLBXBJS))
                    Viewer.showPanelForm.SetFieldValue("A042", vo.ZLBXBJS);
                if (!string.IsNullOrEmpty(vo.XXBJS))
                    Viewer.showPanelForm.SetFieldValue("A043", vo.XXBJS);
                if (!string.IsNullOrEmpty(vo.XHDB))
                    Viewer.showPanelForm.SetFieldValue("A044", vo.XHDB);
                if (!string.IsNullOrEmpty(vo.XT))
                    Viewer.showPanelForm.SetFieldValue("A045", vo.XT);
                if (!string.IsNullOrEmpty(vo.GBZAM))
                    Viewer.showPanelForm.SetFieldValue("A046", vo.GBZAM);
                if (!string.IsNullOrEmpty(vo.GCZAM))
                    Viewer.showPanelForm.SetFieldValue("A047", vo.GCZAM);
                if (!string.IsNullOrEmpty(vo.ZDHS))
                    Viewer.showPanelForm.SetFieldValue("A048", vo.ZDHS);
                if (!string.IsNullOrEmpty(vo.XJG))
                    Viewer.showPanelForm.SetFieldValue("A049", vo.XJG);
                if (!string.IsNullOrEmpty(vo.XNST))
                    Viewer.showPanelForm.SetFieldValue("A050", vo.XNST);
                if (!string.IsNullOrEmpty(vo.CD4XBJS))
                    Viewer.showPanelForm.SetFieldValue("A051", vo.CD4XBJS);
                if (!string.IsNullOrEmpty(vo.CD8XBJS))
                    Viewer.showPanelForm.SetFieldValue("A052", vo.CD8XBJS);
                if (!string.IsNullOrEmpty(vo.BDZL))
                    Viewer.showPanelForm.SetFieldValue("A053", vo.BDZL);

                //梅毒检测
                if (!string.IsNullOrEmpty(vo.MD_TPPA_ELISA))
                {
                    Viewer.showPanelForm.SetFieldValue("X940", "1");
                    if (vo.MD_TPPA_ELISA.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X945", "1");
                    else if (vo.MD_TPPA_ELISA.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X944", "1");
                }

                if (!string.IsNullOrEmpty(vo.MD_RPR_TRUST))
                {
                    Viewer.showPanelForm.SetFieldValue("X941", "1");
                    if (vo.MD_RPR_TRUST.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X949", "1");
                    else if (vo.MD_RPR_TRUST.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X948", "1");
                }

                if (!string.IsNullOrEmpty(vo.HBsAg))
                {
                    Viewer.showPanelForm.SetFieldValue("X963", "1");
                    if (vo.HBsAg.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X967", "1");
                    else if (vo.HBsAg.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X966", "1");
                }

                if (!string.IsNullOrEmpty(vo.HBeAg))
                {
                    Viewer.showPanelForm.SetFieldValue("X971", "1");
                    if (vo.HBeAg.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X975", "1");
                    else if (vo.HBeAg.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X974", "1");
                }

                if (!string.IsNullOrEmpty(vo.HCV_IGG))
                {
                    Viewer.showPanelForm.SetFieldValue("X979", "1");
                    if (vo.HCV_IGG.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X983", "1");
                    else if (vo.HCV_IGG.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X982", "1");
                }
                if (!string.IsNullOrEmpty(vo.HCV_IGM))
                {
                    Viewer.showPanelForm.SetFieldValue("X987", "1");
                    if (vo.HCV_IGM.Contains("阳性"))
                        Viewer.showPanelForm.SetFieldValue("X991", "1");
                    else if (vo.HCV_IGM.Contains("阴性"))
                        Viewer.showPanelForm.SetFieldValue("X990", "1");
                }

                if (vo.iDD > 0 && vo.iDD <= 8)
                    Viewer.showPanelForm.SetFieldValue("X953", "1");
                else if (vo.iDD > 8 && vo.iDD < 64)
                    Viewer.showPanelForm.SetFieldValue("X954", "1");
                else if (vo.iDD >= 64 && vo.iDD < 128)
                    Viewer.showPanelForm.SetFieldValue("X955", "1");
                else if (vo.iDD >= 128 && vo.iDD < 256)
                    Viewer.showPanelForm.SetFieldValue("X956", "1");
                else if (vo.iDD >= 256)
                    Viewer.showPanelForm.SetFieldValue("X957", "1");

            }                   
            #endregion 
            
            #region   //4-I梅毒感染孕产妇登记卡
            if (this.formId == 15)
            {
                //        4——I
                //快速血浆反应素环状片试验（RPR）X133 
                //阴性X134 
                //阳性X135cxxs
                //检测时间X137


                //甲苯胺红不加热血清试验（TRUST）X138
                //阴性X139
                //阳性X140
                //检测时间X142


                //梅毒螺旋体颗粒凝集试验（TPPA）X145
                //阴性X146
                //阳性X147
                //检测时间X149


                //酶联免疫吸附试验（ELISA）X150
                //阴性X151
                //阳性X152
                //检测时间X154


                //免疫层析法-快速体测（RT）X155
                //阴性X156
                //阳性X157
                //检测时间X159


                //梅毒螺旋体IgM抗体检测：
                //未检测X166
                //检测阳性X167
                //检测阴性X168
                //检测时间X169


                //暗视野显微镜梅毒螺旋体检测：
                //未检测X170
                //检测X171
                //（检测到梅毒螺旋体：
                //否X172
                //是X173
                //检测时间X174
            }
            #endregion

        }

        #endregion

        #region Clear
        /// <summary>
        /// Clear
        /// </summary>
        internal void Clear()
        {
            if (!string.IsNullOrEmpty(FormDesignVo.Layout))
            {
                if (DialogBox.Msg("当前" + Viewer.Text + "数据将被清空，请确认？？", MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    uiHelper.BeginLoading(Viewer);
                    Viewer.showPanelForm.InitComponent(FormDesignVo.Layout);
                    uiHelper.CloseLoading(Viewer);
                }
            }
        }
        #endregion

        #region LoadForm
        /// <summary>
        /// LoadForm
        /// </summary>
        /// <param name="formLayout"></param>
        /// <param name="xmlData"></param>
        void LoadForm(string formLayout, string xmlData)
        {
            try
            {
                Viewer.xtraScrollableControl.SuspendLayout();
                Viewer.showPanelForm.ClearComponent();
                uiHelper.BeginLoading(Viewer);
                if (string.IsNullOrEmpty(xmlData))
                    Viewer.showPanelForm.InitComponent(formLayout);
                else
                    Viewer.showPanelForm.InitComponent(formLayout, xmlData);
            }
            catch (System.Exception e)
            {
                DialogBox.Msg(e.Message);
            }
            finally
            {
                Viewer.xtraScrollableControl.ResumeLayout();
                uiHelper.CloseLoading(Viewer);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Save
        /// </summary>
        internal void Save()
        {
            if (Viewer.txtPatName.Tag == null)
            {
                DialogBox.Msg("请先调出患者信息。");
                return;
            }
            EntityPatientInfo patVo = Viewer.txtPatName.Tag as EntityPatientInfo;

            try
            {
                uiHelper.BeginLoading(Viewer);
                string fieldName = string.Empty;
                DateTime dtmNow = Utils.ServerTime();
                EntityRptContagion vo = new EntityRptContagion();

                vo.xmlData = Viewer.showPanelForm.XmlData();
                if (Viewer.showPanelForm.IsAllowSave == false)
                {
                    DialogBox.Msg("存在必填项目没有处理，请检查。项目：" + Viewer.showPanelForm.HintInfo);
                    return;
                }
                vo.rptId = Function.Dec(this.ContagionDisplayVo.rptId);
                vo.reportId = this.ContagionDisplayVo.reportId;

                // 报告时间
                if (ContagionParmData.Any(t => t.reportId == vo.reportId && t.keyId == "reportTime"))
                {
                    fieldName = ContagionParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "reportTime").keyValue;
                    vo.reportTime = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (vo.reportTime.Trim() == string.Empty) vo.reportTime = dtmNow.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    vo.reportTime = dtmNow.ToString("yyyy-MM-dd HH:mm:ss");
                }
                // 上报科室
                if (ContagionParmData.Any(t => t.reportId == vo.reportId && t.keyId == "deptCode"))
                {
                    fieldName = ContagionParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "deptCode").keyValue;
                    string deptName = Viewer.showPanelForm.GetItemInfo(fieldName);
                }
                else
                {
                    string deptName = GlobalLogin.objLogin.DeptName;
                }
                // 报告人
                if (ContagionParmData.Any(t => t.reportId == vo.reportId && t.keyId == "operCode"))
                {
                    fieldName = ContagionParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "operCode").keyValue;
                    vo.reportOperName = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (GlobalDic.DataSourceEmployee.Any(t => t.operName == vo.reportOperName))
                    {
                        vo.reportOperCode = GlobalDic.DataSourceEmployee.FirstOrDefault(t => t.operName == vo.reportOperName).operCode;
                    }
                }
                else
                {
                    vo.reportOperCode = GlobalLogin.objLogin.EmpNo;
                    vo.reportOperName = GlobalLogin.objLogin.EmpName;
                }
                // 报告编号
                if (ContagionParmData.Any(t => t.reportId == vo.reportId && t.keyId == "regCode"))
                {
                    EntityRptContagionParm parm = ContagionParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "regCode");
                    // 计算类
                    if (parm.flag == 1)
                    {
                        string[] fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            vo.registerCode += Viewer.showPanelForm.GetItemInfo(fieldSub);
                        }
                    }
                    else
                    {
                        fieldName = parm.keyValue;
                        vo.registerCode = Viewer.showPanelForm.GetItemInfo(fieldName);
                    }
                }
                if (!string.IsNullOrEmpty(patVo.ipNo) && patVo.ipTimes > 0)
                    vo.patNo = patVo.ipNo;
                else
                    vo.patNo = patVo.cardNo;
                vo.patName = patVo.name;
                vo.patSex = patVo.sex;
                if (!string.IsNullOrEmpty(patVo.birth)) vo.birthday = Function.Datetime(patVo.birth);
                vo.contactTel = patVo.contTel;
                vo.deptCode = patVo.deptCode;
                vo.xmlData = Viewer.showPanelForm.XmlData();
                vo.formId = this.formId;
                vo.operCode = GlobalLogin.objLogin.EmpNo;
                vo.recordDate = dtmNow;
                vo.status = 1;
                vo.patType = Viewer.rdoFlag.SelectedIndex + 1;
                string reqNo = patVo.clNo;

                using (ProxyContagion proxy = new ProxyContagion())
                {
                    decimal rptId = 0;
                    if (proxy.Service.SaveContagion(vo,reqNo, out rptId) > 0)
                    {
                        Viewer.IsSave = true;
                        if (this.ContagionDisplayVo.isNew)
                        {
                            this.ContagionDisplayVo.rptId = rptId.ToString();
                        }
                        Viewer.txtCardNo.Properties.ReadOnly = true;
                        DialogBox.Msg("数据保存成功！");
                    }
                    else
                    {
                        DialogBox.Msg("数据保存失败。");
                    }
                }
            }
            finally
            {
                uiHelper.CloseLoading(Viewer);
            }
        }
        #endregion

        #region XR
        /// <summary>
        /// GetXR
        /// </summary>
        /// <returns></returns>
        XtraReport GetXR()
        {
            EntityFormDesign formVo = null;
            EntityEmrPrintTemplate printVo = null;
            using (ProxyFormDesign proxy = new ProxyFormDesign())
            {
                proxy.Service.GetForm((int)this.formId, out formVo);
                if (formVo == null) return null;
                printVo = proxy.Service.GetFormPrintTemplate(1,formVo.Printtemplateid.ToString());
            }
            DataTable printDataSource = FormTool.GetPrintDataTable(formVo.Layout, Viewer.showPanelForm.XmlData());
            if (printVo.templateFile != null && printVo.templateFile.Length > 0)
            {
                XtraReport xr = new XtraReport();
                MemoryStream stream = new MemoryStream(printVo.templateFile);
                xr.LoadLayout(stream);
                xr.DataSource = printDataSource;
                return xr;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Print
        /// <summary>
        /// Print
        /// </summary>
        internal void Print()
        {
            frmPrintDocumentSimple frm = new frmPrintDocumentSimple(GetXR());
            frm.ShowDialog();
        }
        #endregion

        #region Export
        /// <summary>
        /// Export
        /// </summary>
        internal void Export()
        {
            XtraReport xr = GetXR();
            if (xr != null && xr.DataSource != null)
            {
                xr.Name = Viewer.Text;
                uiHelper.Export(xr);
            }
        }
        #endregion

        #endregion

    }
}
