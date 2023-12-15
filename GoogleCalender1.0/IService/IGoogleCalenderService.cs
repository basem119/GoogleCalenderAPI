using GoogleCalender1._0.DTO;

namespace GoogleCalender1._0.IService
{
    public interface IGoogleCalenderService
    {
         string GetAuthCode();

        Task<GoogleTokenResponse> GetTokens(string code);
        string AddToGoogleCalendar(GoogleCalenderReq googleCalendarReqDTO);
    }
}
