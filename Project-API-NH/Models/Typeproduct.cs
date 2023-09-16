using System;
using System.Collections.Generic;

namespace Project_API_NH.Models;

public partial class Typeproduct
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Thumnail { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Product>? Products { get; set; } = new List<Product>();
}
