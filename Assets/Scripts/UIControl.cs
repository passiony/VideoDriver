using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    private static UIControl _instance;
    public static UIControl Instance => _instance;

    [Header("StartPanel")] [SerializeField]
    private GameObject StartPanel;

    [SerializeField] private Button TestBtn;
    [SerializeField] private Button StartBtn;

    [Header("GroupPanel")] [SerializeField]
    private GameObject GroupPanel;

    [SerializeField] private Button[] m_Menus;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        TestBtn.onClick.AddListener(this.OnTestClick);
        StartBtn.onClick.AddListener(this.OnStartClick);

        for (int i = 0; i < m_Menus.Length; i++)
        {
            int index = i;
            m_Menus[i].onClick.AddListener(() => { this.OnGroupClick(index); });
        }
    }

    private void OnTestClick()
    {
        StartPanel.SetActive(false);
        GroupPanel.SetActive(false);
        DriverManager.Instance.SetMode(0,true);
    }

    private void OnStartClick()
    {
        ShowGroup();
    }

    private void OnGroupClick(int index)
    {
        GroupPanel.SetActive(false);

        DriverManager.Instance.SetMode(index);
    }

    public void ShowGroup()
    {
        StartPanel.SetActive(false);
        GroupPanel.SetActive(true);
    }
}