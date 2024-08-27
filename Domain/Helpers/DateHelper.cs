using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Helpers
{
    public static class DateHelper
    {
        public static bool IsRangesOverlap(DateTime start, DateTime end, DateTime startToCompare, DateTime endToCompare)
        {
            return start < end && startToCompare < endToCompare;
        }
        
        public static bool IsRangesOverlap(TimeSpan start, TimeSpan end, TimeSpan startToCompare, TimeSpan endToCompare)
        {
            return start < endToCompare && startToCompare < end;
        }
    }
}
