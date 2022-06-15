using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameData : MonoBehaviour
{
    [SerializeField]
    GameObject panelDefeat;

    [SerializeField]
    TMPro.TextMeshProUGUI[] txtCoins;

    [SerializeField]
    int totalKillToWin;
    public static int nowKill;
    [SerializeField]
    GameObject winPanel;


    [Header("Level up related")]
    [SerializeField]
    GameObject panelUpgrade;
    [SerializeField]
    TMPro.TextMeshProUGUI txtUpgradeInfo;
    [SerializeField]
    TMPro.TextMeshProUGUI txtId;
    [SerializeField]
    TMPro.TextMeshProUGUI txtHp;
    [SerializeField]
    TMPro.TextMeshProUGUI txtDmg;
    [SerializeField]
    TMPro.TextMeshProUGUI txtRageTime;
    int nextCoinsToUp;
    [SerializeField]
    GameObject btnUpgrade;

    PlayerSaveSystem saveSystem;
    GameSystem gameSys;
    [SerializeField]
    AudioSource bgm;

    void Start() {
        GameObject o = GameObject.Find("GameSystem");
        saveSystem = o.GetComponent<PlayerSaveSystem>();
        gameSys = o.GetComponent<GameSystem>();
    }

    void Update()
    {
        CoinsRedraw();
        PanelShow();
        WinWatcher();
    }

    

    void WinWatcher() {
        if (nowKill >= totalKillToWin) {
            Time.timeScale = 0f;
			Cursor.visible = true;
			Camera.main.transform.gameObject.GetComponent<CameraFollow>().enabled= false;
            winPanel.SetActive(true);
        }
    }

    void PanelShow() {
        if (Input.GetButtonDown("Cancel")) {
            if (panelUpgrade.activeInHierarchy)
            {
                bgm.UnPause();
                Time.timeScale = 1f;
                panelUpgrade.SetActive(false);
            }
            else {
                bgm.Pause();
                Time.timeScale = 0f;
                DrawLevelUpPanel();
                panelUpgrade.SetActive(true);
            }
        }
    }

    public void DrawLevelUpPanel() {
        STRLevelData data = gameSys.GetPlayerLevelData(out int levelPlayer);
        STRStatus status = data.GetStatusData();
        STRId ids = data.GetIdData();
        txtId.SetText(ids.name + " LV "+levelPlayer+", "+ids.shortDesc);
        txtHp.SetText("HP "+status.hp);
        txtDmg.SetText("DMG "+status.dmg);
        txtRageTime.SetText("RAGE "+status.rageTime);

        if (levelPlayer == gameSys.GetTotalMaxLevel() - 1)
        {
            btnUpgrade.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().SetText("Max");
        }
        else { 
            btnUpgrade.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().SetText("Upgrade");
        }
    }

    public void BtnUpgrade() {
        if (btnUpgrade.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text != "Max") {
            if (saveSystem.GetCoins() >= gameSys.GetUpgradeCost())
            {
                saveSystem.UpgradePlayer(gameSys.GetUpgradeCost());
                DrawLevelUpPanel();
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ReThinkStatus();
                txtUpgradeInfo.SetText("Upgrade success.. saved");
                Debug.Log("Upgrade success");
            }
            else {
                txtUpgradeInfo.SetText("Upgrade failed you need coins "+ gameSys.GetUpgradeCost());
                Debug.Log("Upgradef failed");
            }
        }
    }

    public void ShowDefeatPanel() {
        panelDefeat.SetActive(true);
    }

    public void BtnGotToSceneAsync(string sceneName) {
        GameObject.Find("GameSystem").GetComponent<PlayerSaveSystem>().SaveGame();
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void BtnReloadScene() {
        nowKill = 0;
        GameObject.Find("GameSystem").GetComponent<PlayerSaveSystem>().SaveGame();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public void BtnExitGame() {
        GameObject.Find("GameSystem").GetComponent<PlayerSaveSystem>().SaveGame();
        Application.Quit();
    }



    int coinsRedr = 0;
    void CoinsRedraw() {
        coinsRedr = 0;
        while (coinsRedr < txtCoins.Length) {
            txtCoins[coinsRedr].SetText(saveSystem.GetCoins().ToString());
            coinsRedr++;
        }
    }
}
