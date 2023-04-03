using System.Collections;
using System.Collections.Generic;
using Enums;
using Cinemachine;
using UnityEngine;
using System;

namespace Tools
{
    [Serializable]
    public class DirectionalCamerPair
    {
        [SerializeField] private CinemachineVirtualCamera _rightCamera;
        [SerializeField] private CinemachineVirtualCamera _leftCamera;

        private Dictionary<Direction, CinemachineVirtualCamera> _directionalCameras;

        public Dictionary<Direction, CinemachineVirtualCamera> DirectionalCameras
        {
            get
            {
                if (_directionalCameras != null)
                {
                    return _directionalCameras;
                }

                _directionalCameras = new Dictionary<Direction, CinemachineVirtualCamera>
                {
                    { Direction.Right, _rightCamera },
                    { Direction.Left, _leftCamera }
                };
                return _directionalCameras;
            }
        }
    }
}
