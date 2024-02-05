namespace TodoApp.Models.Test
{
    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Children> childrens { get; set; } = [];
    }
}
