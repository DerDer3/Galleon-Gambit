using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

class StoryBlock{
    public string story;
    public string option1Text;
    public string option2Text;
    public StoryBlock option1Block;
    public StoryBlock option2Block;

    public StoryBlock(string story, string option1Text = "", string option2Text = "", StoryBlock option1Block = null, StoryBlock option2Block = null){
        
        this.story = story; 
        this.option1Text = option1Text;
        this.option2Text = option2Text;
        this.option1Block = option1Block;
        this.option2Block = option2Block;

    }
}

public class GameManager : MonoBehaviour
{
    public Text mainText;
    public Button option1;
    public Button option2;

    StoryBlock currentBlock;

    static StoryBlock block3 = new StoryBlock("You avoid the lighthouse and find as you make your way around it that a monster waits to ambush you. Avoiding it fills you with determination. Heal 2 damage.");
    static StoryBlock block2 = new StoryBlock("A monster appears from the shadows and gives chase to the ship. Take 2 damage.");
    static StoryBlock block1 = new StoryBlock("A lighthouse shows in the distance of the foggy waters.", "Venture Forth", "Go Around It", block2, block3);
    
    void Start()
    {
        //mainText.text = "Welcome to the game!";
        //option1.GetComponentInChildren<Text>().text = "Hello";
        //option2.GetComponentInChildren<Text>().text = "Goodbye";

        DisplayBlock(block1);
    }

    void DisplayBlock(StoryBlock block){
        mainText.text = block.story;
        option1.GetComponentInChildren<Text>().text = block.option1Text;
        option2.GetComponentInChildren<Text>().text = block.option2Text;

        currentBlock = block;

         // Check if the current block is block2 or block3
    if (block == block2 || block == block3)
    {
        option1.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
    }
    else
    {
        option1.gameObject.SetActive(true);
        option2.gameObject.SetActive(true);
    }
    }

    public void Button1Clicked(){
        DisplayBlock(currentBlock.option1Block);    
    }

    public void Button2Clicked(){
        DisplayBlock(currentBlock.option2Block);    
    }
}