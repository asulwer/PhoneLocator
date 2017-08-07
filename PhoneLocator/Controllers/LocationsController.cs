using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PhoneLocator.Models;

namespace PhoneLocator.Controllers
{
    public class LocationsController : ApiController
    {
        private PhoneLocatorContext db = new PhoneLocatorContext();

        public IQueryable<Location> Get()
        {
            return db.Locations;
        }

        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            //signalr
            LocationHub hub = new LocationHub();
            await hub.SendNotifications(string.Empty);

            return Ok(location);
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put(Guid id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.Id)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //signalr
            LocationHub hub = new LocationHub();
            await hub.SendNotifications(string.Empty);

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> Post(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);
            await db.SaveChangesAsync();

            //signalr
            LocationHub hub = new LocationHub();
            await hub.SendNotifications(string.Empty);

            return CreatedAtRoute("DefaultApi", new { id = location.Id }, location);
        }

        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> Delete(Guid id)
        {
            Location location = await db.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            await db.SaveChangesAsync();

            //signalr
            LocationHub hub = new LocationHub();
            await hub.SendNotifications(string.Empty);

            return Ok(location);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(Guid id)
        {
            return db.Locations.Count(e => e.Id == id) > 0;
        }
    }
}