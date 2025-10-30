using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

class StoryBlock
{
    public string story;
    public string option1Text;
    public string option2Text;
    public StoryBlock option1Block;
    public StoryBlock option2Block;

    public StoryBlock(string story, string option1Text = "", string option2Text = "", StoryBlock option1Block = null, StoryBlock option2Block = null)
    {
        this.story = story;
        this.option1Text = option1Text;
        this.option2Text = option2Text;
        this.option1Block = option1Block;
        this.option2Block = option2Block;
    }
}

public class EventLevelManager : MonoBehaviour
{
    public Text mainText;
    public Button option1;
    public Button option2;

    StoryBlock currentBlock;
    List<StoryBlock> eventList = new List<StoryBlock>();

    void Start()
    {
        InitializeEvents();
        StartRandomEvent();
    }

    void InitializeEvents()
    {
        // --- Event 1 (Lighthouse) ---
        StoryBlock e1_block3 = new StoryBlock("You avoid the lighthouse and find as you make your way around it that a monster waits to ambush you. Avoiding it fills you with determination. Heal 2 damage.");
        StoryBlock e1_block2 = new StoryBlock("A monster appears from the shadows and gives chase to the ship. Take 2 damage.");
        StoryBlock e1_block1 = new StoryBlock("A lighthouse shows in the distance of the foggy waters.", "Venture Forth", "Go Around It", e1_block2, e1_block3);

        eventList.Add(e1_block1);

        // --- Event 2 (Storm Gathering) ---
        StoryBlock e2_block3 = new StoryBlock("You decide to rest and recover some strength. Heal 1 damage.");
        StoryBlock e2_block2 = new StoryBlock("You push forward into the storm and the mast cracks. Take 1 damage.");
        StoryBlock e2_block1 = new StoryBlock("Dark storm clouds gather above the sea.", "Press On", "Drop Anchor and Wait", e2_block2, e2_block3);

        eventList.Add(e2_block1);

        // --- Event 3 (Bad Food) ---
        StoryBlock e3_block3 = new StoryBlock("You decide to eat the food despite its questionable appearance. You feel sick and take 1 damage.");
        StoryBlock e3_block2 = new StoryBlock("You choose to skip the meal and conserve your strength. Heal 1 damage.");
        StoryBlock e3_block1 = new StoryBlock("The crew offers you a meal, but it looks unappetizing.", "Eat the Food", "Skip the Meal", e3_block3, e3_block2);

        eventList.Add(e3_block1);

        // --- Event 4 (A Round of Grog) ---
        StoryBlock e4_block3 = new StoryBlock("You decline the grog and focus on your duties. Heal 1 damage.");
        StoryBlock e4_block2 = new StoryBlock("You drink the grog and feel invigorated. Heal 2 damage.");
        StoryBlock e4_block1 = new StoryBlock("The crew offers you a round of grog to boost morale.", "Drink the Grog", "Decline", e4_block2, e4_block3);

        eventList.Add(e4_block1);

        // --- Event 5 (Mysterious Island) ---
        StoryBlock e5_block3 = new StoryBlock("You decide to avoid the island and continue your journey.");
        StoryBlock e5_block2 = new StoryBlock("You explore the island and find hidden treasures. Gain 5 gold but loose 2 health.");
        StoryBlock e5_block1 = new StoryBlock("You spot a mysterious island on the horizon.", "Explore the Island", "Sail Past It", e5_block2, e5_block3);

        eventList.Add(e5_block1);

        // -- Event 6 (Ship in the Distance) ---
        StoryBlock e6_block3 = new StoryBlock("You ignore the ship and continue on your course.");
        StoryBlock e6_block2 = new StoryBlock("You investigate the ship and find it abandoned. You take it's treasure. Gain 10 gold.");
        StoryBlock e6_block1 = new StoryBlock("A crew member from the crows nest spots a ship in the distance.", "Investigate the Ship", "Ignore It", e6_block2, e6_block3);

        eventList.Add(e6_block1);

    }

    void StartRandomEvent()
    {
        if (eventList.Count == 0)
        {
            Debug.LogError("No events initialized!");
            return;
        }

        int index = Random.Range(0, eventList.Count);
        DisplayBlock(eventList[index]);
    }

    void DisplayBlock(StoryBlock block)
    {
        currentBlock = block;
        mainText.text = block.story;

        bool hasOptions = !(string.IsNullOrEmpty(block.option1Text) && string.IsNullOrEmpty(block.option2Text));
        option1.gameObject.SetActive(hasOptions);
        option2.gameObject.SetActive(hasOptions);

        if (hasOptions)
        {
            option1.GetComponentInChildren<Text>().text = block.option1Text;
            option2.GetComponentInChildren<Text>().text = block.option2Text;
        }
    }

    public void Button1Clicked()
    {
        if (currentBlock.option1Block != null)
            DisplayBlock(currentBlock.option1Block);
    }

    public void Button2Clicked()
    {
        if (currentBlock.option2Block != null)
            DisplayBlock(currentBlock.option2Block);
    }
}