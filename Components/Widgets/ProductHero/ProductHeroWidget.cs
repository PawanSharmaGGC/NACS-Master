using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Convenience.org.Components.Widgets.ProductHero;
using Convenience.org.Helpers;
using Convenience.org.Models;
using Convenience.org.Repositories.Interfaces;
using Kentico.PageBuilder.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWidget(
    identifier: ProductHeroWidget.IDENTIFIER,
    name: "Product Hero Widget",
    viewComponentType: typeof(ProductHeroWidget),
    propertiesType: typeof(ProductHeroWidgetProperties),
    Description = "Product Hero Widget",
    IconClass = "icon-box-cart",
    AllowCache = true)]

namespace Convenience.org.Components.Widgets.ProductHero
{
    public class ProductHeroWidget : ViewComponent
    {
        public const string IDENTIFIER = "Convenience.ProductHero";
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly MediaLibraryHelpers _mediaLibraryHelpers;

        public ProductHeroWidget(IProductRepository productRepository, IMapper mapper,
            MediaLibraryHelpers mediaLibraryHelpers)
        {
            _productRepository = productRepository;
            _mediaLibraryHelpers = mediaLibraryHelpers;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(ComponentViewModel<ProductHeroWidgetProperties> model)
        {
            string imageAltText = string.Empty;

            var viewModel = new ProductHeroWidgetViewModel();
            viewModel = _mapper.Map<ProductHeroWidgetViewModel>(model.Properties);
            if (model != null && model.Properties.Products != null)
            {
                Product product = new Product();
                if (model.Properties.Products != null && model.Properties.Products.Count() > 0)
                {
                    product = _productRepository.GetProducts(model.Properties.Products.Select(x => x.WebPageGuid).ToList()).FirstOrDefault();
                }
                else
                {
                    product = _productRepository.GetProduct(model.Page.WebPageItemGUID);
                }

                if (product != null && !string.IsNullOrEmpty(product.ProductName))
                {
                    viewModel.ProductDetail = _mapper.Map<ProductViewModel>(product);
                    if (viewModel.ProductDetail == null)
                    {
                        viewModel.ProductDetail = new ProductViewModel();
                    }

                    List<ImageAssetsViewModel> images = new List<ImageAssetsViewModel>();
                    foreach (var item in product.Image)
                    {
                        string imgAltText = string.Empty;

                        ImageAssetsViewModel imageAssetsView = new ImageAssetsViewModel();
                        imageAssetsView.ImageUrl = _mediaLibraryHelpers.GetImagePath(item, ref imageAltText);
                        imageAssetsView.ImageAltText = imageAltText;

                        images.Add(imageAssetsView);
                    }
                    viewModel.ProductDetail.Images = images;
                }
            }

            return View($"~/Components/Widgets/ProductHero/_ProductHero.cshtml", viewModel);
        }
    }
}
