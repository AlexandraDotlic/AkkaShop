﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum OrderStatus: byte
    {
        None = 0,
        Created = 1,
        Canceled = 2
    }
}
