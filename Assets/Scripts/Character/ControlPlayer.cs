using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
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
  
        Vector3 direction = new Vector3(0,0,horizontal);
        transform.Translate(direction * speed * Time.deltaTime);

        if (horizontal != 0){
            anim.SetBool("walk",true);
        }else{
            anim.SetBool("walk",false);
        }

        if(horizontal > 0.01f){
            rb.transform.localScale = new Vector3(1,1,1);
        }
        if(horizontal < -0.01f){
            rb.transform.localScale = new Vector3(1,1,-1);
        }


        var groundCheck = Physics.OverlapSphere(transform.position + groundCheckPosition, groundCheckSize, layerMask);
        if(groundCheck.Length != 1){
            isGrounded = true;
            //anim.SetBool("jump",false);
        }else{
            isGrounded = false;
        }

        if(isGrounded && Input.GetButtonDown("Jump")){
            rb.AddForce(transform.up * forceJump, ForceMode.Impulse);
            anim.SetBool("jump",true);
        }

    }

    public void EndAnimationJump(string message){
            if(message.Equals("EndJump")){
                anim.SetBool("jump",false);
            }
        }

    private void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + groundCheckPosition, groundCheckSize);
    }
}
