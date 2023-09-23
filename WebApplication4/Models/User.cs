﻿using System;
using System.Collections.Generic;

namespace WebApplication4.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
