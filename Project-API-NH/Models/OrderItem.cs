using System;
using System.Collections.Generic;

namespace Project_API_NH.Models;

public partial class Orderitem
{
    public int Id { get; set; }

    public int Qty { get; set; }

    public int Status { get; set; }

    public int ProductId { get; set; }

    public int UserId { get; set; }

    public int? OrderId { get; set; }

    public decimal? Totalmoney { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
