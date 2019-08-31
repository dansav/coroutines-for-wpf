using System;
using System.Windows.Media;

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
            if (_busy) return;
            _busy = true;
            NextFrame?.Invoke();
            _busy = false;
        }
    }
}