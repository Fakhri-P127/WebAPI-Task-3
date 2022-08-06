using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_Task_3.DTOs
{
    public class ListDto<T>
    {
        public List<T> ListItemDtos { get; set; }
        public int TotalCount { get; set; }

    }
}
