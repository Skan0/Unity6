using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : Character
{
    public int Hp = 0;
    int target_Value = 0;
    bool isDead = false;
    [SerializeField] private float m_Speed;
    
    //상속받는 클래스의 start가 virtual로 선언되어 있어서 override로 받아올 수 있다.
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
        if (isDead) return;
        Hp -= dmg;
        if(Hp <= 0)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            AnimatorChange("Dead", true);
            StartCoroutine(Dead());
            isDead = true;
        }
    }
    IEnumerator Dead()
    {
        float Alpha =1.0f;
        
        while(renderer.color.a> 0)
        {
            Alpha-=Time.deltaTime;
            renderer.color -= new Color(renderer.color.r, renderer.color.g, renderer.color.b,
                Alpha);
            yield return null;
        }
        Destroy(gameObject);
    }
}
