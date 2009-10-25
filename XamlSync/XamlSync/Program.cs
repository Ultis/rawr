using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace XamlSync
{
    class Program
    {
        static Dictionary<string, string> wpfMap = new Dictionary<string, string>();
        static Dictionary<string, string> slMap = new Dictionary<string, string>();
        static string vsComnTools;

        static void Main(string[] args)
        {
            slMap["xmlns:toolkit"] = "\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Toolkit\"";
	        slMap["xmlns:controls"] = "\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls\"";
	        slMap["xmlns:Rawr"] = "\"clr-namespace:Rawr;assembly=Rawr.Base\"";
            slMap["xmlns:basics"] = "\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls\"";
    	    slMap["xmlns:common"] = "\"clr-namespace:System.Windows;assembly=System.Windows.Controls\"";
            slMap["xmlns:data"] = "\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data\"";
            slMap["xmlns:dataInput"] = "\"clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Data.Input\"";

            wpfMap["xmlns:toolkit"] = "\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"";
	        wpfMap["xmlns:controls"] = "\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"";
            wpfMap["xmlns:Rawr"] = "\"clr-namespace:Rawr;assembly=Rawr.Base.WPF\"";
	        wpfMap["xmlns:basics"] = "\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"";
	        wpfMap["xmlns:common"] = "\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"";
            wpfMap["xmlns:data"] = "\"clr-namespace:Microsoft.Windows.Controls;assembly=WPFToolkit\"";
            wpfMap["xmlns:dataInput"] = "\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"";

            vsComnTools = Environment.GetEnvironmentVariable("VS90COMNTOOLS");

            string root = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(root, "*.xaml", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                if (!file.Contains("Rawr.WPF") && !file.Contains("Rawr.Silverlight"))
                {
                    if (file.EndsWith("WPF.xaml"))
                    {
                        string slfile = file.Remove(file.Length - 8) + "xaml";
                        if (!File.Exists(slfile))
                        {
                            GenerateSlXaml(file, slfile);
                        }
                        else
                        {
                            ProcessXaml(slfile, file);
                        }
                    }
                    else if (!file.EndsWith("i.xaml") && !file.EndsWith("Generic.xaml"))
                    {
                        string wpffile = file.Remove(file.Length - 4) + "WPF.xaml";
                        // only process if it doesn't exist, otherwise it was processed above already
                        if (!File.Exists(wpffile))
                        {
                            GenerateWpfXaml(file, wpffile);
                        }
                    }
                }
            }
        }

        public static void ProcessXaml(string slFile, string wpfFile)
        {
            // check if they are in sync
            StreamReader sl = new StreamReader(slFile);
            StreamReader wpf = new StreamReader(wpfFile);

            bool needsSync = false;

            string slLine;
            string wpfLine;

            do
            {
                slLine = sl.ReadLine();
                wpfLine = wpf.ReadLine();

                // could do something more sophisticated here, for now ignore any lines with xmlns in them
                if (slLine != null && !slLine.Contains("xmlns"))
                {
                    if (slLine != wpfLine)
                    {
                        needsSync = true;
                        break;
                    }
                }
            } while (slLine != null && wpfLine != null);
            if (slLine != null || wpfLine != null)
            {
                needsSync = true;
            }

            sl.Close();
            wpf.Close();

            if (needsSync)
            {
                // determine which one is newer
                if (File.GetLastWriteTime(slFile) > File.GetLastWriteTime(wpfFile))
                {
                    GenerateWpfXaml(slFile, wpfFile);
                }
                else
                {
                    GenerateSlXaml(wpfFile, slFile);
                }
            }
        }

        public static void GenerateSlXaml(string source, string target)
        {
            GenerateXaml(source, target, slMap);
        }

        public static void GenerateWpfXaml(string source, string target)
        {
            GenerateXaml(source, target, wpfMap);
        }

        public static string GetTempFileName(string extension) 
        {
            var path = Path.GetTempPath();
            var fileName = Guid.NewGuid().ToString() + extension;
            return Path.Combine(path, fileName);
        }

        public static void GenerateXaml(string source, string target, Dictionary<string, string> xmlnsMap)
        {
            string sourceText = File.ReadAllText(source);
            string targetText = Regex.Replace(sourceText, "(xmlns:\\w+)\\s*=\\s*(\"[^\"]+\")", m =>
                {
                    string xmlns = m.Groups[1].Value;
                    string name;
                    if (!xmlnsMap.TryGetValue(xmlns, out name))
                    {
                        name = m.Groups[2].Value;
                    }
                    return xmlns + "=" + name;
                });
            if (File.Exists(target))
            {
                FileInfo info = new FileInfo(target);
                if (info.IsReadOnly)
                {
                    if (vsComnTools != null)
                    {
                        // we're dealing with TFS
                        // try to checkout
                        string bat = GetTempFileName(".bat");
                        StreamWriter batsw = new StreamWriter(bat);
                        batsw.WriteLine("call \"%VS90COMNTOOLS%vsvars32.bat\"");
                        batsw.WriteLine("tf checkout \"" + target + "\"");
                        batsw.Close();

                        ProcessStartInfo p = new System.Diagnostics.ProcessStartInfo(bat);                        
                        Process proc = new System.Diagnostics.Process();
                        proc.StartInfo = p;

                        proc.Start();
                        proc.WaitForExit();

                        File.Delete(bat);
                    }
                    else
                    {
                        Console.WriteLine("Cannot modify " + target);
                        return;
                    }
                }
            }
            StreamWriter sw = new StreamWriter(target);
            sw.Write(targetText);
            sw.Close();
        }
    }
}
