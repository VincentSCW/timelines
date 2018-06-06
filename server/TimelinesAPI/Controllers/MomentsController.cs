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
    public class MomentsController : Controller
    {
        private readonly MomentTableStorageVaults _momentTableStorage;
        public MomentsController(MomentTableStorageVaults tableStorage)
        {
            _momentTableStorage = tableStorage;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MomentModel>), 200)]
        public async Task<IActionResult> GetMomentsByTimeline([FromQuery]string timeline)
        {
            var moments = await _momentTableStorage.GetListAsync($"{MockUser.Username}_{timeline}");
            return Ok(moments.Select(x =>
                new MomentModel {TopicKey = x.PartitionKey, RecordDate = x.RowKey, Content = x.Content}));
        }

        [HttpPost]
        [ProducesResponseType(typeof(MomentModel), 200)]
        public async Task<IActionResult> AddOrUpdateMoment([FromBody] MomentModel model)
        {
            var entity = new MomentEntity(model.TopicKey,
				DateTime.Parse(model.RecordDate).ToString(MomentEntity.DateFormat))
            {
                Content = model.Content
            };
            var succeed =
                await _momentTableStorage.InsertOrReplaceAsync(entity);

            if (succeed)
                return Ok(new MomentModel{TopicKey = entity.PartitionKey, RecordDate = entity.RowKey, Content = entity.Content});
            else
                return BadRequest();
        }

        [HttpDelete("{topic}/{date}")]
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
