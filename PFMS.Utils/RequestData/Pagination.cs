﻿namespace PFMS.Utils.RequestData
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public Pagination()
        {
            PageNumber = 1;
            PageSize = 1000;
        }
    }
}
