using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas _endCanvas;
    [SerializeField] GameObject _startCountDownObj;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _destroyCountText;
    [SerializeField] Text _TimeText;

    private static UIManager instance;
    public static UIManager Instance => instance;
    public float TimeLimit = 60f;
    public int _timeAddperScore = 0;
    private float _countDown = 3.8f;
    private bool _isCountDown = false;
    private Text _startCountDownText;
    private float _addTime = 5f;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        _startCountDownText = _startCountDownObj.GetComponent<Text>();
        StartCoroutine("StartCountDown", _countDown);
        _TimeText.text = TimeLimit.ToString("0");
        _scoreText.text = $"スコア: {LevelManager.Instance.Score}";
        _destroyCountText.text = $"{LevelManager.Instance.MaxDetroyCount}";
    }
    IEnumerator StartCountDown(int second)
    {
        LevelManager.Instance?.PlayCountDown();
        _isCountDown = true;
        yield return new WaitForSeconds(second);
        _startCountDownObj.SetActive(false);
        LevelManager.Instance.GameOver = false;
        GameMusicManager.Instance.PlayBackMusic();
    }
    // Update is called once per frame
    void Update()
    {
        if (!LevelManager.Instance.GameOver)
        {
            if( LevelManager.Instance.Score / 10000 > _timeAddperScore)
            {
                TimeLimit += _addTime;
                _timeAddperScore += 1;
            }
            //時間切れ処理
            if (TimeLimit >= 0)
            {
                TimeLimit -= Time.deltaTime;
                _TimeText.text = TimeLimit.ToString("0");

            }
            else
            {
                if (LevelManager.Instance.Score > SaveManager.HighScore)
                {
                    SaveManager.HighScore = LevelManager.Instance.Score;
                }
                LevelManager.Instance.GameOver = true;
                Instantiate(_endCanvas);
            }
            //目標破壊個数の処理
            if (LevelManager.Instance.MaxDetroyCount <= 0)
            {
                _destroyCountText.text = "0";
            }
            else
            {
                _destroyCountText.text = LevelManager.Instance.MaxDetroyCount.ToString();
            }

        }
        else
        {
            if (_isCountDown)
            {
                //カウントダウン
                if (_countDown < 0.6f)
                {
                    _startCountDownText.text = "Start!";
                    _countDown -= Time.deltaTime;
                }
                else
                {
                    _startCountDownText.text = _countDown.ToString("0");
                    _countDown -= Time.deltaTime;
                }
            }

        }
    }
    private int _curentScore = 0;
    private void FixedUpdate()
    {
        if(LevelManager.Instance.Score - _curentScore <= 5)
        {
            _curentScore = LevelManager.Instance.Score;
        }
        else if (LevelManager.Instance.Score - _curentScore < 1000)
        {
            _curentScore += 10;
        }
        else if (_curentScore < LevelManager.Instance.Score)
        {
            _curentScore += LevelManager.Instance.Score / 60;
        }
        else
        {
            _curentScore = LevelManager.Instance.Score;
        }
        if(_curentScore != LevelManager.Instance.Score)
        {
            _scoreText.transform.localScale = new Vector3(Random.Range(1.1f,1.15f), Random.Range(1.1f, 1.2f), 0);
        }
        else
        {
            _scoreText.transform.localScale = new Vector3(1, 1, 1);
        }
        _scoreText.text = $"スコア: {_curentScore}";
    }

}
