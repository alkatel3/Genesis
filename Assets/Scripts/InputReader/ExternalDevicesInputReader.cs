using Assets.Scripts.Core.Sevrices.Updater;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputReader
{
    public class ExternalDevicesInputReader : IEntityInputSource, IDisposable
    {
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public float VerticalDirection => Input.GetAxisRaw("Vertical");
        public bool Jump { get; private set; }
        public bool Attack { get; private set; }
        public bool Slide { get; private set; }

        public ExternalDevicesInputReader()
        {
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        public void ResetOneTimeAction()
        {
            Jump = false;
            Attack = false;
            Slide = false;
        }

        private void OnUpdate()
        {
            if (Input.GetButtonDown("Jump"))
                Jump = true;

            if (!IsPointerOverUI() && Input.GetButtonDown("Fire1"))
                Attack = true;

            if (!IsPointerOverUI() && Input.GetButton("Horizontal") && Input.GetKey(KeyCode.Mouse1))
                Slide = true;
        }


        private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    }
}
