using System;
using System.Collections;

namespace CoroutinesForWpf
{
    public partial class Coroutine : IDisposable
    {
        private readonly IPump _pumpInstance;

        private EnumeratorNode _currentNode;

        private Coroutine(IEnumerator routine, IPump pump = null)
        {
            _pumpInstance = pump ?? _pump;
            _currentNode = new EnumeratorNode(routine);
            _pumpInstance.NextFrame += OnNextFrame;
        }

        public void Dispose()
        {
            _pumpInstance.NextFrame -= OnNextFrame;
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
                _pumpInstance.NextFrame -= OnNextFrame;
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