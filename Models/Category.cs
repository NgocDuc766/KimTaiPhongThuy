﻿using System;
using System.Collections.Generic;

namespace KimTaiPhongThuy.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
