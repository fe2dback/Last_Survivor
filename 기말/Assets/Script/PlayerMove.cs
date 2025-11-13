using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class PlayerMove : MonoBehaviour
{
    CharacterController CC;
    Animator animator;
    float gravity = 0.1f;


    float speed = 3f;
    float default_speed_front = 3f;
    float default_speed_backward = 2f;
    float sprint = 6f;
    float sitSpeed = 0.5f;
    float JumpPower = 5f;

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

        Debug.Log(CC.isGrounded);
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
        Sit(Input.GetKey(KeyCode.C));
        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }



    void moveHV(float Ix, float Iz) // 플레이어의 이동 처리
    {
        Vector3 inputDirection = new Vector3(Ix, 0f, Iz);

        if (inputDirection.magnitude > 1f)
        {
            inputDirection.Normalize(); // 벡터의 크기를 1로 만듭니다.
        }

        // 3. 플레이어의 방향에 맞춰 로컬 방향 벡터를 월드 방향 벡터로 변환
        Vector3 worldMoveDirection = transform.TransformDirection(inputDirection);

        Vector3 finalMoveVector = worldMoveDirection * speed * Time.deltaTime + Jump();

        CC.Move(finalMoveVector);



        if (CC.velocity.magnitude != 0)
        {
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }
        animator.SetFloat("zDir", Iz);
        animator.SetFloat("xDir", Ix);



        if (Iz > 0) // 앞으로 움직일때
        {
            //animator.SetBool("isWalk", true);

            if (Input.GetKey(KeyCode.LeftShift)) //달릴때
            {
                animator.SetFloat("zDir", 2);
                //animator.SetBool("isRun", true);
                speed = sprint;
            }
            else
            {
                animator.SetFloat("zDir", 1);
                //animator.SetBool("isRun", false);
                if (speed > default_speed_front)
                {
                    //StartCoroutine(DecreaseSpeed(speed, default_speed_front, 0.2f));
                }
            }


            if (Ix > 0)
            {
                //animator.SetBool("isForwardRight", true);
            }
            else if (Ix < 0)
            {
                //animator.SetBool("isForwardLeft", true);
            }
            else
            {
                //animator.SetBool("isForwardRight", false);
                //animator.SetBool("isForwardLeft", false);
            }
        }
        else if (Iz < 0) //뒤로 갈때
        {
            speed = default_speed_backward;
            //animator.SetBool("isRun", false);
            //animator.SetBool("isBack", true);

            if (Ix > 0)
            {
                //animator.SetBool("isBackwardRight", true);
            }
            else if (Ix < 0)
            {
                //animator.SetBool("isBackwardLeft", true);
            }
            else
            {
                //animator.SetBool("isBackwardRight", false);
                //animator.SetBool("isBackwardLeft", false);
            }

        }
        else // Iz가 0일 때
        {
            //animator.SetBool("isWalk", false);
            //animator.SetBool("isBack", false);
            if (Ix != 0 && speed > default_speed_front)
            {
                StartCoroutine(DecreaseSpeed(speed, default_speed_front, 0.2f));
            }
            else if (Ix == 0)
            {
                speed = default_speed_front;
            }
        }
        //우
        if (Ix > 0 && Iz == 0)
        {
            //animator.SetBool("isRight", true);
        }
        //좌
        else if (Ix < 0 && Iz == 0)
        {
            //animator.SetBool("isLeft", true);
        }
        else
        {
            //animator.SetBool("isRight", false);
            //animator.SetBool("isLeft", false);
        }

        if (Ix == 0 && Iz == 0)
        {
            //animator.SetBool("isWalk", false);
            //animator.SetBool("isRun", false);
            //animator.SetBool("isBack", false);
            //animator.SetBool("isRight", false);
            //animator.SetBool("isLeft", false);
            //animator.SetBool("isFowardRight", false);
            //animator.SetBool("isFowardLeft", false);
            //animator.SetBool("isBackwardRight", false);
            //animator.SetBool("isBackwardLeft", false);
        }


    }


    void Sit(bool PressC)
    {
        if (PressC == true)
        {
            Debug.Log("C키 눌림");
            //speed = sitSpeed;
        }
        else
        {
            //speed = 1f;
        }


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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            Debug.Log("플레이어 피격");
        }
    }
}
