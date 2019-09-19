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
    public class ctlZrbbgEdit : BaseController
    {
        #region Override

        /// <summary>
        /// UI.Viewer
        /// </summary>
        private frmZyrbgkEdit Viewer = null;

        /// <summary>
        /// SetUI
        /// </summary>
        /// <param name="child"></param>
        public override void SetUI(frmBase child)
        {
            base.SetUI(child);
            Viewer = (frmZyrbgkEdit)child;
        }
        #endregion

        #region 变量.属性

        /// <summary>
        /// 显示VO
        /// </summary>
        internal EntityZrbbgDisplay ZrbbgDisplayVo { get; set; }

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
        List<EntityRptZrbbgParm> ZrbbgParmData { get; set; }

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
                //using (ProxyEntityFactory proxy = new ProxyEntityFactory())
                //{
                //    ZrbbgParmData = EntityTools.ConvertToEntityList<EntityRptZrbbgParm>(proxy.Service.SelectFullTable(new EntityRptZrbbgParm()));
                //}
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    ZrbbgParmData = proxy.Service.GetZrbbgParm();
                }
                #endregion

                // 传染病报表
                if (ZrbbgParmData != null)
                {
                    if (ZrbbgParmData.Any(t => t.reportId == this.ZrbbgDisplayVo.reportId && t.keyId == "templateId"))
                    {
                        this.formId = Function.Dec(ZrbbgParmData.FirstOrDefault(t => t.reportId == this.ZrbbgDisplayVo.reportId && t.keyId == "templateId").keyValue);
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
                if (Function.Dec(this.ZrbbgDisplayVo.rptId) > 0)
                {
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        EntityRptZrbbg vo = proxy.Service.GetZrbbg(Function.Dec(this.ZrbbgDisplayVo.rptId));

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
                if (this.ZrbbgDisplayVo.pubRoleId != string.Empty && GlobalLogin.objLogin.lstRoleID.IndexOf(this.ZrbbgDisplayVo.pubRoleId) >= 0)
                { }
                else
                {
                    deptCode = this.ZrbbgDisplayVo.owerDeptCode;
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
            GlobalCase.caseInfo.CaseCode = this.ZrbbgDisplayVo.reportId;

            //Viewer.showPanelForm.RefreshPatInfo();

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
                EntityRptZrbbg vo = new EntityRptZrbbg();

                vo.xmlData = Viewer.showPanelForm.XmlData();
                if (Viewer.showPanelForm.IsAllowSave == false)
                {
                    DialogBox.Msg("存在必填项目没有处理，请检查。项目：\r\n" + Viewer.showPanelForm.HintInfo);
                    return;
                }
                vo.rptId = Function.Dec(this.ZrbbgDisplayVo.rptId);
                vo.reportId = this.ZrbbgDisplayVo.reportId;
                vo.reportDate = this.ZrbbgDisplayVo.reportDate;
                vo.bcDate = this.ZrbbgDisplayVo.bcDate;
                vo.skDate = this.ZrbbgDisplayVo.skDate;
                //vo.contactAddr = this.ZrbbgDisplayVo.contactAddr;

                // 报告时间
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "reportDate"))
                {
                    string reportDate = string.Empty;
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "reportDate").keyValue;
                    reportDate = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (string.IsNullOrEmpty(reportDate) && string.IsNullOrEmpty(vo.reportDate))
                        vo.reportDate = dtmNow.ToString("yyyy-MM-dd HH:mm");
                    else if(!string.IsNullOrEmpty(reportDate))
                        vo.reportDate = reportDate;
                }
                else
                {
                    vo.reportDate = dtmNow.ToString("yyyy-MM-dd HH:mm");
                }

                // 编号
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "registerCode"))
                {
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "registerCode").keyValue;
                    vo.registerCode = Viewer.showPanelForm.GetItemInfo(fieldName);
                }
                else
                {
                    vo.registerCode = Function.Datetime(vo.reportDate).ToString("MMdd") + patVo.pid;
                }
                // 报告人
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "reportOperCode"))
                {
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "reportOperCode").keyValue;
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

                // 上报科室
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "reportDept"))
                {
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "reportDept").keyValue;
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

                // 户籍地址
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "fimalyAddr"))
                {
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "fimalyAddr");
                    // 计算类
                    if (parm.flag == 3)
                    {
                        string addr = string.Empty;
                        string[] fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            addr += Viewer.showPanelForm.GetItemInfo(fieldSub);
                            if (fieldSub == "ADDR1" && !string.IsNullOrEmpty(addr))
                                vo.familyAddr += addr + "省";
                            if (fieldSub == "ADDR2" && !string.IsNullOrEmpty(addr))
                                vo.familyAddr += addr + "市";
                            if (fieldSub == "ADDR3" && !string.IsNullOrEmpty(addr))
                                vo.familyAddr += addr + "县";
                            if (fieldSub == "ADDR4" && !string.IsNullOrEmpty(addr))
                                vo.familyAddr += addr + "镇";
                            if (fieldSub == "ADDR5" && !string.IsNullOrEmpty(addr))
                                vo.familyAddr += addr + "村";
                            if (fieldSub == "ADDR6" && !string.IsNullOrEmpty(addr))
                                vo.familyAddr += addr ;
                            addr = string.Empty;
                        }
                    }
                    else if (parm.flag == 1)
                    {
                        string[] fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            vo.familyAddr += Viewer.showPanelForm.GetItemInfo(fieldSub);
                        }
                    }
                    else
                    {
                        fieldName = parm.keyValue;
                        vo.familyAddr = Viewer.showPanelForm.GetItemInfo(fieldName);
                    }
                }

                // 现地址
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "contactAddr"))
                {
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "contactAddr");
                    // 计算类
                    if (parm.flag == 3)
                    {
                        string addr = string.Empty;
                        string[] fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            addr += Viewer.showPanelForm.GetItemInfo(fieldSub);
                            if (fieldSub == "ADDR1" && !string.IsNullOrEmpty(addr))
                                vo.contactAddr += addr + "省";
                            if (fieldSub == "ADDR2" && !string.IsNullOrEmpty(addr))
                                vo.contactAddr += addr + "市";
                            if (fieldSub == "ADDR3" && !string.IsNullOrEmpty(addr))
                                vo.contactAddr += addr + "县";
                            if (fieldSub == "ADDR4" && !string.IsNullOrEmpty(addr))
                                vo.contactAddr += addr + "镇";
                            if (fieldSub == "ADDR5" && !string.IsNullOrEmpty(addr))
                                vo.contactAddr += addr + "村";
                            if (fieldSub == "ADDR6" && !string.IsNullOrEmpty(addr))
                                vo.contactAddr += addr ;
                            addr = string.Empty;
                        }
                    }
                    else if (parm.flag == 1)
                    {
                        string[] fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            vo.contactAddr += Viewer.showPanelForm.GetItemInfo(fieldSub);
                        }
                    }
                    else
                    {
                        fieldName = parm.keyValue;
                        vo.contactAddr = Viewer.showPanelForm.GetItemInfo(fieldName);
                    }
                }

                // 诊断病名
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "diagnoseName"))
                {
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "diagnoseName");
                    // 计算类
                    if (parm.flag == 2)
                    {
                        string[] fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            if (fieldSub.Contains("|"))
                            {
                                string[] str = fieldSub.Split('|');
                                string flg = Viewer.showPanelForm.GetItemInfo(str[0]);
                                if (flg == "1")
                                    vo.diagnoseName += str[1] + "、";
                            }
                            else 
                            {
                                vo.diagnoseName += Viewer.showPanelForm.GetItemInfo(fieldSub);
                            }
                            
                        }
                        if (!string.IsNullOrEmpty(vo.diagnoseName))
                            vo.diagnoseName = vo.diagnoseName.TrimEnd('、');
                        else
                        {
                            DialogBox.Msg("存在必填项目没有处理，请检查。项目：\r\n" + "诊断病名必须勾选或填写");
                            return;
                        }
                    }
                    else
                    {
                        fieldName = parm.keyValue;
                        vo.diagnoseName = Viewer.showPanelForm.GetItemInfo(fieldName);
                    }
                }

                // 诊断日期
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "diagnoseDate"))
                {
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "diagnoseDate").keyValue;
                    vo.diagnoseDate = Viewer.showPanelForm.GetItemInfo(fieldName);

                    if (vo.diagnoseDate.IndexOf("00:00") > 0)
                    {
                        DialogBox.Msg("诊断日期时间不能为 00:00，请检查。项目：\r\n" );
                        return;
                    }
                }

                // 发病日期
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "infectiveDate"))
                {
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "infectiveDate").keyValue;
                    vo.infectiveDate = Viewer.showPanelForm.GetItemInfo(fieldName);
                }

                // 收卡日期
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "skDate"))
                {
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "skDate").keyValue;
                    vo.skDate = Viewer.showPanelForm.GetItemInfo(fieldName);
                }

                // 网报日期
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "bcDate"))
                {
                    string bcDate = string.Empty;
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "bcDate").keyValue;
                    bcDate = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (string.IsNullOrEmpty(bcDate) && string.IsNullOrEmpty(vo.bcDate))
                        vo.bcDate = dtmNow.ToString("yyyy-MM-dd HH:mm");
                    else if (!string.IsNullOrEmpty(bcDate))
                        vo.bcDate = bcDate;
                }
                else
                {
                    vo.bcDate = dtmNow.ToString("yyyy-MM-dd HH:mm");
                }


                // 收卡日期
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "skDate"))
                {
                    string skDate = string.Empty;
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "skDate").keyValue;
                    skDate = Viewer.showPanelForm.GetItemInfo(fieldName);
                    if (string.IsNullOrEmpty(skDate) && string.IsNullOrEmpty(vo.skDate))
                        vo.skDate = dtmNow.ToString("yyyy-MM-dd HH:mm");
                    else if (!string.IsNullOrEmpty(skDate))
                        vo.skDate = skDate;
                }
                else
                {
                    vo.bcDate = dtmNow.ToString("yyyy-MM-dd HH:mm");
                }

                // 家长姓名
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "parentName"))
                {
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "parentName").keyValue;
                    vo.parentName = Viewer.showPanelForm.GetItemInfo(fieldName);
                }

                // 有效证件号
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "idCard"))
                {
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "idCard").keyValue;
                    vo.idCard = Viewer.showPanelForm.GetItemInfo(fieldName);
                }

                // 备注
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "bz"))
                {
                    fieldName = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "bz").keyValue;
                    vo.BZ = Viewer.showPanelForm.GetItemInfo(fieldName);
                }

                // 人群分类
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "rqfl"))
                {
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "rqfl");
                    // 计算类
                    if (parm.flag == 2)
                    {
                        string[] fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            string[] str = fieldSub.Split('|');
                            string flg = Viewer.showPanelForm.GetItemInfo(str[0]);
                            if (flg == "1")
                                vo.RQFL += str[1] + "、";
                        }
                        if (!string.IsNullOrEmpty(vo.RQFL))
                            vo.RQFL = vo.RQFL.TrimEnd('、');
                    }
                    else
                    {
                        fieldName = parm.keyValue;
                        vo.RQFL = Viewer.showPanelForm.GetItemInfo(fieldName);
                    }

                    if (string.IsNullOrEmpty(vo.RQFL))
                    {
                        DialogBox.Msg("存在必填项目没有处理，请检查。项目：\r\n" + "人群分类或职业不能为空");
                        return;
                    }
                }

                // 必填项目
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "required"))
                {
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "required");
                    string required = string.Empty;
                    bool flg = false;
                    string[] fieldNames;
                    // 计算类
                    if (parm.flag != 0)
                    {
                        fieldNames = parm.keyValue.Split('+');
                        for (int i = 0; i < parm.flag;i++ )
                        {
                            required = Viewer.showPanelForm.GetItemInfo(fieldNames[i]);
                            if (required == "1")
                            {
                                flg = true;
                                break;
                            }
                        }

                        if (flg)
                        {
                            string str = string.Empty;
                            string strRequer = string.Empty;
                            for (int i = Function.Int(parm.flag); i < fieldNames.Length; i++)
                            {
                                string[] strNames = fieldNames[i].Split('|');
                                str = Viewer.showPanelForm.GetItemInfo(strNames[0]);
                                if (string.IsNullOrEmpty(str))
                                    strRequer += strNames[1] + Environment.NewLine;
                            }

                            if (!string.IsNullOrEmpty(strRequer))
                            {
                                DialogBox.Msg("存在必填项目没有处理，请检查。项目：\r\n" + strRequer);
                                return;
                            }
                        }
                    }
                }

                //肿瘤诊断依据必填
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "zdyj"))
                {
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "zdyj");
                    string required = string.Empty;
                    bool flg = false;
                    string[] fieldNames;
                    if (parm.flag != 0)
                    {
                        fieldNames = parm.keyValue.Split('+');

                        foreach (string fieldSub in fieldNames)
                        {
                            required = Viewer.showPanelForm.GetItemInfo(fieldSub);
                            if (required == "1")
                            {
                                flg = true;
                                break;
                            }
                        }
                    }

                    if (!flg)
                    {
                        DialogBox.Msg("存在必填项目没有处理，诊断依据必选。");
                        return;
                    }
                }


                // 提示
                string lbTips = string.Empty;
                string mdTips = string.Empty;
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "lbTips"))
                {
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "lbTips");
                    string lbFlg = string.Empty;
                    string[] fieldNames = parm.keyValue.Split('+');
                    if (parm.flag == 1)
                    {
                        foreach (string fieldSub in fieldNames)
                        {
                            lbFlg = Viewer.showPanelForm.GetItemInfo(fieldSub);
                            if (lbFlg == "1")
                                break;
                        }
                    }
                    else
                    {
                        fieldName = parm.keyValue;
                        lbFlg = Viewer.showPanelForm.GetItemInfo(fieldName);
                    }

                    if (lbFlg == "1")
                    {
                        lbTips = "请填写\r\n传染病报告卡艾滋病性病附卡。 \r\n";
                    }
                }

                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "mdTips"))
                {
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "mdTips");
                    string mdFlg = string.Empty;
                    string[] fieldNames = parm.keyValue.Split('+');
                    if (parm.flag == 1)
                    {
                        foreach (string fieldSub in fieldNames)
                        {
                            mdFlg = Viewer.showPanelForm.GetItemInfo(fieldSub);
                            if (mdFlg == "1")
                                break;
                        }
                    }
                    else
                    {
                        fieldName = parm.keyValue;
                        mdFlg = Viewer.showPanelForm.GetItemInfo(fieldName);
                    }

                    if (mdFlg == "1")
                    {
                        mdTips = "传染病报告卡(梅毒)附卡。 \r\n";
                    }
                }
                if(!string.IsNullOrEmpty(lbTips) || !string.IsNullOrEmpty(mdTips))
                    DialogBox.Msg(lbTips + mdTips);

                //性别
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "sex"))
                {
                    string[] fieldNames;
                    string sex = string.Empty;
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "sex");
                    if (parm.flag != 0)
                    {
                        fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            sex = Viewer.showPanelForm.GetItemInfo(fieldSub);
                            if (sex != "0")
                                break;
                        }

                        if (sex == "0")
                        {
                            DialogBox.Msg("存在必填项目没有处理，请检查。项目：\r\n" + "性别");
                            return;
                        }
                    }
                }
                //TRUST/RPR
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "TRUST/RPR"))
                {
                    string[] fieldNames;
                    string tFlg = string.Empty;
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "TRUST/RPR");
                    if (parm.flag != 0)
                    {
                        fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            tFlg = Viewer.showPanelForm.GetItemInfo(fieldSub);
                            if (tFlg != "0")
                                break;
                        }

                        if (tFlg == "0")
                        {
                            DialogBox.Msg("存在必填项目没有处理，请检查。项目：\r\n" + "TRUST/RPR");
                            return;
                        }
                    }
                }
                //TPPA/TPHA 
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "TPPA/TPHA"))
                {
                    string[] fieldNames;
                    string tFlg = string.Empty;
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "TPPA/TPHA");
                    if (parm.flag != 0)
                    {
                        fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            tFlg = Viewer.showPanelForm.GetItemInfo(fieldSub);
                            if (tFlg != "0")
                                break;
                        }

                        if (tFlg == "0")
                        {
                            DialogBox.Msg("存在必填项目没有处理，请检查。项目：\r\n" + "TPPA/TPHA");
                            return;
                        }
                    }
                }

                //传染来源
                if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "zrly"))
                {
                    string[] fieldNames;
                    string zrly = string.Empty;
                    EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "zrly");
                    if (parm.flag != 0)
                    {
                        fieldNames = parm.keyValue.Split('+');
                        foreach (string fieldSub in fieldNames)
                        {
                            zrly = Viewer.showPanelForm.GetItemInfo(fieldSub);
                            if (zrly != "0")
                                break;
                        }

                        if (zrly == "0")
                        {
                            DialogBox.Msg("存在必填项目没有处理，请检查。项目：\r\n" + "传染来源");
                            return;
                        }
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
                //vo.deptCode = patVo.deptCode;
                vo.xmlData = Viewer.showPanelForm.XmlData();
                vo.formId = this.formId;
                vo.operCode = GlobalLogin.objLogin.EmpNo;
                vo.recordDate = dtmNow;
                vo.status = 1;
                vo.patType = Viewer.rdoFlag.SelectedIndex + 1;
                string reqNo = patVo.clNo;

                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    decimal rptId = 0;
                    if (proxy.Service.SaveZrbbg(vo, out rptId) > 0)
                    {
                        Viewer.IsSave = true;
                        if (this.ZrbbgDisplayVo.isNew)
                        {
                            this.ZrbbgDisplayVo.rptId = rptId;
                        }
                        Viewer.txtCardNo.Properties.ReadOnly = true;

                        //病毒性肝炎
                        
                        if (ZrbbgParmData.Any(t => t.reportId == vo.reportId && t.keyId == "ygfk"))
                        {
                            string[] fieldNames;
                            EntityRptZrbbgParm parm = ZrbbgParmData.FirstOrDefault(t => t.reportId == vo.reportId && t.keyId == "ygfk");
                            string ygfk = string.Empty;
                            if (parm.flag != 0)
                            {
                                fieldNames = parm.keyValue.Split('+');
                                foreach (string fieldSub in fieldNames)
                                {
                                    ygfk = Viewer.showPanelForm.GetItemInfo(fieldSub);
                                    if (ygfk != "0")
                                    {
                                        frmZrbygfk frm = new frmZrbygfk(vo.rptId);
                                        frm.ShowDialog();
                                        break;
                                    }
                                }
                                if(ygfk == "0")
                                    proxy.Service.RegisterZrbygfk(vo.rptId, null);
                            }
                        }

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

            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                if (GlobalLogin.objLogin.lstRoleID.Contains("34"))
                {
                    proxy.Service.UpdateZrbbgPrintFlg(Function.Dec(this.ZrbbgDisplayVo.rptId));
                }
            }
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
