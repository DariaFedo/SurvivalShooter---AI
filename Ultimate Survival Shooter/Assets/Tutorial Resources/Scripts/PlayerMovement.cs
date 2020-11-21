using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Animator anim;  //referencja do komponentu animator
    Rigidbody playerRigidbody;
    int floorMask;  // rey-cast the camera view on the artificial invisible floor
    float camRayLength = 100f;

	void Awake () // wywoływana bez względu na to czy skrypt jest enabled czy nie 
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>(); //<> typ komponentu w tych nawiasach którego szukamy
        playerRigidbody = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()  //unity calls this function automatically for physics update - update that runs with physics not with renderer eg. when
        //we move our character, we use fixed update to see that change as he moves
    {
        float h = Input.GetAxisRaw("Horizontal"); //axis is just input from eg keybord ad left right
        float v = Input.GetAxisRaw("Vertical"); //ws up down arrows

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h, float v)
    {
       movement.Set(h, 0f, v);
       movement = movement.normalized * speed * Time.deltaTime; //normalizacja dwóch wektorów tak by ich wartość wypakowa nie było większa niż 1. chodzi głównie o utrzymanie stałej prędkości poruszania się 
        //deltaTime (update ruchu o 6 "jednostek" co 1 sekundę a nie co 1/50 sekundy fixedupdate" - jest to czasy miedzy kazdy updateem

       playerRigidbody.MovePosition(transform.position + movement); // move the player - u need rigidbody 
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) //if it hits sth returns t or f
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

}
