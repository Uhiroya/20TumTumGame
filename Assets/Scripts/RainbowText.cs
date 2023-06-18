using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RainbowText : MonoBehaviour
{
    Text text;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void FixedUpdate()
    {
        text.color = Color.HSVToRGB((Time.time / 2 )  % 1, 1, 1);
    }

}
