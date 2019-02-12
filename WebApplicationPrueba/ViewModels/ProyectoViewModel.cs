using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationPrueba.Models;

namespace WebApplicationPrueba.ViewModels
{
    public class ProyectoViewModel
    {

        public Proyecto Proyecto { get; set; }
        public long Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<System.DateTime> FechaInicio { get; set; }
        public Nullable<System.DateTime> FechaFin { get; set; }
        public virtual ICollection<UsuarioProyecto> UsuarioProyecto { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }

        [Display(Name = "Lista de Usuarios")]
        public ICollection<long> SelectedUsers { get; set; }
        


    }
}