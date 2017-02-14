# Pear Interaction Engine (PIE)

## About PIE
PIE is a set of cross-device Unity components that make interacting with objects easy, regardless of the target platform. PIE currently supports mouse and keyboard, Leap Motion, Oculus Touch, and mixed reality inputs (e.g. HoloLens gestures), with more coming soon. Use PIE if you want to give your users an easy way to interact with virtual objects. Contribute to PIE if you want to make virtual (VR) and mixed reality (MR) better for everyone (i.e. developers, designers, users, ect). We hope you find this useful!

- images of interactions -

## Why PIE?
Interactions are a core component of every user facing software application, yet there are very few pattern guidelines, best practices and tools that can be used to design and develop interactions in virtual (VR) and mixed reality (MR). Over the last year we've explored interactions using cheap headsets, expensive headsets, hand controllers, full body motion capture suits, and just about everything in between. In the process, we created PIE, a set of tools we've used for both rapid prototyping and production quality products. We're happy to share these tools and welcome all contributions as we look forward to the next generation of human-computer interactions.

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
