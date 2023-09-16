using System;
using System.Collections.Generic;

namespace Project_API_NH.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime Ngaytao { get; set; }

    public int UsersId { get; set; }

    public int Status { get; set; }

    public decimal? Totalmoney { get; set; }

    public virtual ICollection<Orderitem>? Orderitems { get; set; } = new List<Orderitem>();

    public virtual User? Users { get; set; } = null!;
}
