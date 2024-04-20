﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eProject.Application.DTOs.Product
{
    public class ProductCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
        public int SoldItem { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaLink { get; set; }
        public string MetaImage { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryMetaLink { get; set; }
        public List<ProductImgaeDTO> Images { get; set; }
    }
}
