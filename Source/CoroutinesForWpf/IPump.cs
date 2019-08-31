using System;

namespace CoroutinesForWpf
{
    public interface IPump : IDisposable
    {
        event Action NextFrame;
    }
}