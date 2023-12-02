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
            DataBaseManager.stringConnection = "Server=.;Database=20230622SP;Trusted_Connection=True;";
        }

        public static string GetImagenComida(string tipo)
        {
            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string querry = "SELEC imaen FROM comidas WHERE tipo_comida = @comida";

                    SqlCommand cmd = new SqlCommand(querry);

                    cmd.Parameters.AddWithValue("comida", tipo);

                    DataBaseManager.connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return reader.GetString(2);
                    }

                    throw new ComidaInvalidaExeption("Comida inexistente");
                }
            }
            catch (DataBaseManagerException ex)
            {

                throw new DataBaseManagerException("Error al leer la base de dato.", ex.InnerException);
            }
        }

        public static bool GuardarTicket<T>(string nombreEmplado, T comida) where T : IComestible, new()
        {
            try
            {
                using (DataBaseManager.connection = new SqlConnection(DataBaseManager.stringConnection))
                {
                    string querry = "INSERT INTO tickests (empleado, ticket) VALUES (@empleados, @tk)";

                    SqlCommand cmd = new SqlCommand(querry, DataBaseManager.connection);

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
