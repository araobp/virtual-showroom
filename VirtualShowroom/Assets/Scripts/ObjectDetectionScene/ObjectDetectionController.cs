using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectDetectionController : MonoBehaviour
{
    [SerializeField] GameObject m_Screen;

    [SerializeField] RenderTexture m_RobotCameraRenderTexture;

    List<Texture2D> m_ContentList;
    int m_ContentIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        List<UnityEngine.Object> pictures = Resources.LoadAll("LargeDisplay", typeof(Texture2D)).ToList();
        m_ContentList = pictures.OrderBy(x => x.name).ToList().ConvertAll(x => (Texture2D)x);
        m_Screen.GetComponent<Renderer>().material.SetTexture("_Texture2D", m_ContentList[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
