using SistemaBiblioteca.Datos;
using SistemaBiblioteca.Logica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SistemaBiblioteca.Datos
{
    public class DatosAutor
    {
        public bool InsertarAutor(Autor autor)
        {
            bool respuesta = false;
            Conexion con = new Conexion(); // Instanciamos tu clase de conexión
            SqlConnection sqlCon = con.ObtenerConexion();

            try
            {
                // 1. Definimos la consulta parametrizada
                string query = "INSERT INTO Autores (Nombre, Apellido) VALUES (@Nombre, @Apellido)";
                SqlCommand comando = new SqlCommand(query, sqlCon);

                // 2. Le pasamos los valores del objeto a los parámetros de SQL
                comando.Parameters.AddWithValue("@Nombre", autor.Nombre);
                comando.Parameters.AddWithValue("@Apellido", autor.Apellido);

                // 3. Abrimos la conexión y ejecutamos
                sqlCon.Open();
                int filasAfectadas = comando.ExecuteNonQuery(); // Devuelve cuántas filas se insertaron

                if (filasAfectadas > 0)
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                // Atrapa cualquier error (ej. se cayó el servidor) para que el programa no colapse
                Console.WriteLine("Error al insertar autor: " + ex.Message);
                respuesta = false;
            }
            finally
            {
                // 4. El bloque finally asegura que la conexión SIEMPRE se cierre
                if (sqlCon.State == ConnectionState.Open)
                {
                    sqlCon.Close();
                }
            }
            return respuesta;
        }
        public DataTable MostrarAutores()
        {
            DataTable tabla = new DataTable();
            Conexion con = new Conexion();
            SqlConnection sqlCon = con.ObtenerConexion();
            try
            {
                string query = "SELECT IdAutor, Nombre, Apellido FROM Autores";
                SqlCommand comando = new SqlCommand(query, sqlCon);
                SqlDataAdapter adapter = new SqlDataAdapter(comando);
                adapter.Fill(tabla); // Llena la tabla con los resultados de la base de datos
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al mostrar autores: " + ex.Message);
            }
            return tabla;
        }
        public bool ModificarAutor(Autor autor)
        {
            bool respuesta = false;
            Conexion con = new Conexion();
            SqlConnection sqlCon = con.ObtenerConexion();
            try
            {
                string query = "UPDATE Autores SET Nombre = @Nombre, Apellido = @Apellido WHERE IdAutor = @IdAutor";
                SqlCommand comando = new SqlCommand(query, sqlCon);
                comando.Parameters.AddWithValue("@IdAutor", autor.IdAutor);
                comando.Parameters.AddWithValue("@Nombre", autor.Nombre);
                comando.Parameters.AddWithValue("@Apellido", autor.Apellido);

                sqlCon.Open();
                if (comando.ExecuteNonQuery() > 0)
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al modificar autor: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return respuesta;
        }
        public bool EliminarAutor(int idAutor)
        {
            bool respuesta = false;
            Conexion con = new Conexion();
            SqlConnection sqlCon = con.ObtenerConexion();
            try
            {
                string query = "DELETE FROM Autores WHERE IdAutor = @IdAutor";
                SqlCommand comando = new SqlCommand(query, sqlCon);
                comando.Parameters.AddWithValue("@IdAutor", idAutor);

                sqlCon.Open();
                if (comando.ExecuteNonQuery() > 0)
                {
                    respuesta = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar autor: " + ex.Message);
            }
            finally
            {
                if (sqlCon.State == ConnectionState.Open) sqlCon.Close();
            }
            return respuesta;
        }
    }
}