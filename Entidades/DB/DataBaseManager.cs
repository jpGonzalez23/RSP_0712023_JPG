using System.Data.SqlClient;
using Entidades.Excepciones;
using Entidades.Exceptions;
using Entidades.Interfaces;

namespace Entidades.DataBase
{
    public static class DataBaseManager
    {
        private static string stringConnection;
        private static SqlConnection connection;
        
        static DataBaseManager()
        {
            DataBaseManager.stringConnection = "Server=.;Database=SP_20231201_JPG;Trusted_Connection=True;";
        }

        public static string GetImagenComida(string tipo)
        {
            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string querry = "SELECT imagen FROM comidas WHERE tipo_comida = @comida";

                    SqlCommand cmd = new SqlCommand(querry, DataBaseManager.connection);

                    cmd.Parameters.AddWithValue("comida", tipo);

                    DataBaseManager.connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return reader.GetString(0);
                    }

                    throw new ComidaInvalidaException("Comida Inexistente\n");
                }
            }
            catch (DataBaseManagerException ex)
            {
                throw new DataBaseManagerException("Error al obtener la imagen de la base de datos");
            }
        }

        public static bool GuardarTicket<T>(string nombreEmplado, T comida) where T : IComestible, new()
        {
            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string query = "INSERT INTO tickets (empleado, ticket) VALUES (@empleado, @tk)";

                    SqlCommand cmd = new SqlCommand(query, DataBaseManager.connection);

                    cmd.Parameters.AddWithValue("empleado", nombreEmplado);
                    cmd.Parameters.AddWithValue("tk", comida.Ticket);

                    DataBaseManager.connection.Open();

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (DataBaseManagerException ex)
            {
                throw new DataBaseManagerException("Error al guardar el ticket", ex.InnerException);
            }
        }
    }
}
