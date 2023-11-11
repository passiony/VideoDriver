using System;
using System.Collections;
using HeatMap2D;
using UnityEngine;

public class MainHead : MonoBehaviour
{
    [SerializeField] private Transform m_Camera;
    [SerializeField] private HeatMap2D_My m_HeatMap;
    [SerializeField] private CaptureUtil m_CaptureUtil;
    private bool Pause = true;
    private MeshRenderer m_HeatMesh;

    private void Awake()
    {
        m_HeatMesh = m_HeatMap.GetComponent<MeshRenderer>();
    }

    public void StartGame()
    {
        m_HeatMesh.enabled = false;
        Pause = false;
        m_HeatMap.pointsCount = 0;
    }

    public void StopGame()
    {
        m_HeatMesh.enabled = true;
        Pause = true;
    }

    public IEnumerator CoCapture(int group)
    {
        Debug.Log("播放完成，开始截图");

        m_HeatMesh.enabled = true;
        string fileName = $"heatmap_group{group}_" + DateTime.Now.ToString("yyyyMMddHHmmssffff");
        m_CaptureUtil.CaptureToLocal(fileName, 2048, 1024);

        yield return new WaitForSeconds(0.1f);
        m_HeatMesh.enabled = false;
    }

    void FixedUpdate()
    {
        if (Pause)
        {
            return;
        }

        DrawHeatPoint();
    }

    void DrawHeatPoint()
    {
        int layer = 1 << LayerMask.NameToLayer("HeatMap");
        Debug.DrawLine(m_Camera.position, m_Camera.position + m_Camera.forward * 100, Color.red);
        if (Physics.Raycast(m_Camera.position, m_Camera.forward, out RaycastHit hit, 100, layer))
        {
            Vector4 point = hit.point;
            point.w = 1.0f;
            m_HeatMap.AddPoint(point);
        }
    }
}