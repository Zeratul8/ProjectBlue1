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
        speed = spriteWidth / (Constants.monsterWalkDistance / Constants.monsterWalkSpeed);
        firstPos = transform.position;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartBackGroundScrolling();
        }
    }

    public void StartBackGroundScrolling()
    {
        StartCoroutine(BackGround_Scrolling());
    }
    public void StopBackGroundScrolling()
    {
        StopAllCoroutines();
    }

    IEnumerator BackGround_Scrolling()
    {

        while (true)
        {
            //transform.position += Vector3.left * speed * Time.deltaTime;
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;

            // ���� x ���� �̹����� �ʺ�ŭ �������� �̵��ߴٸ� �̹����� 2�踸ŭ ���ؼ� �������� �̵�
            if (transform.position.x < -spriteWidth)
            {
                transform.position += new Vector3(spriteWidth * 2, 0, 0);
                //break;
            }

            yield return null;
        }
    }
}
