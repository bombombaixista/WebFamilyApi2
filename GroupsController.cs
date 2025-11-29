using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiPostgre.Data;
using ApiPostgre.Models;

namespace ApiPostgre.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize] // 🔹 exige JWT válido
    public class GroupsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GroupsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetGroups()
        {
            return Ok(_context.Groups.ToList());
        }

        [HttpPost]
        public IActionResult CreateGroup(Group group)
        {
            _context.Groups.Add(group);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetGroups), new { id = group.Id }, group);
        }

        [HttpPost("{groupId}/addUser/{userId}")]
        public IActionResult AddUserToGroup(int groupId, int userId)
        {
            var group = _context.Groups.Include(g => g.Users).FirstOrDefault(g => g.Id == groupId);
            var user = _context.Users.Find(userId);

            if (group == null || user == null)
                return NotFound("Group or User not found");

            group.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { message = $"User {userId} added to Group {groupId}" });
        }

        // 🔹 Novo endpoint expandido: listar usuários de um grupo + seus grupos
        [HttpGet("{groupId}/users")]
        public IActionResult GetUsersInGroup(int groupId)
        {
            var group = _context.Groups
                .Include(g => g.Users)
                .ThenInclude(u => u.Groups) // carrega também os grupos de cada usuário
                .FirstOrDefault(g => g.Id == groupId);

            if (group == null)
                return NotFound("Group not found");

            var users = group.Users.Select(u => new
            {
                u.Id,
                Groups = u.Groups.Select(gr => new { gr.Id }).ToList()
            }).ToList();

            return Ok(new
            {
                GroupId = group.Id,
                Users = users
            });
        }
    }
}
