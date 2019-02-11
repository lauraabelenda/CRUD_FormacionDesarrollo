using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationPrueba.Models;
using WebApplicationPrueba.Models.ViewModels;

namespace WebApplicationPrueba.Controllers
{
    public class UsuarioController : Controller
    {
        Formacion_DesarrolloEntities db = new Formacion_DesarrolloEntities();
        // GET: Usuario
        public ActionResult Index()
        {
            List<ListUsuarioViewModel> list;
            
                list = (from u in db.Usuario
                        select new ListUsuarioViewModel
                        {
                            Id = u.Id,
                            Nombre = u.Nombre,
                            Apellidos = u.Apellidos,
                            Telefono = u.Telefono,
                            Cp = u.Cp,
                            CodDepartamento = u.CodDepartamento

                        }).ToList();

            return View(list);
        }

        public ActionResult NuevoUsuario(long? CodD) { 
      
            this.PopulateDepartmentsDropDownList(CodD);
            return View();
        }

        [HttpPost]

        public ActionResult NuevoUsuario([Bind(Include = "Id,Nombre,Apellidos,Telefono,Cp,CodDepartamento")] Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuario.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {

                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            this.PopulateDepartmentsDropDownList(usuario.CodDepartamento);
            return View(usuario);
        }

        [HttpGet]
        public ActionResult Eliminar(int Id)
        {

            try
            {
                using (Formacion_DesarrolloEntities db = new Formacion_DesarrolloEntities())
                {
                    var usu = db.Usuario.Find(Id);
                    db.Usuario.Remove(usu);
                    db.SaveChanges();
                }
                return Redirect("~/Usuario/");
            }
            catch (Exception)
            {

                throw;
            }

        }


        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario u = db.Usuario.Find(id);
            if (u == null)
            {
                return HttpNotFound();

            }

            this.PopulateDepartmentsDropDownList(u.CodDepartamento);
            return View(u);
        }

        [HttpPost, ActionName("Editar")]
        
        public ActionResult EditarRegistro(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListUsuarioViewModel model = new ListUsuarioViewModel();
            var usuarioToUpdate = db.Usuario.Find(id);
            if (TryUpdateModel(usuarioToUpdate, "", new string[] { "Id", "Nombre", "Apellidos","Teléfono","Cp","CodDepartamento" }))
            {
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (RetryLimitExceededException)
                {

                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            this.PopulateDepartmentsDropDownList(usuarioToUpdate.CodDepartamento);
            return View(usuarioToUpdate);

        }

        
        private void PopulateDepartmentsDropDownList(long? codDepartamento)
        {
            List<Departamento> departmentsQuery = (from d in db.Departamento
                                   select d).ToList();
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "Id", "Nombre",codDepartamento);
        }
     
    }
}