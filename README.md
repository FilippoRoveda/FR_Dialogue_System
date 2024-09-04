DIALOGUE SYSTEM TOOL


-Documentation-


developed by Roveda Filippo


Specialization Project
GP3
Event Horizon School
2023/2024
Index
  0. Overview
1. Download and Installation
2. Project and Folders
3. Dialogue System:
                    3.1 Window Overview
            3.2 Graph Elements
          3.3 Graph Saved Datas
4. Variable System
				4.1 Scripts and Folders
				     4.2 Editor Window
5. Languages Update
6. CSV System:
				6.1 Editor Window
				     6.2 Save .csv files
				          6.3 Modify .csv files
				               6.4 Reload in Graphs
7. Conversion System
				7.1 Editor Window
				     7.2 Results
8. Character Editor
Overview 
- 0 -

The Dialogue System presented here is a tool for the Unity Game Engine that aims to make it easy and fast to write and implement multiple-choice and multilingual dialogues for your games.
The system relies on the Experimental Graphview library of Unity Editor. The tool consists of multiple modules and submodules that will be explained step by step after the introduction to the download and setup of the project.





Download and Installation 
- 1 -

After creating an empty project within Unity, it is necessary to proceed with downloading the package that can be downloaded from the assets of the repository release GitHub at the following link:
https://github.com/FilippoRoveda/FR_Dialogue_System.git








Now drag and drop the package inside your 
project and include everything inside.








Project and Folders 
- 2 -

At this point save your project, close it and open it again so every missing folder could be created.
Now that is done your project folders overview should contain those folders.

In order:
1. CharacterSystem: editor and runtime scripts for character generation.
2. Data: in this folder runtime datas and objects created by the tool system will be stored.
3. DialogueSystem: here are contained all modules and submodules for the dialogue system, like the CSV module, Variable System module and the Conversion module.
4. Editor: in this folder, excluded from build, are contained  editor sided objects, data and     resources generated and used by the tools.                  

And you should see your toolbar updated like that.

In order:
1. Character Editor: Open character editor
2. CSV: Open CSV editor.
3. Editor Window: Open Main Dialogue System editor Window for building your dialogues.
4. Graph to Dialogue converter: Open the Converter editor for runtime objects generation from         editor side generated objects.
5. Variables Editor: Open the Variables editor.



Dialogue System
- 3 -
The Dialogue System module, as previously said, contains everything needed to create, translate and implement your dialogues. As you can see from its folder down here, inside that are contained all his modules and submodules for the tool and subtools.









You have now two ways to open the Dialogue Editor: open it from the DialogueSystem Toolbar selecting “Editor Window” or double clicking on an already existing GraphSO asset in the project saved in the “Editor” folder.


Window Overview 
- 3.1 -


The first case will open you the DS_Main_Editor_Window that is ready to let you operate in a blank space.
Tha will be the possible option gave, in order:
1. File Name: this field lets you modify the current graph name, saving that after changing it will change the name of the generated GraphSO scriptable object file. By default opening the editor it will be set to “DialogueFileName”.
2. Save: save the current graph, override it if it was already existing.
3. Load: load in the graph view another GraphSO scriptable object losing everything contained in the current graph view if not saved. Clicking on the Load option the system will open another window in which you will be able to choose which one to open from the Graphs folder inside the editor side datas section.
4. Clear: empty the graph view from every contained element.
5. Reset: restore the last saved state of the current viewed graphSO.
6. Variable Editor: open the variable editor window (We will see that later).
7. Toggle Minimap: open the graph view minimap.
8. Language: switch the graphview displayed language contents based on every implemented language in the specific script(We will see that later).

Otherwise double clicking on an already existing and saved GraphSO scriptable object you will get his specific asset window with a slightly different options toolbar. 

As you can see you will have fewer options. In the next versions of the tool that could be changed, letting you have dialogue templates.

You can have only one main Dialogue System Editor window but multiple specific asset editor windows, hypothetically one for every asset you have, letting you have a wider view of your dialogues.

What can you do in the Graph View?

Inside the graph view you can operate multiple actions like creating, moving and deleting elements, nodes, groups, select multiple elements and move them in group, zoom in and zoom out from the view. Also there are 2 Context menus that let you create the available elements and operate other operations, one can be opened Right Clicking on a specific point of the grid and instantiating there the selected element, the other context menu could be opened pressing space while the graph view is selected.
Right click context menù.
















Space pressed context menù.


Graph Elements 
- 3.2 -




Let's introduce every element you can build and link while creating your dialogues. You will be able to link them from their Pins creating the flow of the conversation, add  Conditions and Events to change the course of it. Changing Language you will be able to edit every text for every available language type.

