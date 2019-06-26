namespace Report.Ui
{
    partial class frmStatEventDept
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboEndMonth = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.cboStartMonth = new DevExpress.XtraEditors.ComboBoxEdit();
            this.dteEndDate = new DevExpress.XtraEditors.DateEdit();
            this.dteStartDate = new DevExpress.XtraEditors.DateEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ucPrintControl = new Common.Controls.ucPrintControl();
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).BeginInit();
            this.pcBackGround.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEndMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStartMonth.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteEndDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteEndDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteStartDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteStartDate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pcBackGround
            // 
            this.pcBackGround.Controls.Add(this.label1);
            this.pcBackGround.Controls.Add(this.cboEndMonth);
            this.pcBackGround.Controls.Add(this.lblStartDate);
            this.pcBackGround.Controls.Add(this.cboStartMonth);
            this.pcBackGround.Controls.Add(this.dteEndDate);
            this.pcBackGround.Controls.Add(this.dteStartDate);
            this.pcBackGround.Controls.Add(this.label5);
            this.pcBackGround.Controls.Add(this.label6);
            this.pcBackGround.Dock = System.Windows.Forms.DockStyle.Top;
            this.pcBackGround.Location = new System.Drawing.Point(0, 0);
            this.pcBackGround.Size = new System.Drawing.Size(1097, 35);
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
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(339, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 22);
            this.label1.TabIndex = 140;
            this.label1.Text = "月";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboEndMonth
            // 
            this.cboEndMonth.EditValue = "";
            this.cboEndMonth.EnterMoveNextControl = true;
            this.cboEndMonth.Location = new System.Drawing.Point(294, 8);
            this.cboEndMonth.MinimumSize = new System.Drawing.Size(0, 22);
            this.cboEndMonth.Name = "cboEndMonth";
            this.cboEndMonth.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cboEndMonth.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.cboEndMonth.Properties.Appearance.Options.UseFont = true;
            this.cboEndMonth.Properties.Appearance.Options.UseForeColor = true;
            this.cboEndMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.cboEndMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.cboEndMonth.Properties.AppearanceDisabled.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cboEndMonth.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Crimson;
            this.cboEndMonth.Properties.AppearanceDisabled.Options.UseFont = true;
            this.cboEndMonth.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.cboEndMonth.Properties.AppearanceDropDown.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cboEndMonth.Properties.AppearanceDropDown.ForeColor = System.Drawing.Color.Crimson;
            this.cboEndMonth.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cboEndMonth.Properties.AppearanceDropDown.Options.UseForeColor = true;
            this.cboEndMonth.Properties.AppearanceFocused.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cboEndMonth.Properties.AppearanceFocused.ForeColor = System.Drawing.Color.Crimson;
            this.cboEndMonth.Properties.AppearanceFocused.Options.UseFont = true;
            this.cboEndMonth.Properties.AppearanceFocused.Options.UseForeColor = true;
            this.cboEndMonth.Properties.AutoHeight = false;
            this.cboEndMonth.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboEndMonth.Properties.DropDownItemHeight = 22;
            this.cboEndMonth.Properties.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cboEndMonth.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboEndMonth.Size = new System.Drawing.Size(43, 22);
            this.cboEndMonth.TabIndex = 139;
            // 
            // lblStartDate
            // 
            this.lblStartDate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStartDate.Location = new System.Drawing.Point(202, 8);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(16, 22);
            this.lblStartDate.TabIndex = 138;
            this.lblStartDate.Text = "月";
            this.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboStartMonth
            // 
            this.cboStartMonth.EditValue = "";
            this.cboStartMonth.EnterMoveNextControl = true;
            this.cboStartMonth.Location = new System.Drawing.Point(157, 8);
            this.cboStartMonth.MinimumSize = new System.Drawing.Size(0, 22);
            this.cboStartMonth.Name = "cboStartMonth";
            this.cboStartMonth.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cboStartMonth.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.cboStartMonth.Properties.Appearance.Options.UseFont = true;
            this.cboStartMonth.Properties.Appearance.Options.UseForeColor = true;
            this.cboStartMonth.Properties.Appearance.Options.UseTextOptions = true;
            this.cboStartMonth.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.cboStartMonth.Properties.AppearanceDisabled.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cboStartMonth.Properties.AppearanceDisabled.ForeColor = System.Drawing.Color.Crimson;
            this.cboStartMonth.Properties.AppearanceDisabled.Options.UseFont = true;
            this.cboStartMonth.Properties.AppearanceDisabled.Options.UseForeColor = true;
            this.cboStartMonth.Properties.AppearanceDropDown.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cboStartMonth.Properties.AppearanceDropDown.ForeColor = System.Drawing.Color.Crimson;
            this.cboStartMonth.Properties.AppearanceDropDown.Options.UseFont = true;
            this.cboStartMonth.Properties.AppearanceDropDown.Options.UseForeColor = true;
            this.cboStartMonth.Properties.AppearanceFocused.Font = new System.Drawing.Font("宋体", 9.5F);
            this.cboStartMonth.Properties.AppearanceFocused.ForeColor = System.Drawing.Color.Crimson;
            this.cboStartMonth.Properties.AppearanceFocused.Options.UseFont = true;
            this.cboStartMonth.Properties.AppearanceFocused.Options.UseForeColor = true;
            this.cboStartMonth.Properties.AutoHeight = false;
            this.cboStartMonth.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboStartMonth.Properties.DropDownItemHeight = 22;
            this.cboStartMonth.Properties.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cboStartMonth.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cboStartMonth.Size = new System.Drawing.Size(43, 22);
            this.cboStartMonth.TabIndex = 137;
            // 
            // dteEndDate
            // 
            this.dteEndDate.EditValue = null;
            this.dteEndDate.Location = new System.Drawing.Point(239, 8);
            this.dteEndDate.Name = "dteEndDate";
            this.dteEndDate.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dteEndDate.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.dteEndDate.Properties.Appearance.Options.UseFont = true;
            this.dteEndDate.Properties.Appearance.Options.UseForeColor = true;
            this.dteEndDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteEndDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteEndDate.Properties.Mask.EditMask = "yyyy";
            this.dteEndDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dteEndDate.Size = new System.Drawing.Size(56, 22);
            this.dteEndDate.TabIndex = 136;
            // 
            // dteStartDate
            // 
            this.dteStartDate.EditValue = null;
            this.dteStartDate.Location = new System.Drawing.Point(102, 8);
            this.dteStartDate.Name = "dteStartDate";
            this.dteStartDate.Properties.Appearance.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.dteStartDate.Properties.Appearance.ForeColor = System.Drawing.Color.Crimson;
            this.dteStartDate.Properties.Appearance.Options.UseFont = true;
            this.dteStartDate.Properties.Appearance.Options.UseForeColor = true;
            this.dteStartDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteStartDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteStartDate.Properties.Mask.EditMask = "yyyy";
            this.dteStartDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dteStartDate.Size = new System.Drawing.Size(56, 22);
            this.dteStartDate.TabIndex = 133;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(220, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(16, 22);
            this.label5.TabIndex = 135;
            this.label5.Text = "至";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(42, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 22);
            this.label6.TabIndex = 134;
            this.label6.Text = "统计年份:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ucPrintControl
            // 
            this.ucPrintControl.Caption = null;
            this.ucPrintControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPrintControl.IsDockFill = true;
            this.ucPrintControl.IsReloadDictionary = false;
            this.ucPrintControl.IsSave = false;
            this.ucPrintControl.Location = new System.Drawing.Point(0, 35);
            this.ucPrintControl.Name = "ucPrintControl";
            this.ucPrintControl.PrintingSystem = null;
            this.ucPrintControl.ShowStatusBar = false;
            this.ucPrintControl.ShowToolBar = false;
            this.ucPrintControl.Size = new System.Drawing.Size(1097, 447);
            this.ucPrintControl.TabIndex = 13;
            this.ucPrintControl.ValueChanged = false;
            // 
            // frmAnaStat1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 482);
            this.Controls.Add(this.ucPrintControl);
            this.Name = "frmStatEventDept";
            this.Text = "医疗安全（不良）事件统计表（按科室）";
            this.Load += new System.EventHandler(this.frmStatEventDept_Load);
            this.Controls.SetChildIndex(this.marqueeProgressBarControl, 0);
            this.Controls.SetChildIndex(this.pcBackGround, 0);
            this.Controls.SetChildIndex(this.ucPrintControl, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).EndInit();
            this.pcBackGround.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboEndMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboStartMonth.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteEndDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteEndDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteStartDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteStartDate.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label label1;
        internal DevExpress.XtraEditors.ComboBoxEdit cboEndMonth;
        internal System.Windows.Forms.Label lblStartDate;
        internal DevExpress.XtraEditors.ComboBoxEdit cboStartMonth;
        internal DevExpress.XtraEditors.DateEdit dteEndDate;
        internal DevExpress.XtraEditors.DateEdit dteStartDate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        internal Common.Controls.ucPrintControl ucPrintControl;
    }
}