using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : Character
{
    public int Hp = 0;
    int target_Value = 0;
    bool isDead = false;
    [SerializeField] private float m_Speed;
    
    //��ӹ޴� Ŭ������ start�� virtual�� ����Ǿ� �־ override�� �޾ƿ� �� �ִ�.
    public override void Start()
    {
        base.Start(); 
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, Character_Spawner.move_list[target_Value], Time.deltaTime*m_Speed);
        if(Vector2.Distance(transform.position, Character_Spawner.move_list[target_Value]) <= 0.1f)
        {
            target_Value++;
            renderer.flipX = target_Value>=3?true:false;
            target_Value %= 4;
        }
    }
    public void GetDamage(int dmg)
    {
        Hp -= dmg;
        if(Hp < 0)
        {
            AnimatorChange("Dead", true);
            isDead = true;
            Destroy(gameObject);
        }
    }
}
