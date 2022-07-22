using AutoMapper;
using TopEntertainment.Ordenes.AccessData.Commands;
using TopEntertainment.Ordenes.Domain.DTOs;
using TopEntertainment.Ordenes.Domain.Entities;

namespace TopEntertainment.Ordenes.Application.Services
{
    public interface ICompraService
    {
        CompraOnView2DTO GetCompraById(int id);
        Compra GetCompraByImport(int import);

        List<CompraOnView2DTO> GetAllCompras();
        Compra Add(CompraOnCreateDTO Compra);

        void Update(CompraOnView2DTO Compra);
    }
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _repository;
        private readonly ICarritoRepository _carritoRepository;
        private readonly IMapper _mapper;
        public CompraService(ICompraRepository repository, IMapper mapper, ICarritoRepository carrito
            )
        {
            _repository = repository;
            _mapper = mapper;
            _carritoRepository = carrito;
        }

        public Compra Add(CompraOnCreateDTO compra)
        {
            try
            {
                var carrito = _carritoRepository.getCarritoPendienteById(compra.UsuarioId);
                var game = _carritoRepository.GetCarritoPorID(compra.UsuarioId);
                carrito.EstadoID = 2;
                _carritoRepository.Update(carrito);
                Compra compraPrueba = new Compra() { CarritoId = carrito.Id, UsuarioId = compra.UsuarioId, FechaHora = compra.FechaHora, ImporteFinal = compra.Importe };
                var compraMap = _mapper.Map<Compra>(compraPrueba);
                _repository.Add(compraMap);
                return compraMap;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<CompraOnView2DTO> GetAllCompras()
        {
            List<Compra> test = _repository.GetAllCompras();

            List<CompraOnView2DTO> compraMapeada = new List<CompraOnView2DTO>();


            foreach (var compra in test)
            {
                var carrito = _carritoRepository.getCarritoIndividual(compra.CarritoId);
                var juego = _carritoRepository.tenerJuegoCarrito(carrito.Id);
                var clienteMappeado = _mapper.Map<CompraOnView2DTO>(compra);


                foreach (JuegoCarrito iterador in juego)
                {
                    string juegoAgregar = ("ProductoId :" + iterador.ProductoId + "  Cantidad : " + +iterador.Cantidad);
                    clienteMappeado.Comprobante.Add(juegoAgregar);
                }

                compraMapeada.Add(clienteMappeado);
            }








            return compraMapeada;
        }

        public CompraOnView2DTO GetCompraById(int id)
        {
            Compra prueba = _repository.GetCompraById(id);
            //var carrito = _carritoRepository.GetCarritoById(prueba.UsuarioId);
            var carrito = _carritoRepository.getCarritoIndividual(prueba.Id);
            var juego = _carritoRepository.tenerJuegoCarrito(carrito.Id);
            var clienteMappeado = _mapper.Map<CompraOnView2DTO>(prueba);

            foreach (JuegoCarrito iterador in juego)
            {
                string juegoAgregar = ("ProductoId :" + iterador.ProductoId + "  Cantidad : " + +iterador.Cantidad);
                clienteMappeado.Comprobante.Add(juegoAgregar);
            }
            if (prueba == null)
            {
                return null;
            }

            return clienteMappeado;
        }

        public Compra GetCompraByImport(int import)
        {
            return _repository.GetCompraByImport(import);
        }

        public void Update(CompraOnView2DTO compra)
        {
            var compraMap = _mapper.Map<Compra>(compra);

            _repository.Update(compraMap);
        }
    }
}
