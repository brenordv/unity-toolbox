# Raccoon Ninja's Toolbox
Every time I started a project, I re-implemented the same helpers. So I decided to create this package to same me some
time, make it easy to update it and last but not least, to share it with the community and maybe help someone else.

It's all free under GNU General Public License (GPL).

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

## 


https://freesound.org/people/Alivvie/sounds/323437/