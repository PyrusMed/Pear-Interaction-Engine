![pear interaction engine](https://cloud.githubusercontent.com/assets/2764891/23945931/a249de04-0935-11e7-9390-f0ea3d58846c.png)

## What is PIE?
PIE was made to give designers and developers an easier way to create, share and modify user interactions. At its core, PIE is a set of cross-device platform independent Unity components that make creating intuitive interactions easier. PIE currently supports mouse and keyboard, Leap Motion, Oculus Touch, and HoloLens gestures, with more interaction support coming soon. Use PIE if you want a quick way to give your users intuitive interactions in VR, AR, on the desktop and on mobile. Contribute to PIE if you want to make interactions better for all developers, designers, and end users. We hope you find this useful!

![leap hover-small](https://cloud.githubusercontent.com/assets/2764891/22951407/eb0da21c-f2bd-11e6-916c-ff6219d49eb6.gif)
![leap grab-small](https://cloud.githubusercontent.com/assets/2764891/22951403/eb081d74-f2bd-11e6-9382-9c9d43570bb3.gif)
![touch motion rotate-small](https://cloud.githubusercontent.com/assets/2764891/22951404/eb089010-f2bd-11e6-91d8-c4dd47f8e097.gif)
![touch motion zoom-small](https://cloud.githubusercontent.com/assets/2764891/22951406/eb0b8676-f2bd-11e6-899b-b28ffd08f8f1.gif)
![touchmotion drop controller-small](https://cloud.githubusercontent.com/assets/2764891/22951402/eb079b92-f2bd-11e6-8a87-f30bff24c0b1.gif)
![leap zoom-small](https://cloud.githubusercontent.com/assets/2764891/22951405/eb0a088c-f2bd-11e6-9fb0-c5f80b485362.gif)

## Why PIE?
Interactions are a core component of every user facing software application, yet there are very few pattern guidelines, best practices and tools that can be used to design and develop interactions in virtual (VR) and mixed reality (MR). We've explored interactions using cheap headsets, expensive headsets, hand controllers, full body motion capture suits, and just about everything in between. In the process, we created PIE, a set of tools we've used for both rapid prototyping and production quality products. We're happy to share these tools and welcome all contributions as we look forward to the next generation of human-computer interactions.

## PIE Architecture
PIE consists of modular components that can be swapped in and out. These components revolve around the `Controller`, `ControllerBehavior` and `Interaction` classes.
- `Controller` represents a user input device, such as the user's eyes
- `ControllerBehavior` represents something a controller can do, like gazing at an object
- `Interaction` represents how input affects objects, such as highlighting an object when its looked at 

### Example - Fade On Hover (3 steps)

First, we define a `Controller` on the camera, because the user's head is a source of input.
![add controller-optimized](https://cloud.githubusercontent.com/assets/2764891/23585694/aca8a00a-0139-11e7-991b-356de8a67fc5.gif)

Next, we add a `ControllerBehavior` that lets other objects know when the head is looking (or gazing) at them.
![add gaze behavior-optimized](https://cloud.githubusercontent.com/assets/2764891/23585729/946d73ca-013a-11e7-8fb2-5b8c818749c1.gif)

Finally, we add an `Interaction` to the object we want to fade that links the "looking" logic to the "fading" logic.
![add interaction-optimized](https://cloud.githubusercontent.com/assets/2764891/23585857/7ce7d698-013d-11e7-8f19-2575f453077a.gif)

We can even link a single fade action to multiple `Interaction`s to keep things clean and consistent across multiple objects.
![link multiple interactions-optimized](https://cloud.githubusercontent.com/assets/2764891/23626811/055c0afa-0263-11e7-868c-b5b19d88ff29.gif)

### Why was it designed this way?
`Controller`s, `ControllerBehavior`s and `Interaction`s represent how we interact with objects in the real world. For example, if you were to pick up a mug, you would use your hand (`Controller`) to grab (`ControllerBehavior`) the mug that would react to your grab (`Interaction`). As babys we have hands (`Controller`s), but we don't know how to use them (i.e. we don't have any `ControllerBehavior`s). Overtime we learn to use our hands, body parts and tools to interact with objects, like a mug. Right now PIE is in its infancy, but as we grow our set of `Controller`s, `ControllerBehavior`s and `Interaction`s, we'll be able to quickly add complex interactions to our applications using our collective knowledge.

Oh, one more really cool thing. `ControllerBehavior`s and `Interaction`s can be mixed and matched. Using our baby analogy, once a child learns how to grab a mug it will quickly apply its grab logic to other objects, like things it shouldn't put in its mouth :/. PIE was designed similarly, but is much safer. Once we develop a `ControllerBehavior` we can use it to create countless interactions. For example, We've created a `ControllerBehavior` for grabbing with the Leap Motion hand, so that `ControllerBehavior` can be used to pick up any object, whether it's a cube, a mug, or anything else. It can also be used to open a door, or flip on a light switch...the possibilities are endless since we can link the grab logic to any `Intraction`.

If you couldn't tell, we're pretty excited about this :).

## Getting Started with PIE

### Clone and Run A Branch
1. Clone one of our several branches
2. Open one of the example scenes in Assets/Pear.InteractionEngine */Examples/...
3. Run!

### Import PIE Into Your Own Project
Download our latest package from the [releases](https://github.com/PearMed/Pear-Interaction-Engine/releases) section and import it into your Unity project
* The "Pear.InteractionEngine" package has no external requirements
* The "Pear.InteractionEngine HoloLens" package requires
  * [Pear.InteractionEngine](https://github.com/PearMed/Pear-Interaction-Engine/releases)
  * [HoloToolkit](https://github.com/Microsoft/HoloToolkit-Unity/blob/master/GettingStarted.md)
* The "Pear.InteractionEngine Leap Motion" package requires
  * [Pear.InteractionEngine](https://github.com/PearMed/Pear-Interaction-Engine/releases)
  * [Leap Motion Core Assets](https://developer.leapmotion.com/unity#100)
  * [Leap Motion Detection Module](https://developer.leapmotion.com/unity#100)
* The "Pear.InteractionEngine OculusTouch" package requires
  * [Pear.InteractionEngine](https://github.com/PearMed/Pear-Interaction-Engine/releases)
  * [Oculus Utilities for Unity 5](https://developer.oculus.com/downloads/unity/)
  * [Oculus Avatar SDK](https://developer.oculus.com/downloads/unity/)
* The "Pear.InteractionEngine TouchMotion" package requires
  * [Pear.InteractionEngine](https://github.com/PearMed/Pear-Interaction-Engine/releases)
  * [Pear.InteractionEngine LeapMotion](https://github.com/PearMed/Pear-Interaction-Engine/releases)
  * [Pear.InteractionEngine OculusTouch](https://github.com/PearMed/Pear-Interaction-Engine/releases)
  * [Leap Motion Core Assets](https://developer.leapmotion.com/unity#100)
  * [Leap Motion Detection Module](https://developer.leapmotion.com/unity#100)
  * [Oculus Utilities for Unity 5](https://developer.oculus.com/downloads/unity/)
  * [Oculus Avatar SDK](https://developer.oculus.com/downloads/unity/)

## Contributing
If you like PIE, want to make it better, or just want to work on something cool, help us out! We think user interactions are extremely important, so we'd love to work with others to improve how we all design, develop and use interactions in VR, MR and everywhere else. Fork this repo to get started!

## Who is using PIE?
- [Pear Med](http://www.pearmedical.com)

Hopefully this list will continue to grow ;)

## Questions?
If you have any questions please log an [issue](https://github.com/PearMed/Pear-Interaction-Engine/issues) or [contact us directly](http://www.pearmedical.com/contact.html)

# Thanks!
