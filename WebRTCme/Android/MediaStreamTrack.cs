﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebRTCme;
using Webrtc = Org.Webrtc;

namespace WebRtc.Android
{
    internal class MediaStreamTrack : ApiBase, IMediaStreamTrack
    {
        public static IMediaStreamTrack Create(MediaStreamTrackKind mediaStreamTrackKind, string id)
        {
            throw new NotImplementedException();
        }

        public static IMediaStreamTrack Create(Webrtc.MediaStreamTrack nativeMediaStreamTrack)
        {
            return new MediaStreamTrack(nativeMediaStreamTrack);
        }

        private MediaStreamTrack(Webrtc.MediaStreamTrack nativeMediaStreamTrack) : base(nativeMediaStreamTrack)
        { }

        public string ContentHint { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Enabled { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Id => throw new NotImplementedException();

        public bool Isolated => throw new NotImplementedException();

        public MediaStreamTrackKind Kind => throw new NotImplementedException();

        public string Label => throw new NotImplementedException();

        public bool Muted => throw new NotImplementedException();

        public bool Readonly => throw new NotImplementedException();

        public MediaStreamTrackState ReadyState => throw new NotImplementedException();

        public bool Remote => throw new NotImplementedException();

        public event EventHandler OnEnded;
        public event EventHandler OnMute;
        public event EventHandler OnUnmute;

        public Task ApplyConstraints(MediaTrackConstraints contraints)
        {
            throw new NotImplementedException();
        }

        public MediaTrackCapabilities GetCapabilities()
        {
            throw new NotImplementedException();
        }

        public MediaTrackConstraints GetContraints()
        {
            throw new NotImplementedException();
        }

        public MediaTrackSettings GetSettings()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        IMediaStreamTrack IMediaStreamTrack.Clone()
        {
            throw new NotImplementedException();
        }
    }
}
