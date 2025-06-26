﻿namespace PriceLoaderWebApp.Domain.Entities
{
    public class PriceItem
    {
        public string Vendor { get; set; }
        public string Number { get; set; }
        public string SearchVendor { get; set; }
        public string SearchNumber { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
    }
}
