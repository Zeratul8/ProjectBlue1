using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    // 초기 배경 위치 값 저장
    Vector3 firstPos;
    
    // 배경의 속도를 조절
    public float speed;
    // 배경 이미지, 너비
    SpriteRenderer spriteRenderer;
    public float spriteWidth;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 이미지의 실제 너비
        spriteWidth = spriteRenderer.bounds.size.x;
        firstPos = transform.position;
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
                transform.position += new Vector3(2f * spriteWidth, 0, 0);
            }

            yield return null;

        }
    }
}
