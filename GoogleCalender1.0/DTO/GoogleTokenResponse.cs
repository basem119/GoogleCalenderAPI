using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;

namespace GoogleCalender1._0.DTO
{
    public class GoogleTokenResponse
    {       
        public String? access_type { get; set; }
        public long expires_in { get; set; }
        public string? refresh_token { get; set; }
        public string? scope { get; set; }
        public string? token_type { get; set; }
        public String? refreshToken { get; set; }

    }
}
