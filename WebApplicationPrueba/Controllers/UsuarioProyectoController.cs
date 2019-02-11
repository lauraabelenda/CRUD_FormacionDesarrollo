using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplicationPrueba.Models;

namespace WebApplicationPrueba.Controllers
{
    public class UsuarioProyectoController : Controller
    {
        private Formacion_DesarrolloEntities db = new Formacion_DesarrolloEntities();

        // GET: UsuarioProyecto
        public ActionResult Index()
        {
            var usuarioProyecto = db.UsuarioProyecto.Include(u => u.Proyecto).Include(u => u.Usuario);
            return View(usuarioProyecto.ToList());
        }

        // GET: UsuarioProyecto/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioProyecto usuarioProyecto = db.UsuarioProyecto.Find(id);
            if (usuarioProyecto == null)
            {
                return HttpNotFound();
            }
            return View(usuarioProyecto);
        }

        // GET: UsuarioProyecto/Create
        public ActionResult Create()
        {
            ViewBag.Cod_Proyecto = new SelectList(db.Proyecto, "Id", "Nombre");
            ViewBag.Cod_Usuario = new SelectList(db.Usuario, "Id", "Nombre");
            return View();
        }

        // POST: UsuarioProyecto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cod_Usuario,Cod_Proyecto")] UsuarioProyecto usuarioProyecto)
        {
            if (ModelState.IsValid)
            {
                db.UsuarioProyecto.Add(usuarioProyecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Cod_Proyecto = new SelectList(db.Proyecto, "Id", "Nombre", usuarioProyecto.Cod_Proyecto);
            ViewBag.Cod_Usuario = new SelectList(db.Usuario, "Id", "Nombre", usuarioProyecto.Cod_Usuario);
            return View(usuarioProyecto);
        }

        // GET: UsuarioProyecto/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioProyecto usuarioProyecto = db.UsuarioProyecto.Find(id);
            if (usuarioProyecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cod_Proyecto = new SelectList(db.Proyecto, "Id", "Nombre", usuarioProyecto.Cod_Proyecto);
            ViewBag.Cod_Usuario = new SelectList(db.Usuario, "Id", "Nombre", usuarioProyecto.Cod_Usuario);
            return View(usuarioProyecto);
        }

        // POST: UsuarioProyecto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cod_Usuario,Cod_Proyecto")] UsuarioProyecto usuarioProyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usuarioProyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cod_Proyecto = new SelectList(db.Proyecto, "Id", "Nombre", usuarioProyecto.Cod_Proyecto);
            ViewBag.Cod_Usuario = new SelectList(db.Usuario, "Id", "Nombre", usuarioProyecto.Cod_Usuario);
            return View(usuarioProyecto);
        }

        // GET: UsuarioProyecto/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioProyecto usuarioProyecto = db.UsuarioProyecto.Find(id);
            if (usuarioProyecto == null)
            {
                return HttpNotFound();
            }
            return View(usuarioProyecto);
        }

        // POST: UsuarioProyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            UsuarioProyecto usuarioProyecto = db.UsuarioProyecto.Find(id);
            db.UsuarioProyecto.Remove(usuarioProyecto);
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
    }
}
