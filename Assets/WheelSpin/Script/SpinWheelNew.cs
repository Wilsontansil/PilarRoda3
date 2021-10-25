using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SpinWheelNew : MonoBehaviour
{
    public List<string> prize;
    public List<TextMeshProUGUI> txtReward;
    public TextMeshProUGUI txtJackpot100x;
    public TextMeshProUGUI txtJackpot20x;

    public List<AnimationCurve> animationCurves;
    [Range(0, 4)]
    [SerializeField] private int animationSelectNo;

    [SerializeField] private Transform rotateTransfrom;
    [Range(0, 10)]
    [SerializeField] private int randomTime;

    [Range(0, 10)]
    [SerializeField] private float speed;

    /// <summary>
    /// Angle to put stick recieve reward
    /// </summary>
    [SerializeField] private float angleSelected;

    private bool spinning;
    private float anglePerItem;
    private int itemNumber;
    [SerializeField] private bool isClockwise;

    //Action
    public System.Action<int> OnStartSpinAction;
    public System.Action<int> OnEndSpinAction;

    [Header("GameManager")]
    GameManager gameManager;
    BtnScriptTurbo turbo;
    //public Image blurSpin;

    private GameClient client;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        turbo = FindObjectOfType<BtnScriptTurbo>();

    }
    public void SetAngleEnd(int choice)
    {
        if (choice == 0) angleSelected = 90;
        if (choice == 1) angleSelected = 270;
        if (choice == 2) angleSelected = 180;
        if (choice == 3) angleSelected = 0;
    }

    public void SetClockwise(bool isClokwise)
    {
        this.isClockwise = isClokwise;
    }

    void Start()
    {
        spinning = false;
        anglePerItem = 360 / prize.Count;
        randomTime = 5;
        speed = 2.5f;
        isClockwise = true;
        client = GameClient.Instance;
        client.onSpinResult += onSpinResultHandler;
    }

    //void Update()
    //{
    //    if (!spinning)
    //    {
    //        if (Input.GetKeyDown(KeyCode.Keypad1))
    //            Spin(15);
    //        if (Input.GetKeyDown(KeyCode.Keypad2))
    //            Spin(2);
    //        if (Input.GetKeyDown(KeyCode.Keypad3))
    //            Spin(3);
    //        if (Input.GetKeyDown(KeyCode.Keypad4))
    //            Spin(4);
    //        if (Input.GetKeyDown(KeyCode.Keypad5))
    //            Spin(5);
    //        if (Input.GetKeyDown(KeyCode.Keypad6))
    //            Spin(6);
    //        if (Input.GetKeyDown(KeyCode.Keypad7))
    //            Spin(7);
    //        if (Input.GetKeyDown(KeyCode.Keypad0))
    //            Spin(0);
    //        if (Input.GetKeyDown(KeyCode.Keypad8))
    //            Spin(8);
    //        if (Input.GetKeyDown(KeyCode.Keypad9))
    //            Spin(9);

    //    }
    //}

    public void SpinBTN()
    {
        if (!spinning)
        {
            //client.Send("spin", "");
            Spin(Random.Range(0, 20));
            //Spin(5);
        }
    }

    private void onSpinResultHandler(object sender, int message)
    {
        Debug.Log("Spin result handler " + message);
        Spin(message);

    }
    void Spin(int order)
    {

        if (OnStartSpinAction != null)
            OnStartSpinAction.Invoke(order);

        Debug.Log("itemNumber No. : " + itemNumber);

        //float angleAdjust = anglePerItem / (float)2;
        if (turbo.IsTurboMode)
        {
            randomTime = 1;
            speed = 8;
            //LeanTween.alpha(blurSpin.rectTransform, 1, .1f);
        }
        else
        {
            randomTime = 5;
            speed = 2.5f;
        }


        if (!isClockwise)
        {
            itemNumber = order;
            float maxAngle = 360 * randomTime + (itemNumber * anglePerItem) + angleSelected; //  + angleAdjust
            StartCoroutine(SpinTheWheel(3 * randomTime, maxAngle));
        }
        else
        {
            itemNumber = order;
            var _itemNumber = itemNumber;
            if (order == 0) _itemNumber = 0;
            else _itemNumber = prize.Count - order;
            float maxAngle = 360 * randomTime + (_itemNumber * anglePerItem) - angleSelected; // - angleAdjust
            StartCoroutine(SpinTheWheel(3 * randomTime, maxAngle));
        }
    }

    IEnumerator SpinTheWheel(float time, float maxAngle)
    {
        //Debug.Log(time);
        if (!isClockwise)
        {
            spinning = true;

            float timer = 0.0f;
            float startAngle = rotateTransfrom.eulerAngles.z;
            maxAngle = maxAngle - startAngle;

            if (turbo.IsTurboMode)
            {
                animationSelectNo = 2;
            }
            else
            {
                animationSelectNo = Random.Range(0, animationCurves.Count-1);
            }

            int animationCurveNumber = animationSelectNo;
            Debug.Log("Animation Curve No. : " + animationCurveNumber);

            while (timer < time)
            {
                //to calculate rotation
                float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
                rotateTransfrom.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
                timer += speed * Time.deltaTime;
                yield return 0;
            }

            rotateTransfrom.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
            spinning = false;


            if (OnEndSpinAction != null)
                OnEndSpinAction.Invoke(itemNumber);
            Debug.Log("Prize: " + prize[itemNumber]);//use prize[itemNumnber] as per requirement
            gameManager.CheckWinLose(itemNumber);
        }
        else
        {
            spinning = true;

            float timer = 0.0f;
            float startAngle = -rotateTransfrom.eulerAngles.z;
            maxAngle = startAngle - maxAngle;

            if (turbo.IsTurboMode)
            {
                animationSelectNo = 2;
            }
            else
            {
                animationSelectNo = Random.Range(0, animationCurves.Count - 1);

            }
            int animationCurveNumber = animationSelectNo;

            while (timer < time)
            {

                //to calculate rotation
                float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
                rotateTransfrom.eulerAngles = new Vector3(0.0f, 0.0f, angle - startAngle);
                timer += speed * Time.deltaTime;
                yield return 0;
            }

            rotateTransfrom.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle - startAngle);
            spinning = false;


            if (OnEndSpinAction != null)
                OnEndSpinAction.Invoke(itemNumber);
            Debug.Log("Prize: " + prize[itemNumber]);//use prize[itemNumnber] as per requirement
            gameManager.CheckWinLose(itemNumber);

        }

    }

}


