﻿using Assets.Scripts.Core.Sevrices.Updater;
using InputReader;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Player
{
    public class PlayerBrain : IDisposable
    {
        private readonly PlayerEntity _playerEntity;
        private readonly List<IEntityInputSource> _inputSources;

        public PlayerBrain(PlayerEntity playerEntity, List<IEntityInputSource> inputSources)
        {
            _playerEntity = playerEntity;
            _inputSources = inputSources;
            ProjectUpdater.Instance.FixedUpdateCalled += OnFixedUpdate;
        }

        public void Dispose() => ProjectUpdater.Instance.FixedUpdateCalled -= OnFixedUpdate;

        private void OnFixedUpdate()
        {
            _playerEntity.MoveHorizontally(GetHorizontalDirection());
            _playerEntity.MoveVertically(GetVerticalDirection());

            //if (IsSlide)
            //    _playerEntity.Slide(GetHorizontalDirection());

            if (IsJump)
                _playerEntity.Jump();

            if (IsAttack)
                _playerEntity.StartAtack();

            foreach(var source in _inputSources)
            {
                source.ResetOneTimeAction();
            }
        }

        private float GetHorizontalDirection()
        {
            foreach(var inputSource in _inputSources)
            {
                if (inputSource.HorizontalDirection == 0)
                    continue;

                return inputSource.HorizontalDirection;
            }

            return 0;
        }

        private float GetVerticalDirection()
        {
            foreach (var inputSource in _inputSources)
            {
                if (inputSource.VerticalDirection == 0)
                    continue;

                return inputSource.VerticalDirection;
            }

            return 0;
        }

        private bool IsJump => _inputSources.Any(source => source.Jump);
        private bool IsAttack => _inputSources.Any(source => source.Attack);
        private bool IsSlide => _inputSources.Any(source => source.Slide);
    }
}
