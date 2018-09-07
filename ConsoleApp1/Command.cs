using System;

namespace ConsoleApp1
{
    public class Command : EventArgs
    {
        public bool Register { get; set; } = true;
    }
}