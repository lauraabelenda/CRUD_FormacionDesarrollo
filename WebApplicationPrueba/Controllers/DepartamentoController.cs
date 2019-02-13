using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationPrueba.Models;
using WebApplicationPrueba.Controllers;
using System.Net;
using System.Data.Entity.Infrastructure;
using WebApplicationPrueba.ViewModels;


namespace WebApplicationPrueba.Controllers
{
    public class DepartamentoController : Controller
    {

        Formacion_DesarrolloEntities db = new Formacion_DesarrolloEntities();
        // GET: Departamento
        public ActionResult Index()
        {
            List<ListDepartamentoViewModel> list;
            
                 list = (from d in db.Departamento
                            select new ListDepartamentoViewModel
                            {
                                Id = d.Id,
                                Nombre = d.Nombre,
                                Descripcion = d.Descripcion
                            }).ToList();
            
            return View(list);
        }

        public ActionResult NuevoRegistro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NuevoRegistro(DepartamentoViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (Formacion_DesarrolloEntities db = new Formacion_DesarrolloEntities())
                    {
                        var dep = new Departamento();
                        dep.Nombre = model.Nombre;
                        dep.Descripcion = model.Descripcion;

                        db.Departamento.Add(dep);
                        db.SaveChanges();
                        
                    }
                    return Redirect("~/Departamento/");
                }
                return View(model);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        [HttpGet]
        public ActionResult Eliminar(int Id)
        {

            try
            {
                using (Formacion_DesarrolloEntities db = new Formacion_DesarrolloEntities())
                {
                    var dep = db.Departamento.Find(Id);
                    db.Departamento.Remove(dep);
                    db.SaveChanges();
                }
                return Redirect("~/Departamento/");
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        
        public ActionResult EditarRegistro(int? id)
        {
            Departamento dep = db.Departamento.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (dep == null)
            {
                return HttpNotFound();
            }
            this.PopulateDepartmentsDropDownList(dep.Id);
            return View(dep);
        }

        [HttpPost, ActionName("EditarRegistro")]
        
        public ActionResult Editar(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepartamentoViewModel model = new DepartamentoViewModel();
            var departementoUpdate = db.Departamento.Find(Id);
            if (TryUpdateModel(departementoUpdate, "", new string[] { "Id", "Nombre", "Descripcion" }))
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
            PopulateDepartmentsDropDownList(departementoUpdate.Id);
            return View(departementoUpdate);

        }

        private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in db.Departamento
                                   orderby d.Nombre
                                   select d;
            ViewBag.DepartmentID = new SelectList(departmentsQuery, "Id", "Nombre", selectedDepartment);
        }

    }
}