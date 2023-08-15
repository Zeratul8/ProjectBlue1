using UnityEngine;
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;

public static class ExcelParsing
{
    public static void ParseExcelStatData(string excelFile, List<Status> states)
    {
        var result = ParseExcelData(excelFile).AsDataSet();

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
    }

    static IExcelDataReader ParseExcelData(string excelFile)
    {
        string filePath = Application.streamingAssetsPath + "/Excel/" + excelFile + ".xlsx";

        using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                return reader;
            }
        }
    }
}

public class Status
{
    public float Attack;
    public float Health;
    public float CriticalHit;
    public float CriticalDamage;
}
