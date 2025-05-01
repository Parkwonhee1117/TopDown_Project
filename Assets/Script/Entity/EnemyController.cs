using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyController : BaseController
{
    private EnemyManager enemyManager;
    private Transform target;

    [SerializeField] private float followRange = 15f;

    public void Init(EnemyManager enemyManager, Transform target)
    {
        this.enemyManager = enemyManager;
        this.target = target;
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }

    protected override void HandleAction()
    {
        base.HandleAction();

        if(weaponHandler == null || target == null)
        {
            if(!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;
            return;
        }

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        IsAttacking = false;

        if(distance <= followRange)
        {
            lookDirection = direction;

            if (distance < weaponHandler.AttackRange)  // 공격 범위 안이라면
            {
                int layerMaskTarget = weaponHandler.target; // 공격 가능한 대상을 저장장
                RaycastHit2D hit = Physics2D.Raycast( // Raycast는 눈에 보이지 않는 광선을 쏴 무엇과 충돌하는지 감지하는 코드
                transform.position, // (적)위치
                direction, // 플레이어(방향)
                weaponHandler.AttackRange * 1.5f, // Raycast의 최대범위 = 공격범위보다 좀 넓게
                (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget); // 레이어 이름을 정수 숫자 레이어 번호로 표현, 즉 Level은 8번째 layer이므로 8 => 8번쨰 비트만 켜진 숫자

                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer))) // 오브젝트가 맞았는지, 레이어가 타겟 레이어에 포함되는지 검사
                {
                    IsAttacking = true;
                }

                movementDirection = Vector2.zero;
                return;
            }

            movementDirection = direction;
        }
    }

    public override void Death()
    {
        base.Death();
        enemyManager.RemoveEnemyOnDeath(this);
    }
}
