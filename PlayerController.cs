using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;      // LESSON 1 ````  By declaring Rigidbody2d rb; here, were saying "this is a box, for a rigid body component."
    Animator animator;
    SpriteRenderer spriteRenderer;


    public float moveSpeed = 1f;
    public ContactFilter2D movementFilter;
    public float collisionOffset = 0.05f;
    Vector2 movementInput;
     List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // Start is called before the first frame update

    bool canMove = true;




    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //What am I doing here?:
        // Basically, the point of 'rb', 'animator' and 'spriterenderer' is me telling
        // the engine, "hey! when i reference this empty rigidbody container, called rb, i want you to connect this to the
        // component we will get from the parent of this script (the parent of this script is the player, and we are getting its rigid body
        // no we can functionally reference this rigid body in the script, and modify via code we write here, in example, below,
        // we use it for movement!
        //vis versa with the rest of the " void start object = getcomponent<>(); "
  
    }








// fixed update is important, unlike regular update, it is mostly used for physics, and it is called
// at precise timings;
// the difference is, on regular "void update" you are calling this code every single individual frame.
//That means, if you have 100 fps, the code will run at 100fps, (which would mean youre doing it 10x too much)
    private void FixedUpdate() {
        if (canMove) //see can move below,  lines 134 -> 140    TLDR we are are saying IF canmove is true, (a true or false bool connected to the animator) then execute the code in this scope
        { //scope start
            
        
        if(movementInput != Vector2.zero){ // translation: IF (we are attempting to move) is true {    }
            bool success = TryMove(movementInput); // if its true than call the function, "try move" (and send it this info <movementinput)
            if(!success) { // if not success
                success = TryMove(new Vector2(movementInput.x, 0)); // if this is true, tryMove to a new direction, (sliding on walls tech)
            }


            if(!success) {
                    success = TryMove(new Vector2(0, movementInput.y)); //sliding on walls tech the other way
                }
        animator.SetBool("isMoving", success); // tell the animator, which we declared earlier, that we are infact moving
        } 
        else {
            animator.SetBool("isMoving", false); // and ofcourse, if these dont work, we are not moving, so dont play the anim.

             }
        
        //set direction of sprite to move direction
        // basically, we're saying if we move east, flip the sprite TRUE
        if(movementInput.x < 0){
        spriteRenderer.flipX = true;
        }  // vis versa, if we move west, dont flip it.
        else if(movementInput.x > 0){
        spriteRenderer.flipX = false;
            }
        } // true false bool scope end

    }


// here we get into raycasting and collision (super cool stuff, raycasting lasers and stuff)
    private bool TryMove(Vector2 direction) {//yes or no, are we trying to move in a direction?
        if(direction != Vector2.zero) { //if the direction does not equal 0, than{  }
            
        

        //raycasting time! poggers! https://www.youtube.com/watch?v=EINgIoTG8D4
    int count = rb.Cast(
                direction, // X and Y values between  -1 and 1 that represents a direction for the body to cast towards

                movementFilter,  // this is a filter of objects the laser should collide with! the default movementFilter contains any colliders. this means, rays will ignore everything that
                //is not a collider!

                castCollisions, // this is a list we referenced on line 17, 
                moveSpeed * Time.fixedDeltaTime + collisionOffset); // this is us saying the distance we will shoot the ray. In this case it is our movespeed multiplied by delta time, + the offset
                // in the most simple way, the value should always result, in our instantaneous future position, + the offset, which is = to what we define in the editor.
                //this means, if the ray collides with a collider, it will say we have a collision.


//             count = ray cast result
            if(count == 0){ // if the rays do not detect and colliders {  }
            //    our rigid body . Move Towards (current pos   direction in -1 ~ 1      dev defined move speed    fixed delta time to not happen based on FPS )   
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true; // and return true to the IF statement count == 0
            } else {
                return false; // else false
            }
        } else {
            return false; //else false, to line 89
        }


    }

    
    //OnMove Function defining for input handler native to unity
    // MovementInput is our predefined inside of PlayerController Mono.
    // were taking the movement input and making it - the movement value, to do this wwe must "Get//vector2//
    void OnMove(InputValue movementValue) {
        movementInput = movementValue.Get<Vector2>();



    }
    
    
    // attack starts here
    void OnFire() {
        print("Fire pressed!");
        animator.SetTrigger("swordAttack");

    }

    public void LockMovement(){
        canMove = false;

    }

    public void UnlockMovement(){
        canMove = true;


    }







    
}
