# Pear Interaction Engine (PIE)

## About PIE
PIE is a set of cross-device Unity components that make interacting with objects easy, regardless of the target platform. PIE currently supports mouse and keyboard, Leap Motion, Oculus Touch, and HoloLens gestures, with more interaction support coming soon. Use PIE if you want an easy way to give your users intuitive interactions in VR, AR, and on the desktop. Contribute to PIE if you want to make interactions better for everyone developers, designers, and end users. We hope you find this useful!

![leap hover-small](https://cloud.githubusercontent.com/assets/2764891/22951407/eb0da21c-f2bd-11e6-916c-ff6219d49eb6.gif)
![leap grab-small](https://cloud.githubusercontent.com/assets/2764891/22951403/eb081d74-f2bd-11e6-9382-9c9d43570bb3.gif)
![touch motion rotate-small](https://cloud.githubusercontent.com/assets/2764891/22951404/eb089010-f2bd-11e6-91d8-c4dd47f8e097.gif)
![touch motion zoom-small](https://cloud.githubusercontent.com/assets/2764891/22951406/eb0b8676-f2bd-11e6-899b-b28ffd08f8f1.gif)
![touchmotion drop controller-small](https://cloud.githubusercontent.com/assets/2764891/22951402/eb079b92-f2bd-11e6-8a87-f30bff24c0b1.gif)
![leap zoom-small](https://cloud.githubusercontent.com/assets/2764891/22951405/eb0a088c-f2bd-11e6-9fb0-c5f80b485362.gif)

## Why PIE?
Interactions are a core component of every user facing software application, yet there are very few pattern guidelines, best practices and tools that can be used to design and develop interactions in virtual (VR) and mixed reality (MR). We've explored interactions using cheap headsets, expensive headsets, hand controllers, full body motion capture suits, and just about everything in between. In the process, we created PIE, a set of tools we've used for both rapid prototyping and production quality products. We're happy to share these tools and welcome all contributions as we look forward to the next generation of human-computer interactions.

## Getting Started with PIE

### Clone and Run A Branch
1. Clone one of our several branches
2. Open one of the example scenes in Assets/Pear.InteractionEngine */Examples/...
3. Run!

### Import PIE Into Your Own Project
Download our latest package from the [releases](https://github.com/PearMed/Pear-Interaction-Engine/releases) section and import it into your Unity project
  - The "Pear.InteractionEngine" package has no external requirements
  - The "Pear.InteractionEngine TouchMotion" package requires "Pear.InteractionEngine" and the Leap Motion and Oculus Touch Unity modules
  - The "Pear.InteractionEngine HoloLens" package requires "Pear.InteractionEngine" and the HoloToolkit Unity module

## PIE Architecture
PIE consists of modular components that can be swapped in and out. These components revolve around the `Controller` and `InteractableObject` classes. `Controller`s represent user input devices and `InteractableObject`s represent objects that can be manipulated by user input.

### Controller
`Controller` represents a user input device. The camera, for example, is a `Controller` because gaze is a form of input that can be used to hover over objects. Each controller has a set of `ControllerBehavior`s that use the `Controller` to update the state of `InteractableObject`s. For instance, `HoverOnGaze` is a `ControllerBehavior` that updates the state of an `InteractableObject` when the camera hover's over it. `HoverOnGaze` looks something like this.

```csharp
//[...]

// If the camera is hovering over an interactable
// update the interactable's state
if (hoverInteractable != null)
  hoverInteractable.Hovering.Add(Controller);

//[...]
```

### InteractableObject
`InteractableObject` represents objects that can be manipulated by user input. Each `InteractableObject` has a set of states (Grabbing, Moving, Resizing, Rotating, Selected) that are updated by `Controller`s and their `ControllerBehavior`s. In the example above, the `ControllerBehavior` `HoverOnGaze` tells the `InteractableObject` it's being hovered over by the camera `Controller`. Once an `InteractableObject`'s state is updated it lets all of it's listeners know. This pattern is powerful because it allows developers and designers to create common interactions across multiple controllers. Let's look at an example...

`FadeOnHover` is a class that listens for the change in hover state of `InteractableObject`s and sets their opacity accordigly. The code looks something like this:

```csharp
//[...]

// Call the FadeOut function when the interactable is hovered over
interactable.Hovering.OnStart.AddListener(FadeOut);

// Call the FadeIn function when hovering ends
interactable.Hovering.OnEnd.AddListener(FadeIn);

//[...]
```

Now we can hover over the `InteractableObject` with *any* `Controller` and it will FadeIn/FadeOut accordingly. This works great if you want to design and develop common interactions using different forms of input, like mouse and keyboard (desktop), Leap Motion / Oculus Touch (VR), and gestures (MR).

## Contributing
If you like PIE, want to make it better, or just want to work on something cool, help us out! We think user interactions are extremely important, so we'd love to work with others to improve how we all design, develop and use interactions in VR and MR. Fork this repo to get started!

## Who is using PIE?
- [Bosc](http://www.pearmedical.com/bosc.html)

Hopefully this list will continue to grow ;)

## Questions?
If you have any questions please log an [issue](https://github.com/PearMed/Pear-Interaction-Engine/issues) or [contact us directly](http://www.pearmedical.com/contact.html)

# Thanks!
