using Entidades.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Exceptions
{
    public class DataBaseManagerException : Exception
    {
        /// <summary>
        /// Constructor de la clase DataBaseManagerException
        /// </summary>
        /// <param name="message">Recibe un mensaje</param>
        public DataBaseManagerException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Sobrecarga del constructor 
        /// </summary>
        /// <param name="message">Recibe un mensaje</param>
        /// <param name="innerException">Recibe el tipo de exception generada</param>
        public DataBaseManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
