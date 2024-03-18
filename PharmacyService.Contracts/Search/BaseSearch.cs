using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts
{
    public class BaseSearch
    {
        public object SearchTerm { get; set; }
        public SortOptions SortOptions { get; set; } = new SortOptions();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public BaseSearch()
        {
            SortOptions = new SortOptions();
        }
    }

    public class SortOptions
    {
        public string OrderBy { get; set; } = "Name"; // Default sorting field
        public SortOrder Order { get; set; } = SortOrder.Ascending; // Default sorting order
    }

    public enum SortOrder
    {
        Ascending,
        Descending
    }
}
