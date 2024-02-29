﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020020.DomainModels
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
        public string Description { get; set; } = "";
    }
}
