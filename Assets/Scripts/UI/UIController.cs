using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text FPSText;

    void Update()
    {
        FPSText.text = (int)((1 / Time.smoothDeltaTime) * Time.timeScale) + " FPS";
    }
}
