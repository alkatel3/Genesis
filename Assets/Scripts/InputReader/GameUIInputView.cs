using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _attackButton;
        [SerializeField] private Button _slideButton;
        [SerializeField] private Button _inventoryButton;

        public float HorizontalDirection => _joystick.Horizontal;
        public float VerticalDirection => _joystick.Vertical;
        public bool Jump { get; private set; }
        public bool Attack { get; private set; }
        public bool Slide { get; private set; }

        public void ResetOneTimeAction()
        {
            Jump = false;
            Attack = false;
            Slide = false;
        }

        private void Awake()
        {
            _jumpButton.onClick.AddListener(() => Jump = true);
            _attackButton.onClick.AddListener(() => Attack = true);
            _slideButton.onClick.AddListener(() => Slide = true);
        }

        private void OnDestroy()
        {
            _jumpButton.onClick.RemoveAllListeners();
            _attackButton.onClick.RemoveAllListeners();
            _slideButton.onClick.RemoveAllListeners();
        }
    }
}
