using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.Serialization;

namespace WebApplication4.Models
{
    public partial class Task
    {
        [SwaggerSchema(ReadOnly = true)]
        public int ID { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int? Assignee { get; set; }

        public DateTime? Duedate { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public virtual User? AssigneeNavigation { get; set; }
    }
}

