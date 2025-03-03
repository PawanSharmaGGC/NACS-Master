using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACS.Portal.Core.Infrastructure.Search
{
    public class FacetOption
    {
        public string Label { get; set; } = "";
        public string Value { get; set; } = "";
        public int Count { get; set; }
        public bool IsSelected { get; set; }
    }
}
