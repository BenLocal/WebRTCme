﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebRTCme.ConnectionServer
{
    public class IceCandidate
    {
        public string Foundation { get; init; }
        public int Priority { get; init; }
        public string Ip { get; init; }
        public RTCIceProtocol Protocol { get; init; }
        public int Port { get; init; }
        public RTCIceCandidateType Type { get; init; }
        public RTCIceTcpCandidateType? TcpType { get; init; } 
    }
}
