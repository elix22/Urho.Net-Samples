# Urho3D KinematicCharacterController
-----------------------------------------------------------------------------------

### Description
-----------------------------------------------------------------------------------
C# Bullet Physics KinematicCharacterController platforming sample based upon :
Repo - https://github.com/Lumak/Urho3D-KinematicCharacterController


The KinematicCharacterController is adaptation of 1vanK's KinematicCharacterController found here,
https://github.com/1vanK/Urho3DKinematicCharacterController 

#### Implementation Info
* added PhysicsWorld to generate collision callback events when two triggers collide. Used when kinematic rigidbody enters moving kinematic volume.
* to add a moving collision volume, create a new **bool** variable called **IsMovingPlatform** in the Node section and check the check box, as shown in the pic below. And the RigidBody requires trigger and kinematic settings checked in the attributes.
* character also requires a RigidBody set as kinematic and trigger, see https://github.com/elix22/Urho.Net-Samples/blob/main/MovingPlatforms/Source/MovingPlatforms.cs#L151

![alt tag](https://github.com/elix22/Urho.Net-Samples/blob/main/MovingPlatforms/screenshot/EditorMovingPlatform.png)
How to create a moving volume in the editor.


License
-----------------------------------------------------------------------------------
The MIT License (MIT)










