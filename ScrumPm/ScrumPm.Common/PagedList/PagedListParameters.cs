namespace ScrumPm.Common.PagedList
{
    using System.Collections.Generic;

    public class PagedListParameters
    {
        public IList<Order> Orders { get; set; } = new List<Order>();

        public Page Page { get; set; } = new Page();
    }
}
