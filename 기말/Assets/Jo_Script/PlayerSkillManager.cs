using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static SkillData;

public class PlayerSkillManager : MonoBehaviour
{
    public static PlayerSkillManager Instance;

    void Awake()
    {
        Instance = this;

    }
    public SkillSlot slotQ;
    public SkillSlot slotE;
    public SkillSlot slotR;
     [HideInInspector] 
    public SkillSlot[] slots; // index 0=Q, 1=E, 2=R
    void Start()
    {
        slots = new SkillSlot[] { slotQ, slotE, slotR };
        slots[0].key = KeyCode.Q;
        slots[1].key = KeyCode.E;
        slots[2].key = KeyCode.R;
        
        Debug.Log($"[Check] slotQ == slots[0] ? {ReferenceEquals(slotQ, slots[0])}");
        Debug.Log($"[Check] slotE == slots[1] ? {ReferenceEquals(slotE, slots[1])}");
        Debug.Log($"[Check] slotR == slots[2] ? {ReferenceEquals(slotR, slots[2])}");

        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            string name = i == 0 ? "Q" : i == 1 ? "E" : "R";
            string skillName = slot.skillData == null ? "null" : slot.skillData.skillName;
            Debug.Log($"[Start] Slot {name}: skillData={skillName}, isReady={slot.isReady}");
        
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Log("[Raw] Q key down");
        if (Input.GetKeyDown(KeyCode.E))
            Debug.Log("[Raw] E key down");
        if (Input.GetKeyDown(KeyCode.R))
            Debug.Log("[Raw] R key down");
        foreach (var slot in slots)
        {
            // 스킬 연결 안 됨
            if (slot.skillData == null) { 
                
                continue;
        }

            // 쿨타임 중
            if (!slot.isReady) continue;

            // 키 눌림 체크
            if (Input.GetKeyDown(slot.key))
            {
                Debug.Log($"[{slot.key}] 키 입력 감지! skill={slot.skillData.skillName}, isReady={slot.isReady}");
                UseSkill(slot);
            }
        }
    }
    public void AssignSkill(SkillData data)
    {
        Debug.Log($"[PlayerSkillManager] AssignSkill 호출됨: data={(data == null ? "null" : data.skillName)}, type={data?.type}");

        if (data == null)
        {
            Debug.LogError("[PlayerSkillManager] AssignSkill 에 null 데이터가 들어옴!");
            return;
        }
        if (data.type == SkillType.Q)
        {
            slotQ.skillData = data;
            slotQ.iconImage.sprite = data.icon;
            slotQ.cooldownFill.fillAmount = 1f;
            slotQ.isReady = true;
            Debug.Log($"[PlayerSkillManager] Slot Q 에 {data.skillName} 할당, isReady={slotQ.isReady}");
        }
        else if (data.type == SkillType.E)
        {
            slotE.skillData = data;
            slotE.iconImage.sprite = data.icon;
            slotE.cooldownFill.fillAmount = 1f;
            slotE.isReady = true;
        }
        else if (data.type == SkillType.R)
        {
            slotR.skillData = data;
            slotR.iconImage.sprite = data.icon;
            slotR.cooldownFill.fillAmount = 1f;
            slotR.isReady = true;
        }
    }

    void UseSkill(SkillSlot slot)
    {
        Debug.Log(slot.skillData.skillName + " 발동!");

        // TODO: 실제 스킬 효과 실행

        StartCoroutine(StartCooldown(slot));
    }

    IEnumerator StartCooldown(SkillSlot slot)
    {
        slot.isReady = false;

        float time = 0f;
        float total = slot.skillData.cooldown;

        // 스킬 누르면 즉시 아이콘 사라짐
        slot.cooldownFill.fillAmount = 0f;
        slot.timeText.gameObject.SetActive(true);

        while (time < total)
        {
            time += Time.deltaTime;

            // 0 → 1로 채워짐 (점점 아이콘이 다시 보임)
            slot.cooldownFill.fillAmount = time / total;

            slot.timeText.text = Mathf.Ceil(total - time).ToString();
            yield return null;
        }

        // 쿨타임 완료
        slot.cooldownFill.fillAmount = 1f;
        slot.timeText.gameObject.SetActive(false);
        slot.isReady = true;
    }

}
