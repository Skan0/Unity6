using UnityEngine;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Collections;
public class Character_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawn_Prefab;
    [SerializeField] private Monster _spawn_Monster_Prefab;

    public static List<Vector2> move_list = new List<Vector2>();
    List<Vector2> spawn_list = new List<Vector2>();
    List<bool> spawn_list_arry = new List<bool>(); // ���� ĭ�� ĳ���͸� ������ų �� �ִ°� ���°� �Ǵ��� ����
    
    
    void Start()
    {
        Grid_Start();
        for (int i = 0; i < transform.childCount; i++) 
        { 
            move_list.Add(transform.GetChild(i).position);
        }
        StartCoroutine(Spawn_Monster_Coroutine());
    }
    #region Make_Grid
    private void Grid_Start()
    {
        SpriteRenderer parentSprite = GetComponent<SpriteRenderer>();

        //������ �Ǵ� ��������Ʈ�� ũ��
        float parentwidth = parentSprite.bounds.size.x;
        float parentheight = parentSprite.bounds.size.y;

        float xCount = transform.localScale.x / 6;
        float yCount = transform.localScale.y / 3;
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 6; col++)
            {
                //var go = new GameObject(string.Format("Grid{0}:{1}", row, col));
                //go.transform.localScale = new Vector3(xCount, yCount, 1.0f);

                //go.AddComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                //go.GetComponent<SpriteRenderer>().color= Color.black;

                float xPos = (-parentwidth / 2) + (col * xCount) + (xCount / 2);
                float yPos = (parentheight / 2) - (row * yCount) + (yCount / 2);
                spawn_list.Add(new Vector2(xPos, yPos + transform.localPosition.y - yCount));
                spawn_list_arry.Add(false);
                //go.transform.localPosition = 
                //    new Vector3(
                //        xPos, 
                //        yPos+transform.localPosition.y -go.transform.localScale.y);
            }
        }
    }
    #endregion

    #region ĳ���� ��ȯ
    public void Summon()
    {
        int position_value = -1;
        var go = Instantiate(_spawn_Prefab);
        for (int i = 0; i < spawn_list_arry.Count; i++)
        {
            if (spawn_list_arry[i] == false)
            {
                position_value = i;
                spawn_list_arry[i] = true;
                break;
            }
        }
        go.transform.position = spawn_list[position_value];
    }
    #endregion

    #region ���� ��ȯ
    IEnumerator Spawn_Monster_Coroutine()
    {
        var go = Instantiate(_spawn_Monster_Prefab, move_list[0], Quaternion.identity);
        yield return new WaitForSeconds(0.75f);

        StartCoroutine(Spawn_Monster_Coroutine());
    }
    #endregion
}
