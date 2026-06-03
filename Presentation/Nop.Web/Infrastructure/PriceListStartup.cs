using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using Nop.Services.Catalog; // Твій сервіс
using Nop.Web.Areas.Admin.Factories; // Твоя фабрика

namespace Nop.Web.Infrastructure
{
    public class PriceListStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Реєструємо і сервіс, і фабрику в ОДНОМУ твоєму власному файлі
            services.AddScoped<IPriceListService, PriceListService>();
            services.AddScoped<IPriceListModelFactory, PriceListModelFactory>();
            // Реєстрація сервісу
            services.AddScoped<IPriceListItemService, PriceListItemService>();

            // Реєстрація фабрики моделей
            services.AddScoped<IPriceListItemModelFactory, PriceListItemModelFactory>();
        }

        public void Configure(IApplicationBuilder application)
        {
        }

        // Вказуємо порядок (більше 0, щоб завантажилось після базових налаштувань)
        public int Order => 100;
    }
}