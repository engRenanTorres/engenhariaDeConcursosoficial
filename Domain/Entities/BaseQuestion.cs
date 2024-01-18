using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Apllication.Repositories;

namespace Domain.Entities.Inharitance
{
    [Table("Questions")]
    public abstract class BaseQuestion : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Column(name: "Created_at")]
        public DateTime CreatedAt { get; set; }

        [Column(name: "Last_updated_at")]
        public DateTime LastUpdatedAt { get; set; }

        public string Body { get; set; } = "";

        public char Answer { get; set; }

        public string? Tip { get; set; } = "";
        public int CreatedById { get; set; }

        protected ICollection<Choice> _choices = new List<Choice>();

        /*public User? CreatedBy { get; set; }*/
        public abstract ICollection<Choice> Choices { get; set; }
    }
}
