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
indicated by the position (0,0,0) in the *Transform* component.
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
The R, G, and B components should be set to (255, 215, 0),
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
Set the color to black (0, 0, 0),
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
Set the *Scale* to (0.1, 0, 2).
This corresponds to a width of 1 meters and a lenght of 20 meters,
which is pretty close to the size of a standard bowling lane.

The ball needs to be at the start of the lane.
Set the ball transform's position to (0, 0.1, -10).
The ball is also too large.
Set its scale to (0.2, 0.2, 0.2),
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

	public float speed = 10;
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
the Ball inspector will show the speed set to 10.
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
Set its transform's position to (0, 0.19, 0) and its scale to (0.06, .19, 0.06).

Create a new material named "Pin Material",
and set its *Albedo* to white (255, 255, 255),
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

| Pin Number |        Position       |
|:----------:|:---------------------:|
|    (1)     | ( 0.15 , 0.19 , 0.26) |
|    (2)     | (-0.15 , 0.19 , 0.26) |
|    (3)     | ( 0    , 0.19 , 0.52) |
|    (4)     | (-0.3  , 0.19 , 0.52) |
|    (5)     | ( 0.3  , 0.19 , 0.52) |
|    (6)     | ( 0.15 , 0.19 , 0.78) |
|    (7)     | (-0.15 , 0.19 , 0.78) |
|    (8)     | (-0.45 , 0.19 , 0.78) |
|    (9)     | ( 0.45 , 0.19 , 0.78) |

### Change the Pin Scale on the Prefab

We will now see the power of prefabs.
The pins are actually too thin for real bowling pins.
Actual bowling pins are about 12 cm in diameter,
not the 6 cm we used originally.

To change all pin diameters at the same time,
select the pin prefab in the Asset pane and set its scale to (0.12, 0.19, 0.12).
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

Set its transform position to (0, 0.25, 2) and its scale to (1, 0.5, 0.1).
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
Note that this does not change the value of the `speed` variable in the corresponding C# script.

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
Adjusting the camera position when following a non-kinematic object must therefore happen in that method.
The final `MainCamera` script looks like this:

```C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {
	public float distance = 10;
	public float height = 2;
	public GameObject objectToFollow;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
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
The object to follow can be empty (i.e. `null`).
In this case the script does nothing.
The object to follow can be set by dragging an object from the scene
(in our case, the Ball) onto the Main Camera's script slot in the inspector.

When you play the game now,
the camera will follow the ball.
Unfortunately,
this is also true if the ball should happen to fall off the lane.

Fixing this is easy,
but we have reached the end of the tutorial now.

## Where to Go From Here

Think of ways to improve the game:

-   Keep the ball on the lane.
-   Create a second round by removing toppled pins and throw the ball again for a spare.
-   Implement scoring and display the score.

Look at Unity's excellent website to learn how to do all these things and more.

## License

SPDX-License-Identifier: CC-BY-SA-4.0
