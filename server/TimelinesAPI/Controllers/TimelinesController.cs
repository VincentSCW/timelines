using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

		public TimelinesController(TimelineTableStorageVaults timelineTableStorage,
            IMapper mapper)
	    {
		    _timelineTableStorage = timelineTableStorage;
            _mapper = mapper;
	    }

        [HttpGet]
		[ProducesResponseType(typeof(List<TimelineModel>), 200)]
        public async Task<IActionResult> GetTimelines()
        {
	        var timelines = await _timelineTableStorage.GetListAsync(MockUser.Username);
	        return Ok(timelines.OrderBy(x => x.StartTime).Select(x => _mapper.Map<TimelineModel>(x)));
        }

		[HttpGet("{topicKey}")]
		[ProducesResponseType(typeof(TimelineModel), 200)]
		public async Task<IActionResult> GetTimelineByTopicKey(string topicKey)
		{
			var x = await _timelineTableStorage.GetAsync(MockUser.Username, topicKey.ToLower());
			if (x == null)
				return NotFound();

			return Ok(_mapper.Map<TimelineModel>(x));
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
		        return Ok(_mapper.Map<TimelineModel>(entity));
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
