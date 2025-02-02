namespace Institute_Management.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int BatchId { get; set; }
        public Batch Batch { get; set; }
    }
}
