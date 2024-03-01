using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try { 
            HttpClient client=_httpClientFactory.CreateClient("Mango");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");

            //token
            if (withBearer)
            {
                var token = _tokenProvider.GetToken();
                message.Headers.Add("Authorization", $"Bearer {token}");
            }

                message.RequestUri = new Uri(requestDto.Url);

            if(requestDto.Data!= null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data),Encoding.UTF8, "application/json");
            }

            HttpResponseMessage? apiResponse= null;

            message.Method = requestDto.ApiType switch
            {
                ApiType.POST => HttpMethod.Post,
                ApiType.DELETE => HttpMethod.Delete,
                ApiType.PUT => HttpMethod.Put,
                _ => HttpMethod.Get
            };

            apiResponse=await client.SendAsync(message);
            return apiResponse.StatusCode switch
            {
                HttpStatusCode.NotFound => new() { IsSuccess = false, Message = "Not Found" },
                HttpStatusCode.Forbidden => new() { IsSuccess = false, Message = "Access Denied" },
                HttpStatusCode.Unauthorized => new() { IsSuccess = false, Message = "Unauthorized" },
                HttpStatusCode.InternalServerError => new() { IsSuccess = false, Message = "Internal Server Error" },
                _ => JsonConvert.DeserializeObject<ResponseDto>(await apiResponse.Content.ReadAsStringAsync())
            };
       
            }catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false
                };
                return dto;
            }
        }
    }
}
