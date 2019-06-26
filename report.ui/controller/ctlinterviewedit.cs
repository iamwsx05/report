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
    public class ctlinterviewedit : BaseController
    {
        #region Override

        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmInterviewEdit Viewer = null;

        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmInterviewEdit)child;
        }
        #endregion

        #region 变量.属性

        /// <summary>
        /// 显示VO
        /// </summary>
        internal EntityOutpatientInterview InterviewVo { get; set; }


        /// <summary>
        /// 参数
        /// </summary>
        List<EntityRptInterviewParm> InterviewParmData { get; set; }

        /// <summary>
        /// 表单设计VO
        /// </summary>
        EntityFormDesign FormDesignVo = null;

        /// <summary>
        /// 表单ID
        /// </summary>
        decimal formId { get; set; }
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
                Viewer.Location = new System.Drawing.Point(Viewer.Location.X, 0);
                Viewer.Height = Screen.PrimaryScreen.WorkingArea.Height;
                uiHelper.BeginLoading(Viewer);

                #region 参数

                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    InterviewParmData = proxy.Service.GetInterviewParm();
                }
                #endregion

                this.formId = 64;

                if (this.formId > 0)
                {
                    using (ProxyFormDesign proxy = new ProxyFormDesign())
                    {
                        proxy.Service.GetForm((int)this.formId, out FormDesignVo);
                    }
                }
                if (FormDesignVo == null) FormDesignVo = new EntityFormDesign();


                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    EntityOutpatientInterview vo = proxy.Service.GetInterviewVo(Function.Dec(this.InterviewVo.rptId));

                    if (vo != null)
                    {
                        #region patientInfo
                        Viewer.txtCardNo.Text = vo.patNo;
                        GetPatient();
                        #endregion
                        LoadForm(FormDesignVo.Layout, vo.xmlData);
                    }
                    else
                    {
                        Viewer.txtCardNo.Text = this.InterviewVo.patNo;
                        GetPatient();
                        LoadForm(FormDesignVo.Layout, null);
                    }
                }
            }
            finally
            {
                uiHelper.CloseLoading(Viewer);
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
                EntityOutpatientInterview vo = new EntityOutpatientInterview();
                vo.rptId = Function.Dec(this.InterviewVo.rptId);
                vo.patName = patVo.name;
                vo.patSex = patVo.sex;
                vo.patNo = patVo.ipNo;
                vo.birthday = patVo.birth;
                vo.idCard = patVo.ID;
                //vo.contactAddr = patVo.addr;
                //vo.contactTel = patVo.contTel;
                vo.outDeptCode = patVo.deptCode;
                vo.outHospitalTime = Function.Datetime(patVo.outDate).ToString("yyyy-MM-dd HH:mm") ;
                if (!string.IsNullOrEmpty(patVo.birth)) 
                    vo.birthday = patVo.birth;
                vo.contactTel = patVo.contTel;
                vo.xmlData = Viewer.showPanelForm.XmlData();
                vo.recordDate = dtmNow.ToString("yyyy-MM-dd HH:mm:ss") ;
                vo.interviewTime = this.InterviewVo.interviewTime;
                vo.registerid = this.InterviewVo.registerid;
                vo.status = 1;

                // 随访人
                if (InterviewParmData.Any(t => t.keyId == "interviewCode"))
                {
                    fieldName = InterviewParmData.FirstOrDefault(t => t.keyId == "interviewCode").keyValue;
                    fieldName = InterviewParmData.FirstOrDefault(t => t.keyId == "interviewCode").keyValue;
                    string interviewName = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (GlobalDic.DataSourceEmployee.Any(t => t.operName == interviewName))
                    {
                        vo.interviewCode = GlobalDic.DataSourceEmployee.FirstOrDefault(t => t.operName == interviewName).operCode;
                    }
                    else
                        vo.interviewCode = GlobalLogin.objLogin.EmpNo;
                }
                else
                    vo.interviewCode = GlobalLogin.objLogin.EmpNo;

                // 随访时间
                if (InterviewParmData.Any(t => t.keyId == "interviewTime"))
                {
                    string interviewTime = string.Empty;
                    fieldName = InterviewParmData.FirstOrDefault(t => t.keyId == "interviewTime").keyValue;
                    interviewTime = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (string.IsNullOrEmpty(interviewTime) && string.IsNullOrEmpty(vo.interviewTime)) 
                        vo.interviewTime = dtmNow.ToString("yyyy-MM-dd HH:mm");
                }
                else
                    vo.interviewTime = dtmNow.ToString("yyyy-MM-dd HH:mm");

                // 现住地址
                if (InterviewParmData.Any(t => t.keyId == "contactAddr"))
                {
                    string contactAddr = string.Empty;
                    fieldName = InterviewParmData.FirstOrDefault(t => t.keyId == "contactAddr").keyValue;
                    contactAddr = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (!string.IsNullOrEmpty(contactAddr))
                        vo.contactAddr = contactAddr;
                }

                // 联系电话
                if (InterviewParmData.Any(t => t.keyId == "contactTel"))
                {
                    string contactTel = string.Empty;
                    fieldName = InterviewParmData.FirstOrDefault(t => t.keyId == "contactTel").keyValue;
                    contactTel = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (!string.IsNullOrEmpty(contactTel))
                        vo.contactTel = contactTel;
                }

                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    if (proxy.Service.SaveInterview(ref vo) > 0)
                    {
                        Viewer.IsSave = true;
                        Viewer.txtCardNo.Properties.ReadOnly = true;
                        this.InterviewVo.rptId = vo.rptId;
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

                List<EntityPatientInfo> lstPat = null;
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    //lstPat = proxy.Service.GetPatient(cardNo, Viewer.rdoFlag.SelectedIndex + 1, deptCode);
                    lstPat = proxy.Service.GetPatient(cardNo, 2, deptCode);
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

        #endregion

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

            //GlobalCase.caseInfo = new EntityCaseInfo();
            //GlobalCase.caseInfo.CaseCode = this.EventDisplayVo.eventId;
            
            //Viewer.showPanelForm.SetFieldValue("主管医生", pat.doctName);
            //Viewer.showPanelForm.RefreshPatInfo();

            #endregion
        }
        #endregion

        #endregion
    }
}
