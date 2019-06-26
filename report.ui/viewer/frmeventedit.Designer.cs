namespace Report.Ui
{
    partial class frmEventEdit
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEventEdit));
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.blbiClear = new DevExpress.XtraBars.BarLargeButtonItem();
            this.blbiSave = new DevExpress.XtraBars.BarLargeButtonItem();
            this.blbiPrint = new DevExpress.XtraBars.BarLargeButtonItem();
            this.blbiExport = new DevExpress.XtraBars.BarLargeButtonItem();
            this.blbiClose = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelControl = new DevExpress.XtraEditors.PanelControl();
            this.txtCardNo = new DevExpress.XtraEditors.TextEdit();
            this.lblFlagName = new System.Windows.Forms.Label();
            this.rdoFlag = new DevExpress.XtraEditors.RadioGroup();
            this.txtContactAddr = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtBirthday = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtSex = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtPatName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.lblDesc = new DevExpress.XtraEditors.LabelControl();
            this.xtraScrollableControl = new DevExpress.XtraEditors.XtraScrollableControl();
            this.showPanelForm = new Common.Controls.ShowPanelForm();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).BeginInit();
            this.pcBackGround.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).BeginInit();
            this.panelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCardNo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContactAddr.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBirthday.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSex.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatName.Properties)).BeginInit();
            this.xtraScrollableControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pcBackGround
            // 
            this.pcBackGround.Controls.Add(this.xtraScrollableControl);
            this.pcBackGround.Controls.Add(this.panelControl);
            this.pcBackGround.Location = new System.Drawing.Point(0, 60);
            this.pcBackGround.Size = new System.Drawing.Size(1008, 632);
            // 
            // defaultLookAndFeel
            // 
            this.defaultLookAndFeel.LookAndFeel.SkinName = "Office 2010 Blue";
            // 
            // marqueeProgressBarControl
            // 
            this.marqueeProgressBarControl.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.blbiClear,
            this.blbiSave,
            this.blbiPrint,
            this.blbiExport,
            this.blbiClose});
            this.barManager.MaxItemId = 5;
            // 
            // bar1
            // 
            this.bar1.BarAppearance.Disabled.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.BarAppearance.Disabled.Options.UseFont = true;
            this.bar1.BarAppearance.Hovered.Font = new System.Drawing.Font("宋体", 9F);
            this.bar1.BarAppearance.Hovered.Options.UseFont = true;
            this.bar1.BarAppearance.Normal.Font = new System.Drawing.Font("宋体", 9F);
            this.bar1.BarAppearance.Normal.Options.UseFont = true;
            this.bar1.BarAppearance.Pressed.Font = new System.Drawing.Font("宋体", 9F);
            this.bar1.BarAppearance.Pressed.Options.UseFont = true;
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.blbiClear, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.blbiSave, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.blbiPrint, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.blbiExport, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.blbiClose, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.RotateWhenVertical = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // blbiClear
            // 
            this.blbiClear.AllowDrawArrow = false;
            this.blbiClear.AllowDrawArrowInMenu = true;
            this.blbiClear.Caption = "清空";
            this.blbiClear.Glyph = ((System.Drawing.Image)(resources.GetObject("blbiClear.Glyph")));
            this.blbiClear.Id = 0;
            this.blbiClear.Name = "blbiClear";
            this.blbiClear.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.blbiClear_ItemClick);
            // 
            // blbiSave
            // 
            this.blbiSave.AllowDrawArrow = false;
            this.blbiSave.AllowDrawArrowInMenu = true;
            this.blbiSave.Caption = "保存";
            this.blbiSave.Glyph = ((System.Drawing.Image)(resources.GetObject("blbiSave.Glyph")));
            this.blbiSave.Id = 1;
            this.blbiSave.Name = "blbiSave";
            this.blbiSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.blbiSave_ItemClick);
            // 
            // blbiPrint
            // 
            this.blbiPrint.AllowDrawArrow = false;
            this.blbiPrint.AllowDrawArrowInMenu = true;
            this.blbiPrint.Caption = "打印";
            this.blbiPrint.Glyph = ((System.Drawing.Image)(resources.GetObject("blbiPrint.Glyph")));
            this.blbiPrint.Id = 2;
            this.blbiPrint.Name = "blbiPrint";
            this.blbiPrint.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.blbiPrint_ItemClick);
            // 
            // blbiExport
            // 
            this.blbiExport.AllowDrawArrow = false;
            this.blbiExport.AllowDrawArrowInMenu = true;
            this.blbiExport.Caption = "导出";
            this.blbiExport.Glyph = ((System.Drawing.Image)(resources.GetObject("blbiExport.Glyph")));
            this.blbiExport.Id = 3;
            this.blbiExport.Name = "blbiExport";
            this.blbiExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.blbiExport_ItemClick);
            // 
            // blbiClose
            // 
            this.blbiClose.AllowDrawArrow = false;
            this.blbiClose.AllowDrawArrowInMenu = true;
            this.blbiClose.Caption = "关闭";
            this.blbiClose.Glyph = ((System.Drawing.Image)(resources.GetObject("blbiClose.Glyph")));
            this.blbiClose.Id = 4;
            this.blbiClose.Name = "blbiClose";
            this.blbiClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.blbiClose_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1008, 60);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 692);
            this.barDockControlBottom.Size = new System.Drawing.Size(1008, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 60);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 632);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1008, 60);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 632);
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.txtCardNo);
            this.panelControl.Controls.Add(this.lblFlagName);
            this.panelControl.Controls.Add(this.rdoFlag);
            this.panelControl.Controls.Add(this.txtContactAddr);
            this.panelControl.Controls.Add(this.labelControl4);
            this.panelControl.Controls.Add(this.txtBirthday);
            this.panelControl.Controls.Add(this.labelControl3);
            this.panelControl.Controls.Add(this.txtSex);
            this.panelControl.Controls.Add(this.labelControl1);
            this.panelControl.Controls.Add(this.txtPatName);
            this.panelControl.Controls.Add(this.labelControl2);
            this.panelControl.Controls.Add(this.lblDesc);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl.Location = new System.Drawing.Point(2, 2);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(1004, 38);
            this.panelControl.TabIndex = 0;
            // 
            // txtCardNo
            // 
            this.txtCardNo.EditValue = "";
            this.txtCardNo.Location = new System.Drawing.Point(240, 9);
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F, System.Drawing.FontStyle.Bold);
            this.txtCardNo.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.txtCardNo.Properties.Appearance.Options.UseFont = true;
            this.txtCardNo.Properties.Appearance.Options.UseForeColor = true;
            this.txtCardNo.Properties.MaxLength = 15;
            this.txtCardNo.Size = new System.Drawing.Size(114, 20);
            this.txtCardNo.TabIndex = 137;
            this.txtCardNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCardNo_KeyDown);
            // 
            // lblFlagName
            // 
            this.lblFlagName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblFlagName.Location = new System.Drawing.Point(179, 13);
            this.lblFlagName.Name = "lblFlagName";
            this.lblFlagName.Size = new System.Drawing.Size(62, 12);
            this.lblFlagName.TabIndex = 136;
            this.lblFlagName.Text = "诊疗卡号:";
            this.lblFlagName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rdoFlag
            // 
            this.rdoFlag.Location = new System.Drawing.Point(8, 4);
            this.rdoFlag.MenuManager = this.barManager;
            this.rdoFlag.Name = "rdoFlag";
            this.rdoFlag.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoFlag.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoFlag.Properties.Appearance.Options.UseBackColor = true;
            this.rdoFlag.Properties.Appearance.Options.UseFont = true;
            this.rdoFlag.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdoFlag.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "门诊"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "住院"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "职工"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "无")});
            this.rdoFlag.Size = new System.Drawing.Size(176, 28);
            this.rdoFlag.TabIndex = 146;
            this.rdoFlag.SelectedIndexChanged += new System.EventHandler(this.rdoFlag_SelectedIndexChanged);
            // 
            // txtContactAddr
            // 
            this.txtContactAddr.Location = new System.Drawing.Point(784, 9);
            this.txtContactAddr.Name = "txtContactAddr";
            this.txtContactAddr.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10F);
            this.txtContactAddr.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.txtContactAddr.Properties.Appearance.Options.UseFont = true;
            this.txtContactAddr.Properties.Appearance.Options.UseForeColor = true;
            this.txtContactAddr.Properties.ReadOnly = true;
            this.txtContactAddr.Size = new System.Drawing.Size(145, 20);
            this.txtContactAddr.TabIndex = 144;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Location = new System.Drawing.Point(727, 13);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(54, 12);
            this.labelControl4.TabIndex = 145;
            this.labelControl4.Text = "联系地址:";
            // 
            // txtBirthday
            // 
            this.txtBirthday.Location = new System.Drawing.Point(635, 9);
            this.txtBirthday.Name = "txtBirthday";
            this.txtBirthday.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10F);
            this.txtBirthday.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.txtBirthday.Properties.Appearance.Options.UseFont = true;
            this.txtBirthday.Properties.Appearance.Options.UseForeColor = true;
            this.txtBirthday.Properties.ReadOnly = true;
            this.txtBirthday.Size = new System.Drawing.Size(87, 20);
            this.txtBirthday.TabIndex = 142;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Location = new System.Drawing.Point(579, 13);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(54, 12);
            this.labelControl3.TabIndex = 143;
            this.labelControl3.Text = "出生日期:";
            // 
            // txtSex
            // 
            this.txtSex.Location = new System.Drawing.Point(531, 9);
            this.txtSex.Name = "txtSex";
            this.txtSex.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10F);
            this.txtSex.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.txtSex.Properties.Appearance.Options.UseFont = true;
            this.txtSex.Properties.Appearance.Options.UseForeColor = true;
            this.txtSex.Properties.ReadOnly = true;
            this.txtSex.Size = new System.Drawing.Size(36, 20);
            this.txtSex.TabIndex = 140;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(497, 13);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 12);
            this.labelControl1.TabIndex = 141;
            this.labelControl1.Text = "性别:";
            // 
            // txtPatName
            // 
            this.txtPatName.Location = new System.Drawing.Point(399, 9);
            this.txtPatName.Name = "txtPatName";
            this.txtPatName.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10F);
            this.txtPatName.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.txtPatName.Properties.Appearance.Options.UseFont = true;
            this.txtPatName.Properties.Appearance.Options.UseForeColor = true;
            this.txtPatName.Properties.ReadOnly = true;
            this.txtPatName.Size = new System.Drawing.Size(87, 20);
            this.txtPatName.TabIndex = 138;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Location = new System.Drawing.Point(365, 13);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(30, 12);
            this.labelControl2.TabIndex = 139;
            this.labelControl2.Text = "姓名:";
            // 
            // lblDesc
            // 
            this.lblDesc.Appearance.BackColor = System.Drawing.Color.LightYellow;
            this.lblDesc.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblDesc.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblDesc.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblDesc.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblDesc.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblDesc.Location = new System.Drawing.Point(932, 2);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(70, 34);
            this.lblDesc.TabIndex = 70;
            this.lblDesc.Text = "患者信息";
            // 
            // xtraScrollableControl
            // 
            this.xtraScrollableControl.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(200)))), ((int)(((byte)(205)))));
            this.xtraScrollableControl.Appearance.Options.UseBackColor = true;
            this.xtraScrollableControl.Controls.Add(this.showPanelForm);
            this.xtraScrollableControl.Controls.Add(this.panel1);
            this.xtraScrollableControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraScrollableControl.Location = new System.Drawing.Point(2, 40);
            this.xtraScrollableControl.Name = "xtraScrollableControl";
            this.xtraScrollableControl.Size = new System.Drawing.Size(1004, 590);
            this.xtraScrollableControl.TabIndex = 2;
            // 
            // showPanelForm
            // 
            this.showPanelForm.BackColor = System.Drawing.Color.White;
            this.showPanelForm.caseCode = null;
            this.showPanelForm.Formid = 0;
            this.showPanelForm.FormLayout = null;
            this.showPanelForm.FormXmlData = null;
            this.showPanelForm.HintInfo = null;
            this.showPanelForm.IsAllowSave = false;
            this.showPanelForm.Location = new System.Drawing.Point(16, 8);
            this.showPanelForm.Name = "showPanelForm";
            this.showPanelForm.Size = new System.Drawing.Size(724, 474);
            this.showPanelForm.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 560);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1004, 30);
            this.panel1.TabIndex = 0;
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // frmEventEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 692);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "frmEventEdit";
            this.Text = "不良事件";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmEventEdit_FormClosing);
            this.Load += new System.EventHandler(this.frmEventEdit_Load);
            this.Controls.SetChildIndex(this.barDockControlTop, 0);
            this.Controls.SetChildIndex(this.barDockControlBottom, 0);
            this.Controls.SetChildIndex(this.barDockControlRight, 0);
            this.Controls.SetChildIndex(this.barDockControlLeft, 0);
            this.Controls.SetChildIndex(this.marqueeProgressBarControl, 0);
            this.Controls.SetChildIndex(this.pcBackGround, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).EndInit();
            this.pcBackGround.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl)).EndInit();
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCardNo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtContactAddr.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBirthday.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSex.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPatName.Properties)).EndInit();
            this.xtraScrollableControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        internal DevExpress.XtraBars.BarLargeButtonItem blbiClear;
        internal DevExpress.XtraBars.BarLargeButtonItem blbiSave;
        internal DevExpress.XtraBars.BarLargeButtonItem blbiPrint;
        internal DevExpress.XtraBars.BarLargeButtonItem blbiExport;
        internal DevExpress.XtraBars.BarLargeButtonItem blbiClose;
        private DevExpress.XtraEditors.PanelControl panelControl;
        internal DevExpress.XtraEditors.XtraScrollableControl xtraScrollableControl;
        internal Common.Controls.ShowPanelForm showPanelForm;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timer;
        internal DevExpress.XtraEditors.LabelControl lblDesc;
        internal DevExpress.XtraEditors.TextEdit txtCardNo;
        internal DevExpress.XtraEditors.TextEdit txtContactAddr;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        internal DevExpress.XtraEditors.TextEdit txtBirthday;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        internal DevExpress.XtraEditors.TextEdit txtSex;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        internal DevExpress.XtraEditors.TextEdit txtPatName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        internal DevExpress.XtraEditors.RadioGroup rdoFlag;
        internal System.Windows.Forms.Label lblFlagName;
    }
}