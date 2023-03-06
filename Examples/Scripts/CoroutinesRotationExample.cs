using System.Collections;
using Lionext.Coroutines;
using Lionext.Coroutines.Extensions;
using Lionext.Coroutines.YieldInstructions;
using UnityEngine;

namespace LionextExample.Coroutines {
    public class CoroutinesRotationExample : MonoBehaviour {
        [Header("Links")]
        [SerializeField] private Transform _targetTransform;

        [Header("Parameters")]
        [SerializeField] private int _count;
        [SerializeField] private int _framesCount;
        [SerializeField] private float _rotationSpeed;

        private CoroutineSimple[] _rotations;
        
        private void Start() {
            _rotations = new CoroutineSimple[_count];
            
            for (int i = 0; i < _count; i++) {
                _rotations[i] = Test().StartAsRoutine();
            }
            //_rotations = RoutineRunner.StartRoutine(RotateUP(), RotateLeft());
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.X)) {
                _rotations.Pause();
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                _rotations.Resume();
            }
        }

        private IEnumerator Test() {
            WaitForEndOfFrame frame = new WaitForEndOfFrame();
            
            while (Application.isPlaying) {
                yield return frame;
            }
        }

        private IEnumerator RotateUP() {
            for (int i = 0; i < _framesCount; i++) {
                _targetTransform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }
        }
        
        private IEnumerator RotateLeft() {
            for (int i = 0; i < _framesCount * 2; i++) {
                _targetTransform.Rotate(Vector3.left, _rotationSpeed * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator Wait() {
            yield return new WhenAll(_rotations);
            Debug.LogError("Complete");
        }
    }
}