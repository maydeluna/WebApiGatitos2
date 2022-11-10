using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiGatos.Entidades;
using WebApiGatos.Services;
using WebApiGatos.Filtros;

namespace WebApiGatos.Controllers
{
    [ApiController]
    [Route("api/Dueños")]
    public class DueñosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IService service;
        private readonly ServiceTransient serviceTransient;
        private readonly ServiceScoped serviceScoped;
        private readonly ServiceSingleton serviceSingleton;
        private readonly ILogger<DueñosController> logger;
        private readonly IWebHostEnvironment env;

        private readonly string nuevosRegistros = "nuevosRegistros.txt";
        private readonly string registrosConsultados = "registrosConsultados.txt";
        public DueñosController(ApplicationDbContext context, IService service,
            ServiceTransient serviceTransient, ServiceScoped serviceScoped,
            ServiceSingleton serviceSingleton, ILogger<DueñosController> logger,
            IWebHostEnvironment env)
        {
            this.dbContext = context;
            this.service = service;
            this.serviceTransient = serviceTransient;
            this.serviceScoped = serviceScoped;
            this.serviceSingleton = serviceSingleton;
            this.logger = logger;
            this.env = env;
        }

        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(FiltroDeAccion))]
        public ActionResult ObtenerGuid()
        {
            throw new NotImplementedException();
            logger.LogInformation("Durante la ejecucion");
            return Ok(new
            {
                DueñosControllerTransient = serviceTransient.guid,
                ServiceA_Transient = service.GetTransient(),
                DueñosControllerScoped = serviceScoped.guid,
                ServiceA_Scoped = service.GetScoped(),
                DueñosControllerSingleton = serviceSingleton.guid,
                ServiceA_Singleton = service.GetSingleton()
            });
        }


        [HttpGet]
        public async Task<ActionResult<List<Dueño>>> GetAll()
        {
            return await dbContext.Dueños.Include(x => x.Gatos).ToListAsync();
        }


        [HttpGet("/Gatos")]
        public async Task<ActionResult<List<Dueño>>> GetGatos()
        {
            logger.LogInformation("Se obtiene el listado de gatos");
            logger.LogWarning("Mensaje de prueba warning");
            service.EjecutarJob();
            return await dbContext.Dueños.Include(x => x.Gatos).ToListAsync();

        }

        [HttpPost] 
        public async Task<ActionResult> Post([FromBody] Dueño dueño
            )
        {
            dbContext.Add(dueño);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Dueño dueño, int id)
        {
            var exist = await dbContext.Dueños.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound();
            }

            if(dueño.Id != id)
            {
                return BadRequest("El id no coincide");
               
            }

            dbContext.Update(dueño);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Dueños.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("Registro no encontrado");
            }

            dbContext.Remove(new Dueño() { Id = id });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
