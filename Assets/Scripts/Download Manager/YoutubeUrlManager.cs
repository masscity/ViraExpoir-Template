using LightShaft.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DownloadHelper))]
public class YoutubeUrlManager : MonoBehaviour
{
    enum ON_LOADED { JUST_SET_URL, PRELOAD_URL, LOAD_AND_PLAY };

    [SerializeField] ON_LOADED loadType;
    [SerializeField, TextArea(3, 4)] string csvUrl;
    [SerializeField] List<UrlsReceived> youtubeUrls;
    [SerializeField] YoutubePlayer[] youtubePlayers;
    private DownloadHelper downloadFile;

    private void Awake()
    {
        downloadFile = GetComponent<DownloadHelper>();

        downloadFile.GetCsvData(csvUrl, (string error) =>
        {
            Debug.Log("ERROR: " + error);
        }, (string text) =>
        {
            Debug.Log("Received: " + text);
            CsvToYoutubeUrl(text);
            if (loadType == ON_LOADED.LOAD_AND_PLAY)
            {
                for (int i = 0; i < youtubeUrls.Count; i++)
                {
                    for (int j = 0; j < youtubePlayers.Length; j++)
                    {
                        if (youtubePlayers[j].gameObject.name == youtubeUrls[i].id)
                        {
                            youtubePlayers[j].Play(youtubeUrls[i].url);
                        }
                    }
                }
            }
            else if (loadType == ON_LOADED.PRELOAD_URL)
            {
                for (int i = 0; i < youtubeUrls.Count; i++)
                {
                    for (int j = 0; j < youtubePlayers.Length; j++)
                    {
                        if (youtubePlayers[j].gameObject.name == youtubeUrls[i].id)
                        {
                            //youtubePlayers[j].PlayPause();
                            youtubePlayers[j].PreLoadVideo(youtubeUrls[i].url);
                        }
                    }
                }
            }
            else if (loadType == ON_LOADED.JUST_SET_URL)
            {
                for (int i = 0; i < youtubeUrls.Count; i++)
                {
                    for (int j = 0; j < youtubePlayers.Length; j++)
                    {
                        if (youtubePlayers[j].gameObject.name == youtubeUrls[i].id)
                        {
                            youtubePlayers[j].youtubeUrl = youtubeUrls[i].url;
                            youtubePlayers[j].showThumbnailBeforePlay = true;
                            youtubePlayers[j].ShowThumbnailBeforePlay(youtubeUrls[i].url);
                        }
                    }
                }
            }

        });
    }

    public void CsvToYoutubeUrl(string csvText)
    {
        string[] lines = csvText.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (i + 1 < lines.Length)
            {
                UrlsReceived youtubeEntity = new UrlsReceived();
                string[] data = lines[i + 1].Split(',');
                youtubeEntity.id = data[0];
                youtubeEntity.url = data[1];
                youtubeUrls.Add(youtubeEntity);
            }
        }
    }

    private void OnApplicationQuit()
    {
        youtubeUrls.Clear();
        Array.Clear(youtubePlayers, 0, youtubePlayers.Length);
    }

}
