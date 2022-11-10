using Microsoft.AspNetCore.Mvc;
using WebApiGatos.Entidades;
using Microsoft.EntityFrameworkCore;


namespace WebApiGatos.Controllers
{
    [ApiController]
    [Route("api/gatos")]
    public class GatosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger<GatosController> log;

        public GatosController(ApplicationDbContext dbContext, ILogger<GatosController> log)
        {
            this.dbContext = dbContext;
            this.log = log;
        }

        [HttpGet]
        public async Task<ActionResult<List<Gatos>>> GetAll()
        {
            log.LogInformation("Obteniendo listado");
            return await dbContext.Gatos.ToListAsync();
        }



        [HttpGet("{nombre}")]
        public async Task<ActionResult<Gatos>> GetGatos(String nombre)
        {
            var gatos = await dbContext.Gatos.FirstOrDefaultAsync(x => x.Nombre == nombre);

            if(gatos == null)
            {
                return NotFound();
            }

            log.LogInformation("El gato es " + nombre);
            return gatos;
        }
        

        [HttpPost] 
        public async Task<ActionResult> Post([FromBody] Gatos gatos)
        {
            var exist = await dbContext.Gatos.AnyAsync(x => x.Id == gatos.DueñoId);

            if (!exist)
            {
                return BadRequest($"No existe dueño relacionado al id");
            }

            dbContext.Add(gatos);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Gatos gatos, int id)
        {
            var exist = await dbContext.Gatos.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("Gato no encontrado");
            }

            if(gatos.Id != id)
            {
                return BadRequest("Gato sin id coincidente");
            }
            dbContext.Update(gatos);
            dbContext.Update(gatos);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await dbContext.Gatos.AnyAsync(x => x.Id == id);

            if (!exist)
            {
                return NotFound("Registro no encontrado");
            }

            dbContext.Remove(new Gatos() { Id = id });

            await dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
