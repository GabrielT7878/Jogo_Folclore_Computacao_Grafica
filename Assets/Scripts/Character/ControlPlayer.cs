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
            if (Input.GetButtonDown("Fire3") && !roll)
            {
                anim.SetTrigger("Rolar");
                roll = true;
            }
            
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
                if (Input.GetButton("Fire2") && horizontal > 0.01f)
                {
                    rb.AddForce(new Vector3(0, 1, 1) * forceJump, ForceMode.Impulse);
                    anim.SetBool("jump_run", true);
                }
                else if (Input.GetButton("Fire2") && horizontal < -0.01f)
                {
                    rb.AddForce(new Vector3(0, 1, -1) * forceJump, ForceMode.Impulse);
                    anim.SetBool("jump_run", true);
                }
                else if (horizontal > 0.01f)
                {
                    rb.AddForce(new Vector3(0, 1, 0.5f) * forceJump, ForceMode.Impulse);

                }
                else if (horizontal < -0.01f)
                {
                    rb.AddForce(new Vector3(0, 1, -0.5f) * forceJump, ForceMode.Impulse);
                }
                else
                {
                    rb.AddForce(transform.up * forceJump, ForceMode.Impulse);
                }
                anim.SetTrigger("jump");
            }

            if (Input.GetButton("Fire2") && isGrounded && horizontal != 0)
            {
                if (walkSpeed < runSpeed)
                {
                    walkSpeed += 0.1f;
                }
                transform.Translate(direction * walkSpeed * Time.deltaTime);
                anim.SetBool("running", true);
            }
            else
            {
                anim.SetBool("running", false);
                walkSpeed = 3;
            }


        }
        if (roll)
        {
            transform.Translate(new Vector3(0, 0, side) * walkSpeed * 0.5f * Time.deltaTime);
        }

    }

    public void endRoll(int i)
    {
        if (i == 1)
        {
            roll = false;
        }
    }

    public void jumpRun(int i)
    {
        if (i == 1)
        {
            anim.SetBool("jump_run", false);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + groundCheckPosition, groundCheckSize);
    }
}
