using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventosWebApi_v1.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome do evento")]
        [StringLength(80)]
        public string Titulo { get; set; }

        [Required]
        [Display(Name = "Data e Hora")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        public string Estado { get; set; }

        public int TipoId { get; set; }

        public virtual Tipo Tipo { get; set; }

        public int LocalId { get; set; }

        public virtual Local Local { get; set; }
    }
}
