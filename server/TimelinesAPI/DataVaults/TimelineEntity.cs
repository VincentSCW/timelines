using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace TimelinesAPI.DataVaults
{
	public enum ProtectLevel
	{
		/// <summary>
		/// 0: public, 
		/// </summary>
		Public,
		/// <summary>
		/// 1: protected
		/// </summary>
		Protected
	}

	public enum PeriodGroupLevel
	{
		Any,
		ByDay,
		ByMonth,
		ByYear
	}

    public class TimelineEntity : TableEntity
    {
	    public TimelineEntity(string username, string topicKey)
		    : base(username, topicKey)
	    {
	    }

		public TimelineEntity() { }

	    public int ProtectLevelValue { get; set; }
	    [IgnoreProperty]
	    public ProtectLevel ProtectLevel
	    {
		    get => (ProtectLevel) ProtectLevelValue;
		    set => ProtectLevelValue = (int) value;
	    }
		public string AccessKey { get; set; }
		public string Title { get; set; }
		public int PeriodGroupLevelValue { get; set; }

	    [IgnoreProperty]
	    public PeriodGroupLevel PeriodGroupLevel
	    {
		    get => (PeriodGroupLevel) PeriodGroupLevelValue;
		    set => PeriodGroupLevelValue = (int) value;
	    }
    }
}
