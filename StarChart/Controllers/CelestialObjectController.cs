using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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
    }
}
