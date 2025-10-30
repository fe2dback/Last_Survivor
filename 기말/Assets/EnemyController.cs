using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    NavMeshAgent Enemy;
    public Transform player;
    public Transform player_body;
    Vector3 destination;
    bool findlogic = true;
    public bool readyfire = false;
    void Start()
    {
        Enemy = GetComponent<NavMeshAgent>();
        set_Path();
    }

    void set_Path()
    {
        float x = Random.Range(-50, 50);
        float y = transform.position.y;
        float z = Random.Range(-50, 50);
        destination = new Vector3(x, y, z);
    }

    void Update()
    {

        //적을 찾는 로직의 실행여부
        if (findlogic)
        {
            Enemy.SetDestination(destination); // 랜덤으로 생성된 위치로 이동
            if (Enemy.remainingDistance <= Enemy.stoppingDistance) // 멈춰야하는 크기보다 남은거리가 작으면, 즉 도착하면
            {
                set_Path(); //목적지 갱신
            }
        }
        else
        {
            Enemy.SetDestination(player.position); // 플레이어를 확인하면 

        }
    }

    // DetectionAreaTrigger에서 제어할 수 있도록 메서드 제공
    public void SetFindLogic(bool value)
    {
        findlogic = value;
    }

    // 총알 피격 감지 (부모의 콜라이더에서 처리)
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet"))
        {
            Debug.Log("Hit");
            Destroy(gameObject);
        }
    }
}
