using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Entity;
using DevExpress.XtraEditors;
using Common.Utils;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Report.Ui
{
    public partial class frmAdverseInfectious : frmBaseMdi
    {
        public Point p = new Point();
        public frmAdverseInfectious()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                this.defaultLookAndFeel.LookAndFeel.SkinName = GlobalLogin.SkinName;
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(GlobalLogin.SkinName);
            }
        }

        EntityInfectionus vo;
        List<string> lstinfectSource;
        List<string> lstCausationSource;
        List<string> lstDrugSource;
        List<string> lstSampleSource;
        List<EntityDeptList> lstDeptList;
        string OwerDeptCode { get; set; }
        string RoleQuery { get; set; }
        /// <summary>
        /// 报表XR
        /// </summary>
        XtraReport xr = null;

        #region 属性.变量

        /// <summary>
        /// 数据源
        /// </summary>
        BindingSource gvSampleBindingSource { get; set; }

        #endregion


        #region override
        
        /// <summary>
        /// 新报告
        /// </summary>
        public override void New()
        {
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                this.gvSampleBindingSource.DataSource = proxy.Service.GetPathogeny(-1);
            }
            ClearCntrValue(this.pcConent);
        }

        /// <summary>
        /// 检索
        /// </summary>
        public override void Search()
        {
            this.Query();
        }

        /// <summary>
        /// 编辑报告
        /// </summary>
        public override void Edit()
        {
            EditRpt();
            GetDataSource();
        }

        /// <summary>
        /// 删除报告
        /// </summary>
        public override void Delete()
        {
            DeleteRpt();
            this.Query();
        }

        /// <summary>
        /// 保存
        /// </summary>
        #region
        public override void Save()
        {
            EntityPatientInfo patVo = this.txtName.Tag as EntityPatientInfo;

            if (this.txtName.Tag == null)
            {
                DialogBox.Msg("请先调出患者信息。");
                return;
            }

            if (vo != null && vo.rptId > 0 && vo.status == 1)
            {
                DialogBox.Msg("已审核不可修改，请先反审核");
                return;
            }

            this.gvSample.CloseEditor();
            this.gvSample.UpdateCurrentRow();
            
            if (vo == null)
                vo = new EntityInfectionus();
            DateTime dtmNow = Utils.ServerTime();
            GetInfectionusValue(ref vo,patVo);
            if (vo.rptId <= 0)
            {
                vo.reportTime = dtmNow.ToString("yyyy-MM-dd HH:mm:ss");
                vo.reporterId = GlobalLogin.objLogin.EmpNo;
                vo.reporterName = GlobalLogin.objLogin.EmpName;
            }
                
            vo.recordDate = dtmNow;
            List<EntityPathogeny> data = new List<EntityPathogeny>();

            for (int i = this.gvSample.RowCount - 1; i >= 0; i--)
            {
                if (this.gvSample.GetRowCellValue(i, "sampleName") != null && !string.IsNullOrEmpty(this.gvSample.GetRowCellValue(i, "sampleName").ToString()))
                {
                    EntityPathogeny pathogenyVo = new EntityPathogeny();
                    pathogenyVo.sampleName = this.gvSample.GetRowCellValue(i, "sampleName").ToString();
                    pathogenyVo.checkDate = Function.Datetime(this.gvSample.GetRowCellValue(i, "checkDate").ToString()).ToString("yyyy-MM-dd");
                    pathogenyVo.pathogenyName = this.gvSample.GetRowCellValue(i, "pathogenyName").ToString();
                    pathogenyVo.drugName = this.gvSample.GetRowCellValue(i, "drugName").ToString();

                    data.Add(pathogenyVo);
                }
                else
                {
                    this.gvSample.DeleteRow(i);
                }
            }

            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                if (proxy.Service.SaveInfectionus(vo, data) >= 0)
                {
                    DialogBox.Msg("保存成功");
                    vo = null;
                }
                else
                {
                    DialogBox.Msg("保存失败");
                    vo = null;
                }
            }
            this.txtName.Tag = null;
            this.Query();
        }
        #endregion

        /// <summary>
        /// 审核
        /// </summary>
        public override void Confirm()
        {
            vo = GetRowObject();
            //frmInfectionusConfirm frm = new frmInfectionusConfirm(vo);
            //frm.ShowDialog();
            if (DialogBox.Msg("审核通过请点“是”，未通过请点“否”！", MessageBoxIcon.Question) == DialogResult.Yes)
                ConfirmRpt(1);
            else
                ConfirmRpt(2);
            this.Query();
        }

        /// <summary>
        /// 反审核
        /// </summary>
        public override void Cancel()
        {
            ConfirmRpt(0);
            this.Query();
        }

        /// <summary>
        /// 统计
        /// </summary>
        public override void Statistics()
        {

        }

        /// <summary>
        /// 打印
        /// </summary>
        public override void Preview()
        {
            InitXr();

            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    xr.DataSource = proxy.Service.GetXrDataSource(vo.rptId);
                    xr.CreateDocument();
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }

            if (xr != null && xr.DataSource != null)
            {
                xr.Print();
            }
        }

        #endregion

        #region 事件

        private void frmadverseinfectious_Load(object sender, EventArgs e)
        {
            this.Init();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.gvSampleBindingSource.Add(new EntityPathogeny());
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.gvSampleBindingSource.RemoveAt(this.gvSample.FocusedRowHandle);
        }
        private void gcReport_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditRpt();
            GetDataSource();
        }
        private void txtInpatid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string inpatNo = this.txtInpatid.Text.Trim();
                GetPatient(inpatNo);
            }
        }
        private void textEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string inpatNo = this.txtIpNo.Text.Trim();
                GetPatient(inpatNo);
            }
        }
        private void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboDept.Properties.Items.Clear();
        }
        private void cboInfectionSite01_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboInfectionSite01.Properties.Items.Clear();
        }
        private void cboInfectionSite02_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboInfectionSite02.Properties.Items.Clear();
        }
        private void cboCausation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboCausation.Properties.Items.Clear();
        }
        private void gvReport_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            int hand = e.RowHandle;
            if (hand < 0) return;
            EntityInfectionus vo = this.gvReport.GetRow(hand) as EntityInfectionus;

            if (vo.status == 1)
            {
                e.Appearance.ForeColor = Color.Green;// 改变行字体颜色
            }
            if (vo.status == 2)
            {
                e.Appearance.ForeColor = Color.Red;// 改变行字体颜色
            }
            vo = null;
        }
        private void cboInfectionSite01_TextChanged(object sender, EventArgs e)
        {
            this.cboInfectionSite01.Properties.Items.Clear();

            if (!string.IsNullOrEmpty(this.cboInfectionSite01.Text))
            {
                for (int i = 0; i < lstinfectSource.Count; i++)
                {
                    if (lstinfectSource[i].Contains(this.cboInfectionSite01.Text))
                    {
                        this.cboInfectionSite01.Properties.Items.Add(lstinfectSource[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < lstinfectSource.Count; i++)
                {
                    if (lstinfectSource[i].Contains(this.cboInfectionSite01.Text))
                    {
                        this.cboInfectionSite01.Properties.Items.Add(lstinfectSource[i]);
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.cboInfectionSite01.Text) && this.cboInfectionSite01.Properties.Items.Count > 0)
                this.cboInfectionSite01.ShowPopup();
        }
        private void cboInfectionSite02_TextChanged(object sender, EventArgs e)
        {
            this.cboInfectionSite02.Properties.Items.Clear();

            if (!string.IsNullOrEmpty(this.cboInfectionSite02.Text))
            {
                for (int i = 0; i < lstinfectSource.Count; i++)
                {
                     if (lstinfectSource[i].Contains(this.cboInfectionSite02.Text))
                    {
                        this.cboInfectionSite02.Properties.Items.Add(lstinfectSource[i]);
                    }
                }   
            }
            else
            {
                for (int i = 0; i < lstinfectSource.Count; i++)
                {
                    if (lstinfectSource[i].Contains(this.cboInfectionSite02.Text))
                    {
                        this.cboInfectionSite02.Properties.Items.Add(lstinfectSource[i]);
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.cboInfectionSite02.Text) && this.cboInfectionSite02.Properties.Items.Count > 0)
                this.cboInfectionSite02.ShowPopup();
        }
        private void cboCausation_TextChanged(object sender, EventArgs e)
        {
            this.cboCausation.Properties.Items.Clear();

            if (!string.IsNullOrEmpty(this.cboCausation.Text))
            {

                for (int i = 0; i < lstCausationSource.Count; i++)
                {
                    if (lstCausationSource[i].Contains(this.cboCausation.Text))
                    {
                        this.cboCausation.Properties.Items.Add(lstCausationSource[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < lstCausationSource.Count; i++)
                {
                    if (lstCausationSource[i].Contains(this.cboCausation.Text))
                    {
                        this.cboCausation.Properties.Items.Add(lstCausationSource[i]);
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.cboCausation.Text) && this.cboCausation.Properties.Items.Count > 0)
                this.cboCausation.ShowPopup();
        }
        private void cboDeptList_TextChanged(object sender, EventArgs e)
        {
            this.cboPatDept.Properties.Items.Clear();

            if (!string.IsNullOrEmpty(this.cboPatDept.Text))
            {

                for (int i = 0; i < lstDeptList.Count; i++)
                {
                    if (lstDeptList[i].deptName.Contains(this.cboPatDept.Text))
                    {
                        this.cboPatDept.Properties.Items.Add(lstDeptList[i].deptName);
                    }
                }
            }
            else
            {
                for (int i = 0; i < lstDeptList.Count; i++)
                {
                    if (lstDeptList[i].deptName.Contains(this.cboPatDept.Text))
                    {
                        this.cboPatDept.Properties.Items.Add(lstDeptList[i].deptName);
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.cboPatDept.Text) && this.cboPatDept.Properties.Items.Count > 0)
                this.cboPatDept.ShowPopup();
        }
        private void cboDept_TextChanged(object sender, EventArgs e)
        {
            this.cboDept.Properties.Items.Clear();

            if (!string.IsNullOrEmpty(this.cboDept.Text))
            {

                for (int i = 0; i < lstDeptList.Count; i++)
                {
                    if (lstDeptList[i].deptName.Contains(this.cboDept.Text))
                    {
                        this.cboDept.Properties.Items.Add(lstDeptList[i].deptName);
                    }
                }
            }
            else
            {
                for (int i = 0; i < lstDeptList.Count; i++)
                {
                    if (lstDeptList[i].deptName.Contains(this.cboDept.Text))
                    {
                        this.cboDept.Properties.Items.Add(lstDeptList[i].deptName);
                    }
                }
            }
            this.cboDept.ShowPopup();
        }

        #endregion 

        #region 方法
        /// <summary>
        /// Init
        /// </summary>
        #region
        void Init()
        {
            DateTime dtmNow = DateTime.Now;
            this.dteDateStart.DateTime = new DateTime(dtmNow.Year, dtmNow.Month, 1);
            this.dteDateEnd.DateTime = dtmNow;
            this.gvSampleBindingSource = new BindingSource();
            this.gcSample.DataSource = this.gvSampleBindingSource;
            lstinfectSource = new List<string>();

            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    this.gvSampleBindingSource.DataSource = proxy.Service.GetPathogeny(-1);
                    lstinfectSource = proxy.Service.GetInfectionSiteSource();
                    lstCausationSource = proxy.Service.GetCausationSource();
                    lstDrugSource = proxy.Service.GetDrugSource();
                    lstSampleSource = proxy.Service.GetSampleSource();
                    lstDeptList = proxy.Service.getDeptList();

                    for (int i = 0; i < lstinfectSource.Count; i++)
                    {
                        this.cboInfectionSite01.Properties.Items.Add(lstinfectSource[i]);
                        this.cboInfectionSite02.Properties.Items.Add(lstinfectSource[i]);
                    }

                    for (int i = 0; i < lstCausationSource.Count; i++)
                        this.cboCausation.Properties.Items.Add(lstCausationSource[i]);
                    this.repositoryItemCboDrugName.Items.Add("是");
                    this.repositoryItemCboDrugName.Items.Add("否");
                    for (int i = 0; i < lstSampleSource.Count; i++)
                        this.repositoryItemCboSample.Items.Add(lstSampleSource[i]);
                    for (int i = 0; i < lstDeptList.Count; i++)
                    {
                        this.cboPatDept.Properties.Items.Add(lstDeptList[i].deptName);
                        this.cboDept.Properties.Items.Add(lstDeptList[i].deptName);
                    }
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
            
            this.OwerDeptCode = string.Empty;
            foreach (EntityCodeDepartment item in GlobalLogin.objLogin.lstDept)
                this.OwerDeptCode += "'" + item.deptCode + "',";
            if (this.OwerDeptCode != string.Empty) this.OwerDeptCode = this.OwerDeptCode.TrimEnd(',');

            this.RoleQuery = string.Empty;
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                this.RoleQuery = proxy.Service.GetInfectReportRoleQuery(GlobalLogin.objLogin.EmpNo);
            
            this.Query();
        }
        #endregion

        #region InitXr
        /// <summary>
        /// Init
        /// </summary>
        void InitXr()
        {
            #region xr
            decimal printId = 9;
            EntitySysReport rptVo = null;
            using (ProxyCommon proxy = new ProxyCommon())
            {
                rptVo = proxy.Service.GetReport(printId);
            }
            xr = new XtraReport();
            if (rptVo != null)
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(rptVo.rptFile, 0, rptVo.rptFile.Length);
                xr.LoadLayout(ms);
            }
            xr.CreateDocument();
            #endregion
        }

        #endregion

        /// <summary>
        /// Query
        /// </summary>
        #region 
        internal void Query()
        {
            List<EntityParm> dicParm = new List<EntityParm>();
            string beginDate = this.dteDateStart.Text.Trim();
            string endDate = this.dteDateEnd.Text.Trim();

            if (beginDate != string.Empty && endDate != string.Empty)
            {
                if (Function.Datetime(beginDate + " 00:00:00") > Function.Datetime(endDate + " 00:00:00"))
                {
                    DialogBox.Msg("开始时间不能大于结束时间。");
                    return;
                }
                dicParm.Add(Function.GetParm("reportDate", beginDate + "|" + endDate));
            }
            if (this.txtCardNo.Text.Trim() != string.Empty)
            {
                dicParm.Add(Function.GetParm("cardNo", txtCardNo.Text.Trim()));
            }
            if (this.txtPatName.Text.Trim() != string.Empty)
            {
                dicParm.Add(Function.GetParm("patName", txtPatName.Text.Trim()));
            }
            if (this.cboDept.Text != null && !string.IsNullOrEmpty(this.cboDept.Text))
            {
                string deptCode = string.Empty;
                for (int i = 0; i < lstDeptList.Count;i++ )
                {
                    if (lstDeptList[i].deptName == this.cboDept.Text)
                        deptCode = lstDeptList[i].deptCode;
                }
                if(!string.IsNullOrEmpty(deptCode))
                    dicParm.Add(Function.GetParm("deptCode", deptCode));
            }
            if (this.RoleQuery != string.Empty)
            {
                
            }
            else  
            {
                dicParm.Add(Function.GetParm("areaStr", this.OwerDeptCode));
                dicParm.Add(Function.GetParm("selfId", GlobalLogin.objLogin.EmpNo));
            }

            try
            {
                if (dicParm.Count > 0)
                {
                    using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                    {
                        List<EntityInfectionus> dataSource = proxy.Service.GetInfectionusList(dicParm);
                       this.gcReport.DataSource = dataSource;
                    }
                }
                else
                {
                    DialogBox.Msg("请输入查询条件。");
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }
        #endregion


        /// <summary>
        /// EditRpt
        /// </summary>
        #region 
        void EditRpt()
        {
            EntityPatientInfo pat = new EntityPatientInfo();
            vo = GetRowObject();
            decimal rptId = vo.rptId;
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                this.gvSampleBindingSource.DataSource = proxy.Service.GetPathogeny(rptId);
            }

            this.txtAge.Text = vo.patAge;
            this.txtBedNo.Text = vo.bedNo;
            this.txtInpatid.Text = vo.inpatNo;
            this.txtIpNo.Text = vo.inpatNo;
            this.cboPatDept.Text = vo.deptName;
            this.cboPatDept.Properties.Items.Clear();
            this.txtName.Text = vo.patName;
            this.txtSex.Text = vo.patSex;
            this.dteInDate.Text = vo.dateIn.ToString();
            this.mmtDiagnose.Text = vo.inDiagnosis;
            if (vo.incisionType == 1)
                this.chk01.Checked = true;
            if (vo.incisionType == 2)
                this.chk02.Checked = true;
            if (vo.incisionType == 3)
                this.chk03.Checked = true;
            if (vo.incisionType == 4)
                this.chk04.Checked = true;

            this.cboInfectionSite01.Text = vo.infectionSite01;
            this.cboInfectionSite01.Properties.Items.Clear();
            this.cboInfectionSite02.Text = vo.infectionSite02;
            this.cboInfectionSite02.Properties.Items.Clear();
            this.dteInfection01.Text = vo.infectionDate01.ToString();
            this.dteInfection02.Text = vo.infectionDate02.ToString();
            this.cboCausation.Text = vo.infectionReason;
            this.cboCausation.Properties.Items.Clear();
            this.txtDoctor.Text = vo.doctName;
            this.dteOperationDate.Text = vo.operationDate.ToString();
            this.txtOperationName.Text = vo.operationName;
            this.txtName.Tag = pat;
        }
        #endregion

        /// <summary>
        /// ConfirmRpt
        /// </summary>
        /// <param name="flg">1 审核通过 2 审核未通过 0 反审核</param>
        #region
        void ConfirmRpt(int flg)
        {
            vo = GetRowObject();
            if (vo.rptId <= 0)
                return;
            if ((flg == 1 && vo.status == 1) || (flg == 2 && vo.status == 2))
                DialogBox.Msg("已审核");
            else
            {
                vo.status = flg;
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    if (proxy.Service.ComfirmRpt(vo) > 0)
                    {
                        if (flg == 1)
                        {
                            if (this.gvReport.FocusedRowHandle > 0)
                                this.gvReport.Appearance.SelectedRow.ForeColor = Color.Green;
                            DialogBox.Msg("审核成功");
                        }
                        else if (flg == 2)
                        {
                            if (this.gvReport.FocusedRowHandle > 0)
                                this.gvReport.Appearance.SelectedRow.ForeColor = Color.Red;
                            DialogBox.Msg("审核成功");
                        }
                        else
                        {
                            if (this.gvReport.FocusedRowHandle > 0)
                                this.gvReport.Appearance.SelectedRow.ForeColor = Color.Black;
                            DialogBox.Msg("反审核成功");
                        }

                        vo = null;
                    }
                    else
                    {
                        if (flg == 1)
                            DialogBox.Msg("审核失败");
                        else
                            DialogBox.Msg("反审核失败");
                        vo = null;
                    }
                }
            }

        }
        #endregion

        /// <summary>
        /// DeleteRpt
        /// </summary>
        #region
        void DeleteRpt()
        {
            vo = GetRowObject();
            if (vo.rptId <= 0)
                return;
            else
            {
                vo.status = -1;
                using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                {
                    if (proxy.Service.ComfirmRpt(vo) > 0)
                    {
                        DialogBox.Msg("删除成功");
                        vo = null;
                    }
                    else
                    {
                        DialogBox.Msg("删除失败");
                        vo = null;
                    }
                }
            }
        }
        #endregion
        
        /// <summary>
        /// GetPatient  获取病人信息
        /// </summary>
        #region
        internal void GetPatient(string inpatId)
        {
            List<EntityPatientInfo> lstPat = null;
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                lstPat = proxy.Service.GetPatInfo(inpatId);
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
        #endregion

        /// <summary>
        /// SetPatValue
        /// </summary>
        #region
        void SetPatValue(EntityPatientInfo pat)
        {
            ClearCntrValue(this.pcConent);
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
                this.gvSampleBindingSource.DataSource = proxy.Service.GetPathogeny(-1);

            if (this.txtIpNo.Visible == true)
            {
                this.txtIpNo.Visible = false;
                this.txtInpatid.Visible = true;
            }
            else 
            {
                this.txtIpNo.Visible = true;
                this.txtInpatid.Visible = false;
            }

            if (this.txtInpatid.Visible == false)
            {
                this.txtInpatid.Visible = true;
                this.txtIpNo.Visible = true;
            }


            this.txtName.Tag = pat;
            this.txtInpatid.Text = pat.ipNo;
            this.txtIpNo.Text = pat.ipNo;
            this.txtName.Text = pat.name;
            this.txtSex.Text = pat.sexCH;
            this.cboPatDept.Text = pat.deptName;
            this.cboPatDept.Properties.Items.Clear();
            this.txtBedNo.Text = pat.bedNo;
            this.txtAge.Text = pat.age;
            this.txtDoctor.Text = pat.doctName;
            this.dteInDate.Text = pat.inDate.ToString();
            this.mmtDiagnose.Text = pat.email;
            this.txtOperationName.Text = pat.upFlag;
            this.dteOperationDate.Text = pat.zip.ToString();

            if (pat.tremType == "0")
                this.chk01.Checked = true;
            else if (pat.tremType == "I")
                this.chk02.Checked = true;
            else if (pat.tremType == "II")
                this.chk03.Checked = true;
            else if (pat.tremType == "III")
                this.chk04.Checked = true;
        }
        #endregion

        /// <summary>
        /// getInfectionusValue
        /// </summary>
        #region
        void GetInfectionusValue(ref EntityInfectionus vo,EntityPatientInfo pat)
        {
            vo.patName = pat.name;
            vo.inpatNo = pat.ipNo;
            vo.patSex = pat.sex;
            vo.patAge = pat.age;
            vo.deptCode = pat.deptCode;
            vo.birthDay = Function.Datetime((Function.Datetime(pat.birth)).ToString("yyyy-MM-dd"));
            vo.doctId = pat.doctCode;
            vo.dateIn = Function.Datetime(pat.inDate);
            vo.doctName = this.txtDoctor.Text;
            vo.inDiagnosis = this.mmtDiagnose.Text;

            if (this.chk01.Checked == true)
                vo.incisionType = 1;
            if(this.chk02.Checked == true)
                vo.incisionType = 2;
            if (this.chk03.Checked == true)
                vo.incisionType = 3;
            if (this.chk04.Checked == true)
                vo.incisionType = 4;

            vo.infectionSite01 = this.cboInfectionSite01.Text;
            vo.infectionSite02 = this.cboInfectionSite02.Text;
            vo.infectionDate01 = this.dteInfection01.Text;
            vo.infectionDate02 = this.dteInfection02.Text;
            vo.infectionReason = this.cboCausation.Text;
            vo.operationName = this.txtOperationName.Text;
            vo.operationDate = this.dteOperationDate.Text;
        }
        #endregion

        ///<summary>
        /// GetDataSource
        ///</summary>
        #region
        void GetDataSource()
        {
            using (ProxyAdverseEvent proxy = new ProxyAdverseEvent())
            {
                lstinfectSource = proxy.Service.GetInfectionSiteSource();
                lstCausationSource = proxy.Service.GetCausationSource();
                lstDeptList = proxy.Service.getDeptList();

                for (int i = 0; i < lstinfectSource.Count; i++)
                {
                    this.cboInfectionSite01.Properties.Items.Add(lstinfectSource[i]);
                    this.cboInfectionSite02.Properties.Items.Add(lstinfectSource[i]);
                }

                for (int i = 0; i < lstCausationSource.Count; i++)
                    this.cboCausation.Properties.Items.Add(lstCausationSource[i]);
                for (int i = 0; i < lstDeptList.Count; i++)
                {
                    this.cboPatDept.Properties.Items.Add(lstDeptList[i].deptName);
                    this.cboDept.Properties.Items.Add(lstDeptList[i].deptName);
                }
                    
            }
        }
        #endregion

        #region GetRowObject
        /// <summary>
        /// GetRowObject
        /// </summary>
        /// <returns></returns>
        EntityInfectionus GetRowObject()
        {
            if (this.gvReport.FocusedRowHandle < 0) return null;
            return this.gvReport.GetRow(this.gvReport.FocusedRowHandle) as EntityInfectionus;
        }
        #endregion

        ///<summary>
        /// ClearCntrValue
        ///</summary>
        ///<param name="parContainer">容器类控件</param>
        #region
        public void ClearCntrValue(Control parContainer)
        {
            for (int index = 0; index < parContainer.Controls.Count; index++)
            {
                // 如果是容器类控件，递归调用自己
                if (parContainer.Controls[index].HasChildren)
                {
                    ClearCntrValue(parContainer.Controls[index]);
                }
                else
                {
                    switch (parContainer.Controls[index].GetType().Name)
                    {
                        case "TextBoxMaskBox":
                            parContainer.Controls[index].Text = "";
                            break;
                        case "CheckEdit":
                            ((CheckEdit)(parContainer.Controls[index])).Checked = false;
                            break;
                        case "ComboBoxEdit":
                            ((ComboBoxEdit)(parContainer.Controls[index])).Properties.Items.Clear();
                            break;
                        case "DateEdit":
                            ((DateEdit)(parContainer.Controls[index])).EditValue = "";
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        #endregion

        private void cboPatDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cboPatDept.Properties.Items.Clear();
        }

        #endregion
  }
}