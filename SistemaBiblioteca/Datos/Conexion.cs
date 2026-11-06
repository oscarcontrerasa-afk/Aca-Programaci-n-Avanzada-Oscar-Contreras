using System;
using System.Data.SqlClient;
using System.Configuration;

namespace SistemaBiblioteca.Datos
{
    public class Conexion
    {
        // Método público que devolverá la conexión lista para usarse
        public SqlConnection ObtenerConexion()
        {
            // 1. Lee la cadena llamada "ConexionBD" de tu App.config
            string cadena = ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString;

            // 2. Crea el objeto de conexión de SQL Server
            SqlConnection conexion = new SqlConnection(cadena);

            // 3. Devuelve la conexión
            return conexion;
        }
    }
}