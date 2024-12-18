using System.Diagnostics;

namespace Cirno.Standard
{
    class Sys : FunctionNode
    {
        class SysValue : InnerFunctionNode
        {
            public SysValue() : base(new List<string>() { "value" }, null)
            {

            }
            public override ObjectClass Call(List<ObjectClass> arguments, Interpreter interpreter)
            {
                string command = $"{arguments[0]}";
                Process process = new Process();

                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C {command}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                return new StringClass(output);
            }
        }
        public Sys() : base("sys", new List<InnerFunctionNode>())
        {
            base.actions.Add(new SysValue());
        }
    }
}
