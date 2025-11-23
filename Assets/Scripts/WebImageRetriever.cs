using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
 

public class WebImageRetriever : MonoBehaviour
{
    private const string webImage = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/Cat_August_2010-4.jpg/2560px-Cat_August_2010-4.jpg";
    public static Texture2D downloadedTexture;
    [SerializeField]
    private MeshRenderer[] meshes;
    
    void Start()
    {
        Action<Texture2D> callbackAction = (texture) =>
        {
            downloadedTexture = texture;
            SetMeshTexture(meshes, downloadedTexture);
        };
        
        GetWebImage(callbackAction);
    }

    private void GetWebImage(Action<Texture2D> callback)
    {
        if (downloadedTexture != null)
        {
            callback(downloadedTexture);
        }
        else
        {
            StartCoroutine(DownloadImage(callback));
        }
    }

    public IEnumerator DownloadImage(Action<Texture2D> callback) { 

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage);
        yield return request.SendWebRequest();
        callback(DownloadHandlerTexture.GetContent(request));
        Debug.Log("Image Downloaded");
    }

    private void SetMeshTexture(MeshRenderer[] meshes, Texture2D texture)
    {
        foreach (var mesh in meshes)
        {
            mesh.material.mainTexture = texture;
        }
    }
}
