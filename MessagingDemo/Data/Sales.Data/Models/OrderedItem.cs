﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Data.Models
{
  public class OrderedItem
  {
    public Guid Id { get; set; }
    public Product Product { get; set; }
  }
}
