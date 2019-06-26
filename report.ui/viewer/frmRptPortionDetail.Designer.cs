namespace Report.Ui
{
    partial class frmRptPortionDetail
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
            this.rdoFlag = new DevExpress.XtraEditors.RadioGroup();
            this.dteDateStart = new DevExpress.XtraEditors.DateEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.dteDateEnd = new DevExpress.XtraEditors.DateEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.ucPrintControl = new Common.Controls.ucPrintControl();
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).BeginInit();
            this.pcBackGround.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoFlag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pcBackGround
            // 
            this.pcBackGround.Controls.Add(this.rdoFlag);
            this.pcBackGround.Controls.Add(this.dteDateStart);
            this.pcBackGround.Controls.Add(this.label3);
            this.pcBackGround.Controls.Add(this.dteDateEnd);
            this.pcBackGround.Controls.Add(this.label2);
            this.pcBackGround.Dock = System.Windows.Forms.DockStyle.Top;
            this.pcBackGround.Location = new System.Drawing.Point(0, 0);
            this.pcBackGround.Size = new System.Drawing.Size(1052, 35);
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
            // rdoFlag
            // 
            this.rdoFlag.Location = new System.Drawing.Point(328, 3);
            this.rdoFlag.MenuManager = this.barManager;
            this.rdoFlag.Name = "rdoFlag";
            this.rdoFlag.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.rdoFlag.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoFlag.Properties.Appearance.Options.UseBackColor = true;
            this.rdoFlag.Properties.Appearance.Options.UseFont = true;
            this.rdoFlag.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.rdoFlag.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "门诊"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "住院")});
            this.rdoFlag.Size = new System.Drawing.Size(176, 28);
            this.rdoFlag.TabIndex = 152;
            this.rdoFlag.SelectedIndexChanged += new System.EventHandler(this.rdoFlag_SelectedIndexChanged);
            // 
            // dteDateStart
            // 
            this.dteDateStart.EditValue = null;
            this.dteDateStart.Location = new System.Drawing.Point(76, 4);
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
            this.dteDateStart.TabIndex = 148;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.label3.Location = new System.Drawing.Point(181, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 25);
            this.label3.TabIndex = 151;
            this.label3.Text = "至";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dteDateEnd
            // 
            this.dteDateEnd.EditValue = null;
            this.dteDateEnd.Location = new System.Drawing.Point(201, 4);
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
            this.dteDateEnd.TabIndex = 150;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this.label2.Location = new System.Drawing.Point(16, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 25);
            this.label2.TabIndex = 149;
            this.label2.Text = "统计日期:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.ucPrintControl);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 35);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1052, 487);
            this.panelControl1.TabIndex = 12;
            // 
            // ucPrintControl
            // 
            this.ucPrintControl.Caption = null;
            this.ucPrintControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPrintControl.IsDockFill = true;
            this.ucPrintControl.IsReloadDictionary = false;
            this.ucPrintControl.IsSave = false;
            this.ucPrintControl.Location = new System.Drawing.Point(2, 2);
            this.ucPrintControl.Name = "ucPrintControl";
            this.ucPrintControl.PrintingSystem = null;
            this.ucPrintControl.ShowStatusBar = false;
            this.ucPrintControl.ShowToolBar = false;
            this.ucPrintControl.Size = new System.Drawing.Size(1048, 483);
            this.ucPrintControl.TabIndex = 14;
            this.ucPrintControl.ValueChanged = false;
            // 
            // frmRptPortionDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 522);
            this.Controls.Add(this.panelControl1);
            this.Name = "frmRptPortionDetail";
            this.Text = "科室医生药占比报表";
            this.Load += new System.EventHandler(this.frmRptPortionDetail_Load);
            this.Controls.SetChildIndex(this.marqueeProgressBarControl, 0);
            this.Controls.SetChildIndex(this.pcBackGround, 0);
            this.Controls.SetChildIndex(this.panelControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).EndInit();
            this.pcBackGround.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdoFlag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteDateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal DevExpress.XtraEditors.RadioGroup rdoFlag;
        internal DevExpress.XtraEditors.DateEdit dteDateStart;
        private System.Windows.Forms.Label label3;
        internal DevExpress.XtraEditors.DateEdit dteDateEnd;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        internal Common.Controls.ucPrintControl ucPrintControl;
    }
}