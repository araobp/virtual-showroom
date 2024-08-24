using UnityEngine;

public class ChatAPI : RestClient
{
    EndPoint m_EndPoint;

    // Callbacks
    public delegate void HelloCallback(bool err, string text);
    public delegate void ChatCallback(bool err, ChatQuery resp);

    // Start is called before the first frame update
    public void Init(string baseUrl)
    {
        // REST API client init
        m_EndPoint = new EndPoint();
        m_EndPoint.baseUrl = baseUrl;
    }

    // API server
    public void Hello(HelloCallback callback)
    {
        Get(m_EndPoint, "/", (err, text) =>
        {
            callback(err, text);
        });
    }

    public void Chat(string query, ChatCallback callback)
    {
        Get(m_EndPoint, $"/chat?query={query}", (err, text) =>
        {
            ChatQuery resp = JsonUtility.FromJson<ChatQuery>(text);
            callback(err, resp);
        });
    }

}
