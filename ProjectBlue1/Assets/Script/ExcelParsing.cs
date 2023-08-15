using UnityEngine;
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;

public static class ExcelParsing
{
    public static void ParseExcelStatData(string excelFile, List<Status> states)
    {
        var result = ParseExcelData(excelFile).AsDataSet();

        // 시트 개수만큼 반복
        for (int i = 0; i < result.Tables.Count; i++)
        {
            // 해당 시트의 행데이터(한줄씩)로 반복 ( 0 번째는 열 이름이므로 제외 )
            for (int j = 1; j < result.Tables[i].Rows.Count; j++)
            {
                /* 해당 행의 열 개수만큼 반복 ( 보류 )
                for (int k = 0; k < result.Tables[i].Rows[j].ItemArray.Length; k++)
                {
                    _states.Add((float)result.Tables[i].Rows[j][k]);
                }*/


                // Status 변수에 데이터 삽입

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
