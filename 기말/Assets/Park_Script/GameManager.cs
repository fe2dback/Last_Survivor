using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    Transform SpawnPoint;

    public GameObject Player;
    public int level { get; private set; } = 0;

    public bool PlayerDead = false;

    [Header("단위 : 분")]
    public float leftTime;

    private void Awake()
    {
        if (GM == null)
        {
            GM = this;
        }
        else
        {

            Destroy(gameObject);
        }
    }


    public static GameManager Instance
    {
        get
        {
            if (null == GM)
            {
                return null;
            }
            return GM;
        }
    }

    private void Start()
    {
        Spawn();
        StartCoroutine(timeDecrease());
    }


    void Spawn()
    {
        SpawnPoint = GetComponentInChildren<Transform>().Find("PlayerSpawn");
        Player.transform.position = SpawnPoint.position;
    }
    public void AddLevel()
    {
        level++;
        Debug.Log(level);
    }
    IEnumerator timeDecrease()
    {
        leftTime = leftTime * 60;
        while (true)
        {
            Debug.Log(leftTime);
            leftTime -= Time.deltaTime;
            if (leftTime <= 0)
            {
                //게임오버 처리
                Debug.Log("게임오버");
                break;
            }
            yield return null;
        }
    }



}
