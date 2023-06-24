namespace PaginationSample.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Gender { get; set; }
        public decimal DebtorAmount { get; set; }
    }
}
