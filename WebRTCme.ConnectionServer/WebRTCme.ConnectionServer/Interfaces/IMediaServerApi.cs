﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Utilme;

namespace WebRTCme.ConnectionServer
{
    public interface IMediaServerApi
    {
        Task<Result<Unit>> JoinAsync(Guid id, string name, string room);

        Task<Result<Unit>> LeaveAsync(Guid id);

    }
}
