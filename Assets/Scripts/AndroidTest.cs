using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AndroidTest : MonoBehaviour
{
    public Button btnAli;
    public Button btnWX;
    public Button btnTest;
    public TMP_Text txtOutput;

    private static AndroidJavaObject currentActivity;
    private static AndroidJavaClass androidLibPlugin;
    private static AndroidJavaObject honorActivity;
    
    // Start is called before the first frame update
    void Start()
    {
        btnAli.onClick.AddListener(OnClickAli);
        btnWX.onClick.AddListener(OnClickWX);
        btnTest.onClick.AddListener(OnClickTest);
        
#if UNITY_ANDROID && !UNITY_EDITOR
        try {
            // 获取当前Activity
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            
            // 初始化支付Activity
            androidLibPlugin = new AndroidJavaClass("com.plugins.androidLib.AndroidLibPlugin");
            Debug.LogWarning($"YSH Start androidLibPlugin = {androidLibPlugin.GetType()}");
            // androidLibPlugin.CallStatic("SendMsgToUnity", gameObject.name, "ReceiveFromAndroid", "YSH androidLibPlugin SendMsgToUnity");

            // 初始化荣耀Activity
            // honorActivity = new AndroidJavaClass("com.plugins.honormaket.MainActivity");
            // honorActivity.Call("SendMsgToUnity", gameObject.name, "ReceiveFromAndroid", "YSH honorActivity SendMsgToUnity");
            // Debug.LogWarning($"YSH Start honorActivity = {honorActivity.GetType()}");
        }
        catch (System.Exception e) {
            Debug.LogError($"YSH Error initializing Android components: {e.Message}\n{e.StackTrace}");
        }
#endif
    }

    public void ReceiveFromAndroid(string msg)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log($"YSH ReceiveFromAndroid: {msg}");
        txtOutput.text = msg;
#endif
    }

    public void ReceiveFromHonor(string msg)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log($"YSH ReceiveFromHonor: {msg}");
        txtOutput.text = msg;
#endif
    }

    public void ALiPayResult(string result)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log($"YSH ALiPayResult: {result}");
        txtOutput.text = result;
#endif
    }

    void OnClickAli()
    {
        Debug.Log("YSH OnClickAli");
#if UNITY_ANDROID && !UNITY_EDITOR
        if (androidLibPlugin != null) {
            androidLibPlugin.CallStatic("AliPayByApp", "1234567890");
        }
#endif
    }

    void OnClickWX()
    {
        Debug.Log("YSH OnClickWX");
#if UNITY_ANDROID && !UNITY_EDITOR
        if (androidLibPlugin != null) {
            androidLibPlugin.CallStatic("WxPayByApp", "appid", "partnerid", "prepayid", "noncestr", "timestamp", "sign");
        }
#endif
    }

    void OnClickTest()
    {
        Debug.Log("YSH OnClickTest");
#if UNITY_ANDROID && !UNITY_EDITOR
        // 这种打开activity的方式有效果
        // 创建Intent来启动荣耀更新Activity
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", 
            currentActivity, 
            new AndroidJavaClass("com.plugins.androidLib.HonorMarket"));
        
        currentActivity.Call("startActivity", intent);
#endif
    }
}
