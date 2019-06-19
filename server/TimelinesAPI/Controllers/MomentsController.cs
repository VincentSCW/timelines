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
    public class MomentsController : Controller
    {
        private readonly MomentTableStorageVaults _momentTableStorage;
        private IMapper _mapper;

        public MomentsController(MomentTableStorageVaults tableStorage,
            IMapper mapper)
        {
            _momentTableStorage = tableStorage;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MomentModel>), 200)]
        public async Task<IActionResult> GetMomentsByTimeline([FromQuery]string timeline)
        {
            var moments = await _momentTableStorage.GetListAsync($"{MockUser.Username}_{timeline}");
            return Ok(moments.Select(x => _mapper.Map<MomentModel>(x)));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(MomentModel), 200)]
        public async Task<IActionResult> AddOrUpdateMoment([FromBody] MomentModel model)
        {
			var value = await _momentTableStorage.GetAsync(model.TopicKey, DateTime.Parse(model.RecordDate).ToString(MomentEntity.DateFormat));

			// If not exists, it should be a new entity, add it with username
			var entity = new MomentEntity(value == null ? $"{MockUser.Username}_{model.TopicKey}" : model.TopicKey,
				DateTime.Parse(model.RecordDate).ToString(MomentEntity.DateFormat))
            {
                Content = model.Content
            };
            var succeed =
                await _momentTableStorage.InsertOrReplaceAsync(entity);

            if (succeed)
                return Ok(_mapper.Map<MomentModel>(entity));
            else
                return BadRequest();
        }

        [HttpDelete("{topic}/{date}")]
        [Authorize]
		public async Task<IActionResult> DeleteMoment(string topic, string date)
        {
            var succeed =
                await _momentTableStorage.DeleteAsync(topic, DateTime.Parse(date).ToString(MomentEntity.DateFormat));
            if (succeed)
                return NoContent();
            else
                return BadRequest();
        }
    }
}
