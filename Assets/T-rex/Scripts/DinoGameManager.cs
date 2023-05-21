using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DinoStage
{
    public Sprite[]     grounds;
    public GameObject[] objs;
}

public class DinoGameManager : MonoBehaviour
{
    
    #region instance
    public static DinoGameManager instance;
    private SoundManager _sm;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        
        if (_sm == null)
        {
            _sm = FindObjectOfType<SoundManager>();

            if (_sm == null)
            {
                Debug.LogError("SoundManager not found in scene.");
                return;
            }
        }
    }
    #endregion

    public float         gameSpeed = 4;
    public bool          isPlay    = false;
    public int           curStage; // 현재 스테이지
    public int[]         stageScore; // 다음 스테이지로 넘어가기 위한 점수
    public DinoStage[]   stages;
    public delegate void OnPlay(bool isPlay);
    public OnPlay onPlay;
    
    private int _score = 0;
    
    private void Update()
    {
        if (!isPlay && Input.GetKeyDown(KeyCode.Space)) // spacebar 누르면 
        {
            Play();
        }
    }
    public void Play()
    {
        // 스테이지, 속도, 점수 초기화
        curStage = 0;
        gameSpeed = 4;
        _score = 0;

        // 게임 시작
        isPlay = true;
        onPlay.Invoke(isPlay);
        StartCoroutine(AddScore());
    }

    public void GameOver()
    {
        isPlay = false;
        onPlay.Invoke(isPlay);
        
        _sm.Die();
        StopCoroutine(AddScore());
    }

    IEnumerator AddScore()
    {
        while (isPlay)
        {
            try // 예외처리
            {
                // 각 스테이지에 필요한 점수를 도달했을 때
                if (stageScore[curStage] <= _score)
                {                
                    curStage++;
                }
            } catch { }

            _score++;

            // 게임이 점점 빨라지게 
            if (_score % 100 == 0)
            {
                gameSpeed += 1f;
                _sm.Point();
            }
            else
                gameSpeed += 0.01f; 
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    public int GetScore()
    {
        return _score;
    }

}