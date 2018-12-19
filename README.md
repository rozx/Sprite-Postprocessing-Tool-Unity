# Sprite-Postprocessing-Tool

This unity post processing plugin that let's you create different effects for the unity UI image or Sprite Renderer component. 

# How to use it?

Make sure that under the **import settings** for the texture, in **Advanced**, the **Read/Write Enable** option is enabled. Then attach the ```SpritePostprocessing.cs``` script to any gameObjects that have UI Image or Sprite Renderer component attached.

The ```SpritePostprocessing.cs``` will automatic find the source (which is the UI Image or Sprite Renderer component) and apply the effect to it.

## Options

- Ignored Color: the pixels that have the silmilar color will be ignored and will not have effect applied to it.
- Ignore Threshold: determines how two colors are silmilar.
- Color muliplier: determines the color saturation.
- Tint color: the tint color, only applies when post processing method "tint" was selected.

## Note

When changed the value, call ```ApplyTextureChanges()``` to apply the changes.
Also, enable/ disable the component will set/reset the post processing effect of sprite.

# Screenshots

- Editor View

![Editor View](https://raw.githubusercontent.com/rozx/UnityModularSystem/master/Screenshots/editor.PNG)

- Example Preview

![Example](https://raw.githubusercontent.com/rozx/UnityModularSystem/master/Screenshots/preview.PNG)
