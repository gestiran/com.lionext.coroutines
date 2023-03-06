using System.Collections;

namespace Lionext.Coroutines.YieldInstructions {
    public class WhenAll : IEnumerator {
        public object Current => null;
        
        private bool _isComplete;
        private readonly CoroutineSimple[] _coroutine;

        public WhenAll(params CoroutineSimple[] coroutine) => _coroutine = coroutine;

        public bool MoveNext() {
            if (_isComplete) return false;
            
            for (int routineId = 0; routineId < _coroutine.Length; routineId++) {
                if (!_coroutine[routineId].isComplete) return true;
            }
            
            return false;
        }

        public void Reset() => _isComplete = true;
    }
}