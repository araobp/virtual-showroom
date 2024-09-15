using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using static System.Web.HttpUtility;

public class ChatAPI : RestClient
{
    EndPoint m_EndPoint;

    // Callbacks
    public delegate void HelloCallback(bool err, string text);
    public delegate void ChatCallback(bool err, ChatResponse resp);
    public delegate void MoodCallback(bool err, MoodResponse resp);

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

    public void ChatTextOnly(string query, string imageId, string voice, ChatCallback callback)
    {
        string voiceUrlParam = voice == null ? "" : $"&voice={voice}";

        Get(m_EndPoint, $"/chat_with_image?query={query}&image_id={imageId}{voiceUrlParam}", (err, text) =>
        {
            ChatResponse resp = JsonUtility.FromJson<ChatResponse>(text);
            callback(err, resp);
        });
    }

    public void ChatTextAndImage(string query, string imageId, string b64image, string voice, ChatCallback callback)
    {
        string voiceUrlParam = voice == null ? "" : $"&voice={voice}";

        ChatImage chatImage = new ChatImage();
        chatImage.b64image = b64image;
        string jsonBody = JsonUtility.ToJson(chatImage);
        Put(m_EndPoint, $"/chat_with_image?query={query}&image_id={imageId}{voiceUrlParam}", jsonBody, (err, text) =>
        {
            ChatResponse resp = JsonUtility.FromJson<ChatResponse>(text);
            callback(err, resp);
        });
    }

    public void MoodJudgement(string b64image, MoodCallback callback) {
        ChatImage chatImage = new ChatImage();
        chatImage.b64image = b64image;
        string jsonBody = JsonUtility.ToJson(chatImage);
        Put(m_EndPoint, $"/mood_judgement", jsonBody, (err, text) =>
        {
            MoodResponse resp = JsonUtility.FromJson<MoodResponse>(text);
            callback(err, resp);
        });
    }

    // OpenAI's TTS Service
    public IEnumerator TextToSpeech(AudioSource audioSource, string voice, string text)
    {
        string urlParam = $"voice={voice}&text={UrlEncode(text)}";
        string url = $"{m_EndPoint.baseUrl}{"/tts"}?{urlParam}";
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            Debug.Log("tts");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();
            }
        }
    }

}
