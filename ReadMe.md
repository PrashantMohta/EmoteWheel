# EmoteWheel

A mod for the game Hollow Knight that allows players to send emotes over HkmpPouch.

## Creating your own Emotes

All Emotes are [Satchel.Animation](https://github.com/PrashantMohta/Satchel/blob/master/Animation/Animation.cs) files, where the name of the JSON file is the name of the emote, you can have each emote in any sub-directory structure for organization purposes but the paths in the JSON must be a relative path to the image file.

Here's an example [Satchel.Animation](https://github.com/PrashantMohta/Satchel/blob/master/Animation/Animation.cs)

```json
{
	"frames": ["1.png"],
	"fps": 1,
	"loop": true
}
```

This file will load an image `1.png` as a static image.

Example directory structure : 
![image](https://github.com/user-attachments/assets/1cd49b87-9c68-41d3-8b37-23163b6e2ed6)

This will add an emote by the name of angelsus to your game.
