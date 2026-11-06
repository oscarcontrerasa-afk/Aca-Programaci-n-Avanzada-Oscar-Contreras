using SistemaBiblioteca.Datos;
using System;
using System.Data;
using System.Windows.Forms;

namespace SistemaBiblioteca
{
    public partial class FrmLibro : Form
    {
        DatosLibro objetoDatos = new DatosLibro();
        private int idLibroSeleccionado = 0;

        public FrmLibro()
        {
            InitializeComponent();
        }

        private void FrmLibro_Load(object sender, EventArgs e)
        {
            this.Text = "Gestión de Libros";
            CargarAutores();
            CargarLibros();
        }

        private void CargarLibros()
        {
            dgvLibros.DataSource = objetoDatos.MostrarLibros();
        }

        private void CargarAutores()
        {
            try
            {
                DatosAutor objetoAutor = new DatosAutor();
                DataTable dtAutores = objetoAutor.MostrarAutores();
                cmbAutor.DataSource = dtAutores;

                if (dtAutores.Columns.Contains("Nombre"))
                    cmbAutor.DisplayMember = "Nombre";
                else if (dtAutores.Columns.Contains("NombreAutor"))
                    cmbAutor.DisplayMember = "NombreAutor";

                cmbAutor.ValueMember = "IdAutor";
            }
            catch
            {
                // Manejo de autores
            }
        }

        private void dgvLibros_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvLibros.Rows[e.RowIndex];

                if (fila.Cells["IdLibro"].Value != DBNull.Value)
                {
                    idLibroSeleccionado = Convert.ToInt32(fila.Cells["IdLibro"].Value);
                    txtTitulo.Text = fila.Cells["Titulo"].Value?.ToString() ?? "";

                    if (txtAnio != null && fila.Cells["AnioPublicacion"].Value != DBNull.Value)
                    {
                        txtAnio.Text = fila.Cells["AnioPublicacion"].Value.ToString();
                    }

                    if (fila.Cells["IdAutor"].Value != DBNull.Value)
                    {
                        cmbAutor.SelectedValue = fila.Cells["IdAutor"].Value;
                    }
                }
            }
        }

        private void dgvLibros_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvLibros_CellClick(sender, e);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
            {
                MessageBox.Show("Por favor ingresa un título para el libro.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int anioVal = 0;
            if (txtAnio != null)
            {
                int.TryParse(txtAnio.Text.Trim(), out anioVal);
            }

            Libro nuevoLibro = new Libro
            {
                Titulo = txtTitulo.Text.Trim(),
                AnioPublicacion = anioVal,
                IdAutor = Convert.ToInt32(cmbAutor.SelectedValue)
            };

            if (objetoDatos.InsertarLibro(nuevoLibro))
            {
                MessageBox.Show("Libro registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarLibros();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error al guardar el libro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (idLibroSeleccionado == 0)
            {
                MessageBox.Show("Por favor selecciona un libro de la tabla para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int anioVal = 0;
            if (txtAnio != null)
            {
                int.TryParse(txtAnio.Text.Trim(), out anioVal);
            }

            Libro libroEditar = new Libro
            {
                IdLibro = idLibroSeleccionado,
                Titulo = txtTitulo.Text.Trim(),
                AnioPublicacion = anioVal,
                IdAutor = Convert.ToInt32(cmbAutor.SelectedValue)
            };

            if (objetoDatos.ModificarLibro(libroEditar))
            {
                MessageBox.Show("Libro actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarLibros();
                LimpiarCampos();
            }
            else
            {
                MessageBox.Show("Error al modificar el libro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idLibroSeleccionado == 0)
            {
                MessageBox.Show("Por favor selecciona un libro de la tabla para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Estás seguro de que deseas eliminar este libro?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                if (objetoDatos.EliminarLibro(idLibroSeleccionado))
                {
                    MessageBox.Show("Libro eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarLibros();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Error al eliminar el libro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LimpiarCampos()
        {
            txtTitulo.Clear();
            if (txtAnio != null) txtAnio.Clear();
            idLibroSeleccionado = 0;
            if (cmbAutor.Items.Count > 0) cmbAutor.SelectedIndex = 0;
        }
    }
}