using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Educationservices.Data;
using Educationservices.Models;

namespace Educationservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolClassesController : ControllerBase
    {
        private readonly AppDataContext _context;

        public SchoolClassesController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolClass>>> GetClasses()
        {
            return await _context.SchoolClasses.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<SchoolClass>> CreateClass(SchoolClass newClass)
        {
            _context.Add(newClass);
            await _context.SaveChangesAsync();
            return Ok(newClass);
        }
    }
}
