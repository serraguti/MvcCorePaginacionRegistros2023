using Microsoft.AspNetCore.Mvc;
using MvcCorePaginacionRegistros2023.Models;
using MvcCorePaginacionRegistros2023.Repositories;

namespace MvcCorePaginacionRegistros2023.Controllers
{
    public class PaginacionController : Controller
    {
        private RepositoryHospital repo;

        public PaginacionController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult>
            PaginarRegistroVistaDepartamento(int? posicion)
        {
            if (posicion == null)
            {
                posicion = 1;
            }
            int numregistros = this.repo.GetNumeroRegistrosVistaDepartamentos();
            //ESTAMOS EN LA POSICION 1, QUE TENEMOS QUE DEVOLVER A LA VISTA?
            int siguiente = posicion.Value + 1;
            if (siguiente > numregistros)
            {
                //EFECTO OPTICO
                siguiente = numregistros;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            VistaDepartamento vistaDepartamento =
                await this.repo.GetVistaDepartamentoAsync(posicion.Value);
            ViewData["ULTIMO"] = numregistros;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            return View(vistaDepartamento);
        }
    }
}
