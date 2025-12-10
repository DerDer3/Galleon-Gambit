using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;



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
    StoryBlock b1, b2, b3, b4, b5;

    protected override void CreateBlocks()
    {
        // Final outcomes
        b5 = new StoryBlock(
            "You safely navigate past the lighthouse. The crew feels a renewed sense of purpose."
        );

        b4 = new StoryBlock(
            "You investigate the abandoned lighthouse further and find a hidden supply cache."
        );

        // Intermediate outcomes
        b3 = new StoryBlock(
            "You draw closer and discover the lighthouse abandoned, its beacon flickering unevenly.",
            "Explore Inside", "Sail Around"
        );

        b2 = new StoryBlock(
            "You steer wide of the lighthouse. As you pass, it fills you & the crew with determination.",
            "Keep Course", "Turn Back"
        );

        // Root
        b1 = new StoryBlock(
            "A looming lighthouse emerges through the heavy fog.",
            "Approach the Light", "Sail Around It"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        // First choice
        b1.option1Block = b3; // Approach the Light
        b1.option2Block = b2; // Sail Around It

        // Second choices
        b3.option1Block = b4; // Explore Inside
        b3.option2Block = b5; // Sail Around

        b2.option1Block = b5; // Keep Course
        b2.option2Block = b4; // Turn Back
    }
}

//-----------------------------------------
// 2. Storm Event
//-----------------------------------------
public class StormEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3, b4, b5;

    protected override void CreateBlocks()
    {
        // Final outcomes
        b5 = new StoryBlock(
            "The storm passes and you reach calmer waters. The crew feels relieved."
        );

        b4 = new StoryBlock(
            "You find shelter behind a rocky outcrop, letting the storm pass safely."
        );

        // Intermediate outcomes
        b3 = new StoryBlock(
            "You anchor and wait. The crew shares stories as the storm slowly quiets.",
            "Wait Longer", "Set Sail Early"
        );

        b2 = new StoryBlock(
            "You press on. The winds batter the ship, but eventually the clouds begin to part.",
            "Push Forward", "Seek Shelter"
        );

        // Root block
        b1 = new StoryBlock(
            "Dark storm clouds gather above.",
            "Sail Through", "Drop Anchor"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        // First choices
        b1.option1Block = b2; // Sail Through
        b1.option2Block = b3; // Drop Anchor

        // Second choices
        b2.option1Block = b5; // Push Forward → calm waters
        b2.option2Block = b4; // Seek Shelter → behind rocks

        b3.option1Block = b4; // Wait Longer → find shelter
        b3.option2Block = b5; // Set Sail Early → calm waters
    }
}

//-----------------------------------------
// 3. Bad Food Event
//-----------------------------------------
public class BadFoodEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3, b4, b5;

    protected override void CreateBlocks()
    {
        // Final outcomes
        b5 = new StoryBlock(
            "You enjoy a surprisingly tasty meal. The crew nods approvingly."
        );

        b4 = new StoryBlock(
            "You cautiously try a small bite. The crew watches, impressed by your prudence."
        );

        // Intermediate outcomes
        b3 = new StoryBlock(
            "You eat the questionable meal. The crew watches, unsure of your decision.",
            "Take Another Bite", "Stop Eating"
        );

        b2 = new StoryBlock(
            "You skip the meal and stay alert as others dine.",
            "Change Your Mind", "Stick to Your Decision"
        );

        // Root
        b1 = new StoryBlock(
            "The cook offers you a meal of… uncertain origin.",
            "Eat It", "Pass"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        // First choices
        b1.option1Block = b3; // Eat It
        b1.option2Block = b2; // Pass

        // Second choices
        b3.option1Block = b5; // Take Another Bite → tasty meal
        b3.option2Block = b4; // Stop Eating → cautious bite

        b2.option1Block = b4; // Change Your Mind → cautious bite
        b2.option2Block = b5; // Stick to Your Decision → tasty meal
    }
}

