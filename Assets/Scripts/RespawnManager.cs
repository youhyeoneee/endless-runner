using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class StageMob
{
    public List<GameObject> mobs = new List<GameObject>();
}
public class RespawnManager : MonoBehaviour
{
    public List<StageMob> MobPool = new List<StageMob>();
    public int objCnt = 5;
    GameManager gm;
    
    void Awake()
    {
        
        gm = GameManager.instance;

        if (gm == null)
        {
            gm = FindObjectOfType<GameManager>();

            if (gm == null)
            {
                Debug.LogError("GameManager not found in scene.");
                return;
            }
        }

        for (int i = 0; i < gm.stages.Length; i++)
        {
            StageMob stage = new StageMob();
            
            for (int j = 0; j < gm.stages[i].mobs.Length; j++)
            {
                for (int q = 0; q < objCnt; q++)
                {
                    stage.mobs.Add(CreateObj(gm.stages[i].mobs[j], transform));
                }
            }
            
            MobPool.Add(stage);
        }
    }
    
    private void Start()
    {
        GameManager.instance.onPlay += PlayGame;
    }

    void PlayGame(bool isPlay)
    {
        if (isPlay)
        {
            // 시작 전 장애물 비활성화
            for (int i = 0; i < MobPool.Count; i++)
            {
                for (int j = 0; j < MobPool[i].mobs.Count; j++)
                {
                    if (MobPool[i].mobs[j].activeSelf)
                        MobPool[i].mobs[j].SetActive(false);
                }
                
            }
            StartCoroutine(CreateMob());
        }
        else 
            StopAllCoroutines();
    }

    IEnumerator CreateMob()
    {
        yield return new WaitForSeconds(0.5f);
        while (GameManager.instance.isPlay)
        {
            MobPool[gm.curStage].mobs[DeactiveMob(MobPool[gm.curStage].mobs)].SetActive(true);
            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    int DeactiveMob(List<GameObject> mobs)
    {
        List<int> num = new List<int>();
        for (int i = 0; i < mobs.Count; i++)
        {
            if (!mobs[i].activeSelf)
                num.Add(i);
        }

        int x = 0;
        if (num.Count > 0)
            x = num[Random.Range(0, num.Count)];
        
        return x;
    }
    GameObject CreateObj(GameObject obj, Transform parent)
    {
        GameObject copy = Instantiate(obj);
        copy.transform.SetParent(parent);
        copy.SetActive(false);
        return copy;
    }
}
