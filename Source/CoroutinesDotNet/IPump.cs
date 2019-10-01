using System;

namespace CoroutinesDotNet
{
    public interface IPump : IDisposable
    {
        event Action NextFrame;
    }
}