using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MarketUpdateUI : MonoBehaviour
{
    public TMP_Text txtContent;
    public TMP_Text txtDeviceInfo;
    public Button btnCheckUpdate;
    public Button btnCheckUpdateHW;
    public Button btnTestOKHTTP;
    public Button btnTestHonorUpdate;
    public Button btnXiaomiAutoUpdate;
    public Button btnXiaomiUpdate;
    public Button btnJumpVivo;
    public Button btnJumpOppo;
    public Button btnJumpXiaomi;
    public Button btnJumpHuawei;
    public Button btnJumpRY;

    // Start is called before the first frame update
    void Start()
    {
        txtDeviceInfo.text = string.Format("Model: {0}, Name:{1}", SystemInfo.deviceModel, SystemInfo.deviceName);

        btnCheckUpdate.onClick.AddListener(onClickCheckUpdate);
        btnTestOKHTTP.onClick.AddListener(onClickTestOKHTTP);
        btnTestHonorUpdate.onClick.AddListener(onClickTestHonorUpdate);
        btnXiaomiAutoUpdate.onClick.AddListener(onbtnXiaomiAutoUpdate);
        btnXiaomiUpdate.onClick.AddListener(onbtnXiaomiUpdate);
        btnJumpVivo.onClick.AddListener(onbtnJumpVivo);
        btnJumpOppo.onClick.AddListener(onbtnJumpOppo);
        btnJumpXiaomi.onClick.AddListener(onbtnJumpXiaomi);
        btnCheckUpdateHW.onClick.AddListener(onbtnCheckUpdateHW);
        btnJumpHuawei.onClick.AddListener(onbtnJumpHuawei);
        btnJumpRY.onClick.AddListener(onbtnJumpRY);
    }

    private void onClickCheckUpdate()
    {
        txtContent.text = "onClickCheckUpdate";
        PluginPlatform.Instance.CheckMarketUpdate();
    }

    private void onClickTestOKHTTP()
    {
        txtContent.text = "onClickTestOKHTTP";
        PluginPlatform.Instance.TestOKHTTP();
    }

    private void onClickTestHonorUpdate()
    {
        txtContent.text = "测试荣耀更新弹窗";
        PluginPlatform.Instance.CallAndroidLibStaticMethod("handleUpdateWithAppMarket");
    }

    private void onbtnXiaomiAutoUpdate()
    {
        txtContent.text = "onbtnXiaomiAutoUpdate";
        PluginPlatform.Instance.CallAndroidLibStaticMethod("CheckXiaomiAutoUpdate");
    }

    private void onbtnXiaomiUpdate()
    {
        txtContent.text = "onbtnXiaomiUpdate";
        PluginPlatform.Instance.CallAndroidLibStaticMethod("CheckXiaomiUpdate");
    }

    void onbtnJumpVivo(){
        PluginPlatform.Instance.VivoUpdate();
    }



    void onbtnJumpOppo()
    {
        // PluginPlatform.Instance.CallAndroidLibStaticMethod("DoOPPOUpdate");
        PluginPlatform.Instance.CallAndroidLibStaticMethod("DebubgOPPOSign");
    }

    void onbtnJumpXiaomi()
    {
        PluginPlatform.Instance.CallAndroidLibStaticMethod("DoXiaomiUPdate");
    }

    void onbtnCheckUpdateHW()
    {
        PluginPlatform.Instance.CallAndroidLibStaticMethod("CheckHuaweiUpdate");
    }

    void onbtnJumpHuawei()
    {
        PluginPlatform.Instance.CallAndroidLibStaticMethod("DoHuaweiUpdate");
    }

    void onbtnJumpRY()
    {
        PluginPlatform.Instance.CallAndroidLibStaticMethod("DoHonorUpdate");
    }
}
