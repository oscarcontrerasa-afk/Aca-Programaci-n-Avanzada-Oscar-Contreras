using SistemaBiblioteca.Datos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SistemaBiblioteca
{
    public class DatosLibro
    {
        Conexion con = new Conexion();

        // 1. Mostrar Libros
        public DataTable MostrarLibros()
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                string query = @"SELECT l.IdLibro, l.Titulo, ISNULL(l.AnioPublicacion, 0) AS AnioPublicacion, l.IdAutor, 
                                (a.Nombre + ' ' + a.Apellido) AS NombreAutor 
                                FROM Libros l 
                                INNER JOIN Autores a ON l.IdAutor = a.IdAutor";
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        // 2. Insertar Libro
        public bool InsertarLibro(Libro libro)
        {
            bool respuesta = false;
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                string codigoAuto = "LIB-" + DateTime.Now.ToString("HHmmss");

                string query = @"INSERT INTO Libros (Codigo, Titulo, AnioPublicacion, IdAutor, Existencias) 
                                VALUES (@Codigo, @Titulo, @AnioPublicacion, @IdAutor, 1)";
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@Codigo", codigoAuto);
                cmd.Parameters.AddWithValue("@Titulo", libro.Titulo);
                cmd.Parameters.AddWithValue("@AnioPublicacion", libro.AnioPublicacion);
                cmd.Parameters.AddWithValue("@IdAutor", libro.IdAutor);
                try
                {
                    sqlCon.Open();
                    if (cmd.ExecuteNonQuery() > 0) respuesta = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error de SQL Server: " + ex.Message);
                }
            }
            return respuesta;
        }

        // 3. Modificar Libro
        public bool ModificarLibro(Libro libro)
        {
            bool respuesta = false;
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                string query = "UPDATE Libros SET Titulo = @Titulo, AnioPublicacion = @AnioPublicacion, IdAutor = @IdAutor WHERE IdLibro = @IdLibro";
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@IdLibro", libro.IdLibro);
                cmd.Parameters.AddWithValue("@Titulo", libro.Titulo);
                cmd.Parameters.AddWithValue("@AnioPublicacion", libro.AnioPublicacion);
                cmd.Parameters.AddWithValue("@IdAutor", libro.IdAutor);
                try
                {
                    sqlCon.Open();
                    if (cmd.ExecuteNonQuery() > 0) respuesta = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error de SQL Server: " + ex.Message);
                }
            }
            return respuesta;
        }

        // 4. Eliminar Libro
        public bool EliminarLibro(int idLibro)
        {
            bool respuesta = false;
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                string query = "DELETE FROM Libros WHERE IdLibro = @IdLibro";
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@IdLibro", idLibro);
                try
                {
                    sqlCon.Open();
                    if (cmd.ExecuteNonQuery() > 0) respuesta = true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error de SQL Server: " + ex.Message);
                }
            }
            return respuesta;
        }
    }
}