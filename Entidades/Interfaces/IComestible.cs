namespace Entidades.Interfaces
{
    public interface IComestible
    {
        /// <summary>
        /// Propiedades
        /// </summary>
        bool Estado { get; }

        /// <summary>
        /// Propiedad
        /// </summary>
        string Imagen {  get; }

        /// <summary>
        /// prppiedad
        /// </summary>
        string Ticket { get; }

        /// <summary>
        /// Metodo para finalizar preparacion
        /// </summary>
        /// <param name="cocinero"></param>
        void FinalizarPreparacion(string cocinero);

        /// <summary>
        /// Metodo para inciar preparacion
        /// </summary>
        void IniciarPreparacion();
    }
}
