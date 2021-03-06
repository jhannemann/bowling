# How to Make a Bowling Game

This tutorial will walk you through the creation of a simple bowling Game
using the Unity 3D game engine.

## Create a New 3D project

Create a new Unity project and name it *Bowling*.
Make sure that the 3D button is selected to create a 3D game.
This should be the default.
Pick a suitable location (e.g. the desktop),
and click the *Create Project* button.

## Save the Scene

You are presented with an empty game world,
which contains a single directional light source and a default main camera.
This scene is named *untitled*.
Name and save the scene by selecting `File->Save Scene as...`
and name the scene "Scene 1".
Your Assets pane at the bottom of the screen should now contain your saved scene.

## Create the Bowling Lane and the Ball

### The Lane

The lane will be made from the *plane* game object type in Unity.
Create it by selecting `GameObject->3D Object->Plane` from the menu.
Its center is located at the center of our game world,
indicated by the position `(0,0,0)` in the *Transform* component.
Rename the object to "Lane".

The default color is not very appealing.
Color is only one aspect of a game object's material properties.
To change it,
we need to create a new material.
From the menu,
select `Assets->Create->Material` and name the new material "Lane Material".
When you click on it in the Assets pane,
you can set the properties in the Inspector pane on the right.
Set the *Albedo* by clicking on the color picker.
The R, G, and B components should be set to `(255, 215, 0)`,
resulting in a darkish yellow.
To make the lane shiny,
select a *Metallic* value of 0 and a *Smoothness* of 0.6.
Attach the material to the lane by dragging the material from the Assets pane onto the Lane object in the Scene pane.

### The Ball

The bowling ball is a sphere.
Create one by selecting `GameObject->3D Object->Sphere`.
Rename the object to "Ball".
You will note that by default the ball is embedded in our lane
as it shares the same center point.
We can fix this by setting the *y* component of its *Transform* to 1.
This will make the ball float in mid-air.

The ball material will be a shiny black.
Once again create a new material named "Ball Material".
Set the color to black `(0, 0, 0)`,
and choose a *Metallic* value of 0 and a *Smoothness* of 0.8.
Attach the material to the ball just like with the lane.

### Playing the Scene

When we play the scene by hitting the *Play* button at the center top,
the ball keeps being suspended in mid-air.
This is because it is not subject to gravity,
which is handled by Unity's *physics engine*.

In order to make a game object subject to the physics simulation,
it needs a *RigidBody* component.
Add it to the ball by clicking on the ball object in the scene and then select `Add Component->Physics->Rigidbody`.

Playing the scene again,
the ball will now fall to the ground and come to rest on the lane.

### Save the Scene

At a major point like this,
it is always a good idea to save your scene via `File->Save Scenes`.

## Make the Scene More Realistic

A bowling lane is not square,
so let's fix that.
We can use the *Scale* part of the lane's transform component for that.
Set the *Scale* to `(0.1, 0, 2)`.
This corresponds to a width of 1 meters and a lenght of 20 meters,
which is pretty close to the size of a standard bowling lane.

The ball needs to be at the start of the lane.
Set the ball transform's position to `(0, 0.1, -10)`.
The ball is also too large.
Set its scale to `(0.2, 0.2, 0.2)`,
resulting in a fairly standard diameter of 20 cm.

The camera now is too close to the edge of the lane and we cannot see the ball.
Move it back a bit by setting the z-component of its position to -12.

Save the scene.

## Shoot the Ball

A static scene is pretty boring.
Let's shoot the ball along the lane.
To accomplish this,
we will write our first couple of lines of code in a programming language called C#.

### Creating Our First Script

We need to add a C# script to our ball.
Select the ball in the Scene pane and in the inspector select `Add Component->New Script`.
Name the script the same as the object,
i.e. "Ball"
(spelling and capitalization must *exactly* match the object name).
Make sure the language selected is "C Sharp".

Once created,
the script shows up in the Assets pane.
Double-click on it to edit it.

