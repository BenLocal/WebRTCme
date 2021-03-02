﻿using Android.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebRTCme;

namespace WebRtcMe.Android
{
    internal class Window : ApiBase, IWindow
    {
        public static IWindow Create() => new Window();

        public INavigator Navigator() => Android.Navigator.Create();

        public IMediaStream MediaStream() => Android.MediaStream.Create();

        public IRTCPeerConnection RTCPeerConnection(RTCConfiguration configuration) =>
            Android.RTCPeerConnection.Create(configuration);

    }
}
