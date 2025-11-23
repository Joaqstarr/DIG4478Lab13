using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager :MonoBehaviour {

    
    [SerializeField]
    private string city = "Orlando";

    [Header("Scene Assignments")]
    public Light sunLight;
    
    [Header("Weather Configuration")]
    public List<WeatherAsset> weatherAssets; 
    public Material defaultSkybox;

    [System.Serializable]
    public struct WeatherAsset {
        public string weatherCondition; 
        public Material skyboxMaterial;
        public Material skyboxNightMaterial;
        public Color sunColor;
        public Color sunColorNight;
        
    }

    private void Start()
    {
        UpdateWeather();
    }

    [ContextMenu("Update Weather")]
    private void UpdateWeather()
    {
        StartCoroutine(GetWeather(city, OnWeatherDataLoaded));
    }

    public IEnumerator GetWeather(string city, Action<string> callback) {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={ApiSecrets.WEATHER_KEY}";
        using (UnityWebRequest request = UnityWebRequest.Get(url)) {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success) {
                callback(request.downloadHandler.text);
            } else {
                Debug.LogError(request.error);
            }
        }
    }

    public void OnWeatherDataLoaded(string data) {
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(data);

        bool isDaytime = CheckDayNightCycle(info);

        UpdateSceneVisuals(info.weather[0].main, isDaytime);
        
        Debug.Log($"Location Time: {GetLocalTime(info.timezone)} | Condition: {info.weather[0].main}");
    }

    private bool CheckDayNightCycle(WeatherInfo info) {
        if (info.dt > info.sys.sunrise && info.dt < info.sys.sunset) {
            return true;
        }
        return false; 
    }

    private DateTime GetLocalTime(int timezoneOffset) {
        DateTime utcTime = DateTime.UtcNow;
        return utcTime.AddSeconds(timezoneOffset);
    }

    private void UpdateSceneVisuals(string weatherCondition, bool isDay) {
        
        if (isDay) {
            sunLight.intensity = 1.0f;
            RenderSettings.ambientIntensity = 1.0f;
        } else {
            sunLight.intensity = 0.1f;
            RenderSettings.ambientIntensity = 0.2f;

        }

        bool foundMatch = false;
        foreach (var asset in weatherAssets) {
            if (asset.weatherCondition == weatherCondition) {
                RenderSettings.skybox = (isDay)? asset.skyboxMaterial : asset.skyboxNightMaterial;
                sunLight.color = (isDay)?asset.sunColor : asset.sunColorNight;
                foundMatch = true;
                break;
            }
        }

        if (!foundMatch) {
            RenderSettings.skybox = defaultSkybox;
        }
        
        DynamicGI.UpdateEnvironment();
    }
}

[System.Serializable]
public class WeatherInfo {
    public List<WeatherDescription> weather;
    public MainData main;
    public SysData sys;
    public int timezone;
    public long dt; 
}

[System.Serializable]
public class WeatherDescription {
    public string main;
}

[System.Serializable]
public class MainData {
    public float temp;
}

[System.Serializable]
public class SysData {
    public long sunrise;
    public long sunset;
}