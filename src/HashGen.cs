using System;
using BCrypt.Net;

namespace HashGen;

class Program
{
    static void Main()
    {
        Console.WriteLine(BCrypt.Net.BCrypt.HashPassword("admin123", 12));
    }
}
