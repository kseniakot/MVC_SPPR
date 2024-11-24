using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WEB_253503_KOTOVA.UI.Services.CategoryService;
using WEB_253503_KOTOVA.UI.Services.ProductService;
using WEB_253503_KOTOVA.Controllers;
using WEB_253503_KOTOVA.Domain.Entities;
using WEB_253503_KOTOVA.Domain.Models;

public class ProductControllerTests
{
    private readonly ICategoryService _categoryServiceMock;
    private readonly IProductService _productServiceMock;

    public ProductControllerTests()
    {
        _categoryServiceMock = Substitute.For<ICategoryService>();
        _productServiceMock = Substitute.For<IProductService>();
    }

    private ProductController CreateController()
    {
        var controller = new ProductController(_productServiceMock, _categoryServiceMock)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        return controller;
    }

    [Fact]
    public async Task Index_ReturnsAllCategories()
    {
    
        _categoryServiceMock.GetCategoryListAsync()
            .Returns(ResponseData<List<Category>>.Success(new List<Category>
            {
            new Category { Name = "Супы", NormalizedName = "soups" },
            new Category { Name = "Салаты", NormalizedName = "salads" }
            }));

        _productServiceMock.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(ResponseData<ListModel<Dish>>.Success(new ListModel<Dish>
            {
                CurrentPage = 1,
                TotalPages = 1,
                Items = new List<Dish>
                {
                new Dish { Id = 1, Name = "Dish 1" },
                new Dish { Id = 2, Name = "Dish 2" }
                }
            }));

        var controller = CreateController();

    
        var result = await controller.Index(null);
        var viewResult = Assert.IsType<ViewResult>(result);


        Assert.NotNull(viewResult.ViewData["currentCategoryName"]);
        Assert.Equal("Все", viewResult.ViewData["currentCategoryName"]);
        Assert.NotNull(viewResult.ViewData["Categories"]);
        Assert.Equal(2, ((List<Category>)viewResult.ViewData["Categories"]).Count);
    }


    [Fact]
    public async Task Index_ReturnsNotFound_WhenCategoriesFail()
    {
    
        _categoryServiceMock.GetCategoryListAsync()
            .Returns(ResponseData<List<Category>>.Error("Categories not found"));

        var controller = CreateController();

 
        var result = await controller.Index(null);

    
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Categories not found", notFoundResult.Value);
    }

    [Fact]
    public async Task Index_ReturnsNotFound_WhenProductsFail()
    {

        _categoryServiceMock.GetCategoryListAsync()
            .Returns(ResponseData<List<Category>>.Success(new List<Category>()));

        _productServiceMock.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(ResponseData<ListModel<Dish>>.Error("Products not found"));

        var controller = CreateController();

    
        var result = await controller.Index(null);

  
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Products not found", notFoundResult.Value);
    }

    [Fact]
    public async Task Index_ReturnsCorrectModel()
    {

        _categoryServiceMock.GetCategoryListAsync()
            .Returns(ResponseData<List<Category>>.Success(new List<Category>()));

        _productServiceMock.GetProductListAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(ResponseData<ListModel<Dish>>.Success(new ListModel<Dish>
            {
                CurrentPage = 1,
                TotalPages = 2,
                Items = new List<Dish> { new Dish { Id = 1, Name = "Dish 1" } }
            }));

        var controller = CreateController();

        var result = await controller.Index(null);

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ListModel<Dish>>(viewResult.Model);

        Assert.Equal(1, model.CurrentPage);
        Assert.Equal(2, model.TotalPages);
        Assert.Single(model.Items);
        Assert.Equal("Dish 1", model.Items.First().Name);
    }

}