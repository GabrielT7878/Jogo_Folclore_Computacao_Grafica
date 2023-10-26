using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
public class Controller : MonoBehaviour
{

    public float speed = 5;
    public Rigidbody rb;
    public float forceJump;
    public LayerMask layerMask;
    public bool isGrounded;
    public float groundCheckSize;
    public Vector3 groundCheckPosition;

    public Animator anim;
    void Start()
    {
        
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal == 1 || horizontal == -1){
            Vector3 direction = new Vector3(0,0,horizontal);
            transform.Translate(direction * speed * Time.deltaTime);
            anim.SetInteger("teste",1);
        }else{
            anim.SetInteger("teste",0);
        }
        


        var groundCheck = Physics.OverlapSphere(transform.position + groundCheckPosition, groundCheckSize, layerMask);
        if(groundCheck.Length != 1){
            isGrounded = true;
        }else{
            isGrounded = false;
        }

        if(isGrounded && Input.GetButtonDown("Jump")){
            rb.AddForce(transform.up * forceJump, ForceMode.Impulse);
        }

    }

    // private void OnDrawGizmos(){
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawSphere(transform.position + groundCheckPosition, groundCheckSize);
    // }
}
