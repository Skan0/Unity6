using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MonsterCount_T;
    [SerializeField] private TextMeshProUGUI Money_T;
    [SerializeField] private TextMeshProUGUI Summon_T;

    [SerializeField] private Animator MoneyAnimation;

    private void Start()
    {
        GameManager.instance.OnMoneyUp += Money_Anim;
    }
    private void Update()
    {
        MonsterCount_T.text = GameManager.instance.monsters.Count.ToString()+"/ 100";
        Money_T.text = GameManager.instance.Money.ToString();
        Summon_T.text = GameManager.instance.SummonCount.ToString();
        Summon_T.color = GameManager.instance.Money >= GameManager.instance.SummonCount ? Color.white : Color.red;
    }
    void Money_Anim()
    {
        MoneyAnimation.SetTrigger("Get");

    }
}
