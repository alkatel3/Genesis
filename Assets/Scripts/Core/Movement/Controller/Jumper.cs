using Assets.Scripts.StatsSystem;
using Assets.Scripts.StatsSystem.Enum;
using Core.Movement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Movement.Controller
{
    public class Jumper
    {
        private readonly JumpData _jumpData;
        private readonly Rigidbody2D _rigidbody;
        private readonly float _maxVerticalSize;
        private readonly Transform _transform;
        private readonly IStatValueGiver _statValueGiver;

        private float _startjumpVerticalPos;

        public bool IsJumping { get; private set; }

        public Jumper(Rigidbody2D rigidbody2D, JumpData jumpData, float maxVerticalSize, IStatValueGiver statValueGiver)
        {
            _rigidbody = rigidbody2D;
            _jumpData = jumpData;
            _maxVerticalSize = maxVerticalSize;
            _statValueGiver = statValueGiver;
            _transform = _rigidbody.transform;
        }

        public void Jump()
        {
            if (IsJumping)
            {
                return;
            }
            IsJumping = true;
            _startjumpVerticalPos = _rigidbody.position.y;
            var jumpModificator = _transform.localScale.y / _maxVerticalSize;
            var currentJumpForce = _statValueGiver.GetStatValue(StatType.JumpForce) * jumpModificator;
            _rigidbody.gravityScale = _jumpData.GravityScale * jumpModificator;
            _rigidbody.AddForce(Vector2.up * currentJumpForce);
        }

        public void UpdateJump()
        {
            if(_rigidbody.velocity.y<0 && _transform.position.y < _startjumpVerticalPos)
            {
                ResetJump();
                return;
            }

            var distance = _rigidbody.transform.position.y - _startjumpVerticalPos;
        }

        private void ResetJump()
        {
            _rigidbody.gravityScale = 0;
            _transform.position = new Vector2(_transform.position.x, _startjumpVerticalPos);

            IsJumping = false;
        }
    }
}
