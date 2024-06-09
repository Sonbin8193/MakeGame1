using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject waterCollider;
    //Khoi tao UI Manager cachs 1
    //public static UIManager Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = FindObjectOfType<UIManager>();
    //        }
    //        return instance;
    //    }
    //}
    //Khoi tao UI Manager cachs 2
    private void Awake()
    {
        instance=this;
        waterCollider.SetActive(false);
    }
    [SerializeField]TMP_Text coinText;
    public void SetCoin(int coin)
    {
        coinText.text = coin.ToString();
    }

    public void WaterPoition()
    {
        StartCoroutine(ActiveWaterPoition());
    }

    IEnumerator ActiveWaterPoition()
    {
        waterCollider.SetActive(true);
        yield return new WaitForSeconds(10f);
        waterCollider.SetActive(false);
    }
}
