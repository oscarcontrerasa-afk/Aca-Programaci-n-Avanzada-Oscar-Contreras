using SistemaBiblioteca.Datos;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SistemaBiblioteca
{
    public class DatosPrestamo
    {
        Conexion con = new Conexion();

        // Muestra el historial completo uniendo nombres de usuarios y libros
        public DataTable MostrarPrestamos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                string query = @"SELECT p.IdPrestamo, u.Documento, (u.Nombre + ' ' + u.Apellido) AS Usuario, 
                                 l.Titulo AS Libro, p.FechaPrestamo, p.FechaDevolucion, p.Estado 
                                 FROM Prestamos p 
                                 INNER JOIN Usuarios u ON p.IdUsuario = u.IdUsuario
                                 INNER JOIN Libros l ON p.IdLibro = l.IdLibro";
                SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(query, sqlCon));
                da.Fill(dt);
            }
            return dt;
        }

        // RF05: Registrar Préstamo
        public bool RegistrarPrestamo(int idUsuario, int idLibro)
        {
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                string query = "INSERT INTO Prestamos (IdUsuario, IdLibro, FechaPrestamo, Estado) VALUES (@IdU, @IdL, GETDATE(), 'Prestado')";
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@IdU", idUsuario);
                cmd.Parameters.AddWithValue("@IdL", idLibro);
                try { sqlCon.Open(); return cmd.ExecuteNonQuery() > 0; }
                catch (Exception ex) { throw new Exception("Error al registrar préstamo: " + ex.Message); }
            }
        }

        // RF06: Registrar Devolución (Actualiza fecha y estado)
        public bool RegistrarDevolucion(int idPrestamo)
        {
            using (SqlConnection sqlCon = con.ObtenerConexion())
            {
                string query = "UPDATE Prestamos SET Estado = 'Devuelto', FechaDevolucion = GETDATE() WHERE IdPrestamo = @IdP";
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@IdP", idPrestamo);
                try { sqlCon.Open(); return cmd.ExecuteNonQuery() > 0; }
                catch (Exception ex) { throw new Exception("Error al registrar devolución: " + ex.Message); }
            }
        }
    }
}