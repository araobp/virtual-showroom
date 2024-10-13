using System.Collections.Generic;

public static class Metadata
{
    // picture file name: [context, place]
    static public Dictionary<string, List<string>> METADATA = new Dictionary<string, List<string>>()
        {
            {"IMG_0227", new List<string>{"yokohama", "Rinko Park"}},
            {"IMG_0230", new List<string>{"yokohama", "Yamashita Rinkō Line Promenade"}},
            {"IMG_0232", new List<string>{"yokohama", "Yokohama International Passenger Terminal"}},
            {"IMG_0236", new List<string>{"yokohama", "Yokohama International Passenger Terminal"}},
            {"IMG_0238", new List<string>{"yokohama", "Yokohama International Passenger Terminal"}},
            {"IMG_0248", new List<string>{"takanawa_gateway_station", "JR Takanawa Gateway Station"}},
            {"IMG_0254", new List<string>{"takanawa_gateway_station", "JR Takanawa Gateway Station"}},
            {"IMG_0256", new List<string>{"takanawa_gateway_station", "JR Takanawa Gateway Station"}},
            {"PolyHaven_Hansaplatz", new List<string>{"hansaplatz", "Hanzaplatz"}},
            {"PolyHaven_DresdenStation", new List<string>{"dresden_station", "Dresden Central Station"}},
            {"PolyHaven_HamburgStation", new List<string>{"hamburg_station", "Hamburg Central Station"}}
        };
}
