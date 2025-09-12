namespace training_catalog_api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public ICollection<Training> Trainings { get; set; } = new List<Training>();
    }
}