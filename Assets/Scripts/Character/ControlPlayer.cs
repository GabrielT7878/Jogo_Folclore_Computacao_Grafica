using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    public float walkSpeed = 5;
    public Rigidbody rb;
    public float forceJump;
    public LayerMask layerMask;
    public bool isGrounded;
    public float groundCheckSize;
    public Vector3 groundCheckPosition;
    public int rollForce;
    public bool roll = false;
    private int side = 1;
    public float runSpeed;


    public Animator anim;

    void Start()
    {

    }

    void Update()
    {

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (!roll)
        {
            Vector3 direction = new Vector3(0, 0, horizontal);

            transform.Translate(direction * walkSpeed * Time.deltaTime);


            if (horizontal != 0)
            {
                anim.SetBool("walk", true);
            }
            else
            {
                anim.SetBool("walk", false);
            }

            if (horizontal > 0.01f)
            {
                rb.transform.localScale = new Vector3(1, 1, 1);
                side = 1;
            }
            if (horizontal < -0.01f)
            {
                rb.transform.localScale = new Vector3(1, 1, -1);
                side = -1;
            }



            var groundCheck = Physics.OverlapSphere(transform.position + groundCheckPosition, groundCheckSize, layerMask);
            if (groundCheck.Length != 1)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }

            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                rb.AddForce(transform.up * forceJump, ForceMode.Impulse);
                anim.SetTrigger("jump");
            }

            if (Input.GetButton("Fire2") && isGrounded && horizontal != 0){
                walkSpeed = runSpeed;
                transform.Translate(direction * walkSpeed * Time.deltaTime);
                anim.SetBool("running", true);
            }else{
                anim.SetBool("running", false);
                walkSpeed = 3;
            }


        }

        if (Input.GetButtonDown("Fire3") && !roll)
        {
            anim.SetTrigger("Rolar");
            roll = true;
        }
        if (roll)
        {
            
            transform.Translate(new Vector3(0, 0, side) * walkSpeed*0.5f * Time.deltaTime);
        }

        


    }

    public void endRoll(int i)
    {
        if (i == 1)
        {
            roll = false;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + groundCheckPosition, groundCheckSize);
    }
}
