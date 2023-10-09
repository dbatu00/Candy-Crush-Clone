# Candy-Crush-Clone
Row Match Game w/ level unlocking and online downloads

!! Most of the code is here https://github.com/dbatu00/Candy-Crush-Clone/blob/main/Assets/Scripts/ !!

A simple, level-based, mobile game called “Row Match”.
Implementation done in Unity and C#.

Flow:
- When the user launches the game, s/he will see a LevelsButton in the MainScene.
- After tapping the button, LevelsPopup should appear and list all the available levels.
- Each row (which represents the level) should show this information
- The level number and moves count
- Highest score of the level
- Green “Play” button to enter the LevelScene (if the level is playable) or Gray
“Locked” button which shows that previous levels should be played first.
- After tapping the PlayButton, LevelScene should be loaded according to the level. (You
can find gameplay details in the Gameplay section)
- When a user finishes the level with a new highest score
- MainScene should be loaded
- Celebration particles and animation should be shown to the user
- After the celebration, LevelsPopup appears automatically
- After the popup, the unlocked level should be displayed as the button changed from
a gray locked button to a green play button.
- When a user finishes the level without a new highest score
- MainScene should be loaded
- And LevelsPopup appears automatically
- Playable in the editor

Gameplay:
- The goal in Row Match is to create the maximum number of rows that have the same type of game
items with limited moves.

Mechanics:
- There are four types (colors) of items.
- There is a rectangular grid, where each cell can have one item. The grid can have a width
and height of four to nine cells.
- Any adjacent item in a grid can be swapped by swiping.
- There is a predefined limited number of moves for each level. A move is spent after a swipe
operation. The user should see the count of remaining moves in the scene.
- When all cells in a row have the same type of item, the row gets completed automatically
and the user earns a score for each completed item.
- Red: 100 - Green: 150 - Blue: 200 - Yellow: 250 points. The user should see the current
score and highest score in the scene.
- No swap operation can be performed with a completed cell.
- If there is no possible row match to gain more score, the game should automatically end
without requiring the user to finish the remaining moves.


Levels:
- Row Match can be playable online or offline. The first 10 levels should be included in the project to
make them available to users for initial launch even without internet connection. As soon as
possible, rest of the levels should be downloaded once from the given URLs and should be saved
to persistent storage to be available for later launches.
The level file structure is:
- level_number: number of the level
- grid_width: width of the grid
- grid_height: height of the grid
- move_count: given move count for the level
- grid: a list of colors (r: Red, g: Green, b: Blue, y: Yellow) starts from the bottom left of the
grid, ends at the top right of the grid.
