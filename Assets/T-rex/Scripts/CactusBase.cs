using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusBase : MonoBehaviour
{
    public Vector2 startPosition;

    private void OnEnable()
    {
        transform.position = startPosition;
    }

    void Update()
    {
        if (DinoGameManager.instance.isPlay)
        {
            // 왼쪽으로 이동
            transform.Translate(Vector2.left * Time.deltaTime * DinoGameManager.instance.gameSpeed);

            // 왼쪽 넘어가면 안보이게 
            if (transform.position.x < -6)
            {
                gameObject.SetActive(false);
            }
        }

    }

}