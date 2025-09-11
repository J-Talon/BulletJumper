using Input;
using Unity.VisualScripting;
using UnityEngine;

namespace Entity
{
    public class Player: GameEntity, InputListener
    {


        private float moveSpeed = 5;
        //private Rigidbody2D rb;
        
        public void Start()
        {
            ((InputListener)this).subscribe();
            //rb = GetComponent<Rigidbody2D>();
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
          //do something related to a game over state here
        }


        public void keyMovementVectorUpdate(Vector2 vector)
        {
            // rb.linearVelocity = vector * moveSpeed;
        }

        public void mousePositionUpdate(Vector2 mousePosition)
        {
          //  Debug.Log(mousePosition);
        }

        public void leftMousePress(float mouseValue)
        {
          //  Debug.Log("left mouse press");
        }

        public void leftMouseRelease(float mouseValue)
        {
          //  Debug.Log("left mouse release");
        }

    }
}