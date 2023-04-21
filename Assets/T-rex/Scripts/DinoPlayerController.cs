using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoPlayerController : MonoBehaviour
{
    private Animator animator;
    private bool isJump = false;
    private bool isTop = false;
    public float jumpHeight = 0;
    public float jumpSpeed = 0;

    private Vector2 startPosition;
    void Start()
    {
        
        startPosition = transform.position;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (DinoGameManager.instance.isPlay)
        {
            animator.SetBool("die", false);
            // 달리는 애니메이션으로 설정하기
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }

        // 점프 애니메이션으로 설정하기
        if (Input.GetKeyDown(KeyCode.Space) && DinoGameManager.instance.isPlay)
        {
            animator.SetBool("run", false);
            animator.SetBool("jump", true);
            isJump = true;
        }
        else if (transform.position.y <= startPosition.y && isJump)
        {
            isJump = false;
            isTop = false;
            transform.position = startPosition;

            // 땅에 착지했을 경우
            if (transform.position.y == startPosition.y)
            {
                animator.SetBool("jump", false);
                animator.SetBool("run", true);
            }
        }

        if (isJump)
        {
            if (transform.position.y <= jumpHeight - 0.1f && !isTop)
            {
                transform.position = Vector2.Lerp(transform.position,
                    new Vector2(transform.position.x, jumpHeight), jumpSpeed * Time.deltaTime);
            }
            else
            {
                isTop = true;
            }

            if (transform.position.y > startPosition.y && isTop)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    startPosition, jumpSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Mob"))
        {
            DinoGameManager.instance.GameOver();
            animator.SetBool("die", true);
        }
    }
}
