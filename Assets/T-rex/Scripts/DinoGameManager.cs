using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float gameSpeed = 1;
    public bool isPlay = false;
    public delegate void OnPlay(bool isPlay);
    public OnPlay onPlay;

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
            score++;
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