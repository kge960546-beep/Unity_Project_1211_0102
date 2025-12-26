using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform rectBackground; //조이스틱 패드
    [SerializeField] private RectTransform rectJoystick;   // 조이스틱 손잡이

    [SerializeField] private float radius; //조이스틱 백그라운드의 반지름의 범위를 저장시킬 변수

    [SerializeField] private GameObject goPlayer;  //움직일 플레이어
    [SerializeField] private float moveSpeed = 3.0f;  // 움직이는 속도
    [SerializeField] private float rotateSpeed = 8.0f;  //회전속도

    [SerializeField] Animator animator; //애니메이터 삽입
    [SerializeField] private float currentSpeed; //애니메이터용 속도계산

    private bool isTouch = false;
    private Vector3 movePosition; //움직일 좌표
    private Vector3 moveDir;

    private void Awake()
    {
        animator = goPlayer.GetComponent<Animator>();
    }
    private void Start()
    {
        //rectTransform 에 접근해서 반지금을 구한 값을 넣기 위함
        this.radius = rectBackground.rect.width * 0.5f;
    }
    private void Update()
    {
        //이동구현
        if (this.isTouch)
        {
            this.goPlayer.transform.position += this.movePosition;
            //이동방향으로 케릭터 회전
            if (moveDir.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                goPlayer.transform.rotation = Quaternion.Slerp(goPlayer.transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            }
            //애니메이터
            if (animator != null)
            {
                animator.SetFloat("Speed", currentSpeed);
            }
            else
            {
                if (animator != null)
                {
                    animator.SetFloat("Speed", 0.0f);
                }
            }
        }
    }
    //터치가 시작되었을때
    public void OnPointerDown(PointerEventData eventData)
    {
        this.isTouch = true;
    }
    //터치를 중지했을때
    public void OnPointerUp(PointerEventData eventData)
    {
        //조이스틱 위치를 원위치로 되돌리기
        rectJoystick.localPosition = Vector3.zero;
        this.isTouch = false;
        //움직이다 손을 놓았을때 방향진행이 되는 현상 방지
        this.movePosition = Vector3.zero;

        if (animator != null)
        {
            animator.SetFloat("Speed", 0.0f);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        //마우스 좌표에서 조이스틱 백그라운드 좌표값을 뺀 값만큼 조이스틱을 움지기위한 벡터
        Vector2 value = eventData.position - (Vector2)rectBackground.position;
        //조이스틱이 백그라운드 밖으로 나가는걸 방지하는 함수
        value = Vector2.ClampMagnitude(value, radius);
        //거리에 따라 움직이는 속도를 다르게 하기
        float distance = Vector2.Distance(rectBackground.position, rectJoystick.position) / this.radius;
        //부모 객체인 백그라운드 기분으로 떨어질 상대적인 좌표값을 넣음
        rectJoystick.localPosition = value;
        //value의 방향값만 구하기
        value = value.normalized;
        //조이스틱을 월드기준 xz방향 벡터로 변환
        moveDir = new Vector3(value.x, 0.0f, value.y);
        // 조이스틱을 얼마나 멀리 밀었는지(distance)에 따라 실제 이동속도 계산
        currentSpeed = moveSpeed * distance;
        //최종 이동량 = 방향 * 속도 * 프레임
        this.movePosition = moveDir * currentSpeed * Time.deltaTime;
    }
}
