﻿#nullable disable
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
                foreach(var childModel in parentObj.Kids)
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
            //_context.SaveChanges();
            try
            {
                await _context.SaveChangesAsync();
                return Ok(parentObj);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}