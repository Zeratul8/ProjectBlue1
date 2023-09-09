using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    // �ʱ� ��� ��ġ �� ����
    Vector3 firstPos;

    // ����� �ӵ��� ����
    [SerializeField]
    float speed;
    // ��� �̹���, �ʺ�
    SpriteRenderer[] spriteRenderer;
    float spriteWidth;
    private void Start()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        // �̹����� ���� �ʺ�
        spriteWidth = spriteRenderer[0].bounds.size.x;
        Debug.Log(spriteWidth);
        firstPos = transform.position;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            BackGroundScrolling();
        }
    }

    public void BackGroundScrolling()
    {
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
                transform.position += new Vector3(spriteWidth, 0, 0);
                break;
            }

            yield return null;
        }
    }
}
