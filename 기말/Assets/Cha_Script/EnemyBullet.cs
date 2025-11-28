using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Damage Settings")]
    public float bulletDamage = 5f;        // RandedEnemyAttackController에서 덮어씀
    public string targetTag = "Player";    // 맞출 대상 태그

    [Header("Options")]
    public bool destroyOnHit = true;       // 맞으면 삭제 여부
    public GameObject hitEffect;           // 히트 이펙트 (옵션)

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 판정
        if (other.CompareTag(targetTag))
        {
            // 플레이어 체력 스크립트 (너 프로젝트에 맞게 수정)
            PlayerStates playerHealth = other.GetComponent<PlayerStates>();
            if (playerHealth != null)
            {
                playerHealth.Hit(bulletDamage);
            }

            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);

            if (destroyOnHit)
                Destroy(gameObject);
        }
        else
        {
            // 벽/바닥 충돌 시 삭제 (옵션)
            if (!other.isTrigger && destroyOnHit)
            {
                if (hitEffect != null)
                    Instantiate(hitEffect, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}
