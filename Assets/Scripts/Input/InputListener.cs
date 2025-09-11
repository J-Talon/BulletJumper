using UnityEngine;

namespace Input
{
    public interface InputListener
    {

        public void subscribe()
        {
            InputManager manager = InputManager.instance;
            manager.keyboardMoveDispatcher.action += keyMovementVectorUpdate;
            manager.mouseMoveDispatcher.action += mousePositionUpdate;
            manager.mouseDownDispatcher.action += leftMousePress;
            manager.mouseUpDispatcher.action += leftMouseRelease;
        }

        public void unsubscribe()
        {
            InputManager manager = InputManager.instance;
            manager.keyboardMoveDispatcher.action -= keyMovementVectorUpdate;
            manager.mouseMoveDispatcher.action -= mousePositionUpdate;
            manager.mouseDownDispatcher.action -= leftMousePress;
            manager.mouseUpDispatcher.action -= leftMouseRelease;
        }


        public abstract void keyMovementVectorUpdate(Vector2 vector);
        
        public abstract void mousePositionUpdate(Vector2 mousePosition);
        
        public abstract void leftMousePress(float mouseValue);
        
        public abstract void leftMouseRelease(float mouseValue);

    }
}