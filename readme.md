# Raccoon Ninja's Toolbox
Every time I started a project, I re-implemented the same helpers. So I decided to create this package to save me some
time, make it easy to update it and last but not least, to share it with the community and maybe help someone else.

It's all free under MIT License.


# Requirements
Unity 6 (6000.0) or later. The package has no additional dependencies.


# Features
## Singleton
Let's say you want to implement a singleton class called `GameScoreManager`. In this case, you need to:
1. Create a script named `GameScoreManager.cs`;
2. Instead of inheriting from MonoBehaviour, you need to inherit from `BaseSingletonController<GameScoreManager>`;

That's it. Now you can access your singleton class from anywhere in your code by calling `GameScoreManager.Instance`.

Note that the Awake routine is used to setup the Singleton, but if you need to do something there, you can override
the `PostAwake` method.

> In the `_Demos/Scripts` folder there's an example in the file `SingletonGameObject.cs`

## Int/Float min and max with slider
If you need to define a min and max value and then get a random value between them, or just use the min and max values,
then you use `RangedInt` and `RangedFloat`. They are exactly the same, differing just in the data type, so I'll show how
to use `RangedInt` and you can apply the same to `RangedFloat`.

Creating a serialized property of RangedInt with the default min (0)/max (1) values.
```csharp
[SerializeField] private RangedInt rangedInt;
```

Creating a serialized property of RangedInt with the default min (0) and setting max to 15.
```csharp
[SerializeField, MinMaxIntRange(max: 15)] private RangedInt rangedInt;
```

Creating a serialized property of RangedInt with the min -10 and setting max to 10.
```csharp
[SerializeField, MinMaxIntRange(-10, 15)] private RangedInt rangedInt;
```

Considering that the sliders were not changed, to access the min and max values, you can use:
```csharp
rangedInt.MinValue; // -10
rangedInt.MaxValue; // 15
```

You can also get a random value between the two:
```csharp
rangedInt.Random(); // -10 <= x <= 15
```

> Note: The Max is always inclusive, even for int.

> In the `_Demos/Scripts` folder there's a bunch of examples in the file `Demo.cs`

### Is it possible to create a slider for another type?
Yes, but I personally didn't see a use-case for this, especially if we are considering the min and max values types like
double or decimal offers.

If you want to, you can easily create your own slider by following those steps.
For this example, let's say you want to create a slider for `double`.
(I'm going to suggest some class names, but you can change them to whatever you want. If you do, remember to adjust the code below.)
1. Create a class named `RangedDouble` inheriting from `RangedNumeric<double>`;
2. Create a class named `MinMaxDoubleRangeAttribute` inheriting from `System.Attribute` and implementing `IMinMaxRangeAttribute<double>`;
3. Create a class named `MinMaxDoubleRangeDrawer` inheriting from `NumericSliderDrawer<MinMaxDoubleRangeAttribute, double>`;
4. In class `MinMaxDoubleRangeDrawer`, add the following attribute: `[CustomPropertyDrawer(typeof(RangedDouble), true)]`
5. Implement all the required methods in `MinMaxDoubleRangeDrawer`.

There you go. All ready. Fair warning: This might be slightly annoying to make.

> You can use `MinMaxFloatSliderDrawer` and `MinMaxIntSliderDrawer` as examples.

## Typed audio clip
This is a ScriptableObject where you can select a range for volume and pitch to be randomized. If the main camera has an
audio source, you can also play the audio clip on it using the inspector button to get a feel of how your audio clip
is going to sound like.

When using the Play method of this scriptable object, you can pass a callback that will be called after the audio
is done (based on the PracticalDuration). This is optional and uses Unity 6's Awaitable internally — no additional
components or singletons required. You can also pass a CancellationToken for caller-controlled cancellation.

> In the `_Demos/ScriptableObjectAssets` folder there's a clip example and in `_Demos/Scripts` there's a `TypedAudioClipDemo.cs` showing the callback in action.

The audio file was downloaded from [freesound.org](https://freesound.org/people/Alivvie/sounds/323437/).

## Callback runner
This is a singleton that manages and helps you run coroutines.
You place it in your scene and you'll be able to:
1. Start a coroutine from any class (even if it's not a MonoBehaviour);
2. Add a delay to the coroutine;
3. Use a normal function as a Coroutine;
4. Stop a coroutine;
5. Using UnityEvents you also can:
    - Know when a Coroutine started;
    - Know when a Coroutine finished;
    - Know when a Coroutine was stopped by the code/user;

With this, all coroutines are identified by an ID (Guid), so you can keep track of it, if you need to.

> In the `_Demos/Scripts` folder there's an example in the file `CallbackRunnerDemo.cs`

## Readonly inspector field
This is a custom attribute that you can use to make a field in the inspector read-only. This is useful when you want to
show a value, but don't want to allow the user to change it.

It's not perfect, if you use it on a list, the list items will be readonly, but you'll still be able to add or remove
items from it. I haven't found a way to make the list controls read-only.

> In the `_Demos/Scripts` folder there's a bunch of examples in the file `Demo.cs`


# Warranties and Support
None and almost none. This package a collection of tools I've been using and is provided AS-IS. In case of bugs or feature
requests, feel free to open an issue or a pull request. I'll try to help as much as I can, but I can't guarantee anything.

If you like this and made some features better, I encourage you to open a pull request. I'll be happy to review it and
merge into this package. (This will probably never happen, but if does, I'll create a contributors section here, add
you and create a link to your project.)

## Disclaimer

The license does not provide a warranty or take on liability. The software is provided "as is", and the user assumes
responsibility for any issues that may arise from using the software.
