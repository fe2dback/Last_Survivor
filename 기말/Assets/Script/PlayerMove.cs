using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class PlayerMove : MonoBehaviour
{
    CharacterController CC;
    Animator animator;


    float gravity = 0.3f;


    float speed = 1f;
    float sprint = 2f;
    float sitSpeed = 0.5f;
    float JumpPower = 0.075f;

    float yAxis = 0f;

    float rot;

    Vector3 sumVector, xVector, yVector, zVector;



    void Start()
    {
        CC = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerInput();
        
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


    void playerInput() //플레이어의 마우스, 키보드 입력을 받는 함수
    {
        moveHV(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Interactive(Input.GetKeyDown(KeyCode.E));
        Sit(Input.GetKey(KeyCode.C));
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }

    void MoveRotate(float Ix) // 대각선이동의 회전
    {

        //rot += Time.deltaTime * 0.25f;
        //rot = Mathf.Clamp(rot, 0, 0.5f);
        rot = 0.25f;
        if(CamMove.FPSviewMode == false) // 1인칭모드일때는 회전을 적용하면 화면이 개1같이돌아감
        {
            if (Ix > 0)
            {
                transform.Rotate(0, rot, 0);
            }
            else if (Ix < 0)
            {
                transform.Rotate(0, -rot, 0);
            }
        }

    }

    void moveHV(float Ix, float Iz) //플레이어의 이동 처리
    {
        xVector = transform.right * speed * Ix * Time.deltaTime;
        zVector = transform.forward * speed * Iz * Time.deltaTime;

        sumVector = xVector + Jump() + zVector; //-> 여기가 문제인듯
        CC.Move(sumVector); // CharacterController를 이용한 이동


        ;       if(Iz !=0)
        {
            MoveRotate(Ix);
        }
        
        if (Iz > 0) // 앞으로 움직일때
        {
            speed = 1f;
            animator.SetBool("isWalk", true);


            if(Input.GetKey(KeyCode.LeftShift)) //달릴때
            {
                animator.SetBool("isRun", true);       
                speed = sprint;
                
                if(Ix != 0)
                {
                    MoveRotate(Ix);
                }
            }
            else
            {
                animator.SetBool("isRun", false);

                StartCoroutine(DecreaseSpeed(sprint, speed, 0.2f));
            }

        }
        else if(Iz < 0) //뒤로 갈때
        {
            animator.SetBool("isBackWalk", true);
        }
        else
        {
            animator.SetBool("isBackWalk", false);
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);

        }

        if (Ix > 0 && Iz == 0)// 앞으로 가지 않으면서 오른쪽으로 갈때
        {
            animator.SetBool("isRight", true);

        }
        else if (Ix < 0 && Iz == 0) // 앞으로 가지 않으면서 왼쪽으로 갈때
        {
            animator.SetBool("isLeft", true);
        }
        else
        {
            animator.SetBool("isRight", false);
            animator.SetBool("isLeft", false);
        }

    }


     void Sit(bool PressC)
     {
        if(PressC == true)
        {
            Debug.Log("C키 눌림");
            speed = sitSpeed;
        }
        else
        {
            speed = 1f;
        }

        
     }


    void Interactive(bool PressE) // 상호작용(E키)
    {
        if (PressE == true)
        {
            Debug.Log("E키 눌림");
        }
    }
    
    Vector3 Jump()
    {
        if (CC.isGrounded == true)
        {
            yAxis = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                yAxis = JumpPower;
            }
        }
        else
        {
            yAxis -= gravity * Time.deltaTime;

        }
        return yVector = new Vector3(0, yAxis, 0);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "EnemyBullet")
        {
            Debug.Log("플레이어 피격");
        }
}
}
