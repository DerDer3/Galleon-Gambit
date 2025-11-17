using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

//-----------------------------------------
// 1. Lighthouse Event
//-----------------------------------------

public class LighthouseEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks()
    {
        b3 = new StoryBlock(
            "You steer wide of the lighthouse. As you pass it fills you & the crew with determination." //Copyright, transformative effect, gives new meaning.
        );

        b2 = new StoryBlock(
            "You draw closer and discover the lighthouse abandoned, its beacon flickering unevenly."
        );

        b1 = new StoryBlock(
            "A looming lighthouse emerges through the heavy fog.",
            "Approach the Light", "Sail Around It"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b2;
        b1.option2Block = b3;
    }
}

//-----------------------------------------
// 2. Storm Event
//-----------------------------------------
public class StormEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks()
    {
        b3 = new StoryBlock(
            "You anchor and wait. The crew shares stories as the storm slowly quiets."
        );

        b2 = new StoryBlock(
            "You press on. The winds batter the ship, but eventually the clouds begin to part."
        );

        b1 = new StoryBlock(
            "Dark storm clouds gather above.",
            "Sail Through", "Drop Anchor"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b2;
        b1.option2Block = b3;
    }
}

//-----------------------------------------
// 3. Bad Food Event
//-----------------------------------------
public class BadFoodEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks()
    {
        b3 = new StoryBlock(
            "You eat the questionable meal. The crew watches, unsure of your decision."
        );

        b2 = new StoryBlock(
            "You skip the meal and stay alert as others dine."
        );

        b1 = new StoryBlock(
            "The cook offers you a meal of… uncertain origin.",
            "Eat It", "Pass"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b3;
        b1.option2Block = b2;
    }
}

//-----------------------------------------
// 4. Siren Song Event
//-----------------------------------------
public class SirenSongEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks()
    {
        b3 = new StoryBlock(
            "You turn away from the singing, letting the eerie melody fade behind you."
        );

        b2 = new StoryBlock(
            "You move toward the sound and discover smooth stones arranged in strange patterns on the shoreline."
        );

        b1 = new StoryBlock(
            "A soft, enchanting singing drifts across the waves.",
            "Investigate", "Sail Away"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b2;
        b1.option2Block = b3;
    }
}

//-----------------------------------------
// 5. Ghost Ship Event
//-----------------------------------------
public class GhostShipEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks()
    {
        b3 = new StoryBlock(
            "You avoid the ghostly silhouette. It drifts silently past before dissolving into mist."
        );

        b2 = new StoryBlock(
            "You board the silent ship. Lanterns glow faintly, yet the decks are completely deserted."
        );

        b1 = new StoryBlock(
            "A pale ship with tattered sails glides across the water.",
            "Board It", "Ignore It"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b2;
        b1.option2Block = b3;
    }
}

//-----------------------------------------
// 6. Kraken Ripples Event
//-----------------------------------------
public class KrakenRipplesEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks()
    {
        b3 = new StoryBlock(
            "You steer clear of the bubbling section of water, keeping a safe distance."
        );

        b2 = new StoryBlock(
            "You draw near and notice massive circular markings on nearby rock formations."
        );

        b1 = new StoryBlock(
            "A rhythmic bubbling rises from beneath the waves.",
            "Approach", "Retreat"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b2;
        b1.option2Block = b3;
    }
}

//-----------------------------------------
// 7. Message in a Bottle Event
//-----------------------------------------
public class BottleEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks()
    {
        b3 = new StoryBlock(
            "You ignore the bottle and continue your course."
        );

        b2 = new StoryBlock(
            "You retrieve the bottle and find a scrap of parchment with faded markings."
        );

        b1 = new StoryBlock(
            "A corked bottle floats alongside the ship.",
            "Retrieve It", "Ignore"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b2;
        b1.option2Block = b3;
    }
}

//-----------------------------------------
// 8. Mermaid on the Rocks Event
//-----------------------------------------
public class MermaidEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3;

    protected override void CreateBlocks()
    {
        b3 = new StoryBlock(
            "You pass by at a distance. The shimmering figure slips gracefully into the water."
        );

        b2 = new StoryBlock(
            "You approach, but she disappears beneath the waves, leaving only ripples behind."
        );

        b1 = new StoryBlock(
            "A shimmering figure sits upon a rocky outcrop, singing softly.",
            "Approach", "Keep Distance"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        b1.option1Block = b2;
        b1.option2Block = b3;
    }
}


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
    new SirenSongEvent(),
    new GhostShipEvent(),
    new KrakenRipplesEvent(),
    new BottleEvent(),
    new MermaidEvent()
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
