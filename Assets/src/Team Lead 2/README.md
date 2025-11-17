# cardPrefab

The Card prefab is a prefab that represents a single playable card within the game. It has a script attached that lets you click the hover over the card. As well as contains the Card class you would like it to. A card prefab is intialized and then a card class object is attached to it by either calling the associated card subclass or using the card creator. 

CardPrefab
│
├── Canvas
  ├── Artwork (Image)
  ├── TitleText (TMP Text)
  ├── CostText (TMP Text)
└── Scripts
    ├── CardObject

To Initialize a card you use the SetCard function which takes in a card object and the current gamestate as an argument.
