    using System.Collections;
    using System.Collections.Generic;
    using Lionext.Coroutines.Handles;
    using UnityEngine;
    using UnityEngine.LowLevel;

    namespace Lionext.Coroutines {
        public static class CoroutinesUtility {
            private static readonly CoroutineHandle _handle;
            private static readonly List<CoroutineSimple> _coroutines;
            
            static CoroutinesUtility() {
                _handle = new CoroutineHandle();
                _coroutines = new List<CoroutineSimple>();
                ConnectToLoop();

            #if UNITY_EDITOR
                Application.quitting += OnQuit;
            #endif
            }
            
            // Need separate to scene routines and global routines
            // Need editor realisation

            public static CoroutineSimple[] StartGlobalCoroutine(params IEnumerator[] enumerators) {
                CoroutineSimple[] result = new CoroutineSimple[enumerators.Length];

                for (int enumeratorId = 0; enumeratorId < enumerators.Length; enumeratorId++) {
                    result[enumeratorId] = StartGlobalCoroutine(enumerators[enumeratorId]);
                }
                
                return result;
            }

            public static CoroutineSimple StartGlobalCoroutine(IEnumerator enumerator) {
                CoroutineSimple routine = new CoroutineSimple(enumerator, _handle.Running);
                _coroutines.Add(routine);
                return routine;
            }

            public static CoroutineSimple[] StartObjectCoroutine<T>(T root, params IEnumerator[] enumerators) where T : MonoBehaviour {
                CoroutineSimple[] result = new CoroutineSimple[enumerators.Length];
                
                for (int enumeratorId = 0; enumeratorId < enumerators.Length; enumeratorId++) {
                    result[enumeratorId] = StartObjectCoroutine(root, enumerators[enumeratorId]);
                }
                
                return result;
            }

            public static CoroutineSimple StartObjectCoroutine<T>(T root, IEnumerator enumerator) where T : MonoBehaviour {
                CoroutineSimple coroutine = StartGlobalCoroutine(enumerator);
                StartGlobalCoroutine(MonoObserver(root, coroutine));
                return coroutine;
            }

            private static IEnumerator MonoObserver<T>(T observed, CoroutineSimple coroutine) where T : MonoBehaviour {
                while (coroutine.isComplete || observed != null) yield return null;
                StopCoroutine(coroutine);
            }
            
            public static void StopAll() => StopCoroutine(_coroutines.ToArray());
            
            public static void StopCoroutine<T>(params T[] coroutines) where T : CoroutineSimple {
                for (int coroutineId = 0; coroutineId < coroutines.Length; coroutineId++) StopCoroutine(coroutines[coroutineId]);
            }
            
            public static void StopCoroutine<T>(T routine) where T : CoroutineSimple => routine.ChangeState(_handle.Stopped);
            
            public static void Pause(params CoroutineSimple[] routines) {
                for (int routineId = 0; routineId < _coroutines.Count; routineId++) Pause(_coroutines[routineId]);
            }
            
            public static void Pause(CoroutineSimple routine) => routine.ChangeState(_handle.Pause);

            public static void Resume(params CoroutineSimple[] routines) {
                for (int routineId = 0; routineId < routines.Length; routineId++) _coroutines[routineId].ChangeState(_handle.Running);
            }

            private static void Update() => UpdateRoutines();

            private static void UpdateRoutines() {
                for (int coroutineId = _coroutines.Count - 1; coroutineId >= 0; coroutineId--) {
                    if (_coroutines[coroutineId].UpdateState()) continue;
                    StopCoroutine(_coroutines[coroutineId]);
                    _coroutines.RemoveAt(coroutineId);
                }
            }

            private static void ConnectToLoop() {
                PlayerLoopSystem currentLoop = PlayerLoop.GetCurrentPlayerLoop();
                PlayerLoopSystem[] currentSystems = currentLoop.subSystemList;

                PlayerLoopSystem[] newSystems = new PlayerLoopSystem[currentSystems.Length + 1];

                for (int systemId = 0; systemId < currentSystems.Length; systemId++) newSystems[systemId] = currentSystems[systemId];

                newSystems[currentSystems.Length] = CreateCurrentSystem();

                currentLoop.subSystemList = newSystems;
                PlayerLoop.SetPlayerLoop(currentLoop);
            }

        #if UNITY_EDITOR
            
            private static void OnQuit() {
                StopAll();
                DisconnectFromLoop();
            }
            
            private static void DisconnectFromLoop() {
                PlayerLoopSystem currentLoop = PlayerLoop.GetCurrentPlayerLoop();
                PlayerLoopSystem[] currentSystems = currentLoop.subSystemList;

                PlayerLoopSystem[] newSystems = new PlayerLoopSystem[currentSystems.Length - 1];

                for (int newSystemId = 0, systemId = 0; newSystemId < newSystems.Length; newSystemId++, systemId++) {
                    if (currentSystems[systemId].type == typeof(PlayerLoopSystem)) {
                        systemId++;
                        continue;
                    }

                    newSystems[newSystemId] = currentSystems[systemId];
                }
                
                currentLoop.subSystemList = newSystems;
                PlayerLoop.SetPlayerLoop(currentLoop);
            }
            
        #endif

            private static PlayerLoopSystem CreateCurrentSystem() {
                PlayerLoopSystem system = new PlayerLoopSystem();

                system.type = typeof(PlayerLoopSystem);
                system.updateDelegate = Update;

                return system;
            }
        }
    }