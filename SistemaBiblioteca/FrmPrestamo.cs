using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

namespace SistemaBiblioteca
{
    public partial class FrmPrestamo : Form
    {
        DatosPrestamo datos = new DatosPrestamo();
        DatosUsuario datosUsuario = new DatosUsuario();
        DatosLibro datosLibro = new DatosLibro(); // Usamos la clase que ya tenías hecha

        ComboBox cmbUsuarios, cmbLibros;
        DataGridView dgvPrestamos;
        Button btnPrestar, btnDevolver;
        TextBox txtIdPrestamo;

        public FrmPrestamo()
        {
            InitializeComponent();
            ConfigurarInterfaz();
            CargarCombos();
            CargarDatos();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Gestión de Préstamos y Devoluciones";
            this.Size = new Size(850, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            int lblX = 20, ctrlX = 100, y = 20, spacing = 40;

            this.Controls.Add(new Label { Text = "Préstamo ID:", Left = lblX, Top = y, Width = 80 });
            txtIdPrestamo = new TextBox { Left = ctrlX, Top = y, Width = 50, ReadOnly = true };
            this.Controls.Add(txtIdPrestamo); y += spacing;

            this.Controls.Add(new Label { Text = "Usuario:", Left = lblX, Top = y, Width = 80 });
            cmbUsuarios = new ComboBox { Left = ctrlX, Top = y, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(cmbUsuarios); y += spacing;

            this.Controls.Add(new Label { Text = "Libro:", Left = lblX, Top = y, Width = 80 });
            cmbLibros = new ComboBox { Left = ctrlX, Top = y, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            this.Controls.Add(cmbLibros); y += spacing;

            btnPrestar = new Button { Text = "Registrar Préstamo", Left = 20, Top = y, Width = 130, Height = 35 };
            btnPrestar.Click += BtnPrestar_Click;

            btnDevolver = new Button { Text = "Registrar Devolución", Left = 160, Top = y, Width = 140, Height = 35 };
            btnDevolver.Click += BtnDevolver_Click;

            this.Controls.Add(btnPrestar);
            this.Controls.Add(btnDevolver);

            dgvPrestamos = new DataGridView { Left = 330, Top = 20, Width = 480, Height = 400, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvPrestamos.CellClick += DgvPrestamos_CellClick;
            this.Controls.Add(dgvPrestamos);
        }

        private void CargarCombos()
        {
            try
            {
                // Carga los usuarios en el ComboBox
                cmbUsuarios.DataSource = datosUsuario.MostrarUsuarios();
                cmbUsuarios.DisplayMember = "Documento"; // Muestra el documento en la lista
                cmbUsuarios.ValueMember = "IdUsuario";   // Pero guarda el ID internamente

                // Carga los libros en el ComboBox
                cmbLibros.DataSource = datosLibro.MostrarLibros();
                cmbLibros.DisplayMember = "Titulo";
                cmbLibros.ValueMember = "IdLibro";
            }
            catch { }
        }

        private void CargarDatos()
        {
            dgvPrestamos.DataSource = datos.MostrarPrestamos();
        }

        private void DgvPrestamos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtIdPrestamo.Text = dgvPrestamos.Rows[e.RowIndex].Cells["IdPrestamo"].Value.ToString();
            }
        }

        // Cumple RF05: Registrar préstamo validando selecciones
        private void BtnPrestar_Click(object sender, EventArgs e)
        {
            if (cmbUsuarios.SelectedValue == null || cmbLibros.SelectedValue == null)
            {
                MessageBox.Show("Seleccione un usuario y un libro de las listas.");
                return;
            }

            int idU = Convert.ToInt32(cmbUsuarios.SelectedValue);
            int idL = Convert.ToInt32(cmbLibros.SelectedValue);

            try
            {
                if (datos.RegistrarPrestamo(idU, idL))
                {
                    MessageBox.Show("Préstamo registrado exitosamente.");
                    CargarDatos();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // Cumple RF06: Registrar devolución actualizando fecha y estado
        private void BtnDevolver_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdPrestamo.Text))
            {
                MessageBox.Show("Seleccione un préstamo de la tabla (a la derecha) para devolver.");
                return;
            }

            try
            {
                if (datos.RegistrarDevolucion(int.Parse(txtIdPrestamo.Text)))
                {
                    MessageBox.Show("Devolución registrada exitosamente.");
                    CargarDatos();
                    txtIdPrestamo.Clear();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}