using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject startText;
    [SerializeField] private GameObject retryUI;
    [SerializeField] private GameObject scoreUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Color dayColor = Color.white;
    [SerializeField] private Color nightColor = Color.black;
    private int _score;
    private int _scoreDay; // 밤 -> 낮
    private Camera _camera;
    private float smoothness = 0.02f;
        
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

    public float duration = 3.0F;
    void Start()
    {
        _camera = Camera.main;

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = ConvertScore(highScore);
        
        _gm.onPlay += PlayGame;
    }

    void PlayGame(bool isPlay)
    {
        if (isPlay)
        {
            _camera.backgroundColor = dayColor;
            
            // 게임 시작 안내 문구 비활성화
            if (startText.activeSelf) 
                startText.SetActive(false);
        
            // 다시하기 UI 비활성화
            if (retryUI.activeSelf)
                retryUI.SetActive(false);
        
            // 점수 UI 활성화
            if (!scoreUI.activeSelf)
                scoreUI.SetActive(true);
            
            scoreText.text = ConvertScore(_gm.GetScore());
            StartCoroutine(AddScore());
        }
        else
        {
            StopAllCoroutines();
            GameOver();
        }
       
    }

    void GameOver()
    {
        retryUI.SetActive(true);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // 최고 점수 갱신
        if (highScore < _score)
        {
            PlayerPrefs.SetInt("HighScore", _score);
            highScoreText.text = ConvertScore(_score);
        }
    }

    IEnumerator AddScore()
    {
        while (_gm.isPlay)
        {
            _score = _gm.GetScore();
           

            if (_score > 0)
            {
                string scoreStr = ConvertScore(_score);

                // 100점마다 깜빡임
                if (_score % 100 == 0)
                    yield return StartCoroutine(BlinkText(scoreStr));
                else
                    scoreText.text = scoreStr;
                
                // 밤 & 낮 
                if (_score % 300 == 0) // 밤모드 
                {
                    _scoreDay = _score + 200;
                    StartCoroutine(SetCameraBackGround(dayColor, nightColor));
                }
                else if (_score == _scoreDay) // 낮모드
                {
                    StartCoroutine(SetCameraBackGround(nightColor, dayColor));
                }
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    string ConvertScore(int num)
    {
        // 5자리 형태 
        return string.Format("{0:D5}", num);
    }
    
    public IEnumerator BlinkText(string scoreStr)
    {
        int cnt = 0;
        while (cnt < 3) {
            scoreText.text = "";
            yield return new WaitForSeconds (.2f);
            scoreText.text = scoreStr;
            yield return new WaitForSeconds (.2f);
            cnt++;
        }
    }

    IEnumerator SetCameraBackGround(Color color1, Color color2)
    {
        float progress = 0; //This float will serve as the 3rd parameter of the lerp function.
        float increment = smoothness/duration; //The amount of change to apply.
        while(progress < 1)
        {
            _camera.backgroundColor = Color.Lerp(color1, color2, progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }

}
