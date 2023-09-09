using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    // 초기 배경 위치 값 저장
    Vector3 firstPos;

    // 배경의 속도를 조절
    [SerializeField]
    float speed;
    // 배경 이미지, 너비
    SpriteRenderer[] spriteRenderer;
    float spriteWidth;
    private void Start()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        // 이미지의 실제 너비
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

            // 현재 x 값이 이미지의 너비만큼 왼쪽으로 이동했다면 이미지의 2배만큼 더해서 우측으로 이동
            if (transform.position.x < -spriteWidth)
            {
                transform.position += new Vector3(spriteWidth, 0, 0);
                break;
            }

            yield return null;
        }
    }
}
