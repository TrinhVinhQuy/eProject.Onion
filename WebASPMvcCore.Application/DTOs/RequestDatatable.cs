using Microsoft.AspNetCore.Mvc;

namespace WebASPMvcCore.Application.DTOs
{
    public class RequestDatatable
    {
        [BindProperty(Name = "length")]
        public int PageSize { get; set; }
        [BindProperty(Name = "start")]
        public int Page { get; set; }

        [BindProperty(Name = "search[value]")]
        public string? Keyword { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
    }
}
