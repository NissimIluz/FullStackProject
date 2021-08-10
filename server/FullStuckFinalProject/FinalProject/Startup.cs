using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Config;
using ProjectContracts;
using DALContract;
using InfraDAL;
using UserServiceImpl;
using DocumentServiceImpl;
using MarkerServiceImp;
using SharingServiceImp;
using System.Net.WebSockets;
using System.Threading;
using Adapters;
using DynamicLoaderServiceImp;

namespace FinalProject
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
            services.Configure<ConfigOptions>(Configuration.GetSection("MySettings"));

            var lsc = new LoadServices("..\\..\\servicesDLLs");
            lsc.Load(services);
            services.AddControllers();
            services.AddMvc().AddJsonOptions(options =>
            { options.JsonSerializerOptions.IgnoreNullValues = true; });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region usewebsockets
            app.UseWebSockets();
            #endregion
            #region usefunction
            app.Use(async (context, next) =>
                {
                if (context.Request.Path == "/ws")
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        #region MessangerInfraStructure
                        var messanger = app.ApplicationServices.GetService<IMessenger>();
                        var id = context.Request.Query["id"];
                            var webSocketAdapter = new WebSocketAdapter();
                        webSocketAdapter.Socket = webSocket;
                        await messanger.Add(id, webSocketAdapter);
                        //await messanger.Send(id, new MessageBody() { Code =null });
                        #endregion
                        await webSocket.ReceiveAsync(new Memory<byte>(), CancellationToken.None);



                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
                });

            #endregion
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
