using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    public Button uiButton; // UI 버튼을 연결합니다.

    void Start()
    {
        animator = GetComponent<Animator>();

        // UI 버튼의 클릭 이벤트에 OnUIButtonClicked 함수를 연결합니다.
        if (uiButton != null)
        {
            uiButton.onClick.AddListener(OnUIButtonClicked);
        }
        else
        {
            Debug.LogError("UIButton is not assigned in the Inspector.");
        }
    }

    // UI 버튼이 클릭되었을 때 호출되는 함수
    void OnUIButtonClicked()
    {
        StartCoroutine(StartTalkingAnimation());
    }

    IEnumerator StartTalkingAnimation()
    {
        // 2초간 대기합니다.
        yield return new WaitForSeconds(2f);

        // talk 애니메이션을 재생합니다.
        animator.SetBool("talk", true);

        // 5초 후에 StopTalkingAnimation 코루틴을 시작합니다.
        yield return new WaitForSeconds(7f);

        // talk 애니메이션을 멈춥니다.
        animator.SetBool("talk", false);
    }
}
