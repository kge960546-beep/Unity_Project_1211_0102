using UnityEngine;

public class DifficultyEvaluator : MonoBehaviour
{
    public float CurrentDifficulty {  get; private set; }
    TimeService ts;

    public float Evaluate()
    {
        float difficulty = 0f;

        difficulty += ts.accumulatedFixedDeltaTime * 0.1f;

        //추후 난이도 조절을 위해 값을 추가로 조절하고 싶으면 추가하면 됨

        CurrentDifficulty = Mathf.Clamp(difficulty, 0f, 100f);
        return CurrentDifficulty;
    }
}
