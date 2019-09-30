using System;
using System.Threading;

namespace CoroutinesDotNet.CustomPumpExample
{
    public class BackgroundThreadEventPump : IPump
    {
        private readonly int _frameIntervalMs;
        private readonly Thread _thread;

        private bool _continue = true;
        private bool _busy;

        public BackgroundThreadEventPump(int frameIntervalMs)
        {
            _frameIntervalMs = frameIntervalMs;
            _thread = new Thread(FramePump);
            _thread.IsBackground = true;
            _thread.Name = "Coroutine thread";
            _thread.Start();
        }

        public event Action NextFrame;

        public void Dispose()
        {
            _continue = false;
            _thread.Join();
        }

        private void FramePump()
        {
            while (_continue)
            {
                if (_busy) return;
                _busy = true;
                try
                {
                    NextFrame?.Invoke();
                }
                finally
                {
                    _busy = false;
                }
                Thread.Sleep(_frameIntervalMs);
            }
        }
    }
}