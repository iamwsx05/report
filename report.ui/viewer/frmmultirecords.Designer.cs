namespace Report.Ui
{
    partial class frmMultiRecords
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMultiRecords));
            this.gcPats = new DevExpress.XtraGrid.GridControl();
            this.gvPats = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCheck2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn28 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn43 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn24 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn42 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn29 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn25 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn26 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn41 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).BeginInit();
            this.pcBackGround.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // pcBackGround
            // 
            this.pcBackGround.Controls.Add(this.gcPats);
            this.pcBackGround.Size = new System.Drawing.Size(770, 230);
            // 
            // defaultLookAndFeel
            // 
            this.defaultLookAndFeel.LookAndFeel.SkinName = "Office 2010 Blue";
            // 
            // marqueeProgressBarControl
            // 
            this.marqueeProgressBarControl.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // gcPats
            // 
            this.gcPats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcPats.Location = new System.Drawing.Point(2, 2);
            this.gcPats.MainView = this.gvPats;
            this.gcPats.Name = "gcPats";
            this.gcPats.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.colCheck});
            this.gcPats.Size = new System.Drawing.Size(766, 226);
            this.gcPats.TabIndex = 16;
            this.gcPats.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvPats});
            // 
            // gvPats
            // 
            this.gvPats.Appearance.HeaderPanel.Font = new System.Drawing.Font("宋体", 9F);
            this.gvPats.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvPats.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvPats.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvPats.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvPats.Appearance.Row.Font = new System.Drawing.Font("宋体", 9F);
            this.gvPats.Appearance.Row.Options.UseFont = true;
            this.gvPats.Appearance.ViewCaption.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold);
            this.gvPats.Appearance.ViewCaption.Options.UseFont = true;
            this.gvPats.ColumnPanelRowHeight = 22;
            this.gvPats.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCheck2,
            this.gridColumn28,
            this.gridColumn43,
            this.gridColumn17,
            this.gridColumn21,
            this.gridColumn24,
            this.gridColumn42,
            this.gridColumn29,
            this.gridColumn25,
            this.gridColumn26,
            this.gridColumn41});
            this.gvPats.GridControl = this.gcPats;
            this.gvPats.IndicatorWidth = 38;
            this.gvPats.Name = "gvPats";
            this.gvPats.OptionsPrint.PrintHeader = false;
            this.gvPats.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvPats.OptionsView.ColumnAutoWidth = false;
            this.gvPats.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvPats.OptionsView.ShowGroupPanel = false;
            this.gvPats.OptionsView.ShowViewCaption = true;
            this.gvPats.RowHeight = 27;
            this.gvPats.ViewCaptionHeight = 26;
            // 
            // colCheck2
            // 
            this.colCheck2.Caption = "  ";
            this.colCheck2.ColumnEdit = this.colCheck;
            this.colCheck2.FieldName = "check";
            this.colCheck2.Name = "colCheck2";
            this.colCheck2.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.colCheck2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.colCheck2.OptionsFilter.AllowAutoFilter = false;
            this.colCheck2.OptionsFilter.AllowFilter = false;
            this.colCheck2.Width = 25;
            // 
            // colCheck
            // 
            this.colCheck.AutoHeight = false;
            this.colCheck.Caption = "";
            this.colCheck.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.colCheck.Name = "colCheck";
            this.colCheck.PictureChecked = ((System.Drawing.Image)(resources.GetObject("colCheck.PictureChecked")));
            this.colCheck.ValueChecked = 1;
            this.colCheck.ValueUnchecked = 0;
            // 
            // gridColumn28
            // 
            this.gridColumn28.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn28.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn28.Caption = "上报时间";
            this.gridColumn28.FieldName = "reportTime";
            this.gridColumn28.Name = "gridColumn28";
            this.gridColumn28.OptionsColumn.AllowEdit = false;
            this.gridColumn28.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn28.OptionsFilter.AllowFilter = false;
            this.gridColumn28.Visible = true;
            this.gridColumn28.VisibleIndex = 0;
            this.gridColumn28.Width = 125;
            // 
            // gridColumn43
            // 
            this.gridColumn43.Caption = "报告类型";
            this.gridColumn43.FieldName = "reportType";
            this.gridColumn43.Name = "gridColumn43";
            this.gridColumn43.OptionsColumn.AllowEdit = false;
            this.gridColumn43.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn43.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn43.OptionsFilter.AllowFilter = false;
            this.gridColumn43.Visible = true;
            this.gridColumn43.VisibleIndex = 1;
            this.gridColumn43.Width = 79;
            // 
            // gridColumn17
            // 
            this.gridColumn17.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn17.AppearanceCell.ForeColor = System.Drawing.Color.Crimson;
            this.gridColumn17.AppearanceCell.Options.UseFont = true;
            this.gridColumn17.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn17.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn17.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn17.Caption = "事件编码";
            this.gridColumn17.FieldName = "eventCode";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.OptionsColumn.AllowEdit = false;
            this.gridColumn17.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn17.OptionsFilter.AllowFilter = false;
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 2;
            this.gridColumn17.Width = 92;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "不良事件名称";
            this.gridColumn21.FieldName = "eventName";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.OptionsColumn.AllowEdit = false;
            this.gridColumn21.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn21.OptionsFilter.AllowFilter = false;
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 3;
            this.gridColumn21.Width = 197;
            // 
            // gridColumn24
            // 
            this.gridColumn24.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn24.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn24.Caption = "病历/门诊号";
            this.gridColumn24.FieldName = "patNo";
            this.gridColumn24.Name = "gridColumn24";
            this.gridColumn24.OptionsColumn.AllowEdit = false;
            this.gridColumn24.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn24.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn24.OptionsFilter.AllowFilter = false;
            this.gridColumn24.Visible = true;
            this.gridColumn24.VisibleIndex = 4;
            this.gridColumn24.Width = 88;
            // 
            // gridColumn42
            // 
            this.gridColumn42.Caption = "患者姓名";
            this.gridColumn42.FieldName = "patName";
            this.gridColumn42.Name = "gridColumn42";
            this.gridColumn42.OptionsColumn.AllowEdit = false;
            this.gridColumn42.Visible = true;
            this.gridColumn42.VisibleIndex = 5;
            // 
            // gridColumn29
            // 
            this.gridColumn29.Caption = "性别";
            this.gridColumn29.FieldName = "patSex";
            this.gridColumn29.Name = "gridColumn29";
            this.gridColumn29.OptionsColumn.AllowEdit = false;
            this.gridColumn29.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn29.OptionsFilter.AllowFilter = false;
            this.gridColumn29.Visible = true;
            this.gridColumn29.VisibleIndex = 6;
            this.gridColumn29.Width = 48;
            // 
            // gridColumn25
            // 
            this.gridColumn25.Caption = "年龄";
            this.gridColumn25.FieldName = "patAge";
            this.gridColumn25.Name = "gridColumn25";
            this.gridColumn25.OptionsColumn.AllowEdit = false;
            this.gridColumn25.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn25.OptionsFilter.AllowFilter = false;
            this.gridColumn25.Visible = true;
            this.gridColumn25.VisibleIndex = 7;
            this.gridColumn25.Width = 54;
            // 
            // gridColumn26
            // 
            this.gridColumn26.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn26.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn26.Caption = "联系电话";
            this.gridColumn26.FieldName = "contactTel";
            this.gridColumn26.Name = "gridColumn26";
            this.gridColumn26.OptionsColumn.AllowEdit = false;
            this.gridColumn26.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn26.OptionsFilter.AllowFilter = false;
            this.gridColumn26.Visible = true;
            this.gridColumn26.VisibleIndex = 9;
            this.gridColumn26.Width = 137;
            // 
            // gridColumn41
            // 
            this.gridColumn41.Caption = "科室名称";
            this.gridColumn41.FieldName = "deptName";
            this.gridColumn41.Name = "gridColumn41";
            this.gridColumn41.OptionsColumn.AllowEdit = false;
            this.gridColumn41.Visible = true;
            this.gridColumn41.VisibleIndex = 8;
            this.gridColumn41.Width = 117;
            // 
            // frmMultiRecords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 230);
            this.MaximizeBox = false;
            this.Name = "frmMultiRecords";
            this.Text = "选择记录  (双击可选择)";
            this.Load += new System.EventHandler(this.frmMultiRecords_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMultiRecords_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).EndInit();
            this.pcBackGround.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcPats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvPats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colCheck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevExpress.XtraGrid.GridControl gcPats;
        internal DevExpress.XtraGrid.Views.Grid.GridView gvPats;
        private DevExpress.XtraGrid.Columns.GridColumn colCheck2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit colCheck;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn28;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn43;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn21;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn24;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn42;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn29;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn25;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn26;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn41;
    }
}