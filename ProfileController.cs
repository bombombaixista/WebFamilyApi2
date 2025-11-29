using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ApiPostgre.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiPostgre.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize] // 🔹 exige JWT válido
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("me")]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Token inválido");

            var user = _context.Users
                .Include(u => u.Groups)
                .FirstOrDefault(u => u.Id == int.Parse(userId));

            if (user == null)
                return NotFound("Usuário não encontrado");

            return Ok(new
            {
                Message = "Usuário autenticado com sucesso",
                UserId = user.Id,
                Groups = user.Groups.Select(g => new { g.Id }).ToList()
            });
        }
    }
}
