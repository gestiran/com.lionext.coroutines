using System.Collections;
using UnityEngine;

namespace Lionext.Coroutines.YieldInstructions {
    public class Wait : IEnumerator {
        public object Current => null;
        
        private float _endTime;

        /// <summary> Creates a yield instruction to wait for a given number of seconds using scaled time. </summary>
        public Wait(float time) => _endTime = Time.fixedTime + time;

        public bool MoveNext() => Time.fixedTime < _endTime;

        public void Reset() => _endTime = -1f;
    }
}