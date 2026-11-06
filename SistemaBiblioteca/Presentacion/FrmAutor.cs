using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SistemaBiblioteca.Logica;
using SistemaBiblioteca.Datos;
namespace SistemaBiblioteca.Presentacion
{
    public partial class FrmAutor : Form
    {
        private int idAutorSeleccionado = 0;
        
        public FrmAutor()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {// 1. Validación de campos obligatorios
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("El nombre y el apellido son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Detiene el proceso si faltan datos
            }

            // 2. Capturar los datos en el objeto Autor
            Autor nuevoAutor = new Autor();
            nuevoAutor.Nombre = txtNombre.Text.Trim();
            nuevoAutor.Apellido = txtApellido.Text.Trim();

            // 3. Enviar a la capa de datos
            DatosAutor datos = new DatosAutor();
            bool exito = datos.InsertarAutor(nuevoAutor);

            // 4. Mostrar resultado y limpiar campos
            if (exito == true)
            {
                MessageBox.Show("Autor registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNombre.Clear();
                txtApellido.Clear(); 
                dgvAutores.DataSource = datos.MostrarAutores();
            }
            else
            {
                MessageBox.Show("Ocurrió un error al guardar el autor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dgvAutores_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void FrmAutor_Load(object sender, EventArgs e)
        {
            DatosAutor datos = new DatosAutor();
            dgvAutores.DataSource = datos.MostrarAutores();
        }

        private void dgvAutores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                idAutorSeleccionado = Convert.ToInt32(dgvAutores.Rows[e.RowIndex].Cells["IdAutor"].Value);
                txtNombre.Text = dgvAutores.Rows[e.RowIndex].Cells["Nombre"].Value.ToString();
                txtApellido.Text = dgvAutores.Rows[e.RowIndex].Cells["Apellido"].Value.ToString();
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idAutorSeleccionado > 0)
            {
                Autor autorModificado = new Autor();
                autorModificado.IdAutor = idAutorSeleccionado;
                autorModificado.Nombre = txtNombre.Text;
                autorModificado.Apellido = txtApellido.Text;

                DatosAutor datos = new DatosAutor();
                if (datos.ModificarAutor(autorModificado))
                {
                    MessageBox.Show("Autor modificado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Actualizar la tabla y limpiar las cajas de texto
                    dgvAutores.DataSource = datos.MostrarAutores();
                    txtNombre.Clear();
                    txtApellido.Clear();
                    idAutorSeleccionado = 0; // Reiniciamos el ID
                }
                else
                {
                    MessageBox.Show("Error al modificar el autor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un autor de la lista haciendo doble clic.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idAutorSeleccionado > 0)
            {
                DialogResult confirmacion = MessageBox.Show("¿Está seguro que desea eliminar este autor?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmacion == DialogResult.Yes)
                {
                    DatosAutor datos = new DatosAutor();
                    if (datos.EliminarAutor(idAutorSeleccionado))
                    {
                        MessageBox.Show("Autor eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Actualizar la tabla y limpiar
                        dgvAutores.DataSource = datos.MostrarAutores();
                        txtNombre.Clear();
                        txtApellido.Clear();
                        idAutorSeleccionado = 0;
                    }
                    else
                    {
                        MessageBox.Show("Error al eliminar el autor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un autor de la lista haciendo doble clic.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
