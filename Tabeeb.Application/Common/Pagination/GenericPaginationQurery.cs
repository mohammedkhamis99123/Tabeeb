using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tabeeb.Application.Common.Pagination
{
    public  class GenericPaginationQurery<T>:IRequest<T>
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string SearchTerm { get; set; } = "";

        public string SortBy { get; set; } = "Id";

        public string SortOrder { get; set; } = "asc";
    }
}
