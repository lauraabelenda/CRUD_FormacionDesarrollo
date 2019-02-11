using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationPrueba.Models.ViewModels
{
    public class ListUsuarioViewModel : Controller
    {
        public long Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        
        [RegularExpression(@"[9|8|7|6][0-9]{8}$", ErrorMessage = "Numero de teléfono incorrecto")]
        public string Telefono { get; set; }
        [Display(Name = "Código Postal")]
        [Required]
        public int Cp { get; set; }
        [Display(Name = "Código departamento")]
        [Required]
        public long CodDepartamento { get; set; }
        
    }
          
}