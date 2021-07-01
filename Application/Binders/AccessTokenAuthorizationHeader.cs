using Application.Binders;
using Microsoft.AspNetCore.Mvc;

namespace Application.Binders
{
    [ModelBinder(BinderType = typeof(AuthorizationHeaderBinder))]
    public class AccessTokenAuthorizationHeader
    {
        public string TokenValue { get; set; }
    }
}
