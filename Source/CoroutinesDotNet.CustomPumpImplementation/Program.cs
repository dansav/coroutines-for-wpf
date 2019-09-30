using System;
using System.Collections;

namespace CoroutinesDotNet.CustomPumpExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Press enter to exit...");
            Console.WriteLine();

            var routine = CustomCoroutine.Start(AnimateText());

            Console.ReadLine();
            routine.Dispose();
        }

        private static IEnumerator AnimateText()
        {
            var texts = new[] {"Hello ...", "Hello world!" };

            while (true)
            {
                for (var i = 0; i < texts.Length; i++)
                {
                    if (i % 2 == 0)
                    {
                        yield return AnimateLine(texts[i]);
                    }
                    else
                    {
                        yield return new WaitFor(TimeSpan.FromMilliseconds(900));
                        Console.CursorLeft = 0;
                        Console.WriteLine(texts[i]);
                        yield return new WaitForSeconds(1);
                    }
                }
            }
        }

        private static IEnumerator AnimateLine(string line)
        {
            for (int i = 0; i < line.Length; i++)
            {
                Console.Write(line[i]);
                yield return null;
            }
        }
    }
}
