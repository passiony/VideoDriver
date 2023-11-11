using System;
using System.IO;
using UnityEngine;

public class CaptureUtil : MonoBehaviour
{
    private Camera m_PhotonCamera;

    private void Awake()
    {
        m_PhotonCamera = gameObject.GetComponent<Camera>();
    }

    /// 生成相机照片并保存
    public void CaptureToLocal(string fileName,int width, int height)
    {
        gameObject.SetActive(true);
        RenderTexture rt = RenderTexture.GetTemporary(width, height, 16, RenderTextureFormat.ARGB32);
        m_PhotonCamera.targetTexture = rt;
        m_PhotonCamera.Render();
        RenderTexture.active = rt;

        Texture2D tex2d = new Texture2D(width, height, TextureFormat.ARGB32, false);
        tex2d.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex2d.Apply();
        tex2d.name = fileName;
        
        //保存图像到本地文件夹中
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        //数据写入
        byte[] bytes = tex2d.EncodeToPNG();
        if (bytes != null)
        {
            string savePath = Application.persistentDataPath + "\\" + tex2d.name + ".png";
            Debug.Log("图片保存成功：" + savePath);
            File.WriteAllBytes(savePath, bytes);
        }

        m_PhotonCamera.targetTexture = null;
        RenderTexture.ReleaseTemporary(rt);
        gameObject.SetActive(false);
    }
}