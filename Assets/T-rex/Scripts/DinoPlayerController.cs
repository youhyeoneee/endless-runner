using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoPlayerController : MonoBehaviour
{
    
    [SerializeField] private float jumpHeight = 0;
    [SerializeField] private float jumpSpeed  = 0;
    
    private Animator animator;
    private bool     isJump          = false;
    private bool     isTop           = false;
    private Vector2  startPosition;
    
    #region instance
    private DinoGameManager _gm;

    private void Awake()
    {
        _gm = DinoGameManager.instance;
        
        if (_gm == null)
        {
            _gm = FindObjectOfType<DinoGameManager>();

            if (_gm == null)
            {
                Debug.LogError("GameManager not found in scene.");
                return;
            }
        }
        
    }
    #endregion

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (_gm.isPlay)
        {
            animator.SetBool("die", false);
            // 달리는 애니메이션으로 설정하기
            animator.SetBool("run", true);
        }
        else
            animator.SetBool("run", false);

        // 점프 애니메이션으로 설정하기
        if (_gm.isPlay)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("run", false);
                animator.SetBool("jump", true);
                isJump = true;

                SoundManager.instance.Jump();
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
                    isTop = true;

                if (transform.position.y > startPosition.y && isTop)
                {
                    transform.position = Vector2.MoveTowards(transform.position,
                        startPosition, jumpSpeed * Time.deltaTime);
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Mob") && _gm.isPlay)
        {
            _gm.GameOver();
            animator.SetBool("die", true);
        }
    }
}
