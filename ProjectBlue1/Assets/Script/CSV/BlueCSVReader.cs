using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCSVReader
{
    public static List<Dictionary<string, float>> Read(TextAsset csvTextAsset)
    {
        List<Dictionary<string, float>> data = new List<Dictionary<string, float>>();
        string text = csvTextAsset.text;

        string[] rows = text.Split('\n');

        int rowsCount = rows.Length;

        if (rowsCount <= 1) return data;

        string[] headers = rows[0].Split(',');
        headers[headers.Length - 1] = headers[headers.Length - 1].Trim('\r');

        for (int i =1; i < rowsCount; i++)
        {
            string[] values = rows[i].Split(',');
            
            int valuesCount = values.Length;
            Debug.Log("value °¹¼ö : " + valuesCount);

            Dictionary<string, float> rowsData = new Dictionary<string, float>();

            for (int j =0; j < valuesCount; j++)
            {
                if (valuesCount <= 1) break;
                Debug.Log("value °ª : " + values[j]);

                if (values[j].Split(' ')[0].Length > 0 && values[j].Split('\n')[0].Length > 0)
                {
                    float value = float.Parse(values[j].Split(' ')[0]);
                    rowsData.Add(headers[j], value);
                }
            }

            data.Add(rowsData);

        }

        return data;
    }
}
