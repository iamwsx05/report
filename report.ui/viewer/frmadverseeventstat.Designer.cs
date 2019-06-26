
namespace Report.Ui
{
    partial class frmAdverseEventStat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdverseEventStat));
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.cboLevel = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cboEventType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.dteStart = new DevExpress.XtraEditors.DateEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.dteEnd = new DevExpress.XtraEditors.DateEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gcReport = new DevExpress.XtraGrid.GridControl();
            this.gvReport = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCheck2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn28 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn43 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn44 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn24 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn42 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn29 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn25 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn26 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn41 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ucDept = new Common.Controls.ucDept();
            this.lueReporter = new Common.Controls.LookUpEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lblTip = new System.Windows.Forms.Label();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).BeginInit();
            this.pcBackGround.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLevel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEventType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.colCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueReporter.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pcBackGround
            // 
            this.pcBackGround.Controls.Add(this.lblTip);
            this.pcBackGround.Controls.Add(this.labelControl1);
            this.pcBackGround.Controls.Add(this.lueReporter);
            this.pcBackGround.Controls.Add(this.ucDept);
            this.pcBackGround.Controls.Add(this.labelControl3);
            this.pcBackGround.Controls.Add(this.cboLevel);
            this.pcBackGround.Controls.Add(this.labelControl2);
            this.pcBackGround.Controls.Add(this.cboEventType);
            this.pcBackGround.Controls.Add(this.dteStart);
            this.pcBackGround.Controls.Add(this.labelControl7);
            this.pcBackGround.Controls.Add(this.dteEnd);
            this.pcBackGround.Dock = System.Windows.Forms.DockStyle.Top;
            this.pcBackGround.Location = new System.Drawing.Point(0, 0);
            this.pcBackGround.Size = new System.Drawing.Size(1195, 35);
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
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Location = new System.Drawing.Point(680, 10);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(54, 12);
            this.labelControl3.TabIndex = 115;
            this.labelControl3.Text = "事件等级:";
            // 
            // cboLevel
            // 
            this.cboLevel.Location = new System.Drawing.Point(744, 7);
            this.cboLevel.MenuManager = this.barManager;
            this.cboLevel.Name = "cboLevel";
            this.cboLevel.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboLevel.Size = new System.Drawing.Size(50, 20);
            this.cboLevel.TabIndex = 114;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(456, 10);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(54, 12);
            this.labelControl2.TabIndex = 113;
            this.labelControl2.Text = "事件类型:";
            // 
            // cboEventType
            // 
            this.cboEventType.Location = new System.Drawing.Point(520, 7);
            this.cboEventType.MenuManager = this.barManager;
            this.cboEventType.Name = "cboEventType";
            this.cboEventType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboEventType.Size = new System.Drawing.Size(150, 20);
            this.cboEventType.TabIndex = 112;
            // 
            // dteStart
            // 
            this.dteStart.EditValue = null;
            this.dteStart.Location = new System.Drawing.Point(88, 7);
            this.dteStart.Name = "dteStart";
            this.dteStart.Properties.Appearance.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.dteStart.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.dteStart.Properties.Appearance.Options.UseFont = true;
            this.dteStart.Properties.Appearance.Options.UseForeColor = true;
            this.dteStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteStart.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dteStart.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dteStart.Size = new System.Drawing.Size(100, 22);
            this.dteStart.TabIndex = 107;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl7.Location = new System.Drawing.Point(14, 12);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(72, 12);
            this.labelControl7.TabIndex = 109;
            this.labelControl7.Text = "查询时间 从:";
            // 
            // dteEnd
            // 
            this.dteEnd.EditValue = null;
            this.dteEnd.Location = new System.Drawing.Point(192, 7);
            this.dteEnd.Name = "dteEnd";
            this.dteEnd.Properties.Appearance.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.dteEnd.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.dteEnd.Properties.Appearance.Options.UseFont = true;
            this.dteEnd.Properties.Appearance.Options.UseForeColor = true;
            this.dteEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteEnd.Properties.Mask.EditMask = "yyyy-MM-dd";
            this.dteEnd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dteEnd.Size = new System.Drawing.Size(100, 22);
            this.dteEnd.TabIndex = 108;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gcReport);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 35);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1195, 543);
            this.panelControl1.TabIndex = 11;
            // 
            // gcReport
            // 
            this.gcReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcReport.Location = new System.Drawing.Point(2, 2);
            this.gcReport.MainView = this.gvReport;
            this.gcReport.MenuManager = this.barManager;
            this.gcReport.Name = "gcReport";
            this.gcReport.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.colCheck});
            this.gcReport.Size = new System.Drawing.Size(1191, 539);
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
            this.gridColumn28,
            this.gridColumn43,
            this.gridColumn44,
            this.gridColumn2,
            this.gridColumn17,
            this.gridColumn1,
            this.gridColumn21,
            this.gridColumn24,
            this.gridColumn42,
            this.gridColumn29,
            this.gridColumn25,
            this.gridColumn26,
            this.gridColumn41});
            this.gvReport.GridControl = this.gcReport;
            this.gvReport.IndicatorWidth = 38;
            this.gvReport.Name = "gvReport";
            this.gvReport.OptionsPrint.PrintHeader = false;
            this.gvReport.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvReport.OptionsView.ColumnAutoWidth = false;
            this.gvReport.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvReport.OptionsView.ShowGroupPanel = false;
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
            // gridColumn28
            // 
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
            this.gridColumn43.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn43.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn43.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn43.Caption = "上报人";
            this.gridColumn43.FieldName = "reportOper";
            this.gridColumn43.Name = "gridColumn43";
            this.gridColumn43.OptionsColumn.AllowEdit = false;
            this.gridColumn43.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn43.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn43.OptionsFilter.AllowFilter = false;
            this.gridColumn43.Visible = true;
            this.gridColumn43.VisibleIndex = 1;
            this.gridColumn43.Width = 117;
            // 
            // gridColumn44
            // 
            this.gridColumn44.Caption = "上报科室";
            this.gridColumn44.FieldName = "reportDeptName";
            this.gridColumn44.Name = "gridColumn44";
            this.gridColumn44.OptionsColumn.AllowEdit = false;
            this.gridColumn44.Visible = true;
            this.gridColumn44.VisibleIndex = 2;
            this.gridColumn44.Width = 115;
            // 
            // gridColumn17
            // 
            this.gridColumn17.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn17.AppearanceCell.ForeColor = System.Drawing.Color.Crimson;
            this.gridColumn17.AppearanceCell.Options.UseFont = true;
            this.gridColumn17.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn17.Caption = "事件编码";
            this.gridColumn17.FieldName = "eventCode";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.OptionsColumn.AllowEdit = false;
            this.gridColumn17.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn17.OptionsFilter.AllowFilter = false;
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 4;
            this.gridColumn17.Width = 121;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn1.Caption = "事件等级";
            this.gridColumn1.FieldName = "eventLevel";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 5;
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
            this.gridColumn21.VisibleIndex = 6;
            this.gridColumn21.Width = 197;
            // 
            // gridColumn24
            // 
            this.gridColumn24.Caption = "病历/门诊号";
            this.gridColumn24.FieldName = "patNo";
            this.gridColumn24.Name = "gridColumn24";
            this.gridColumn24.OptionsColumn.AllowEdit = false;
            this.gridColumn24.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn24.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn24.OptionsFilter.AllowFilter = false;
            this.gridColumn24.Visible = true;
            this.gridColumn24.VisibleIndex = 7;
            this.gridColumn24.Width = 93;
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
            this.gridColumn42.VisibleIndex = 8;
            this.gridColumn42.Width = 84;
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
            this.gridColumn29.VisibleIndex = 9;
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
            this.gridColumn25.VisibleIndex = 10;
            this.gridColumn25.Width = 54;
            // 
            // gridColumn26
            // 
            this.gridColumn26.Caption = "联系电话";
            this.gridColumn26.FieldName = "contactTel";
            this.gridColumn26.Name = "gridColumn26";
            this.gridColumn26.OptionsColumn.AllowEdit = false;
            this.gridColumn26.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn26.OptionsFilter.AllowFilter = false;
            this.gridColumn26.Visible = true;
            this.gridColumn26.VisibleIndex = 12;
            this.gridColumn26.Width = 137;
            // 
            // gridColumn41
            // 
            this.gridColumn41.Caption = "患者所在科室";
            this.gridColumn41.FieldName = "deptName";
            this.gridColumn41.Name = "gridColumn41";
            this.gridColumn41.OptionsColumn.AllowEdit = false;
            this.gridColumn41.Visible = true;
            this.gridColumn41.VisibleIndex = 11;
            this.gridColumn41.Width = 117;
            // 
            // ucDept
            // 
            this.ucDept.DeptName = "";
            this.ucDept.DeptVo = null;
            this.ucDept.IsShowOwnDept = false;
            this.ucDept.Location = new System.Drawing.Point(296, 6);
            this.ucDept.Name = "ucDept";
            this.ucDept.Size = new System.Drawing.Size(159, 23);
            this.ucDept.TabIndex = 130;
            // 
            // lueReporter
            // 
            this.lueReporter.CellValueChanged = false;
            this.lueReporter.IsButtonFind = false;
            this.lueReporter.Location = new System.Drawing.Point(856, 8);
            this.lueReporter.Name = "lueReporter";
            this.lueReporter.ParentBandedGridView = null;
            this.lueReporter.ParentBindingSource = null;
            this.lueReporter.ParentGridView = null;
            this.lueReporter.Properties.AppearanceDisabled.Options.UseBackColor = true;
            this.lueReporter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueReporter.Properties.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.lueReporter.Properties.DataSource = null;
            this.lueReporter.Properties.DBRow = null;
            this.lueReporter.Properties.DBValue = null;
            this.lueReporter.Properties.DescCode = null;
            this.lueReporter.Properties.DisplayColumn = null;
            this.lueReporter.Properties.DisplayValue = null;
            this.lueReporter.Properties.Essential = false;
            this.lueReporter.Properties.FieldName = null;
            this.lueReporter.Properties.FilterColumn = null;
            this.lueReporter.Properties.ForbidPoput = false;
            this.lueReporter.Properties.HideColumn = null;
            this.lueReporter.Properties.IsAutoPopup = false;
            this.lueReporter.Properties.IsCheckValid = true;
            this.lueReporter.Properties.IsDescField = false;
            this.lueReporter.Properties.IsFreeInput = false;
            this.lueReporter.Properties.IsHideValueColumn = false;
            this.lueReporter.Properties.IsSelectedMoveNextControl = false;
            this.lueReporter.Properties.IsShowColumnHeaders = false;
            this.lueReporter.Properties.IsShowDescInfo = false;
            this.lueReporter.Properties.IsShowRowNo = false;
            this.lueReporter.Properties.IsTab = true;
            this.lueReporter.Properties.IsUseShowColumn = false;
            this.lueReporter.Properties.ParentBandedGridView = null;
            this.lueReporter.Properties.ParentBindingSource = null;
            this.lueReporter.Properties.ParentGridView = null;
            this.lueReporter.Properties.PopupFormMinSize = new System.Drawing.Size(10, 10);
            this.lueReporter.Properties.PopupFormSize = new System.Drawing.Size(10, 10);
            this.lueReporter.Properties.PopupHeight = 0;
            this.lueReporter.Properties.PopupSizeable = false;
            this.lueReporter.Properties.PopupWidth = 0;
            this.lueReporter.Properties.PresentationMode = 0;
            this.lueReporter.Properties.ShowColumn = null;
            this.lueReporter.Properties.ShowPopupCloseButton = false;
            this.lueReporter.Properties.ShowPopupShadow = false;
            this.lueReporter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lueReporter.Properties.ValueColumn = null;
            this.lueReporter.Size = new System.Drawing.Size(100, 20);
            this.lueReporter.TabIndex = 132;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(808, 11);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(42, 12);
            this.labelControl1.TabIndex = 131;
            this.labelControl1.Text = "上报人:";
            // 
            // lblTip
            // 
            this.lblTip.AutoSize = true;
            this.lblTip.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTip.ForeColor = System.Drawing.Color.Red;
            this.lblTip.Location = new System.Drawing.Point(976, 10);
            this.lblTip.Name = "lblTip";
            this.lblTip.Size = new System.Drawing.Size(72, 16);
            this.lblTip.TabIndex = 133;
            this.lblTip.Text = "事件数：";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "事件发生时间";
            this.gridColumn2.FieldName = "eventTime";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsFilter.AllowAutoFilter = false;
            this.gridColumn2.OptionsFilter.AllowFilter = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 3;
            this.gridColumn2.Width = 120;
            // 
            // frmAdverseEventStat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 578);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmAdverseEventStat";
            this.Text = "不良事件统计查询";
            this.Load += new System.EventHandler(this.frmadverseeventstat_Load);
            this.Controls.SetChildIndex(this.marqueeProgressBarControl, 0);
            this.Controls.SetChildIndex(this.pcBackGround, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).EndInit();
            this.pcBackGround.ResumeLayout(false);
            this.pcBackGround.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboLevel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEventType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.colCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueReporter.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevExpress.XtraEditors.LabelControl labelControl3;
        internal DevExpress.XtraEditors.ComboBoxEdit cboLevel;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        internal DevExpress.XtraEditors.ComboBoxEdit cboEventType;
        internal DevExpress.XtraEditors.DateEdit dteStart;
        internal DevExpress.XtraEditors.LabelControl labelControl7;
        internal DevExpress.XtraEditors.DateEdit dteEnd;
        internal DevExpress.XtraEditors.PanelControl panelControl1;
        internal DevExpress.XtraGrid.GridControl gcReport;
        internal DevExpress.XtraGrid.Views.Grid.GridView gvReport;
        internal DevExpress.XtraGrid.Columns.GridColumn colCheck2;
        internal DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit colCheck;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn28;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn43;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn44;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn21;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn24;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn42;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn29;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn25;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn26;
        internal DevExpress.XtraGrid.Columns.GridColumn gridColumn41;
        internal Common.Controls.ucDept ucDept;
        internal DevExpress.XtraEditors.LabelControl labelControl1;
        internal Common.Controls.LookUpEdit lueReporter;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        internal System.Windows.Forms.Label lblTip;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    }
}