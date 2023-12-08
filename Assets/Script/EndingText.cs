using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class EndingText : MonoBehaviour
{
    [SerializeField]
    TextAsset csv_text;
    List<string[]> _textData = new List<string[]>();

    
    List<GameObject> textObjectList = new List<GameObject>();

    [SerializeField]
    GameObject textPrehab;

    private int size = 0;

    [SerializeField]
    private int offset = -150;

    private Vector2 _des;

    [SerializeField]
    float speed;

    private RectTransform _rectTransform;

    public void Awake()
    {
        foreach (GameObject go in textObjectList)
        {
            Destroy(go);
        }
        textObjectList.Clear();
        _textData.Clear();

        StringReader sr = new StringReader(csv_text.text);
        while (sr.Peek() > -1)
        {
            string line = sr.ReadLine();
            line = line.Replace("\\n", "\n");
            line = line.Replace("\\t", "\t");
            Debug.Log(line);
            string[] line_splits = line.Split(new char[] { ',' });

            _textData.Add(line_splits);
        }

        foreach (string[] textSet in _textData)
        {
            GameObject tgb = Instantiate(textPrehab, this.transform);
            Transform image = tgb.transform.Find("Image");
            image.Find("CategoryText").GetComponent<TextMeshProUGUI>().text = textSet[0];
            TextMeshProUGUI tmpro = image.Find("NameText").GetComponent<TextMeshProUGUI>();
            tmpro.text = "";
            for(int i = 1; i < textSet.Length; i++)
            {
                tmpro.text += textSet[i] + "\n";
            }
            textObjectList.Add(tgb);

        }

        size = (textObjectList.Count - 1) * 400;
        
        _rectTransform = this.GetComponent<RectTransform>();
        _des = new Vector2(_rectTransform.anchoredPosition.x, size + offset);

    }


    private void Update()
    {
        Debug.Log(_rectTransform.anchoredPosition);
        _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, _des, speed*Time.deltaTime);
    }
}