The new script should look like this:

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
```

We will add a public attribute named `speed` and a private one named `rigidBody`.
In the `Start ()` method,
we will retrieve the ball's Rigidbody component.
We will the use it in the `Update ()` method to set the ball's speed when the user presses the space bar.

The resulting code looks like this:

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public float speed;
	private Rigidbody rigidBody;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			rigidBody.velocity = new Vector3 (0, 0, speed);
		}
	}
}
```

After saving the script,
the Ball inspector will show the speed as zero.
In the editor, set the speed to 10 m/s.
Playing the scene,
the ball will now shoot down the lane,
decelerating quickly.

You will note that the ball does not exactly behave like a real bowling ball.
In part this is because,
at the default mass of 1 kg,
it is too light.
We will change that later.
On top of that,
a real bowling lane has a very peculiar physical behavior because there is a coat of oil on top of it.
This is not reflected by our current physics engine settings,
and we will ignore this for this simple project.

### Make the Ball into a Prefab

This is not strictly needed at this point,
but in a real bowling game,
we want to create many balls,
most of them sharing the same properties.
The same will hold true for our pins,
where it will prove very useful.

Unity allows for easy replication of objects that share the same properties via a concept called *Prefabs*.
To create a prefab,
simply drag it from the Scene pane onto the Assets pane.
Any changes to the prefab in the Assets pane will immediately be reflected in all connected prefabs in the Scene pane.

Save the scene before taking the next step.

## Add the Head Pin

Our first pin is a simple cylinder.
Create it by selecting `GameObject->3D Object->Cylinder`.
Set its transform's position to `(0, 0.19, 0)`
and its scale to `(0.06, 0.19, 0.06)`.

Create a new material named "Pin Material",
and set its *Albedo* to white `(255, 255, 255)`,
and its *Metallic* component to 0.25 and its *Smoothness* component to 0.8.

When you play back the scene and shoot the ball,
you will notice that the ball hits the pin but does not topple it.
This is because,
as previously mentioned,
by default the game object is not subject to physics unless it has a Rigidbody component.
Add it just as you did for the ball and stick to the default values for now.

Playing the scene now,
the ball will hit and topple the pin.

Save the scene.

## Add More Pins and Add a Backstop

Eventually,
we will have 10 pins.

### Create an Empty Game Object

All those pins will be in positions defined relative to the head pin.
This is why Unity allows for hierarchies of game objects,
where objects can be placed in relation to a parent object.
The parent object can be an empty placeholder,
which is exactly what we will do now.
Create an empty game object named "Pins" via `GameObject->Create Empty`.
Then drag the "Pin" object in the Scene pane on top of the "Pins" object,
which makes it a child of the (now not empty any more) "Pins" object.

### Make Pin into a Prefab

Make the head pin into a prefab by dragging it from the Scene pane into the Assets pane.

### Create More Pins

With the prefab in place,
we can now create more copies of the head pin.
Right-click on the head pin in the Scene pane.
Then,
copy and paste the pin nine times,
creating pins named "Pin (1)" through "Pin (9)".
Set the following positions:

| Pin Number |         Position        |
|:----------:|:-----------------------:|
|    (1)     | `( 0.15 , 0.19 , 0.26)` |
|    (2)     | `(-0.15 , 0.19 , 0.26)` |
|    (3)     | `( 0.0  , 0.19 , 0.52)` |
|    (4)     | `(-0.3  , 0.19 , 0.52)` |
|    (5)     | `( 0.3  , 0.19 , 0.52)` |
|    (6)     | `( 0.15 , 0.19 , 0.78)` |
|    (7)     | `(-0.15 , 0.19 , 0.78)` |
|    (8)     | `(-0.45 , 0.19 , 0.78)` |
|    (9)     | `( 0.45 , 0.19 , 0.78)` |

### Change the Pin Scale on the Prefab

We will now see the power of prefabs.
The pins are actually too thin for real bowling pins.
Actual bowling pins are about 12 cm in diameter,
not the 6 cm we used originally.

