#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWebApi.Models;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly UserDbContext _context;

        public PeopleController(UserDbContext context)
        {
            _context = context;
        }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.Persons.Include(a => a.Kids).ToListAsync();
        }
        
        // Post: api/People
        [HttpPost]
        public async Task<IActionResult> postFile(IFormFile file)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files/Uploaded");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileInfo fileInfo = new FileInfo(file.FileName);
            string fileName = file.FileName + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using(var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
              file.CopyTo(stream);
            }
            return Ok("File Uploaded Successfully");
        }


        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            var person = await _context.Persons.Include(a => a.Kids).FirstOrDefaultAsync(a => a.Id == id);

            if (person == null)
            {
                return NotFound();
            }
            return person;
        }

        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.Id)
            {
                return BadRequest();
            }
            //_context.Entry(person).State = EntityState.Modified;
            var parentObj = _context.Persons.Where(a => a.Id == person.Id)
                .Include(b => b.Kids).FirstOrDefault();
            if (parentObj != null)
            {
                _context.Entry(parentObj).CurrentValues.SetValues(person);
            }
            foreach(var childObj in parentObj.Kids.ToList())
            {
                // delete old those not in array child
                if (!person.Kids.Any(a => a.Id == childObj.Id))
                {
                    _context.Kids.Remove(childObj);
                }
                // update existing and insert
                foreach(var childModel in person.Kids)
                {
                    var existingChild = parentObj.Kids.Where(a => a.Id == childModel.Id 
                    && a.Id != default(int)).SingleOrDefault();
                    
                    if (existingChild != null)
                    {
                        _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                    }
                    else
                    {
                        var newChildModel = new Kid
                        {
                            KidName = childModel.KidName
                        };
                        parentObj.Kids.Add(newChildModel);
                    }
                }
            }
            await _context.SaveChangesAsync();
            return Ok(parentObj);
            //return NoContent();
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            var person = _context.Persons.Where(x => x.Id == id)
                .Include(a => a.Kids).FirstOrDefault();
            if (person == null)
            {
                return NotFound();
            }
            foreach (var child in person.Kids)
            {
                _context.Kids.Remove(child);
            }
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return Ok("Successfully Deleted ID : " + id);
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}
