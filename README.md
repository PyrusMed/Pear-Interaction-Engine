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
PIE consists of modular components that can be swapped in and out. These components revolve around the `Controller`, `ControllerBehavior` and `Interaction` classes.
- `Controller`s represent user input devices
- `ControllerBehavior`s represent things a controller can do
- `Interaction`s define how users interact with the virtual world

### Example - Fade On Hover (3 steps)

First, we define a `Controller` on the camera, because the user's head is a source of input.
![add controller-optimized](https://cloud.githubusercontent.com/assets/2764891/23585694/aca8a00a-0139-11e7-991b-356de8a67fc5.gif)

Next, we add a `ControllerBehavior` that lets other objects know when the head is looking (or gazing) at them.
![add gaze behavior-optimized](https://cloud.githubusercontent.com/assets/2764891/23585729/946d73ca-013a-11e7-8fb2-5b8c818749c1.gif)

Finally, we add an `Interaction` to the object we want to fade that links the "looking" logic to the "fading" logic.
![add interaction-optimized](https://cloud.githubusercontent.com/assets/2764891/23585857/7ce7d698-013d-11e7-8f19-2575f453077a.gif)

### Why was it designed this way?
`Controller`s, `ControllerBehavior`s and `Interaction`s are separate, modular components that can be mixed and matched, which makes designing and developing interactions easier. As we iterated we noticed 2 big problems: 1) we were writing a lot of code for every interaction, and 2) to adjust an interaction we had to write even more code. Essentially, we were writing large classes that weren't taking full advantage of the inspector. Now, with PIE, if we want to create a new hover effect, for example, we just create a single component for the new effect and reuse the existing `Controller` and `ControllerBehavior` components that define the "looking" logic. Not only do we write less code, this pattern forces us to think about interactions in chunks, which makes problems smaller, and keeps the inspector organized and easier to use (designers appreciate this last one). Our hope is that as we add more `Controller`s, `ControllerBehavior`s and `Interaciton`s it will be easier for anyone to create intuitive cross-platform apps.

## Contributing
If you like PIE, want to make it better, or just want to work on something cool, help us out! We think user interactions are extremely important, so we'd love to work with others to improve how we all design, develop and use interactions in VR, MR and everywhere else. Fork this repo to get started!

## Who is using PIE?
- [Bosc](http://www.pearmedical.com/bosc.html)

Hopefully this list will continue to grow ;)

## Questions?
If you have any questions please log an [issue](https://github.com/PearMed/Pear-Interaction-Engine/issues) or [contact us directly](http://www.pearmedical.com/contact.html)

# Thanks!
