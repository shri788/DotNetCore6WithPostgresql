using System.Text.Json.Serialization;

namespace TestWebApi.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //[JsonIgnore]
        public virtual IList<Grade>? Grades { get; set; }
    }

    public class Grade
    {
        public int GradeId { get; set; }
        public string? GradeName { get; set; }
        public string? Section { get; set; }

        public Student? Student { get; set; }

    }
}
