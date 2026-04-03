using System;
using System.Collections.Generic;
using System.Text;

namespace WSC.Store.Application.Dtos
{
    public class CreateInventoryRecordDto
    {
        public int ProductId { get; set; }
        public int InStock { get; set; }
        public int MinStock { get; set; }

    }
}
