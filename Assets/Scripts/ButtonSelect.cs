using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonSelect : MonoBehaviour
{
    public Canvas canvas;
    public RectTransform canvasRect;
    public Vector2 MousePos;
    private Image _selectSprite;
    private RectTransform _myRectTransform;
    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        _selectSprite = GetComponent<Image>();
        _myRectTransform = GetComponent<RectTransform>();
    }
    public void Update()
    {
        MousePos = Input.mousePosition;

        /*
         * Mouse_Image‚ð•\Ž¦‚·‚éˆÊ’u‚ÉMousePos‚ðŽg‚¤
         */

        if (EventSystem.current.IsPointerOverGameObject())
        {
            if(Mathf.Abs(transform.position .x - MousePos.x) <= _myRectTransform.sizeDelta.x / 2
                && Mathf.Abs(transform.position.y - MousePos.y) <= _myRectTransform.sizeDelta.y / 2)
            {
                if(_selectSprite.enabled == false)
                {
                    StartManagaer.Instance.GetTouch();
                    _selectSprite.enabled = true;
                }
            }
            else
            {
                _selectSprite.enabled = false;
            }
            
        }
        else
        {
            _selectSprite.enabled = false;
        }
    }
}
