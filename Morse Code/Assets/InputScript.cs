using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class InputScript : MonoBehaviour
{
    public float spaceMeter, spaceDelay, letterDelay, noSpaceMeter;
    public Text inputText, upperTyre, lowerTyre, gotext1, gotext2;

    public Queue<string> desiredTextQueue = new Queue<string>(), answerTextQueue = new Queue<string>(), chars = new Queue<string>();
    public string desiredMorseCode, currMorseChar, greenString, blackString;
    public TextMeshProUGUI desiredText, answerText;
    public int lastInd, letterNo;
    public bool deleted, visible = false,firstEncounter;
    public List<int> randomizer = new List<int>{0};
    public Image morse,gameOverPanel;
    public Sprite morseon, morseoff;
    public string[] desiredPool, answerPool;
    public AudioSource audioSrc;

    public string[][] poolOfDesired = new string[][]
    {
    new string[] { "Anybody here? ",
            "What? You shut up! ",
            "You are pissing me off, don't write! ",
            "Fuck you",
            "No, FUCK YOU111! ",
            "Stupid",
            "And you are faggot. ",
            "Get lost, wanker! ",
            "Your mother was happy to suck it. ",
            "I'm sorry, bro. ",
            "Sure, will we meet again? ",
 },
   
    new string[] { "OK, what game? ",
            "Who is there? ",
            "It's you again! Long time, no see. ",
 },


    new string[] {"ABCDEFG",
        "HIJKLMNOP",
        "Fuck" }

    };

    public string[][] poolOfAnswers = new string[][]
    {
        new string[] {"      ",
            "Shut up! ",
            "No-no-no... you don't get it. SHUT UP and stop writing to me. ",
            "Ha-ha, I will tell you the truth, your dad left you because of your morse code addiction. ",
            "No FUCK YOU! ",
            "Idiot ",
            "Your GF is imaginary. Oops! ",
            "And i like you, sweetie ",
            "Your dick is so small, that your dickpick resolution is 13x2 pixels. ",
            "Wow, it's rude, I don't have mother man... ",
            "It's ok, bro. It was a pleasure to talk to you, my friend, have a nice day. ",
            "Only if you would like) ",
},

        new string[] { "Let's play a game? ",
            "Knock-knock... ",
            "Fuck you, you stupid cunt! ",
            "Yeah, nice to meet you again. ",

 },
        
         new string[]
        {"Let's learn an alfabet! ",
            "Nice, go on!",
            "HAHA fooled you, faggot)" }


    };

    public bool captureActive,firstTry;


    public AudioClip dotSound, dashSound;


    private string[] alphabet =
//A     B       C       D      E    F       G
{".-", "-...", "-.-.", "-..", ".", "..-.", "--.",
    //H       I     J       K      L       M     N
     "....", "..", ".---", "-.-", ".-..", "--", "-.",
    //O      P       Q       R      S      T    U
     "---", ".--.", "--.-", ".-.", "...", "-", "..-",
    //V       W      X       Y       Z
     "...-", ".--", "-..-", "-.--", "--..",
    //0        1        2        3        4        
     "-----", ".----", "..---", "...--", "....-",
    //5        6        7        8        9    
     ".....", "-....", "--...", "---..", "----."};

    void Start()
    {
        StartRestart();
    }

    // Update is called once per frame

        public IEnumerator AudioGone()
    {
        for (float i = 1; i >= 0; i-=0.01f)
        {
            audioSrc.volume = i;
            
        }
        yield return new WaitForEndOfFrame();
    }

    void Update()
    {

        noSpaceMeter += Time.deltaTime;
        if (captureActive)
        {
            if (firstTry)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //audioSrc.volume = 1;
                    audioSrc.Play();

                }

                    if (Input.GetKey(KeyCode.Space))
                {
                    noSpaceMeter = 0;
                    spaceMeter += Time.deltaTime;
                    deleted = false;
                    morse.sprite = morseon;
                    
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    noSpaceMeter = 0;
                    Comparator();
                    deleted = false;
                    morse.sprite = morseoff;
                    //StartCoroutine(AudioGone());
                    audioSrc.Stop();
                }
                if (noSpaceMeter > 2f && !deleted)
                {
                    inputText.text = inputText.text.Substring(0, lastInd);
                    deleted = true;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space)) firstTry = true;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            NextPhrase();
        }
    }

    public void Comparator()
    {
        if (spaceMeter < 0.2f)
        {
            inputText.text += ".";
        }
        else inputText.text += "-";
        spaceMeter = 0;

       
        Compare();

    }

    public void Compare()
    {

        if (currMorseChar.Length != 0)
        {
            if (inputText.text.Substring(lastInd, Mathf.Clamp(currMorseChar.Length, 0, inputText.text.Length - lastInd)).CompareTo(currMorseChar) == 0)
            {
                lastInd += currMorseChar.Length + 1;
                currMorseChar = chars.Dequeue();
                inputText.text += " ";
                if (blackString.Substring(0, 1) != " " && blackString.Substring(0, 1) != "'")
                {
                    greenString += blackString.Substring(0, 1);
                    if (blackString.Length - 1 > 0)
                    {
                        blackString = blackString.Substring(1, blackString.Length - 1);
                    }
                    else
                    {

                        NextPhrase();
                    }
                }
                else
                {
                    greenString += blackString.Substring(0, 2);
                    blackString = blackString.Substring(2, blackString.Length - 2);
                }

                if (blackString.Substring(0, 1) == "," || blackString.Substring(0, 1) == "." || blackString.Substring(0, 1) == "?" || blackString.Substring(0, 1) == "!")
                {
                    greenString += blackString.Substring(0, 2);

                    if (blackString.Length - 2 > 0)
                    {
                        blackString = blackString.Substring(2, blackString.Length - 2);
                    }
                    else
                    {
                        NextPhrase();
                    }

                }


                desiredText.text = "<color=#008000ff>" + greenString + "</color>" + blackString;


                letterNo++;

            }
        }

    }

    public void DesiredSetup()
    {
        //desiredText.text = morseScript.text.text;

        if (desiredTextQueue.Count>0)
        {
            desiredText.text = desiredTextQueue.Dequeue();
        }
        else desiredText.text = "";
        if (answerTextQueue.Count > 0)
        {
            answerText.text = answerTextQueue.Dequeue();
        }
        else answerText.text = "";

        if(answerTextQueue.Count==0 && desiredTextQueue.Count == 0)
        {
            GameOver();
        }

        inputText.text = null;

        blackString = desiredText.text;
        greenString = null;
        ToMorseCode(desiredText.text);
        if (desiredMorseCode.Length > 0)
        {
            string[] c = desiredMorseCode.Split('/');

            chars.Clear();
            for (int i = 0; i < c.Length; i++)
            {
                chars.Enqueue(c[i]);
            }
            currMorseChar = chars.Dequeue();
        }
        else currMorseChar = null;


        lastInd = 0;
        letterNo = 0;
       
       

    }

    public void PlayMorseCodeMessage(string message)
    {
        StartCoroutine("_PlayMorseCodeMessage", message);
    }

    private IEnumerator _PlayMorseCodeMessage(string message)
    {
        // Remove all characters that are not supported by Morse code...
        Regex regex = new Regex("[^A-z0-9 ]");
        message = regex.Replace(message.ToUpper(), "");
        Debug.Log(message);

        // Convert the message into Morse code audio...
        foreach (char letter in message)
        {
            if (letter == ' ')
                yield return new WaitForSeconds(spaceDelay);
            else
            {
                int index = letter - 'A';
                if (index < 0)
                    index = letter - '0' + 26;
                string letterCode = alphabet[index];
                foreach (char bit in letterCode)
                {
                    // Dot or Dash?
                    AudioClip sound = dotSound;
                    if (bit == '-') sound = dashSound;

                    // Play the audio clip and wait for it to end before playing the next one.
                    GetComponent<AudioSource>().PlayOneShot(sound);
                    yield return new WaitForSeconds(sound.length + letterDelay);
                }
            }
        }
    }

    public void ToMorseCode(string message)
    {
        
        string morseResult = null;

        Regex regex = new Regex("[^A-z0-9 ]");
        message = regex.Replace(message.ToUpper(), "");

        foreach (char letter in message)
        {
            if (letter == ' ')
                morseResult += "";
            else
            {
                int index = letter - 'A';
                if (index < 0)
                    index = letter - '0' + 26;
                string letterCode = alphabet[index];
                foreach (char bit in letterCode)
                {
                    // Dot or Dash?
                    morseResult += bit;


                }
                morseResult += "/";
            }
        }
        desiredMorseCode = morseResult;
    }

    public IEnumerator TextAnim()
    { Color color;
        
        color = new Color(0, 0, 0, 0);
        answerText.color = color;
        upperTyre.color = color;
        yield return new WaitForSeconds(1f);
        
        for (float i = 0; i < 1; i+=0.01f)
        {
            
            color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.01f);
            answerText.color = color;
            upperTyre.color = color;
            
        }

        


        }
    public IEnumerator DesiredTextAnim()
    {
        Color color1;

        color1 = new Color(0.5f, 0.5f, 0.5f, 0);
        desiredText.color = color1;
        lowerTyre.color = color1;
        yield return new WaitForSeconds(2f);
        for (float i = 0; i < 1; i += 0.01f)
        {

            color1 = new Color(0.5f, 0.5f, 0.5f, i);
            yield return new WaitForSeconds(0.01f);
            desiredText.color = color1;
            lowerTyre.color = color1;

        }
    }


    public void NextPhrase()
    {
        Color color = new Color(0, 0, 0, 1);
        Color color1 = new Color(0.5f, 0.5f, 0.5f, 1);

        blackString = null;
        StopAllCoroutines();
        answerText.color = color;
        upperTyre.color = color;
        desiredText.color = color1;
        lowerTyre.color = color1;
        StartCoroutine(TextAnim());
        StartCoroutine(DesiredTextAnim());
        DesiredSetup();
    }


    public void GameOver()
    {
        captureActive = false;
        firstTry = false;
        StartCoroutine(GOAnim());
    }

    public void StartRestart()
    {
        if (randomizer.Count > 0)
        {
            int rand;

            rand = Random.Range(0, randomizer.Count-1);




            desiredPool = poolOfDesired[randomizer[rand]];
            answerPool = poolOfAnswers[randomizer[rand]];

            if (randomizer[rand] == 0 && !randomizer.Contains(2))
            {
                randomizer.Add(1);
                randomizer.Add(2);
            }

            randomizer.RemoveAt(rand);

            for (int i = 0; i < desiredPool.Length; i++)
            {
                desiredTextQueue.Enqueue(desiredPool[i]);
            }

            for (int i = 0; i < answerPool.Length; i++)
            {
                answerTextQueue.Enqueue(answerPool[i]);
            }



            DesiredSetup();


            ToMorseCode(desiredText.text);
        }
        else gotext2.text = "That's it folks=)";


    }

    public IEnumerator GOAnim()
    {
        Color color,color2;

        color = new Color(0, 0, 0, 0);
        color2 = new Color(0, 0, 0, 0);
        
        gameOverPanel.color = color;
        gotext1.color = color2;
        gotext2.color = color2;
        yield return new WaitForSeconds(4f);
        gameOverPanel.gameObject.SetActive(true);

        for (float i = 0; i < 1; i += 0.01f)
        {

            color = new Color(0, 0, 0, i);
            color2 = new Color(1, 1, 1, i);
            yield return new WaitForSeconds(0.01f);
            gameOverPanel.color = color;
            gotext1.color = color2;
            gotext2.color = color2;

        }
        visible = true;




    }

}
