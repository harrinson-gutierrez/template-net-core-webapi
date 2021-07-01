using Application.Interfaces.Resources;
using Microsoft.Extensions.Localization;

namespace Infrastructure.Resources
{
    public class SharedResources : IAppResource
    {
        private IStringLocalizer<SharedResources> Localizer;

        public SharedResources(IStringLocalizer<SharedResources> localizer)
        {
            Localizer = localizer;
        }

        public string this[string key]
        {
            get
            {
                return Localizer[key];
            }
        }

        public string this[string key, object[] arguments]
        {
            get
            {
                return Localizer[key, arguments];
            }
        }
    }
}
