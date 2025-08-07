namespace LogPriceChange0._1
{
    partial class wfLoginform
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
            this.logf_tb_username = new System.Windows.Forms.TextBox();
            this.logf_tb_password = new System.Windows.Forms.TextBox();
            this.logf_btn_login = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.logf_pb_eyeopen = new System.Windows.Forms.PictureBox();
            this.logf_pb_eyeclose = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.logf_pb_eyeopen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logf_pb_eyeclose)).BeginInit();
            this.SuspendLayout();
            // 
            // logf_tb_username
            // 
            this.logf_tb_username.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logf_tb_username.Location = new System.Drawing.Point(295, 147);
            this.logf_tb_username.Name = "logf_tb_username";
            this.logf_tb_username.Size = new System.Drawing.Size(248, 29);
            this.logf_tb_username.TabIndex = 1;
            // 
            // logf_tb_password
            // 
            this.logf_tb_password.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logf_tb_password.Location = new System.Drawing.Point(295, 185);
            this.logf_tb_password.Name = "logf_tb_password";
            this.logf_tb_password.Size = new System.Drawing.Size(248, 29);
            this.logf_tb_password.TabIndex = 2;
            // 
            // logf_btn_login
            // 
            this.logf_btn_login.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logf_btn_login.Location = new System.Drawing.Point(349, 221);
            this.logf_btn_login.Name = "logf_btn_login";
            this.logf_btn_login.Size = new System.Drawing.Size(150, 35);
            this.logf_btn_login.TabIndex = 3;
            this.logf_btn_login.Text = "Login";
            this.logf_btn_login.UseVisualStyleBackColor = true;
            this.logf_btn_login.Click += new System.EventHandler(this.logf_btn_login_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(197, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Username :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(197, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 21);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password :";
            // 
            // logf_pb_eyeopen
            // 
            this.logf_pb_eyeopen.BackColor = System.Drawing.Color.White;
            this.logf_pb_eyeopen.Image = global::LogPriceChange0._1.Properties.Resources.close;
            this.logf_pb_eyeopen.Location = new System.Drawing.Point(516, 188);
            this.logf_pb_eyeopen.Name = "logf_pb_eyeopen";
            this.logf_pb_eyeopen.Size = new System.Drawing.Size(24, 24);
            this.logf_pb_eyeopen.TabIndex = 5;
            this.logf_pb_eyeopen.TabStop = false;
            // 
            // logf_pb_eyeclose
            // 
            this.logf_pb_eyeclose.BackColor = System.Drawing.Color.White;
            this.logf_pb_eyeclose.Image = global::LogPriceChange0._1.Properties.Resources.view;
            this.logf_pb_eyeclose.Location = new System.Drawing.Point(516, 188);
            this.logf_pb_eyeclose.Name = "logf_pb_eyeclose";
            this.logf_pb_eyeclose.Size = new System.Drawing.Size(24, 24);
            this.logf_pb_eyeclose.TabIndex = 4;
            this.logf_pb_eyeclose.TabStop = false;
            // 
            // wfLoginform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.logf_pb_eyeopen);
            this.Controls.Add(this.logf_pb_eyeclose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.logf_btn_login);
            this.Controls.Add(this.logf_tb_password);
            this.Controls.Add(this.logf_tb_username);
            this.Name = "wfLoginform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "wfLoginform";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.wfLoginform_FormClosed);
            this.Load += new System.EventHandler(this.wfLoginform_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logf_pb_eyeopen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logf_pb_eyeclose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox logf_tb_username;
        private System.Windows.Forms.TextBox logf_tb_password;
        private System.Windows.Forms.Button logf_btn_login;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox logf_pb_eyeclose;
        private System.Windows.Forms.PictureBox logf_pb_eyeopen;
    }
}