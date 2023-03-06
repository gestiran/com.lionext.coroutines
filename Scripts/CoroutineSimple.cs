using System;
using System.Collections;
using System.Collections.Generic;
using Lionext.Coroutines.Handles;

namespace Lionext.Coroutines {
    public class CoroutineSimple : IEnumerator, IEquatable<CoroutineSimple> {
        public bool isEmptyStack => _stack.Count == 0;
        public bool isComplete => _stack.Count == 0 && _enumerator == null;
        public object Current => _enumerator.Current;
        
        private IEnumerator _enumerator;
        private CoroutineUpdateProcess _state;
        
        private readonly Stack<IEnumerator> _stack;
        private readonly int _hash;

        public CoroutineSimple(IEnumerator enumerator, CoroutineUpdateProcess state) {
            _hash = enumerator.GetHashCode();
            _enumerator = enumerator;
            _state = state;
            _stack = new Stack<IEnumerator>();
        }

        public bool UpdateState() => _state(this);

        public void ChangeState(CoroutineUpdateProcess state) => _state = state;

        public void ChangeEnumerator(IEnumerator enumerator) => _enumerator = enumerator;

        public IEnumerator Pop() => _stack.Pop();
        
        public void Push() => _stack.Push(_enumerator);
        
        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset() => _enumerator.Reset();

        public bool Equals(CoroutineSimple other) => other != null && _hash.Equals(other._hash);
        
        public override bool Equals(object obj) => obj is CoroutineSimple other && _hash.Equals(other._hash);

        public override int GetHashCode() => _hash;
    }
}