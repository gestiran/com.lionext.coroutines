using System;
using System.Collections;
using System.Collections.Generic;
using Lionext.Coroutines.Handles;

namespace Lionext.Coroutines {
    public class CoroutineSimple : IEquatable<CoroutineSimple> {
        private IEnumerator _enumerator;
        private Stack<IEnumerator> _stack;
        private CoroutineUpdateProcess _state;
        
        private readonly int _hash;

        public CoroutineSimple(IEnumerator enumerator, CoroutineUpdateProcess state) {
            _hash = enumerator.GetHashCode();
            _enumerator = enumerator;
            _state = state;
            _stack = new Stack<IEnumerator>();
        }

        public bool TryUpdate() => _state(ref _stack, ref _enumerator);

        public void ChangeState(CoroutineUpdateProcess state) => _state = state;

        public bool Equals(CoroutineSimple other) => other != null && _hash.Equals(other._hash);
        
        public override bool Equals(object obj) => obj is CoroutineSimple other && _hash.Equals(other._hash);

        public override int GetHashCode() => _hash;
    }
}