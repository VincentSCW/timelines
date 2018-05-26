using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace TimelinesAPI.DataVaults
{
    public class MomentEntity : TableEntity
    {
        public MomentEntity(string topic, DateTime recordDate)
        {
            PartitionKey = topic;
            RowKey = recordDate.ToShortDateString();
        }

        public MomentEntity() { }

        public string Content { get; set; }
    }
}
