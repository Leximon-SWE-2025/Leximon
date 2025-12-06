# Leximon

Leximon is a top-down turn-based battle game where players use their knowledge of synonyms and antonyms to defeat computer-controlled enemies. Players attack or defend by selecting a synonym or antonym from a list of words depending on the category of word, or type, which is assigned to the player or enemy. Using an antonym to the enemy’s type results in an effective attack, and using a synonym to the player’s type results in an effective defense.
Students may fight a few battles during their breaks at school or sit down for longer sessions of Leximon during their free time, all the while having fun as they learn synonyms and antonyms.

## Background
One of the key linguistic concepts students learn near the end of elementary school is synonyms and antonyms. These constructs can greatly enhance students’ ability to identify and define new vocabulary words. For many people it is easier to understand the meaning of a word if presented with a synonym or antonym than when presented with its definition. Synonyms are crucial for creating variation in writing and can often make the difference between boring and interesting literature.

We chose to create a game which presents students with an opportunity to practice identifying synonyms and antonyms in a fun and engaging way. We also chose the language of Pokémon-like battles for our gameplay since these games have proved popular for children (and adults), making it easy for students to understand how to play.


## Build the Game 
### Required Tools
- Godot Engine - .NET 4.5.1
- Godot Engine - .NET 4.5.1 required dependencies

### Instructions
These instructions have only been tested on Windows 11

1. Download and install the correct version of Godot Engine <https://godotengine.org/>
    1. If required download and install any other required software as listed on the download page for Godot Engine
2. Clone the repository from the desired commit (tagged commits are releases)
3. Run the Godot executable
4. Import the project in the Godot editor by selecting import from the project list and using the folder for the repository
5. Open the game
6. Goto [Run the Project](#run-the-project-in-the-godot-editor) or [Export the project](#export-the-project) as desired

### Run the Project in the Godot Editor
7. Build the project by clicking the Build Project button, located on the top right of the UI
8. Wait for the build to complete
9. Run the project by clicking the Run Project button, located directly right of the Build Project button

### Export the project
7. Ensure Export templates are installed
    1. Select: Editor -> Manage Export Templates
    2. Ensure best available mirror is selected
    3. Press Download and Install
    4. Wait for download to complete
    5. Close Export Template Manager
8. Select Project -> Export 
9. Add desired export template(s)
10. Click Export All
11. Click Release
12. The exported game will be in the folder choosen when setting up the export template(s)

Note: all files produced by export must be kept in the same directory as the executable file. 

## Download Release
The [releases section](https://github.com/Leximon-SWE-2025/Leximon/releases) of the GitHub repository has precompiled executable of the game for the Windows operating system.
To play these, download the desired release as a zip, unzip it, and run the provided exe file. 

Note: all files provided in the release must be kept in the same directory as the executable file. 

## Play The Game

The controls of the game is as follows
- Use w,a,s,d or the arrow keys to move
- Use f to open the inventory to veiw player info and word info
- Use e when close to an enemy to engage in a battle
- Use esacpe to close the inventory, leave a battle, or open the exit menu

- In the inventory, any word can be clicked on with the mouse to view its types as well as its definitions

- In the battle menu, a word can be clicked to open a menu to choose how to use it
  - The options are to attack, defend, or cancel selection
- The help text can be clicked on to view the word's definition(s)

- When either combatant runs out of health the battle is over
  - Every 3 battles won, the Player's type is randomized

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
  - Prompt: "c#, how to efficiently get n unique random numbers in the range 0..r"
- [gpt-oss:20b](https://ollama.com/library/gpt-oss:20b): Used to generate list of words and their types (full chat [here](./AI-disclosure/wordlist-generation/chat_history.md)).
- [ChatGPT Free](https://chatgpt.com): Code to select the enemies move. Chat [here](./AI-disclosure/Enemy_Select_Random_Move/chat_history.md)
- [ChatGPT Free](https://chatgpt.com): Code to create a save button. Chat [here](./AI-disclosure/save_button/save_button_chat_history.md)
