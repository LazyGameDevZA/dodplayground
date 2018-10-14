using System;

namespace ComponentGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using(var game = new ComponentGame())
                game.Run();
        }
    }
}
