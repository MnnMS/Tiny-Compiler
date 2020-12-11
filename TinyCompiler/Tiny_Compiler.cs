using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public static class Tiny_Compiler
    {
        public static Scanner Tiny_Scanner = new Scanner();

        public static List<Token> TokenStream = new List<Token>();

        public static void Start_Compiling(string SourceCode) //character by character
        {
            Tiny_Scanner.StartScanning(SourceCode);
        }
    }
}
