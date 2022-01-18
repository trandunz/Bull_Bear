using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class API : MonoBehaviour
{
    private const string ETHURL = "https://api.coinbase.com/v2/prices/ETH-USD/spot";
    private const string BTCURL = "https://api.coinbase.com/v2/prices/BTC-USD/spot";
    [SerializeField] private Text m_EthText;
    [SerializeField] private Text m_BTCText;
    private string m_Temp;
    private string m_Temp2;
    private char[] m_BTCArray;
    private char[] m_BTCPriceArray;
    private char[] m_ETHArray;
    private char[] m_ETHPriceArray;

    private int m_BTCValue;
    private int m_ETHValue;

    [SerializeField] private Image m_ETHImage;
    [SerializeField] private Image m_BTCImage;

    [SerializeField] private Sprite m_UpArrow;
    [SerializeField] private Sprite m_DownArrow;
    [SerializeField] private int UpdateRate;
    [SerializeField] private int m_ComparisonRate;

    private float m_UpdateSpeed;
    private float m_ComparisonUpdateSpeed;
    private bool m_UpdateComparisonValue;

    private int m_LastBTCValue;
    private int m_LastETHValue;

    private Vector3 m_ETHStartRotation;
    private Vector3 m_ETHDownRotation;

    private Vector3 m_ETHSideRotation;
    private Vector3 m_BTCSideRotation;

    private Vector3 m_BTCStartRotation;
    private Vector3 m_BTCDownRotation;

    [SerializeField] private Script_Rotator m_EthObject;
    [SerializeField] private Script_Rotator m_BTCObject;

    [SerializeField] private bool m_RefreshComparisonOverTime;

    bool doOnce = true;

    private void Start()
    {
        m_BTCArray = new char[66];
        m_ETHArray = new char[66];
        m_UpdateSpeed = 0.0f;
        m_ComparisonUpdateSpeed = 0.0f;
        Request();

        m_ETHStartRotation = m_ETHImage.transform.rotation.eulerAngles;
        m_ETHDownRotation = new Vector3(m_ETHImage.transform.rotation.eulerAngles.x + 180, m_ETHImage.transform.rotation.eulerAngles.y, m_ETHImage.transform.rotation.eulerAngles.z);
        m_ETHSideRotation = new Vector3(m_ETHImage.transform.rotation.eulerAngles.x , m_ETHImage.transform.rotation.eulerAngles.y, m_ETHImage.transform.rotation.eulerAngles.z - 45);


        m_BTCStartRotation = m_BTCImage.transform.rotation.eulerAngles;
        m_BTCDownRotation = new Vector3(m_BTCImage.transform.rotation.eulerAngles.x + 180, m_BTCImage.transform.rotation.eulerAngles.y, m_BTCImage.transform.rotation.eulerAngles.z);
        m_BTCSideRotation = new Vector3(m_BTCImage.transform.rotation.eulerAngles.x , m_BTCImage.transform.rotation.eulerAngles.y, m_BTCImage.transform.rotation.eulerAngles.z + 45);
        Debug.Log(m_LastETHValue);
        Debug.Log(m_LastBTCValue);
        m_UpdateComparisonValue = false;
    }

    private void Update()
    {
        UpdateETH();
        UpdateBTC();

        if (m_RefreshComparisonOverTime)
        {
            m_ComparisonUpdateSpeed -= Time.deltaTime;
            if (m_ComparisonUpdateSpeed <= 0)
            {
                m_UpdateComparisonValue = true;
                m_ComparisonUpdateSpeed = m_ComparisonRate;
            }
        }

        m_UpdateSpeed -= Time.deltaTime;
        if (m_UpdateSpeed <= 0)
        {
            Request();
            m_UpdateSpeed = UpdateRate;
            Debug.Log("Updated!");
        }

        if (m_LastBTCValue == 0 && m_LastETHValue == 0)
        {
            m_LastBTCValue = m_BTCValue;
            m_LastETHValue = m_ETHValue;
            Debug.Log(m_LastBTCValue + "!");
            Debug.Log(m_LastETHValue + "!");
            
        }

        if (m_UpdateComparisonValue && m_RefreshComparisonOverTime)
        {
            m_LastBTCValue = m_BTCValue;
            m_LastETHValue = m_ETHValue;
            Debug.Log(m_LastBTCValue + "!");
            Debug.Log(m_LastETHValue + "!");
            m_UpdateComparisonValue = false;
        }
    }

    private void UpdateETH()
    {
        if (m_LastETHValue < m_ETHValue)
        {
            m_EthObject.transform.rotation = Quaternion.RotateTowards(m_EthObject.transform.rotation,Quaternion.Euler(m_ETHStartRotation), Time.deltaTime * 100);
            //m_ETHImage.transform.rotation = Quaternion.Euler(m_ETHStartRotation);
            //m_EthObject.gameObject.transform.rotation = Quaternion.Euler(m_ETHStartRotation);
            m_EthObject.GetComponent<Renderer>().material.color = Color.green;
            Debug.Log("ETH WENT UP!");
        }
        if (m_LastETHValue > m_ETHValue)
        {
            m_EthObject.transform.rotation = Quaternion.RotateTowards(m_EthObject.transform.rotation,Quaternion.Euler(m_ETHDownRotation), Time.deltaTime * 100);
            //m_ETHImage.transform.rotation = Quaternion.Euler(m_ETHDownRotation);
            //m_EthObject.gameObject.transform.rotation = Quaternion.Euler(m_ETHDownRotation);
            m_EthObject.GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("ETH WENT DOWN!");
        }
        if (m_LastETHValue == m_ETHValue)
        {
            m_EthObject.transform.rotation = Quaternion.RotateTowards(m_EthObject.transform.rotation, Quaternion.Euler(m_ETHSideRotation), Time.deltaTime * 100);
            //m_ETHImage.transform.rotation = Quaternion.Euler(m_ETHSideRotation);
            m_EthObject.GetComponent<Renderer>().material.color = Color.yellow;
            //m_EthObject.gameObject.transform.rotation = Quaternion.Euler(m_ETHSideRotation);
        }
    }

    private void UpdateBTC()
    {
        if (m_LastBTCValue < m_BTCValue)
        {
            m_BTCObject.transform.rotation = Quaternion.RotateTowards(m_BTCObject.transform.rotation, Quaternion.Euler(m_BTCStartRotation), Time.deltaTime * 100);
            //m_BTCImage.transform.rotation = Quaternion.Euler(m_BTCStartRotation);
            m_BTCObject.GetComponent<Renderer>().material.color = Color.green;
            //m_BTCObject.gameObject.transform.rotation = Quaternion.Euler(m_BTCStartRotation);
            Debug.Log("btc WENT UP!");
        }
        if (m_LastBTCValue > m_BTCValue)
        {
            //m_BTCImage.transform.rotation = Quaternion.Euler(m_BTCDownRotation);
            //m_BTCObject.gameObject.transform.rotation = Quaternion.Euler(m_BTCDownRotation);
            m_BTCObject.GetComponent<Renderer>().material.color = Color.red;
            m_BTCObject.transform.rotation = Quaternion.RotateTowards(m_BTCObject.transform.rotation, Quaternion.Euler(m_BTCDownRotation), Time.deltaTime * 100);
            Debug.Log("btc WENT DOWN!");
        }
        if (m_LastBTCValue == m_BTCValue)
        {
            m_BTCObject.transform.rotation = Quaternion.RotateTowards(m_BTCObject.transform.rotation, Quaternion.Euler(m_BTCSideRotation), Time.deltaTime * 100);
            //m_BTCImage.transform.rotation = Quaternion.Euler(m_BTCSideRotation);
            m_BTCObject.GetComponent<Renderer>().material.color = Color.yellow;
            //m_BTCObject.gameObject.transform.rotation = Quaternion.Euler(m_BTCSideRotation);
        }

    }

    private void Request()
    {
        WWW ETHRequest = new WWW(ETHURL);
        WWW BTCRequest = new WWW(BTCURL);
        StartCoroutine(OnRespondETH(ETHRequest));
        StartCoroutine(OnRespondBTC(BTCRequest));
    }

    private IEnumerator OnRespondETH(WWW Req)
    {
        yield return Req;
        GetETHValue(Req);
    }

    private IEnumerator OnRespondBTC(WWW Req2)
    {
        yield return Req2;
        GetBTCValue(Req2);
    }

    private void GetETHValue(WWW _Req)
    {
        m_Temp = _Req.text;
        m_ETHArray = m_Temp.ToCharArray();
        for (int i = 0; i < 49; i++)
        {
            m_ETHArray[i] = ' ';
        }
        for (int i = 56; i < 58; i++)
        {
            m_ETHArray[i] = ' ';
        }

        m_ETHPriceArray = new char[4];
        m_ETHPriceArray[0] = m_ETHArray[49];
        m_ETHPriceArray[1] = m_ETHArray[50];
        m_ETHPriceArray[2] = m_ETHArray[51];
        m_ETHPriceArray[3] = m_ETHArray[52];
        /*m_ETHPriceArray[4] = m_ETHArray[53];*/
        /*m_ETHPriceArray[5] = m_ETHArray[54];
        m_ETHPriceArray[6] = m_ETHArray[55];*/

        string VALUE = new string(m_ETHPriceArray);
        int difference = int.Parse(VALUE) - m_LastETHValue;
        if (difference.ToString() == VALUE)
        {
            m_LastETHValue = m_ETHValue;
        }
        m_EthText.text = VALUE + " : " + (int.Parse(VALUE) - m_LastETHValue);
        m_ETHValue = int.Parse(VALUE);
        Debug.Log(m_ETHValue);
    }

    private void GetBTCValue(WWW _Req2)
    {
        m_Temp2 = _Req2.text;
        m_BTCArray = m_Temp2.ToCharArray();
        for (int i = 0; i < 49; i++)
        {
            m_BTCArray[i] = ' ';
        }
        for (int i = 57; i < 59; i++)
        {
            m_BTCArray[i] = ' ';
        }

        m_BTCPriceArray = new char[5];
        m_BTCPriceArray[0] = m_BTCArray[49];
        m_BTCPriceArray[1] = m_BTCArray[50];
        m_BTCPriceArray[2] = m_BTCArray[51];
        m_BTCPriceArray[3] = m_BTCArray[52];
        m_BTCPriceArray[4] = m_BTCArray[53];
        /*m_BTCPriceArray[5] = m_BTCArray[54];
        m_BTCPriceArray[6] = m_BTCArray[55];*/
        /*m_BTCPriceArray[7] = m_BTCArray[56];*/

        string VALUE2 = new string(m_BTCPriceArray);
        int difference = int.Parse(VALUE2) - m_LastBTCValue;
        if (difference.ToString() == VALUE2)
        {
            m_LastBTCValue = m_BTCValue;
        }
        m_BTCText.text = VALUE2 + " : " + difference;
        m_BTCValue = int.Parse(VALUE2);
        Debug.Log(m_BTCValue);
    }
}