To change all pin diameters at the same time,
select the pin prefab in the Asset pane and set its scale to `(0.12, 0.19, 0.12)`.
Note that all connected pins in the scene change their size in unison.

### Adding a Backstop

Play the scene.
You will notice that the ball barely topples the first pin.
However,
after the ball has been shot,
we can press the space bar again to give it another speed boost.
That way,
we can topple more pins and even shoot the ball off the lane.
We will fix the former problem later,
but to prevent the ball from going past the pins too far,
we will add a backstop.

Create a cube game object by selecting `GameObject->3D Object->Cube`.
Name it "Backstop" and make it a child of the "Pins" object.

Set its transform position to `(0, 0.25, 2)`
and its scale to `(1, 0.5, 0.1)`.
Since we want the ball to hit it and stop rather than bounce off of it,
do *not* add a Rigidbody component to it to disable physics simulation.

Save the scene.

## Make the Physics More Realistic

Right now,
the mass of all Rigidbody objects is 1 kg.
In real life,
a bowling ball has a mass of about 7 kg,
and a bowling pin has a mass of about 1.5 kg.
Change the mass in the Rigidbody component of the Ball and Pin **prefabs**.

At 10 m/s,
the original ball speed is realistic but too low for the lane's current physical properties.
A good speed for our purpose is 13 m/s.
Change the speed in the Ball prefab's script component to 13 m/s.

Playing the scene now,
you should see a much improved behavior of the ball and the pins.

Save the scene.

## Aim and Throw the Ball

So far,
the player could only shoot,
not aim the ball.
Also, the player could add speed to the ball by repeatedly pressing the space bar even after the ball had been thrown.
Let's fix this.

### Throw the Ball

To prevent manipulation of the ball after it has been thrown,
let's introduce a Boolean variable `thrown` to the Ball script.
Initially,
it is false.
When the ball has been thrown by pressing the space bar,
we will set it to true.
From that point on, we will ignore additional space bar presses.

Open the `Ball` C# script by double-clicking it.
Just below line 8,
add the line

```C#
private bool thrown = false;
```

Change the `Update ()` method to:

```C#
void Update () {
	if (!thrown && Input.GetKeyDown (KeyCode.Space)) {
		thrown = true;
		rigidBody.velocity = new Vector3 (0, 0, speed);
	}
}
```

Playing the game,
it should now be impossible to increase the ball speed by hitting the space bar after the ball has been thrown.

### Aim the Ball

In order to aim the ball,
we want to use the arrow keys on the keyboard to move the ball to the left and right before throwing it.
Alternatively,
gamers are used to use the `A` and `D` keys for the same operation.
Players using a game controller would like to use their controller's joystick.
Unity provides a unified way to get input from a variety of controllers via the `Input.GetAxis ()` method.
The "Horizontal" axis corresponds to the horizontal movement of a game controller's joystick,
or, by default,
the keyboards left and right cursor keys,
the numeric keypads `4` and `6` keys,
or the `A` and `D` keys,
all at the same time.
The mapping can then be changed in the game's preferences.

#### Kinematics

Manipulation of a game object via `GetAxis ()` can happen in two different ways.
If the manipulation is handled by the physics engine,
we can apply a force to the object,
and its acceleration will then be determined via Newton's first law:

F = ma.

This makes aiming the ball quite difficult,
though,
as the ball would not come to a rest until we apply an additional force to counter its inertia to eventually make it stop.

A more intuitive and easier way is to simply move the object proportional to its axis input and make the object stop when there is no input.

This mode of operation,
where an objects position is directly manipulated independently of the pysics engine is called *kinematic*.

We will therefore set the Ball to kinematic by default by checking the corresponding box in its prefab's Rigidbody component.

