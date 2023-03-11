using Player;
using UnityEngine;

namespace Assets.Scripts
{
    public class InputReader : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _playerEntity;

        private float _horizontalDirection;
        private float _verticalDirection;

        public void Update()
        {
            _horizontalDirection = Input.GetAxisRaw("Horizontal");
            _verticalDirection = Input.GetAxisRaw("Vertical");

            if (Input.GetButtonDown("Jump"))
            {
                _playerEntity.Jump();
            }

            if (Input.GetButtonDown("Fire1"))
                _playerEntity.StartAtack();
        }

        private void FixedUpdate()
        {
            _playerEntity.MoveHorizontally(_horizontalDirection);
            _playerEntity.MoveVertically(_verticalDirection);
        }
    }
}
