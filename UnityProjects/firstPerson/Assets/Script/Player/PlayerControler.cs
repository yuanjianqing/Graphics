using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    //Rigidbody rb;
    //float speed = 20000f;
    public CharacterController controller;
    private Vector3 playerVelocity;
    private float jumpHeight = 4.0f;
    public float speed = 12f;
    public bool watcher;
    float jumpTime;
    float jumpDuration = 0.5f;
    //Vector3 frontDir;

    float groundDistance = .4f; //地面距离
    float jumpHight = 0.1f;
    bool jumpPressed;
    bool timeSet;

    public Transform frontDir;

    [Header("关键的点")]
    public float footDistance = 0.8f;

    [Header("状态")]
    public bool isIdling0;
    public bool isWalking1;
    public bool isGrounded;


    public string currentState = "isIdling";
    [Header("环境")]
    public Transform groundPosition;
    public LayerMask groundMask;
    public float gravityValue = -9.81f;

    private void Awake()
    {
        //rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        PhysicsCheck();
        switch (currentState)
        {
            case "isIdling":
            case "isWalking": GroundMovement(); break;
            case "isJumping": MidAirMovement(); break;
        }

    }

    void GroundMovement()
    {

        //speed = 0.25f;
        float forwardBackwardVelocity = Input.GetAxis("Vertical");
        float leftwardRightwardVelocity = Input.GetAxis("Horizontal");
        /*if (forwardBackwardVelocity != 0 && leftwardRightwardVelocity != 0)
        {
            trans.localPosition += new Vector3(leftwardRightwardVelocity * step / Mathf.Sqrt(2), 0, forwardBackwardVelocity * step/ Mathf.Sqrt(2));
        }
        else
        {
            trans.localPosition += new Vector3(leftwardRightwardVelocity * step, 0, forwardBackwardVelocity * step);
        }*/


        /*float sin = Mathf.Sin(Mathf.PI * transform.rotation.eulerAngles.y/180);

        float cos = Mathf.Cos(Mathf.PI * transform.rotation.eulerAngles.y/180);
        //Vector3 move = transform.right * leftwardRightwardVelocity + transform.forward * forwardBackwardVelocity;
        Vector3 move = new Vector3(leftwardRightwardVelocity * cos + forwardBackwardVelocity * sin, 0, -leftwardRightwardVelocity * sin + forwardBackwardVelocity * cos);*/



        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        move = frontDir.transform.forward * forwardBackwardVelocity + frontDir.transform.right * leftwardRightwardVelocity;

        controller.Move(move * speed * Time.deltaTime);
    }


    void MidAirMovement()
    {
        controller.Move(new Vector3(0f, jumpHight, 0f));
        if(jumpPressed && !timeSet)
        {
            jumpTime = Time.time + jumpDuration;
            timeSet = true;
        }
        else if(jumpTime < Time.time)
        {
            currentState = "isIdling";
            timeSet = false;
        }
        if(isGrounded)
        {
            jumpPressed = false;
        }
    }

    void PhysicsCheck()
    {
        isGrounded = Raycast(transform.position - Vector3.up * footDistance, -Vector3.up, groundDistance, groundMask) ? true : false; ;
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    bool Raycast(Vector3 position, Vector3 rayDirection, float length, LayerMask layer)
    {
        bool isHit = Physics.Raycast(position, rayDirection, length, layer);
        watcher = isHit;

        Color color = isHit ? Color.red : Color.green;

        Debug.DrawRay(position, rayDirection * length, color);

        return isHit;
    }

}
