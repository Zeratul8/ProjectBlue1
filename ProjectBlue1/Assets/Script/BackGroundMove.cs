using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    // �ʱ� ��� ��ġ �� ����
    Vector3 firstPos;
    
    // ����� �ӵ��� ����
    public float speed;
    // ��� �̹���, �ʺ�
    SpriteRenderer spriteRenderer;
    public float spriteWidth;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // �̹����� ���� �ʺ�
        spriteWidth = spriteRenderer.bounds.size.x;
        firstPos = transform.position;
        StartCoroutine(BackGround_Scrolling());
    }
    IEnumerator BackGround_Scrolling()
    {
        while (true)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;

            // ���� x ���� �̹����� �ʺ�ŭ �������� �̵��ߴٸ� �̹����� 2�踸ŭ ���ؼ� �������� �̵�
            if (transform.position.x < -spriteWidth)
            {
                transform.position += new Vector3(2f * spriteWidth, 0, 0);
            }

            yield return null;

        }
    }
}
