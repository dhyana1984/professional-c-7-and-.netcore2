﻿using System;
namespace SystemTransactionSample
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string Isbn { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}
