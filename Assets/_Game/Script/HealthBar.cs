using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image imageFill;
    [SerializeField] Vector3 offset;

    float hp, maxHp;

    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        imageFill.fillAmount = Mathf.Lerp(imageFill.fillAmount,hp/maxHp,Time.deltaTime*1f);
        transform.position = target.position + offset;
    }

    public void OnInit(float maxHp,Transform target)
    {
        this.target = target;
        this.maxHp = maxHp;
        hp = maxHp;
        imageFill.fillAmount = 1;
    }

    public void SetNewHp(float hp)
    {
        this.hp = hp;
    }

}
