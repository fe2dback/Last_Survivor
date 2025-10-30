using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public Transform firePos;
    public GameObject Bullet;

    private EnemyController enemyController;
    private float time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyController.readyfire == true)
        {
            transform.LookAt(enemyController.player_body.position);
            shoot(0.3f);
        }
        else
        {
            return;
        }
        
    }

    void shoot(float set_time)
    {

        time += Time.deltaTime;
        if (time >= set_time)
        {
            // 실행할 로직
            Instantiate(Bullet, firePos.position, firePos.rotation);
            time = 0;
        }
        
    }
}
