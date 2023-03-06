using System.Collections;
using UnityEngine;

namespace Lionext.Coroutines.YieldInstructions {
    public class WaitRealtime : IEnumerator {
        public object Current => null;
        private float _endTime;

        /// <summary> Creates a yield instruction to wait for a given number of seconds using unscaled time. </summary>
        public WaitRealtime(float time) => _endTime = Time.fixedTime + time * Time.timeScale;

        public bool MoveNext() => Time.fixedTime < _endTime;

        public void Reset() => _endTime = -1f;
    }
}