using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public GameObject goldBarRight;
    public GameObject goldBarLeft;
    public GameObject goldCount;
    public GameObject countdown;

    private TMPro.TextMeshProUGUI text;
    private TMPro.TextMeshProUGUI countdownText;
    private double timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = goldCount.GetComponent<TMPro.TextMeshProUGUI>();

        countdownText = countdown.GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float scale = (float)GameManager.gold / GameManager.startingGold;

        text.text = GameManager.gold.ToString();

        Vector3 end = new Vector3(scale, .5f, 1);

        goldBarLeft.transform.localScale = Vector3.Lerp(goldBarLeft.transform.localScale, end, .1f);
        goldBarRight.transform.localScale = Vector3.Lerp(goldBarRight.transform.localScale, end, .1f);

        if (GameManager.gold <= 0)
        {
            gameObject.SetActive(false);
        }

        timer += Time.deltaTime;

        countdownText.text = Mathf.Max(0,(60f - Mathf.Round((float)GameManager.timer))).ToString();
    }
}
