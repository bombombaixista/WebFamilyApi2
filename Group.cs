namespace ApiPostgre.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // relação muitos-para-muitos com usuários
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
