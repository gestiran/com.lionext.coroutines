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
        [SerializeField] private int _framesCount;
        [SerializeField] private float _rotationSpeed;

        private CoroutineSimple[] _rotations;
        
        private void Start() {
            _rotations = CoroutinesUtility.StartRoutine(RotateUP(), RotateLeft());

            WhenAllTest().StartAsCoroutine();
            WhenAnyTest().StartAsCoroutine();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.X)) {
                _rotations.Pause();
            }

            if (Input.GetKeyDown(KeyCode.C)) {
                _rotations.Resume();
            }
        }

        private IEnumerator RotateUP() {
            for (int i = 0; i < _framesCount; i++) {
                _targetTransform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }
            
            Debug.LogError("RotateLeft - Complete");
        }
        
        private IEnumerator RotateLeft() {
            for (int i = 0; i < _framesCount * 2; i++) {
                _targetTransform.Rotate(Vector3.left, _rotationSpeed * Time.deltaTime);

                yield return new WaitForEndOfFrame();
            }
            
            Debug.LogError("RotateLeft - Complete");
        }

        private IEnumerator WhenAllTest() {
            yield return new WhenAll(_rotations);
            Debug.LogError("WhenAllTest - Complete");
        }
        
        private IEnumerator WhenAnyTest() {
            yield return new WhenAny(_rotations);
            Debug.LogError("WhenAnyTest - Complete");
        }
    }
}