Then, we will modify the Ball's C# script like this:

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public float speed = 10;
	private Rigidbody rigidBody;
	private bool thrown = false;
	public float horizontalSpeed = 0.1f;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		if (!thrown) {
			float xAxis = Input.GetAxis ("Horizontal");
			Vector3 position = transform.position;
			position.x += xAxis*horizontalSpeed;
			transform.position = position;
		}
		if (!thrown && Input.GetKeyDown (KeyCode.Space)) {
			thrown = true;
			rigidBody.isKinematic = false;
			rigidBody.velocity = new Vector3 (0, 0, speed);
		}
	}
}
```

As you see,
we have introduced a new attribute `horizontalSpeed` which determines how fast we move the ball on horizontal axis input.

Since the Ball prefab now is kinematic by default,
the ball needs to be made subject to the physics engine when thrown.
This is accomplished by the `ridigbody.isKinematic = false;` line just after the space bar has been pressed.

Also, of course,
we only need to get axis input until the ball has been thrown.

You will also note that we can easily move the ball off the lane.
It is fairly simple to write code to keep the ball inside the lane at all times,
but for simplicity's sake,
we will not do that here.

Save your scene.

## Make the Camera Follow the Ball

Playing the game,
it is hard to see how many pins we have knocked down,
because the camera is so far away from the pins.

An easy way to fix this is to have the camera follow the ball at a safe distance,
showing the toppling of the pins in all its gory detail.
This requires another script,
this time attached to the main camera.

In the scene pane,
click on the Main Camera,
and in the inspector,
click `Add Component->New Script`.
Name it "MainCamera".

Edit the script by double-clicking on it.
We will need three public attributes.

The first one will represent the game object to follow.
The second one will determine the height of the camera above that object,
and the third one will be the distance from that object.

Positional updates from the physics engine happen in the `FixedUpdate ()` method rather than the `Update ()` method.
Adjusting the camera position when following a non-kinematic object must therefore happen in the `LateUpdate ()` method
that is being called right after `FixedUpdate ()`.
The final `MainCamera` script looks like this:

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
	public float distance;
	public float height;
	public GameObject objectToFollow;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void LateUpdate () {
		if (objectToFollow == null)
			return;

		Vector3 destination = objectToFollow.transform.position;
		destination.x = 0.0f;
		destination.y += height;
		destination.z += distance;
		transform.position = destination;
	}
}
```

The camera's *x* position is kept at the center of the lane.
The *y* and *z* components are offset by the corresponding height and distance.
In the editor, set the height to 1 m and the distance to -3 m.
The object to follow can be empty (i.e. `null`).
In this case the script does nothing.
The object to follow can be set by dragging an object from the scene
(in our case, the Ball) onto the Main Camera's script slot in the inspector.

When you play the game now,
the camera will follow the ball.
Unfortunately,
this is also true if the ball should happen to fall off the lane.

## Organizing the Project

As you keep adding more assets to the game,
it is a good idea to organize them into appropriate folders.
Right-click on the Assets pane in the Project tab and select `Create->Folder`.
Create 4 folders with the following names:
-   Scenes
-   Scripts
-   Materials
-   Prefabs

When done,
move the assets into their corresponding folders by dragging and dropping them.

## Importing the Lane Model

In the top-level project folder on GitHub,
there is a folder named *assets*.
It contains two models,
which have been created using a 3D design program.
In order to import it,
create a folder named *Models* in the assets folder in the project pane.
Enter into that folder and select `Assets->Import New Asset...` from the menu.
Point the file dialog to the location of the model named `Lane.fbx` and open it.
On the import settings,
make sure that the model scale is set to `1`.
Uncheck the `Import Animation` and `Import Materials` options,
as the model does not contain useful animation or material information.
When you are done importing,
add the model as a game object to the scene by dragging it into the scene pane.
Note that its name in the scene pane is blue,
making it a prefab.
Any changes you subsequently make to the import settings will be reflected in all game objects using that model.
Also note that the actual mesh is named `Body1` and is a child of an
---otherwise empty---
game object named `Lane`.
Attach the lane material to the body by dragging it onto the lane.

Delete the previous lane from the scene and set the new lane's position to `(0, -0.1, 10)`.

## Add Colliders to the Lane

