using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Excepciones
{
    public class ComidaInvalidaException : Exception
    {
        /// <summary>
        /// Constructor de la clase ComidaInvalidaException
        /// </summary>
        /// <param name="message">Recibe un mensaje</param>
        public ComidaInvalidaException(string? message) : base(message)
        {
        }
    }
}
