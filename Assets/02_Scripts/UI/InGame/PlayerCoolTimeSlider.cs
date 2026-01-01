using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoolTimeSlider : MonoBehaviour
{
    [Header("쿨타임 설정")]
    [SerializeField] public float maxCoolTime;
    [SerializeField] public float currentCoolTime;
    [SerializeField] private Slider coolTimeSlider;
    [SerializeField] private bool isCoolTime;

    [Header("데이터 소스(무기/스킬 컨트롤러)")]
    [SerializeField] private ActiveSkillStateControllerBehaviour coolTimeController;

    private TimeService timeService;


    
    private void Awake()
    {
        if(GameManager.Instance != null)
        {
            timeService = GameManager.Instance.GetService<TimeService>();
        }       
    }
    void Start()
    {
        if (coolTimeController == null)
            coolTimeController = FindObjectsOfType<ActiveSkillStateControllerBehaviour>().Where(obj => obj.isDefaultSkill).First(); ;

        if (coolTimeSlider == null)
            coolTimeSlider = GetComponent<Slider>();        

        coolTimeSlider.minValue = 0.0f;
        coolTimeSlider.maxValue = 1.0f;      
        
        if(coolTimeController != null)
        {
            maxCoolTime = coolTimeController.context.period;
        }

        currentCoolTime = Mathf.Clamp(currentCoolTime, 0.0f, maxCoolTime);
        UpdateCoolTimeSlider(currentCoolTime, maxCoolTime);
    }
    private void Update()
    {
        if (!isCoolTime) return;
        if (maxCoolTime <= 0) return;

        currentCoolTime += Time.deltaTime;

        if(currentCoolTime >= maxCoolTime)
        {
            currentCoolTime = maxCoolTime;
            isCoolTime = false;           
        }
        UpdateCoolTimeSlider(currentCoolTime, maxCoolTime);        
    }
    public void UpdateCoolTimeSlider(float currentCoolTime, float maxCoolTime)
    {
        if (coolTimeSlider == null) return;
        if (maxCoolTime <= 0) return;

        coolTimeSlider.value = currentCoolTime / maxCoolTime;
    }
    public void StartCoolTime()
    {
        if(coolTimeController != null)
        {
            maxCoolTime = coolTimeController.context.period;
        }
        //Debug.Log($"[테스트] 갱신된 MaxTime: {maxCoolTime}");
        Debug.Log($"현재 period : {coolTimeController.context.period}");

        currentCoolTime = 0.0f;
        isCoolTime = true;
        UpdateCoolTimeSlider(currentCoolTime, maxCoolTime);
    }
    public void AttackStart()
    {
        isCoolTime = false;
        currentCoolTime = 0;
        UpdateCoolTimeSlider(currentCoolTime, maxCoolTime);
    }
    public void AttackFinished()
    {
        if (coolTimeController != null)
        {
            maxCoolTime = coolTimeController.context.period;
        }
        //Debug.Log($"[테스트] 갱신된 MaxTime: {maxCoolTime}");
        Debug.Log($"현재 period : {coolTimeController.context.period}");

        currentCoolTime = 0.0f;
        isCoolTime = true;
        UpdateCoolTimeSlider(currentCoolTime, maxCoolTime);
    }
}