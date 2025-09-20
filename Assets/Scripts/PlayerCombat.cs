using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Animator animator;

    InputAction attackAction;

    [SerializeField] int attackDamage = 10;
    bool isAttacking = false;
    bool comboing = false;
    void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (attackAction.IsPressed())
        {
            if (!isAttacking)
            {
                isAttacking = true;
                animator.SetBool("IsAttacking", true);
                StartCoroutine(AttackCoroutine(0.5f, 0.34f));
                comboing = true;
            }
        }
    }

    void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCapsuleAll(attackPoint.position, new Vector2(1f, 1f), CapsuleDirection2D.Vertical, 24f, enemyLayer);
        foreach (Collider2D enemy in colliders)
        {
            enemy.GetComponent<HealthSystem>().TakeDamage(attackDamage);
        }
    }

    void ResetAttack()
    {
        if (!attackAction.IsPressed())
        {
            isAttacking = false;
            animator.SetBool("IsAttacking", false);
        }
        else if (comboing)
        {
            StartCoroutine(AttackCoroutine(0.2f, 0.26f));
            comboing = false;
        }
        else
        {
            StartCoroutine(AttackCoroutine(0.5f, 0.34f));
            comboing = true;
        }
    }

    IEnumerator AttackCoroutine(float attackDelay, float resetDelay)
    {
        yield return new WaitForSeconds(attackDelay);
        Attack();
        yield return new WaitForSeconds(resetDelay);
        ResetAttack();
    }
}
