using UnityEngine;
using Tools;
using Enums;
using Core;
using System;
using Animation;
using Core.Movement.Data;
using Core.Movement.Controller;
using Assets.Scripts.StatsSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private AnimationController _animator;
        [SerializeField] private DirectionalMovementData _directionalMovementData;
        [SerializeField] private JumpData _jumpData;
        [SerializeField] private DirectionalCamerPair _cameras;

        private Rigidbody2D _rigidbody;
        private DirectionMover _directionMover;
        private Jumper _jumper;

        public void Initialize(IStatValueGiver statValueGiver)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _directionMover = new DirectionMover(_rigidbody, _directionalMovementData, statValueGiver);
            _jumper = new Jumper(_rigidbody, _jumpData, _directionalMovementData.MaxSize, statValueGiver); 

            _rigidbody.position = new Vector2(_rigidbody.position.x, _directionalMovementData.MaxVerticalPosition);
            transform.localScale = Vector2.one * _directionalMovementData.MinSize;
        }

        private void Update()
        {
            try
            {
                if (_jumper.IsJumping)
                    _jumper.UpdateJump();

                UpdateAnimations();
                UpdateCameras();
            }
            catch(Exception ex)
            {
                Debug.Log(nameof(ex.InnerException));
            }
        }

        private void UpdateCameras()
        {
            foreach(var camaraPair in _cameras.DirectionalCameras)
                camaraPair.Value.enabled = camaraPair.Key == _directionMover.Direction;
        }

        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Walk, _directionMover.IsMoving);
            _animator.PlayAnimation(AnimationType.Slide, _directionalMovementData.CurrentSpeed == _directionalMovementData.RunSpeed);
            _animator.PlayAnimation(AnimationType.Jump, _jumper.IsJumping);
        }

        public void MoveHorizontally(float direction) => _directionMover.MoveHorizontally(direction);

        public void MoveVertically(float direction)
        {
            if (_jumper.IsJumping)
                return;

            _directionMover.MoveVertically(direction);
        }

        public void Jump() => _jumper.Jump();

        public void Slide(float direction) => _directionMover.Slide(direction);


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
    }
}
