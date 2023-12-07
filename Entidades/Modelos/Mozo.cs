using Entidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public delegate void DelegadoNuevoIngreso<T>(T menu);

    public class Mozo<T> where T : IComestible, new()
    {
        public event DelegadoNuevoIngreso<T> OnPedido;

        private CancellationTokenSource cancellation;
        private T menu;
        private Task tarea;

        public bool EmpezarATrabajar
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                                                  this.tarea.Status == TaskStatus.WaitingToRun ||
                                                  this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.EmpezarATrabajar)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.TomarPedios();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        private void TomarPedios()
        {
            CancellationToken token = this.cancellation.Token;

            this.tarea = Task.Run(() =>
            {
                while (!this.cancellation.IsCancellationRequested)
                {
                    this.NotificarNuevoPedido();
                    Thread.Sleep(5000);
                }
            }, token) ;
        }

        public void NotificarNuevoPedido()
        {
            if (this.OnPedido is not null)
            {
                this.menu = new T();
                this.menu.IniciarPreparacion();
                this.OnPedido.Invoke(this.menu);
            }
        }
    }
}
