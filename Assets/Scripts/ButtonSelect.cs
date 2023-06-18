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
    private void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        _selectSprite = GetComponent<Image>();
    }
    public void Update()
    {
        MousePos = Input.mousePosition;

        /*
         * Mouse_Image‚ð•\Ž¦‚·‚éˆÊ’u‚ÉMousePos‚ðŽg‚¤
         */

        if (EventSystem.current.IsPointerOverGameObject())
        {
            if(Mathf.Abs(transform.position .x - MousePos.x) <= 130
                && Mathf.Abs(transform.position.y - MousePos.y) <= 130)
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
