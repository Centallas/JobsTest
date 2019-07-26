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

namespace PRUEBA_TECNICA
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {

            txtNombres.Text = txtApellidos.Text = txtDireccion.Text = "";
            btnSave.Text = "Guardar";
            //btnCancel.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtNombres.Text.Trim();
            model.LastName = txtApellidos.Text.Trim();
            model.Address = txtDireccion.Text.Trim();
            using (DBEntities db = new DBEntities())
            {
                if (model.CustomerID == 0) //Insert              
                    db.Customers.Add(model);
                else //Update   
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Enviado satisfactoriamente");

        }

        void PopulateDataGridView()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using (DBEntities db = new DBEntities())
            {
                dgvCustomer.DataSource = db.Customers.ToList<Customer>();

            }

        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["CustomerID"].Value);
                using (DBEntities db = new DBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtNombres.Text = model.FirstName;
                    txtApellidos.Text = model.LastName;
                    txtDireccion.Text = model.Address;

                }
                btnSave.Text = "Actualizar";
                btnDelete.Enabled = true;
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("¿Esta seguro/a que desea borrar este registro?","EF CRUD Operaation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Customers.Attach(model);
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Registro borrado exitosamente");
                   

                }

            }

        }
    }
}
