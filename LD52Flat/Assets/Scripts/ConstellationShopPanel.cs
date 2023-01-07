using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConstellationShopPanel : MonoBehaviour
{
    public Constellation Constellation;
    public TMP_Text TitleText;
    public TMP_Text CostText;
    public TMP_Text SizeText;
    public TMP_Text CountText;
    public GameObject Sold;
    public GameObject ByeButton;
    public Action<ConstellationShopPanel> OnShopPanelClick = delegate { };
    // Start is called before the first frame update
    public void Init(Constellation constellation)
    {
        TitleText.text = constellation.Name;
        CostText.text = constellation.Cost.ToString();
        CountText.text = constellation.starPattern.Length + " stars";
        if (constellation.Size == 0) SizeText.text = "S";
        else if (constellation.Size == 1) SizeText.text = "M";
        else if (constellation.Size == 2) SizeText.text = "L";
        Sold.SetActive(false);
        Constellation = constellation;
        if (constellation.State > 0) Solded();
    }
    public void OnByeAction()
    {
        OnShopPanelClick(this);
    }

    public void Solded()
    {
        Sold.SetActive(true);
        ByeButton.SetActive(false);
    }

}
