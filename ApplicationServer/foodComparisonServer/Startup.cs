using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace foodComparisonServer
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region FETCH ENVIRONMENT WISE JWT DETAILS
            string JWTIssuer = Configuration["JWTSetting:Issuer"];
            string JWTAudiance = Configuration["JWTSetting:Audience"];
            string JWTSecret = Configuration["JWTSetting:Key"];
            string JWTExpiry = Configuration["JWTSetting:ExpiryInMins"];
            #endregion

            //var key = Encoding.ASCII.GetBytes(JWTSecret);
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = true,
            //        ValidIssuer = JWTIssuer,
            //        ValidateAudience = true,
            //        ValidAudience = JWTAudiance,
            //        ValidateLifetime = true
            //    };
            //});

            // services.AddMvc(options => {
            //     options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            // });
            services.AddMvc();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                //options.InvalidModelStateResponseFactory = actionContext =>
                //{
                    //ApplicationResponseModel ObjResponse = new ApplicationResponseModel();
                    //ObjResponse.Response = -1;
                    //ObjResponse.Data = actionContext.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    //ObjResponse.Code = "VALIDATION_ERRORS";
                    //return new BadRequestObjectResult(ObjResponse);
                //};
            });
            // // Register the Swagger generator, defining 1 or more Swagger documents
            //services.AddApiVersioning(v =>
            //{
            //    v.ReportApiVersions = true;
            //    v.AssumeDefaultVersionWhenUnspecified = true;
            //    v.DefaultApiVersion = new ApiVersion(1, 0);
            //    v.ApiVersionReader = new HeaderApiVersionReader("x-api-version"); //HTTP Header-Based Versioning
            //});
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Template APIs", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {

            //FORWARD HEADERS
            #region 
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                //ForwardedHeaders = ForwardedHeaders.All,
                RequireHeaderSymmetry = false,
                ForwardLimit = null
            });
            #endregion

            // CORS POLICY
            #region
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            #endregion

            // SECURITY HEADERS
            #region
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=86400; includeSubDomains");
                context.Response.Headers.Add("Server", "-");
                context.Response.Headers.Add
                (
                    "Content-Security-Policy",
                    //"default-src 'self'; " +
                    "img-src 'self' myblobacc.blob.core.windows.net; " +
                    "font-src 'self'; " +
                    // "style-src 'self' 'unsafe-inline'; " +
                    // "script-src 'self' 'unsafe-inline' 'nonce-KIBdfgEKjb34ueiw567bfkshbvfi4KhtIUE3IWF' " +
                    // " 'nonce-rewgljnOIBU3iu2btli4tbllwwe'; " +
                    "frame-src 'self';" +
                    "connect-src 'self';"
                );

                await next();
            });
            #endregion

            //EXCEPTION HANDLING
            #region
            if (HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //LOG EXCEPTION IN DB
                app.UseDeveloperExceptionPage();
            }
            #endregion

            //MIDDLEWARES
            #region
            //app.UseMiddleware<MiddlewareAntiXss>();
            //app.UseMiddleware<MiddlewareHeaderValidation>();
            #endregion

            //SWAGGER
            #region 
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
           //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
            //});
            #endregion

            //MODULES & FEATURES
            #region
            app.UseAuthentication();
            DefaultFilesOptions _FileOptions = new DefaultFilesOptions();
            _FileOptions.DefaultFileNames.Clear();
            _FileOptions.DefaultFileNames.Add("index.html");
            app.UseDefaultFiles(_FileOptions)
            .UseStaticFiles()
            .UseMvc();
            #endregion
        }
    }
}
