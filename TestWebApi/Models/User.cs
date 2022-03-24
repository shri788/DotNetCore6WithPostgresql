using System.ComponentModel.DataAnnotations;

namespace TestWebApi.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }

        public string? name { get; set; }
    }
}