When you play the game now,
you will find that the pins and the ball will immediately fall through the lane
into the void below.
This is because,
by default,
the import process will not generate colliders for the meshes in the model.
The reason is that mesh colliders can drastically slow down physics calculations and thus the frame rate of a game.
Luckily,
for most models,
a simple box or sphere collider is appropriate and much faster.

Since most models consist of multiple meshes that can be covered by a common set of colliders,
it is a good idea to attach the colliders to the top-level game object.
So in the scene,
select the `Lane` object
(i.e. the parent of `Body1`).
Then,
add 5 box collider components with the following settings:

| Collider   |          Center          |           Size          |
|:----------:|:------------------------:|:-----------------------:|
|    (1)     | `( 0    ,  0    , 10 )`  | `( 1    , 0.2  , 20  )` |
|    (2)     | `(-0.95 ,  0    , 10 )`  | `( 0.1  , 0.2  , 20  )` |
|    (3)     | `( 0.95 ,  0    , 10 )`  | `( 0.1  , 0.2  , 20  )` |
|    (4)     | `(-0.75 , -0.15 , 10 )`  | `( 0.5  , 0.1  , 20  )` |
|    (5)     | `( 0.75 , -0.15 , 10 )`  | `( 0.5  , 0.1  , 20  )` |

When you play the game now,
the ball will stay on the lane.
It will also roll and stay in the gutter when thrown accordingly.
When you hit the pins,
you will see that they also will roll into the gutter.
Since the box colliders are not a perfect fit for the gutters,
you may see some parts of the pins being submerged into the gutter surface.
This can be prevented by adding an additional,
angled box collider that will result in the gutter's curve being followed more exactly.
Adding all these colliders is still considerably faster than using a full-blown mesh collider.

## Import the Bowling Pin Model

Import the bowling pin model from the file named `Pin.fbx` into the *Models* folder.
Use a scale factor of `2` and this time around,
do enable `Generate Colliders`.
Do *not* import animations but *do* import the materials.

When done,
drag the pin into the scene.
Disable the first original pin by unchecking it in its inspector.
Move the new pin into its place by setting the position of its parent game object to `(0, 0.15, 0)`.

The pin's body has a mesh collider but no Rigidbody attached to it.
Add a new Rigidbody component and set its mass to 1.5 kg.

The mesh collider exactly follows the mesh as of now.
This again is computationally expesive.
The behavior of bowling pins is,
however,
linked to their body shape,
so a simple capsule collider will not do.
There is a compromise solution that allows you to create a convex mesh collider that at least somewhat follows the mesh shape but is much simpler and faster.
This collider can be generated by selecting the `Convex` option in the mesh collider.
Then,
you can pre-compute (or *coook*) this collider by selecting `Everything` from the `Cooking Options` drop down menu.
The default skin width of 0.01 should be OK.
As a result,
you should see a collider that somewhat follows the pin's shape,
but which only has a couple of elements.

When you play the game,
this pin should behave just as expected when being hit.

## Replace the Original Pins

Delete all the original pins from the `Pins` hierarchy in the scene.
Then,
delete the original pin prefab.
Create a new pin prefab by dragging the newly created pin into the *Prefabs* folder.
Now move the pin in the scene into the `Pins` hierarchy and duplicate it 9 times.
Set the *x* and *z* position components to the values of the original pins to recreate the standard pin layout.

## Move the Pins and Create the Ball Pit

Move the pins and backstop to the end of the lane by setting the position of the *Pins* game object
(which contains the pins and the backstop)
to `(0, 0, 8)`.
Change the scale of the backstop to `(2.2, 1.25, 0.1)`.
Create a new material named *Pit Material* and set its albedo to black `(0, 0, 0, 255)` and assign it to the backstop.

In the *Pins* game object,
create two cubes named *Left Wall* and *Right Wall*.
Assign the pit material to them and set the left wall's position to `(-1.05, 0.25, 1)` and its scale to `(0.1, 1.25, 2)`.
Set the right wall's position to `(1.05, 0.25, 1)` and its scale to `(0.1, 1.25, 2)`.

To account for the longer lane,
change the ball speed to 16 m/s and set the angular drag to `0.005`.

