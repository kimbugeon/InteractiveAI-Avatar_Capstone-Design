using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TextToSpeech : MonoBehaviour
{
    private string ttsApiUrl = "http://localhost:6001/speak";

    public void Speak(string text)
    {
        StartCoroutine(SendTextToSpeech(text));
    }

    private IEnumerator SendTextToSpeech(string text)
    {
        var requestData = "{\"text\": \"" + text + "\"}";

        using (var request = new UnityWebRequest(ttsApiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(requestData);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                Debug.Log("TTS request succeeded");
            }
        }
    }
}
