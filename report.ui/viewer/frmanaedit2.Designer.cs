namespace Report.Ui
{
    partial class frmAnaEdit2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAnaEdit2));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.blbiSave = new DevExpress.XtraBars.BarLargeButtonItem();
            this.blbiDel = new DevExpress.XtraBars.BarLargeButtonItem();
            this.blbiClose = new DevExpress.XtraBars.BarLargeButtonItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.chk01 = new DevExpress.XtraEditors.CheckEdit();
            this.chk02 = new DevExpress.XtraEditors.CheckEdit();
            this.chk04 = new DevExpress.XtraEditors.CheckEdit();
            this.chk03 = new DevExpress.XtraEditors.CheckEdit();
            this.chk08 = new DevExpress.XtraEditors.CheckEdit();
            this.chk07 = new DevExpress.XtraEditors.CheckEdit();
            this.chk06 = new DevExpress.XtraEditors.CheckEdit();
            this.chk05 = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).BeginInit();
            this.pcBackGround.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk01.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk02.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk04.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk03.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk08.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk07.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk06.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk05.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pcBackGround
            // 
            this.pcBackGround.Controls.Add(this.chk08);
            this.pcBackGround.Controls.Add(this.chk07);
            this.pcBackGround.Controls.Add(this.chk06);
            this.pcBackGround.Controls.Add(this.chk05);
            this.pcBackGround.Controls.Add(this.chk04);
            this.pcBackGround.Controls.Add(this.chk03);
            this.pcBackGround.Controls.Add(this.chk02);
            this.pcBackGround.Controls.Add(this.chk01);
            this.pcBackGround.Controls.Add(this.labelControl1);
            this.pcBackGround.Location = new System.Drawing.Point(0, 60);
            this.pcBackGround.Size = new System.Drawing.Size(235, 402);
            // 
            // defaultLookAndFeel
            // 
            this.defaultLookAndFeel.LookAndFeel.SkinName = "Office 2010 Blue";
            // 
            // marqueeProgressBarControl
            // 
            this.marqueeProgressBarControl.Properties.Appearance.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.blbiSave,
            this.blbiDel,
            this.blbiClose});
            this.barManager1.MaxItemId = 3;
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
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.blbiSave, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.blbiDel, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.blbiClose, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.UseWholeRow = true;
            this.bar1.Text = "Tools";
            // 
            // blbiSave
            // 
            this.blbiSave.AllowDrawArrow = false;
            this.blbiSave.AllowDrawArrowInMenu = true;
            this.blbiSave.Caption = "保存";
            this.blbiSave.Glyph = ((System.Drawing.Image)(resources.GetObject("blbiSave.Glyph")));
            this.blbiSave.Id = 0;
            this.blbiSave.Name = "blbiSave";
            this.blbiSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.blbiSave_ItemClick);
            // 
            // blbiDel
            // 
            this.blbiDel.AllowDrawArrow = false;
            this.blbiDel.AllowDrawArrowInMenu = true;
            this.blbiDel.Caption = "删除";
            this.blbiDel.Glyph = ((System.Drawing.Image)(resources.GetObject("blbiDel.Glyph")));
            this.blbiDel.Id = 1;
            this.blbiDel.Name = "blbiDel";
            this.blbiDel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.blbiDel_ItemClick);
            // 
            // blbiClose
            // 
            this.blbiClose.AllowDrawArrow = false;
            this.blbiClose.AllowDrawArrowInMenu = true;
            this.blbiClose.Caption = "关闭";
            this.blbiClose.Glyph = ((System.Drawing.Image)(resources.GetObject("blbiClose.Glyph")));
            this.blbiClose.Id = 2;
            this.blbiClose.Name = "blbiClose";
            this.blbiClose.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.blbiClose_ItemClick);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(235, 60);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 462);
            this.barDockControlBottom.Size = new System.Drawing.Size(235, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 60);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 402);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(235, 60);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 402);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Location = new System.Drawing.Point(28, 32);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 12);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "并发症：";
            // 
            // chk01
            // 
            this.chk01.Location = new System.Drawing.Point(84, 28);
            this.chk01.MenuManager = this.barManager1;
            this.chk01.Name = "chk01";
            this.chk01.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.chk01.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F);
            this.chk01.Properties.Appearance.Options.UseBackColor = true;
            this.chk01.Properties.Appearance.Options.UseFont = true;
            this.chk01.Properties.Caption = "恶心呕吐";
            this.chk01.Size = new System.Drawing.Size(75, 19);
            this.chk01.TabIndex = 18;
            // 
            // chk02
            // 
            this.chk02.Location = new System.Drawing.Point(84, 66);
            this.chk02.MenuManager = this.barManager1;
            this.chk02.Name = "chk02";
            this.chk02.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.chk02.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F);
            this.chk02.Properties.Appearance.Options.UseBackColor = true;
            this.chk02.Properties.Appearance.Options.UseFont = true;
            this.chk02.Properties.Caption = "排尿困难";
            this.chk02.Size = new System.Drawing.Size(75, 19);
            this.chk02.TabIndex = 19;
            // 
            // chk04
            // 
            this.chk04.Location = new System.Drawing.Point(84, 142);
            this.chk04.MenuManager = this.barManager1;
            this.chk04.Name = "chk04";
            this.chk04.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.chk04.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F);
            this.chk04.Properties.Appearance.Options.UseBackColor = true;
            this.chk04.Properties.Appearance.Options.UseFont = true;
            this.chk04.Properties.Caption = "镇痛不全";
            this.chk04.Size = new System.Drawing.Size(75, 19);
            this.chk04.TabIndex = 21;
            // 
            // chk03
            // 
            this.chk03.Location = new System.Drawing.Point(84, 104);
            this.chk03.MenuManager = this.barManager1;
            this.chk03.Name = "chk03";
            this.chk03.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.chk03.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F);
            this.chk03.Properties.Appearance.Options.UseBackColor = true;
            this.chk03.Properties.Appearance.Options.UseFont = true;
            this.chk03.Properties.Caption = "皮肤瘙痒";
            this.chk03.Size = new System.Drawing.Size(75, 19);
            this.chk03.TabIndex = 20;
            // 
            // chk08
            // 
            this.chk08.Location = new System.Drawing.Point(84, 294);
            this.chk08.MenuManager = this.barManager1;
            this.chk08.Name = "chk08";
            this.chk08.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.chk08.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F);
            this.chk08.Properties.Appearance.Options.UseBackColor = true;
            this.chk08.Properties.Appearance.Options.UseFont = true;
            this.chk08.Properties.Caption = "呼吸抑制";
            this.chk08.Size = new System.Drawing.Size(75, 19);
            this.chk08.TabIndex = 25;
            // 
            // chk07
            // 
            this.chk07.Location = new System.Drawing.Point(84, 256);
            this.chk07.MenuManager = this.barManager1;
            this.chk07.Name = "chk07";
            this.chk07.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.chk07.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F);
            this.chk07.Properties.Appearance.Options.UseBackColor = true;
            this.chk07.Properties.Appearance.Options.UseFont = true;
            this.chk07.Properties.Caption = "镇静过度";
            this.chk07.Size = new System.Drawing.Size(75, 19);
            this.chk07.TabIndex = 24;
            // 
            // chk06
            // 
            this.chk06.Location = new System.Drawing.Point(84, 218);
            this.chk06.MenuManager = this.barManager1;
            this.chk06.Name = "chk06";
            this.chk06.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.chk06.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F);
            this.chk06.Properties.Appearance.Options.UseBackColor = true;
            this.chk06.Properties.Appearance.Options.UseFont = true;
            this.chk06.Properties.Caption = "感觉及运动异常";
            this.chk06.Size = new System.Drawing.Size(116, 19);
            this.chk06.TabIndex = 23;
            // 
            // chk05
            // 
            this.chk05.Location = new System.Drawing.Point(84, 180);
            this.chk05.MenuManager = this.barManager1;
            this.chk05.Name = "chk05";
            this.chk05.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.chk05.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9.5F);
            this.chk05.Properties.Appearance.Options.UseBackColor = true;
            this.chk05.Properties.Appearance.Options.UseFont = true;
            this.chk05.Properties.Caption = "硬膜外导管脱出";
            this.chk05.Size = new System.Drawing.Size(116, 19);
            this.chk05.TabIndex = 22;
            // 
            // frmAnaEdit2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 462);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MaximizeBox = false;
            this.Name = "frmAnaEdit2";
            this.Text = "编辑";
            this.Load += new System.EventHandler(this.frmAnaEdit2_Load);
            this.Controls.SetChildIndex(this.barDockControlTop, 0);
            this.Controls.SetChildIndex(this.barDockControlBottom, 0);
            this.Controls.SetChildIndex(this.barDockControlRight, 0);
            this.Controls.SetChildIndex(this.barDockControlLeft, 0);
            this.Controls.SetChildIndex(this.marqueeProgressBarControl, 0);
            this.Controls.SetChildIndex(this.pcBackGround, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pcBackGround)).EndInit();
            this.pcBackGround.ResumeLayout(false);
            this.pcBackGround.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.marqueeProgressBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk01.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk02.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk04.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk03.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk08.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk07.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk06.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chk05.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarLargeButtonItem blbiSave;
        private DevExpress.XtraBars.BarLargeButtonItem blbiDel;
        private DevExpress.XtraBars.BarLargeButtonItem blbiClose;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit chk08;
        private DevExpress.XtraEditors.CheckEdit chk07;
        private DevExpress.XtraEditors.CheckEdit chk06;
        private DevExpress.XtraEditors.CheckEdit chk05;
        private DevExpress.XtraEditors.CheckEdit chk04;
        private DevExpress.XtraEditors.CheckEdit chk03;
        private DevExpress.XtraEditors.CheckEdit chk02;
        private DevExpress.XtraEditors.CheckEdit chk01;
    }
}