using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameEndManager : MonoBehaviour
{
    [SerializeField] GameObject _gameEndText;
    [SerializeField] GameObject _gameScoreText;
    [SerializeField] GameObject _gameHighScoreText;
    [SerializeField] GameObject _gameHighScoreTrueText;
    [SerializeField] GameObject _sceneChangeButton;
    private float _deleteTime = 1.5f;
    [SerializeField]
    private float _moveRange = 150.0f;
    void Start()
    {
        LevelManager.Instance.PlayTimeUp();
        StartCoroutine("ActiveScoreText", _deleteTime + 0.8f);
        StartCoroutine("ActiveButton", 3f);
        if(LevelManager.Instance.MaxDetroyCount <= 0)
        {
            _gameEndText.GetComponent<Text>().text = $"GameClear!";
            SaveManager.ClearFlag[(int)SaveManager.Difficult] = true;
            if (LevelManager.Instance.Score == SaveManager.HighScore)
            {
                StartCoroutine("ActiveHighScoreText", _deleteTime + 2.5f);
            }
        }
        else
        {
            _gameEndText.GetComponent<Text>().color = Color.blue;
            _gameEndText.GetComponent<Text>().text = $"TimeUp!";
        }

        
    }

    // Update is called once per frame
    private float _timeCount;
    void Update()
    {
        _timeCount += Time.deltaTime;
        if(!(_deleteTime < _timeCount))
        _gameEndText.transform.localPosition += new Vector3(0, _moveRange / _deleteTime * Time.deltaTime, 0);
    }

    IEnumerator ActiveScoreText(int second)
    {
        yield return new WaitForSeconds(second);
        LevelManager.Instance.PlayScore();
        _gameScoreText.SetActive(true);
        
    }
    IEnumerator ActiveHighScoreText(int second)
    {
        yield return new WaitForSeconds(second);
        LevelManager.Instance.PlayHighScore();
        _gameHighScoreTrueText.SetActive(true);
        _gameHighScoreText.GetComponent<Text>().text = $"ハイスコア:{SaveManager.HighScore}";
    }
    IEnumerator ActiveButton(int second)
    {
        yield return new WaitForSeconds(second);
        _sceneChangeButton.SetActive(true);
    }
    public void Drop()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
