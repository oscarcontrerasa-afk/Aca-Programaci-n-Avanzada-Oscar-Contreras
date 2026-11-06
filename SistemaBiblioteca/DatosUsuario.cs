using SistemaBiblioteca.Datos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SistemaBiblioteca
{
    public class DatosUsuario
    {
        Conexion con = new Conexion();

        public DataTable MostrarUsuarios()
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Usuarios", sqlCon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public bool InsertarUsuario(Usuario obj)
        {
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                string query = "INSERT INTO Usuarios (Nombre, Apellido, Documento, Telefono, Correo) VALUES (@Nom, @Ape, @Doc, @Tel, @Cor)";
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@Nom", obj.Nombre);
                cmd.Parameters.AddWithValue("@Ape", obj.Apellido);
                cmd.Parameters.AddWithValue("@Doc", obj.Documento);
                cmd.Parameters.AddWithValue("@Tel", obj.Telefono ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cor", obj.Correo ?? (object)DBNull.Value);
                try { sqlCon.Open(); return cmd.ExecuteNonQuery() > 0; }
                catch (Exception ex) { throw new Exception("Error al insertar usuario: " + ex.Message); }
            }
        }

        public bool ModificarUsuario(Usuario obj)
        {
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                string query = "UPDATE Usuarios SET Nombre=@Nom, Apellido=@Ape, Documento=@Doc, Telefono=@Tel, Correo=@Cor WHERE IdUsuario=@Id";
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@Id", obj.IdUsuario);
                cmd.Parameters.AddWithValue("@Nom", obj.Nombre);
                cmd.Parameters.AddWithValue("@Ape", obj.Apellido);
                cmd.Parameters.AddWithValue("@Doc", obj.Documento);
                cmd.Parameters.AddWithValue("@Tel", obj.Telefono ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cor", obj.Correo ?? (object)DBNull.Value);
                try { sqlCon.Open(); return cmd.ExecuteNonQuery() > 0; }
                catch (Exception ex) { throw new Exception("Error al modificar usuario: " + ex.Message); }
            }
        }

        public bool EliminarUsuario(int id)
        {
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Usuarios WHERE IdUsuario=@Id", sqlCon);
                cmd.Parameters.AddWithValue("@Id", id);
                try { sqlCon.Open(); return cmd.ExecuteNonQuery() > 0; }
                catch (Exception ) { throw new Exception("No se puede eliminar. Verifica que no tenga préstamos activos."); }
            }
        }
    }
}