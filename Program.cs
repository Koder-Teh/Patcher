using Patcher.Interface;
using Patcher.Other;
using Patcher.Patching;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Patcher
{
    internal class Program
    {
        public static Enviroment Enviroment { get; private set; }

        public static void Init()
        {
            Enviroment = new Enviroment();
        }

        static async Task Main(string[] args)
        {
            Init();

            if (!HelpOrProblem(args, out string problem))
            {
                ErrorExit(problem);
                return;
            }

            Template? template = JsonSerializer.Deserialize(File.ReadAllText(Enviroment._args.file_config.FullName), SourceGenerationContext.Default.Template);

            if (template == null || template.patchs == null)
            {
                ErrorExit($"problem with template!");
                return;
            }

            Enviroment._template = template;

            Console.WriteLine($"Load Patchs: {template.patchs.Count}");

            Enviroment._handler = new HandlerFile();
            await Enviroment._handler.ProcessHandler();
        }

        public static bool HelpOrProblem(string[] args, out string problem)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("========== Patcher ==========");
                Console.WriteLine("patcher <PATHEXE> <PATHCONFIG> [OPTIONS]");
                Console.WriteLine("<PATHEXE> - Program for patch");
                Console.WriteLine("<PATHCONFIG> - Template for patch of program");
                Console.WriteLine("Options:");
                Console.WriteLine("--mask - Use mask for exe from config");
                Console.WriteLine("========== Patcher ==========");
                problem = string.Empty;
                return false;
            }
            else
            {
                FileInfo file = new FileInfo(args[0]);
                FileInfo fileconfig = new FileInfo(args[1]);
                if (!file.Exists || args[0] == string.Empty)
                {
                    problem = $"{file.FullName} does not exist.";
                    return false;
                }

                if (!fileconfig.Exists || args[1] == string.Empty)
                {
                    problem = $"config does not exist.";
                    return false;
                }
                
                Args arguments = new Args();
                arguments.args = args;
                arguments.file = file;
                arguments.file_config = fileconfig;
                arguments.UpdateOptions();
                Enviroment._args = arguments;
            }

            problem = string.Empty;
            return true;
        }

        public static void ErrorExit(string content)
        {
            if(content != string.Empty)
            {
                Console.WriteLine(content);
            }
        }
    }
}
