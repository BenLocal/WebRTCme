﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebRtcBindingsWeb.Extensions;
using WebRtcBindingsWeb.Interops;
using WebRTCme;

namespace WebRtcBindingsWeb.Api
{
    public abstract class ApiBase : INativeObject
    {
        protected ApiBase(IJSRuntime jsRuntime, JsObjectRef jsObjectRef = null)
        {
            JsRuntime = jsRuntime;
            NativeObject = jsObjectRef;
        }

        public IJSRuntime JsRuntime { get; }
        
        private object _nativeObject;
        public object NativeObject// { get; protected set; }
        {
            get => _nativeObject;
            set
            { 
                _nativeObject = value;
                JsObjects.Add(value as JsObjectRef);
            }
        }

        public List<JsObjectRef> JsObjects { get; set; } = new List<JsObjectRef>();

        public List<IDisposable> JsEvents { get; set; } = new List<IDisposable>();
        

        public void Dispose()
        {
            foreach (var jsEvent in JsEvents)
            {
                jsEvent.Dispose();
            }
            foreach (var jsObjectRef in JsObjects)
            {
                JsRuntime.DeleteJsObjectRef(jsObjectRef.JsObjectRefId);
            }

        }

        protected void AddNativeEventListener(string eventName, EventHandler eventHandler)
        {
            JsEvents.Add(JsRuntime.AddJsEventListener(NativeObject as JsObjectRef, null, eventName,
                JsEventHandler.Create(() =>
                {
                    eventHandler?.Invoke(this, EventArgs.Empty);
                },
                null, false)));
        }

        protected void AddNativeEventListener<T>(string eventName, EventHandler<T> eventHandler)
        {
            JsEvents.Add(JsRuntime.AddJsEventListener(NativeObject as JsObjectRef, null, eventName,
                JsEventHandler.Create<T>(e =>
                {
                    eventHandler?.Invoke(this, e);
                },
                null, false)));
        }

        protected T GetNativeProperty<T>(string propertyName) => JsRuntime.GetJsPropertyValue<T>(
            NativeObject, propertyName);

        protected void SetNativeProperty(string propertyName, object value) => JsRuntime.SetJsProperty(
            NativeObject, propertyName, value);
    }
}
