﻿using Entidades.Enumerados;
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
        }

        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";

        bool IComestible.Estado => this.estado;

        public string Imagen => this.imagen; 


        private void AgregarIngredientes()
        {
            this.ingredientes = this.random.IngredientesAleatorios();
        }

        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            
            return stringBuilder.ToString();

        }



        public override string ToString() => this.MostrarDatos();

        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = this.ingredientes.CalcularCostoIngredientes(Hamburguesa.costoBase);
            this.estado = !this.estado;
        }

        public void IniciarPreparacion()
        {
            if (!this.estado)
            {
                int indice = this.random.Next(1, 9);

                try
                {
                    this.imagen = DataBaseManager.GetImagenComida($"Hamburguesa {indice}");
                    this.AgregarIngredientes();
                }
                catch (DataBaseManagerException ex)
                {
                    throw new DataBaseManagerException(ex.Message);
                }
            }
        }
    }
}