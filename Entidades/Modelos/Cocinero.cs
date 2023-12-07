using Entidades.DataBase;
using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;

namespace Entidades.Modelos
{
    /// <summary>
    /// Declaracion de delgado demora
    /// </summary>
    /// <param name="demora">Recibe el tiempo de demora</param>
    public delegate void DelegadoDemoraAtencion(double demora);
    
    /// <summary>
    /// Declaracion del delegado nuevo ingreso
    /// </summary>
    /// <param name="menu">Recibe un menu</param>
    public delegate void DelegadoPedidoEnCurso(IComestible menu);

    public class Cocinero<T> where T : IComestible, new()
    {
        /// <summary>
        /// Declaracion de eventos
        /// </summary>
        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoPedidoEnCurso OnPedido;

        private CancellationTokenSource cancellation;

        private int cantPedidosFinalizados;
        private double demoraPreparacionTotal;
        
        private string nombre;

        private Task tarea;
        private T pedidoEnPreparacion;

        private Mozo<T> mozo;
        private Queue<T> pedidos;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nombre">Recibe el nombre del cocinero</param>
        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            this.mozo = new Mozo<T>();
            this.pedidos = new Queue<T>();

            this.mozo.OnPedido += this.TomarNuevoPedido;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                                                  this.tarea.Status == TaskStatus.WaitingToRun ||
                                                  this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    
                    this.mozo.EmpezarATrabajar = true;
                    this.EmpezarACocinar();
                }
                else
                {
                    this.cancellation.Cancel();
                    this.mozo.EmpezarATrabajar = !this.mozo.EmpezarATrabajar;
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }

        public string Nombre { get => nombre; }

        public int CantPedidosFinalizados { get => this.cantPedidosFinalizados; }

        public Queue<T> Pedidos { get => this.pedidos;}

        /// <summary>
        /// Metodo para iniciar un nuevo ingreso
        /// </summary>
        /// <exception cref="DataBaseManagerException">Se genera una excepcion al gurardar el ticket</exception>
        private void EmpezarACocinar()
        {
            CancellationToken token = this.cancellation.Token;

            this.tarea = Task.Run(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    this.EsperarProximoIngreso();
                    this.cantPedidosFinalizados++;

                    try
                    {
                        DataBaseManager.GuardarTicket(this.Nombre, this.pedidoEnPreparacion);
                    }
                    catch (DataBaseManagerException ex)
                    {
                        FileManager.Guardar(ex.Message, "logs.txt", true);
                        throw new DataBaseManagerException("Error al guardar el ticket", ex.InnerException);
                    }
                }
            }, token);
        }

        /// <summary>
        /// Metodo para esperar un nuevo ingreso, durmiendo el hilo principal por 1s
        /// </summary>
        private void EsperarProximoIngreso()
        {
            int tiempoEspera = 0;

            while (this.OnDemora is not null && !this.pedidoEnPreparacion.Estado && !this.cancellation.IsCancellationRequested)
            {
                tiempoEspera++;
                this.OnDemora.Invoke(tiempoEspera);
                Thread.Sleep(1000);
            }

            this.demoraPreparacionTotal += tiempoEspera;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="menu"></param>
        public void TomarNuevoPedido(T menu)
        {
            if (this.OnPedido is not null && menu is not null)
            {
                this.pedidos.Enqueue(menu);
            }
        }
    }
}
