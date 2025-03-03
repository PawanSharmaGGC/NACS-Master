using System.Collections.Generic;

namespace Convenience.org.Models;

public class PaginationViewModel
{
    public string Position { get; set; }
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public PaginationTypeEnum PaginationType { get; set; }  
    public string RequestPath { get; set; }
    public List<string> PageNumbers { get; set; }
}


public enum PaginationTypeEnum
{
    Dot,
    Number,
}
