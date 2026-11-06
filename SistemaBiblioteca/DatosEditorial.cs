using SistemaBiblioteca.Datos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SistemaBiblioteca
{
    public class DatosEditorial
    {
        Conexion con = new Conexion();

        public DataTable MostrarEditoriales()
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("SELECT * FROM Editoriales", sqlCon));
                da.Fill(dt);
            }
            return dt;
        }

        public bool InsertarEditorial(Editorial obj)
        {
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Editoriales (Nombre, Telefono) VALUES (@Nom, @Tel)", sqlCon);
                cmd.Parameters.AddWithValue("@Nom", obj.Nombre);
                cmd.Parameters.AddWithValue("@Tel", obj.Telefono ?? (object)DBNull.Value);
                try { sqlCon.Open(); return cmd.ExecuteNonQuery() > 0; }
                catch (Exception ex) { throw new Exception("Error: " + ex.Message); }
            }
        }

        public bool EliminarEditorial(int id)
        {
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Editoriales WHERE IdEditorial=@Id", sqlCon);
                cmd.Parameters.AddWithValue("@Id", id);
                try { sqlCon.Open(); return cmd.ExecuteNonQuery() > 0; }
                catch { throw new Exception("No se puede eliminar porque hay libros asociados a esta editorial."); }
            }
        }
    }
}