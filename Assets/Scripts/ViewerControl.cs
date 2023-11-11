using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ViewerControl : MonoBehaviour
{
    public CaptureUtil m_CaptureUtil;

    private int[] ViewNum = { 0, 0, 0 };
    private Transform m_Camera;

    public TextMeshProUGUI[] TextArray1;
    public TextMeshProUGUI[] TextArray2;
    public TextMeshProUGUI[] TextArray3;
    public TextMeshProUGUI[] TextArray4;

    private List<TextMeshProUGUI[]> Array;

    public bool Pause = true;

    void Start()
    {
        m_Camera = Camera.main.transform;
        Array = new List<TextMeshProUGUI[]>();
        Array.Add(TextArray1);
        Array.Add(TextArray2);
        Array.Add(TextArray3);
        Array.Add(TextArray4);

        ClearText();
        Pause = true;
    }

    void ClearText()
    {
        for (int i = 0; i < ViewNum.Length; i++)
        {
            ViewNum[i] = 0;
        }
        foreach (var array in Array)
        {
            foreach (var tmp in array)
            {
                tmp.text = "";
            }
        }
    }

    void FixedUpdate()
    {
        if (Pause) return;

        int layer = 1 << LayerMask.NameToLayer("Viewer");
        Debug.DrawLine(m_Camera.position, m_Camera.position + m_Camera.forward * 100, Color.red);
        if (Physics.Raycast(m_Camera.position, m_Camera.forward, out RaycastHit hit, 100, layer))
        {
            var name = hit.collider.name;
            var id = int.Parse(name);
            ViewNum[id - 1]++;
        }
    }

    public void StartGame()
    {
        Pause = false;
    }

    public void StopGame()
    {
        Pause = true;
    }

    public IEnumerator CoCapture(int group)
    {
        float total = ViewNum[0] + ViewNum[1] + ViewNum[2];
        Array[group][0].text = (ViewNum[0] / total).ToString("P") + "\n" + $"{ViewNum[0]}/{total}";
        Array[group][1].text = (ViewNum[1] / total).ToString("P") + "\n" + $"{ViewNum[1]}/{total}";
        Array[group][2].text = (ViewNum[2] / total).ToString("P") + "\n" + $"{ViewNum[2]}/{total}";

        Debug.Log($"viewnum:{ViewNum[0]},{ViewNum[1]},{ViewNum[2]}");
        string fileName = $"look_group{group}_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
        m_CaptureUtil.CaptureToLocal(fileName, 2048, 1024);
        yield return new WaitForSeconds(0.1f);
        
        ClearText();
    }
}