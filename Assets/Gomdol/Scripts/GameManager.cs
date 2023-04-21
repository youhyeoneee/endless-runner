using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stage
{
    public Sprite[] grounds;
    public GameObject[] mobs; 
}

public class GameManager : MonoBehaviour
{
    #region instance
    public static GameManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    public delegate void OnPlay(bool isPlay);
    public OnPlay onPlay;
    
    public float gameSpeed = 4;
    public bool isPlay = false;
    public GameObject playBtn;

    public Text bestScoreText;
    public Text scoreText;
    public int score = 0;

    public int curStage; // 현재 스테이지
    public int[] stageScore; // 다음 스테이지로 넘어가기 위한 점수
    public Stage[] stages;
    private void Start()
    {
        bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
    }
    IEnumerator AddScore()
    {
        while (isPlay)
        {
            try // 예외처리
            {
                // 각 스테이지에 필요한 점수를 도달했을 때
                if (stageScore[curStage] <= score)
                    curStage++;
            } catch { }
           
            
            score++;
            scoreText.text = score.ToString();
            gameSpeed += 0.01f; // 게임이 점점 빨라지게 
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void PlayBtnClick()
    {
        playBtn.SetActive(false);
    
        // 스테이지 초기화
        curStage = 0;
        
        isPlay = true;
        onPlay.Invoke(isPlay);
        
        // 점수 초기화
        score = 0;
        scoreText.text = score.ToString();
        
        // 점수 증가 시작
        StartCoroutine(AddScore());
    }

    public void GameOver()
    {
        playBtn.SetActive(true);
        isPlay = false;
        onPlay.Invoke(isPlay);
        
        // 점수 증가 중지 
        StopCoroutine(AddScore());
        
        // 점수 저장
        if (PlayerPrefs.GetInt("BestScore", 0) < score)
        {
            PlayerPrefs.SetInt("BestScore", score);
            bestScoreText.text = score.ToString();
        }
    }
}