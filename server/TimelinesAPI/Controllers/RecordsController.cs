using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimelinesAPI.DataVaults;
using TimelinesAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TimelinesAPI.Controllers
{
    [Route("api/[controller]")]
    public class RecordsController : Controller
    {
        private readonly RecordTableStorageVaults _recordTableStorageVaults;
        private readonly IMapper _mapper;

        public RecordsController(RecordTableStorageVaults recordTableStorageVaults,
            IMapper mapper)
        {
            _recordTableStorageVaults = recordTableStorageVaults;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<RecordModel>), 200)]
        public async Task<IActionResult> GetRecordsByYear([FromQuery]int year)
        {
            var records = await _recordTableStorageVaults.GetListAsync($"{MockUser.Username}_{year}");
            return Ok(records.OrderBy(x => x.Date).Select(x => _mapper.Map<RecordModel>(x)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(RecordModel), 200)]
        public async Task<IActionResult> AddOrUpdateRecord([FromBody]RecordModel model)
        {
            RecordEntity value = null;
            if (model.Key != null)
                value = await _recordTableStorageVaults.GetAsync($"{MockUser.Username}_{model.Date.Year}", model.Key);

            if (value == null)
            {
                value = new RecordEntity($"{MockUser.Username}_{model.Date.Year}");
            }
            value.Date = model.Date;
            value.Description = model.Description;
            value.ImageUrl = model.ImageUrl;
            value.Title = model.Title;

            var succeed = await _recordTableStorageVaults.InsertOrReplaceAsync(value);
            if (succeed)
                return Ok(_mapper.Map<RecordModel>(value));
            else
                return BadRequest();
        }

        [HttpDelete("{year}/{key}")]
        public async Task<IActionResult> DeleteRecord(int year, string key)
        {
            var succeed = await _recordTableStorageVaults.DeleteAsync($"{MockUser.Username}_{year}", key);
            if (succeed)
                return NoContent();
            else
                return BadRequest();
        }
    }
}
