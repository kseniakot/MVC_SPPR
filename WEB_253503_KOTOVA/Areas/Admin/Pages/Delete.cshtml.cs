﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.UI.Services.ProductService;

namespace WEB_253503_KOTOVA.UI.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _context;

        public DeleteModel(IProductService context)
        {
            _context = context;
        }

        [BindProperty]
        public Dish Dish { get; set; } = default!;


        // Отображение блюда, которое будет удалено
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _context.GetProductByIdAsync(id.Value);

            if (!response.Successfull || response.Data == null)
            {
                return NotFound();
            }

            Dish = response.Data;
            return Page();
        }

        // Удаление блюда при подтверждении
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _context.DeleteProductAsync(id.Value);
            if (!response.Successfull)
            {
                ModelState.AddModelError("", "Не удалось удалить блюдо: " + response.ErrorMessage);
                return Page();
            }

            return RedirectToPage("./Index"); // Перенаправление на список после удаления
        }


    }
}
