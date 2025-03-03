using Convenience.org.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Convenience.org.Components.ViewComponents.CPagination;

public class CPaginationViewComponent : ViewComponent
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CPaginationViewComponent(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public IViewComponentResult Invoke(string position, int currentPage, int totalItems, PaginationTypeEnum paginationType = PaginationTypeEnum.Number, string requestPath = "", int pageSize = 10)
    {
        //var pageSize = 10;
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        requestPath = string.IsNullOrEmpty(requestPath) ? _httpContextAccessor.HttpContext.Request.Path : requestPath;

        // Create a model for the pagination
        var model = new PaginationViewModel
        {
            Position = position,
            TotalItems = totalItems,
            CurrentPage = currentPage,
            TotalPages = totalPages,
            PaginationType = paginationType,
            RequestPath = requestPath,
            PageNumbers = GeneratePageNumbers(currentPage, totalPages)
        };

        return View("~/Components/ViewComponents/CPagination/CPagination.cshtml", model);

    }

    public List<string> GeneratePageNumbers(int currentPage, int totalPages)
    {
        var pages = new List<string>();
        const int maxVisiblePages = 5;

        if (totalPages <= maxVisiblePages)
        {
            for (int i = 1; i <= totalPages; i++)
            {
                pages.Add(i.ToString());
            }
        }
        else
        {
            if (currentPage > maxVisiblePages - 2)
            {
                pages.Add("...");
            }
            if (currentPage - 1 > 1 && currentPage + 1 < totalPages)
            {
                pages.Add((currentPage - 1).ToString());
                pages.Add(currentPage.ToString());
                pages.Add((currentPage + 1).ToString());
            }
            else if (currentPage - 1 <= 1)
            {
                pages.Add(currentPage.ToString());
                pages.Add((currentPage + 1).ToString());
            }
            else if (currentPage + 1 >= totalPages)
            {
                pages.Add((currentPage - 1).ToString());
                pages.Add(currentPage.ToString());
            }
            if (currentPage < totalPages - 2)
            {
                pages.Add("...");
            }
            if (!pages.Contains(totalPages.ToString()))
            {
                pages.Add(totalPages.ToString());
            }
        }

        return pages;
    }

}

