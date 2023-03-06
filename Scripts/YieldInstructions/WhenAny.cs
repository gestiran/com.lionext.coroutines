using System.Collections;

namespace Lionext.Coroutines.YieldInstructions {
    public class WhenAny : IEnumerator {
        public object Current => null;
        
        private bool _isComplete;
        private readonly CoroutineSimple[] _coroutines;

        public WhenAny(params CoroutineSimple[] coroutines) => _coroutines = coroutines;

        public bool MoveNext() {
            if (_isComplete) return false;
            
            for (int coroutineId = 0; coroutineId < _coroutines.Length; coroutineId++) {
                if (!_coroutines[coroutineId].isComplete) continue; 
                return false;
            }
            
            return true;
        }

        public void Reset() => _isComplete = true;
    }
}