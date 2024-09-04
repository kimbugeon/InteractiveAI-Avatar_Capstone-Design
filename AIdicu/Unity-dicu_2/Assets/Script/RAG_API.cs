using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class RAG_API : MonoBehaviour
{
    private string apiUrl = "http://localhost:5000/ask";
    public TextToSpeech textToSpeech; // TextToSpeech 스크립트를 연결합니다.

    public InputField questionInputField; // 질문 입력 필드
    public Button submitButton; // 제출 버튼
    public Text questionText; // 질문 텍스트 필드
    public Text answerText; // 답변 텍스트 필드

    void Start()
    {
        if (submitButton != null)
        {
            submitButton.onClick.AddListener(OnSubmit);
        }
        else
        {
            Debug.LogError("SubmitButton is not assigned in the Inspector.");
        }

        if (questionInputField == null || questionText == null || answerText == null || textToSpeech == null)
        {
            Debug.LogError("One or more UI components are not assigned in the Inspector.");
        }
    }

    public void OnSubmit()
    {
        if (questionInputField != null)
        {
            string question = questionInputField.text;
            if (!string.IsNullOrEmpty(question))
            {
                StartCoroutine(PostQuestion(question));
                questionInputField.text = ""; // 질문 입력 필드 초기화
            }
        }
    }

    private IEnumerator PostQuestion(string question)
    {
        var requestData = "{\"question\": \"" + question + "\"}";

        using (var request = new UnityWebRequest(apiUrl, "POST"))
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
                string jsonResponse = request.downloadHandler.text;
                var response = JsonUtility.FromJson<ResponseData>(jsonResponse);

                Debug.Log("Result: " + response.result);
                Debug.Log("Sources:");
                foreach (var source in response.sources)
                {
                    Debug.Log(source);
                }

                textToSpeech.Speak(response.result); // TTS를 호출합니다.

                // 질문과 답변을 텍스트 필드에 표시합니다.
                if (questionText != null && answerText != null)
                {
                    questionText.text = "Q: " + question;
                    answerText.text = "A: " + response.result;
                }
                else
                {
                    Debug.LogError("QuestionText or AnswerText is not assigned in the Inspector.");
                }
            }
        }
    }

    [System.Serializable]
    public class ResponseData
    {
        public string result;
        public string[] sources;
    }
}
