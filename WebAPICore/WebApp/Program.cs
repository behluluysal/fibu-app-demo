using App.Repository;
using App.Repository.ApiClient;
using App.Repository.AuthenticationRepo;
using Core.Utility;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Radzen;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UseCases;
using UseCases.AuthenticationUseCases;
using WebApp.Utility;

namespace WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");



            //dependency injection
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy(Permission.Dashboards.View, policy=> policy.RequireClaim(Permission.Dashboards.View));

                // The rest omitted for brevity.
            });
            builder.Services.AddSingleton<ITokenRepository, TokenRepository>();
            builder.Services.AddSingleton<AuthenticationStateProvider, JwtTokenAuthenticationStateProvider>();


            builder.Services.AddSingleton<IWebApiExecuter>(sp =>
                new WebApiExecuter(
                    "https://localhost:5001",
                    new HttpClient(),
                    sp.GetRequiredService<ITokenRepository>()));

            #region dependencies for features
            builder.Services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
            builder.Services.AddTransient<IAuthenticationUseCases, AuthenticationUseCases>();
            #endregion

            #region dependencies for uis
            builder.Services.AddTransient<IConnectionTestRepository, ConnectionTestRepository>();
            builder.Services.AddTransient<IConnectionTestScreenUseCases, ConnectionTestScreenUseCases>();

            builder.Services.AddTransient<IPaymentRepository, PaymentRepository>();
            builder.Services.AddTransient<IPaymentsScreenUseCases, PaymentsScreenUseCases>();

            builder.Services.AddTransient<IOfferRepository, OfferRepository>();
            builder.Services.AddTransient<IOfferScreenUseCases, OfferScreenUseCases>();

            builder.Services.AddTransient<IRequestRepository, RequestRepository>();
            builder.Services.AddTransient<IRequestsScreenUseCases, RequestsScreenUseCases>();

            builder.Services.AddTransient<IBPRPEmailRepository, BPRPEmailRepository>();

            builder.Services.AddTransient<IProductRepository, ProductRepository>();
            builder.Services.AddTransient<IProductsScreenUseCases, ProductsScreenUseCases>();

            builder.Services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            builder.Services.AddTransient<IApplicationUsersScreenUseCases, ApplicationUsersScreenUseCases>();

            builder.Services.AddTransient<ITagRepository, TagRepository>();
            builder.Services.AddTransient<ITagsScreenUseCases, TagsScreenUseCases>();

            builder.Services.AddTransient<IRoleRepository, RoleRepository>();
            builder.Services.AddTransient<IRolesScreenUseCases, RolesScreenUseCases>();

            builder.Services.AddTransient<ISupplierCompaniesScreenUseCases, SupplierCompaniesScreenUseCases>();
            builder.Services.AddTransient<ISupplierCompanyRepository, SupplierCompanyRepository>();

            builder.Services.AddTransient<IResponsiblePeopleScreenUseCases, ResponsiblePeopleScreenUseCases>();
            builder.Services.AddTransient<IResponsiblePersonRepository, ResponsiblePersonRepository>();

            builder.Services.AddTransient<IBusinessPartnersScreenUseCases, BusinessPartnersScreenUseCases>();
            builder.Services.AddTransient<IBusinessPartnerRepository, BusinessPartnerRepository>();

            builder.Services.AddTransient<IBPResponsiblePeopleScreenUseCases, BPResponsiblePeopleScreenUseCases>();
            builder.Services.AddTransient<IBPResponsiblePersonRepository, BPResponsiblePersonRepository>();

            builder.Services.AddTransient<IRequestedProductsScreenUseCases, RequestedProductsScreenUseCases>();
            builder.Services.AddTransient<IRequestedProductRepository, RequestedProductRepository>();

            builder.Services.AddTransient<IChatsScreenUseCases, ChatsScreenUseCases>();
            builder.Services.AddTransient<IChatRepository, ChatRepository>();
            #endregion




            //dependency injection end


            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            #region Radzen components
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();
            #endregion




            await builder.Build().RunAsync();
        }
    }
}
