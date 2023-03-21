using Assets.Scripts.Player;
using Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private GameUIInputView _gameUIInputView;

        private ExternalDevicesInputReader _externalDevicesInput;
        private PlayerBrain _playerBrain;

        private bool _onPause = false;

        private void Awake()
        {
            _externalDevicesInput = new ExternalDevicesInputReader();
            _playerBrain = new PlayerBrain(_playerEntity, new List<IEntityInputSource>
            {
                _gameUIInputView,
                _externalDevicesInput

            });
        }

        private void Update()
        {
            if (_onPause)
                return;

            _externalDevicesInput.OnUpdate();
        }

        private void FixedUpdate()
        {
            if (_onPause)
                return;

            _playerBrain.OnFixedUpdate();
        }
    }
}
