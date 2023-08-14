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
    public Object excelFile; // ���� ���� �ۺ����� �ޱ�
    public Status _states;

    private void Start()
    {
        // ������ ������ ���� �Ľ� ����
        if (excelFile != null)
        {
            string filePath = Application.dataPath + "/Excel/" + excelFile.name + ".xlsx";

            using(var stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();

                    // ��Ʈ ������ŭ �ݺ�
                    for(int i = 0; i <result.Tables.Count; i++)
                    {
                        // �ش� ��Ʈ�� �൥����(���پ�)�� �ݺ� ( 0 ��°�� �� �̸��̹Ƿ� ���� )
                        for (int j = 1; j < result.Tables[i].Rows.Count; j++)
                        {
                            /* �ش� ���� �� ������ŭ �ݺ� ( ���� )
                            for (int k = 0; k < result.Tables[i].Rows[j].ItemArray.Length; k++)
                            {
                                _states.Add((float)result.Tables[i].Rows[j][k]);
                            }*/


                            // Status ������ ������ ���� ( ���� �ذ� �ȵ� )
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
