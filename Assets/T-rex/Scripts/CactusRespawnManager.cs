using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


[System.Serializable]
public class StageObj
{
    public List<GameObject> objs = new List<GameObject>();
}
public class CactusRespawnManager : MonoBehaviour
{
    public List<StageObj> objPool = new List<StageObj>();
    public int objCnt = 5;
    
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

        for (int i = 0; i < _gm.stages.Length; i++)
        {
            StageObj stage = new StageObj();
            
            for (int j = 0; j < _gm.stages[i].objs.Length; j++)
            {
                for (int q = 0; q < objCnt; q++)
                {
                    stage.objs.Add(CreateObj(_gm.stages[i].objs[j], transform));
                }
            }
            
            objPool.Add(stage);
        }
    }
    #endregion

    private void Start()
    {
        _gm.onPlay += PlayGame;

    }

    void PlayGame(bool isPlay)
    {
        if (isPlay)
        {
            // 시작 전 장애물 비활성화
            for (int i = 0; i < objPool.Count; i++)
            {
                for (int j = 0; j < objPool[i].objs.Count; j++)
                {
                    if (objPool[i].objs[j].activeSelf)
                        objPool[i].objs[j].SetActive(false);
                }
                
            }
            StartCoroutine(CreateCactus());
        }
        else
            StopAllCoroutines();
    }

    IEnumerator CreateCactus()
    {
        while (DinoGameManager.instance.isPlay)
        {
            objPool[_gm.curStage].objs[SelectDeactivateObj(objPool[_gm.curStage].objs)].SetActive(true);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    int SelectDeactivateObj(List<GameObject> objs)
    {
        List<int> deactiveList = new List<int>();
        for (int i = 0; i < objs.Count; i++)
        {
            // 비활성화된 것을 추리기
            if (!objs[i].activeSelf)
                deactiveList.Add(i);
        }

        int num = 0;
        if (deactiveList.Count > 0)
            num = deactiveList[Random.Range(0, deactiveList.Count)];

        return num;
    }

    GameObject CreateObj(GameObject obj, Transform parent)
    {
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);
        return copy;
    }
}