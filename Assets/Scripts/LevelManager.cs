using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class LevelManager : MonoBehaviour
{
    
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject _ball ;
    [SerializeField] Text _sumBallNumberText;

    [SerializeField] Transform _field;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] GameObject _destroyParticle;
    [SerializeField] FadeScoreUI _fadeScore;
    
    public int MaxBallNumber = 40;
    
    public int BallDestroyNumber = 20;
    public float BallConnectRenge = 1.5f;
    
    public int MaxDetroyCount { get; private set; } = 50;

    public int Score = 0;
    public bool GameOver = true;
    ///<summary>シングルトンのインスタンス<summary>
    private static LevelManager instance;
    public static LevelManager Instance => instance;
    private List<BallManager> _selectBallList= new();
    private Queue<int> _addScoreQueue= new();

    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioSource _audioSource2;
    [SerializeField] AudioClip[] _audioClips;
    [SerializeField] AudioClip[] _audioClips2;
    [SerializeField] AudioClip[] _audioPianoClips;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        print($"現在の難易度は{SaveManager.Difficult}です");
        switch (SaveManager.Difficult)
        {
            case SaveManager.Difficulty.Easy:
                MaxDetroyCount = 30;
                break;
            case SaveManager.Difficulty.Noramal:
                MaxDetroyCount = 50;
                break;
            case SaveManager.Difficulty.Hard:
                MaxDetroyCount = 70;
                break;
            case SaveManager.Difficulty.Expert:
                MaxDetroyCount = 90;
                break;
        }
        BallSpawn(40);
    }
    // Update is called once per frame
    void Update()
    {
        LineRendererUpdate();
    }
    //ボールの生成
    private void BallSpawn(int count)
    {
        for(int i = 0; i < count; i++) 
        {
            float random =  Random.Range(-0.1f , 0.1f);
            Vector3 pos = new Vector3(i % 5 - 2 + random, i / 5 + 5, 0);
            Instantiate(_ball , pos , Quaternion.identity, _field).GetComponent<BallManager>().Initialize(BallProbability(Random.Range(0, 100)));
        }
    }
    private int BallProbability(int rand)
    {

        if(rand < 10)
        {
            return 1;
        }
        else if (rand < 20)
        {
            return 2;
        }
        else if (rand < 35)
        {
            return 3;
        }
        else if (rand < 55)
        {
            return 4;
        }
        else if (rand < 75)
        {
            return 5;
        }
        else if (rand < 85)
        {
            return 6;
        }
        else if (rand < 90)
        {
            return 7;
        }
        else if (rand < 95)
        {
            return 8;
        }
        else
        {
            return 9;
        }

    }
    public void BallEnter(BallManager ball)
    {
        float Length;
        if (_selectBallList.Count == 0){
            return;
        }
        if (!ball.IsSelect && (BallSumNumber() <= BallDestroyNumber || ball.BallNumber < 0))
        {
            _audioSource.PlayOneShot(_audioClips[0]);
            Length = (_selectBallList[_selectBallList.Count - 1 ].transform.position 
                - ball.transform.position).magnitude;
            if(Length < BallConnectRenge) 
            {
                _selectBallList.Add(ball);
                ball.SetIsSelect(true);
            }
        }
        else
        {
            
            if (_selectBallList.Count - 2 >= 0){
                if ((_selectBallList[_selectBallList.Count - 2].transform.position
                    - ball.transform.position).magnitude < ball.transform.localScale.x)
                {
                    _audioSource.PlayOneShot(_audioClips[1]);
                    _selectBallList[_selectBallList.Count - 1].SetIsSelect(false);
                    _selectBallList.Remove(_selectBallList[_selectBallList.Count - 1]);
                }
            }

        }
    }
    public void BallDown(BallManager ball)
    {
        if (!GameOver)
        {
            _selectBallList.Add(ball);
            ball.SetIsSelect(true);
        }
    }
    public void BallUp()
    {
        print(BallSumNumber());
        if(BallSumNumber() == BallDestroyNumber && !GameOver)
        {
            RemoveBalls();
        }
        else
        {
            foreach (BallManager ball in _selectBallList)
            {
                ball.SetIsSelect(false);
            }
        }
        _selectBallList.Clear();
    }
    private int BallSumNumber()
    {
        int sum = 0;
        foreach (BallManager ball in _selectBallList)
        {
            sum += ball.BallNumber;
        }
        return sum;
    }
    private void RemoveBalls()
    {
        List<BallManager> _deleteBallList = new();
        foreach (BallManager ball in _selectBallList)
        {
            _deleteBallList.Add(ball);
        }
        StartCoroutine("DestroyBalls", _deleteBallList);
        BallSpawn(_selectBallList.Count);
        MaxDetroyCount -= _selectBallList.Count;
        AddScore(_selectBallList.Count, _selectBallList);
        _selectBallList.Clear();
    }
    IEnumerator DestroyBalls(List<BallManager> balls)
    {
        foreach(BallManager ball in balls)
        {
            yield return new WaitForSeconds(0.2f);
            _audioSource2.pitch += 0.05f;
            _audioSource2.PlayOneShot(_audioClips2[0]);
            
            Instantiate(_fadeScore, RectTransformUtility.WorldToScreenPoint(Camera.main, ball.transform.position),
               Quaternion.identity, Canvas.transform).Initialize(_addScoreQueue.Dequeue());
            Instantiate(_destroyParticle,ball.transform.position,Quaternion.identity,_field);
            Destroy(ball.gameObject);
        }
        yield return new WaitForSeconds(0.2f);
        _audioSource2.pitch = 1f;
    }
    private int _lineColorMode;
    private float _lineColorAlpha = 0.4f;
    private void LineRendererUpdate()
    {
        if (_selectBallList.Count >= 2)
        {
            _lineRenderer.positionCount = _selectBallList.Count;
            _lineRenderer.SetPositions(_selectBallList.Select(ball => ball.transform.position)?.ToArray());
            _lineRenderer.gameObject.SetActive(true);
        }
        else
        {
            _lineRenderer.gameObject.SetActive(false);
        }
        if(BallSumNumber() < 20 && _lineColorMode != 1)
        {
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lineRenderer.startColor = new Color(Color.blue.r, Color.blue.g, Color.blue.b, _lineColorAlpha);
            _lineRenderer.endColor = new Color(Color.blue.r, Color.blue.g, Color.blue.b, _lineColorAlpha);
            _sumBallNumberText.color = Color.white;
            _lineColorMode = 1;
        }
        else if(BallSumNumber() == 20 && _lineColorMode != 2)
        {
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lineRenderer.startColor = new Color(Color.green.r, Color.green.g, Color.green.b, _lineColorAlpha);
            _lineRenderer.endColor = new Color(Color.green.r, Color.green.g, Color.green.b, _lineColorAlpha);
            _sumBallNumberText.color = Color.green;
            _lineColorMode = 2;
        }
        else if(BallSumNumber() > 20 && _lineColorMode != 3)
        {
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lineRenderer.startColor = new Color(Color.red.r, Color.red.g, Color.red.b, _lineColorAlpha);
            _lineRenderer.endColor = new Color(Color.red.r, Color.red.g, Color.red.b, _lineColorAlpha);
            _sumBallNumberText.color =Color.red;
            _lineColorMode = 3;
        }
        _sumBallNumberText.text = BallSumNumber().ToString();
    }
    private void AddScore(int DestroyNum,List<BallManager> destroyList)
    {
        int addScore = 10;
        for(int i = 0; i < DestroyNum; i++)
        {
            int Ballnum = destroyList[i].BallNumber;
            Score += addScore * Ballnum;
            _addScoreQueue.Enqueue(addScore * Ballnum);
            addScore = addScore * 2 + Ballnum;
        }
    }
    public void PlayCountDown()
    {
        _audioSource.PlayOneShot(_audioClips[2]);
    }
    public void PlayTimeUp()
    {
        _audioSource.PlayOneShot(_audioClips[3]);
    }
    public void PlayScore()
    {
        _audioSource.PlayOneShot(_audioClips[4]);
    }
    public void PlayHighScore()
    {
        _audioSource.PlayOneShot(_audioClips[5]);
    }
}
