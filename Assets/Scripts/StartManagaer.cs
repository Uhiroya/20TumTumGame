using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartManagaer : MonoBehaviour
{
    [SerializeField] GameObject[] _buttonObjs = null;
    [SerializeField] GameObject[] _clearObjs  = null;

    static private StartManagaer instance;
    public static StartManagaer Instance => instance;
    
    [SerializeField] AudioClip _audioClip = null;
    AudioSource _audioSource = null;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        _audioSource = GetComponent<AudioSource>(); 
        for(int i=0; i < _clearObjs.Length; i++)
        {
            if (SaveManager.ClearFlag[i] == true)
            {
                _clearObjs[i].SetActive(true);
            }
        }
        
    }

    public void GetTouch()
    {
        _audioSource.PlayOneShot(_audioClip);
    }

    public void DropButton(int buttonNum)
    {
        SaveManager.Difficult  = (SaveManager.Difficulty) buttonNum;
        SceneManager.LoadScene("GameScene");
    }
}
