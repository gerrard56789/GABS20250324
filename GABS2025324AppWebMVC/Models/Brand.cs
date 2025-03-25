using System;
using System.Collections.Generic;

namespace GABS2025324AppWebMVC.Models;

public partial class Brand
{
    public int BrandsId { get; set; }

    public string BrandName { get; set; } = null!;

    public string? Country { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
