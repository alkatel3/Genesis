using UnityEngine;
using Tools;
using Enums;
using Assets.Scripts.Player;
using System;
using Assets.Scripts.Player.PlayerAnimation;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private AnimationController _animator;

        [Header("HorizontalMovement")]
        [SerializeField] private float _walkSpeed;
        [SerializeField] private float _runSpeed;
        [SerializeField] private Direction _direction;
        [Header("VerticalMovement")]
        [SerializeField] private float _verticalSpeed;

        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;



        [SerializeField] private float _minVerticalPosition;
        [SerializeField] private float _maxVerticalPosition;

        [Header("Jump")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _gravityScale;
        [SerializeField] private DirectionalCamerPair _cameras;
        private Rigidbody2D _rigidbody;

        private float _sizeModificator;
        private bool _isJumping;
        private float _startJumpVerticalPoint;
        private float _currentSpeed;

        private Vector2 _movement;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            float positionDifference = _maxVerticalPosition - _minVerticalPosition;
            float sizeDifference = _maxSize - _minSize;
            _sizeModificator = sizeDifference / positionDifference;
            //UpdateSize();
            _rigidbody.position = new Vector2(_rigidbody.position.x, _maxVerticalPosition);
            transform.localScale = Vector2.one * _minSize;
        }

        private void Update()
        {
            if (_isJumping)
                UpdateJump();

            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Walk, _movement.magnitude > 0 );
            _animator.PlayAnimation(AnimationType.Slide, _currentSpeed==_runSpeed);
            _animator.PlayAnimation(AnimationType.Jump, _isJumping);
        }

        public void MoveHorizontally(float direction)
        {
            _currentSpeed = _walkSpeed;
            _movement.x = direction;
            SetDirection(direction);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _currentSpeed;
            _rigidbody.velocity = velocity;
        }

        public void Slide(float direction)
        {
            _currentSpeed = _runSpeed;
            _movement.x = direction;
            SetDirection(direction);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _currentSpeed;
            _rigidbody.velocity = velocity;
        }

        public void MoveVertically(float direction)
        {
            if (_isJumping)
                return;

            _movement.y = direction;
            Vector2 velocity = _rigidbody.velocity;
            velocity.y = direction * _verticalSpeed;
            _rigidbody.velocity = velocity;
            if (direction == 0)
                return;

            float verticalPosition = Mathf.Clamp(transform.position.y, _minVerticalPosition, _maxVerticalPosition);
            _rigidbody.position = new Vector2(_rigidbody.position.x, verticalPosition);
            UpdateSize();
        }

        public void Jump()
        {
            if (_isJumping)
                return;

            _isJumping = true;
            _rigidbody.AddForce(Vector2.up * _jumpForce);
            _rigidbody.gravityScale = _gravityScale;
            _startJumpVerticalPoint = transform.position.y;
        }

        private void UpdateSize()
        {
            float verticalDelta = _maxVerticalPosition - transform.position.y;
            float currentSizeModificator = _minSize + _sizeModificator * verticalDelta;
            transform.localScale = Vector2.one * currentSizeModificator;
        }

        private void SetDirection(float direction)
        {
            if ((_direction == Direction.Right && direction < 0) ||
                (_direction  == Direction.Left && direction > 0))
                Flip();
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _direction = _direction == Direction.Right ? Direction.Left : Direction.Right;

            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = cameraPair.Key == _direction;
        }

        private void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && _rigidbody.position.y <= _startJumpVerticalPoint)
            {
                ResetJump();
                return;
            }
        }

        private void ResetJump()
        {
            _isJumping = false; 
            _rigidbody.position = new Vector2(_rigidbody.position.x, _startJumpVerticalPoint);
            _rigidbody.gravityScale = 0;
        }      
        
        public void StartAtack()
        {
            if (!_animator.PlayAnimation(AnimationType.Atack, true))
                return;

            _animator.ActionRequested += Atack;
            _animator.AnimationEnded += EndAttack;
        }

        private void Atack()
        {
            Debug.Log("Atack");
        }

        private void EndAttack()
        {
            _animator.ActionRequested -= Atack;
            _animator.AnimationEnded -= EndAttack;
            _animator.PlayAnimation(AnimationType.Atack, false);
        }

        //public void StartSlize()
        //{
        //    if (!_animator.PlayAnimation(AnimationType.Slide, true))
        //        return;

        //    _animator.ActionRequested += Slize;
        //    _animator.AnimationEnded += EndSlize;
        //}

        //private void Slize()
        //{
        //    Debug.Log("Slide");
        //}

        //private void EndSlize()
        //{
        //    _animator.ActionRequested -= Slize;
        //    _animator.AnimationEnded -= EndSlize;
        //    _animator.PlayAnimation(AnimationType.Slide, false);
        //}
    }
}
