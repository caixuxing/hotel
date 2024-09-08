using Hotel.Application.Hotel.Commands.CreateHotel;
using Hotel.Application.Hotel.Commands.SaveARHotel;
using Hotel.Application.Hotel.Query.FindARHotelByCode;
using Hotel.Application.Hotel.Query.PageList;
using Hotel.Application.TaskTrigger.Commands.SaveTaskTrigger;
using Hotel.Domain.Repo;
using Hotel.WebUI.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTaste;
using Newtonsoft.Json;
using Snowflake.Core;
using System.Diagnostics;
using System.Text;

namespace Hotel.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMediator _mediator;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly IdWorker _idWorker;

        private readonly IBookRepository _bookRepository;

        public HomeController(ILogger<HomeController> logger, IMediator mediator, IHttpClientFactory httpClientFactory, IdWorker idWorker, IBookRepository bookRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _httpClientFactory = httpClientFactory;
            _idWorker = idWorker;
            _bookRepository = bookRepository;
        }
        public async Task<IActionResult> Index()
        {
            List<long> taskId = new List<long>();

            for (int i = 0; i < 1000; i++)
            {
                taskId.Add(_idWorker.NextId());
            }


            // _bookRepository.AddBook(new Domain.Entity.Book() { Title = "MongoDB测试", Author = "caicx", No = _idWorker.NextId() });

           var data= await _bookRepository.GetListAsync(x => x.No == 1832689690612994048);


            return View();
        }

        /// <summary>
        /// 获取酒店分页列表
        /// </summary>
        /// <returns></returns>
        [HttpPost,Route("page")]
        public async Task<JsonResult> GetHotelPageList([FromBody] HotelPageParamQry qry)
        {
             var data = await _mediator.Send(qry);
            return Json(data);
        }

        /// <summary>
        /// 按酒店Code获取配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("findARHotelByCode/{code}")]
        public async Task<JsonResult> GetARHotelByCode([FromRoute] string code)
        {
            var data = await _mediator.Send(new FindARHotelParamQry(code== "undefined"?string.Empty:code));
            return Json(data);
        }

        /// <summary>
        /// 保存酒店平台映射配置
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost, Route("save")]
        public async Task<ObjectResult> SaveARHotel([FromBody] SaveARHotelCmd cmd)
            =>Ok(await _mediator.Send(cmd));



        /// <summary>
        /// 保存任务触发配置信息
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost, Route("saveTaskTrigger")]
        public async Task<ObjectResult> SaveTaskTrigger([FromBody] SaveTaskTriggerCmd cmd)
            => Ok(await _mediator.Send(cmd));


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async Task<ObjectResult> Create([FromBody] HotelInputCmd cmd) 
            => Ok(await _mediator.Send(cmd));
    }
}
