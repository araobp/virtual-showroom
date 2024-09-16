using System;
using Microsoft.Unity.VisualStudio.Editor;

[Serializable]
public class ChatResponse
{
    public string query;
    public string answer;
    public string b64audio;
}

public class ChatImage{
    public string b64image;
}

public class MoodResponse {
    public string mood;
}
