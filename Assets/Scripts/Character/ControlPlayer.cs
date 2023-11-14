using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class ControlPlayer : MonoBehaviour
{
    public float walkSpeed = 5;
    public Rigidbody rb;
    public float forceJump;
    public LayerMask layerMask;
    public bool isGrounded;
    public float groundCheckSize;
    public Vector3 groundCheckPosition;
    public bool roll = false;
    private int side = 1;
    public float runSpeed;
    public CapsuleCollider col;
    private float horizontal;
    private float vertical;
    public Camera cam;
    public PlayableDirector playableDirector;
    public GameObject lanca;
    public Transform maoDireita;
    public GameObject ball;
    public Animator anim;
    public bool lockWalkStatus = false;

    public Vector3 direction;

    void Start()
    {
        direction = new Vector3(0, 0, 0);
    }

    void Update()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        climbingCipe();

        if (!roll)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                anim.SetTrigger("attack");
                anim.SetBool("walk", false);
                anim.SetBool("running", false);
                lockWalkStatus = true;
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                anim.SetTrigger("normal_attack");
                anim.SetBool("walk", false);
                anim.SetBool("running", false);
                lockWalkStatus = true;
            }

            rolling();

            walk();

            isGroudend();

            jump();

            running();

            idleCipe();
        }
        else
        {
            //move automatic while rolling
            transform.Translate(new Vector3(0, 0, side) * walkSpeed * 0.5f * Time.deltaTime);
        }

    }

    public void climbingCipe()
    {
        if (vertical > 0.01f && rb.isKinematic)
        {
            rb.transform.Translate(new Vector3(0, 1, 0) * walkSpeed * Time.deltaTime);
            anim.SetBool("subir", true);
            anim.SetBool("idle_cipo", false);
        }
        else if (vertical < -0.01f && rb.isKinematic && isGroudend())
        {
            rb.transform.Translate(new Vector3(0, -1, 0) * walkSpeed * Time.deltaTime);
            anim.SetBool("subir", true);
            anim.SetBool("idle_cipo", false);
        }
        else if (rb.isKinematic)
        {
            anim.SetBool("subir", false);
            anim.SetBool("idle_cipo", true);
        }
    }

    public void rolling()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            anim.SetTrigger("Rolar");
            roll = true;
            col.height = 1.42f;
            col.center = new Vector3(0, 0.6f, 0);
        }
    }

    public void walk()
    {   
        if(lockWalkStatus){
            return;
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
            return;
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
    }

    public void idleCipe()
    {
        if (horizontal != 0 && rb.isKinematic)
        {
            rb.isKinematic = false;
            anim.SetBool("subir", false);
            anim.SetBool("idle_cipo", false);
        }
    }

    public bool isGroudend()
    {
        var groundCheck = Physics.OverlapSphere(transform.position + groundCheckPosition, groundCheckSize, layerMask);
        if (groundCheck.Length != 1)
        {
            isGrounded = true;
            return true;
        }
        else
        {
            isGrounded = false;
            return false;
        }
    }

    public void jump()
    {
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
    }

    public void running()
    {
        if(lockWalkStatus){
            return;
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

    public void endRoll(int i)
    {
        if (i == 1)
        {
            roll = false;
            col.height = 2.45f;
            col.center = new Vector3(0, 1.19f, 0);
        }
    }

    public void jumpRun(int i)
    {
        if (i == 1)
        {
            anim.SetBool("jump_run", false);
        }
    }

    public void attack()
    {
        GameObject l = Instantiate(lanca, maoDireita.position, Quaternion.identity);
        l.transform.localScale = new Vector3(30f, 30f, 30f * side);
        l.AddComponent<Rigidbody>();
        l.GetComponent<Rigidbody>().useGravity = false;
        l.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 10 * side);
        Destroy(l, 2f);
    }

    public void endAttack(){
        lockWalkStatus = false;
    }

    public void endNormalAttack(){
        lockWalkStatus = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Jumper")
        {
            anim.SetTrigger("jump");
            rb.AddForce(transform.up * forceJump * 2.5f, ForceMode.Impulse);
        }

        if (other.gameObject.tag == "Damage")
        {
            if (horizontal > 0.01f)
            {
                rb.AddForce(new Vector3(0, 0.5f, -0.5f) * forceJump * 0.8f, ForceMode.Impulse);
            }
            else if (horizontal < -0.01f)
            {
                rb.AddForce(new Vector3(0, 0.5f, 0.5f) * forceJump * 0.8f, ForceMode.Impulse);
            }
            else
            {
                rb.AddForce(new Vector3(0, 1f, 0.5f) * forceJump, ForceMode.Impulse);
            }
        }

        if (other.gameObject.tag == "EnemyCutscene")
        {
            CinemachineBrain cine = cam.GetComponent<CinemachineBrain>();
            cine.enabled = true;
            playableDirector.Play();
        }

        if (other.gameObject.tag == "ball")
        {
            ball.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "EnemyCutscene")
        {
            CinemachineBrain cine = cam.GetComponent<CinemachineBrain>();
            cine.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cipo")
        {
            rb.isKinematic = true;
            anim.SetBool("idle_cipo", true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + groundCheckPosition, groundCheckSize);
    }
}




