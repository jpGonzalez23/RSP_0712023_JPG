using Entidades.Enumerados;


namespace Entidades.MetodosDeExtension
{

    public static class IngredientesExtension
    {
        /// <summary>
        /// Metodo de extension para calcular el costo
        /// </summary>
        /// <param name="ingredientes">recibe una lista</param>
        /// <param name="costoInicial">recibe el costo incial</param>
        /// <returns>Retorna el precio final</returns>
        public static double CalcularCostoIngredientes(this List<EIngrediente> ingredientes, int costoInicial)
        {
            ingredientes.ForEach(ingrediente => costoInicial += (costoInicial * (int)ingrediente / 100));
            return costoInicial;
        }

        /// <summary>
        /// Metodo para agregar ingredientes de manera de aleatoria
        /// </summary>
        /// <param name="random">Recibe un objeto ramdon</param>
        /// <returns></returns>
        public static List<EIngrediente> IngredientesAleatorios(this Random random)
        {
            List<EIngrediente> ingredientes = new List<EIngrediente>()
            {
                EIngrediente.QUESO,
                EIngrediente.JAMON,
                EIngrediente.PANCETA,
                EIngrediente.HUEVO,
                EIngrediente.ADHERESO
            };

            int cant = random.Next(1, ingredientes.Count + 1);
            
            return ingredientes.Take(cant).ToList();
        }
    }
}
