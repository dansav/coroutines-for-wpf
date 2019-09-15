using System;
using System.Collections;

namespace CoroutinesDotNet
{
    public class CoroutineRunner : IDisposable
    {
        private readonly IPump _pump;

        private EnumeratorNode _currentNode;

        public CoroutineRunner(IEnumerator routine, IPump pump)
        {
            _pump = pump;
            _currentNode = new EnumeratorNode(routine);
            _pump.NextFrame += OnNextFrame;
        }

        public void Dispose()
        {
            _pump.NextFrame -= OnNextFrame;
        }

        private void OnNextFrame()
        {
            if (_currentNode.MoveNext())
            {
                if (_currentNode.Current is IEnumerator e)
                {
                    _currentNode = new EnumeratorNode(e, _currentNode);
                    OnNextFrame();
                }
            }
            else if (_currentNode.Parent != null)
            {
                _currentNode = _currentNode.Parent;
                OnNextFrame();
            }
            else
            {
                _pump.NextFrame -= OnNextFrame;
            }
        }

        private class EnumeratorNode
        {
            private readonly IEnumerator _enumerator;
            public EnumeratorNode(IEnumerator enumerator, EnumeratorNode parent = null)
            {
                _enumerator = enumerator;
                Parent = parent;
            }

            public EnumeratorNode Parent { get; }

            public object Current => _enumerator.Current;

            public bool MoveNext() => _enumerator.MoveNext();
        }
    }
}