using System.Collections;
using UnityEngine;

public class Monster : Character
{
    int target_Value = 0;
    [SerializeField] private float m_Speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start(); 
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, Character_Spawner.move_list[target_Value], Time.deltaTime*m_Speed);
        if(Vector2.Distance(transform.position, Character_Spawner.move_list[target_Value]) <= 0.1f)
        {
            target_Value++;
            target_Value /= 4;
        }
    }
}
