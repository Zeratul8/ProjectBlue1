using UnityEngine;
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;
using System;
using System.Data;

public static class ExcelParsing
{
    public static void ParseExcelStatData(string excelFile, List<Status> states)
    {
        using (var result = ParseExcelData(excelFile))
        {
            if (result != null)
            {
                // ��Ʈ ������ŭ �ݺ�
                for (int i = 0; i < result.Tables.Count; i++)
                {
                    // �ش� ��Ʈ�� �൥����(���پ�)�� �ݺ� ( 0 ��°�� �� �̸��̹Ƿ� ���� )
                    for (int j = 1; j < result.Tables[i].Rows.Count; j++)
                    {
                        /* �ش� ���� �� ������ŭ �ݺ� ( ���� )
                        for (int k = 0; k < result.Tables[i].Rows[j].ItemArray.Length; k++)
                        {
                            _states.Add((float)result.Tables[i].Rows[j][k]);
                        }*/


                        // Status ������ ������ ����
                        Status status = new Status();
                        status.Attack = float.Parse(result.Tables[i].Rows[j][0].ToString());
                        status.Health = float.Parse(result.Tables[i].Rows[j][1].ToString());
                        status.CriticalHit = float.Parse(result.Tables[i].Rows[j][2].ToString());
                        status.CriticalDamage = float.Parse(result.Tables[i].Rows[j][3].ToString());

                        states.Add(status);
                    }
                }

                Debug.Log("�ε��Ϸ�!");
            }
            else
            {
                Debug.Log("���� �о�� �����Ͱ� ����!");
                return;
            }
        }
    }

    static DataSet ParseExcelData(string excelFile)
    {
        string filePath = Application.streamingAssetsPath + "/Excel/" + excelFile + ".xlsx";
        if(File.Exists(filePath))
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    Debug.Log("������!");
                    return reader.AsDataSet();
                }
            }
        }
        else
        {
            Debug.Log("!!!!!!!!!!!!!!������ ã���� ����!!!!!!!!");
            return null;
        }
    }
}

[Serializable]
public class Status
{
    public float Attack;
    public float Health;
    public float CriticalHit;
    public float CriticalDamage;
}
