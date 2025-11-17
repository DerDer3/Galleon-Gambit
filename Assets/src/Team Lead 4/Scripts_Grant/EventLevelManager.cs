using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

#region ----- TEMPLATE PATTERN -----

// Story node for branching narrative
public class StoryBlock
{
    public string story;
    public string option1Text;
    public string option2Text;

    public StoryBlock option1Block;
    public StoryBlock option2Block;

    public StoryBlock(string story, string option1 = "", string option2 = "",
                      StoryBlock opt1Block = null, StoryBlock opt2Block = null)
    {
        this.story = story;
        this.option1Text = option1;
        this.option2Text = option2;
        this.option1Block = opt1Block;
        this.option2Block = opt2Block;
    }
}


//----------------------------
// TEMPLATE BASE CLASS
//----------------------------
public abstract class StoryEventTemplate
{
    protected StoryBlock root;

    // Template Method
    public StoryBlock BuildEvent()
    {
        CreateBlocks();
        LinkBlocks();
        return root;
    }

    // Steps subclasses must implement
    protected abstract void CreateBlocks();
    protected abstract void LinkBlocks();
}

#endregion

#region ----- STATIC EVENT DEFINITIONS -----

// Each event gets its own class.
// All are statically bound (compile-time known).

public class LighthouseEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks()
    {
        b3 = new StoryBlock(
            "You avoid the lighthouse and a monster waits to ambush you. Avoiding it fills you with determination. Heal 2 damage."
        );

        b2 = new StoryBlock(
            "A monster appears from the shadows and gives chase to the ship. Take 2 damage."
        );

        b1 = new StoryBlock(
            "A lighthouse shows in the distance of the foggy waters.",
            "Venture Forth", "Go Around It"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b2;
        b1.option2Block = b3;
    }
}

public class StormEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks() // Removing override will create a compile-time error, static binding.
    {
        b3 = new StoryBlock("You decide to rest. Heal 1 damage.");
        b2 = new StoryBlock("You push forward and the mast cracks. Take 1 damage.");
        b1 = new StoryBlock("Dark clouds gather above.", "Press On", "Drop Anchor");

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b2;
        b1.option2Block = b3;
    }
}

public class BadFoodEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks()
    {
        b3 = new StoryBlock("You eat the food. You feel sick and take 1 damage.");
        b2 = new StoryBlock("You skip the meal. Heal 1 damage.");
        b1 = new StoryBlock("The crew offers you a meal.", "Eat the Food", "Skip the Meal");

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b3;
        b1.option2Block = b2;
    }
}

// Add more events in same pattern…

#endregion

//==============================================================
//                 MANAGER — USES TEMPLATE CLASSES
//==============================================================
public class EventLevelManager : MonoBehaviour
{
    public Text mainText;
    public Button option1;
    public Button option2;

    StoryBlock currentBlock;

    // Static binding: compile-time event list
    List<StoryEventTemplate> events = new List<StoryEventTemplate>()
    {
        new LighthouseEvent(),
        new StormEvent(),
        new BadFoodEvent(),
        // Add new events here…
    };

    void Start()
    {
        StartRandomEvent();
    }

    void StartRandomEvent()
    {
        int index = Random.Range(0, events.Count);
        StoryBlock start = events[index].BuildEvent();
        DisplayBlock(start);
    }

    void DisplayBlock(StoryBlock block)
    {
        currentBlock = block;
        mainText.text = block.story;

        bool hasOptions = !(string.IsNullOrEmpty(block.option1Text) &&
                            string.IsNullOrEmpty(block.option2Text));

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
