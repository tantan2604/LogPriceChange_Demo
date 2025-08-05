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
            this.btn_logout = new System.Windows.Forms.Button();
            this.lbl_adminLog = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tabCtr_forApproval = new System.Windows.Forms.TabControl();
            this.tabCtrforApproval = new System.Windows.Forms.TabPage();
            this.dgv_forApproval = new System.Windows.Forms.DataGridView();
            this.tabCtrApproved = new System.Windows.Forms.TabPage();
            this.dgv_Approved = new System.Windows.Forms.DataGridView();
            this.btn_approve = new System.Windows.Forms.Button();
            this.btn_reject = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblApprovedCount = new System.Windows.Forms.Label();
            this.lblForApproval = new System.Windows.Forms.Label();
            this.lblRejectedCount = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabCtr_forApproval.SuspendLayout();
            this.tabCtrforApproval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_forApproval)).BeginInit();
            this.tabCtrApproved.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Approved)).BeginInit();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(27)))), ((int)(((byte)(36)))));
            this.panel1.Controls.Add(this.panel6);
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
            this.panel3.Controls.Add(this.btn_logout);
            this.panel3.Controls.Add(this.lbl_adminLog);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 58);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(175, 509);
            this.panel3.TabIndex = 2;
            // 
            // btn_logout
            // 
            this.btn_logout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_logout.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_logout.Location = new System.Drawing.Point(0, 473);
            this.btn_logout.Name = "btn_logout";
            this.btn_logout.Size = new System.Drawing.Size(175, 36);
            this.btn_logout.TabIndex = 1;
            this.btn_logout.Text = "Log out";
            this.btn_logout.UseVisualStyleBackColor = true;
            this.btn_logout.Click += new System.EventHandler(this.btn_logout_Click);
            // 
            // lbl_adminLog
            // 
            this.lbl_adminLog.AutoSize = true;
            this.lbl_adminLog.Location = new System.Drawing.Point(59, 131);
            this.lbl_adminLog.Name = "lbl_adminLog";
            this.lbl_adminLog.Size = new System.Drawing.Size(35, 13);
            this.lbl_adminLog.TabIndex = 0;
            this.lbl_adminLog.Text = "label1";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel9);
            this.panel4.Controls.Add(this.panel8);
            this.panel4.Controls.Add(this.panel7);
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
            this.tabCtr_forApproval.SelectedIndexChanged += new System.EventHandler(this.tabCtr_forApproval_SelectedIndexChanged);
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
            // dgv_forApproval
            // 
            this.dgv_forApproval.BackgroundColor = System.Drawing.Color.White;
            this.dgv_forApproval.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_forApproval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_forApproval.Location = new System.Drawing.Point(3, 3);
            this.dgv_forApproval.Name = "dgv_forApproval";
            this.dgv_forApproval.Size = new System.Drawing.Size(1038, 292);
            this.dgv_forApproval.TabIndex = 0;
            this.dgv_forApproval.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_forApproval_CellClick);
            // 
            // tabCtrApproved
            // 
            this.tabCtrApproved.Controls.Add(this.dgv_Approved);
            this.tabCtrApproved.Location = new System.Drawing.Point(4, 22);
            this.tabCtrApproved.Name = "tabCtrApproved";
            this.tabCtrApproved.Padding = new System.Windows.Forms.Padding(3);
            this.tabCtrApproved.Size = new System.Drawing.Size(1044, 298);
            this.tabCtrApproved.TabIndex = 1;
            this.tabCtrApproved.Text = "Approved";
            this.tabCtrApproved.UseVisualStyleBackColor = true;
            // 
            // dgv_Approved
            // 
            this.dgv_Approved.BackgroundColor = System.Drawing.Color.White;
            this.dgv_Approved.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Approved.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Approved.Location = new System.Drawing.Point(3, 3);
            this.dgv_Approved.Name = "dgv_Approved";
            this.dgv_Approved.Size = new System.Drawing.Size(1038, 292);
            this.dgv_Approved.TabIndex = 0;
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
            this.btn_approve.Click += new System.EventHandler(this.btn_approve_Click);
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
            this.btn_reject.Click += new System.EventHandler(this.btn_reject_Click);
            // 
            // panel6
            // 
            this.panel6.Location = new System.Drawing.Point(175, 58);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(352, 131);
            this.panel6.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.lblApprovedCount);
            this.panel7.Controls.Add(this.label1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1058, 131);
            this.panel7.TabIndex = 1;
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.lblForApproval);
            this.panel8.Controls.Add(this.label2);
            this.panel8.Location = new System.Drawing.Point(353, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(352, 131);
            this.panel8.TabIndex = 2;
            // 
            // panel9
            // 
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.lblRejectedCount);
            this.panel9.Controls.Add(this.label3);
            this.panel9.Location = new System.Drawing.Point(706, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(352, 131);
            this.panel9.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Approved";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Pending";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Rejected";
            // 
            // lblApprovedCount
            // 
            this.lblApprovedCount.AutoSize = true;
            this.lblApprovedCount.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApprovedCount.Location = new System.Drawing.Point(263, 50);
            this.lblApprovedCount.Name = "lblApprovedCount";
            this.lblApprovedCount.Size = new System.Drawing.Size(72, 30);
            this.lblApprovedCount.TabIndex = 1;
            this.lblApprovedCount.Text = "label4";
            // 
            // lblForApproval
            // 
            this.lblForApproval.AutoSize = true;
            this.lblForApproval.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblForApproval.Location = new System.Drawing.Point(263, 50);
            this.lblForApproval.Name = "lblForApproval";
            this.lblForApproval.Size = new System.Drawing.Size(72, 30);
            this.lblForApproval.TabIndex = 2;
            this.lblForApproval.Text = "label4";
            // 
            // lblRejectedCount
            // 
            this.lblRejectedCount.AutoSize = true;
            this.lblRejectedCount.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRejectedCount.Location = new System.Drawing.Point(267, 50);
            this.lblRejectedCount.Name = "lblRejectedCount";
            this.lblRejectedCount.Size = new System.Drawing.Size(72, 30);
            this.lblRejectedCount.TabIndex = 2;
            this.lblRejectedCount.Text = "label4";
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
            this.Load += new System.EventHandler(this.AdminForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.tabCtr_forApproval.ResumeLayout(false);
            this.tabCtrforApproval.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_forApproval)).EndInit();
            this.tabCtrApproved.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Approved)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
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
        private System.Windows.Forms.Label lbl_adminLog;
        private System.Windows.Forms.Button btn_logout;
        private System.Windows.Forms.TabControl tabCtr_forApproval;
        private System.Windows.Forms.TabPage tabCtrforApproval;
        private System.Windows.Forms.DataGridView dgv_forApproval;
        private System.Windows.Forms.TabPage tabCtrApproved;
        private System.Windows.Forms.DataGridView dgv_Approved;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblApprovedCount;
        private System.Windows.Forms.Label lblRejectedCount;
        private System.Windows.Forms.Label lblForApproval;
    }
}