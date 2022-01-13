using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DownloadHelper))]
public class OpenNewTabUrls : MonoBehaviour
{

    [SerializeField] bool isChangeable = false;

    [Header("Changeable"), Space(5)]
    [SerializeField, TextArea(3, 4)] string csvUrl;
    [SerializeField] List<UrlsReceived> listOfLink;
    [SerializeField] Button[] buttons;
    private DownloadHelper downloadHelper;

    private void Awake()
    {
        if (isChangeable)
        {
            downloadHelper = GetComponent<DownloadHelper>();

            downloadHelper.GetCsvData(csvUrl, (string error) =>
            {
                Debug.Log("ERROR: " + error);
            }, (string text) =>
            {
                Debug.Log("Received: " + text);
                CsvToUrl(text);
                for (int i = 0; i < listOfLink.Count; i++)
                {
                    int i2 = i;
                    for (int j = 0; j < buttons.Length; j++)
                    {
                        int j2 = j;
                        if (buttons[j2].gameObject.name == listOfLink[i2].id)
                        {
                            buttons[j2].onClick.AddListener(delegate { OpenDefaultNewtab(listOfLink[i2].url); });
                            //Debug.Log("url: " + url + " | to button: " + buttons[j2].gameObject.name);
                        }
                    }
                }
            });
        }

    }

    public void OpenDefaultNewtab(string url)
    {
        if (url is null)
        {
            throw new ArgumentNullException(nameof(url));
        }
#if !UNITY_EDITOR
		OpenNewTab(url);
#endif
#if UNITY_EDITOR
        Debug.Log("Opening new tab: " + url);
#endif
    }

    public void CsvToUrl(string csvText)
    {
        string[] lines = csvText.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (i + 1 < lines.Length)
            {
                UrlsReceived newLink = new();
                string[] data = lines[i + 1].Split(',');
                newLink.id = data[0].Trim();
                newLink.url = data[1].Trim();
                listOfLink.Add(newLink);
            }
        }
    }

    [DllImport("__Internal")]
    private static extern void OpenNewTab(string url);
}
