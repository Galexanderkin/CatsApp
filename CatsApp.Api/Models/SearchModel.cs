using Microsoft.AspNetCore.Mvc;

namespace CatsApp.Api.Models;

public class SearchModel
{
    [FromQuery (Name = "search-text")]
    public string? SearchText { get; set; }

    [FromQuery(Name = "page-num")]
    public int PageNum { get; set; }

    [FromQuery(Name = "page-size")]
    public int PageSize { get; set; }
}
