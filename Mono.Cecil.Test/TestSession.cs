using ILRuntime.Mono.Cecil;
using ILRuntime.Mono.Cecil.Cil;
using System;
using System.IO;

namespace Mono.Cecil.Test
{
    class TestSession
    {
        FileStream AssemblyFs;
        FileStream PdbFs;
        ModuleDefinition moduleDefinition;
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assemblyPath">程序集路径</param>
        public void Load(string assemblyPath)
        {
            AssemblyFs = new FileStream(assemblyPath, FileMode.Open, FileAccess.Read);
            {
                var path = Path.GetDirectoryName(assemblyPath);
                var name = Path.GetFileNameWithoutExtension(assemblyPath);
                var pdbPath = Path.Combine(path, name) + ".pdb";

                PdbFs = new FileStream(pdbPath, FileMode.Open);
                {
                    ISymbolReaderProvider symbolReaderProvider = new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider();

                    LoadAssembly(AssemblyFs, PdbFs, symbolReaderProvider);
                }
            }
        }

        private void LoadAssembly(System.IO.Stream stream, System.IO.Stream symbol, ISymbolReaderProvider symbolReader)
        {
            // https://github.com/jbevain/cecil/wiki/HOWTO Check the Cecil wiki for some usage examples.
            // 从程序集中加载模块
            moduleDefinition = ModuleDefinition.ReadModule(stream);
            // 加载调试文件
            //if (symbolReader != null && symbol != null)
            //{
            //    module.ReadSymbols(symbolReader.GetSymbolReader(module, symbol));
            //}

        }
        public void PrintTypes()
        {
            foreach (TypeDefinition type in moduleDefinition.Types)
            {
                Console.WriteLine(type.FullName);
                foreach (MethodDefinition method in type.Methods)
                {
                    Console.WriteLine($"\t{method.FullName}");
                    if (method.Body == null || method.Body.Instructions == null)
                    {
                        continue;
                    }
                    foreach (Instruction instruction in method.Body.Instructions)
                    {
                        Console.WriteLine($"\t\t{instruction.OpCode}");
                    }
                }
            }
        }
        public void Dispose()
        {
            AssemblyFs?.Close();
            PdbFs?.Close();
        }
    }
}
