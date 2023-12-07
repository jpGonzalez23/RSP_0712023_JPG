using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;
using Entidades.DataBase;

namespace Entidades.Modelos
{
    public class Hamburguesa : IComestible
    {

        private static int costoBase;
        private bool esDoble;
        private double costo;
        private bool estado;
        private string imagen;
        
        List<EIngrediente> ingredientes;
        
        Random random;

        /// <summary>
        /// Constructor estatico
        /// </summary>
        static Hamburguesa() => Hamburguesa.costoBase = 1500;

        /// <summary>
        /// Constructor sin parametros
        /// </summary>
        public Hamburguesa() : this(false) { }

        /// <summary>
        /// Sobrecarga del constructor
        /// </summary>
        /// <param name="esDoble">Recibe si la hamburguesa esDoble</param>
        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
            this.ingredientes = new List<EIngrediente>();
        }

        /// <summary>
        /// Propiedad para mostrar el total a pagar
        /// </summary>
        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";

        /// <summary>
        /// Propiedad para mostrar el estado
        /// </summary>
        public bool Estado => this.estado;

        /// <summary>
        /// Propiedad para mostrar la imagen
        /// </summary>
        public string Imagen => this.imagen; 

        /// <summary>
        /// Motodo para agregar un ingrediente aleatoriamente 
        /// </summary>
        private void AgregarIngredientes()
        {
            this.ingredientes = this.random.IngredientesAleatorios();
        }

        /// <summary>
        /// Meotodo para mostrar la hamburguesa
        /// </summary>
        /// <returns>Retoran una cadena de strings</returns>
        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            
            return stringBuilder.ToString();

        }

        /// <summary>
        /// Polimorfismo del ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.MostrarDatos();

        /// <summary>
        /// Metodo para finalizar la preparacion
        /// </summary>
        /// <param name="cocinero">Recibe el nombre del cocinero</param>
        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = this.ingredientes.CalcularCostoIngredientes(Hamburguesa.costoBase);
            this.estado = !this.Estado;
        }

        /// <summary>
        /// Metodo para inciar la preparacion
        /// </summary>
        public void IniciarPreparacion()
        {
            if (!this.estado)
            {
                int indice = this.random.Next(1, 9);

                try
                {
                    this.imagen = DataBaseManager.GetImagenComida($"Hamburguesa_{indice}");
                    this.AgregarIngredientes();
                }
                catch (DataBaseManagerException ex)
                {
                    FileManager.Guardar(ex.Message, "logs.txt", true);
                    throw new DataBaseManagerException(ex.Message);
                }
            }
        }
    }
}