using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationPrueba.Models;
using WebApplicationPrueba.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplicationPrueba.Controllers
{
    public class ProyectoController : Controller
    {
        private Formacion_DesarrolloEntities db = new Formacion_DesarrolloEntities();

        // GET: Proyecto
        public ActionResult Index()
        {
           
            return View(db.Proyecto.ToList());
        }

        // GET: Proyecto/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // GET: Proyecto/Create
        public ActionResult Create(long? id)
        {
            //var proyecto = new Proyecto();
            //proyecto.UsuarioProyecto = new List<UsuarioProyecto>();
            //List<Usuario> usersQuery = (from u in db.Usuario
            //                            select u).ToList();
            //ViewBag.Usuarios = new MultiSelectList(usersQuery, "Id", "Nombre");
            //return View();

           
            ViewBag.Usuarios = new MultiSelectList(db.Usuario, "Id", "Nombre");
            return View();
        }



        // POST: Proyecto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.Proyecto.Add(proyecto);
                db.SaveChanges();
                foreach (var user in proyecto.SelectedUsers)
                {
                    var obj = new UsuarioProyecto() {Cod_Usuario = user, Cod_Proyecto = proyecto.Id };
                    db.UsuarioProyecto.Add(obj);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proyecto);
        }

        // GET: Proyecto/Edit/5
        public ActionResult Edit(long? id)
        {

            Proyecto proyecto = db.Proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            proyecto.SelectedUsers = db.UsuarioProyecto.Where(x => x.Cod_Proyecto == proyecto.Id).Select(a => a.Cod_Usuario).ToList();
            ViewBag.Usuarios = new MultiSelectList(db.Usuario, "Id", "Nombre",proyecto.SelectedUsers);

            
            return View(proyecto);
        }

        // POST: Proyecto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyecto).State = EntityState.Modified;
                var existingUserIds =
                    db.UsuarioProyecto.Where(s => s.Cod_Proyecto == proyecto.Id).Select(s => s.Cod_Usuario);
                var usersToAdd = proyecto.SelectedUsers.Except(existingUserIds);
                foreach (var userId in usersToAdd)
                {
                    var obj = new UsuarioProyecto() { Cod_Usuario = userId, Cod_Proyecto = proyecto.Id };
                    db.UsuarioProyecto.Add(obj);
                }

                var deleteUserIds = existingUserIds.Except(proyecto.SelectedUsers);
                foreach (var userId in deleteUserIds)
                {
                    var objUserId = (from u in db.UsuarioProyecto
                                     where u.Cod_Usuario == userId && u.Cod_Proyecto == proyecto.Id
                                     select u).Single();
                    db.UsuarioProyecto.Remove(objUserId);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(proyecto);
        }

        // GET: Proyecto/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // POST: Proyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Proyecto proyecto = db.Proyecto.Find(id);
            db.Proyecto.Remove(proyecto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //private void PopulateAssignedUserData(Proyecto proyecto)
        //{
        //    var allUsers = db.Usuario;
        //    var proyectUsers = new HashSet<long>(proyecto.UsuarioProyecto.Select(c => c.Cod_Usuario));
        //    var viewModel = new List<AssignedUsersData>();
        //    foreach (var user in allUsers)
        //    {
        //        viewModel.Add(new AssignedUsersData
        //        {
        //            Id = user.Id,
        //            Nombre = user.Nombre,
        //            Assigned = proyectUsers.Contains(user.Id)
        //        });
        //    }
        //    ViewBag.Usuarios = viewModel;
        //}

        private void PopulateUserList(long? codP)
        {
            List<Usuario> usersQuery = (from u in db.Usuario
                                        join up in db.UsuarioProyecto on u.Id equals up.Cod_Usuario
                                        join p in db.Proyecto on up.Cod_Proyecto equals p.Id
                                        where p.Id == codP
                                        select u).ToList();

            ViewBag.Usuarios = new MultiSelectList(usersQuery, "Id", "Nombre");
           
        }

        //private void UpdateProyectUsers(string[] selectedUsers, Proyecto proyectToUpdate)
        //{
        //    if (selectedUsers == null)
        //    {
        //        proyectToUpdate.UsuarioProyecto = new List<UsuarioProyecto>();
        //        return;
        //    }
        //    var selectedUsersHS = new HashSet<string>(selectedUsers);
        //    var proyectUsers = new HashSet<long>(proyectToUpdate.Usuarios.Select(u => u.Id));

        //    foreach (var user in db.UsuarioProyecto)
        //    {
        //        if (selectedUsersHS.Contains(user.Id.ToString()))
        //        {
        //            if (!proyectUsers.Contains(user.Cod_Usuario))
        //            {
        //                proyectToUpdate.UsuarioProyecto.Add(user);
        //            }
        //        }
        //        else
        //        {
        //            if (proyectUsers.Contains(user.Cod_Usuario))
        //            {
        //                proyectToUpdate.UsuarioProyecto.Remove(user);
        //            }
        //        }
        //    }

            
        //}
    }
}
