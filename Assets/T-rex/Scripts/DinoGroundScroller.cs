using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoGroundScroller : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] tiles;

    private SpriteRenderer  _temp;

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
        _temp = tiles[0];
    }

    void Update()
    {
        if (_gm.isPlay)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (-5 >= tiles[i].transform.position.x)
                {
                    for (int q = 0; q < tiles.Length; q++)
                    {
                        if (_temp.transform.position.x < tiles[q].transform.position.x)
                            _temp = tiles[q];
                    }

                    tiles[i].transform.position = new Vector2(_temp.transform.position.x + 1, -0.3f);

                    int idx = Random.Range(0, _gm.stages[_gm.curStage].grounds.Length);
                    tiles[i].sprite = _gm.stages[_gm.curStage].grounds[idx];
                }
            }
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].transform.Translate(new Vector2(-1, 0) * Time.deltaTime * DinoGameManager.instance.gameSpeed);
            }
        }
       
    }
}