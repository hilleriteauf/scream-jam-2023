using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TorchCounter : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCount(int count)
    {
        text.text = count.ToString();
    }
}
