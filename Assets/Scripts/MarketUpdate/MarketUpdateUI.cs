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
    public Button btnSetPkgInfo;
    public TMP_InputField inputPackage;
    public TMP_InputField inputVersion;
    public TMP_InputField inputVersionCode;

    // 弹窗相关 UI（在 Inspector 中绑定）
    public GameObject confirmDialog;
    public TMP_Text confirmDialogTitle;
    public TMP_Text confirmDialogMessage;
    public Button confirmDialogButton;
    public Button cancelDialogButton;
    // 回调引用（用于在隐藏时清理或调用）
    private Action confirmCallback;
    private Action cancelCallback;

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
        btnSetPkgInfo.onClick.AddListener(onbtnSetPkgInfo);

        ShowDefaultPkgInfo();
        // 初始确保 dialog 隐藏
        if (confirmDialog != null)
            confirmDialog.SetActive(false);
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

    void onbtnSetPkgInfo()
    {
        string pkg = inputPackage.text;
        string ver = inputVersion.text;
        int verCode = int.Parse(inputVersionCode.text);
        Debug.LogWarning(string.Format("onbtnSetPkgInfo pkg = {0} version = {1} versionCode = {2}", pkg, ver, verCode));
        PluginPlatform.Instance.SetPackageInfo(pkg, ver, verCode);
    }

    void ShowDefaultPkgInfo()
    {
        inputPackage.text = Application.identifier;
        inputVersion.text = Application.version;
        inputVersionCode.text = "1";

    }

    // ---------- 确认/取消 二次弹窗 功能 ----------

    /// <summary>
    /// 显示确认弹窗（带确认与取消按钮），按钮文字和回调可配置。
    /// 需要在 Inspector 中为 confirmDialog、confirmDialogTitle、confirmDialogMessage、
    /// confirmDialogButton、cancelDialogButton 赋值（可用一个预制体）。
    /// </summary>
    public void ShowConfirmDialog(string title, string message,
        string confirmText = "确定", string cancelText = "取消",
        Action onConfirm = null, Action onCancel = null)
    {
        if (confirmDialog == null)
        {
            Debug.LogWarning("Confirm dialog GameObject is not assigned. Invoking confirm callback directly.");
            onConfirm?.Invoke();
            return;
        }

        // 确保 UI 存在
        if (confirmDialogTitle != null) confirmDialogTitle.text = title ?? "";
        if (confirmDialogMessage != null) confirmDialogMessage.text = message ?? "";

        SetButtonText(confirmDialogButton, confirmText);
        SetButtonText(cancelDialogButton, cancelText);

        // 清理旧监听，避免重复执行
        if (confirmDialogButton != null) confirmDialogButton.onClick.RemoveAllListeners();
        if (cancelDialogButton != null) cancelDialogButton.onClick.RemoveAllListeners();

        confirmCallback = onConfirm;
        cancelCallback = onCancel;

        if (confirmDialogButton != null)
        {
            confirmDialogButton.onClick.AddListener(() =>
            {
                try { confirmCallback?.Invoke(); }
                catch (Exception e) { Debug.LogError($"Error in confirm callback: {e}"); }
                HideConfirmDialog();
            });
        }

        if (cancelDialogButton != null)
        {
            cancelDialogButton.onClick.AddListener(() =>
            {
                try { cancelCallback?.Invoke(); }
                catch (Exception e) { Debug.LogError($"Error in cancel callback: {e}"); }
                HideConfirmDialog();
            });
        }

        confirmDialog.SetActive(true);
    }

    /// <summary>
    /// 隐藏并清理弹窗
    /// </summary>
    public void HideConfirmDialog()
    {
        if (confirmDialog == null)
            return;

        // 尝试移除所有监听并清除回调引用
        if (confirmDialogButton != null) confirmDialogButton.onClick.RemoveAllListeners();
        if (cancelDialogButton != null) cancelDialogButton.onClick.RemoveAllListeners();

        confirmCallback = null;
        cancelCallback = null;

        confirmDialog.SetActive(false);

        // 清空文本（可选）
        if (confirmDialogTitle != null) confirmDialogTitle.text = "";
        if (confirmDialogMessage != null) confirmDialogMessage.text = "";
    }

    private void SetButtonText(Button btn, string text)
    {
        if (btn == null) return;
        // 优先查找 TMP_Text，再退回到 legacy Text
        var tmp = btn.GetComponentInChildren<TMP_Text>();
        if (tmp != null) { tmp.text = text ?? ""; return; }

        var legacy = btn.GetComponentInChildren<Text>();
        if (legacy != null) legacy.text = text ?? "";
    }
}