//-----------------------------------------
// 4. Siren Song Event
//-----------------------------------------
public class SirenSongEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3, b4, b5;

    protected override void CreateBlocks()
    {
        // Final outcomes
        b5 = new StoryBlock(
            "You carefully follow the patterns of stones and uncover a hidden treasure washed ashore."
        );

        b4 = new StoryBlock(
            "You ignore the mysterious patterns and continue sailing. The crew feels a mix of relief and curiosity."
        );

        // Intermediate outcomes
        b3 = new StoryBlock(
            "You turn away from the singing, letting the eerie melody fade behind you.",
            "Double Back", "Keep Sailing"
        );

        b2 = new StoryBlock(
            "You move toward the sound and discover smooth stones arranged in strange patterns on the shoreline.",
            "Examine Stones", "Move On"
        );

        // Root
        b1 = new StoryBlock(
            "A soft, enchanting singing drifts across the waves.",
            "Investigate", "Sail Away"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        // First choices
        b1.option1Block = b2; // Investigate
        b1.option2Block = b3; // Sail Away

        // Second choices
        b2.option1Block = b5; // Examine Stones → find treasure
        b2.option2Block = b4; // Move On → continue sailing

        b3.option1Block = b5; // Double Back → find treasure
        b3.option2Block = b4; // Keep Sailing → continue sailing
    }
}

//-----------------------------------------
// 5. Ghost Ship Event
//-----------------------------------------
public class GhostShipEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3, b4, b5;

    protected override void CreateBlocks()
    {
        // Final outcomes
        b5 = new StoryBlock(
            "You explore the ship carefully and discover a hidden cache of old maps and supplies."
        );

        b4 = new StoryBlock(
            "You choose to sail past the ghost ship. The crew remains uneasy, but the ship fades into the mist."
        );

        // Intermediate outcomes
        b3 = new StoryBlock(
            "You avoid the ghostly silhouette. It drifts silently past before dissolving into mist.",
            "Double Back", "Keep Sailing"
        );

        b2 = new StoryBlock(
            "You board the silent ship. Lanterns glow faintly, yet the decks are completely deserted.",
            "Search Thoroughly", "Leave Quickly"
        );

        // Root
        b1 = new StoryBlock(
            "A pale ship with tattered sails glides across the water.",
            "Board It", "Ignore It"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        // First choices
        b1.option1Block = b2; // Board It
        b1.option2Block = b3; // Ignore It

        // Second choices
        b2.option1Block = b5; // Search Thoroughly → find maps and supplies
        b2.option2Block = b4; // Leave Quickly → sail past

        b3.option1Block = b5; // Double Back → discover maps and supplies
        b3.option2Block = b4; // Keep Sailing → sail past
    }
}

//-----------------------------------------
// 6. Kraken Ripples Event
//-----------------------------------------
public class KrakenRipplesEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3, b4, b5;

    protected override void CreateBlocks()
    {
        // Final outcomes
        b5 = new StoryBlock(
            "You investigate the markings closely and discover they reveal an underwater cave entrance."
        );

        b4 = new StoryBlock(
            "You decide to stay clear of the bubbling water. The crew feels safer but remains curious about what was hidden."
        );

        // Intermediate outcomes
        b3 = new StoryBlock(
            "You steer clear of the bubbling section of water, keeping a safe distance.",
            "Circle Back", "Keep Distance"
        );

        b2 = new StoryBlock(
            "You draw near and notice massive circular markings on nearby rock formations.",
            "Dive In", "Observe from Afar"
        );

        // Root
        b1 = new StoryBlock(
            "A rhythmic bubbling rises from beneath the waves.",
            "Approach", "Retreat"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        // First choices
        b1.option1Block = b2; // Approach
        b1.option2Block = b3; // Retreat

        // Second choices
        b2.option1Block = b5; // Dive In → underwater cave
        b2.option2Block = b4; // Observe from Afar → stay safe

        b3.option1Block = b5; // Circle Back → underwater cave
        b3.option2Block = b4; // Keep Distance → stay safe
    }
}

