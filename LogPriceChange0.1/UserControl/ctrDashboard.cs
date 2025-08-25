using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LogPriceChange0._1
{
    public partial class ctrDashboard : UserControl
    {

        private string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Desktop\CameraHaus\LogPriceChange_Demo\pricematrix.accdb";

        private OleDbConnection connection;

        private ctrLogPriceChange _ctrLogPriceChange;

        private BindingSource bindingSource = new BindingSource();


        private string loggedUsername = UserSession.Username;


        #region ************************** METHODS **************************
        private void dgvDocStat_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var dgv = sender as DataGridView;
            var row = dgv.Rows[e.RowIndex];

            var docIdValue = row.Cells["DocID"].Value;
            if (docIdValue != DBNull.Value && docIdValue != null && !string.IsNullOrEmpty(docIdValue.ToString()))
            {
                row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFECA1");
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }
      
        private void LoadDataByStatus(string docStatus)
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM tbl_logpricechange WHERE DocStatus = @status";

                    using (OleDbCommand cmd = new OleDbCommand(query, connection))
                    {
                        // Add parameter
                        cmd.Parameters.AddWithValue("@status", docStatus);

                        // Fill DataTable
                        OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Bind to DataGridView
                        dgvDocStat.DataSource = dt;

                        dgvDocStat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                        dgvDocStat.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                        dgvDocStat.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                        dgvDocStat.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

                        

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message);
                }
            }
        }
        // Function to determine the type of row based on column suffixes
      
        #endregion

        public ctrDashboard()
        {
            InitializeComponent(); 
            LoadDataByStatus("DRAFT");

        }
        private void ctrDashboard_Load_1(object sender, EventArgs e)
        {
            //LoadDataAndStatus();
            LoadDataByStatus("DRAFT");
        }
        #region ************************** DataGridView Cell Edit Logic **************************
        public class ColumnMapping
        {
            public int BaseIndex { get; set; }
            public int ValueIndex { get; set; }
            public int RateIndex { get; set; }
        }

        private readonly List<ColumnMapping> columnMappings = new List<ColumnMapping>
        {
            //supplier column mappings
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  43  ,  RateIndex =  40  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  49  ,  RateIndex =  46  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  55  ,  RateIndex =  52  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  61  ,  RateIndex =  58  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  67  ,  RateIndex =  64  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  73  ,  RateIndex =  70  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  79  ,  RateIndex =  76  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  85  ,  RateIndex =  82  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  91  ,  RateIndex =  88  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  97  ,  RateIndex =  94  },
            new ColumnMapping { BaseIndex = 37  , ValueIndex =  109 ,  RateIndex =  106 },
            //promo column mappings
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  44  ,  RateIndex =  41  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  50  ,  RateIndex =  47  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  56  ,  RateIndex =  53  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  62  ,  RateIndex =  59  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  68  ,  RateIndex =  65  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  74  ,  RateIndex =  71  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  80  ,  RateIndex =  77  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  86  ,  RateIndex =  83  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  92  ,  RateIndex =  89  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  98  ,  RateIndex =  95  },
            new ColumnMapping { BaseIndex = 38  , ValueIndex =  110 ,  RateIndex =  107 },




        };

        private void UpdateDependentValues(int rowIndex, int editedColumnIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvDocStat.Rows.Count)
                return;

            var row = dgvDocStat.Rows[rowIndex];

            foreach (var mapping in columnMappings)
            {
                if (editedColumnIndex != mapping.ValueIndex && editedColumnIndex != mapping.RateIndex)
                    continue;

                // Get Base value
                if (!decimal.TryParse(row.Cells[mapping.BaseIndex]?.Value?.ToString(), out decimal baseValue) || baseValue == 0)
                    continue;

                if (editedColumnIndex == mapping.ValueIndex &&
                    decimal.TryParse(row.Cells[mapping.ValueIndex]?.Value?.ToString(), out decimal val))
                {
                    // Calculate rate
                    decimal newRate = Math.Round((val / baseValue) * 100, 2);

                    // If bound to a DataTable, update DataRow directly
                    if (row.DataBoundItem is DataRowView drv)
                        drv[mapping.RateIndex] = newRate;
                    else
                        row.Cells[mapping.RateIndex].Value = newRate;
                }
                else if (editedColumnIndex == mapping.RateIndex &&
                         decimal.TryParse(row.Cells[mapping.RateIndex]?.Value?.ToString(), out decimal rate))
                {
                    // Calculate value
                    decimal newValue = Math.Round((baseValue * rate) / 100, 2);

                    if (row.DataBoundItem is DataRowView drv)
                        drv[mapping.ValueIndex] = newValue;
                    else
                        row.Cells[mapping.ValueIndex].Value = newValue;
                }
            }
        }

        private void dgvDocStat_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Make sure the cell value is committed before reading it
                dgvDocStat.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dgvDocStat.EndEdit();

                UpdateDependentValues(e.RowIndex, e.ColumnIndex);
            }
        }


        #endregion

        private void dgvDocStat_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string docId = dgvDocStat.Rows[e.RowIndex].Cells["DocID"].Value?.ToString();
                int id = Convert.ToInt32(dgvDocStat.Rows[e.RowIndex].Cells["ID"].Value);

                if (!string.IsNullOrEmpty(docId))
                {
                    // Pass both DocID (string) and ID (int)
                    _ctrLogPriceChange = new ctrLogPriceChange(docId);
                    LoadControl(_ctrLogPriceChange);
                }
            }
        }

        private void LoadControl(UserControl control)
        {
            if (control == null) control = new ctrLogPriceChange();

            control.Dock = DockStyle.Fill;

            // Use the host that contains this dashboard (usually a Panel on the Form)
            Control host = this.Parent ?? this;

            host.SuspendLayout();
            host.Controls.Clear();
            host.Controls.Add(control);
            control.BringToFront();
            host.ResumeLayout();
        }

        private void btnloadForm_Click(object sender, EventArgs e)
        {
            LoadDataByStatus("DRAFT");
        }
    }
}
