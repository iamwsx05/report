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
    /// 事件编辑控制类
    /// </summary>
    public class ctlEventEdit : BaseController
    {
        #region Override

        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmEventEdit Viewer = null;

        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmEventEdit)child;
        }
        #endregion

        #region 变量.属性

        /// <summary>
        /// 显示VO
        /// </summary>
        internal EntityEventDisplay EventDisplayVo { get; set; }

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
        List<EntityRptEventParm> EventParmData { get; set; }

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
                    EventParmData = EntityTools.ConvertToEntityList<EntityRptEventParm>(proxy.Service.SelectFullTable(new EntityRptEventParm()));
                }
                #endregion

                // 不良事件报表
                if (EventParmData != null)
                {
                    if (EventParmData.Any(t => t.eventId == this.EventDisplayVo.eventId && t.keyId == "templateId"))
                    {
                        this.formId = Function.Dec(EventParmData.FirstOrDefault(t => t.eventId == this.EventDisplayVo.eventId && t.keyId == "templateId").keyValue);
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
                if (Function.Dec(this.EventDisplayVo.rptId) > 0)
                {
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        EntityRptEvent vo = proxy.Service.GetEvent(Function.Dec(this.EventDisplayVo.rptId));

                        #region patientInfo
                        Viewer.rdoFlag.SelectedIndex = vo.patType - 1;
                        Viewer.rdoFlag.Properties.ReadOnly = true;
                        Viewer.txtCardNo.Text = vo.patNo;
                        GetPatient();
                        #endregion

                        LoadForm(FormDesignVo.Layout, vo.xmlData);
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
                if (Viewer.rdoFlag.SelectedIndex == 0 && cardNo.Length < 10)
                {
                    cardNo = cardNo.PadLeft(10, '0');
                    Viewer.txtCardNo.Text = cardNo;
                }
                string deptCode = string.Empty;
                if (this.EventDisplayVo.pubRoleId != string.Empty && GlobalLogin.objLogin.lstRoleID.IndexOf(this.EventDisplayVo.pubRoleId) >= 0)
                { }
                else
                {
                    deptCode = this.EventDisplayVo.owerDeptCode;
                }
                List<EntityPatientInfo> lstPat = null;
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    lstPat = proxy.Service.GetPatient(cardNo, Viewer.rdoFlag.SelectedIndex + 1, deptCode);
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
            GlobalPatient.currPatient.BedNo = pat.bedNo;
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

            GlobalPatient.currPatient.RegisterID = pat.ID;

            GlobalCase.caseInfo = new EntityCaseInfo();
            GlobalCase.caseInfo.CaseCode = this.EventDisplayVo.eventId;

            if (this.EventDisplayVo.eventId == "13")
            {
                Viewer.showPanelForm.SetFieldValue("X020", pat.corp);
                Viewer.showPanelForm.SetFieldValue("X024", Viewer.txtCardNo.Text);
            }

            //Viewer.showPanelForm.RefreshPatInfo();

            #endregion
        }
        #endregion

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
            string reqStr = string.Empty;
            EntityPatientInfo patVo = Viewer.txtPatName.Tag as EntityPatientInfo;

            try
            {
                uiHelper.BeginLoading(Viewer);
                string fieldName = string.Empty;
                DateTime dtmNow = Utils.ServerTime();
                EntityRptEvent vo = new EntityRptEvent();
                vo.rptId = Function.Dec(this.EventDisplayVo.rptId);
                vo.eventId = this.EventDisplayVo.eventId;
                vo.reportTime = this.EventDisplayVo.reportTime;
                // 报告时间
                if (EventParmData.Any(t => t.eventId == vo.eventId && t.keyId == "reportTime"))
                {
                    string reportTime = string.Empty;
                    fieldName = EventParmData.FirstOrDefault(t => t.eventId == vo.eventId && t.keyId == "reportTime").keyValue;
                    reportTime = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (string.IsNullOrEmpty(reportTime) && string.IsNullOrEmpty(vo.reportTime))
                        vo.reportTime = dtmNow.ToString("yyyy-MM-dd HH:mm:ss");
                    else if (!string.IsNullOrEmpty(reportTime))
                        vo.reportTime = reportTime;
                }
                else if(string.IsNullOrEmpty(vo.reportTime))
                {
                    vo.reportTime = dtmNow.ToString("yyyy-MM-dd HH:mm:ss");
                }

                // 报告人
                if (EventParmData.Any(t => t.eventId == vo.eventId && t.keyId == "operCode"))
                {
                    fieldName = EventParmData.FirstOrDefault(t => t.eventId == vo.eventId && t.keyId == "operCode").keyValue;
                    vo.reportOperName = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (GlobalDic.DataSourceEmployee.Any(t => t.operName == vo.reportOperName))
                    {
                        vo.reportOperCode = GlobalDic.DataSourceEmployee.FirstOrDefault(t => t.operName == vo.reportOperName).operCode;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(patVo.ipNo) && patVo.ipTimes > 0 && vo.eventId == "17")
                            vo.reportOperCode = GlobalLogin.objLogin.EmpNo;
                        else if (vo.eventId == "17")
                            vo.reportOperCode = GlobalLogin.objLogin.EmpNo;
                    }
                }
                else
                {
                    vo.reportOperCode = GlobalLogin.objLogin.EmpNo;
                    vo.reportOperName = GlobalLogin.objLogin.EmpName;
                }
                // 上报科室
                if (EventParmData.Any(t => t.eventId == vo.eventId && t.keyId == "deptCode"))
                {
                    fieldName = EventParmData.FirstOrDefault(t => t.eventId == vo.eventId && t.keyId == "deptCode").keyValue;
                    string reportDeptName = Viewer.showPanelForm.GetItemInfo(fieldName);
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        vo.reportDeptCode = proxy.Service.GetDeptCode(reportDeptName);
                    }
                }
                else
                {
                    vo.reportDeptCode = patVo.deptCode;
                }
                // 事件编码
                if (EventParmData.Any(t => t.eventId == vo.eventId && t.keyId == "eventCode"))
                {
                    fieldName = EventParmData.FirstOrDefault(t => t.eventId == vo.eventId && t.keyId == "eventCode").keyValue;
                    vo.eventCode = Viewer.showPanelForm.GetItemInfo(fieldName);
                }
                else if (patVo.pid == "\\")
                {
                    vo.eventCode = Function.Datetime(vo.reportTime).ToString("MMddHHmm") + vo.reportOperCode;
                }
                else
                {
                    vo.eventCode = patVo.pid;
                }

                // 事件名称
                if (EventParmData.Any(t => t.eventId == vo.eventId && t.keyId == "eventName"))
                {
                    fieldName = EventParmData.FirstOrDefault(t => t.eventId == vo.eventId && t.keyId == "eventName").keyValue;
                    vo.eventName = Viewer.showPanelForm.GetItemInfo(fieldName);
                }
                else
                {
                    vo.eventName = Viewer.Text + "-" + patVo.name;
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
                if (Viewer.showPanelForm.IsAllowSave == false)
                {
                    DialogBox.Msg("存在必填项目没有处理，请检查。项目：\r\n" + Viewer.showPanelForm.HintInfo);

                    return;
                }

                if (vo.eventId == "13")
                {
                    int reqBlfyFlg = 0;
                    //不良反应结果
                    if (EventParmData.Any(t => t.eventId == vo.eventId && t.keyId == "BLFYJG"))
                    {
                        string kyeStr = EventParmData.FirstOrDefault(t => t.eventId == vo.eventId && t.keyId == "BLFYJG").keyValue;
                        string[] fieldNames = kyeStr.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            string[] str = fieldSub.Split('|');
                            string value = Viewer.showPanelForm.GetItemInfo(str[0]);
                            if (value == "1")
                            {
                                reqBlfyFlg = 1;
                                break;
                            }
                        }

                        if (reqBlfyFlg == 0)
                        {
                            reqStr += "不良反应事件结果" + Environment.NewLine;
                        }
                    }

                    //停药后反应
                    int reqTyhfyFlg = 0;
                    if (EventParmData.Any(t => t.eventId == vo.eventId && t.keyId == "TYHFY"))
                    {
                        string kyeStr = EventParmData.FirstOrDefault(t => t.eventId == vo.eventId && t.keyId == "TYHFY").keyValue;
                        string[] fieldNames = kyeStr.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            string[] str = fieldSub.Split('|');
                            string value = Viewer.showPanelForm.GetItemInfo(str[0]);
                            if (value == "1")
                            {
                                reqTyhfyFlg = 1;
                                break;
                            }
                        }

                        if (reqTyhfyFlg == 0)
                        {
                            reqStr += "停药或减量后，反应/事件是否消失或减轻" + Environment.NewLine;
                        }
                    }

                    //报告人评价
                    int reqBgrpjFlg = 0;
                    if (EventParmData.Any(t => t.eventId == vo.eventId && t.keyId == "BGRPJ"))
                    {
                        string kyeStr = EventParmData.FirstOrDefault(t => t.eventId == vo.eventId && t.keyId == "BGRPJ").keyValue;
                        string[] fieldNames = kyeStr.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            string[] str = fieldSub.Split('|');
                            string value = Viewer.showPanelForm.GetItemInfo(str[0]);
                            if (value == "1")
                            {
                                reqBgrpjFlg = 1;
                                break;
                            }
                        }

                        if (reqBgrpjFlg == 0)
                        {
                            reqStr += "报告人评价" + Environment.NewLine;
                        }
                    }

                    //报告单位评价
                    int reqBgdwpjFlg = 0;
                    if (EventParmData.Any(t => t.eventId == vo.eventId && t.keyId == "BGDWPJ"))
                    {
                        string kyeStr = EventParmData.FirstOrDefault(t => t.eventId == vo.eventId && t.keyId == "BGDWPJ").keyValue;
                        string[] fieldNames = kyeStr.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            string[] str = fieldSub.Split('|');
                            string value = Viewer.showPanelForm.GetItemInfo(str[0]);
                            if (value == "1")
                            {
                                reqBgdwpjFlg = 1;
                                break;
                            }
                        }

                        if (reqBgdwpjFlg == 0)
                        {
                            reqStr += "报告单位评价" + Environment.NewLine;
                        }
                    }

                    //报告人信息 职业
                    int reqBgrxxFlg = 0;
                    if (EventParmData.Any(t => t.eventId == vo.eventId && t.keyId == "BGDWPJ"))
                    {
                        string kyeStr = EventParmData.FirstOrDefault(t => t.eventId == vo.eventId && t.keyId == "BGDWPJ").keyValue;
                        string[] fieldNames = kyeStr.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            string[] str = fieldSub.Split('|');
                            string value = Viewer.showPanelForm.GetItemInfo(str[0]);
                            if (value == "1")
                            {
                                reqBgrxxFlg = 1;
                                break;
                            }
                            else if (!string.IsNullOrEmpty(value) && value != "0")
                            {
                                reqBgrxxFlg = 1;
                                break;
                            }
                        }

                        if (reqBgrxxFlg == 0)
                        {
                            reqStr += "报告人职业" + Environment.NewLine;
                        }
                    }

                    if (!string.IsNullOrEmpty(reqStr))
                    {
                        DialogBox.Msg("存在必填项目没有处理，请检查。项目：\r\n" + reqStr);
                        return;
                    }
                }

                vo.formId = this.formId;
                vo.operCode = GlobalLogin.objLogin.EmpNo;
                vo.recordDate = dtmNow;
                vo.status = 1;
                vo.patType = Viewer.rdoFlag.SelectedIndex + 1;

                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    decimal rptId = 0;

                    if (proxy.Service.SaveEvent(vo, out rptId) > 0)
                    {
                        Viewer.IsSave = true;
                        if (this.EventDisplayVo.isNew)
                        {
                            this.EventDisplayVo.rptId = rptId.ToString();
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
                printVo = proxy.Service.GetFormPrintTemplate(1, formVo.Printtemplateid.ToString());
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
