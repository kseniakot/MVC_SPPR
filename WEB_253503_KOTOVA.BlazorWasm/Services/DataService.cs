using System.Net.Http.Json;
using System.Text;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.WebUtilities;

namespace WEB_253503_KOTOVA.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly int _pageSize;
        private readonly IAccessTokenProvider _tokenProvider;

        public event Action DataLoaded;
        public List<Category> Categories { get; set; } = new();
        public List<Dish> Dishes { get; set; } = new();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public Category SelectedCategory { get; set; }

        public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _pageSize = int.Parse(configuration["ItemsPerPage"] ?? "3");
            _tokenProvider = tokenProvider;
        }
        private async Task<bool> AttachAccessTokenAsync()
        {
            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Value);
                return true;
            }
            return false;
        }

       

        public async Task GetProductListAsync(int pageNo = 1)
        {
            if (!await AttachAccessTokenAsync())
            {
                ErrorMessage = "Access denied. Please log in.";
                Success = false;
                return;
            }
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
