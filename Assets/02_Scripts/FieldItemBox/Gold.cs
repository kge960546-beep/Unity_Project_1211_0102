using UnityEngine;
public enum GoldType
{
    gold_1,
    gold_5,
    gold_10
}
public class Gold : MonoBehaviour
{
    [SerializeField] private GoldType goldType;
    private string playerTag = "Player";   
    
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(playerTag)) return;

        if(GoldService.instance != null)
        {
            GoldService.instance.GetGold(goldType);
        }
        
        Destroy(gameObject);
    }   
}
