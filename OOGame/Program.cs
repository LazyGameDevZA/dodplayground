using System;

namespace OOGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using(var game = new ObjectOrientedGame())
                game.Run();
        }
    }
}
