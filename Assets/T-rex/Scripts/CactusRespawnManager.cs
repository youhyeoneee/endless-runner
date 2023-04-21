using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CactusRespawnManager : MonoBehaviour
{
    public List<GameObject> cactusPool = new List<GameObject>();
    public GameObject[] objs;
    public int objCnt = 1;
    private void Awake()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            for (int j = 0; j < objCnt; j++)
            {
                cactusPool.Add(CreateObj(objs[i], transform));
            }
        }
    }

    private void Start()
    {
        DinoGameManager.instance.onPlay += PlayGame;

    }

    void PlayGame(bool isPlay)
    {
        if (isPlay)
        {
            // 시작 전 장애물 비화성화
            for (int i = 0; i < cactusPool.Count; i++)
            {
                if (cactusPool[i].activeSelf)
                        cactusPool[i].SetActive(false);
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
            cactusPool[SelectDeactivateCactus()].SetActive(true);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    int SelectDeactivateCactus()
    {
        List<int> deactiveList = new List<int>();
        for (int i = 0; i < cactusPool.Count; i++)
        {
            // 비활성화된 것을 추리기
            if (!cactusPool[i].activeSelf)
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