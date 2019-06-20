using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimelinesAPI.DataVaults
{
    public class RecordEntity : TableEntity
    {
        public RecordEntity(int yearAsKey)
            : base(yearAsKey.ToString(), KeyGenerator.NewKey())
        { }

        public RecordEntity() { }

        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }
}
