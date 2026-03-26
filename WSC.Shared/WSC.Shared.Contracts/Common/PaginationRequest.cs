using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Shared.Contracts.Common
{
    public class PaginationRequest
    {
        private const int MaxPageSize = 50;

        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize; 
            set => _pageSize = value <= 0 ? 10 : (value > MaxPageSize ? MaxPageSize : value);
        }
    }
}
