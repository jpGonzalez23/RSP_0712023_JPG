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

        static Hamburguesa() => Hamburguesa.costoBase = 1500;

        public Hamburguesa() : this(false) { }

        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
            this.ingredientes = new List<EIngrediente>();
        }

        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";

        //bool IComestible.Estado => this.estado;

        public bool Estado => this.estado;

        public string Imagen => this.imagen; 

        /// <summary>
        /// 
        /// </summary>
        private void AgregarIngredientes()
        {
            this.ingredientes = this.random.IngredientesAleatorios();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            
            return stringBuilder.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.MostrarDatos();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cocinero"></param>
        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = this.ingredientes.CalcularCostoIngredientes(Hamburguesa.costoBase);
            this.estado = !this.Estado;
        }

        /// <summary>
        /// 
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
                    throw new DataBaseManagerException(ex.Message);
                    FileManager.Guardar(ex.Message, "logs.txt", true);
                }
            }
        }
    }
}