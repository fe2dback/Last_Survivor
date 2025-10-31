using UnityEngine;

public class DetectionAreaTrigger_ : MonoBehaviour
{
    private EnemyController_ enemyController;

    void Start()
    {
        // 부모 오브젝트 내 EnemyController 가져오기
        enemyController = GetComponentInParent<EnemyController_>();
    }

    private void OnTriggerStay(Collider other) 
    {
        //플레이어가 범위내로 들어올때
        if (other.CompareTag("Player"))
        {
            enemyController.SetFindLogic(false);
            enemyController.readyfire_ = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //플레이어가 범위밖으로 나갈때
        if (other.CompareTag("Player"))
        {
            enemyController.SetFindLogic(true);
            enemyController.readyfire_ = false;
        }
    }
}
