# Raccoon Ninja's Toolbox
Every time I started a project, I re-implemented the same helpers. So I decided to create this package to same me some
time, make it easy to update it and last but not least, to share it with the community and maybe help someone else.

It's all free under MIT License.


# Requirements
The Demos requires `Text Mesh Pro`, but the actual package doesn't have any requirements.


# Features
## Singleton
Let's say you want to implement a singleton class called `GameScoreManager`. In this case, you need to:
1. Create a script named `GameScoreManager.cs`;
2. Instead of inheriting from MonoBehavior, you need to inherit from `BaseSingletonController<GameScoreManager>`;

That's it. Now you can access your singleton class from anywhere in your code by calling `GameScoreManager.Instance`. 

Note that the Awake routine is used to setup the Singleton, but if you need to do something there, you can override
the `PostAwake` method.

> In the `_Demos/Scripts` folder there's an example in the file `SingletonGameObject.cs`

## Int/Float min and max with slider
If you need to define a min and max value and then get a random value between them, or just use the min and max values, 
then you use `RangedInt` and `RangedFloat`. They are exactly the same, differing just in the data type, so I'll show how
to use `RangedInt` and you can apply the same to `RangedFloat`.

Creating a serialized property of RangedInt with the default min (0)/max (1) values. 
```charp
[SerializeField] private RangedInt rangedInt;
```

Creating a serialized property of RangedInt with the default min (0) and setting max to 15.
```charp
[SerializeField, MinMaxIntRange(max: 15)] private RangedInt rangedInt;
```

Creating a serialized property of RangedInt with the min -10 and setting max to 10.
```charp
[SerializeField, MinMaxIntRange(-10, 15)] private RangedInt rangedInt;
```

Considering that the sliders were not changed, to access the min and max values, you can use:
```charp
rangedInt.MinValue; // -10
rangedInt.MaxValue; // 15
```

You can also get a random value between the two:
```charp
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
is done. This is optional, but if you need it, you also must add an instance of the CallbackRunner component. There is 
a prefab for it. (More info about it below.)

> In the `_Demos/ScriptableObjectAsserts` folder there's a clip example.

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


# TODO
Add proper tests instead of relying on demos.


# License
The MIT License is a permissive free software license that originates from the Massachusetts Institute of Technology. 
It's a simple and flexible license that's widely used in many open source projects.

## Key Features of the MIT License:

1. **Simplicity**: The MIT License is quite straightforward and easy to understand. This is one of the reasons why it's
one of the most popular software licenses.

2. **Permission for Broad Use**: The license allows users to use, copy, modify, merge, publish, distribute, sublicense, 
and/or sell copies of the software.

3. **Use in Proprietary Software**: The MIT license allows you to use, modify, and distribute the software in your 
proprietary software. This means that you can include MIT-licensed software in your own software, and you can 
distribute that software under any license you want, including a proprietary license.

4. **Requirement for Attribution**: If you use MIT-licensed software, you must include the original copyright notice 
and the text of the MIT license.

Here is the full text of the MIT License for reference:

```
MIT License

Copyright (c) [year] [fullname]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## Disclaimer

The license does not provide a warranty or take on liability. The software is provided "as is", and the user assumes 
responsibility for any issues that may arise from using the software.

## Conclusion

The MIT License is a very flexible license that provides a lot of freedom to users while maintaining a minimal 
requirement for attribution. This makes it a popular choice for many open source projects. However, like all licenses, 
it's important to read and understand the terms of the MIT License before using MIT-licensed software in your projects.
Always consult with a legal expert if you have questions or doubts.