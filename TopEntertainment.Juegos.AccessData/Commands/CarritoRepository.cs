using TopEntertainment.Ordenes.Domain.DTOs;
using TopEntertainment.Ordenes.Domain.Entities;

namespace TopEntertainment.Ordenes.AccessData.Commands
{
    public class CarritoRepository : ICarritoRepository
    {

        private readonly OrdenesContext _context;

        public CarritoRepository(OrdenesContext context)
        {
            _context = context;
        }

        public void Add(Carrito Carrito)
        {
            _context.Add(Carrito);
            _context.SaveChanges();
        }
        public List<Carrito> tenerTodosLosCarritos()
        {
            return _context.Carritos.ToList();
        }
        public Carrito GetCarritoById(int id)
        {
            var carritoDev = (from carrito in _context.Carritos
                              where carrito.UsuarioId == id
                              select carrito).FirstOrDefault();

            return carritoDev;
        }

        public Carrito getCarritoIndividual(int id)
        {
            var carritoDev = (from carrito in _context.Carritos
                              where carrito.Id == id
                              select carrito).FirstOrDefault();

            return carritoDev;
        }

        public Carrito getCarritoPendienteById(int id)
        {
            var carritoDev = (from carrito in _context.Carritos
                              where carrito.UsuarioId == id && carrito.EstadoID == 1
                              select carrito).FirstOrDefault();

            return carritoDev;
        }
        public void Update(Carrito Carrito)
        {
            _context.Update(Carrito);
            _context.SaveChanges();
        }

        public void AddCarrito(int cliente)
        {
            Carrito carrito = new Carrito() { UsuarioId = cliente, EstadoID = 1 };
            Add(carrito);

        }

        public void addJuego(int id, CarritoJuegoDTO CarritoJuegoDTO)
        {
            JuegoCarrito juego = new JuegoCarrito() { CarritoID = id, ProductoId = CarritoJuegoDTO.ProductoId, Cantidad = CarritoJuegoDTO.Cantidad };//, Compradetalle = new CompraDetalle()};
            _context.Add(juego);
            _context.SaveChanges();
        }
        public JuegoCarrito GetJuegoPorProducto(int idProducto, int idCarricto)
        {
            var carritoJuego = (from juego in _context.JuegoCarrito
                                where juego.ProductoId == idProducto && juego.CarritoID == idCarricto
                                select juego).FirstOrDefault();

            return carritoJuego;
        }


        public JuegoCarrito GetCarritoPorID(int id)
        {

            return _context.JuegoCarrito.FirstOrDefault(c => c.Carrito.Id == id);
        }
        public void modificarCantidad(JuegoCarrito juego, int cantidad)
        {
            var juegoVar = juego;
            juegoVar.Cantidad = cantidad;
            _context.Update(juegoVar);
            _context.SaveChanges();
        }

        public void eliminarJuego(JuegoCarrito juego)
        {
            _context.Remove(juego);
            _context.SaveChanges();
        }

        public List<JuegoCarrito> tenerJuegoCarrito(int id)
        {
            var carritoJuego = (from JuegoCarrito in _context.JuegoCarrito
                                where JuegoCarrito.CarritoID == id
                                select JuegoCarrito).ToList();
            return carritoJuego;
        }

        public Carrito estaClienteIn(int IdCliente)
        {
            var carritoIn = (from carrito in _context.Carritos
                             where carrito.UsuarioId == IdCliente && carrito.EstadoID == 1
                             select carrito).FirstOrDefault();
            return carritoIn;
        }



    }
}
