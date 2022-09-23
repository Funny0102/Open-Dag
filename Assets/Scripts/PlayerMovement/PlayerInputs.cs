using UnityEngine;

    public class PlayerInputs : MonoBehaviour
    {
        public InputActions input;

        private void Awake()
        {
            input = new InputActions();
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }
    }

