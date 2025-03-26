using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GABS2025324AppWebMVC.Models;

public partial class Warehouse
{
    public int WarehouseId { get; set; }

    [Display (Name ="Nombre Bodega")]
    public string WarehouseName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
