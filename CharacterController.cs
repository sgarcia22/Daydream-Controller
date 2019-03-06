using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    Vector3 position;
    float step;
    bool moving = false;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            Vector3 previousLocation = transform.position;
            if (moving)
            {
                transform.position = Vector3.MoveTowards(transform.position, position, step);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - previousLocation), 1f);
                if (anim.GetBool("isRun") == false)
                    anim.SetBool("isRun", true);
            }
            else
                if (anim.GetBool("isRun") == true)
                  anim.SetBool("isRun", false);

            if (GvrControllerInput.AppButtonDown)
            {
                anim.SetBool("jump", true);
                StartCoroutine(StopJump());
            }

            if (GvrPointerInputModule.Pointer.enabled == true)
            {
                if (GvrControllerInput.ClickButtonDown)
                {
                    Debug.Log("Pressing Button");
                    position = GvrPointerInputModule.Pointer.CurrentRaycastResult.worldPosition;
                    position.y = transform.position.y;
                    step = speed * Time.deltaTime;
                    moving = true;

                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - previousLocation), 1f);
                    StartCoroutine(MoveTowards());
                }
            }
        } catch (Exception e)
        {
            
        }
    }

    IEnumerator StopJump()
    {
        yield return new WaitForSeconds(.5f);
        anim.SetBool("jump", false);
    }

    IEnumerator MoveTowards ()
    {
        yield return new WaitUntil(() => transform.position == position);
        moving = false;
    }
}
