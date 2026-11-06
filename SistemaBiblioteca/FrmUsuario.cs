using System;
using System.Windows.Forms;
using System.Drawing;

namespace SistemaBiblioteca
{
    public partial class FrmUsuario : Form
    {
        DatosUsuario datos = new DatosUsuario();

        TextBox txtId, txtNombre, txtApellido, txtDocumento, txtTelefono, txtCorreo;
        DataGridView dgvUsuarios;
        Button btnGuardar, btnActualizar, btnEliminar, btnLimpiar;

        public FrmUsuario()
        {
            InitializeComponent();
            ConfigurarInterfaz();
            CargarDatos();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Gestión de Usuarios";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            int lblX = 20, txtX = 120, y = 20, spacing = 35;

            this.Controls.Add(new Label { Text = "ID:", Left = lblX, Top = y, Width = 80 });
            txtId = new TextBox { Left = txtX, Top = y, Width = 50, ReadOnly = true };
            this.Controls.Add(txtId); y += spacing;

            this.Controls.Add(new Label { Text = "Nombre *:", Left = lblX, Top = y, Width = 80 });
            txtNombre = new TextBox { Left = txtX, Top = y, Width = 200 };
            this.Controls.Add(txtNombre); y += spacing;

            this.Controls.Add(new Label { Text = "Apellido *:", Left = lblX, Top = y, Width = 80 });
            txtApellido = new TextBox { Left = txtX, Top = y, Width = 200 };
            this.Controls.Add(txtApellido); y += spacing;

            this.Controls.Add(new Label { Text = "Documento *:", Left = lblX, Top = y, Width = 80 });
            txtDocumento = new TextBox { Left = txtX, Top = y, Width = 200 };
            this.Controls.Add(txtDocumento); y += spacing;

            this.Controls.Add(new Label { Text = "Teléfono:", Left = lblX, Top = y, Width = 80 });
            txtTelefono = new TextBox { Left = txtX, Top = y, Width = 200 };
            this.Controls.Add(txtTelefono); y += spacing;

            this.Controls.Add(new Label { Text = "Correo:", Left = lblX, Top = y, Width = 80 });
            txtCorreo = new TextBox { Left = txtX, Top = y, Width = 200 };
            this.Controls.Add(txtCorreo);

            int btnY = y + 50, btnX = 20;
            btnGuardar = new Button { Text = "Guardar", Left = btnX, Top = btnY, Width = 80 };
            btnGuardar.Click += BtnGuardar_Click;

            btnActualizar = new Button { Text = "Actualizar", Left = btnX + 90, Top = btnY, Width = 80 };
            btnActualizar.Click += BtnActualizar_Click;

            btnEliminar = new Button { Text = "Eliminar", Left = btnX + 180, Top = btnY, Width = 80 };
            btnEliminar.Click += BtnEliminar_Click;

            btnLimpiar = new Button { Text = "Limpiar", Left = btnX + 270, Top = btnY, Width = 80 };
            btnLimpiar.Click += (s, e) => LimpiarCampos();

            this.Controls.Add(btnGuardar); this.Controls.Add(btnActualizar);
            this.Controls.Add(btnEliminar); this.Controls.Add(btnLimpiar);

            dgvUsuarios = new DataGridView { Left = 350, Top = 20, Width = 400, Height = 400, ReadOnly = true, AllowUserToAddRows = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect };
            dgvUsuarios.CellClick += DgvUsuarios_CellClick;
            this.Controls.Add(dgvUsuarios);
        }

        private void CargarDatos()
        {
            dgvUsuarios.DataSource = datos.MostrarUsuarios();
        }

        private void LimpiarCampos()
        {
            txtId.Clear(); txtNombre.Clear(); txtApellido.Clear();
            txtDocumento.Clear(); txtTelefono.Clear(); txtCorreo.Clear();
        }

        private void DgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsuarios.Rows[e.RowIndex];
                txtId.Text = row.Cells["IdUsuario"].Value.ToString();
                txtNombre.Text = row.Cells["Nombre"].Value.ToString();
                txtApellido.Text = row.Cells["Apellido"].Value.ToString();
                txtDocumento.Text = row.Cells["Documento"].Value.ToString();
                txtTelefono.Text = row.Cells["Telefono"].Value.ToString();
                txtCorreo.Text = row.Cells["Correo"].Value.ToString();
            }
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                MessageBox.Show("Los campos con * son obligatorios.");
                return;
            }
            Usuario u = new Usuario { Nombre = txtNombre.Text, Apellido = txtApellido.Text, Documento = txtDocumento.Text, Telefono = txtTelefono.Text, Correo = txtCorreo.Text };
            if (datos.InsertarUsuario(u)) { MessageBox.Show("Usuario Guardado."); CargarDatos(); LimpiarCampos(); }
        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) return;
            Usuario u = new Usuario { IdUsuario = int.Parse(txtId.Text), Nombre = txtNombre.Text, Apellido = txtApellido.Text, Documento = txtDocumento.Text, Telefono = txtTelefono.Text, Correo = txtCorreo.Text };
            if (datos.ModificarUsuario(u)) { MessageBox.Show("Usuario Actualizado."); CargarDatos(); LimpiarCampos(); }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) return;
            try
            {
                if (datos.EliminarUsuario(int.Parse(txtId.Text))) { MessageBox.Show("Usuario Eliminado."); CargarDatos(); LimpiarCampos(); }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error de Integridad"); }
        }
    }
}