using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Educationservices.Data;
using Educationservices.Models;

namespace Educationservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramsController : ControllerBase
    {
        private readonly AppDataContext _context;

        public ProgramsController(AppDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProgramModel>>> GetPrograms()
        {
            return await _context.Programs.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ProgramModel>> CreateProgram(ProgramModel newProgram)
        {
            _context.Add(newProgram);
            await _context.SaveChangesAsync();
            return Ok(newProgram);
        }
    }
}
