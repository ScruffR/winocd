namespace GDBfrontend.controls
{
    partial class OpenOCDconfigControl
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.openocdcfgFlash_tb = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.openocdCfgfile_btn = new System.Windows.Forms.Button();
            this.OpenOCDcfgfile_tb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.selectexe_btn = new System.Windows.Forms.Button();
            this.openOCDexe_tb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.openocdcfgFlash_tb);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.openocdCfgfile_btn);
            this.groupBox1.Controls.Add(this.OpenOCDcfgfile_tb);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.selectexe_btn);
            this.groupBox1.Controls.Add(this.openOCDexe_tb);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 166);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OpenOCD Paths";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(209, 131);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(24, 20);
            this.button2.TabIndex = 8;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // openocdcfgFlash_tb
            // 
            this.openocdcfgFlash_tb.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.openocdcfgFlash_tb.Location = new System.Drawing.Point(9, 131);
            this.openocdcfgFlash_tb.Name = "openocdcfgFlash_tb";
            this.openocdcfgFlash_tb.ReadOnly = true;
            this.openocdcfgFlash_tb.Size = new System.Drawing.Size(194, 20);
            this.openocdcfgFlash_tb.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "OpenOCD ConfigFile (Flash)";
            // 
            // openocdCfgfile_btn
            // 
            this.openocdCfgfile_btn.Location = new System.Drawing.Point(209, 81);
            this.openocdCfgfile_btn.Name = "openocdCfgfile_btn";
            this.openocdCfgfile_btn.Size = new System.Drawing.Size(24, 20);
            this.openocdCfgfile_btn.TabIndex = 5;
            this.openocdCfgfile_btn.Text = "...";
            this.openocdCfgfile_btn.UseVisualStyleBackColor = true;
            this.openocdCfgfile_btn.Click += new System.EventHandler(this.openocdCfgfile_btn_Click);
            // 
            // OpenOCDcfgfile_tb
            // 
            this.OpenOCDcfgfile_tb.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.OpenOCDcfgfile_tb.Location = new System.Drawing.Point(9, 81);
            this.OpenOCDcfgfile_tb.Name = "OpenOCDcfgfile_tb";
            this.OpenOCDcfgfile_tb.ReadOnly = true;
            this.OpenOCDcfgfile_tb.Size = new System.Drawing.Size(194, 20);
            this.OpenOCDcfgfile_tb.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(146, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "OpenOCD ConfigFile (Debug)";
            // 
            // selectexe_btn
            // 
            this.selectexe_btn.Location = new System.Drawing.Point(209, 32);
            this.selectexe_btn.Name = "selectexe_btn";
            this.selectexe_btn.Size = new System.Drawing.Size(24, 20);
            this.selectexe_btn.TabIndex = 2;
            this.selectexe_btn.Text = "...";
            this.selectexe_btn.UseVisualStyleBackColor = true;
            this.selectexe_btn.Click += new System.EventHandler(this.selectexe_btn_Click);
            // 
            // openOCDexe_tb
            // 
            this.openOCDexe_tb.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.openOCDexe_tb.Location = new System.Drawing.Point(9, 32);
            this.openOCDexe_tb.Name = "openOCDexe_tb";
            this.openOCDexe_tb.ReadOnly = true;
            this.openOCDexe_tb.Size = new System.Drawing.Size(194, 20);
            this.openOCDexe_tb.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "OpenOCD executeable :";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // OpenOCDconfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "OpenOCDconfigControl";
            this.Size = new System.Drawing.Size(256, 177);
            this.Load += new System.EventHandler(this.OpenOCDconfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button selectexe_btn;
        private System.Windows.Forms.TextBox openOCDexe_tb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox openocdcfgFlash_tb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button openocdCfgfile_btn;
        private System.Windows.Forms.TextBox OpenOCDcfgfile_tb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

    }
}
