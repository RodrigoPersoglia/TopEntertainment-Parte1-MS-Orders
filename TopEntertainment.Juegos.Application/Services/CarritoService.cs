using TopEntertainment.Ordenes.AccessData.Commands;
using TopEntertainment.Ordenes.Domain.DTOs;
using TopEntertainment.Ordenes.Domain.Entities;

namespace TopEntertainment.Ordenes.Application.Services
{
    public interface ICarritoService
    {
        Carrito GetCarritoById(int id);

        void AddCarrito(int cliente);

        void addJuegoCarrito(CarritoJuegoDTO carritoDetalle);
        void modificarCantidad(int cantidad, int idProducto, int idCliente);
        void eliminarJuegoCarrito(int idCliente, int idProducto);
        CarritoCompletoDTO carritoCompleto(int idCliente);

        List<Carrito> obtenerCarrito();

    }
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _repository;

        public CarritoService(ICarritoRepository repository)
        {
            _repository = repository;
        }
        public void AddCarrito(int cliente)
        {
            var confirmacion = _repository.estaClienteIn(cliente);
            if (confirmacion != null)
            {
                throw new FormatException();
            }
            else
            {
                _repository.AddCarrito(cliente);
            }

        }
        public void stockMenos(int id)
        {
            var url = "https://localhost:7284/juegos/stockMenos";
            HttpClient juegoCliente = new HttpClient();
            var juego = juegoCliente.PutAsync($"{url}/{id}", null);

        }

        public void stockMas(int id)
        {
            var url = "https://localhost:7284/juegos/stockMas";
            HttpClient juegoCliente = new HttpClient();
            var juego = juegoCliente.PutAsync($"{url}/{id}", null);

        }

        public bool hayStock(int id)
        {
            var url = "https://localhost:7284/juegos/hayStock";
            HttpClient juegoCliente = new HttpClient();
            var juego = juegoCliente.GetAsync($"{url}/{id}");

            return true;
        }
        public void addJuegoCarrito(CarritoJuegoDTO carritoDetalle)
        {

            var juego = _repository.getCarritoPendienteById(carritoDetalle.UsuarioId);

            var comprobar = _repository.GetJuegoPorProducto(carritoDetalle.ProductoId, juego.Id);
            _repository.addJuego(juego.Id, carritoDetalle);
            stockMenos(carritoDetalle.ProductoId);

        }

        public bool hayStock(CarritoJuegoDTO juego)
        {

            return true;
        }


        public void eliminarJuegoCarrito(int idCliente, int idProducto)
        {
            var carrito = _repository.getCarritoPendienteById(idCliente);
            var juego = _repository.GetJuegoPorProducto(idProducto, carrito.Id);
            _repository.eliminarJuego(juego);
            stockMas(idProducto);
        }

        public Carrito GetCarritoById(int id)
        {
            return _repository.GetCarritoById(id);
        }

        public void modificarCantidad(int cantidad, int idProducto, int idCliente)
        {
            var carrito = _repository.getCarritoPendienteById(idCliente);
            var juego = _repository.GetJuegoPorProducto(idProducto, carrito.Id);
            _repository.modificarCantidad(juego, cantidad);
        }

        public CarritoCompletoDTO carritoCompleto(int idCliente)
        {
            var carrito = _repository.getCarritoPendienteById(idCliente);
            if (carrito != null)
            {
                var juego = _repository.tenerJuegoCarrito(carrito.Id);
                List<JuegoCompletoDTO> juegoTotal = new List<JuegoCompletoDTO>();
                foreach (JuegoCarrito iterador in juego)
                {
                    JuegoCompletoDTO juegoAgregar = new JuegoCompletoDTO() { ProductoId = iterador.ProductoId, Cantidad = iterador.Cantidad };
                    juegoTotal.Add(juegoAgregar);
                }
                CarritoCompletoDTO carritoCompleto = new CarritoCompletoDTO() { Id = carrito.Id, UsuarioId = carrito.UsuarioId, EstadoID = carrito.EstadoID, Juegos = juegoTotal };
                return carritoCompleto;
            }
            else { AddCarrito(idCliente); return carritoCompleto(idCliente); }
        }

        public List<Carrito> obtenerCarrito()
        {
            return _repository.tenerTodosLosCarritos();
        }

    }
}
