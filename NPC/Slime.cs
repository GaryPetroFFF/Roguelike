using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Character
{
    //Variables
    private Transform target_;
    private int attackRadius_;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        characterSight_ = CharacterSightEnum.LEFT;
        target_ = GameObject.FindGameObjectWithTag("Player").transform;
        attackRadius_ = 1;
        speed_ = 1;
        health_ = 10;
        armor_ = 0;
        damage_ = 5;
    }

    // Update is called once per frame
    new void Update()
    {
        if (Vector3.Distance(transform.position, target_.position) < attackRadius_ && currentState_ != CharacterStateEnum.ATTACKING)
        {
            StartCoroutine(AttackCo());
        }
        else if(currentState_ == CharacterStateEnum.WALKING)
        {
            Vector3 change = Vector3.MoveTowards(transform.position, target_.position, speed_ * Time.deltaTime);
            if (change != Vector3.zero) { 
                Move(change);
                animator_.SetBool("isWalking", true);
            }
            else
            {
                animator_.SetBool("isWalking", false);
            }
            FlipCharacterSight();
        }
    }

    new protected IEnumerator AttackCo()
    {
        animator_.SetBool("isAttacking", true);
        currentState_ = CharacterStateEnum.ATTACKING;
        yield return null;
        animator_.SetBool("isAttacking", false);
        yield return new WaitForSeconds(.83f);
        currentState_ = CharacterStateEnum.WALKING;
    }

    new protected void Move(Vector3 change)
    {
        transform.position = change;
        if (change.x <= transform.position.x)
        {
            characterSight_ = CharacterSightEnum.LEFT;
        }
        else
        {
            characterSight_ = CharacterSightEnum.RIGHT;
        }
    }

    new protected virtual void FlipCharacterSight()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch(characterSight_)
        {
            case CharacterSightEnum.RIGHT:
                spriteRenderer.flipX = true;
                break;
            case CharacterSightEnum.LEFT:
                spriteRenderer.flipX = false;
                break;
        }
    }
}
