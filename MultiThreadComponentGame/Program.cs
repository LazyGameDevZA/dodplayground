using System;

namespace MultiThreadComponentGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using(var game = new MultiThreadComponentGame())
                game.Run();
        }
    }
}
