using System;
using System.Collections.Generic;

namespace Project_API_NH.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? Thumbnail { get; set; }

    public string? Brandname { get; set; }

    public int Qty { get; set; }

    public int Status { get; set; }

    public int TypeproductId { get; set; }

    public int SupplierId { get; set; }

    public virtual ICollection<Orderitem>? Orderitems { get; set; } = new List<Orderitem>();

    public virtual Supplier Supplier { get; set; } = null!;

    public virtual Typeproduct Typeproduct { get; set; } = null!;
}
