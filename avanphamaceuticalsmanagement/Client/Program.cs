using avanphamaceuticalsmanagement.Client;
using avanphamaceuticalsmanagement.Client.Services;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using avanphamaceuticalsmanagement.Client.Pages.Dashboard;

namespace avanphamaceuticalsmanagement.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddHttpClient("avanphamaceuticalsmanagement.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("avanphamaceuticalsmanagement.ServerAPI"));



            builder.Services.AddScoped<IGenericService, GenericService>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<IndexBase>();
            builder.Services.AddMudServices();
            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
    }
}