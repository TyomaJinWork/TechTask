using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class CreateWindowDTO
    {
        public string Name { get; set; } = string.Empty;
        public required string WindowStartDate { get; set; }
        public required string WindowEndDate { get; set; }
        public required string IntervalStartTime { get; set; }
        public required string IntervalEndTime { get; set; }
        public required decimal Price { get; set; }
    }
}
