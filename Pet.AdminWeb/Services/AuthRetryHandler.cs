using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Pet.AdminWeb.Services // hoặc namespace bạn đang dùng
{
    public class AuthRetryHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRetryHandler(IHttpContextAccessor httpContextAccessor)   
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Lấy token từ cookie hoặc header
            var token = _httpContextAccessor.HttpContext?.Request?.Cookies["AuthToken"]; // Hoặc tên cookie khác nếu bạn đặt

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
