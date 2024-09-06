using Hotel.Application.Hotel.Commands.CreateHotel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{


    /// <summary>
    /// 酒店
    /// </summary>
    [ApiController]
    [Route("api/hotel")]
    public class HotelController : ControllerBase
    {


        private readonly IMediator _mediator;

        public HotelController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// 创建酒店
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async Task<ObjectResult> CreateSysUser([FromBody] HotelInputCmd cmd) => Ok(await _mediator.Send(cmd));



        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpGet, Route("")]
        public async Task<ObjectResult> Test() => Ok(await Task.FromResult("OK"));
    }
}