//-----------------------------------------
// 7. Message in a Bottle Event
//-----------------------------------------
public class BottleEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3, b4, b5;

    protected override void CreateBlocks()
    {
        // Final outcomes
        b5 = new StoryBlock(
            "You carefully decipher the parchment and uncover a hidden message revealing a nearby island with treasure."
        );

        b4 = new StoryBlock(
            "You put the bottle aside and continue sailing. The crew remains curious but focused on the voyage."
        );

        // Intermediate outcomes
        b3 = new StoryBlock(
            "You ignore the bottle and continue your course.",
            "Go Back for It", "Stay On Course"
        );

        b2 = new StoryBlock(
            "You retrieve the bottle and find a scrap of parchment with faded markings.",
            "Decipher Parchment", "Set It Aside"
        );

        // Root
        b1 = new StoryBlock(
            "A corked bottle floats alongside the ship.",
            "Retrieve It", "Ignore"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        // First choices
        b1.option1Block = b2; // Retrieve It
        b1.option2Block = b3; // Ignore

        // Second choices
        b2.option1Block = b5; // Decipher Parchment → find treasure
        b2.option2Block = b4; // Set It Aside → continue sailing

        b3.option1Block = b5; // Go Back for It → find treasure
        b3.option2Block = b4; // Stay On Course → continue sailing
    }
}

//-----------------------------------------
// 8. Mermaid on the Rocks Event
//-----------------------------------------
public class MermaidEvent : StoryEventTemplate
{
    StoryBlock b1, b2, b3, b4, b5;

    protected override void CreateBlocks()
    {
        // Final outcomes
        b5 = new StoryBlock(
            "You manage to communicate with the mermaid and she gifts the crew a magical pearl."
        );

        b4 = new StoryBlock(
            "You sail past the mermaid. The crew remains intrigued but continues their journey safely."
        );

        // Intermediate outcomes
        b3 = new StoryBlock(
            "You pass by at a distance. The shimmering figure slips gracefully into the water.",
            "Turn Back", "Keep Distance"
        );

        b2 = new StoryBlock(
            "You approach, but she disappears beneath the waves, leaving only ripples behind.",
            "Dive In", "Observe from Afar"
        );

        // Root
        b1 = new StoryBlock(
            "A shimmering figure sits upon a rocky outcrop, singing softly.",
            "Approach", "Keep Distance"
        );

        root = b1;
    }

    protected override void LinkBlocks()
    {
        // First choices
        b1.option1Block = b2; // Approach
        b1.option2Block = b3; // Keep Distance

        // Second choices
        b2.option1Block = b5; // Dive In → magical pearl
        b2.option2Block = b4; // Observe from Afar → continue journey

        b3.option1Block = b5; // Turn Back → magical pearl
        b3.option2Block = b4; // Keep Distance → continue journey
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

    public UnityEvent Completed;

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

    public void StartRandomEvent()
    {
        int index = Random.Range(0, events.Count);
        StoryBlock start = events[index].BuildEvent();
        DisplayBlock(start);
    }

    void DisplayBlock(StoryBlock block)
    {
        currentBlock = block;
        mainText.text = block.story;

        // Show buttons only if text exists
        bool option1Visible = !string.IsNullOrEmpty(block.option1Text);
        bool option2Visible = !string.IsNullOrEmpty(block.option2Text);

        option1.gameObject.SetActive(option1Visible);
        option2.gameObject.SetActive(option2Visible);

        if (option1Visible)
            option1.GetComponentInChildren<Text>().text = block.option1Text;
        if (option2Visible)
            option2.GetComponentInChildren<Text>().text = block.option2Text;

        // If no options, this is the end of the event
        if (!option1Visible && !option2Visible)
            StartCoroutine(OnCompleted());
    }

    public void Button1Clicked()
    {
        if (currentBlock.option1Block != null)
            DisplayBlock(currentBlock.option1Block);
        else
            StartCoroutine(OnCompleted()); // end of story
    }

    public void Button2Clicked()
    {
        if (currentBlock.option2Block != null)
            DisplayBlock(currentBlock.option2Block);
        else
            StartCoroutine(OnCompleted()); // end of story
    }

    IEnumerator OnCompleted()
    {
        // Optional: pause to let the last message show
        yield return new WaitForSeconds(2f);
        Completed.Invoke();
    }
}










