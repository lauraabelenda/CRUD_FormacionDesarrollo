//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationPrueba.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UsuarioProyecto
    {
        public long Id { get; set; }
        public long Cod_Usuario { get; set; }
        public long Cod_Proyecto { get; set; }
    
        public virtual Proyecto Proyecto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
