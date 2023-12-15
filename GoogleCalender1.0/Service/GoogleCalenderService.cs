using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using GoogleCalender1._0.Common;
using GoogleCalender1._0.DTO;
using GoogleCalender1._0.IService;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace GoogleCalender1._0.Service
{
    public class GoogleCalenderService : IGoogleCalenderService
    {

        private readonly HttpClient _httpClient;

        public GoogleCalenderService()
        {
            _httpClient = new HttpClient();
        }

        public string GetAuthCode()
        {
            try
            {
                string scopeURL1 = "https://accounts.google.com/o/oauth2/auth?redirect_uri={0}&prompt={1}&response_type={2}&client_id={3}&scope={4}&access_type={5}";
                var redirectURL = "http://localhost:3000";
                string prompt = "consent";
                string response_type = "code";
                string clientID = "785164392068-kg6b3553aab9b8t6447o26tpdqa9c7f1.apps.googleusercontent.com";
                string scope = "https://www.googleapis.com/auth/calendar";
                string access_type = "offline";
                string redirect_uri_encode = Method.urlEncodeForGoogle(redirectURL);
                var mainURL = string.Format(scopeURL1, redirectURL, prompt, response_type, clientID, scope, access_type);
                return mainURL;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public async Task<GoogleTokenResponse> GetTokens(string code)
        {
             var clientId = "785164392068-kg6b3553aab9b8t6447o26tpdqa9c7f1.apps.googleusercontent.com";
             string clientSecret = "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";            
             var redirectURL = "http://localhost:3000";
             var tokenEndpoint = "https://accounts.google.com/o/oauth2/token";
            var content = new StringContent($"code={code}&redirect_uri={Uri.EscapeDataString(redirectURL)}&client_id={clientId}" +
                 $"&client_secret={clientSecret}&grant_type=authorization_code", Encoding.UTF8, "application/x-www-form-urlencoded");
             var response = await _httpClient.PostAsync(tokenEndpoint, content);
             var responseContent = await response.Content.ReadAsStringAsync();
             if (response.IsSuccessStatusCode)
             {
                 var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenResponse>(responseContent);
                 return tokenResponse;
             }
             else
             {
                 // Handle the error case when authentication fails
                 throw new Exception($"Failed to authenticate: {responseContent}");
             }
            
            
        }

        public string AddToGoogleCalendar(GoogleCalenderReq googleCalendarReqDTO)
        {
            try
            {
                var token = new TokenResponse
                {
                    
                    RefreshToken = googleCalendarReqDTO.refreshToken

                };
                var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
                  new GoogleAuthorizationCodeFlow.Initializer
                  {
                      ClientSecrets = new ClientSecrets
                      {
                          ClientId = "785164392068-kg6b3553aab9b8t6447o26tpdqa9c7f1.apps.googleusercontent.com",
                          ClientSecret = "OCSPX-k1Nw9noLzATaAdbz4YuvkKJxDukh"
                      }

                  }), "user", token);

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials,
                });

                Event newEvent = new Event()
                {
                    Summary = googleCalendarReqDTO.Summary,
                    Description = googleCalendarReqDTO.Description,
                    Start = new EventDateTime()
                    {
                        DateTime = googleCalendarReqDTO.StartTime,
                        //TimeZone = Method.WindowsToIana();    //user's time zone
                    },
                    End = new EventDateTime()
                    {
                        DateTime = googleCalendarReqDTO.EndTime,
                        //TimeZone = Method.WindowsToIana();    //user's time zone
                    },
                    Reminders = new Event.RemindersData()
                    {
                        UseDefault = false,
                        Overrides = new EventReminder[] {

                new EventReminder() {
                    Method = "email", Minutes = 30
                  },

                  new EventReminder() {
                    Method = "popup", Minutes = 15
                  },

                  new EventReminder() {
                    Method = "popup", Minutes = 1
                  },
              }
                    }

                };

                EventsResource.InsertRequest insertRequest = service.Events.Insert(newEvent, googleCalendarReqDTO.CalenderId);
                Event createdEvent = insertRequest.Execute();
                return createdEvent.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return string.Empty;
            }
        }

      
    }
}
