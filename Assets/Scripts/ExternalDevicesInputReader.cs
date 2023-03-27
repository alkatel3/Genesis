using InputReader;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class ExternalDevicesInputReader : MonoBehaviour, IEntityInputSource
    {
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public float VerticalDirection => Input.GetAxisRaw("Vertical");
        public bool Jump { get; private set; }
        public bool Attack { get; private set; }
        public bool Slide { get; private set; }

        public void OnUpdate()
        {
            if (!IsPointerOverUI() && Input.GetButtonDown("Jump"))
                Jump = true;

            if (!IsPointerOverUI() && Input.GetButtonDown("Fire1"))
                Attack = true;

            if (!IsPointerOverUI() && Input.GetButton("Horizontal") && Input.GetKey(KeyCode.Mouse1))
                Slide = true;
        }

        private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

        public void ResetOneTimeAction()
        {
            Jump = false;
            Attack = false;
            Slide = false;
        }
    }
}
