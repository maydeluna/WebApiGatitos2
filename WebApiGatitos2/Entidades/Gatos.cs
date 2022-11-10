using System.ComponentModel.DataAnnotations;
using WebApiGatos.Validaciones;

namespace WebApiGatos.Entidades
{
    public class Gatos
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

       
      

        public int DueñoId { get; set; }
    }
}
