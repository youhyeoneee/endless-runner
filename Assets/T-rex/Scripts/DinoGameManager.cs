using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DinoStage
{
    public Sprite[] grounds;
    public GameObject[] objs;
}

public class DinoGameManager : MonoBehaviour
{
    
    #region instance
    public static DinoGameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    public float gameSpeed = 4;
    public bool isPlay = false;
    public delegate void OnPlay(bool isPlay);
    public OnPlay onPlay;
    public int curStage; // 현재 스테이지
    public int[] stageScore; // 다음 스테이지로 넘어가기 위한 점수
    public DinoStage[] stages;
    
    [@SerializeField] private GameObject startText;
    [@SerializeField] private GameObject retryUI;
    [@SerializeField] private GameObject scoreUI;
    [@SerializeField] private Text scoreText;
    [@SerializeField] private Text highScoreText;

    private int score = 0;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = ConvertScore(highScore);

    }
    private void Update()
    {
        if (!isPlay && Input.GetKeyDown(KeyCode.Space)) // spacebar 누르면 
        {
            Play();
        }
    }
    public void Play()
    {
        
        // 게임 시작 안내 문구 비활성화
        if (startText.activeSelf) 
            startText.SetActive(false);
        
        // 다시하기 UI 비활성화
        if (retryUI.activeSelf)
            retryUI.SetActive(false);
        
        // 점수 UI 활성화
        if (!scoreUI.activeSelf)
            scoreUI.SetActive(true);
        
        // 스테이지 초기화
        curStage = 0;

        // 속도 초기화
        gameSpeed = 4;
        isPlay = true;
        onPlay.Invoke(isPlay);

        score = 0;
        scoreText.text = ConvertScore(score);
        StartCoroutine(AddScore());
    }

    public void GameOver()
    {
        retryUI.SetActive(true);
        isPlay = false;
        onPlay.Invoke(isPlay);
        
        StopCoroutine(AddScore());
        
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        // 최고 점수 갱신
        if (highScore < score)
        {
            PlayerPrefs.SetInt("HighScore", score);
            highScoreText.text = ConvertScore(score);
        }
    }

    IEnumerator AddScore()
    {
        
        while (isPlay)
        {
            try // 예외처리
            {
                // 각 스테이지에 필요한 점수를 도달했을 때
                if (stageScore[curStage] <= score)
                {                
                    curStage++;
                }
            } catch { }

            score++;
            gameSpeed += 0.01f; // 게임이 점점 빨라지게 
            scoreText.text = ConvertScore(score);
            yield return new WaitForSeconds(0.1f);
        }
    }

    string ConvertScore(int num)
    {
        // 5자리 형태 
        return string.Format("{0:D5}", num);
    }
}