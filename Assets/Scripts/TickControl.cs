using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TickControl : MonoBehaviour
{
    public Button m_Company;
    public Button m_TickBtn;
    public Button m_BackBtn;
    
    public GameObject m_TickTok;
    
    void Start()
    {
        m_Company.onClick.AddListener(OnCompanyClick);
        m_TickBtn.onClick.AddListener(OnTickTokClick);
        m_BackBtn.onClick.AddListener(OnBackClick);
    }

    private void OnTickTokClick()
    {
        m_TickTok.SetActive(true);
    }

    private void OnBackClick()
    {
        m_TickTok.SetActive(false);
    }

    private void OnCompanyClick()
    {
        DriverManager.Instance.StartGame();
    }

}
