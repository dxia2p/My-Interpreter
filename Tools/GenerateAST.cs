using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolInterpreter.Tools {
    internal class GenerateAST {
        private static void defineType(StreamWriter sw, string baseName, string className, string fields) {
            sw.WriteLine("internal class " + className + " : " + baseName + " {");

            // Constructor
            sw.WriteLine("  " + className + "(" + fields + ") {");

            // Store constructor parameters in fields
            string[] fieldsList = fields.Split(", ");
            foreach (string field in fieldsList) {
                string name = field.Split(" ")[1];
                sw.WriteLine("      this." + name + " = " + name + ";");
            }

            sw.WriteLine("  }");

            // Fields
            sw.WriteLine("");
            foreach (string field in fieldsList) {
                sw.WriteLine("  " + field + ";");
            }

            sw.WriteLine("}");
        }
        private static void defineAst(string outputDir, string baseName, string namesp, string[] types) {
            string path = Path.Combine(outputDir, baseName, ".cs");

            using StreamWriter sw = File.CreateText(path);

            // Output namespace
            sw.WriteLine("namespace " + namesp + " {");

            // Output base class
            sw.WriteLine("abstract class " + baseName + " {");

            // Base class closing bracket '}'
            sw.WriteLine("}");

            // AST classes
            foreach (string type in types) {
                string className = type.Split(":")[0].Trim();
                string fields = type.Split(":")[1].Trim();
                defineType(sw, baseName, className, fields);
            }

            // Closing bracket '}' for namespace
            sw.WriteLine("}");
        }
        static int Main(string[] args) {
            if (args.Length != 1) {
                Console.WriteLine("First argument: <output directory>");
                return 1;
            }

            string outputDir = args[0];
            string[] types = {
                "Binary : Expr left, Token operator, Expr right",
                "Grouping : Expr expression",
                "Literal : Object value",
                "Unary : Token operator, Expr right"
            };
            defineAst(outputDir, "Expr", "CoolInterpreter", types);


            return 0;
        }
    }
}