## Create an Alley Prefab

Create an empty game object and name it *Alley*.
Make sure its position is `(0, 0, 0)`.
Move the lane and pins into the alley,
making them child objects.

In the *Alley* object,
create three planes named *Ground Left*, *Ground Right*, and *Ground Front*.
Set their transforms to the following values:

| Ground |       Position       |       Scale        |
|:------:|:--------------------:|:------------------:|
| Left   | `( -2 , 0.01 ,  0 )` | `( 0.2 , 1 , 2  )` |
| Right  | `( -2 , 0.01 ,  0 )` | `( 0.2 , 1 , 2  )` |
| Front  | `(  0 , 0.01 , -12)` | `( 0.6 , 1 , 0.4)` |

Create a new material named *Ground Material* and set its albedo to `(0, 167, 255, 255)` and assign it to the three ground planes.

Make the alley into a prefab by dragging it into the *Prefabs* folder.

## Duplicate the Alley and Create Surrounding Walls

Duplicate the alley prefab 6 times and set their positions as follows:

| Alley Number |      Position     |
|:------------:|:-----------------:|
|     (1)      | `(  -6 , 0 , 0 )` |
|     (2)      | `(   6 , 0 , 0 )` |
|     (3)      | `( -12 , 0 , 0 )` |
|     (4)      | `(  12 , 0 , 0 )` |
|     (5)      | `( -18 , 0 , 0 )` |
|     (6)      | `(  18 , 0 , 0 )` |

Create three planes at the scene's top level
and name them *Back Wall*, *Left Wall*, and *Right Wall*,
respectively.
Set their positions, rotations, and scales as follows:

| Wall  |       Position      |        Scale        |       Rotation      |
|:-----:|:-------------------:|:-------------------:|:-------------------:|
| Back  | `(   0 , 2 ,  10 )` | `( 4.2 , 1 , 0.4 )` | `( -90 , 0 ,   0 )` |
| Left  | `( -21 , 2 ,  -2 )` | `( 0.4 , 1 , 2.4 )` | `(   0 , 0 , -90 )` |
| Right | `(  21 , 2 ,  -2 )` | `( 0.4 , 1 , 2.4 )` | `(   0 , 0 ,  90 )` |

Since by default,
the graphics engine does not render the back faces of planes,
you need to set the rotation of the planes
so that all fronts face to the inside of the room.

Create a new material and call it *Wall Material*.
Set the albedo to `(144, 51, 197, 255)`
and assign it to the newly created walls.

## Reload the Scene after the Throw

You can use Unity's *Scene Manager* to reload the scene after the ball comes to rest after a throw.

Modify the ball script by adding the line

```C#
using UnityEngine.SceneManagement;
```
after the other `using` statments at the top of the script.

Then, add a `FixedUpdate()` method like this:

```C#
void FixedUpdate() {
	  if (thrown && rigidBody.IsSleeping()) {
		    SceneManager.LoadScene("Scene 1");
	}
}
```

The `IsSleeping()` method essentially checks whether an object has come to rest in the physics engine.
If so and if the object has previously been thrown,
the ball is at rest in the ball pit,
and the code then tells Unity to reload the scene for playing again.

The default sensitivity for `IsSleeping` is so high that you may have to wait a long time for the ball to come to rest.
You can change the setting in `Edit->Project Settings->Physics`.
A good value is `0.02`,
although you may have to experiment what the best setting may be for your game.

When the scene is reloaded,
the scene lighting gets darker.
This is a peculiarity of the Unity Editor and will not happen in a deployed game.
You can get rid of this artifact by selecting `Window->Lighting->Settings`,
unchecking `Auto Generate`, and click the `Generate Lighting` button.

## Where to Go From Here

Think of ways to improve the game:

-   Keep the ball on the lane when aiming.
-   Create a second round by removing toppled pins and throw the ball again for a spare.
-   Implement scoring and display the score.

Look at Unity's excellent website to learn how to do all these things and more.

## License

SPDX-License-Identifier: CC-BY-SA-4.0
