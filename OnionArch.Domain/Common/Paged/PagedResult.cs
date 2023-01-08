using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Domain.Common.Paged
{
    public class PagedResult<T> : PagedOption
    {
        public int Count { get; set; }
        public int PageCount { get => Convert.ToInt32(Math.Ceiling(Count / (double)PageSize)); }
        public IQueryable<T>? QueryableData { get; set; }
    }
}
