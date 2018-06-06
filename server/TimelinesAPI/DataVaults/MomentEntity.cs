using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace TimelinesAPI.DataVaults
{
    public class MomentEntity : TableEntity
    {
        public static string DateFormat = "yyyy-MM-dd";

        public MomentEntity(string topicKey, string recordDate)
            : base(topicKey.ToLower(), recordDate)
        {
        }

        public MomentEntity() { }

        public string Content { get; set; }
    }
}
