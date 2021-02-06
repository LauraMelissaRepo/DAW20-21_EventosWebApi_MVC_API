using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventosWebApi_v1.Models
{
    public class Local
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome do local ou espaço")]
        [StringLength(80)]
        public string NomeLocal { get; set; }

        [Required]
        [StringLength(80)]
        public string Morada { get; set; }

        [Required]
        [StringLength(80)]
        public string Localidade { get; set; }
    }
}
