using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
	        return Ok(timelines.OrderBy(x => x.StartTime).Select(x =>
		        new TimelineModel
		        {
			        PeriodGroupLevel = x.PeriodGroupLevel,
			        ProtectLevel = x.ProtectLevel,
			        Username = x.PartitionKey,
			        TopicKey = x.RowKey,
			        Title = x.Title,
					IsCompleted = x.IsCompleted,
                    StartTime = x.StartTime
		        }
	        ));
        }

		[HttpGet("{topicKey}")]
		[ProducesResponseType(typeof(TimelineModel), 200)]
		public async Task<IActionResult> GetTimelineByTopicKey(string topicKey)
		{
			var x = await _timelineTableStorage.GetAsync(MockUser.Username, topicKey.ToLower());
			if (x == null)
				return NotFound();

			return Ok(new TimelineModel
			{
                AccessKey = x.AccessKey,
				PeriodGroupLevel = x.PeriodGroupLevel,
				ProtectLevel = x.ProtectLevel,
				Username = x.PartitionKey,
				TopicKey = x.RowKey,
				Title = x.Title,
				IsCompleted = x.IsCompleted,
                StartTime = x.StartTime
			});
		}

		[HttpPost]
		[Authorize]
		[ProducesResponseType(typeof(TimelineModel), 200)]
		public async Task<IActionResult> AddOrUpdateTimeline([FromBody]TimelineModel model)
        {
			var entity = new TimelineEntity(MockUser.Username, model.TopicKey)
			{
				AccessKey = model.AccessKey,
				ProtectLevel = model.ProtectLevel,
				PeriodGroupLevel = model.PeriodGroupLevel,
				Title = model.Title,
				IsCompleted = model.IsCompleted,
                StartTime = model.StartTime
			};

	        var succeed = await _timelineTableStorage.InsertOrReplaceAsync(entity);
	        if (succeed)
		        return Ok(new TimelineModel {
                    AccessKey = entity.AccessKey,
			        PeriodGroupLevel = entity.PeriodGroupLevel,
			        ProtectLevel = entity.ProtectLevel,
			        Username = entity.PartitionKey,
			        TopicKey = entity.RowKey,
			        Title = entity.Title,
					IsCompleted = entity.IsCompleted,
                    StartTime = entity.StartTime
		        });
	        else
		        return BadRequest();
		}

        [HttpDelete("{key}")]
        [Authorize]
		public async Task<IActionResult> DeleteTimeline(string key)
        {
	        var succeed =
		        await _timelineTableStorage.DeleteAsync(MockUser.Username, key);
	        if (succeed)
		        return NoContent();
	        else
		        return BadRequest();
		}

	    [HttpPost("{key}/verify")]
		[ProducesResponseType(typeof(bool), 200)]
	    public async Task<IActionResult> VerifyAccessCode([FromBody] TimelineModel model)
	    {
		    var x = await _timelineTableStorage.GetAsync(MockUser.Username, model.TopicKey.ToLower());
		    if (x == null)
			    return NotFound();

		    return Ok(x.AccessKey == model.AccessKey);
	    }
    }
}
