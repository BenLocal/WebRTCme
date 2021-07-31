﻿using UtilmeSdpTransform;
using System;
using System.Collections.Generic;
using System.Text;
using WebRTCme.Connection.MediaSoup;
using Utilme.SdpTransform;
using System.Linq;

namespace WebRTCme.Connection.MediaSoup.Proxy.Client.Sdp
{
    static class CommonUtils
    {
        public static RtpCapabilities ExtractRtpCapabilities(Utilme.SdpTransform.Sdp sdp)
        {
            Dictionary<int, RtpCodecCapability> codecsDictionary = new();
            List<RtpHeaderExtension> headerExtensions = new();

            bool gotAudio = false;
            bool gotVideo = false;

            foreach (var m in sdp.MediaDescriptions)
            {
                var kind = m.Media;
                MediaKind mediaKind = MediaKind.Video;
                switch (kind)
                {
                    case "audio":
                        mediaKind = MediaKind.Audio;
                        if (gotAudio)
                            continue;
                        gotAudio = true;
                        break;
                    case "video":
                        mediaKind = MediaKind.Video;
                        if (gotVideo)
                            continue;
                        gotVideo = true;
                        break;
                    default:
                        continue;
                }

                var rtpmapAttributes = MediaDescriptionParser.ToRtpmapAttributes(m);
                foreach (var rtpmap in rtpmapAttributes)
                {
                    var codec = new RtpCodecCapability 
                    {
                        Kind = mediaKind,
                        MimeType = $"{kind}/{rtpmap.EncodingName}",
                        PreferredPayloadType = rtpmap.PayloadType,
                        ClockRate = rtpmap.ClockRate,
                        Channels = rtpmap.Channels 
                    };
                    codecsDictionary.Add(codec.PreferredPayloadType, codec);
                }

                var fmtpAttributes = MediaDescriptionParser.ToFmtpAttributes(m);
                foreach (var fmtp in fmtpAttributes)
                {
                    if (!codecsDictionary.TryGetValue(fmtp.PayloadType, out var codec))
                        continue;
                    SetMediaDescriptorParametersObject(codec, fmtp.Value);
                }

                var rtcpFbAttributes = MediaDescriptionParser.ToRtcpFbAttributes(m);
                foreach (var rtcpFb in rtcpFbAttributes)
                {
                    if (!codecsDictionary.TryGetValue(rtcpFb.PayloadType, out var codec))
                        continue;
                    RtcpFeedback feedback = new() 
                    {
                        Type = rtcpFb.Type,
                        Parameter = rtcpFb.SubType
                    };
                    codec.RtcpFeedback = new RtcpFeedback[] { feedback };
                }

                var extmapAttributes = MediaDescriptionParser.ToExtmapAttributes(m);
                foreach (var extmap in extmapAttributes)
                {
                    RtpHeaderExtension headerExtension = new()
                    {
                        Kind = mediaKind,
                        Uri = extmap.Uri,
                        PreferedId = extmap.Value
                    };
                    headerExtensions.Add(headerExtension);
                }
            }

            RtpCapabilities rtpCapabilities = new()
            {
                Codecs = codecsDictionary.Values.ToArray(),
                HeaderExtensions = headerExtensions.ToArray()
            };
            return rtpCapabilities;
        }

