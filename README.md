# Coroutines for WPF

[![Nuget](https://img.shields.io/nuget/v/CoroutinesForWpf.svg)](https://www.nuget.org/packages/CoroutinesForWpf/)

Influenced by coroutines in Unity3D. Giving the ability to sequentially declare animations and other time dependent tasks.

## Compatibility

The WPF specific parts requires one of:

* .NET Framework 3.5 or later
* .NET Core 3.0

With a custom event pump, you can use any .NET version compatible with .NET Standard 1.1 or .NET Framework 3.5 or newer.

## Example code

```C#
private void OnClick()
{
    var routine = Coroutine.Start(GreetTheWorld());

    // To abort the coroutine, just call Dispose()
    //routine.Dispose();
}

private IEnumerator GreetTheWorld()
{
    TextBlock1.Text = "Hello ...";

    yield return new WaitForSeconds(2.0);

    TextBlock1.Text = "Hello World!";
}
```

More example code can be found in the source code.

* Main WPF example: [CoroutinesForWpf.Example](Source/CoroutinesForWpf.Example/)
* Custom event pump example: [CoroutinesDotNet.CustomPumpExample](Source/CoroutinesDotNet.CustomPumpExample/)
