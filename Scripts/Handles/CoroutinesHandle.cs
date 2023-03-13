using System.Collections;
using System.Collections.Generic;
using Lionext.GameLoop;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Lionext.Coroutines.Handles {
    public class CoroutinesHandle {
        private readonly List<CoroutineSimple> _coroutines;
        private readonly PlayerLoopSystem _updateSystem;

        public CoroutinesHandle() {
            _coroutines = new List<CoroutineSimple>();
            _updateSystem = GameLoopUtility.CreateSystem<CoroutinesHandle>(UpdateRoutines);
            ConnectToLoop();
        }
        
        public CoroutineSimple StartGlobal(IEnumerator enumerator) {
            CoroutineSimple coroutine = new CoroutineSimple(enumerator, RunningUpdate);
            _coroutines.Add(coroutine);

            return coroutine;
        }

        public CoroutineSimple StartObject<T>(T root, IEnumerator enumerator) where T : MonoBehaviour {
            CoroutineSimple coroutine = StartGlobal(enumerator);
            StartGlobal(MonoObserver(root, coroutine));

            return coroutine;
        }

        public void StopAll() {
            for (int coroutineId = 0; coroutineId < _coroutines.Count; coroutineId++) Stop(_coroutines[coroutineId]);
        }

        public void Stop(CoroutineSimple coroutine) => coroutine.ChangeState(StoppedUpdate);

        public void Pause(CoroutineSimple coroutine) => coroutine.ChangeState(PauseUpdate);

        public void Resume(CoroutineSimple coroutine) => coroutine.ChangeState(RunningUpdate);

        private void UpdateRoutines() {
            for (int coroutineId = _coroutines.Count - 1; coroutineId >= 0; coroutineId--) {
                if (_coroutines[coroutineId].MoveNext()) continue;
                Stop(_coroutines[coroutineId]);
                _coroutines.RemoveAt(coroutineId);
            }
        }

        public void ConnectToLoop() => GameLoopUtility.AddLoop(_updateSystem);

        public void DisconnectFromLoop() => GameLoopUtility.RemoveLoop(_updateSystem);

        private bool RunningUpdate(CoroutineSimple coroutine) {
            if (!coroutine.MoveNextCurrent()) {
                if (coroutine.isEmptyStack) {
                    coroutine.Reset();

                    return false;
                }
                coroutine.PopCurrent();
            } else if (coroutine.Current is IEnumerator current) coroutine.PushCurrent(current);

            return true;
        }

        private bool PauseUpdate(CoroutineSimple coroutine) => true;

        private bool StoppedUpdate(CoroutineSimple coroutine) => false;
        
        private IEnumerator MonoObserver<T>(T observed, CoroutineSimple coroutine) where T : MonoBehaviour {
            while (coroutine.isComplete || observed != null) yield return null;
            Stop(coroutine);
        }
    }
}