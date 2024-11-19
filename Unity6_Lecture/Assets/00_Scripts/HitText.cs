using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HitText : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1;
    [SerializeField] private float riseDuration = 0.5f;
    [SerializeField] private float fadeDuration = 0.5f;
    public Vector3 offset = new Vector3(0,0.2f,0);

    public TextMeshPro damageText;
    private Color textColor;

    public void Initialize(int damage)
    {
        damageText.text = damage.ToString();
        textColor = damageText.color;
        StartCoroutine(MoveAndFade());
    }
    IEnumerator MoveAndFade()
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + offset;

        float elapsedTime = 0;
        while (elapsedTime < riseDuration) { 
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime/riseDuration);//위치에 대한 선형보간법
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            textColor.a = Mathf.Lerp(1,0,elapsedTime/fadeDuration);//float에 대한 선형 보간법
            damageText.color = textColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
