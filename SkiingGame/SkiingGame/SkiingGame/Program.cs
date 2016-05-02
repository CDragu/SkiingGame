using System;

namespace SkiingGame
{
#if WINDOWS || XBOX
    static class Program
    {
       
        static void Main(string[] args)
        {
            using (RunSequence game = new RunSequence())
            {
                game.Run();
            }
        }
    }
#endif
}

