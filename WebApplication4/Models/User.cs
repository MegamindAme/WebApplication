using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace WebApplication4.Models
{
    public partial class User
    {
        [SwaggerSchema(ReadOnly = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        [SwaggerSchema(ReadOnly = true)]
        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
