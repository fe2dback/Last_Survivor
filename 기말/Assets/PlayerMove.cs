using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    CharacterController CC;
    Animator animator;


    float gravity = 0.3f;


    float speed = 1f;
    float sprint = 2f;
    float JumpPower = 1.5f;

    float yAxis = 0f;

    float rot;

    Vector3 sumVector, xVector, yVector, zVector;

    // Start is called before the first frame update
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


    IEnumerator DecreaseValue(float start, float end, float duration)
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


    void playerInput()
    {
        moveHV(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }

    void MoveRotate(float Ix)
    {

        rot += Time.deltaTime * 0.25f;
        rot = Mathf.Clamp(rot, 0, 0.5f);

        if(Ix > 0)
        {
            transform.Rotate(0, rot, 0);
        }
        else if(Ix < 0)
        {
            transform.Rotate(0, -rot, 0);
        }
        else
        {
            rot = 0;
        }
        Debug.Log(rot);
    }

    void moveHV(float Ix, float Iz)
    {
        xVector = transform.right * speed * Ix * Time.deltaTime;
        zVector = transform.forward * speed * Iz * Time.deltaTime;

        sumVector = xVector + moveJ() + zVector;
        CC.Move(sumVector);
;       if(Iz !=0)
        {
            MoveRotate(Ix);
        }
        
        if (Iz > 0)
        {
            speed = 1f;
            animator.SetBool("isWalk", true);


            if(Input.GetKey(KeyCode.LeftShift))
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

                StartCoroutine(DecreaseValue(sprint, speed, 0.2f));
            }

        }
        else if(Iz < 0) 
        {
            animator.SetBool("isBackWalk", true);
        }
        else
        {
            animator.SetBool("isBackWalk", false);
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);

        }

        if (Ix > 0 && Iz == 0)
        {
            animator.SetBool("isRight", true);

        }
        else if (Ix < 0 && Iz == 0)
        {
            animator.SetBool("isLeft", true);
        }
        else
        {
            animator.SetBool("isRight", false);
            animator.SetBool("isLeft", false);
        }
        
    }
    
    Vector3 moveJ()
    {
        if (CC.isGrounded == true)
        {
            yAxis = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                yAxis = JumpPower*0.05f;   
            }
        }
        else
        {
            yAxis -= gravity * Time.deltaTime;

        }
        return yVector = new Vector3(0, yAxis, 0);
    }
}
