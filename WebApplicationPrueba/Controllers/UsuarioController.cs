using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationPrueba.Models;
using WebApplicationPrueba.ViewModels;

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
                        CodDepartamento = u.CodDepartamento,
                        UsuarioProyecto = u.UsuarioProyecto

                    }).ToList();

            return View(list);
        }

        public ActionResult NuevoUsuario(long? CodD) { 
      
            this.PopulateDepartmentsDropDownList(CodD);
            ViewBag.Proyectos = new MultiSelectList(db.Proyecto, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoUsuario(Usuario usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Usuario.Add(usuario);
                    db.SaveChanges();
                    foreach (var proyect in usuario.SelectedProyects)
                    {
                        var obj = new UsuarioProyecto() { Cod_Proyecto = proyect, Cod_Usuario = usuario.Id };
                        db.UsuarioProyecto.Add(obj);
                    }
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
            u.SelectedProyects = db.UsuarioProyecto.Where(x => x.Cod_Usuario == u.Id).Select(a => a.Cod_Proyecto).ToList();
            ViewBag.Proyectos = new MultiSelectList(db.Proyecto, "Id", "Nombre", u.SelectedProyects);
            this.PopulateDepartmentsDropDownList(u.CodDepartamento);

            return View(u);
        }

        [HttpPost, ActionName("Editar")]
       
        public ActionResult EditarRegistro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;

                var existingProyects =
                    db.UsuarioProyecto.Where(x => x.Cod_Usuario == usuario.Id).Select(s => s.Cod_Proyecto);

                var proyectsToAdd = usuario.SelectedProyects.Except(existingProyects);
                foreach (var pro in proyectsToAdd)
                {
                    var obj = new UsuarioProyecto() { Cod_Proyecto = pro, Cod_Usuario = usuario.Id };
                    db.UsuarioProyecto.Add(obj);
                }

                var delectedProyects = existingProyects.Except(usuario.SelectedProyects);
                foreach (var pro in delectedProyects)
                {
                    var objP = (from u in db.UsuarioProyecto
                                where u.Cod_Proyecto == pro && u.Cod_Usuario == usuario.Id
                                select u).Single();
                    db.UsuarioProyecto.Remove(objP);
                }
                db.SaveChanges();
                this.PopulateDepartmentsDropDownList(usuario.CodDepartamento);
                return RedirectToAction("Index");
            }

            return View(usuario);
        }

        [HttpGet]
        public ActionResult Eliminar(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuario usuario = db.Usuario.Find(Id);
            if (usuario == null)
            {
                return HttpNotFound();
            }
            PopulateProyectsList(usuario.Id);
            return View(usuario);


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Deleted;
                var existingProyects =
                    db.UsuarioProyecto.Where(s => s.Cod_Usuario == usuario.Id).Select(s => s.Cod_Proyecto);
                if (existingProyects != null)
                {
                    foreach (var pro in existingProyects)
                    {
                        var obj = (from u in db.UsuarioProyecto
                                         where u.Cod_Proyecto == pro && u.Cod_Usuario == usuario.Id
                                         select u).Single();
                        db.UsuarioProyecto.Remove(obj);
                    }
                }


                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        private void PopulateDepartmentsDropDownList(long? codDepartamento)
        {
            List<Departamento> departmentsQuery = (from d in db.Departamento
                                   select d).ToList();
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "Id", "Nombre",codDepartamento);
        }

        private void PopulateProyectsList(long? codU)
        {
            List<Proyecto> proyectsQuery = (from p in db.Proyecto
                                        join up in db.UsuarioProyecto on p.Id equals up.Cod_Proyecto
                                        join u in db.Usuario on up.Cod_Usuario equals u.Id
                                        where u.Id == codU
                                        select p).ToList();

            ViewBag.Proyectos = new MultiSelectList(proyectsQuery, "Id", "Nombre");

        }

    }
}