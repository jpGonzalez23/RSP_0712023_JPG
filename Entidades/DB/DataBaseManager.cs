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

        /// <summary>
        /// Metodo para obtener la imagen de la base de datos
        /// </summary>
        /// <param name="tipo">Recibe el tipo</param>
        /// <returns></returns>
        /// <exception cref="ComidaInvalidaException">Genera una excepcion si la comida no existe</exception>
        /// <exception cref="DataBaseManagerException">Genera una excepcion si hubo un error al conectarse a la base de datos</exception>
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

        /// <summary>
        /// Metodo para guardar los tickets
        /// </summary>
        /// <typeparam name="T">Puede ser del tipo generico</typeparam>
        /// <param name="nombreEmplado">Recibe el nombre del empleado</param>
        /// <param name="comida">Recibe el tipo de comida</param>
        /// <returns>Retorna true si se pudo guardar correctamente en la base de datos</returns>
        /// <exception cref="DataBaseManagerException">Genera una excepcion si hubo problemas en la coneccion en la base de datos</exception>
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
