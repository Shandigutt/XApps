﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using XApps.WebApi.Models;
using XApps.WebApi.DataContext;

namespace XApps.WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AppController : ApiController
    {
        private XAppsDataContext db = new XAppsDataContext();

        // GET api/App
        public IQueryable GetApps()
        {
            var AppRe = db.Apps.Select(i => new { i.AppID, i.AppName, i.AuthorID, i.CategoryID, i.RepoName, i.LatestHash, i.isPublished, i.description });
            return AppRe;
        }

        // GET api/App/5
        [ResponseType(typeof(App))]

        public IHttpActionResult GetApp(int id)
        {
            var AppRe = db.Apps.Select(i => new { i.AppID, i.AppName, i.AuthorID, i.CategoryID, i.RepoName, i.LatestHash, i.isPublished, i.description });
            var product = AppRe.Where((p) => p.AppID == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // PUT api/App/5
        public IHttpActionResult PutApp(int id, App app)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != app.AppID)
            {
                return BadRequest();
            }

            db.Entry(app).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/App
        [ResponseType(typeof(App))]
        public IHttpActionResult PostApp(App app)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Apps.Add(app);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = app.AppID }, app);
        }

        // DELETE api/App/5
        [ResponseType(typeof(App))]
        public IHttpActionResult DeleteApp(int id)
        {
            App app = db.Apps.Find(id);
            if (app == null)
            {
                return NotFound();
            }

            db.Apps.Remove(app);
            db.SaveChanges();

            return Ok(app);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppExists(int id)
        {
            return db.Apps.Count(e => e.AppID == id) > 0;
        }

        [ActionName("AppByAppName")]
        public IHttpActionResult GetAppByAppName(String AppName)
        {
            var AppRe = db.Apps.Select(i => new { i.AppID, i.AppName, i.AuthorID, i.CategoryID, i.RepoName, i.LatestHash, i.isPublished, i.description });
            var product = AppRe.Where((p) => p.AppName == AppName);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [ActionName("AppBycategory")]
        public IHttpActionResult GetAppBycategory(String CategoryName) 
        {
            //get category id

            var CatRe = db.Categories.Select(i => new { i.CategoryID, i.CategoryName });
            var CatPro = CatRe.FirstOrDefault((p) => p.CategoryName == CategoryName);

            var AppRe = db.Apps.Select(i => new { i.AppID, i.AppName, i.AuthorID, i.CategoryID, i.RepoName, i.LatestHash, i.isPublished, i.description });
            var product = AppRe.Where((p) => p.CategoryID == CatPro.CategoryID);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
            
        }


    }
}