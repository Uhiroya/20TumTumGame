using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BallManager : MonoBehaviour
{
    private int _ballNumber;
    public int BallNumber => _ballNumber;
    public bool IsSelect { get; private set; }
    [SerializeField] GameObject _selectSprite;
    [SerializeField] Text _numberText;
    public void Update()
    {
        if(LevelManager.Instance.GameOver == false)
        {
            this.enabled = false;
        }
    }
    public void Initialize(int ballnumber)
    {
        _ballNumber = ballnumber;
        _numberText.text = ballnumber.ToString();
        float ballscale = ballnumber * 0.03f + 0.8f;
        transform.localScale = new Vector3(ballscale, ballscale,1) ;
    }
    public BallManager(int ballNumber)
    {
        _ballNumber = ballNumber;
    }
    public void OnMouseDown()
    {
        LevelManager.Instance.BallDown(this);
    }
    public void OnMouseUp() 
    {
        LevelManager.Instance.BallUp();

    }
    public void OnMouseEnter()
    {
        LevelManager.Instance.BallEnter(this);
    }
    public void SetBallNumber(int ballNumber)
    {
        _ballNumber = ballNumber;
    }
    public void SetIsSelect(bool isSelect)
    {
        IsSelect = isSelect;
        _selectSprite.SetActive(isSelect);
    }
}
