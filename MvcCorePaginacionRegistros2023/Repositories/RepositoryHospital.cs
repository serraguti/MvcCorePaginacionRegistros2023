using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCorePaginacionRegistros2023.Data;
using MvcCorePaginacionRegistros2023.Models;
using System.Data;

#region SQL SERVER
//CREATE VIEW V_DEPARTAMENTOS_INDIVIDUAL
//AS
//	SELECT CAST(
//	ROW_NUMBER() OVER(ORDER BY DEPT_NO) AS INT) AS POSICION,
//    ISNULL(DEPT_NO, 0) AS DEPT_NO, DNOMBRE, LOC 
//	FROM DEPT
//GO

//CREATE PROCEDURE SP_GRUPO_DEPARTAMENTOS
//(@POSICION INT)
//AS
//    SELECT DEPT_NO, DNOMBRE, LOC
//	FROM V_DEPARTAMENTOS_INDIVIDUAL 
//	WHERE POSICION >= @POSICION AND POSICION < (@POSICION + 2)
//GO

//CREATE PROCEDURE SP_GRUPO_EMPLEADOS_OFICIO
//(@POSICION INT, @OFICIO NVARCHAR(50))
//AS
//    SELECT EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO FROM
//		(SELECT CAST(
//			ROW_NUMBER() OVER(ORDER BY APELLIDO) AS INT) AS POSICION,
//            EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
//		FROM EMP
//		WHERE OFICIO = @OFICIO) AS QUERY
//	WHERE QUERY.POSICION >= @POSICION AND QUERY.POSICION < (@POSICION + 3)
//GO

//VERSION 2 CON PARAMETRO DE SALIDA
//ALTER PROCEDURE SP_GRUPO_EMPLEADOS_OFICIO
//(@POSICION INT, @OFICIO NVARCHAR(50)
//, @NUMEROREGISTROS INT OUT)
//AS
//    SELECT @NUMEROREGISTROS = COUNT(EMP_NO) FROM
//    EMP WHERE OFICIO = @OFICIO
//	SELECT EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO FROM
//		(SELECT CAST(
//			ROW_NUMBER() OVER(ORDER BY APELLIDO) AS INT) AS POSICION,
//            EMP_NO, APELLIDO, OFICIO, SALARIO, DEPT_NO
//		FROM EMP
//		WHERE OFICIO = @OFICIO) AS QUERY
//	WHERE QUERY.POSICION >= @POSICION AND QUERY.POSICION < (@POSICION + 3)
//GO


#endregion

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

        public async Task<List<VistaDepartamento>>
            GetGrupoVistaDepartamentoAsync(int posicion)
        {
            //SELECT* FROM V_DEPARTAMENTOS_INDIVIDUAL
            //WHERE POSICION >= 1 AND POSICION< 3
            var consulta = from datos in this.context.VistaDepartamentos
                           where datos.Posicion >= posicion
                           && datos.Posicion < (posicion + 2)
                           select datos;
            return await consulta.ToListAsync();
        }

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

        public async Task<List<Departamento>> 
            GetGrupoDepartamentosAsync(int posicion)
        {
            string sql = "SP_GRUPO_DEPARTAMENTOS @POSICION";
            SqlParameter pamposicion =
                new SqlParameter("@POSICION", posicion);
            var consulta =
                this.context.Departamentos.FromSqlRaw(sql, pamposicion);
            return await consulta.ToListAsync();
        }

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

        public async Task<ModelPaginarEmpleados> 
            GetEmpleadosOficioAsync
            (int posicion, string oficio)
        {
            string sql = 
                "SP_GRUPO_EMPLEADOS_OFICIO @POSICION, @OFICIO, "
                + " @NUMEROREGISTROS OUT";
            SqlParameter pamposicion =
                new SqlParameter("@POSICION", posicion);
            SqlParameter pamoficio =
                new SqlParameter("@OFICIO", oficio);
            SqlParameter pamregistros =
                new SqlParameter("@NUMEROREGISTROS", -1);
            pamregistros.Direction = ParameterDirection.Output;
            var consulta =
                this.context.Empleados.FromSqlRaw(sql
                , pamposicion, pamoficio, pamregistros);
            List<Empleado> empleados = await consulta.ToListAsync();
            int registros = (int)pamregistros.Value;
            return new ModelPaginarEmpleados
            {
                NumeroRegistros = registros, Empleados = empleados
            };
        }

        public int GetNumeroEmpleadosOficio(string oficio)
        {
            return this.context.Empleados
                .Where(z => z.Oficio == oficio).Count();
        }

        public async Task<List<Empleado>> GetGrupoEmpleadosAsync(int posicion)
        {
            string sql = "SP_GRUPO_EMPLEADOS @POSICION";
            SqlParameter pamposicion =
                new SqlParameter("@POSICION", posicion);
            var consulta =
                this.context.Empleados.FromSqlRaw(sql, pamposicion);
            return await consulta.ToListAsync();
        }

        public int GetNumeroEmpleados()
        {
            return this.context.Empleados.Count();
        }

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
