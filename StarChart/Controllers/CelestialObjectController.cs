using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            try
            {
                var co = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);

                if (co == null)
                {
                    return NotFound();
                }

                co.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id).ToList();

                return Ok(co);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            try
            {
                var cos = _context.CelestialObjects.Where(o => o.Name == name);

                if (cos.Count() == 0)
                {
                    return NotFound();
                }

                foreach (var co in cos)
                {
                    co.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == co.Id).ToList();
                }                

                return Ok(cos);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var cos = _context.CelestialObjects;

                foreach (var co in cos)
                {
                    co.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == co.Id).ToList();
                }

                return Ok(cos);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            try
            {
                _context.CelestialObjects.Add(celestialObject);
                _context.SaveChanges();

                var created = _context.CelestialObjects.FirstOrDefault(o => o.Name == celestialObject.Name);

                return CreatedAtRoute("GetById", new { id = created.Id }, created);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            try
            {
                var co = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);

                if (co == null)
                {
                    return NotFound();
                }

                co.Name = celestialObject.Name;
                co.OrbitalPeriod = celestialObject.OrbitalPeriod;
                co.OrbitedObjectId = celestialObject.OrbitedObjectId;

                _context.CelestialObjects.Update(co);
                _context.SaveChanges();

                return NoContent();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            try
            {
                var co = _context.CelestialObjects.FirstOrDefault(o => o.Id == id);

                if (co == null)
                {
                    return NotFound();
                }

                co.Name = name;

                _context.CelestialObjects.Update(co);
                _context.SaveChanges();

                return NoContent();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var cos = _context.CelestialObjects.Where(o => o.Id == id || o.OrbitedObjectId == id);

                if (cos.Count() == 0)
                {
                    return NotFound();
                }

                _context.CelestialObjects.RemoveRange(cos);
                _context.SaveChanges();

                return NoContent();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
