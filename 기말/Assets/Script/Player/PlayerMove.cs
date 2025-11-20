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

    Vector3 sumVector, xVector, yVector, zVector;



    void Start()
    {
        CC = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void FixedUpdate()
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
        Sit(Input.GetKey(KeyCode.C));
        rotate();
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



        if (Ix != 0 || Iz != 0)
        {
            animator.SetBool("isMove", true);
            animator.SetFloat("zDir", Iz, 0.25f, Time.deltaTime);
            animator.SetFloat("xDir", Ix, 0.25f, Time.deltaTime);

        }
        else
        {
            animator.SetBool("isMove", false);
        }

        


        if (Iz > 0) // 앞으로 움직일때
        {

            if (Input.GetKey(KeyCode.LeftShift)) //달릴때
            {
                animator.SetFloat("zDir", 2, 0.25f, Time.deltaTime);

                speed = sprint;
            }
            else
            {
                if (speed > default_speed_front)
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
            else if (Ix == 0)
            {
                speed = default_speed_front;
            }
        }



    }

    void rotate()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * RotSpeed * Time.deltaTime;
        transform.Rotate(0,mouseX,0);
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
