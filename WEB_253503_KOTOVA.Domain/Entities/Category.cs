﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253503_KOTOVA.Domain.Entities
{
    public class Category
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public ICollection<Dish> Dishes { get; set; }

        public Category()
        {
            Dishes = new List<Dish>();
        }
    }
}