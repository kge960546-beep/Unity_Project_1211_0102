using UnityEngine;

public enum BarricadePattern
{
    ManulPositions,
    Circle,
    Square,
    HorizontalLIne,
    VerticalLIne
}


[CreateAssetMenu(fileName = "BarricadePattern", menuName = "Game/Stage/Barricade/Pattern SO")]
public class BarricadePatternSO : ScriptableObject
{
    public GameObject barricadePrefab;

    public BarricadePattern patternType;

    // 공통 설정
    public float spacing = 1f;

    // 원형 패턴
    public float circleRadius = 10f;
    public int circleCount = 30;

    // 사각 패턴
    public float squaureWidth = 12f;
    public float squaereHeight = 8f;

    // 라인 패턴
    public int lineCount = 10;
    public float lineLength = 15f;

    // 수동 포지션
    public Vector3[] localPositions;
}
