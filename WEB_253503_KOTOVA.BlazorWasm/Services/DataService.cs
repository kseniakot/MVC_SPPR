﻿using System.Net.Http.Json;
using System.Text;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace WEB_253503_KOTOVA.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly int _pageSize;

        public event Action DataLoaded;
        public List<Category> Categories { get; set; } = new();
        public List<Dish> Dishes { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public Category SelectedCategory { get; set; }

        public DataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _pageSize = int.Parse(configuration["ItemsPerPage"] ?? "3");
        }

        public async Task GetAllProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ResponseData<List<Dish>>>("dishes/all");
                if (response?.Successfull == true)
                {
                    Dishes = response.Data ?? new List<Dish>();
                    Success = true;
                }
                else
                {
                    Dishes = new List<Dish>();
                    ErrorMessage = response?.ErrorMessage ?? "Failed to fetch dishes.";
                    Success = false;
                }
                DataLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Success = false;
            }
        }

        public async Task GetProductListAsync(int pageNo = 1)
        {
            try
            {
                var route = new StringBuilder("dishes/");
                if (SelectedCategory != null)
                {
                    route.Append($"{SelectedCategory.NormalizedName}/");
                }
                var queryData = new List<KeyValuePair<string, string>>
                {
                    KeyValuePair.Create("pageNo", pageNo.ToString()),
                    KeyValuePair.Create("pageSize", _pageSize.ToString())
                };
                var url = QueryHelpers.AddQueryString(route.ToString(), queryData);
                var response = await _httpClient.GetFromJsonAsync<ResponseData<ListModel<Dish>>>(url);
                if (response?.Successfull == true)
                {
                    Dishes = response.Data?.Items ?? new List<Dish>();
                    CurrentPage = response.Data?.CurrentPage ?? 1;
                    TotalPages = response.Data?.TotalPages ?? 1;
                    Success = true;
                }
                else
                {
                    Dishes = new List<Dish>();
                    ErrorMessage = response?.ErrorMessage ?? "Failed to fetch data.";
                    Success = false;
                }
                DataLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Success = false;
            }
        }

        public async Task GetCategoryListAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ResponseData<List<Category>>>("categories");
                if (response?.Successfull == true)
                {
                    Categories = response.Data ?? new List<Category>();
                    Success = true;
                }
                else
                {
                    Categories = new List<Category>();
                    ErrorMessage = response?.ErrorMessage ?? "Failed to fetch categories.";
                    Success = false;
                }
                DataLoaded?.Invoke();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                Success = false;
            }
        }
    }
}
