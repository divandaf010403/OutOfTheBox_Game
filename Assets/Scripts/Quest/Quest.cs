using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public enum QuestProgress { NOT_AVAILABLE, AVAILABLE, ACCEPTED, COMPLETE, DONE }

    public string title;            //title quest
    public int id;                  //ID number quest
    public QuestProgress progress;  //state of quest
    public string descirption;      //quest giver/receiver
    public string hint;             //quest giver/receiver
    public string congratulation;   //quest giver/receiver
    public string summery;          //quest giver/receiver
    public int nextQuest;           //next quest if available

    public string questObjective;   //name of quest obejctive (alse name)
    public int questObjectiveCount; //current number of quest
    public int questObjectiveReq;   //require objective quest

    public int expReward;
    public int goldReward;
    public string itemReward;
}
