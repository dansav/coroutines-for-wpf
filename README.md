# Coroutines for WPF

[![Nuget](https://img.shields.io/nuget/v/CoroutinesForWpf.svg)](https://www.nuget.org/packages/CoroutinesForWpf/)

Influenced by coroutines in Unity. Giving the ability to sequentially declare animations and other time dependent tasks.

Example code:

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
