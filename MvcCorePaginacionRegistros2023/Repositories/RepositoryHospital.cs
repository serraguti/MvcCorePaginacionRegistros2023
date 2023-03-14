using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros2023.Data;
using MvcCorePaginacionRegistros2023.Models;

namespace MvcCorePaginacionRegistros2023.Repositories
{
    public class RepositoryHospital
    {
        private HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        #region VISTA DEPARTAMENTOS

        public int GetNumeroRegistrosVistaDepartamentos()
        {
            return this.context.VistaDepartamentos.Count();
        }

        public async Task<VistaDepartamento> 
            GetVistaDepartamentoAsync(int posicion)
        {
            VistaDepartamento vista = await
                this.context.VistaDepartamentos
                .FirstOrDefaultAsync(x => x.Posicion == posicion);
            return vista;
        }

        #endregion

        #region DEPARTAMENTOS

        public async Task<List<Departamento>> GetDepartamentos()
        {
            return await this.context.Departamentos.ToListAsync();
        }

        public async Task<Departamento> FindDepartamento(int iddepartamento)
        {
            Departamento departamento =
                await this.context.Departamentos
                .FirstOrDefaultAsync(x => x.IdDepartamento == iddepartamento);
            return departamento;
        }

        #endregion

        #region EMPLEADOS
        public async Task<List<Empleado>> GetEmpleados()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleado(int idempleado)
        {
            Empleado empleado =
                await this.context.Empleados
                .FirstOrDefaultAsync(x => x.IdEmpleado == idempleado);
            return empleado;
        }
        #endregion
    }
}
