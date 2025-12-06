# Leximon

This is the repo for the Leximon project for Group 2 in SWEI @ UML Fall 2025

## Build the Game 
The tools needed to build the game:
- Windows Operating System
- Godot Version 4.5.1
- .NET SDK
- Git bash

For the first time building the game, click on the green button called code.
Copy the https link, go to git bash then at the desired path,
type in git clone. Then paste the https linked just recently copied.

Type git fetch hit enter, then type git pull then hit enter. This should transfer 
the files for Leximon into the users folder.

Open Godot Version 4.5.1, go to import then choose the path the Leximon file. Select the folder. The build should be successful.

To run the game, enter F5.

## Play The Game

The controls of the game is as follows
- Use w,a,s,d or the arrow keys to move
- Use f to open the inventory to veiw player info and word info
- Use e when close to an enemy to engage in a battle
- Use esacpe to close the inventory, leave a battle, or open the exit menu


## Resources

- gitignore file sourced from [gitignore.io](https://www.toptal.com/developers/gitignore) under the CC0 license as outlined in the project's [git repository](https://github.com/toptal/gitignore.io)
- godot git addon <https://github.com/godotengine/godot-git-plugin> used under MIT License as outlined [here](./addons/godot-git-plugin/LICENSE)

## Image Attributions

- <https://kenney.nl/assets/1-bit-input-prompts-pixel-16> CC0
- <https://kenney.nl/assets/tiny-dungeon> CC0
- <https://www.pexels.com/photo/hallway-inside-a-dungeon-13778249/> (License)[https://www.pexels.com/license/]

## Data Sources

- Word Definitions from [Wiktionary](https://en.wiktionary.org/wiki/Wiktionary:Main_Page) under the [Attribution-ShareAlike 4.0 International License](https://creativecommons.org/licenses/by-sa/4.0/)

## References

- <https://stackoverflow.com/questions/723211/quick-way-to-create-a-list-of-values-in-c>
- <https://www.reddit.com/r/godot/comments/1ag5agj/boxcontainers_seem_to_squish_child_nodes_to/>
- <https://docs.godotengine.org/en/stable/tutorials/inputs/input_examples.html>
- <https://www.youtube.com/watch?v=3oWiAF_UbEA>
- <https://docs.godotengine.org/en/stable/classes/class_fileaccess.html>
- <https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/deserialization>
- <https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record>
- <https://learn.microsoft.com/en-us/dotnet/standard/base-types/stringbuilder>
- <https://www.reddit.com/r/godot/comments/mhnpgl/grid_container_with_scroll/>
- <https://forum.godotengine.org/t/how-do-i-fix-error-no-export-template-found-at-the-expected-path-path/1982>
- <https://docs.godotengine.org/en/stable/tutorials/io/saving_games.html>
- <https://stackoverflow.com/questions/438939/is-there-any-way-to-call-the-parent-version-of-an-overridden-method-c-net>
- <https://www.gdquest.com/library/pixel_art_setup_godot4/>
- <https://docs.godotengine.org/en/stable/tutorials/inputs/handling_quit_requests.html>
- <https://stackoverflow.com/questions/1999181/is-there-a-standard-never-returns-attribute-for-c-sharp-functions>
- <https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators>

## AI Disclosure
- [ChatGPT 5.1](https://chatgpt.com/): Code to select random moves to show to the player
  - "c#, how to efficiently get n unique random numbers in the range 0..r"
- [gpt-oss:20b](https://ollama.com/library/gpt-oss:20b): Used to generate list of words and their types (full chat [here](./AI-disclosure/wordlist-generation/chat_history.md)).
- [ChatGPT Free](https://chatgpt.com): Code to select the enemies move. Chat [here](./AI-disclosure/Enemy_Select_Random_Move/chat_history.md)
