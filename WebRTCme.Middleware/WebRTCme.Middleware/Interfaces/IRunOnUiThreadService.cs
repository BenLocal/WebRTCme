﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebRTCme.Middleware
{
    public interface IRunOnUiThreadService
    {
        void Invoke(Action action);
    }
}
