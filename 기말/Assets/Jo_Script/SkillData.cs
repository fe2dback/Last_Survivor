using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
public enum SkillType {Q, E, R}
[CreateAssetMenu(fileName = "Skill", menuName = "Game/Skill")]
public class SkillData : ScriptableObject
{

    public string skillName;
    public Sprite icon;
    public float cooldown;
    public SkillType type;

    // 추가: 스킬 성능 관련 값들
    public float damage;          // 피해량
    public float range;           // 사거리 / 범위
    public float projectileSpeed; // 투사체 속도 (원거리면)
    public float duration;        // 지속 시간 (버프/장판 등)

    [System.Serializable]
    public class SkillSlot
    {
        [HideInInspector] public KeyCode key;                  // Q / E / R
        public SkillData skillData;          // 유저가 선택해 넣은 스킬
        public bool isReady = true;

        public Image cooldownFill;           // 쿨타임 오버레이
        public TextMeshProUGUI timeText;     // 남은 시간 표시
        public Image iconImage;              // 스킬 아이콘
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
