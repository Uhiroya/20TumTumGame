using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadeScoreUI : MonoBehaviour
{
    [SerializeField]
    private Vector3 _targetdiff;
    [SerializeField]
    private float _deleteTime = 1.5f;
    [SerializeField]
    private float _moveRange = 50.0f;
    [SerializeField]
    private float _moveTime = 1.0f;
    [SerializeField]
    private float _endAlpha = 0.0f;

    private float _timeCount = 0.0f;
    [SerializeField]
    private Text _text;

    private GameObject _target;
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("ScoreUI");
        Destroy(gameObject, _deleteTime - 0.8f);
    }
    public void Initialize(int score)
    {
        if(score > 10000)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 0);
        }
        else if (score >1000)
        {
            transform.localScale = new Vector3(2, 2, 0);
        }
        else if (score > 500)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 0);
        }
        _text.text = score.ToString();
    }
    void FixedUpdate()
    {
        _timeCount += Time.deltaTime;
        if (_timeCount < _moveTime)
        {
            gameObject.transform.localPosition += new Vector3(0, _moveRange / _moveTime * Time.deltaTime, 0);
        }
        else
        {
            gameObject.transform.position = gameObject.transform.position + ( (_target.transform.position + _targetdiff) - gameObject.transform.position )* (_timeCount / _deleteTime);
        }
        
        float _alpha = 1.0f - (1.0f - _endAlpha) * (_timeCount / _deleteTime);
        if (_alpha <= 0.0f) _alpha = 0.0f;
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _alpha);
    }

}
