using System;

namespace Mono.Cecil.Test
{
    class Program
    {
        static int Main(string[] args)
        {
            // See https://aka.ms/new-console-template for more information
            Console.WriteLine("Hello, World!");

            string path = args[0]; // 程序集路径

            // 加载热更文件
            TestSession testSession = new TestSession();
            testSession.Load(path);

            testSession.PrintTypes();

            return 0;
        }
    }
}
