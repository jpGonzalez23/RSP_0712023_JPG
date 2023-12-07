using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Modelos;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            string data = "Test";
            string nombreInvalido = "";

            FileManager.Guardar(data, nombreInvalido, false);
        }

        [TestMethod]
        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {
            //arrange
            Cocinero<Hamburguesa> cocinero = new Cocinero<Hamburguesa>("Pepe");

            //act
            int pedidos = cocinero.CantPedidosFinalizados;

            //assert
            Assert.AreEqual(0, pedidos);
        }
    }
}