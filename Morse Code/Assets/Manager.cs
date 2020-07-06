using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    public GameObject infoPanel,gameOverPanel;
    public InputScript inputScript;
    public bool final=false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (infoPanel.active == true)
            {
                infoPanel.SetActive(false);
                inputScript.captureActive = true;
            }

            if (gameOverPanel.active == true)
            {
                if (inputScript.randomizer.Count == 0)
                {
                    if (final == false)
                    {
                        inputScript.gotext2.text = "That's it folks=)";
                        final = true;
                    }
                }
                if (final == true && inputScript.visible == true)
                {
                    Application.Quit();
                }
                if (inputScript.visible == true && final == false)
                {
                    gameOverPanel.SetActive(false);
                    inputScript.captureActive = true;
                    inputScript.firstTry = false;
                    inputScript.StartRestart();
                    inputScript.visible = false;
                }

            }
        }



    }
}
