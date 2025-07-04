using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Models.Enums
{
    public enum OrderStatus
    {
        Created = 0,
        SubmittedToChannel = 1,
        Paid = 2,
        Settled = 3,
        CANCELLED = 98,
        FAILED = 99
    }
}
