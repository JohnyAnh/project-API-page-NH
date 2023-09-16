using System;
using System.Collections.Generic;

namespace Project_API_NH.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Tel { get; set; } = null!;

    public string? Address { get; set; }

    public string? Thumbnail { get; set; }

    public string? RoleTitle { get; set; }

    public string? JobTitle { get; set; }

    public string Password { get; set; } = null!;

    public int Status { get; set; }

    public virtual ICollection<Orderitem>? Orderitems { get; set; } = new List<Orderitem>();

    public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();
}
