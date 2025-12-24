using UnityEngine;

// 일시정지 테스트
public class PauseControllService : MonoBehaviour, IGameManagementService
{
    private int pauseRefCount = 0;

    public void Pause()
    {
        pauseRefCount++;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseRefCount--;
        if (pauseRefCount <= 0)
        {
            pauseRefCount = 0;
            Time.timeScale = 1f;
        }
    }
}
