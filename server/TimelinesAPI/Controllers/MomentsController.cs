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
                new MomentModel {Topic = x.PartitionKey, RecordDate = x.RowKey, Content = x.Content}));
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateMoment([FromBody] MomentModel model)
        {
            var succeed =
                await _tableStorage.InsertOrReplaceMomentAsync(
                    new MomentEntity(model.Topic, DateTime.Parse(model.RecordDate)) {Content = model.Content});

            if (succeed)
                return NoContent();
            else
                return BadRequest();
        }
    }
}
