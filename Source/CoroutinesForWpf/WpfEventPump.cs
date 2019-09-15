using System;
using System.Windows.Media;
using CoroutinesDotNet;

namespace CoroutinesForWpf
{
    public class WpfEventPump : IPump
    {
        private bool _busy;

        public WpfEventPump()
        {
            CompositionTarget.Rendering += DispatchNextFrame;
        }

        public event Action NextFrame;

        public void Dispose()
        {
            CompositionTarget.Rendering -= DispatchNextFrame;
        }

        private void DispatchNextFrame(object sender, EventArgs args)
        {
            // naive solution for skipping frames if previous execution takes to long
            // TODO: what happens if there are more than one coroutine running... maybe there should be a busy flag for each item in invocation list
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
        }
    }
}