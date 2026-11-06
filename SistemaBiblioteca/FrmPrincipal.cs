using SistemaBiblioteca.Presentacion;
using System;
using System.Windows.Forms;

namespace SistemaBiblioteca
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
            ConfigurarInterfaz();
        }

        private void ConfigurarInterfaz()
        {
            this.Text = "Sistema de Gestión de Biblioteca - Menú Principal";
            this.Size = new System.Drawing.Size(300, 320);
            this.StartPosition = FormStartPosition.CenterScreen;

            Button btnLibros = new Button { Text = "Gestión de Libros", Top = 30, Left = 65, Width = 150, Height = 40 };
            btnLibros.Click += (s, e) => new FrmLibro().ShowDialog();

            Button btnAutores = new Button { Text = "Gestión de Autores", Top = 80, Left = 65, Width = 150, Height = 40 };
            btnAutores.Click += (s, e) => new FrmAutor().ShowDialog();

            Button btnUsuarios = new Button { Text = "Gestión de Usuarios", Top = 130, Left = 65, Width = 150, Height = 40 };
            btnUsuarios.Click += (s, e) => new FrmUsuario().ShowDialog();

            Button btnPrestamos = new Button { Text = "Préstamos y Devoluciones", Top = 180, Left = 65, Width = 150, Height = 40 };
            btnPrestamos.Click += (s, e) => new FrmPrestamo().ShowDialog();

            this.Controls.Add(btnLibros);
            this.Controls.Add(btnAutores);
            this.Controls.Add(btnUsuarios);
            this.Controls.Add(btnPrestamos);
        }
    }
}