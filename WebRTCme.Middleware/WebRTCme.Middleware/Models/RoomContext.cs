﻿using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using WebRTCme;
using WebRTCme.Middleware;

namespace WebRtcMeMiddleware
{
    public class RoomContext
    {
        public RoomState RoomState { get; set; }
        public RoomRequestParameters RoomRequestParameters { get; set; }

        public Dictionary<string /*peerUserName*/, IRTCPeerConnection> PeerConnections { get; set; } = new();

        public RTCIceServer[] IceServers { get; set; }

        public TaskCompletionSource<IMediaStream> ConnectTcs { get; set; } = new();

        public TaskCompletionSource<Unit> DisconnectTcs { get; set; } = new();
    }
}
