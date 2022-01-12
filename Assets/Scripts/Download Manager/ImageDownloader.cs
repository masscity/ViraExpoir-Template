using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DownloadHelper))]
public class ImageDownloader : MonoBehaviour
{
    [SerializeField, TextArea(3, 4)] string csvUrl;
    [SerializeField] Image targetImage;
    [SerializeField] DownloadHelper downloadHelper;

    [SerializeField] List<UrlsReceived> listOfLink;

    private void Awake()
    {
        downloadHelper = GetComponent<DownloadHelper>();

        downloadHelper.Get(csvUrl, (string error) =>
        {
            Debug.Log("ERROR: " + error);
        }, (string text) =>
        {
            Debug.Log("Received: " + text);
            CsvToUrl(text);
            for (int i = 0; i < listOfLink.Count; i++)
            {
                int i2 = i;
                if (listOfLink[i2].id == targetImage.name)
                {
                    downloadHelper.GetTexture(listOfLink[i2].url, (string error) =>
                    {
                        Debug.LogError("Unable get Texture: " + error);
                    }, (Texture2D texture2D) =>
                    {
                        Debug.Log("Success receiving image");
                        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(.5f, .5f));
                        targetImage.sprite = sprite;
                    });
                }
            }
        });

        
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
}
