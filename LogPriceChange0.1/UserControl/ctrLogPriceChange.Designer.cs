namespace LogPriceChange0._1
{
    partial class ctrLogPriceChange
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
            this.components = new System.ComponentModel.Container();
            this.cmsRemoveGroup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsRemoveGroupItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnlpcsubmit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btn_draft = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv_Main = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tb_docID = new System.Windows.Forms.TextBox();
            this.lpc_dgv_searchbycode = new System.Windows.Forms.DataGridView();
            this.label8 = new System.Windows.Forms.Label();
            this.lpc_rbtn_temporary = new System.Windows.Forms.RadioButton();
            this.lpc_rbtn_permanent = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.lpc_tb_searchbycode = new System.Windows.Forms.TextBox();
            this.lpc_tb_promotitle = new System.Windows.Forms.TextBox();
            this.lpc_tb_supplier = new System.Windows.Forms.TextBox();
            this.lpc_dtp_enddate = new System.Windows.Forms.DateTimePicker();
            this.lpc_dtp_startdate = new System.Windows.Forms.DateTimePicker();
            this.lpc_dtp_memodate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmsRemoveGroup.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Main)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lpc_dgv_searchbycode)).BeginInit();
            this.SuspendLayout();
            // 
            // cmsRemoveGroup
            // 
            this.cmsRemoveGroup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsRemoveGroupItem});
            this.cmsRemoveGroup.Name = "cmsRemoveGroup";
            this.cmsRemoveGroup.Size = new System.Drawing.Size(199, 26);
            // 
            // cmsRemoveGroupItem
            // 
            this.cmsRemoveGroupItem.Name = "cmsRemoveGroupItem";
            this.cmsRemoveGroupItem.Size = new System.Drawing.Size(198, 22);
            this.cmsRemoveGroupItem.Text = "Remove Product Group";
            // 
            // btnlpcsubmit
            // 
            this.btnlpcsubmit.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnlpcsubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnlpcsubmit.Location = new System.Drawing.Point(997, 5);
            this.btnlpcsubmit.Name = "btnlpcsubmit";
            this.btnlpcsubmit.Size = new System.Drawing.Size(101, 40);
            this.btnlpcsubmit.TabIndex = 0;
            this.btnlpcsubmit.Text = "Submit";
            this.btnlpcsubmit.UseVisualStyleBackColor = true;
            this.btnlpcsubmit.Click += new System.EventHandler(this.btnlpcsubmit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btn_draft);
            this.panel1.Controls.Add(this.btnlpcsubmit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(10, 558);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5, 5, 10, 5);
            this.panel1.Size = new System.Drawing.Size(1108, 50);
            this.panel1.TabIndex = 15;
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(795, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(101, 40);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear form";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_draft
            // 
            this.btn_draft.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_draft.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_draft.Location = new System.Drawing.Point(896, 5);
            this.btn_draft.Name = "btn_draft";
            this.btn_draft.Size = new System.Drawing.Size(101, 40);
            this.btn_draft.TabIndex = 1;
            this.btn_draft.Text = "Save Draft";
            this.btn_draft.UseVisualStyleBackColor = true;
            this.btn_draft.Click += new System.EventHandler(this.btn_draft_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(180, 229);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 16;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgv_Main);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(10, 210);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1108, 348);
            this.panel2.TabIndex = 17;
            // 
            // dgv_Main
            // 
            this.dgv_Main.AllowUserToAddRows = false;
            this.dgv_Main.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgv_Main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Main.Location = new System.Drawing.Point(0, 0);
            this.dgv_Main.Name = "dgv_Main";
            this.dgv_Main.Size = new System.Drawing.Size(1108, 348);
            this.dgv_Main.TabIndex = 5;
            this.dgv_Main.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgv_Main_RowPostPaint);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.tb_docID);
            this.panel3.Controls.Add(this.lpc_dgv_searchbycode);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.lpc_rbtn_temporary);
            this.panel3.Controls.Add(this.lpc_rbtn_permanent);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.lpc_tb_searchbycode);
            this.panel3.Controls.Add(this.lpc_tb_promotitle);
            this.panel3.Controls.Add(this.lpc_tb_supplier);
            this.panel3.Controls.Add(this.lpc_dtp_enddate);
            this.panel3.Controls.Add(this.lpc_dtp_startdate);
            this.panel3.Controls.Add(this.lpc_dtp_memodate);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1108, 200);
            this.panel3.TabIndex = 18;
            // 
            // tb_docID
            // 
            this.tb_docID.Location = new System.Drawing.Point(362, 6);
            this.tb_docID.Name = "tb_docID";
            this.tb_docID.Size = new System.Drawing.Size(138, 20);
            this.tb_docID.TabIndex = 35;
            this.tb_docID.Visible = false;
            this.tb_docID.TextChanged += new System.EventHandler(this.tb_docID_TextChanged);
            // 
            // lpc_dgv_searchbycode
            // 
            this.lpc_dgv_searchbycode.AllowUserToAddRows = false;
            this.lpc_dgv_searchbycode.AllowUserToDeleteRows = false;
            this.lpc_dgv_searchbycode.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.lpc_dgv_searchbycode.BackgroundColor = System.Drawing.Color.White;
            this.lpc_dgv_searchbycode.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lpc_dgv_searchbycode.Location = new System.Drawing.Point(673, 39);
            this.lpc_dgv_searchbycode.Name = "lpc_dgv_searchbycode";
            this.lpc_dgv_searchbycode.Size = new System.Drawing.Size(432, 144);
            this.lpc_dgv_searchbycode.TabIndex = 34;
            this.lpc_dgv_searchbycode.Visible = false;
            this.lpc_dgv_searchbycode.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.lpc_dgv_searchbycode_CellValueChanged);
            this.lpc_dgv_searchbycode.CurrentCellDirtyStateChanged += new System.EventHandler(this.lpc_dgv_searchbycode_CurrentCellDirtyStateChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(33, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 20);
            this.label8.TabIndex = 28;
            this.label8.Text = "Promo Type :";
            // 
            // lpc_rbtn_temporary
            // 
            this.lpc_rbtn_temporary.AutoSize = true;
            this.lpc_rbtn_temporary.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lpc_rbtn_temporary.Location = new System.Drawing.Point(246, 91);
            this.lpc_rbtn_temporary.Name = "lpc_rbtn_temporary";
            this.lpc_rbtn_temporary.Size = new System.Drawing.Size(88, 24);
            this.lpc_rbtn_temporary.TabIndex = 27;
            this.lpc_rbtn_temporary.Text = "Temporary";
            this.lpc_rbtn_temporary.UseVisualStyleBackColor = true;
            // 
            // lpc_rbtn_permanent
            // 
            this.lpc_rbtn_permanent.AutoSize = true;
            this.lpc_rbtn_permanent.Checked = true;
            this.lpc_rbtn_permanent.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lpc_rbtn_permanent.Location = new System.Drawing.Point(131, 91);
            this.lpc_rbtn_permanent.Name = "lpc_rbtn_permanent";
            this.lpc_rbtn_permanent.Size = new System.Drawing.Size(88, 24);
            this.lpc_rbtn_permanent.TabIndex = 26;
            this.lpc_rbtn_permanent.TabStop = true;
            this.lpc_rbtn_permanent.Text = "Permanent";
            this.lpc_rbtn_permanent.UseVisualStyleBackColor = true;
            this.lpc_rbtn_permanent.CheckedChanged += new System.EventHandler(this.lpc_rbtn_permanent_CheckedChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial Rounded MT Bold", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(536, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "Search by code :";
            // 
            // lpc_tb_searchbycode
            // 
            this.lpc_tb_searchbycode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lpc_tb_searchbycode.Location = new System.Drawing.Point(673, 13);
            this.lpc_tb_searchbycode.Name = "lpc_tb_searchbycode";
            this.lpc_tb_searchbycode.Size = new System.Drawing.Size(209, 20);
            this.lpc_tb_searchbycode.TabIndex = 6;
            this.lpc_tb_searchbycode.TextChanged += new System.EventHandler(this.lpc_tb_searchbycode_TextChanged);
            // 
            // lpc_tb_promotitle
            // 
            this.lpc_tb_promotitle.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lpc_tb_promotitle.Location = new System.Drawing.Point(132, 59);
            this.lpc_tb_promotitle.Name = "lpc_tb_promotitle";
            this.lpc_tb_promotitle.Size = new System.Drawing.Size(209, 25);
            this.lpc_tb_promotitle.TabIndex = 18;
            // 
            // lpc_tb_supplier
            // 
            this.lpc_tb_supplier.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lpc_tb_supplier.Location = new System.Drawing.Point(132, 33);
            this.lpc_tb_supplier.Name = "lpc_tb_supplier";
            this.lpc_tb_supplier.Size = new System.Drawing.Size(209, 25);
            this.lpc_tb_supplier.TabIndex = 17;
            // 
            // lpc_dtp_enddate
            // 
            this.lpc_dtp_enddate.CustomFormat = "MM/dd/yyyy hh:mm:ss tt";
            this.lpc_dtp_enddate.Enabled = false;
            this.lpc_dtp_enddate.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lpc_dtp_enddate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.lpc_dtp_enddate.Location = new System.Drawing.Point(131, 146);
            this.lpc_dtp_enddate.Name = "lpc_dtp_enddate";
            this.lpc_dtp_enddate.Size = new System.Drawing.Size(209, 25);
            this.lpc_dtp_enddate.TabIndex = 25;
            this.lpc_dtp_enddate.Value = new System.DateTime(2025, 7, 16, 10, 1, 31, 0);
            // 
            // lpc_dtp_startdate
            // 
            this.lpc_dtp_startdate.CustomFormat = "MM/dd/yyyy hh:mm:ss tt";
            this.lpc_dtp_startdate.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lpc_dtp_startdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.lpc_dtp_startdate.Location = new System.Drawing.Point(131, 120);
            this.lpc_dtp_startdate.Name = "lpc_dtp_startdate";
            this.lpc_dtp_startdate.Size = new System.Drawing.Size(209, 25);
            this.lpc_dtp_startdate.TabIndex = 24;
            // 
            // lpc_dtp_memodate
            // 
            this.lpc_dtp_memodate.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lpc_dtp_memodate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.lpc_dtp_memodate.Location = new System.Drawing.Point(132, 6);
            this.lpc_dtp_memodate.Name = "lpc_dtp_memodate";
            this.lpc_dtp_memodate.Size = new System.Drawing.Size(209, 25);
            this.lpc_dtp_memodate.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(35, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 20);
            this.label6.TabIndex = 19;
            this.label6.Text = "End Date    :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(35, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 20);
            this.label5.TabIndex = 20;
            this.label5.Text = "Start Date   :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(35, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 20);
            this.label4.TabIndex = 21;
            this.label4.Text = "Promo Title :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(35, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 20);
            this.label3.TabIndex = 22;
            this.label3.Text = "Supplier      :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(35, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 20);
            this.label2.TabIndex = 23;
            this.label2.Text = "Date            :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(386, -25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 22);
            this.label1.TabIndex = 15;
            this.label1.Text = "Log Price Change";
            // 
            // ctrLogPriceChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.panel1);
            this.Name = "ctrLogPriceChange";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(1128, 618);
            this.Load += new System.EventHandler(this.ctrLogPriceChange_Load);
            this.cmsRemoveGroup.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Main)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lpc_dgv_searchbycode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnlpcsubmit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ContextMenuStrip cmsRemoveGroup;
        private System.Windows.Forms.ToolStripMenuItem cmsRemoveGroupItem;
        private System.Windows.Forms.Button btn_draft;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton lpc_rbtn_temporary;
        private System.Windows.Forms.RadioButton lpc_rbtn_permanent;
        private System.Windows.Forms.TextBox lpc_tb_promotitle;
        private System.Windows.Forms.TextBox lpc_tb_supplier;
        private System.Windows.Forms.DateTimePicker lpc_dtp_enddate;
        private System.Windows.Forms.DateTimePicker lpc_dtp_startdate;
        private System.Windows.Forms.DateTimePicker lpc_dtp_memodate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox lpc_tb_searchbycode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgv_Main;
        public System.Windows.Forms.DataGridView lpc_dgv_searchbycode;
        private System.Windows.Forms.TextBox tb_docID;
    }
}
