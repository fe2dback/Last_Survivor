using UnityEngine;

public class DetectionAreaTrigger : MonoBehaviour
{
    private EnemyController enemyController;

    void Start()
    {
        // 부모 오브젝트 내 EnemyController 가져오기
        enemyController = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerStay(Collider other) 
    {
        //플레이어가 범위내로 들어올때
        if (other.CompareTag("Player"))
        {
            enemyController.SetFindLogic(false);
            enemyController.readyfire = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //플레이어가 범위밖으로 나갈때
        if (other.CompareTag("Player"))
        {
            enemyController.SetFindLogic(true);
            enemyController.readyfire = false;
        }
    }
}
