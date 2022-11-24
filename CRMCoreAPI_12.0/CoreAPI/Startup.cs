using System;
using CRMCoreAPI.Filters;
using CRMCoreAPI.Service;
using CRMCoreAPI.ServiceInterface;
using CRMCoreAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.IO;
using CRMCoreAPI;

namespace CoreAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        } 

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        readonly string allowSpecificOrigins = "_allowSpecificOrigins";
        /// <summary>
        /// // This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddNewtonsoftJson();
            var appSettings = Configuration.GetSection("AppSettings");
            ////////for identity server authentication/////////
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.Authority = Configuration.GetSection("IdentityUrl").Value;//"http://localhost:27159";//idenitty server url
                    options.RequireHttpsMetadata = false;

                    //options.TokenValidationParameters.ValidateAudience = false; options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
                    // options.Audience = "oidc";
                    options.ApiName = "CRMAPI";
                });
            /////////////////////////////////////////////
            services.AddMvc(options =>
            {
                options.InputFormatters.Insert(0, new RawRequestBodyInputFormatter());
            });
                        
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CRM API",
                    Description = "CRM micro services",
                    TermsOfService = new Uri("https://www.talygen.com"),
                    Contact = new OpenApiContact()
                    {
                        Name = "TALYGEN CRM API",
                        Email = string.Empty,
                        Url = new Uri("https://www.talygen.com")
                    }

                });
            });

            services.AddCors();

            services.AddControllers();
            services.AddHttpContextAccessor();

            services.AddDistributedMemoryCache();
           

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddDistributedMemoryCache();
            //services.AddSingleton<IExpenseServices, ExpenseServices>();
            services.AddSingleton<ILeadService, LeadService>();
            services.AddSingleton<IBrandService, BrandService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IQuotationService, QuotationService>();
            services.AddSingleton<IContractService, ContractService>();
            services.AddSingleton<IStageService, StageService>();
            services.AddSingleton<IForecastService, ForecastServices>();
            services.AddSingleton<IDealService, DealService>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<ICampaignService, CampaignService>();
            services.AddSingleton<IActivityService, ActivityService>();
            services.AddSingleton<IVendorService, VendorService>();
            services.AddSingleton<IPurchaseOrderService, PurchaseOrderService>();
            services.AddSingleton<IPriceBookService, PriceBookService>();
            services.AddSingleton<ISalesOrderService, SalesOrderService>();
            services.AddSingleton<ITerritoryService, TerritoryService>();
            services.AddSingleton<ITrophiesAndBadgesService, TrophiesAndBadgesService>();
            services.AddSingleton<IAttachmentService, AttachmentService>();
            services.AddSingleton<INoteServices, NoteServices>();
            services.AddSingleton<IContactService, ContactService>();
            services.AddSingleton<IWidgetsService, WidgetsService>();
            services.AddSingleton<ILayoutService, LayoutService>();
            services.AddSingleton<ICommonService, CommonService>();
            services.AddSingleton<IItemService, ItemService>();
            services.AddSingleton<IInvoiceService, InvoiceService>();
            services.AddSingleton<IGoodInwardsService, GoodInwardsService>();
            services.AddSingleton<IUnifiedService, UnifiedService>();
            services.AddSingleton<ITaxService, TaxService>();
            services.AddConsulConfig(Configuration);
            services.AddSingleton<IDevicesService, Devices>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "CRM API V1");
            });
            
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); 
            app.UseAuthentication();
            app.UseConsul(Configuration);
            
            //for idenity server token
            app.UseMiddleware<AuthenticationMiddleware>();

           
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "CRM API V1");
            });           
        }
    }
}
