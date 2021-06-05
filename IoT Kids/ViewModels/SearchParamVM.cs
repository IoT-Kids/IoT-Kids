using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoT_Kids.AssistingModels
{
    public class SearchParamVM
    {
        public string UserId { get; set; }
        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
