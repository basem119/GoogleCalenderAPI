using GoogleCalender1._0.DTO;
using GoogleCalender1._0.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCalender1._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private IGoogleCalenderService _googleCalendarService;
        public UserController(IGoogleCalenderService googleCalendarService)
        {
            _googleCalendarService = googleCalendarService;
        }


        [HttpGet]
        [Route("/api/google")]
        public async Task<IActionResult> GoogleAuth()
        {
            return Redirect(_googleCalendarService.GetAuthCode());
        }
       
        [HttpGet]
        public async Task<IActionResult> callback(string code)
        {
            if (code == null)
                throw new Exception("empty code");
         
            var token = await _googleCalendarService.GetTokens(code);
            return Ok(token);
        }
 
        [HttpPost]
        [Route("/user/calendarevent")]
        public async Task<IActionResult> AddCalendarEvent([Bind("summary,description,location,startDateTime,endDateTime")] GoogleCalenderReq calendarEventReqDTO)
        {
            calendarEventReqDTO.CalenderId = "primary";
            var data =  _googleCalendarService.AddToGoogleCalendar(calendarEventReqDTO);
            if (data == null) 
                return Ok(StatusCodes.Status406NotAcceptable);
            else
             return Ok(data);
        }
    }
}
