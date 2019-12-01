using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityFrameworkCRUD
{
    public partial class Form1 : Form
    {
        string stitle = "Data";
        Customer model = new Customer();
        public Form1()
        {
           
            InitializeComponent();
        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void Validasi()
        {
            
            txtFullName.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtAddress.Text = string.Empty;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtFullName.Text == string.Empty || txtCity.Text == string.Empty)
            {
                MessageBox.Show("FirstName Wajib Di isi", stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
               
                model.FirstName = txtFullName.Text.Trim();
                model.LastName = txtLastName.Text.Trim();
                model.City = txtCity.Text.Trim();
                model.Address = txtAddress.Text.Trim();


                using (EFDBEntities db = new EFDBEntities())
                {
                    if (model.CustomerID == 0)//insert

                        db.Customers.Add(model);
                    else
                        db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                Clear();
                MessageBox.Show("Data Berhasil ditambahkan");
                LoadData();
            }
        }
        void Clear()
        {
            txtFullName.Text =
            txtLastName.Text = 
            txtCity.Text = 
            txtAddress.Text = "";

            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            this.ActiveControl = txtFullName;
            LoadData();
        }
        void LoadData()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using(EFDBEntities db = new EFDBEntities())
            {
                dgvCustomer.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void DgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["dgvCustomerID"].Value);
                using (EFDBEntities db = new EFDBEntities())
                {
                    model = db.Customers.Where(X => X.CustomerID == model.CustomerID).FirstOrDefault();
                    txtFullName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;
                    txtCity.Text = model.City;
                    txtAddress.Text = model.Address;

                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you Sure to Delete this Data","Message", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (EFDBEntities db = new EFDBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State==EntityState.Detached)
                    {
                        db.Customers.Attach(model);
                        db.Customers.Remove(model);
                        db.SaveChanges();
                        LoadData();
                        Clear();
                        MessageBox.Show("Deleted Successfully");
                    }
                }
            }
        }
    }
}
