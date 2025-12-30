using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum Brand
{
    Unknown,
    Honor,
    Oppo,
    Vivo,
    Xiaomi,
    Huawei,
}

public class PluginPlatform : MonoBehaviour
{
    public static PluginPlatform Instance { get { return instance; } }
    private static PluginPlatform instance;

    private static AndroidJavaObject currentActivity;
    private static AndroidJavaClass androidLibPlugin;

    public MarketUpdateUI marketUpdateUI;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = new PluginPlatform();
        Debug.LogWarning("YSH Start pkgName = " + Application.identifier);

#if UNITY_ANDROID && !UNITY_EDITOR
        try {
            // 获取当前Activity
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            
            // 初始化安卓插件aar的Activity，传入应用包名
            androidLibPlugin = new AndroidJavaClass("com.plugins.androidLib.AndroidLibPlugin");
            androidLibPlugin.CallStatic("InitAndroidLib", Application.identifier);
        }
        catch (System.Exception e) {
            Debug.LogError($"YSH Error initializing Android components: {e.Message}\n{e.StackTrace}");
        }
#endif
    }

    public static Brand GetDeviceBrand()
    {
        if (Application.platform != RuntimePlatform.Android)
            return Brand.Unknown;

        string model = SystemInfo.deviceModel;
        string info = string.Format("{0}", model ?? "").ToLowerInvariant();

        // Honor can sometimes appear as manufacturer "HUAWEI", so check "honor" first
        if (info.Contains("honor"))
            return Brand.Honor;

        if (info.Contains("huawei"))
            return Brand.Huawei;

        // Xiaomi often reports "xiaomi" or "redmi" (Redmi is Xiaomi sub-brand)
        if (info.Contains("xiaomi") || info.Contains("redmi"))
            return Brand.Xiaomi;

        if (info.Contains("vivo") || info.Contains("iqoo"))
            return Brand.Vivo;

        if (info.Contains("oppo") || info.Contains("realme"))
            return Brand.Oppo;

        return Brand.Unknown;
    }

    /// <summary>
    /// 检测应用市场更新
    /// </summary>
    public void CheckMarketUpdate()
    {
        var b = GetDeviceBrand();
        Debug.LogWarning("YSH CheckMarketUpdate Brand = " + b);
#if UNITY_ANDROID && !UNITY_EDITOR
        androidLibPlugin.CallStatic("CheckMarketUpdate", (int)b);
#endif
    }

    public void CheckMarketUpdateResult(string response)
    {
        string tips = "YSH PluginPlatform CheckHonorUpdate CheckMarketUpdateResult response = " + response;
        Debug.LogWarning(tips);
        marketUpdateUI.txtContent.text = tips;
    }

    public void TestOKHTTP()
    {
        Debug.LogWarning("YSH TestOKHTTP");
#if UNITY_ANDROID && !UNITY_EDITOR
        androidLibPlugin.CallStatic("CheckOKHTTP");
#endif
    }

    public void TestOKHTTPResult(string response)
    {
        string tips = "YSH PluginPlatform TestOKHTTP response = " + response;
        Debug.LogWarning(tips);
        marketUpdateUI.txtContent.text = tips;
    }

    public void TestHonorUpdate()
    {
        Debug.LogWarning("YSH TestHonorUpdate");
#if UNITY_ANDROID && !UNITY_EDITOR
        androidLibPlugin.CallStatic("HandleHonorUpdate");
#endif
    }

    public void XiaomiUpdate(bool bAuto)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.LogWarning("YSH XiaomiUpdate bAuto = "+bAuto);
        if (bAuto)
            androidLibPlugin.CallStatic("CheckXiaomiAutoUpdate");
        else
            androidLibPlugin.CallStatic("CheckXiaomiManualUpdate");
#endif
    }

    public void OnVivoUpdateResult(string response){
        Debug.LogWarning("YSH PluginPlatform OnVivoUpdateResult response = "+response);
        marketUpdateUI.txtContent.text = "vivo 是否有更新 = " +response;
    }

    public void VivoUpdate()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.LogWarning("YSH VivoUpdate");
        androidLibPlugin.CallStatic("DoVivoUpdate");
#endif
    }


    public void CallAndroidLibStaticMethod(string methodName, params object[] args)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (androidLibPlugin != null)
        {
            androidLibPlugin.CallStatic(methodName, args);
        }
#endif
    }

    public void CallAndroidLibMethod(string methodName, params object[] args)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (androidLibPlugin != null)
        {
            androidLibPlugin.Call(methodName, args);
        }
#endif
    }
}
