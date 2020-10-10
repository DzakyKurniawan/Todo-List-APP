using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Base
{
    public class BaseModel
    {
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
