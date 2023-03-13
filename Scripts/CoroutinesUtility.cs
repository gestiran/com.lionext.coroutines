    using System.Collections;
    using Lionext.Coroutines.Handles;
    using UnityEngine;

    namespace Lionext.Coroutines {
        public static class CoroutinesUtility {
            private static readonly CoroutinesHandle _handle;
            
            static CoroutinesUtility() {
                _handle = new CoroutinesHandle();
                _handle.ConnectToLoop();
            #if UNITY_EDITOR
                Application.quitting += OnQuit;
            #endif
            }

            public static CoroutineSimple[] StartGlobal(params IEnumerator[] enumerators) {
                CoroutineSimple[] result = new CoroutineSimple[enumerators.Length];

                for (int enumeratorId = 0; enumeratorId < enumerators.Length; enumeratorId++) {
                    result[enumeratorId] = _handle.StartGlobal(enumerators[enumeratorId]);
                }
                
                return result;
            }

            public static CoroutineSimple StartGlobal(IEnumerator enumerator) {
                return _handle.StartGlobal(enumerator);
            }

            public static CoroutineSimple[] StartObject<T>(T root, params IEnumerator[] enumerators) where T : MonoBehaviour {
                CoroutineSimple[] result = new CoroutineSimple[enumerators.Length];

                for (int enumeratorId = 0; enumeratorId < enumerators.Length; enumeratorId++) {
                    result[enumeratorId] = _handle.StartObject(root, enumerators[enumeratorId]);
                }

                return result;
            }

            public static CoroutineSimple StartObject<T>(T root, IEnumerator enumerator) where T : MonoBehaviour {
                return _handle.StartObject(root, enumerator);
            }
            
            public static void StopAll() => _handle.StopAll();

            public static void Stop(params CoroutineSimple[] coroutines) {
                for (int coroutineId = 0; coroutineId < coroutines.Length; coroutineId++) {
                    _handle.Stop(coroutines[coroutineId]);
                }
            }

            public static void Stop(CoroutineSimple coroutine) => _handle.Stop(coroutine);

            public static void Pause(params CoroutineSimple[] coroutines) {
                for (int coroutineId = 0; coroutineId < coroutines.Length; coroutineId++) {
                    _handle.Pause(coroutines[coroutineId]);
                }
            }

            public static void Pause(CoroutineSimple coroutine) => _handle.Pause(coroutine);

            public static void Resume(params CoroutineSimple[] coroutines) {
                for (int coroutineId = 0; coroutineId < coroutines.Length; coroutineId++) {
                    _handle.Resume(coroutines[coroutineId]);
                }
            }

            public static void Resume(CoroutineSimple coroutine) => _handle.Resume(coroutine);

        #if UNITY_EDITOR
            
            private static void OnQuit() {
                _handle.StopAll();
                _handle.DisconnectFromLoop();
            }
            
        #endif
        }
    }