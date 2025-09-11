using Input;
using Unity.VisualScripting;
using UnityEngine;

namespace Entity
{
    public class Player: GameEntity, InputListener
    {


        private float moveSpeed = 1;
        private Rigidbody2D rb;
        
        public void Awake()
        {
            ((InputListener)this).subscribe();
            rb = GetComponent<Rigidbody2D>();
        }

        
        //damages the player
        //returns whether the player was damaged
        public override bool damage()
        {
            health -= 1;
            if (health <= 0)
            {
                this.die();
            }

            return true;
        }


        public override void die()
        {
        }


        public void keyMovementVectorUpdate(Vector2 vector)
        {
            rb.linearVelocity = vector;
        }

        public void mousePositionUpdate(Vector2 mousePosition)
        {
            Debug.Log("mouse vector update");
        }

        public void leftMousePress(float mouseValue)
        {
            Debug.Log("left mouse press");
        }

        public void leftMouseRelease(float mouseValue)
        {
            Debug.Log("left mouse release");
        }

    }
}