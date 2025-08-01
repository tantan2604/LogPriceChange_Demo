namespace LogPriceChange0._1
{
    partial class AdminForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tabCtr_forApproval = new System.Windows.Forms.TabControl();
            this.tabCtrforApproval = new System.Windows.Forms.TabPage();
            this.tabCtrApproved = new System.Windows.Forms.TabPage();
            this.btn_approve = new System.Windows.Forms.Button();
            this.btn_reject = new System.Windows.Forms.Button();
            this.dgv_forApproval = new System.Windows.Forms.DataGridView();
            this.panel5.SuspendLayout();
            this.tabCtr_forApproval.SuspendLayout();
            this.tabCtrforApproval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_forApproval)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1233, 58);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 567);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1233, 64);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 58);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(175, 509);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(175, 58);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1058, 131);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.tabCtr_forApproval);
            this.panel5.Controls.Add(this.btn_approve);
            this.panel5.Controls.Add(this.btn_reject);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(175, 189);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1058, 378);
            this.panel5.TabIndex = 4;
            // 
            // tabCtr_forApproval
            // 
            this.tabCtr_forApproval.Controls.Add(this.tabCtrforApproval);
            this.tabCtr_forApproval.Controls.Add(this.tabCtrApproved);
            this.tabCtr_forApproval.Location = new System.Drawing.Point(6, 6);
            this.tabCtr_forApproval.Name = "tabCtr_forApproval";
            this.tabCtr_forApproval.SelectedIndex = 0;
            this.tabCtr_forApproval.Size = new System.Drawing.Size(1052, 324);
            this.tabCtr_forApproval.TabIndex = 3;
            // 
            // tabCtrforApproval
            // 
            this.tabCtrforApproval.Controls.Add(this.dgv_forApproval);
            this.tabCtrforApproval.Location = new System.Drawing.Point(4, 22);
            this.tabCtrforApproval.Name = "tabCtrforApproval";
            this.tabCtrforApproval.Padding = new System.Windows.Forms.Padding(3);
            this.tabCtrforApproval.Size = new System.Drawing.Size(1044, 298);
            this.tabCtrforApproval.TabIndex = 0;
            this.tabCtrforApproval.Text = "For Approval";
            this.tabCtrforApproval.UseVisualStyleBackColor = true;
            // 
            // tabCtrApproved
            // 
            this.tabCtrApproved.Location = new System.Drawing.Point(4, 22);
            this.tabCtrApproved.Name = "tabCtrApproved";
            this.tabCtrApproved.Padding = new System.Windows.Forms.Padding(3);
            this.tabCtrApproved.Size = new System.Drawing.Size(1044, 298);
            this.tabCtrApproved.TabIndex = 1;
            this.tabCtrApproved.Text = "Approved";
            this.tabCtrApproved.UseVisualStyleBackColor = true;
            // 
            // btn_approve
            // 
            this.btn_approve.BackColor = System.Drawing.Color.Blue;
            this.btn_approve.ForeColor = System.Drawing.Color.White;
            this.btn_approve.Location = new System.Drawing.Point(838, 335);
            this.btn_approve.Name = "btn_approve";
            this.btn_approve.Size = new System.Drawing.Size(101, 36);
            this.btn_approve.TabIndex = 2;
            this.btn_approve.Text = "APPROVE";
            this.btn_approve.UseVisualStyleBackColor = false;
            // 
            // btn_reject
            // 
            this.btn_reject.BackColor = System.Drawing.Color.Red;
            this.btn_reject.ForeColor = System.Drawing.Color.White;
            this.btn_reject.Location = new System.Drawing.Point(945, 335);
            this.btn_reject.Name = "btn_reject";
            this.btn_reject.Size = new System.Drawing.Size(101, 36);
            this.btn_reject.TabIndex = 1;
            this.btn_reject.Text = "REJECT";
            this.btn_reject.UseVisualStyleBackColor = false;
            // 
            // dgv_forApproval
            // 
            this.dgv_forApproval.BackgroundColor = System.Drawing.Color.White;
            this.dgv_forApproval.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_forApproval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_forApproval.Location = new System.Drawing.Point(3, 3);
            this.dgv_forApproval.Name = "dgv_forApproval";
            this.dgv_forApproval.Size = new System.Drawing.Size(1038, 292);
            this.dgv_forApproval.TabIndex = 0;
            // 
            // AdminForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 631);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(1249, 670);
            this.Name = "AdminForm";
            this.Text = "AdminForm";
            this.panel5.ResumeLayout(false);
            this.tabCtr_forApproval.ResumeLayout(false);
            this.tabCtrforApproval.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_forApproval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btn_reject;
        private System.Windows.Forms.Button btn_approve;
        private System.Windows.Forms.TabControl tabCtr_forApproval;
        private System.Windows.Forms.TabPage tabCtrforApproval;
        private System.Windows.Forms.TabPage tabCtrApproved;
        private System.Windows.Forms.DataGridView dgv_forApproval;
    }
}