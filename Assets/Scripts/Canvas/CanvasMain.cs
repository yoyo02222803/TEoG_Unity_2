﻿using System.Collections.Generic;
using UnityEngine;

public class CanvasMain : MonoBehaviour
{
    #region Properties

    [SerializeField] private KeyBindings Keys = null;
    [SerializeField] private GameObject Gameui = null;
    [SerializeField] private GameObject Battle = null;
    [SerializeField] private GameObject Menus = null;
    [SerializeField] private GameObject Buildings = null;
    [SerializeField] private GameObject PauseMenu = null;
    [SerializeField] private HomeMain Home = null;

    [SerializeField]
    private MenuPanels menuPanels = new MenuPanels();

    [SerializeField]
    private CombatMain combatMain;

    #endregion Properties

    private void Update()
    {
        if (Keys.escKey.GetKeyDown())
        {
            EscapePause();
        }
        else if (Keys.hideAllKey.GetKeyDown())
        {
            if (GameManager.CurState.Equals(GameState.Free))
            {
                Gameui.SetActive(!Gameui.activeSelf);
            }
        }
        // if in menus or main game(not combat)
        if (GameManager.KeyBindsActive)
        {
            if (Keys.saveKey.GetKeyDown())
            {
                ResumePause(menuPanels.Savemenu);
            }
            else if (Keys.optionsKey.GetKeyDown())
            {
                ResumePause(menuPanels.Options);
            }
            else if (Keys.questKey.GetKeyDown())
            {
                ResumePause(menuPanels.QuestMenu);
            }
            else if (Keys.inventoryKey.GetKeyDown())
            {
                ResumePause(menuPanels.Inventory);
            }
            else if (Keys.voreKey.GetKeyDown())
            {
                ResumePause(menuPanels.Vore);
            }
            else if (Keys.essenceKey.GetKeyDown())
            {
                ResumePause(menuPanels.Essence);
            }
            else if (Keys.lvlKey.GetKeyDown())
            {
                ResumePause(menuPanels.LevelUp);
            }
            else if (Keys.lookKey.GetKeyDown())
            {
                ResumePause(menuPanels.Looks);
            }
            if (Keys.eventKey.GetKeyDown())
            {
                BigEventLog();
            }
        }
    }

    public void Intro()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.GetChild(0).gameObject.SetActive(true);
        GameManager.CurState = GameState.Intro;
    }

    public void Resume()
    {
        foreach (Transform child in Menus.transform)
        {
            child.gameObject.SetActive(false);
        }
        ToggleBigPanel(Gameui);
        GameManager.CurState = GameState.Free;
    }

    public void Pause()
    {
        ToggleBigPanel(Menus);
        foreach (Transform child in Menus.transform)
        {
            child.gameObject.SetActive(false);
        }
        GameManager.CurState = GameState.Menu;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public bool BigEventLog()
    {
        if (GameManager.CurState.Equals(GameState.Free))
        {
            Pause();
            menuPanels.Bigeventlog.SetActive(true);
            return true;
        }
        Resume();
        return false;
    }

    public void StartCombat(EnemyPrefab enemy)
    {
        GameManager.CurState = GameState.Battle;
        foreach (Transform p in Battle.transform)
        {
            p.gameObject.SetActive(false);
        }
        ToggleBigPanel(Battle);
        List<EnemyPrefab> toAdd = new List<EnemyPrefab> { enemy };
        combatMain.SetUpCombat(toAdd);
    }

    private void ToggleBigPanel(GameObject toActivate)
    {
        foreach (Transform bigPanel in transform)
        {
            bigPanel.gameObject.SetActive(false);
        }
        toActivate.SetActive(true);
    }

    public void ResumePause(GameObject toBeActivated)
    {
        if (GameManager.CurState.Equals(GameState.Menu))
        {
            Resume();
        }
        else
        {
            Pause();
            toBeActivated.SetActive(true);
        }
    }

    public void EnterHome()
    {
        GameManager.CurState = GameState.Home;
        ToggleBigPanel(Home.gameObject);
        Home.transform.SleepChildren(Home.transform.GetChild(0));
    }

    public void EscapePause()
    {
        if (GameManager.CurState.Equals(GameState.Menu))
        {
            Resume();
        }
        else if (GameManager.CurState.Equals(GameState.PauseMenu))
        {
            if (GameManager.LastState.Equals(GameState.Free) || GameManager.LastState.Equals(GameState.Menu) || GameManager.LastState.Equals(GameState.PauseMenu))
            {
                Resume();
            }
            else
            {
                PauseMenu.SetActive(false);
                GameManager.CurState = GameManager.LastState;
            }
        }
        else
        {
            GameManager.CurState = GameState.PauseMenu;
            PauseMenu.SetActive(true);
        }
    }

    public void EnterBuilding(GameObject buildingToEnter)
    {
        GameManager.CurState = GameState.Home;
        ToggleBigPanel(Buildings);
        // Disable all buildings
        foreach (Transform building in Buildings.transform)
        {
            building.gameObject.SetActive(false);
        }
        buildingToEnter.SetActive(true);
    }
}