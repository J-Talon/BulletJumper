using UnityEngine;

namespace Input
{
    public interface InputListener
    {

        //In order to prevent initialization errors subscribe should be called in Start(), not Awake()
        public void subscribe()
        {
            InputManager manager = InputManager.instance;
            manager.keyboardMoveDispatcher.action += keyMovementVectorUpdate;
            manager.mouseMoveDispatcher.action += mousePositionUpdate;
            manager.mouseDownDispatcher.action += leftMousePress;
            manager.mouseUpDispatcher.action += leftMouseRelease;
            manager.mouseHoldDownDispatcher.action += mouseHoldDown;
        }

        public void unsubscribe()
        {
            InputManager manager = InputManager.instance;
            manager.keyboardMoveDispatcher.action -= keyMovementVectorUpdate;
            manager.mouseMoveDispatcher.action -= mousePositionUpdate;
            manager.mouseDownDispatcher.action -= leftMousePress;
            manager.mouseUpDispatcher.action -= leftMouseRelease;
            manager.mouseHoldDownDispatcher.action -= mouseHoldDown;
        }


        public abstract void keyMovementVectorUpdate(Vector2 vector);
        
        public abstract void mousePositionUpdate(Vector2 mousePosition);
        
        public abstract void leftMousePress(float mouseValue);
        
        public abstract void leftMouseRelease(float mouseValue);

        public abstract void mouseHoldDown(float mouseValue);

    }
}