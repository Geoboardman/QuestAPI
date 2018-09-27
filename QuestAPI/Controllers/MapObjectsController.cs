using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestAPI.Models;

namespace QuestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapObjectsController : ControllerBase
    {
        private readonly GameWorldContext _context;

        public MapObjectsController(GameWorldContext context)
        {
            _context = context;
        }

        // GET: api/MapObjects
        [HttpGet]
        public IEnumerable<MapObjects> GetMapObjects()
        {
            return _context.MapObjects;
        }

        // GET: api/MapObjects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMapObjects([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapObjects = await _context.MapObjects.FindAsync(id);

            if (mapObjects == null)
            {
                return NotFound();
            }

            return Ok(mapObjects);
        }
        
        // GET: api/MapObjects/byRegion?region=50.403,-44.403&timestamp=
        [HttpGet("byRegion")]
        public async Task<IActionResult> GetMapObjects(string region, DateTime? timestamp)
        {
            System.Diagnostics.Debugger.Break();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var objectsInRegion = _context.MapObjects
            .Where(o => o.Region == region)
            .Count();

            if(objectsInRegion <= 0)
            {
                GenerateMapObjects(region);
            }

            List<MapObjects> mapObjects;
            if (timestamp != null)
                mapObjects = await _context.MapObjects.Where(p => p.Region == region && p.TimeStamp >= timestamp).ToListAsync<MapObjects>();
            else
                mapObjects = await _context.MapObjects.Where(p => p.Region == region).ToListAsync<MapObjects>();

            if (mapObjects == null)
            {
                return NotFound();
            }

            return Ok(mapObjects);
        }
        
        // PUT: api/MapObjects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMapObjects([FromRoute] int id, [FromBody] MapObjects mapObjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mapObjects.Id)
            {
                return BadRequest();
            }

            _context.Entry(mapObjects).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapObjectsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MapObjects
        [HttpPost]
        public async Task<IActionResult> PostMapObjects([FromBody] MapObjects mapObjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MapObjects.Add(mapObjects);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMapObjects", new { id = mapObjects.Id }, mapObjects);
        }

        // DELETE: api/MapObjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMapObjects([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mapObjects = await _context.MapObjects.FindAsync(id);
            if (mapObjects == null)
            {
                return NotFound();
            }

            _context.MapObjects.Remove(mapObjects);
            await _context.SaveChangesAsync();

            return Ok(mapObjects);
        }

        private bool MapObjectsExists(int id)
        {
            return _context.MapObjects.Any(e => e.Id == id);
        }

        private void GenerateMapObjects(string region)
        {
            //Variables for generating monsters
            //Move these somehwere??
            int minMonsters = 6;
            int maxMonsters = 10;
            string[] latlon;
            Random rnd = new Random();
            MapObjects newObject;
            Monsters mon;
            int numMonsters = rnd.Next(minMonsters, maxMonsters);
            for(int i = 0; i < numMonsters; i++)
            {
                newObject = new MapObjects();
                newObject.Region = region;
                newObject.Type = "monster";
                newObject.TimeStamp = DateTime.UtcNow;
                //Choose monster type
                mon = _context.Monsters.OrderBy(r => Guid.NewGuid()).Take(1).First();
                newObject.Attributes = mon.Attributes;
                //Generate random location from region
                latlon = region.Split(',');
                newObject.Lat = Convert.ToDecimal(latlon[0]);
                newObject.Lon = Convert.ToDecimal(latlon[1]);
                newObject.Lat += Decimal.Divide(rnd.Next(1, 10), 1000) - .005m;
                newObject.Lon += Decimal.Divide(rnd.Next(1, 10), 1000) - .005m;

                _context.MapObjects.Add(newObject);
            }

            _context.SaveChanges();
        }
    }
}