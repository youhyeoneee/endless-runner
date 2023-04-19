using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScroller : MonoBehaviour
{
    public SpriteRenderer[] tiles;
    // public Sprite[] groundImg;
    private SpriteRenderer temp; // 제일 오른쪽 끝에 있는 땅

    private GameManager gm;
    
    void Start()
    {
        gm = GameManager.instance;
        temp = tiles[0];
    }

    void Update()
    {

        if (GameManager.instance.isPlay)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (-5 >= tiles[i].transform.position.x)
                {
                    for (int q = 0; q < tiles.Length; q++)
                    {
                        if (temp.transform.position.x < tiles[q].transform.position.x)
                            temp = tiles[q];
                    }

                    tiles[i].transform.position = new Vector2(temp.transform.position.x + 1, -0.3f);
                    tiles[i].sprite = gm.stages[gm.curStage]
                        .grounds[Random.Range(0, gm.stages[gm.curStage].grounds.Length)];
                    // tiles[i].sprite = groundImg[Random.Range(0, groundImg.Length)];
                }
            }
        
            // 왼쪽으로 이동
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].transform.Translate(new Vector2(-1, 0) * Time.deltaTime * GameManager.instance.gameSpeed);
            }
            
        }
        
    }
}
