﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_KOTOVA.Domain.Entities
{
    public class Dish
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } 
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public string? MimeType { get; set; }
    }
}
