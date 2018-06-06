using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TimelinesAPI.DataVaults;
using TimelinesAPI.Models;

namespace TimelinesAPI.Controllers
{
    [Route("api/[controller]")]
    public class TimelinesController : Controller
    {
	    private readonly TimelineTableStorageVaults _timelineTableStorage;

		public TimelinesController(TimelineTableStorageVaults timelineTableStorage)
	    {
		    _timelineTableStorage = timelineTableStorage;
	    }

        [HttpGet]
		[ProducesResponseType(typeof(List<TimelineModel>), 200)]
        public async Task<IActionResult> GetTimelines()
        {
	        var timelines = await _timelineTableStorage.GetListAsync(MockUser.Username);
	        return Ok(timelines.Select(x =>
		        new TimelineModel
		        {
			        AccessKey = x.AccessKey,
			        PeriodGroupLevel = x.PeriodGroupLevel,
			        ProtectLevel = x.ProtectLevel,
			        Username = x.PartitionKey,
			        TopicKey = x.RowKey,
			        Title = x.Title
		        }
	        ));
        }

        //[HttpGet("{topicKey}")]
        //public async Task<IActionResult> GetTimelineByTopicKey(string topicKey)
        //{
        //    return "value";
        //}

        [HttpPost]
        [ProducesResponseType(typeof(TimelineModel), 200)]
		public async Task<IActionResult> AddOrUpdateTimeline([FromBody]TimelineModel model)
        {
			var entity = new TimelineEntity(MockUser.Username, model.TopicKey)
			{
				AccessKey = model.AccessKey,
				ProtectLevel = model.ProtectLevel,
				PeriodGroupLevel = model.PeriodGroupLevel,
				Title = model.Title
			};

	        var succeed = await _timelineTableStorage.InsertOrReplaceAsync(entity);
	        if (succeed)
		        return Ok(new TimelineModel {
			        AccessKey = entity.AccessKey,
			        PeriodGroupLevel = entity.PeriodGroupLevel,
			        ProtectLevel = entity.ProtectLevel,
			        Username = entity.PartitionKey,
			        TopicKey = entity.RowKey,
			        Title = entity.Title
		        });
	        else
		        return BadRequest();
		}

        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteTimeline(string key)
        {
	        var succeed =
		        await _timelineTableStorage.DeleteAsync(MockUser.Username, key);
	        if (succeed)
		        return NoContent();
	        else
		        return BadRequest();
		}
    }
}
