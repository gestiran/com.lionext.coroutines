using System.Collections;

namespace Lionext.Coroutines.YieldInstructions {
    public class WhenAll : IEnumerator {
        public object Current => null;
        
        private bool _isEnd;
        private readonly CoroutineSimple[] _routines;

        public WhenAll(params CoroutineSimple[] routines) => _routines = routines;

        public bool MoveNext() {
            if (_isEnd) return false;
            
            // for (int routineId = 0; routineId < _routines.Length; routineId++) {
            //     if (_routines[routineId].isRunning) return true;
            // }
            
            return false;
        }

        public void Reset() => _isEnd = true;
    }
}