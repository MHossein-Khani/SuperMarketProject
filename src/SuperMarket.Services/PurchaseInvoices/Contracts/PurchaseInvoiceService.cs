﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Services.PurchaseInvoices.Contracts
{
    public interface PurchaseInvoiceService
    {
        void Add(AddPurchaseInvoiceDto dto);
    }
}