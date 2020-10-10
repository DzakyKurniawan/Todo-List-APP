using System;
using System.Collections.Generic;
using System.Text;

namespace Data.ViewModel
{
    public class PagingRequester
    {
        public string username { get; set; }
        public int status { get; set; }
        public string keyword { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
