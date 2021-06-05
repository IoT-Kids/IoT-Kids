using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IoT_Kids.ModelMappers;
using IoT_Kids.Repositories.IRepositories;
using IoT_Kids.Repositories.IRepositories.IMembers;
using IoT_Kids.Repositories.IRepositories.IMembershipPlans;
using IoT_Kids.Repositories.IRepositories.IUsers;
using IoT_Kids.Repositories.IRepositories.Payments;
using IoT_Kids.Repositories.Members;
using IoT_Kids.Repositories.Payments;
using IoT_Kids.Repositories.MembershipPlans;
using IoT_Kids.Repositories.MembershipTrans;
using IoT_Kids.Repositories.Users;
using IoT_Kids.ScheduledTasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using IoT_Kids.Repositories.IRepositories.IMembershipTrans;

namespace IoT_Kids
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IMembershipPlanRepo, MembershipPlanRepo>();
            services.AddScoped<IMemberRepo, MemberRepo>();
            services.AddScoped<ICouponRepo, CouponRepo>();
            services.AddScoped<IPaymentRepo, PaymentRepo>();
            services.AddScoped<IMembershipTranRepo, MembershipTranRepo>();
            services.AddScoped<IUserMembershipLogRepo, UserMembershipLogRepo>();
            services.AddAutoMapper(typeof(ModelMapping));
            services.AddHostedService<SetExpiredMemberStatus>();
            services.AddScoped<IScTask, ScTask>();
            // services.AddScoped<ILogger>();

            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("IoTKidsOpenAPI", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "IoT Kids Open API",
                    Version = "1" // explain later
                });

                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(cmlCommentsFullPath);
            });

            // this service added because it solve this error
            //" A possible object cycle was detected which is not supported."
            // and we added a package NewtonSoftJson to help us solve the error
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseSwagger();

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("https://localhost:44352/swagger/IoTKidsOpenAPI/swagger.json", "IoT Kids Open API");
                x.RoutePrefix = "";
            });
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
