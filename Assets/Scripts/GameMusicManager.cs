using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoBehaviour
{
    static private GameMusicManager instance;
    public static GameMusicManager Instance => instance;
    public AudioSource BackMusic;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        BackMusic = GetComponent<AudioSource>();
        print(BackMusic);
    }


    // Update is called once per frame
    void Update()
    {
        if(UIManager.Instance != null)
        {
            if (LevelManager.Instance.GameOver == true)
            {
                BackMusic.Stop();
            }
            if (UIManager.Instance.TimeLimit < 30)
            {
                BackMusic.pitch = 1.03f;
            }
            if (UIManager.Instance.TimeLimit < 20)
            {
                BackMusic.pitch = 1.06f;
            }
            if (UIManager.Instance.TimeLimit < 10)
            {
                BackMusic.pitch = 1.1f;
            }
            if (UIManager.Instance.TimeLimit < 5)
            {
                BackMusic.pitch = 1.15f;
            }
            if (UIManager.Instance.TimeLimit < 2)
            {
                BackMusic.pitch = 1.2f;
            }
        }
    }

    public void PlayBackMusic()
    {
        BackMusic.Play();
    }
}
