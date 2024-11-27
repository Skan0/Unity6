using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

//여러가지 함수를 이벤트로 받아올 수 있다.
public delegate void OnMoneyUpEventHandler();
public class GameManager : MonoBehaviour
{
    //정적변수로 만들었기 때문에 어디에서든 접근할 수 있다.
    public static GameManager instance = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    
    public int Money = 50;
    public int SummonCount = 20;

    //이렇게 이벤트로 받아오면 이벤트 안에 있는 함수들이 모두 사용된다.
    public event OnMoneyUpEventHandler OnMoneyUp;

    public List<Monster> monsters = new List<Monster>();
    public void GetMoney(int value)
    {
        Money += value;
        OnMoneyUp?.Invoke();
    }
    public void AddMonsters(Monster monster)
    {
        monsters.Add(monster);
    }
    public void RemoveMonster(Monster monster) 
    { 
        monsters.Remove(monster);
    }
}
