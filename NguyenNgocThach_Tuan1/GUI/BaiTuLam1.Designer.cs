namespace NguyenNgocThach_Tuan1.GUI
{
    partial class BaiTuLam1
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
            this.txtCapMaTran = new System.Windows.Forms.TextBox();
            this.btnO = new System.Windows.Forms.Button();
            this.panel_BanCo = new System.Windows.Forms.Panel();
            this.btnX = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cấp ma trận:";
            // 
            // txtCapMaTran
            // 
            this.txtCapMaTran.Location = new System.Drawing.Point(12, 29);
            this.txtCapMaTran.Name = "txtCapMaTran";
            this.txtCapMaTran.Size = new System.Drawing.Size(68, 20);
            this.txtCapMaTran.TabIndex = 1;
            // 
            // btnO
            // 
            this.btnO.Location = new System.Drawing.Point(12, 72);
            this.btnO.Name = "btnO";
            this.btnO.Size = new System.Drawing.Size(34, 26);
            this.btnO.TabIndex = 2;
            this.btnO.Text = "O";
            this.btnO.UseVisualStyleBackColor = true;
            this.btnO.Click += new System.EventHandler(this.btnO_Click);
            // 
            // panel_BanCo
            // 
            this.panel_BanCo.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel_BanCo.Location = new System.Drawing.Point(86, 12);
            this.panel_BanCo.Name = "panel_BanCo";
            this.panel_BanCo.Size = new System.Drawing.Size(10, 10);
            this.panel_BanCo.TabIndex = 3;
            // 
            // btnX
            // 
            this.btnX.Location = new System.Drawing.Point(46, 72);
            this.btnX.Name = "btnX";
            this.btnX.Size = new System.Drawing.Size(34, 26);
            this.btnX.TabIndex = 4;
            this.btnX.Text = "X";
            this.btnX.UseVisualStyleBackColor = true;
            this.btnX.Click += new System.EventHandler(this.btnX_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Nước đi trước";
            // 
            // BaiTuLam1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 101);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnX);
            this.Controls.Add(this.panel_BanCo);
            this.Controls.Add(this.btnO);
            this.Controls.Add(this.txtCapMaTran);
            this.Controls.Add(this.label1);
            this.Name = "BaiTuLam1";
            this.Text = "BaiTuLam1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCapMaTran;
        private System.Windows.Forms.Button btnO;
        private System.Windows.Forms.Panel panel_BanCo;
        private System.Windows.Forms.Button btnX;
        private System.Windows.Forms.Label label2;
    }
}