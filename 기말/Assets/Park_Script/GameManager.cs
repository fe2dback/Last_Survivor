using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    Transform SpawnPoint;

    public GameObject Player;
    public int level { get; private set; } = 0;

    public bool PlayerDead = false;

    // 남은 시간(분 단위) – 인스펙터에서 설정 (예: 3 -> 3분)
    [SerializeField] private float leftTimeMinutes = 15f;

    // 내부에서 실제로 줄여갈 초 단위 시간
    private float leftTimeSeconds;

    public TextMeshProUGUI leftTimeText;

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
            if (GM == null) return null;
            return GM;
        }
    }

    private void Start()
    {
        SpawnPoint = GetComponentInChildren<Transform>().Find("PlayerSpawn");
        Player.transform.position = SpawnPoint.position;

        // 분 → 초로 변환해서 실제 남은 시간 설정
        leftTimeSeconds = leftTimeMinutes * 60f;
        UpdateLeftTimeUI();

        StartCoroutine(TimeDecrease());
    }

    public void AddLevel()
    {
        level++;
        Debug.Log(level);
    }

    IEnumerator TimeDecrease()
    {
        while (true)
        {
            if (PlayerDead)
                yield break;

            leftTimeSeconds -= Time.deltaTime;

            if (leftTimeSeconds < 0f)
                leftTimeSeconds = 0f;

            UpdateLeftTimeUI();

            if (leftTimeSeconds <= 0f)
            {
                Debug.Log("게임오버");
                // TODO: 게임오버 처리
                yield break;
            }

            yield return null;
        }
    }

    void UpdateLeftTimeUI()
    {
        if (leftTimeText == null) return;

        int minutes = (int)(leftTimeSeconds / 60);
        int seconds = (int)(leftTimeSeconds % 60);

        leftTimeText.text = $"{minutes:00}:{seconds:00}";
    }

    // 외부에서 남은 시간(초) 필요할 때
    public float GetLeftTimeSeconds()
    {
        return leftTimeSeconds;
    }

    // 남은 시간(분)으로 설정하고 싶을 때
    public void SetLeftTimeMinutes(float minutes)
    {
        leftTimeMinutes = minutes;
        leftTimeSeconds = leftTimeMinutes * 60f;
        UpdateLeftTimeUI();
    }

    static IEnumerator DecreaseSpeed(float start, float end, float duration)
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
