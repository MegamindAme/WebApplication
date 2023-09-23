using System;
using System.Collections.Generic;

namespace WebApplication4.Models;

public partial class Task
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int? Assignee { get; set; }


    public DateTime? Duedate { get; set; }

    public virtual User? AssigneeNavigation { get; set; }
}
