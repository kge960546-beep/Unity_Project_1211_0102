using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadSubject : MonoBehaviour
{
    private readonly List<IEnemyDead> observers = new List<IEnemyDead>(); //인터페이스 리스트 등록
    public void RegisterObserver(IEnemyDead observer) //구독 신청
    {
        if(!observers.Contains(observer))
            observers.Add(observer);
    }
    public void UnregisterObserver(IEnemyDead observer) //구독 해지
    {
        if(observers.Contains(observer))
            observers.Remove(observer);
    }
    public void NotifyEnemyDead() //송출
    {
        foreach (IEnemyDead observer in observers)
        {
            observer.OnEnemyDead();
        }
    }
}
