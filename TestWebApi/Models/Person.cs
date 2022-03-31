namespace TestWebApi.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Kid>? Kids { get; set; }

    }

    public class Kid
    {
        public int Id { get; set; }
        public string? KidName { get; set; }
    }
}
