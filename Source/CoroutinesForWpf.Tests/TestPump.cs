using System;

namespace CoroutinesForWpf.Tests
{
    public class TestPump : IPump
    {
        public event Action NextFrame;

        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public void Advance()
        {
            NextFrame?.Invoke();
        }
    }
}
