using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationPrueba.Models.ViewModels
{
    public class DepartamentoViewModel
    {
        public long Id { get; set; }
        [Required]
        [Display(Name ="Nombre")]
        public string Nombre { get; set; }
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }
      
    }
}