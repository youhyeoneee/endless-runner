using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoGroundScroller : MonoBehaviour
{
    public SpriteRenderer[] tiles;
    public Sprite[] groundImg;

    private SpriteRenderer temp;
    private DinoGameManager gm;

    void Start()
    {
        gm = DinoGameManager.instance;
        temp = tiles[0];
    }

    void Update()
    {
        if (gm.isPlay)
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

                    int idx = Random.Range(0, gm.stages[gm.curStage].grounds.Length);
                    tiles[i].sprite = gm.stages[gm.curStage].grounds[idx];
                }
            }
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].transform.Translate(new Vector2(-1, 0) * Time.deltaTime * DinoGameManager.instance.gameSpeed);
            }
        }
       
    }
}