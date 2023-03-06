    using System.Collections;
    using System.Collections.Generic;
    using Lionext.Coroutines.Handles;
    using UnityEngine;
    using UnityEngine.LowLevel;

    namespace Lionext.Coroutines {
        public static class CoroutinesUtility {
            private static readonly CoroutineHandle _handle;
            private static readonly List<CoroutineSimple> _routines;
            
            static CoroutinesUtility() {
                _handle = new CoroutineHandle();
                _routines = new List<CoroutineSimple>();
                ConnectToLoop();

            #if UNITY_EDITOR
                Application.quitting += OnQuit;
            #endif
            }
            
            // Need separate to scene routines and global routines
            // Need editor realisation

            public static CoroutineSimple StartRoutine<T>(T enumerator) where T : IEnumerator {
                CoroutineSimple routine = new CoroutineSimple(enumerator, _handle.Running);
                _routines.Add(routine);
                return routine;
            }
            
            public static CoroutineSimple[] StartRoutine<T>(params T[] enumerators) where T : IEnumerator {
                CoroutineSimple[] result = new CoroutineSimple[enumerators.Length];
                for (int routineId = 0; routineId < enumerators.Length; routineId++) result[routineId] = StartRoutine(enumerators[routineId]);
                return result;
            }

            public static void StopAll() => StopRoutine(_routines.ToArray());
            
            public static void StopRoutine<T>(params T[] routines) where T : CoroutineSimple {
                for (int routineId = 0; routineId < routines.Length; routineId++) StopRoutine(routines[routineId]);
            }
            
            public static void StopRoutine<T>(T routine) where T : CoroutineSimple => routine.ChangeState(_handle.Stopped);
            
            public static void Pause(params CoroutineSimple[] routines) {
                for (int routineId = 0; routineId < _routines.Count; routineId++) Pause(_routines[routineId]);
            }
            
            public static void Pause(CoroutineSimple routine) => routine.ChangeState(_handle.Pause);

            public static void Resume(params CoroutineSimple[] routines) {
                for (int routineId = 0; routineId < routines.Length; routineId++) _routines[routineId].ChangeState(_handle.Running);
            }

            private static void Update() => UpdateRoutines();

            private static void UpdateRoutines() {
                for (int routineId = _routines.Count - 1; routineId >= 0; routineId--) {
                    if (_routines[routineId].UpdateState()) continue;
                    StopRoutine(_routines[routineId]);
                    _routines.RemoveAt(routineId);
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