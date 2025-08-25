namespace LogPriceChange0._1
{
    partial class ctrDashboard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblDocStat = new System.Windows.Forms.Label();
            this.dgvDocStat = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnloadForm = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocStat)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblDocStat);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1012, 32);
            this.panel2.TabIndex = 8;
            // 
            // lblDocStat
            // 
            this.lblDocStat.AutoSize = true;
            this.lblDocStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocStat.Location = new System.Drawing.Point(17, 9);
            this.lblDocStat.Name = "lblDocStat";
            this.lblDocStat.Size = new System.Drawing.Size(69, 20);
            this.lblDocStat.TabIndex = 0;
            this.lblDocStat.Text = "Promos";
            // 
            // dgvDocStat
            // 
            this.dgvDocStat.AllowUserToAddRows = false;
            this.dgvDocStat.BackgroundColor = System.Drawing.Color.Snow;
            this.dgvDocStat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDocStat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDocStat.Location = new System.Drawing.Point(0, 0);
            this.dgvDocStat.Name = "dgvDocStat";
            this.dgvDocStat.Size = new System.Drawing.Size(1012, 441);
            this.dgvDocStat.TabIndex = 0;
            this.dgvDocStat.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocStat_CellContentClick);
            this.dgvDocStat.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocStat_CellEndEdit);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgvDocStat);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 72);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1012, 441);
            this.panel3.TabIndex = 9;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 513);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1012, 45);
            this.panel4.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnloadForm);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5, 5, 30, 5);
            this.panel1.Size = new System.Drawing.Size(1012, 40);
            this.panel1.TabIndex = 7;
            // 
            // btnloadForm
            // 
            this.btnloadForm.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnloadForm.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnloadForm.Location = new System.Drawing.Point(881, 5);
            this.btnloadForm.Name = "btnloadForm";
            this.btnloadForm.Size = new System.Drawing.Size(101, 30);
            this.btnloadForm.TabIndex = 2;
            this.btnloadForm.Text = "Load Form";
            this.btnloadForm.UseVisualStyleBackColor = true;
            this.btnloadForm.Click += new System.EventHandler(this.btnloadForm_Click);
            // 
            // ctrDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Name = "ctrDashboard";
            this.Size = new System.Drawing.Size(1012, 558);
            this.Load += new System.EventHandler(this.ctrDashboard_Load_1);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDocStat)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblDocStat;
        private System.Windows.Forms.DataGridView dgvDocStat;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnloadForm;
    }
}
