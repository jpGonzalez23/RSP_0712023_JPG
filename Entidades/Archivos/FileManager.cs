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

        static FileManager()
        {
            FileManager.path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\20231205_Gonzalez_JP\\";
            FileManager.ValidaExistenciaDeDirectorio();
        }

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

        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(FileManager.path + nombreArchivo, append))
                {
                    sw.WriteLine(data);
                }
            }
            catch (FileManagerException ex)
            {

                throw new FileManagerException("Error al guardar un archivo", ex.InnerException);
            }
        }

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