        static void SetMediaDescriptorParametersObject(RtpCodecCapability codec, string fmtpValue)
        {
            var tokens = fmtpValue.Split(';');

            if (codec.MimeType.Equals("audio/opus"))
            {
                OpusParameters opus = new();
                foreach (var token in tokens)
                {
                    var kvp = token.Split('=');
                    if (kvp[0].Equals("useinbandfec"))
                        opus.UseInbandFec = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("usedtx"))
                        opus.UsedTx = int.Parse(kvp[1]);
                }
                codec.Parameters = opus;
            }
            if (codec.MimeType.Equals("video/H264"))
            {
                H264Parameters h264 = new();
                foreach (var token in tokens)
                {
                    var kvp = token.Split('=');
                    if (kvp[0].Equals("packetization-mode"))
                        h264.PacketizationMode = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("profile-level-id"))
                        h264.ProfileLevelId = kvp[1];
                    else if (kvp[0].Equals("level-asymmetry-allowed"))
                        h264.LevelAsymmetryAllowed = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("x-google-start-bitrate"))
                        h264.XGoogleStartBitrate = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("x-google-max-bitrate"))
                        h264.XGoogleMaxBitrate = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("x-google-min-bitrate"))
                        h264.XGoogleMinBitrate = int.Parse(kvp[1]);
                }
                codec.Parameters = h264;
            }
            else if (codec.MimeType.Equals("video/VP8"))
            {
                VP8Parameters vp8 = new();
                foreach (var token in tokens)
                {
                    var kvp = token.Split('=');
                    if (kvp[0].Equals("profile-id"))
                        vp8.ProfileId = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("x-google-start-bitrate"))
                        vp8.XGoogleStartBitrate = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("x-google-max-bitrate"))
                        vp8.XGoogleMaxBitrate = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("x-google-min-bitrate"))
                        vp8.XGoogleMinBitrate = int.Parse(kvp[1]);
                }
                codec.Parameters = vp8;
            }
            else if (codec.MimeType.Equals("video/VP9"))
            {
                VP9Parameters vp9 = new();
                foreach (var token in tokens)
                {
                    var kvp = token.Split('=');
                    if (kvp[0].Equals("profile-id"))
                        vp9.ProfileId = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("x-google-start-bitrate"))
                        vp9.XGoogleStartBitrate = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("x-google-max-bitrate"))
                        vp9.XGoogleMaxBitrate = int.Parse(kvp[1]);
                    else if (kvp[0].Equals("x-google-min-bitrate"))
                        vp9.XGoogleMinBitrate = int.Parse(kvp[1]);
                }
                codec.Parameters = vp9;
            }
            else if (codec.MimeType.Equals("video/rtx"))
            {
                RtxParameters rtx = new();
                foreach (var token in tokens)
                {
                    var kvp = token.Split('=');
                    if (kvp[0].Equals("apt"))
                        rtx.Apt = int.Parse(kvp[1]);
                }
                codec.Parameters = rtx;
            }
            else
                codec.Parameters = null;

        }

        internal static DtlsParameters ExtractDtlsParameters(Utilme.SdpTransform.Sdp sdp)
        {
            var mediaObject = sdp.MediaDescriptions
                .Select(md => SdpMediaDescriptionToMediaObject(md))
                .SingleOrDefault(mo => mo.IceUfrag is not null && mo.Port != 0);
            if (mediaObject is null)
                throw new Exception("No active media section found");

            var fingerprint = mediaObject.Fingerprint ?? 
                sdp.Attributes.FirstOrDefault(a => a.StartsWith(Fingerprint.Name)).ToFingerprint();
            DtlsRole? role = null;

            switch (mediaObject.Setup)
            {
                case "active":
                    role = DtlsRole.Client;
                    break;
                case "passive":
                    role = DtlsRole.Server;
                    break;
                case "actpass":
                    role = DtlsRole.Auto;
                    break;
            }

            return new DtlsParameters 
            {
                DtlsRole = role,
                Fingerprints = new DtlsFingerprint[] 
                { 
                    new DtlsFingerprint
                    {
                        Algorithm = fingerprint.HashFunction.DisplayName(),
                        Value = BitConverter.ToString(fingerprint.HashValue).Replace("-",":")
                    }
                }  
	        };
        }

        public static MediaObject SdpMediaDescriptionToMediaObject(MediaDescription mediaDescription)
        {
            return new MediaObject
            {
                Mid = mediaDescription.Attributes
                    .SingleOrDefault(a => a.StartsWith(Mid.Name))?.ToMid(),
                Msid = mediaDescription.Attributes
                    .SingleOrDefault(a => a.StartsWith(Msid.Name))?.ToMsid(),
                IceUfrag = mediaDescription.Attributes
                    .SingleOrDefault(a => a.StartsWith(IceUfrag.Name))?.ToIceUfrag(),
                IcePwd = mediaDescription.Attributes
                    .SingleOrDefault(a => a.StartsWith(IcePwd.Name))?.ToIcePwd(),
                IceOptions = mediaDescription.Attributes
                        .SingleOrDefault(a => a.StartsWith(IceOptions.Name))?.ToIceOptions(),
                Fingerprint = mediaDescription.Attributes
                        .SingleOrDefault(a => a.StartsWith(Fingerprint.Name))?.ToFingerprint(),
                Candidates = mediaDescription.Attributes
                    .Where(a => a.StartsWith(Candidate.Name))
                    .Select(c => c.ToCandidate())
                    .ToList(),
                Rtpmaps = mediaDescription.Attributes
                    .Where(a => a.StartsWith(Rtpmap.Name))
                    .Select(r => r.ToRtpmap())
                    .ToList(),
                RtcpFbs = mediaDescription.Attributes
                    .Where(a => a.StartsWith(RtcpFb.Name))
                    .Select(r => r.ToRtcpFb())
                    .ToList(),
                Fmtps = mediaDescription.Attributes
                    .Where(a => a.StartsWith(Fmtp.Name))
                    .Select(r => r.ToFmtp())
                    .ToList(),
                Ssrcs = mediaDescription.Attributes
                    .Where(a => a.StartsWith(Ssrc.Name))
                    .Select(r => r.ToSsrc())
                    .ToList(),
                SsrcGroups = mediaDescription.Attributes
                    .Where(a => a.StartsWith(SsrcGroup.Name))
                    .Select(r => r.ToSsrcGroup())
                    .ToList(),
                Rids = mediaDescription.Attributes
                    .Where(a => a.StartsWith(Rid.Name))
                    .Select(r => r.ToRid())
                    .ToList()
            };
        }
    }
}
