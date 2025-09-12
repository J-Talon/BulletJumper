
using Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {

        public static InputManager instance;

        [SerializeField]
        public InputActionAsset controls;
        

        private InputAction keyboardMoveAction;
        private InputAction mouseMoveAction;
        private InputAction mouseButtonAction;

        public EventDispatcher<Vector2> keyboardMoveDispatcher;
        public EventDispatcher<Vector2> mouseMoveDispatcher;
        public EventDispatcher<float> mouseDownDispatcher;
        public EventDispatcher<float> mouseUpDispatcher;

        private Vector2 lastKeyboardVector;
        private Vector2 lastMousePosition;
        private float lastLeftMouseState;
        
        private void Awake()
        {
            instance = this;
            keyboardMoveAction = controls.FindActionMap("Player").FindAction("Move");
            mouseMoveAction = controls.FindActionMap("Player").FindAction("Look");
            mouseButtonAction = controls.FindActionMap("Player").FindAction("Fire");
            
            DontDestroyOnLoad(gameObject);
            lastKeyboardVector = Vector2.zero;
            lastMousePosition = Vector2.zero;
            lastLeftMouseState = 0;
            
            keyboardMoveDispatcher = new EventDispatcher<Vector2>();
            mouseMoveDispatcher = new EventDispatcher<Vector2>();
            mouseDownDispatcher = new EventDispatcher<float>();
            mouseUpDispatcher = new EventDispatcher<float>();
        }


        //this is required to have the input work
        private void OnEnable()
        {
            controls.Enable();
        }

        //this is required to have the input work
        private void OnDisable()
        {
            controls.Disable();
        }
        
        
        void Update()
        {
            Vector2 keyboardVector = keyboardMoveAction.ReadValue<Vector2>();
            
            if (lastKeyboardVector != keyboardVector)
            {
                keyboardMoveDispatcher.dispatchEvent(keyboardVector);
                lastKeyboardVector = keyboardVector;
            }

            Vector2 mouseVector = mouseMoveAction.ReadValue<Vector2>();
            if (mouseVector.x != lastMousePosition.x && mouseVector.y != lastMousePosition.y)
            {
                mouseMoveDispatcher.dispatchEvent(mouseVector);
                lastMousePosition = mouseVector;
            }
            
            
            float mouseState = mouseButtonAction.ReadValue<float>();
            if (mouseState >= 1 && lastLeftMouseState < 1)
                mouseDownDispatcher.dispatchEvent(mouseState);
            else if (mouseState < 1 && lastLeftMouseState >= 1)
                mouseUpDispatcher.dispatchEvent(mouseState);
            lastLeftMouseState = mouseState;
            
        }
    }
    
}
