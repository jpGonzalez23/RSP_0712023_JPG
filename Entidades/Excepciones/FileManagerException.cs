using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Exceptions
{
    public class FileManagerException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Recibe un mesaje</param>
        public FileManagerException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Sobrecarga del constructor
        /// </summary>
        /// <param name="message">Recibe un mensaje</param>
        /// <param name="innerException">Recibe el tipo de exception</param>
        public FileManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
