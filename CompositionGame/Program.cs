using System;

namespace CompositionGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using(var game = new CompositionGame())
                game.Run();
        }
    }
}
