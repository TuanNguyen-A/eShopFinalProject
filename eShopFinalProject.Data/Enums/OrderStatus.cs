using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Enums
{
    public enum OrderStatus
    {
        NotProcessed,
        CashOnDelivery,
        Processing,
        Dispatched,
        Cancelled,
        Delivered
    }
}
