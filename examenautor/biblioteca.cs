using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace examenautor
{
    public partial class biblioteca : Form
    {

        public string conexiondbsql = Properties.Settings.Default.conexion;
        public string sqlconsulta = "";
        public int id = 0;

        public biblioteca()
        {
            InitializeComponent();
        }

        private void refrescar()
        {
            var consulta = " select numero , cat.categoria, anio, aut.Autor, libro  from bibliotecadigital bib  " +
                " inner join categoria  cat on cat.idcategoria = bib.Categoria "+
                "inner join Autor$ aut on aut.IdAutor = bib.Autor";

            SqlDataAdapter da = new SqlDataAdapter(consulta, conexiondbsql);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dgvautores.DataSource = ds.Tables[0];

        }

        private void cargarautores()
        {

            var consulta = string.Format(" select idautor, Autor from [dbo].[Autor$] " );
            SqlDataAdapter da = new SqlDataAdapter(consulta, conexiondbsql);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmbautor.DisplayMember = "Autor";
            cmbautor.ValueMember = "idautor";
            cmbautor.DataSource = ds.Tables[0];


        }

        private void cargaracategoria()
        {

            var consulta = string.Format(" select idcategoria, categoria  from categoria ");
            SqlDataAdapter da = new SqlDataAdapter(consulta, conexiondbsql);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmbcategoria.DisplayMember = "categoria";
            cmbcategoria.ValueMember = "idcategoria";
            cmbcategoria.DataSource = ds.Tables[0];


        }




        private void biblioteca_Load(object sender, EventArgs e)
        {
            this.refrescar();
            this.cargaracategoria();
            this.cargarautores();
        }

        //private void btnagregar_MouseClick(object sender, MouseEventArgs e)
        //{

        //}

        private void btnagregar_Click(object sender, EventArgs e)
        {

            if (txtlibro.Text == "" || txtanio.Text == "")
            {
                MessageBox.Show("El biblioteca  esta vacio.");
            }
            else
            {


             

                SqlConnection conexion = new SqlConnection(conexiondbsql);
                conexion.Open();
               
                SqlCommand cmd = new SqlCommand("insertabiblioteca", conexion);
                cmd.CommandType  =CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idcategoria", SqlDbType.Int).Value = cmbcategoria.SelectedValue;
                cmd.Parameters.AddWithValue("@anio", SqlDbType.Int).Value = Convert.ToInt32( txtanio.Text);
                cmd.Parameters.AddWithValue("@autor", SqlDbType.Int).Value = cmbautor.SelectedValue;
                cmd.Parameters.AddWithValue("@Libro", SqlDbType.NVarChar).Value = txtlibro.Text;
                cmd.ExecuteNonQuery();

                txtlibro.Text = "";
                txtanio.Text = "";
                this.refrescar();
                MessageBox.Show("Se agrego a la biblioteca de forma exitosa.");
                conexion.Close();
            }

        }

        private void btneditar_Click(object sender, EventArgs e)
        {

            if (txtlibro.Text == "" || txtanio.Text == "")
            {
                MessageBox.Show("El biblioteca  esta vacio.");
            }
            else
            {




                SqlConnection conexion = new SqlConnection(conexiondbsql);
                conexion.Open();

                SqlCommand cmd = new SqlCommand("editabiblioteca", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idcategoria", SqlDbType.Int).Value = cmbcategoria.SelectedValue;
                cmd.Parameters.AddWithValue("@anio", SqlDbType.Int).Value = Convert.ToInt32(txtanio.Text);
                cmd.Parameters.AddWithValue("@autor", SqlDbType.Int).Value = cmbautor.SelectedValue;
                cmd.Parameters.AddWithValue("@Libro", SqlDbType.NVarChar).Value = txtlibro.Text;
                cmd.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                cmd.ExecuteNonQuery();

                txtlibro.Text = "";
                txtanio.Text = "";
                this.refrescar();
                MessageBox.Show("Se agrego a la biblioteca de forma exitosa.");
                conexion.Close();
            }

        }

        private void dgvautores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;

            DataGridViewRow selectedrow = dgvautores.Rows[index];
            id = Convert.ToInt32(selectedrow.Cells[0].Value);
            cmbcategoria.Text = selectedrow.Cells[1].Value.ToString();
            txtanio.Text = selectedrow.Cells[2].Value.ToString();
            cmbautor.Text = selectedrow.Cells[3].Value.ToString();
            txtlibro.Text = selectedrow.Cells[4].Value.ToString();
         


        }

        private void btneliminar_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                MessageBox.Show("El biblioteca  esta vacio.");
            }
            else
            {




                SqlConnection conexion = new SqlConnection(conexiondbsql);
                conexion.Open();

                SqlCommand cmd = new SqlCommand("deletebiblioteca", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", SqlDbType.Int).Value = id;

                cmd.ExecuteNonQuery();

                txtlibro.Text = "";
                txtanio.Text = "";
                this.refrescar();
                MessageBox.Show("Se elimino a la biblioteca de forma exitosa.");
                conexion.Close();
            }

        }
    }
}
