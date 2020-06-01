using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUIController : MonoBehaviour
{
    public Slider HPGauge;
    public Text HPText;
    CharacterStatus CharStat;

    // Start is called before the first frame update
    void Start()
    {
        CharStat = this.transform.root.GetComponent<CharacterStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        HPbarSet();
    }

    void HPbarSet()
    {
        //케릭터 HP 받아옴
        float HP = CharStat.HP;
        HPGauge.maxValue = CharStat.MaxHP;
        HPGauge.value = Mathf.Lerp(HPGauge.value,HP,Time.deltaTime *10);
        HPText.text = "HP : "+CharStat.HP+ " / " +CharStat.MaxHP;
    }
}
