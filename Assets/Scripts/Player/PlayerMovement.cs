using UnityEngine;

namespace Balthazariy.ArenaBattle.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 4.0f;
        [SerializeField] private float _sprintSpeed = 6.0f;
        [SerializeField] private float _rotationSpeed = 1.0f;
        [SerializeField] private float _speedChangeRate = 10.0f;

        [SerializeField] private CharacterController _characterController;

        private Vector3 _direction;

        private bool _isSprint;
        private float _currentSpeed;
        private bool _isAnalogMovement;

        private void Update()
        {
            var x = Input.GetAxis("Horizontal");
            var z = Input.GetAxis("Vertical");
            _direction = new Vector3(x, 0, z);

            Move();
        }


        private void FixedUpdate()
        {
        }

        private void Move()
        {
            float targetSpeed = _isSprint ? _sprintSpeed : _moveSpeed;

            if (_direction == Vector3.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_characterController.velocity.x, 0.0f, _characterController.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _isAnalogMovement ? _direction.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _currentSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * _speedChangeRate);

                _currentSpeed = Mathf.Round(_currentSpeed * 1000f) / 1000f;
            }
            else
            {
                _currentSpeed = targetSpeed;
            }

            Vector3 inputDirection = new Vector3(_direction.x, 0.0f, _direction.z).normalized;

            if (_direction != Vector3.zero)
            {
                inputDirection = transform.right * _direction.x + transform.forward * _direction.z;
            }

            _characterController.Move(inputDirection.normalized * (_currentSpeed * Time.deltaTime) + new Vector3(0.0f, 0.0f, 0.0f) * Time.deltaTime);
        }
    }
}