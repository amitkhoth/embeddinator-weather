﻿using System;
using System.Json;
using System.Net;


public class XAMWeatherFetcher
{

    static string urlTemplate = @"https://query.yahooapis.com/v1/public/yql?q=select%20item.condition%20from%20weather.forecast%20where%20woeid%20in%20(select%20woeid%20from%20geo.places(1)%20where%20text%3D%22{0}%2C%20{1}%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys";
    public string City { get; private set; }
    public string State { get; private set; }

    public XAMWeatherFetcher(string city, string state)
    {
        City = city;
        State = state;
    }

    public XAMWeatherResult GetWeather()
    {
        try
        {
            if(string.IsNullOrWhiteSpace(City) || string.IsNullOrWhiteSpace(State))
                return null;
            
            using (var wc = new WebClient())
            {
                var url = string.Format(urlTemplate, City, State);
                var str = wc.DownloadString(url);
                var json = JsonValue.Parse(str)["query"]["results"]["channel"]["item"]["condition"];
                var result = new XAMWeatherResult(json["temp"], json["text"], City, State);
                return result;
            }
        }
        catch (Exception ex)
        {
            // Log some of the exception messages
#if __ANDROID__
            Android.Util.Log.Error("XAMWeather", ex.ToString());
#else
            Console.WriteLine(ex.ToString());
#endif

            return null;
        }

    }
}
