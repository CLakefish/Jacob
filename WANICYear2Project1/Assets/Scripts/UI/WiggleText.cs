using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleText : MonoBehaviour
{
    [Header("References")]
    private TMPro.TMP_Text text;

    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float size = 0.01f;
    [SerializeField] private float magnitude = 10;

    [Header("Run Animation")]
    [SerializeField] private bool runAnim = false;
    [SerializeField] private float delayTime;
    [SerializeField] private float downPos;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();

        if (runAnim) StartCoroutine(RunIn());
    }

    // Update is called once per frame
    void Update()
    {
        if (text == null) return;

        text.ForceMeshUpdate();

        for (int i = 0; i < text.text.Length; i++)
        {
            var charInfo = text.textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = text.textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * speed + orig.x * size) * magnitude, 0);
            }
        }

        for (int i = 0; i < text.textInfo.meshInfo.Length; i++)
        {
            var meshInfo = text.textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            text.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    private IEnumerator RunIn()
    {
        Vector3 velocity = new();
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position;
        endPos.y -= downPos;

        yield return new WaitForSeconds(delayTime);

        while (transform.position != endPos)
        {
            transform.position = Vector3.SmoothDamp(transform.position, endPos, ref velocity, 0.5f);
            yield return null;
        }
    }
}
