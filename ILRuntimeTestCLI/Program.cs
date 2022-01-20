using ILRuntimeTest.TestBase;
using System;
using System.Collections.Generic;
using System.IO;

namespace ILRuntimeTestCLI
{
    class Program
    {
        static int Main(string[] args)
        {
            SetConsoleOutToFile();

            if (args.Length <2)
            {
                Console.WriteLine("Usage: ILRuntimeTestCLI path useRegister[true|false]");
                return -1;
            }

            string path = args[0]; // 热更DLL路径
            bool useRegister = args[1].ToLower() == "true"; // 是否使用寄存器版本
            TestSession session = new TestSession();
            session.Load(path, useRegister);
            int ignoreCnt = 0;
            int todoCnt = 0;
            List<TestResultInfo> failedTests = new List<TestResultInfo>();
            foreach(var i in session.TestList)
            {                
                i.Run(true);
                var res = i.CheckResult();
                if (res.Result == ILRuntimeTest.Test.TestResults.Failed)
                {
                    if (res.HasTodo)
                        todoCnt++;
                    else
                        failedTests.Add(res);
                }
                else if (res.Result == ILRuntimeTest.Test.TestResults.Ignored)
                    ignoreCnt++;

                Console.WriteLine(res.Message);
                Console.WriteLine("===============================");
            }
            Console.WriteLine("===============================");
            Console.WriteLine($"{failedTests.Count} tests failed");
            foreach(var i in failedTests)
            {
                Console.WriteLine($"Test name:{i.TestName}, Message:{i.Message}");
                Console.WriteLine("===============================");
            }
            Console.WriteLine($"Ran {session.TestList.Count} tests, {failedTests.Count} failded, {ignoreCnt} ignored, {todoCnt} todos");
            

            
            session.Dispose();
            return failedTests.Count <= 0 ? 0 : -1;
        }

        /// <summary>
        /// 设置将输出到控制台
        /// </summary>
        static void SetConsoleOutToFile()
        {
            // 如果需要进行恢复操作可以将 Console.Out 进行缓存，并再次进行设置
            // Console.SetOut(Console.Out);

            string logFileName = "out.txt";
            FileStream filestream = new FileStream(logFileName, FileMode.Create);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);

            // 打开日志文件夹
            System.Diagnostics.Process.Start("Explorer.exe", Environment.CurrentDirectory);
        }
    }
}
