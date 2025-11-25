using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class PlayerMove : MonoBehaviour
{
    CharacterController CC;
    Animator animator;
    
    public AudioSource Walk;//
    public AudioSource Run; // -> 배열로 담을것

    float gravity = 0.3f;
    [Tooltip("감도설정")]
    public float RotSpeed = 100f; 

    float speed = 3f;
    float default_speed_front = 3f;
    float default_speed_backward = 2f;
    float sprint = 6f;
    float sitSpeed = 0.5f;
    float JumpPower = 4f;

    float yAxis = 0f;

    float rot;


    bool ismove = false; //
    bool isrun = false;  // -> 움직임 사운드 변경

    Vector3 sumVector, xVector, yVector, zVector;



    void Start()
    {
        CC = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();


    }

    private void Update()
    {
        if(!GameManager.Instance.PlayerDead)
        {
            RotateY();
            PlayerInput();          
        }
        Audio();
        //Debug.Log(CC.velocity.magnitude);
    }



    IEnumerator DecreaseSpeed(float start, float end, float duration) // speed 감소 코루틴
    {
        float timer = 0f;
        while (timer < duration)
        {
            float currentValue = Mathf.Lerp(start, end, timer / duration);
            timer += Time.deltaTime;
            speed = currentValue;
            yield return null;
        }
    }



    void PlayerInput() //플레이어의 마우스, 키보드 입력을 받는 함수
    {
        moveHV(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void Audio()
    {
        bool Dead = !GameManager.Instance.PlayerDead;
        //플레이어 움직임 사운드
        if (ismove && !isrun && Dead)
        {
            if (!Walk.isPlaying)
            {
                Walk.Play();
            }
        }
        else
        {
            Walk.Stop();
        }


        if (isrun && Dead)
        {
            if (!Run.isPlaying)
            {
                Run.Play();
            }
        }
        else
        {
            Run.Stop();
        }

    }


    void moveHV(float Ix, float Iz) // 플레이어의 이동 처리
    {
        Vector3 InputDirection = new Vector3(Ix, 0f, Iz);

        if (InputDirection.magnitude > 1f)
        {
            InputDirection.Normalize();
        }
 
        Vector3 WorldMoveDirection = transform.TransformDirection(InputDirection);

        Vector3 FinalMoveVector = WorldMoveDirection * speed * Time.deltaTime + Jump();

        CC.Move(FinalMoveVector);



        if (Ix != 0 || Iz != 0)
        {
            animator.SetBool("isMove", true);
            animator.SetFloat("zDir", Iz, 0.25f, Time.deltaTime);
            animator.SetFloat("xDir", Ix, 0.25f, Time.deltaTime);
            
            if(Ix != 0 && Iz != 0)
            {
                ismove = false; 
                isrun = true;
            }
            else
            {
                ismove = true;
                isrun = false;
            }
        }
        else
        {
            animator.SetBool("isMove", false);
            isrun = false;
            ismove = false;
        }

        if (Iz > 0) // 앞으로 움직일때
        {
            speed = default_speed_front;
            if (Input.GetKey(KeyCode.LeftShift)) //달릴때
            {
                animator.SetFloat("zDir", 2, 0.25f, Time.deltaTime);
                
                speed = sprint;

                isrun = true;
            }
            else
            {
                if (speed > default_speed_front) // 뛰지않았는데 현재속도가 기본속도보다 크면 줄여줌
                {
                    StartCoroutine(DecreaseSpeed(speed, default_speed_front, 0.2f));
                }

            }

        }
        else if (Iz < 0) //뒤로 갈때
        {
            speed = default_speed_backward;
        }
        else // Iz가 0일 때
        {

            if (Ix != 0 && speed > default_speed_front)
            {
                StartCoroutine(DecreaseSpeed(speed, default_speed_front, 0.2f));
            }
        }
       
    }

    //플레이어 회전
    void RotateY()
    {
        float mouseX = Input.GetAxis("Mouse X") * RotSpeed * Time.deltaTime;
        transform.Rotate(0,mouseX,0);
    }



    Vector3 Jump()
    {
        if (CC.isGrounded == true)
        {
            yAxis = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("isJump");
                yAxis = JumpPower;
            }
            animator.SetTrigger("isground");
        }
        else
        {
            yAxis -= gravity * Time.deltaTime;


        }
        return yVector = new Vector3(0, yAxis, 0) * Time.deltaTime;
    }


}
