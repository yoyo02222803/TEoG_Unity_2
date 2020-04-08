﻿using TMPro;
using UnityEngine;

public class TownHall : Building
{
    [SerializeField] private TextMeshProUGUI changeNameText = null;
    [SerializeField] private GameObject nameBox = null;
    [SerializeField] private TextMeshProUGUI textBox = null;
    [SerializeField] private GiveQuests giveQuests = new GiveQuests();
    [SerializeField] private GiveQuestRewards questRewards = new GiveQuestRewards();
    private string SetChangeNameBtnText { set => changeNameText.text = value; }

    private void SetSetTextBox(string value) => textBox.text = value;

    // Start is called before the first frame update
    public void Start()
    {
        foreach (QuestButton q in giveQuests.QuestButtons)
        {
            q.Btn.onClick.AddListener(() => Instantiate(giveQuests.QuestPanelPrefab, transform).Setup(q));
        }
        foreach (QuestRewardButton q in questRewards.RewardButtons)
        {
            q.Btn.onClick.AddListener(delegate
            {
                SetSetTextBox(QuestReward.GetReward(q.Quest));
                q.Btn.gameObject.SetActive(false);
            });
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        textBox.text = "Welcome";
        giveQuests.AlreadyHasQuest();
        questRewards.RewardButtons.ForEach(q => q.Btn.gameObject.SetActive(QuestsSystem.QuestIsCompleted(q.Quest)));
    }

    public void ToggleNameChange()
    {
        bool isActive = nameBox.activeSelf;
        nameBox.SetActive(!isActive);
        SetChangeNameBtnText = isActive ? "Change name" : "Back";
        SetSetTextBox("");
    }
}