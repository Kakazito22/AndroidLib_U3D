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

    public void SetPackageInfo(string packageName, string version, int versionCode)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        androidLibPlugin.CallStatic("SetPackageInfo", packageName, version, versionCode);
#endif
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

    public void OnCheckUpdateResult(string response)
    {
        string tips = "YSH PluginPlatform OnCheckUpdateResult response = " + response;
        Debug.Log(tips);

        if (response == "UPDATE")
        {
            marketUpdateUI.ShowConfirmDialog("更新提示", "检测到应用有更新，是否前往应用市场进行更新？",
                "前往更新", "退出应用",
                () => {
                    var b = GetDeviceBrand();
                    Debug.LogWarning("YSH User confirmed to update the app. Brand = "+b);
                    marketUpdateUI.txtContent.text = "用户选择前往更新应用";
                    CallAndroidLibStaticMethod("JumpMarketDetail", (int)b);
                },
                () => {
                    Debug.LogWarning("YSH User chose to quit the app instead of updating.");
                    marketUpdateUI.txtContent.text = "用户选择退出应用";
                });
        }
        else if (response == "NO_UPDATE")
        {
            marketUpdateUI.txtContent.text = "沒有检测到更新";
        }
        else
        {
            marketUpdateUI.txtContent.text = "测更新失败，请检查网络后重试！";
            marketUpdateUI.ShowConfirmDialog("提示", "检测更新失败，请检查网络后重试！",
                "重试", "退出应用",
                () => {
                    Debug.LogWarning("YSH User chose to retry checking for updates.");
                    marketUpdateUI.txtContent.text = "用户选择重试检测更新";
                    CheckMarketUpdate();
                },
                () => {
                    Debug.LogWarning("YSH User chose to quit the app instead of retrying.");
                    marketUpdateUI.txtContent.text = "用户选择退出应用";
                });
        }

        
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
