using System.Collections;

namespace Lionext.Coroutines.YieldInstructions {
    public class WhenAny : IEnumerator {
        public object Current => null;
        
        private bool _isEnd;
        private readonly CoroutineSimple[] _routines;

        public WhenAny(params CoroutineSimple[] routines) => _routines = routines;

        public bool MoveNext() {
            if (_isEnd) return false;
            
            // for (int routineId = 0; routineId < _routines.Length; routineId++) {
            //     if (!_routines[routineId].isRunning) return false;
            // }
            
            return true;
        }

        public void Reset() => _isEnd = true;
    }
}