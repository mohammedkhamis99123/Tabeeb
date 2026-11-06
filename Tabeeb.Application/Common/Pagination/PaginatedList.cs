using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Common.Pagination
{
    public  class PaginatedList<T>
    {
        public int TotalPages {  get; set; }
        public IReadOnlyList<T> Items { get; set; }

        public int TotalCount {  get; set; }

        public int Page {  get; set; }

        public bool HasPreviousPage {  get; set; }
        public bool HasNextPage { get; set; }



        public static  async Task<PaginatedList<T>> CreateAsync(IQueryable<T>Items, int page,int pageSize)
        {
            var Data = Items.Skip((page-1) * pageSize).Take(pageSize);

            var totalPages = (int)Math.Ceiling((decimal)Items.Count() / pageSize);
            return new PaginatedList<T>
            {
               
                Page = page,
                Items = Data.ToList(),
                TotalPages = totalPages,
                TotalCount = Items.Count(),
                HasPreviousPage = totalPages > page,
                HasNextPage = totalPages > page


            };
        }
    }
}
