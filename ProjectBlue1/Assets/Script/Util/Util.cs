using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    //확률설정
    public static int GetPriority(int[] table)
    {
        if (table == null || table.Length == 0) return -1;
        int sum = 0;
        int num = 0;

        //sum 으로 전체 확률 지정하기, num으로 랜덤한 수 뽑기
        for (int i = 0; i < table.Length; i++)
            sum += table[i];
        num = Random.Range(1, sum + 1);
        sum = 0;

        for (int i = 0; i < table.Length; i++)
        {
            if (num > sum && num <= sum + table[i])
            {
                return i;
            }
            sum += table[i];
        }
        return -1;
    }
    public static int GetPriority(float[] table)
    {
        if (table == null || table.Length == 0) return -1;
        float sum = 0;
        float num = 0;

        //sum 으로 전체 확률 지정하기, num으로 랜덤한 수 뽑기
        for (int i = 0; i < table.Length; i++)
            sum += table[i];
        num = Random.Range(0.0f, sum);
        sum = 0;

        for (int i = 0; i < table.Length; i++)
        {
            if (num > sum && num <= sum + table[i])
            {
                return i;
            }
            sum += table[i];
        }
        return -1;
    }
}
