using UnityEngine;
using System.Data;
using System;
using System.IO;
using ExcelDataReader;
using Object = UnityEngine.Object;
using System.Collections.Generic;

public class ExcelParsing : MonoBehaviour
{
    [Header("Excel File Input")]
    public Object excelFile; // 엑셀 파일 퍼블릭으로 받기
    public Status _states;

    private void Start()
    {
        // 파일이 존재할 때만 파싱 시작
        if (excelFile != null)
        {
            string filePath = Application.dataPath + "/Excel/" + excelFile.name + ".xlsx";

            using(var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    // 시트 개수만큼 반복
                    for(int i = 0; i <result.Tables.Count; i++)
                    {
                        // 해당 시트의 행데이터(한줄씩)로 반복 ( 0 번째는 열 이름이므로 제외 )
                        for (int j = 1; j < result.Tables[i].Rows.Count; j++)
                        {
                            /* 해당 행의 열 개수만큼 반복 ( 보류 )
                            for (int k = 0; k < result.Tables[i].Rows[j].ItemArray.Length; k++)
                            {
                                _states.Add((float)result.Tables[i].Rows[j][k]);
                            }*/


                            // Status 변수에 데이터 삽입 ( 아직 해결 안됨 )
                            _states.Attack = (float)result.Tables[i].Rows[j][0];
                            print(_states.Attack);
                            _states.Health = (float)result.Tables[i].Rows[j][1];
                            _states.CriticalHit = (float)result.Tables[i].Rows[j][2];
                            _states.CriticalDamage = (float)result.Tables[i].Rows[j][3];
                        }
                    }
                }
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