Edges and Pins
In this system to link nodes you create Edges between Pins. Nodes usually have both Input Pins and Output Pins, exceptions are made for start nodes and end nodes, the first of course has no input pin that links him to preview nodes and the second one has no output pin to link him to other nodes. Dragging an input pin to an output one will create an edge, and vice versa.
Remember, In this system many dialogues could link one of them output pins to the same input pin but an output pin can link itself to only one input pin. Conditions to the choices are allowed too.
Conditions











Conditions lay themself to the Global Dialogue Variable System of which we will talk more later. You can make conditions on those variables and decide which will be the operation made on them, such as identity comparison with a value, or a Higher or Lower condition check. Condition affect the flow of the dialogue in 2 ways:
Choice conditions: decide if the choice will be available or not, if only one of the conditions is not met the choice condition will be hidden.
Branch conditions: change the flow of the dialogue between the TRUE linked node and the FALSE one.

Game Events
Even though they are not a visual element of the Graphview, Game Events are primary elements of what is the construction of a dialogue and are directly included and implemented in the Dialogue System Tool even at this stage. Let's now see their dedicated folder and how they appear in the inspector.

















The inspector simply displays the string dedicated to this event. The string being a basic and common data to all systems is the best generic way to encapsulate information about an event.
The user will be able to extend the class if he wants and build the Event Manager and Event Reader able to bring to the desired implementation.



Start Node



The Start Node is the starting node of every dialogue, you can edit its name and the text of its single choice pin in which you can add conditions.

Single Node



Very similar to the previous one, it only has the input connection pin since only the start node could be the first node of a dialogue.

Multiple Node

















Share the same feature with the single node, the specific addition is the possibility to have multiple outcome output pins with their conditions.
Branch Node
The branche node has two fixed and outcome pins, TRUE and FALSE. In those pins it is not possible to add any additional condition. The node also contains a condition container in which you can add as many variable conditions as you like. Every one of them will be checked and if only one result is false the entire node will be false.

Event Node

















In the event node you can add events to your dialogue progression. There Are two types of events in this tool: the Game Events and the Variable Events. Game Events are generic events that hold a string representing itself, this is the most generic type of data possible to suit every base implementation of a game, the user has the possibility to extend this class based on his needs. The other ones are Variable Events, those events are linked to the Global Dialogue Variables created by the Variable System and they can be set and modified. There is no limit to the number of events you can add to the node, for both of the types.

End Node












The end node is the final node for every dialogue, a dialogue can also have multiple possibilities for its ending, so in your ramifications you can add multiple end nodes, one for each end you like to reach. It contains his text and a boolean parameter called “IsRepeatable” that lets the system know if the dialogue just ended could be repeated or not.

Group


A group is mainly a estetic container that helps you build more precise and clear dialogue graphs, you can drag and drop nodes inside it. Every node saved and converted will now have a field in which is recorded to which group they belong.



Graph Saved Datas
- 3.3 -

Once you are satisfied with the result of your graph, you can save it using the “Save” button on the toolbar in the editor window. This will call the system used to convert the data that is currently saved in the graph view and convert it into a scriptable object of type GraphSO that will receive the name specified previously.









Here is the “Graphs” save folder inside the Data folder in the Editor section and an example of GraphSO inside it. Let’s take a closer look at the inspector for this object.




As you can see, it contains a set of lists representing the data, grouped by type, of the elements that were inside the graph view at the time of saving. Now we will move on to an illustration of the information contained in the various types of data.
Group dates




Name: name of the group.
Id: ID unique for the group.
Position: spatial position where the group was at the time of the rescue.

Dialogue nodes data
In this system, dialogue nodes mean the set of Start Nodes, Single Nodes and Multiple Nodes.

Start Node Data

Name: name of the node.
Node ID: unique ID of the node.
Group ID: unique ID of the group to which it belongs (if present).

Node Type: type of node.
Position: position of the node at the time of the last save.

Texts: textual information of the node, each element of the list contains the peculiar text for one of the various languages ​​​​ currently implemented and a reference to which language it is.

Choices: list of dialogue choices present for this node, limited to one for the node in question.

	







Choice:
Choice ID: unique ID identifying the choice.
Next Node ID: unique ID referring to the node to which the choice will lead.
Conditions: list of Variable Conditions related to the Choice in question.


Single Node Data





It presents the same information as the previous node with the only change being the change in node type.







Multiple Node Data










The only addition here is the ability to have more items in the Choices list.














Event Node Data





Here the Game Events list field and the Variable Events field are added, a container that holds the lists of Variable Events divided by type.
















Branch Node Data









In the Branch node “Conditions” is added a condition container containing the variable conditions that will be verified. Furthermore the only two fixed elements in the “Choices” list are the two choices TRUE and FALSE.


End Node Data







Here the boolean field “IsDialogueRepetable” is added.



