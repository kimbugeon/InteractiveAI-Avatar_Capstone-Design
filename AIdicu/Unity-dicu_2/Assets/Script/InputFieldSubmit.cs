using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFieldSubmit : MonoBehaviour
{
    public InputField inputField;
    public Button submitButton;

    void Start()
    {
        // 입력 필드의 엔터 키 입력 이벤트를 감지하여 버튼을 클릭합니다.
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    void OnEndEdit(string text)
    {
        // 입력 필드에서 엔터 키를 눌렀을 때만 버튼을 클릭합니다.
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (submitButton != null)
            {
                // 버튼을 클릭합니다.
                submitButton.onClick.Invoke();
            }
        }
    }
}
