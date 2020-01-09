﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _repository;

        public int PageSize = 4;

        public ProductController(IProductRepository repo)
        {
            _repository = repo;
        }

        public ViewResult List(string category, int productPage = 1)
          => View(new ProductsListViewModel
          {
              Products = _repository.Products
              .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
              Paginginfo = new Paginginfo
              {
                  CurrentPage = productPage,
                  ItemsPerPage = PageSize,
                  Totalitems = category == null ? _repository.Products.Count() 
                  : _repository.Products.Where(e => e.Category == category).Count()
              },
              CurrentCategory = category
          });
    }
}