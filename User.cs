namespace ApiPostgre.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; }

        // Senha com hash/salt
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        // 🔹 Adicione esta propriedade
        public string? Name { get; set; }

        // Relacionamentos
        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}
