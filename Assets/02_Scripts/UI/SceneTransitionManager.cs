using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("설정")]
    public Image targetImage;       // 쉐이더가 적용된 그 이미지
    public string propertyName = "_Lerp"; // 쉐이더 그래프에서 만든 변수 이름 (중요!)
    public float transitionDuration = 1.0f; // 줄어드는 데 걸리는 시간

    [Header("크기 조절 (테스트하며 맞추세요)")]
    public float maxScale = 80f;   // 구멍이 화면을 다 덮을 때의 크기 (충분히 크게!)
    public float minScale = 0f;     // 구멍이 아예 없을 때의 크기

    private Material materialInstance;

    

    private void Awake()
    {
        // 싱글톤 패턴 (씬이 바뀌어도 파괴되지 않게 설정)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 핵심: 씬 넘어가도 살아있음

            if (targetImage != null)
            {
                // 머티리얼 인스턴스 가져오기
                materialInstance = targetImage.material;
                
                targetImage.gameObject.SetActive(false);
            }
        }
        else
        {
            Destroy(gameObject);
        }        
    }

    // 외부에서 부르는 함수: IrisSceneLoader.Instance.LoadScene("이동할씬이름");
    public void LoadScene(string sceneName)
    {        
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {

        if(targetImage != null)
        {
            targetImage.gameObject.SetActive(true);

            materialInstance.SetFloat(propertyName, maxScale);
        }
        // 1. 구멍 작아지기 (Max -> Min) : 화면이 검게 변함
        yield return StartCoroutine(AnimateIris(maxScale, minScale));

        // 2. 씬 로드 (비동기)
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false; // 로딩 완료 대기

        // 로딩이 90% 될 때까지 대기
        while (op.progress < 0.9f)
        {
            yield return null;
        }

        // 로딩 완료 후 씬 전환 허용
        op.allowSceneActivation = true;
        while (!op.isDone) yield return null;

        // 3. 구멍 커지기 (Min -> Max) : 새 화면이 보임
        yield return StartCoroutine(AnimateIris(minScale, maxScale));

        targetImage.gameObject.SetActive(false);
    }

    // 쉐이더 값을 부드럽게 변경하는 코루틴
    private IEnumerator AnimateIris(float start, float end)
    {
        float timer = 0f;
        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = timer / transitionDuration;

            // 부드러운 움직임 (SmoothStep)
            t = t * t * (3f - 2f * t);

            float currentValue = Mathf.Lerp(start, end, t);

            // 쉐이더에 값 적용
            if (materialInstance != null)
            {
                materialInstance.SetFloat(propertyName, currentValue);
            }

            yield return null;
        }
        // 끝값 확실하게 고정
        if (materialInstance != null) materialInstance.SetFloat(propertyName, end);
    }
}