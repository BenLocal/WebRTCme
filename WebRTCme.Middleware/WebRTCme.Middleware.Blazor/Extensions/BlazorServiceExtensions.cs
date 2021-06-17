﻿using Blazorme;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WebRTCme.Middleware.Blazor.Services;
using WebRTCme.Middleware.Services;

namespace WebRTCme.Middleware
{
    public static class BlazorServiceExtensions
    {
        public static IServiceCollection AddBlazorMiddleware(this IServiceCollection services)
        {
            services.AddBlazorDialog();

            services.AddSingleton<IModalPopup, ModalPopup>();
            services.AddSingleton<INavigation, Navigation>();
            services.AddSingleton<IRunOnUiThread, RunOnUiThread>();
            services.AddSingleton<IWebRtcIncomingFileStreamFactory, WebRtcIncomingFileStreamFactory>();
            services.AddSingleton<IVideoRecorderFileStreamFactory, VideoRecorderFileStreamFactory>();

            services.AddMiddleware();

            services.AddStreamSaver();

            return services;

        }

    }
}
