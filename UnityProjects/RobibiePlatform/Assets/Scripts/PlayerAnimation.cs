using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    PlayerMovement movenent;
    Rigidbody2D rb;

    int groundID;
    int hangingID;
    int crouchID;
    int speedID;
    int fallID;

    void Start()
    {
        anim = GetComponent<Animator>();
        movenent = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();

        groundID = Animator.StringToHash("isOnGround");
        hangingID = Animator.StringToHash("isHanging");
        crouchID = Animator.StringToHash("isCrouching");
        speedID = Animator.StringToHash("speed");
        fallID = Animator.StringToHash("verticalVelocity");
    }

    // Update is called once per frame
    void Update()
    {
        //anim.SetFloat("speed", Mathf.Abs(movenent.xVelocity));
        anim.SetFloat(speedID, Mathf.Abs(movenent.xVelocity));
        //anim.SetBool("isOnGround", movenent.isOnGround);
        anim.SetBool(groundID, movenent.isOnGround);
        anim.SetBool(hangingID, movenent.isHanging);
        anim.SetBool(crouchID, movenent.isCrouch);
        anim.SetFloat(fallID, rb.velocity.y);
    }
    public void StepAudio()
    {
        AudioManager.PlayFootstepAudio();
    }

    public void CrouchStepAudio()
    {
        AudioManager.PlayCrouchFootstepAudio();
    }
}
