using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectDetectionController : MonoBehaviour
{
    [SerializeField] GameObject m_Screen;

    [SerializeField] RenderTexture m_RobotCameraRenderTexture;

    [SerializeField] string m_BaseUrl;

    ChatAPI m_Api;

    List<Texture2D> m_ContentList;
    int m_ContentIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        List<UnityEngine.Object> pictures = Resources.LoadAll("LargeDisplay", typeof(Texture2D)).ToList();
        m_ContentList = pictures.OrderBy(x => x.name).ToList().ConvertAll(x => (Texture2D)x);
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", m_ContentList[0]);

        // Initializing API
        m_Api = GetComponent<ChatAPI>();
        m_Api.Init(m_BaseUrl);
        m_Api.Hello((err, text) =>
        {
            if (err)
            {
                Debug.LogWarning("API server not running!");
            }
            else
            {
                Debug.Log(text);
            }
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ask() {

        void ProcessChatResponse(ChatResponse resp)
        {
            Debug.Log(resp.answer);
        }


        // Read texture data from RenderTexture
        RenderTexture rt = m_RobotCameraRenderTexture;
        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;

        // Encode it to JPEG
        byte[] bytes;
        bytes = tex.EncodeToJPG();

        // Convert it to Base64
        string b64image = Convert.ToBase64String(bytes);

        string query = "What can you see in this image?";
        m_Api.ObjectDetectionChatTextAndImage(query, b64image, (err, resp) =>
            {
                if (err)
                {
                    Debug.Log("Chat failed");
                }
                else
                {
                    ProcessChatResponse(resp);
                }
            });
    }
}
