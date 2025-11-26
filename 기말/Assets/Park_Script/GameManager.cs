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

    private float leftTime = 30f;

    private void Awake()
    {
        if(GM == null)
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
            if(null == GM)
            {
                return null;
            }
            return GM;
        }
    }

    private void Start()
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
        while (true)
        {
            leftTime -= Time.deltaTime;
            if(leftTime <= 0)
            {
                //게임오버 처리
                Debug.Log("게임오버");
                break;
            }
            yield return null;
        }
    }

    static IEnumerator DecreaseSpeed(float start, float end, float duration) //감소 코루틴
    {
        float timer = 0f;
        while (timer < duration)
        {
            float currentValue = Mathf.Lerp(start, end, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
    }


}
