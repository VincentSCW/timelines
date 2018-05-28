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
        private readonly TableStorageVaults _tableStorage;
        public MomentsController(TableStorageVaults tableStorage)
        {
            _tableStorage = tableStorage;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MomentModel>), 200)]
        public async Task<IActionResult> GetMomentsByTimeline([FromQuery]string timeline)
        {
            var moments = await _tableStorage.GetMomentsAsync(timeline);
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
                await _tableStorage.InsertOrReplaceMomentAsync(entity);

            if (succeed)
                return Ok(new MomentModel{TopicKey = entity.PartitionKey, RecordDate = entity.RowKey, Content = entity.Content});
            else
                return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMoment([FromQuery] string topic, [FromQuery] string date)
        {
            var succeed =
                await _tableStorage.DeleteMomentAsync(topic, DateTime.Parse(date).ToString(MomentEntity.DateFormat));
            if (succeed)
                return NoContent();
            else
                return BadRequest();
        }
    }
}
