using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models;

public partial class Task
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? Assignee { get; set; }


    public DateTime? Duedate { get; set; }

    public virtual User? AssigneeNavigation { get; set; }
}
