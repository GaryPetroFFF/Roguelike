using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Classes
    protected enum CharacterSightEnum { LEFT, RIGHT };
    protected enum CharacterStateEnum
    {
        WALKING,
        ATTACKING
    }
    //Work variables
    protected Rigidbody2D rigidBidy2D_;
    protected Animator animator_;
    protected CharacterStateEnum currentState_;
    protected CharacterSightEnum characterSight_;
    //Stats
    protected int health_;
    protected int armor_;
    protected int damage_;
    protected int speed_;

    // Start is called before the first frame update
    protected void Start()
    {
        health_ = 0;
        armor_ = 0;
        damage_ = 0;
        speed_ = 0;
        rigidBidy2D_ = GetComponent<Rigidbody2D>();
        animator_ = GetComponent<Animator>();
        currentState_ = CharacterStateEnum.WALKING;
    }

    // Update is called once per frame
    protected void Update()
    {

        Vector3 change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButton("Fire1") && currentState_ != CharacterStateEnum.ATTACKING)
        {
            StartCoroutine(AttackCo());
        }
        else if (currentState_ == CharacterStateEnum.WALKING)
        {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemy = collision.GetComponent<Rigidbody2D>();
            if (enemy != null)
            {
                Vector2 difference = enemy.transform.position - transform.position;
                difference = difference.normalized * damage_;
                enemy.AddForce(difference);
                enemy.GetComponent<Character>().GetDamage(damage_);
                StartCoroutine(KnockbackCo(enemy));
            }
        }

    }

    protected IEnumerator AttackCo()
    {
        animator_.SetBool("isAttacking", true);
        currentState_ = CharacterStateEnum.ATTACKING;
        yield return null;
        animator_.SetBool("isAttacking", false);
        yield return new WaitForSeconds(.3f);
        currentState_ = CharacterStateEnum.WALKING;
    }

    protected IEnumerator KnockbackCo(Rigidbody2D enemy)
    {
        if (enemy != null)
        {
            yield return new WaitForSeconds(damage_ / 10);
            enemy.velocity = Vector2.zero;
        }
    }
    
    protected void Move(Vector3 change) 
    {
        change.Normalize();
        rigidBidy2D_.MovePosition(transform.position + change * speed_ * Time.deltaTime); 
        if (change.x < 0) 
        { 
            characterSight_ = CharacterSightEnum.LEFT;
        }
        else if (change.x > 0)
        {
            characterSight_ = CharacterSightEnum.RIGHT;
        }
    }

    protected virtual void FlipCharacterSight()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        switch(characterSight_)
        {
            case CharacterSightEnum.LEFT:
                spriteRenderer.flipX = true;
                break;
            case CharacterSightEnum.RIGHT:
                spriteRenderer.flipX = false;
                break;
        }
    }

    public void GetDamage(int damage)
    {
        health_ -= (damage - armor_);
        Debug.Log(this + health_.ToString());
    }
}
