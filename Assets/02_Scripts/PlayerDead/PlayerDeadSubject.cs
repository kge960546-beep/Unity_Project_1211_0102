using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadSubject : MonoBehaviour
{
    private readonly List<IPlayerDead> observers = new List<IPlayerDead>(); //인터페이스 리스트 등록
    public void RegisterObserver(IPlayerDead observer) //구독 신청
    {
        if(!observers.Contains(observer))
            observers.Add(observer);
    }
    public void UnregisterObserver(IPlayerDead observer) //구독 해지
    {
        if(observers.Contains(observer))
            observers.Remove(observer);
    }
    public void NotifyPlayerDead() //송출
    {
        foreach (IPlayerDead observer in observers)
        {
            observer.OnPlayerDead();
        }
    }
}
