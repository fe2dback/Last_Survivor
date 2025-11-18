using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    public int level { get; private set; } = 0;

    private float leftTime = 30f;

    private void Awake()
    {
        if(GM == null)
        {
            GM = this;
        }
        else
        {
            Debug.LogError("중복된 인스턴스");
            Destroy(gameObject);
        }
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
