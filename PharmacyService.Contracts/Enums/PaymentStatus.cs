﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyService.Contracts.Enums
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
}
