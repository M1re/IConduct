namespace IConduct.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ManagerID { get; set; }
        public bool Enable { get; set; }
        public List<Employee> Subordinates { get; set; } = new();
    }
}