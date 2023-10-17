using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fade : MonoBehaviour
{
    public TMP_Text m_Text;
    public float DisipearSpeed;
    // Start is called before the first frame update
    void Start()
    {
        m_Text = GetComponentInChildren<TMP_Text>();
        gameObject.GetComponent<Canvas>().worldCamera = GameObject.FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Text.alpha -= Time.deltaTime * DisipearSpeed;
        if (m_Text.alpha <= 0)
        {
            Destroy(gameObject);
        }
    }
}
