using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class DriverManager : MonoBehaviour
{
    private static DriverManager _instance;
    public static DriverManager Instance => _instance;

    [SerializeField] private VideoPlayer m_EnvPlayer;
    [SerializeField] private GameObject[] m_Groups;
    [SerializeField] private MainHead m_MainHead;
    [SerializeField] private ViewerControl m_Viewer;

    private bool m_Pause = true;
    private int m_Mode;
    private bool m_Test;
    private GameObject current;

    private void Awake()
    {
        _instance = this;
    }

    void OnEnable()
    {
        m_EnvPlayer.loopPointReached += OnPlayFinished;
    }

    private void OnDisable()
    {
        m_EnvPlayer.loopPointReached -= OnPlayFinished;
    }

    public void StartGame()
    {
        if (current == null)
        {
            return;
        }
        
        Debug.Log("自动驾驶开始");
        m_MainHead.StartGame();
        m_Viewer.StartGame();

        //开始播放
        var videos = current.GetComponentsInChildren<VideoPlayer>();
        foreach (var video in videos)
        {
            video.Play();
        }

        m_EnvPlayer.Play();
    }

    private void OnPlayFinished(VideoPlayer source)
    {
        StopGame();
    }

    void StopGame()
    {
        m_Pause = true;
        m_MainHead.StopGame();
        m_Viewer.StopGame();
        StopCoroutine("CoTestModeTimer");
        
        //停止播放
        var videos = current.GetComponentsInChildren<VideoPlayer>();
        foreach (var video in videos)
        {
            video.Stop();
        }

        m_EnvPlayer.Stop();

        //隐藏groups
        Debug.Log("播放完成，开始截图");
        StartCoroutine(CoCapture());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            StopGame();
        }
    }

    IEnumerator CoCapture()
    {
        yield return m_MainHead.CoCapture(m_Mode);
        yield return m_Viewer.CoCapture(m_Mode);
        UIControl.Instance.ShowGroup();
    }

    public void SetMode(int mode, bool test = false)
    {
        m_Mode = mode;
        m_Test = test;
        foreach (var group in m_Groups)
        {
            group.SetActive(false);
        }

        current = m_Groups[(int)mode];
        current.SetActive(true);
        // StartGame();
        if (test)
        {
            StartCoroutine("CoTestModeTimer");
        }
    }

    IEnumerator CoTestModeTimer()
    {
        yield return new WaitForSeconds(30);
        StopGame();
    }
}