using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun_ : MonoBehaviour
{
    public Transform firePos_;
    public GameObject Bullet_;

    private EnemyController_ enemyController_;
    private float time_ = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        enemyController_ = GetComponentInParent<EnemyController_>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyController_.readyfire_ == true)
        {
            transform.LookAt(enemyController_.player_body_.position);
            shoot_(3f);
        }
        else
        {
            return;
        }
        
    }

    void shoot_(float set_time)
    {

        time_ += Time.deltaTime;
        if (time_ >= set_time)
        {
            // 실행할 로직
            Instantiate(Bullet_, firePos_.position, firePos_.rotation);
            time_ = 0;
        }
        
    }
}
