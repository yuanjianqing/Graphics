using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    protected Animator anim;
    protected Controler movement;

    int groundID;
    int jumpID;
    int speedID;
    int fireID;
    int gunHeldID;

    public void BaseAwake()
    {
        anim = GetComponent<Animator>();
        movement = GetComponentInParent<Controler>();

    }
    public void BaseStart()
    {
        groundID = Animator.StringToHash("isGrounded");
        jumpID = Animator.StringToHash("isJumping");
        speedID = Animator.StringToHash("speed");
        fireID = Animator.StringToHash("isFiring");
        gunHeldID = Animator.StringToHash("isGunHeld");
    }

    // Update is called once per frame
    public void BaseUpdate()
    {
        anim.SetFloat(speedID, Mathf.Abs(movement.direction));
        anim.SetBool(groundID, movement.isGrounded);
        anim.SetBool(jumpID, movement.isJumping);
        anim.SetBool(fireID, movement.isFiring);
        anim.SetBool(gunHeldID, movement.isGunHeld);
    }

}
