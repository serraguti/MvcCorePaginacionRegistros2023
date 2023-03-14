using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros2023.Models;

namespace MvcCorePaginacionRegistros2023.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext
            (DbContextOptions<HospitalContext> options)
            : base(options) { }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<VistaDepartamento> VistaDepartamentos { get; set; }
    }
}
