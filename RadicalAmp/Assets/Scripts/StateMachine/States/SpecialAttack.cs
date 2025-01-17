﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttack : IState
{
    private Actor actor;
    private bool animationFinished;
    private Transform target;
    private bool hasToCheck;
    
    public SpecialAttack(Actor actor, Transform target)
    {
        this.actor = actor;
        this.target = target;
    }

    public void Enter()
    {
        animationFinished = false;
        hasToCheck = true;
    }

    public IEnumerator PlaySpecialAttack()
    {
       //actor.gameObject.GetComponent<Feedback>().NewStateAnimation("specialAttack");
       animationFinished = false;
       actor.attacking = true;
       yield return new WaitForSeconds(actor.GetComponent<Feedback>().specialAttackAnimation.length);
       animationFinished = true;
       actor.attacking = false;
       yield break;
    }

    public void Execute()
    {
        if(hasToCheck)
        {
            if(actor.CheckBeat(this))
            {
                actor.GetComponent<Feedback>().PlayAnimationForState("specialWindup");

                hasToCheck = false;
            }
        }

        if(!actor.windupFinished)
        {
            actor.FaceTarget();
        }

        if(actor.windupFinished)
        {
            actor.StartCoroutine(PlaySpecialAttack());
            actor.windupFinished = false;
        }

        if(animationFinished)
        {
            //Go back to Idle
            actor.StateMachine.ChangeState(new Idle(actor));
        }
    }

    public void Exit()
    {
        
    }
}
