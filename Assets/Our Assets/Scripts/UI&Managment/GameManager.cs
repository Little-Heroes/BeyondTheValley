using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //journal stuff
    public struct WhenAndWhere
    {
        private int level;
        private int subLevel;
        private float runTime;

        public WhenAndWhere(int _level, int _subLevel)
        {
            level = _level;
            subLevel = _subLevel;
            runTime = Time.time;
        }

    }

    private struct PossessInfo
    {
        public WhenAndWhere initPossessMoment;
        public int killsWith;
        public WhenAndWhere expungedMoment;
    }
    int currentPossess = 0;
    List<PossessInfo> possessInfos = new List<PossessInfo>();

    //current level stuff
    private int level;
    private int subLevel;
    private float runTime;

    //GameOverStuff
    public Canvas GameOverCanvas;

    private Button mainMenu;
    private Button retry;
    private Button quitGame;
    private Button journal;

    private Image killedBy;

    #region startup methods
    private void setupGOC()
    {
        if (GameOverCanvas)
        {
            Button[] butts = GameOverCanvas.gameObject.GetComponentsInChildren<Button>();
            foreach (Button button in butts)
            {
                if (button.name == "ReturnToMenu") { mainMenu = button; }
                if (button.name == "Retry") { retry = button; }
                if (button.name == "Quit") { quitGame = button; }
                if (button.name == "Journal") { journal = button; }
            }
            GameOverCanvas.enabled = false;
        }
    }

    #endregion startup methods

    #region Button Methods

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion Button Methods

    private void Start()
    {
        setupGOC();
        level = 1;
        subLevel = 1;
        runTime = Time.time;
    }
    private void Update()
    {
        runTime += Time.deltaTime;
    }              

    public void GameOver()
    {
        if (GameOverCanvas) { GameOverCanvas.enabled = true; }
    }

    public void OnPossess(WhenAndWhere waw)
    {
        PossessInfo posInfo = new PossessInfo();
        posInfo.initPossessMoment = waw;
        possessInfos.Add(posInfo);
    }
    public void OnExpunge(int kills, WhenAndWhere waw)
    {
        PossessInfo temp = possessInfos[currentPossess];
        temp.killsWith = kills;
        temp.expungedMoment = waw;
        possessInfos[currentPossess] = temp;
        currentPossess++;
    }
}
