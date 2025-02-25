using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float walkSpeed = 10f;
    [SerializeField] float jumpPower = 5f;

    Vector2 moveInput;
    Rigidbody myRigidbody;
    float xcoord = 0;
    float ycoord = 0;
    public bool isGrounded = true;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        xcoord = Mathf.MoveTowards(xcoord,moveInput.x, Time.deltaTime*4);
        ycoord =Mathf.MoveTowards(ycoord, moveInput.y, Time.deltaTime*4);
        animator.SetFloat("X", xcoord);
        animator.SetFloat("Y", ycoord);
        Run();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        animator.SetBool("jump", true);
        myRigidbody.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
        isGrounded = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Grounded")
        {
            animator.SetBool("jump", false);
            isGrounded = true;
        }
    }
    void Run()
    {
        Vector3 playerVelocity = new Vector3(moveInput.x * walkSpeed,myRigidbody.velocity.y, moveInput.y * walkSpeed);
        myRigidbody.velocity = transform.TransformDirection(playerVelocity);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}