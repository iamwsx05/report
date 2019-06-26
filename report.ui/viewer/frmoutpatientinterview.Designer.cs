namespace Report.Ui
{
    partial class frmOutpatientInterview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOutpatientInterview));
            this.txtCardNo = new DevExpress.XtraEditors.TextEdit();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPatName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dteDateStart = new DevExpress.XtraEditors.DateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.dteDateEnd = new DevExpress.XtraEditors.DateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.gcReport = new DevExpress.XtraGrid.GridControl();
            this.gvReport = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCheck2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn42 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn24 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn29 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn25 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn26 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn28 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn43 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ucDept = new Common.Controls.ucDept();
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).BeginInit();
            this.pcBackGround.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCardNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // pcBackGround
            // 
            this.pcBackGround.Controls.Add(this.ucDept);
            this.pcBackGround.Controls.Add(this.txtCardNo);
            this.pcBackGround.Controls.Add(this.label9);
            this.pcBackGround.Controls.Add(this.txtPatName);
            this.pcBackGround.Controls.Add(this.labelControl2);
            this.pcBackGround.Controls.Add(this.dteDateStart);
            this.pcBackGround.Controls.Add(this.label3);
            this.pcBackGround.Controls.Add(this.dteDateEnd);
            this.pcBackGround.Controls.Add(this.label2);
            this.pcBackGround.Dock = System.Windows.Forms.DockStyle.Top;
            this.pcBackGround.Location = new System.Drawing.Point(0, 0);
            this.pcBackGround.Size = new System.Drawing.Size(1126, 35);
            this.pcBackGround.Visible = true;
            // 
            // defaultLookAndFeel
            // 
            this.defaultLookAndFeel.LookAndFeel.SkinName = "Office 2010 Blue";
            // 
            // marqueeProgressBarControl
            // 
            this.marqueeProgressBarControl.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // txtCardNo
            // 
            this.txtCardNo.Location = new System.Drawing.Point(381, 8);
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtCardNo.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.txtCardNo.Properties.Appearance.Options.UseFont = true;
            this.txtCardNo.Properties.Appearance.Options.UseForeColor = true;
            this.txtCardNo.Size = new System.Drawing.Size(90, 20);
            this.txtCardNo.TabIndex = 144;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(320, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 12);
            this.label9.TabIndex = 143;
            this.label9.Text = "住院号:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPatName
            // 
            this.txtPatName.Location = new System.Drawing.Point(523, 8);
            this.txtPatName.Name = "txtPatName";
            this.txtPatName.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10F);
            this.txtPatName.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.txtPatName.Properties.Appearance.Options.UseFont = true;
            this.txtPatName.Properties.Appearance.Options.UseForeColor = true;
            this.txtPatName.Size = new System.Drawing.Size(90, 20);
            this.txtPatName.TabIndex = 145;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(489, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(30, 12);
            this.labelControl2.TabIndex = 146;
            this.labelControl2.Text = "姓名:";
            // 
            // dteDateStart
            // 
            this.dteDateStart.EditValue = null;
            this.dteDateStart.Location = new System.Drawing.Point(76, 8);
            this.dteDateStart.Name = "dteDateStart";
            this.dteDateStart.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dteDateStart.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.dteDateStart.Properties.Appearance.Options.UseFont = true;
            this.dteDateStart.Properties.Appearance.Options.UseForeColor = true;
            this.dteDateStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDateStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDateStart.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dteDateStart.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dteDateStart.Size = new System.Drawing.Size(100, 22);
            this.dteDateStart.TabIndex = 139;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.label3.Location = new System.Drawing.Point(181, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 25);
            this.label3.TabIndex = 142;
            this.label3.Text = "至";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dteDateEnd
            // 
            this.dteDateEnd.EditValue = null;
            this.dteDateEnd.Location = new System.Drawing.Point(201, 8);
            this.dteDateEnd.Name = "dteDateEnd";
            this.dteDateEnd.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dteDateEnd.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.dteDateEnd.Properties.Appearance.Options.UseFont = true;
            this.dteDateEnd.Properties.Appearance.Options.UseForeColor = true;
            this.dteDateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDateEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteDateEnd.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dteDateEnd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dteDateEnd.Size = new System.Drawing.Size(100, 22);
            this.dteDateEnd.TabIndex = 141;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.label2.Location = new System.Drawing.Point(16, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 25);
            this.label2.TabIndex = 140;
            this.label2.Text = "随访日期:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gcReport
            // 
            this.gcReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcReport.Location = new System.Drawing.Point(0, 35);
            this.gcReport.MainView = this.gvReport;
            this.gcReport.MenuManager = this.barManager;
            this.gcReport.Name = "gcReport";
            this.gcReport.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.colCheck});
            this.gcReport.Size = new System.Drawing.Size(1126, 444);
            this.gcReport.TabIndex = 16;
            this.gcReport.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvReport});
            // 
            // gvReport
            // 
            this.gvReport.Appearance.HeaderPanel.Font = new System.Drawing.Font("宋体", 9F);
            this.gvReport.Appearance.HeaderPanel.Options.UseFont = true;
            this.gvReport.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.gvReport.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gvReport.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gvReport.Appearance.Row.Font = new System.Drawing.Font("宋体", 9F);
            this.gvReport.Appearance.Row.Options.UseFont = true;
            this.gvReport.Appearance.ViewCaption.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold);
            this.gvReport.Appearance.ViewCaption.Options.UseFont = true;
            this.gvReport.ColumnPanelRowHeight = 22;
            this.gvReport.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCheck2,
            this.gridColumn42,
            this.gridColumn24,
            this.gridColumn29,
            this.gridColumn25,
            this.gridColumn4,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn5,
            this.gridColumn26,
            this.gridColumn3,
            this.gridColumn28,
            this.gridColumn43});
            this.gvReport.GridControl = this.gcReport;
            this.gvReport.IndicatorWidth = 38;
            this.gvReport.Name = "gvReport";
            this.gvReport.OptionsPrint.PrintHeader = false;
            this.gvReport.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvReport.OptionsView.ColumnAutoWidth = false;
            this.gvReport.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvReport.OptionsView.ShowGroupPanel = false;
            this.gvReport.OptionsView.ShowViewCaption = true;
            this.gvReport.RowHeight = 27;
            this.gvReport.ViewCaptionHeight = 26;
            this.gvReport.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvReport_CustomDrawRowIndicator);
            this.gvReport.DoubleClick += new System.EventHandler(this.gvReport_DoubleClick);
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
            // gridColumn42
            // 
            this.gridColumn42.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn42.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn42.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn42.Caption = "患者姓名";
            this.gridColumn42.FieldName = "patName";
            this.gridColumn42.Name = "gridColumn42";
            this.gridColumn42.OptionsColumn.AllowEdit = false;
            this.gridColumn42.Visible = true;
            this.gridColumn42.VisibleIndex = 0;
            this.gridColumn42.Width = 84;
            // 
            // gridColumn24
            // 
            this.gridColumn24.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn24.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn24.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn24.Caption = "住院号";
            this.gridColumn24.FieldName = "patNo";
            this.gridColumn24.Name = "gridColumn24";
            this.gridColumn24.OptionsColumn.AllowEdit = false;
            this.gridColumn24.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn24.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn24.OptionsFilter.AllowFilter = false;
            this.gridColumn24.Visible = true;
            this.gridColumn24.VisibleIndex = 1;
            this.gridColumn24.Width = 93;
            // 
            // gridColumn29
            // 
            this.gridColumn29.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn29.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn29.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn29.Caption = "性别";
            this.gridColumn29.FieldName = "patSex";
            this.gridColumn29.Name = "gridColumn29";
            this.gridColumn29.OptionsColumn.AllowEdit = false;
            this.gridColumn29.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn29.OptionsFilter.AllowFilter = false;
            this.gridColumn29.Visible = true;
            this.gridColumn29.VisibleIndex = 2;
            this.gridColumn29.Width = 48;
            // 
            // gridColumn25
            // 
            this.gridColumn25.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn25.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn25.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn25.Caption = "年龄";
            this.gridColumn25.FieldName = "patAge";
            this.gridColumn25.Name = "gridColumn25";
            this.gridColumn25.OptionsColumn.AllowEdit = false;
            this.gridColumn25.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn25.OptionsFilter.AllowFilter = false;
            this.gridColumn25.Visible = true;
            this.gridColumn25.VisibleIndex = 3;
            this.gridColumn25.Width = 54;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "住院次数";
            this.gridColumn4.FieldName = "inCount";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            this.gridColumn4.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn4.OptionsFilter.AllowFilter = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 4;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn6.AppearanceCell.Options.UseFont = true;
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "入院时间";
            this.gridColumn6.FieldName = "inDeptTime";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            this.gridColumn6.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn6.OptionsFilter.AllowFilter = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 120;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn7.AppearanceCell.Options.UseFont = true;
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "入院科室";
            this.gridColumn7.FieldName = "inDeptName";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsColumn.AllowFocus = false;
            this.gridColumn7.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn7.OptionsFilter.AllowFilter = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            this.gridColumn7.Width = 150;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "出院时间";
            this.gridColumn1.FieldName = "outHospitalTime";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn1.OptionsFilter.AllowFilter = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 7;
            this.gridColumn1.Width = 120;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "出院科室";
            this.gridColumn2.FieldName = "outDeptName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 8;
            this.gridColumn2.Width = 150;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "主管医生";
            this.gridColumn5.FieldName = "doctName";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn5.OptionsFilter.AllowFilter = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 9;
            // 
            // gridColumn26
            // 
            this.gridColumn26.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn26.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn26.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn26.Caption = "联系电话";
            this.gridColumn26.FieldName = "contactTel";
            this.gridColumn26.Name = "gridColumn26";
            this.gridColumn26.OptionsColumn.AllowEdit = false;
            this.gridColumn26.OptionsColumn.AllowFocus = false;
            this.gridColumn26.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn26.OptionsFilter.AllowFilter = false;
            this.gridColumn26.Visible = true;
            this.gridColumn26.VisibleIndex = 10;
            this.gridColumn26.Width = 137;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "现住址";
            this.gridColumn3.FieldName = "contactAddr";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            this.gridColumn3.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn3.OptionsFilter.AllowFilter = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 11;
            this.gridColumn3.Width = 200;
            // 
            // gridColumn28
            // 
            this.gridColumn28.AppearanceCell.ForeColor = System.Drawing.Color.Red;
            this.gridColumn28.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn28.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn28.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn28.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn28.Caption = "随访时间";
            this.gridColumn28.FieldName = "interviewTime";
            this.gridColumn28.Name = "gridColumn28";
            this.gridColumn28.OptionsColumn.AllowEdit = false;
            this.gridColumn28.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn28.OptionsFilter.AllowFilter = false;
            this.gridColumn28.Visible = true;
            this.gridColumn28.VisibleIndex = 12;
            this.gridColumn28.Width = 125;
            // 
            // gridColumn43
            // 
            this.gridColumn43.AppearanceCell.ForeColor = System.Drawing.Color.Red;
            this.gridColumn43.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn43.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn43.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn43.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn43.Caption = "随访人";
            this.gridColumn43.FieldName = "interviewName";
            this.gridColumn43.Name = "gridColumn43";
            this.gridColumn43.OptionsColumn.AllowEdit = false;
            this.gridColumn43.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn43.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn43.OptionsFilter.AllowFilter = false;
            this.gridColumn43.Visible = true;
            this.gridColumn43.VisibleIndex = 13;
            this.gridColumn43.Width = 117;
            // 
            // ucDept
            // 
            this.ucDept.DeptName = "";
            this.ucDept.DeptVo = null;
            this.ucDept.IsShowOwnDept = false;
            this.ucDept.Location = new System.Drawing.Point(632, 8);
            this.ucDept.Name = "ucDept";
            this.ucDept.Size = new System.Drawing.Size(159, 23);
            this.ucDept.TabIndex = 147;
            this.ucDept.Visible = false;
            // 
            // frmOutpatientInterview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 479);
            this.Controls.Add(this.gcReport);
            this.Name = "frmOutpatientInterview";
            this.Text = "随访记录";
            this.Load += new System.EventHandler(this.frmOutpatientInterview_Load);
            this.Controls.SetChildIndex(this.marqueeProgressBarControl, 0);
            this.Controls.SetChildIndex(this.pcBackGround, 0);
            this.Controls.SetChildIndex(this.gcReport, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).EndInit();
            this.pcBackGround.ResumeLayout(false);
            this.pcBackGround.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCardNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colCheck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevExpress.XtraEditors.TextEdit txtCardNo;
        private System.Windows.Forms.Label label9;
        internal DevExpress.XtraEditors.TextEdit txtPatName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        internal DevExpress.XtraEditors.DateEdit dteDateStart;
        private System.Windows.Forms.Label label3;
        internal DevExpress.XtraEditors.DateEdit dteDateEnd;
        private System.Windows.Forms.Label label2;
        internal DevExpress.XtraGrid.GridControl gcReport;
        internal DevExpress.XtraGrid.Views.Grid.GridView gvReport;
        private DevExpress.XtraGrid.Columns.GridColumn colCheck2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit colCheck;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn28;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn43;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn24;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn42;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn29;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn25;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn26;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        internal Common.Controls.ucDept ucDept;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
    }
}