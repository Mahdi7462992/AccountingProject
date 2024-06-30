using Accounting.DataLayer.Context;
using Accounting.utility.Convertor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting.App.Accounting
{
    public partial class frmReport : Form
    {
        public int TypeID = 0;
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            if (TypeID == 1)
            {
                this.Text = "گزارش دریافتی ها";
            }
            else
            {
                this.Text = "گزارش پرداختی ها";
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Filter();
        }

        void Filter()
        {
            using(UnitOfWork db = new UnitOfWork())
            {
                var result = db.AccountingRepository.Get(a => a.TypeID == TypeID);
                //dgReport.AutoGenerateColumns = false;
                //dgReport.DataSource= result;
                dgReport.Rows.Clear();

                foreach(var accounting in result)
                {
                    string customerName = db.CustomerRepository.GetCustomerNameById(accounting.CustomerID);
                    dgReport.Rows.Add(accounting.ID,customerName,accounting.Amount,accounting.DateTitle.ToShamsi(),accounting.Description);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Filter();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(dgReport.CurrentRow!=null)
            {
                int id= int.Parse(dgReport.CurrentRow.Cells[0].Value.ToString());
                if(RtlMessageBox.Show("آیا از حذف مطمئن هستید","هشدار",MessageBoxButtons.YesNo)
                    ==DialogResult.Yes)
                {
                    using(UnitOfWork db= new UnitOfWork())
                    {
                        db.AccountingRepository.Delete(id);
                        db.Save();
                        Filter();
                    }
                }
            }
        }
    }
}
