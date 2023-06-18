using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreAnimation : MonoBehaviour
{
    [SerializeField] Text _text;
    private int _animScore;
    // Update is called once per frame
    void FixedUpdate()
    {
        if(_animScore < LevelManager.Instance.Score)
        {
            _animScore += LevelManager.Instance.Score / 90;
        }
        else
        {
            _animScore = LevelManager.Instance.Score;
        }
        _text.text = $"ƒXƒRƒA:{_animScore}";
    }
}
