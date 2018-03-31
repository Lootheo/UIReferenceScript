# Class Creator for Child Elements references

This script adds a function on the editor that creates a class of the selected gameobject of the scene with all the references to the ui elements in it.

## Example

A gameobject called "MenuMonster" has the next hierarchy

MenuMonster
-Canvas
--Foreground(Image)
--Background(Image)
--Hitpoints(Text)
--Mana(Text)

Selecting MenuMonster and using the script would create a class called MenuMonster that has the next variables:

Image foregroundImage;
Image backgroundImage;
Text hitpointsText; 
Text manaText;


## Usage

Put this on the Editor Folder

Select the parent gameobject that contains the UI Elements.

Go to Assets->Create C# Child References

## Future Work
This seems to be useful for other things other than UI child elements.
