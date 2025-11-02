using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController_ : MonoBehaviour
{
    NavMeshAgent Enemy_;
    public Transform player_;
    public Transform player_body_;
    Vector3 destination_;
    bool findlogic_ = true;
    public bool readyfire_ = false;
    State state;
    enum State
    {
        Idle,
        Chase,
        Attack
    }

    void ChangeState(State newState)
    {
        state = newState;
    }

    IEnumerator StateManger()
    {
        while(true)
        {
            yield return StartCoroutine(state.ToString());
        }
    }

    IEnumerator Idle()
    {
        //Debug.Log("Idle State");
        yield return null;
    }

    IEnumerator Chase()
    {
        //Debug.Log("Chase State");
        yield return null;
    }

    IEnumerator Attack()
    {
        //Debug.Log("Attack State");
        yield return null;
    }


    void Start()
    {
        Enemy_ = GetComponent<NavMeshAgent>();
        StartCoroutine(StateManger());
        set_Path_();
    }

    void set_Path_()
    {
        float x = Random.Range(-110, 110);
        float y = transform.position.y;
        float z = Random.Range(-110, 110);
        destination_ = new Vector3(x, y, z);
    }

    void Update()
    {

        //적을 찾는 로직의 실행여부
        if (findlogic_)
        {
            ChangeState(State.Idle);
            Enemy_.SetDestination(destination_); // 랜덤으로 생성된 위치로 이동
            if (Enemy_.remainingDistance <= Enemy_.stoppingDistance) // 멈춰야하는 크기보다 남은거리가 작으면, 즉 도착하면
            {
                set_Path_(); //목적지 갱신
            }
        }
        else
        {
            ChangeState(State.Chase);
            Enemy_.SetDestination(player_.position); // 플레이어를 확인하면 
        }
    }

    // DetectionAreaTrigger에서 제어할 수 있도록 메서드 제공
    public void SetFindLogic(bool value)
    {
        findlogic_ = value;
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
