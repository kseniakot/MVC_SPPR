using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEB_253503_KOTOVA.Domain.Entities;

namespace WEB_253503_KOTOVA.Domain.Models
{
    public class CartItem
    {
        public Dish Dish { get; set; }
        public int Count { get; set; } = 1;
    }
}
