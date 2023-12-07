using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades.Files
{
    
    public static class FileManager
    {
        private static string path;

        /// <summary>
        /// Constructor estatico
        /// </summary>
        static FileManager()
        {
            FileManager.path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\07122023_Gonzalez_JP\\";
            FileManager.ValidaExistenciaDeDirectorio();
        }

        /// <summary>
        /// Metodo que valida si existe el directorio, caso contrario lo crea
        /// </summary>
        /// <exception cref="FileManagerException">Genera una excepcion</exception>
        private static void ValidaExistenciaDeDirectorio()
        {
            if (!Directory.Exists(FileManager.path))
            {
                try
                {
                    Directory.CreateDirectory(FileManager.path);
                }
                catch (FileManagerException ex)
                {
                    throw new FileManagerException("¡Error al crear el directorio", ex.InnerException);
                }
            }
        }

        /// <summary>
        /// Metodo estatico para guardar archivos
        /// </summary>
        /// <param name="data">Recibe lo que se va a guardar</param>
        /// <param name="nombreArchivo">Recibe el nombre del archivo y su formato .txt .json</param>
        /// <param name="append">Recibe un true para reiscribir el archivo</param>
        /// <exception cref="FileManagerException">Se genera una excepcion</exception>
        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            try
            {
                string filePath = Path.Combine(FileManager.path, nombreArchivo);
                
                using (StreamWriter sw = new StreamWriter(filePath, append))
                {
                    sw.WriteLine(data);
                }
            }
            catch (FileManagerException ex)
            {

                throw new FileManagerException("Error al guardar un archivo", ex.InnerException);
            }
        }

        /// <summary>
        /// Metodo estatico para serializar
        /// </summary>
        /// <typeparam name="T">Es tipo Genererico</typeparam>
        /// <param name="elementos">Recibe el objeto para guardar</param>
        /// <param name="nombreArchivo">Recibe el nombre del archivo</param>
        /// <returns></returns>
        /// <exception cref="FileManagerException"></exception>
        public static bool Serializar<T>(T elementos, string nombreArchivo) where T : class
        {
            try
            {
                FileManager.Guardar(System.Text.Json.JsonSerializer.Serialize(elementos, typeof(T)), nombreArchivo, false);
                return true;
            }
            catch (FileManagerException ex)
            {

                throw new FileManagerException("Error al serializar", ex.InnerException);
            }
        }
    }
}
