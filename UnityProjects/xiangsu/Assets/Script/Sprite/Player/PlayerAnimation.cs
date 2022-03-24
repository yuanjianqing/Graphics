using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : AnimationController
{

    int dieID;
    int hurtID;
    int liftID;


    PlayerControler playerMovement;

    

    private void Awake()
    {
        BaseAwake();


        playerMovement = GetComponent<PlayerControler>();

    }
    void Start()
    {
        BaseStart();

        dieID = Animator.StringToHash("isDying");
        hurtID = Animator.StringToHash("isHurting");
        liftID = Animator.StringToHash("isLifting");
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();

        anim.SetBool(dieID, playerMovement.isDying);
        anim.SetBool(hurtID, playerMovement.isHurting);
        anim.SetBool(liftID, playerMovement.isLifting);
    }



    public void SetHurtFlagFalse()
    {
        anim.SetBool(hurtID, false);
        playerMovement.isHurting = false;
    }
}
