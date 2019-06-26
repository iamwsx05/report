using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Entity;
using System.Collections;
using DevExpress.XtraGrid;

namespace Report.Ui
{
    /// <summary>
    /// 编辑3
    /// </summary>
    public partial class frmAnaEdit3 : frmBasePopup
    {
        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        public frmAnaEdit3()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                this.defaultLookAndFeel.LookAndFeel.SkinName = GlobalLogin.SkinName;
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(GlobalLogin.SkinName);
            }
        }
        #endregion

        #region 属性.变量

        /// <summary>
        /// 数据源
        /// </summary>
        BindingSource gvDataBindingSource { get; set; }

        #endregion

        #region 方法

        #region Init
        /// <summary>
        /// Init
        /// </summary>
        void Init()
        {
            this.gvDataBindingSource = new BindingSource();
            this.gcData.DataSource = this.gvDataBindingSource;
            try
            {
                uiHelper.BeginLoading(this);
                using (ProxyAnaReport proxy = new ProxyAnaReport())
                {
                    this.gvDataBindingSource.DataSource = proxy.Service.GetAnaStatTemp();
                }
            }
            finally
            {
                uiHelper.CloseLoading(this);
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Save
        /// </summary>
        void Save()
        {
            this.gvData.CloseEditor();
            List<EntityAnaStatTemp> data = new List<EntityAnaStatTemp>();
            EntityAnaStatTemp vo = null;
            for (int i = this.gvData.RowCount - 1; i >= 0; i--)
            {
                if (this.gvData.GetRowCellValue(i, "Fmonth") != null && !string.IsNullOrEmpty(this.gvData.GetRowCellValue(i, "Fmonth").ToString()))
                {
                    vo = new EntityAnaStatTemp();
                    vo.Fmonth = Function.Datetime(this.gvData.GetRowCellValue(i, "Fmonth").ToString()).ToString("yyyy-MM");
                    vo.Field1 = Function.Int(this.gvData.GetRowCellValue(i, "Field1").ToString());
                    vo.Field2 = Function.Int(this.gvData.GetRowCellValue(i, "Field2").ToString());
                    vo.Field3 = Function.Int(this.gvData.GetRowCellValue(i, "Field3").ToString());
                    vo.Field4 = Function.Int(this.gvData.GetRowCellValue(i, "Field4").ToString());
                    if (data.Any(t => t.Fmonth == vo.Fmonth))
                    {
                        DialogBox.Msg("月份存在相同，请检查。");
                        return;
                    }
                    data.Add(vo);
                }
                else
                {
                    this.gvData.DeleteRow(i);
                }
            }
            if (DialogBox.Msg("确认保存？", MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (ProxyAnaReport proxy = new ProxyAnaReport())
                    {
                        if (proxy.Service.SaveAnaStatTemp(data) > 0)
                        {
                            DialogBox.Msg("保存成功！");
                        }
                        else
                        {
                            DialogBox.Msg("保存失败。");
                        }
                    }
                }
                catch (Exception ex)
                {
                    DialogBox.Msg(ex.Message);
                }
            }
        }
        #endregion

        #endregion

        #region 事件

        private void frmAnaEdit3_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void blbiAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.gvDataBindingSource.Add(new EntityAnaStatTemp());
        }

        private void blbiDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.gvDataBindingSource.RemoveAt(this.gvData.FocusedRowHandle);
        }

        private void blbiSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Save();
        }

        private void blbiExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            uiHelper.ExportToXls(this.gvData);
        }

        private void blbiPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            uiHelper.Print(this.gcData);
        }

        private void blblClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
