using UnityEngine;
using System.Collections;
using System;

public class ScoreBoard : MonoBehaviour {

    public Sprite[] numbers;
    public SpriteRenderer units;
    public SpriteRenderer tens;
    public SpriteRenderer hundreds;
//    SpriteRenderer tens

	// Use this for initialization
	void Start () {

      //  Sprite units = (Sprite)transform.FindChild("Units").GetComponent<Sprite>();
       // Sprite tens = transform.FindChild("Tens").GetComponent<SpriteRenderer>();
       // SpriteRenderer hundreds = transform.FindChild("Hundreds").GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () {

        int number = GameManager.Instance.TeamBodyCount;

        int unitsDigit=0;

        int tensDigit=0;

        int hundredsDigit = 0;

        string s = number.ToString();
        Debug.Log(s.Length);
        if (s.Length == 2)
        {
            s="0" + s;
        }
        else if (s.Length == 1)
        {
            s = "00" + s;
        }

        Debug.Log(s);

        hundredsDigit = Convert.ToInt16(s.Substring(0,1));
        tensDigit = Convert.ToInt16(s.Substring(1,1));
        unitsDigit = Convert.ToInt16(s.Substring(2,1));
       

        units.sprite = numbers[unitsDigit];
        tens.sprite = numbers[tensDigit];
        hundreds.sprite = numbers[hundredsDigit];


        


	}




}
