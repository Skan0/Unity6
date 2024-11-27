using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

//�������� �Լ��� �̺�Ʈ�� �޾ƿ� �� �ִ�.
public delegate void OnMoneyUpEventHandler();
public class GameManager : MonoBehaviour
{
    //���������� ������� ������ ��𿡼��� ������ �� �ִ�.
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

    //�̷��� �̺�Ʈ�� �޾ƿ��� �̺�Ʈ �ȿ� �ִ� �Լ����� ��� ���ȴ�.
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
