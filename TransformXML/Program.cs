using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Xsl;

namespace TransformXML
{
    // Created By: Ghiath Al-Qaisi
    // Date: 2016-08-28
    class Program
    {
        // Variables to store arguments input values.
        public static string XmlFilePath;
        public static string XsltFileName;

        static int Main(string[] args)
        {
            // Parameters have to be provided.
            if (!args.Any())
            {
                Console.WriteLine();
                Console.WriteLine(@"Usage: TransformXML.exe [Mandatory: files Path] [Mandatory: xslt file path]");
                Console.WriteLine("Example: TransformXML.exe \"C:\\FilesToMerge\" \"XSLT/XSLTIndent.xslt\"");
                Console.WriteLine();
                return 1;
            }

            // Show help
            if (args[0] == "-h" || args[0] == "-help" || args[0] == "help")
            {
                Console.WriteLine();
                Console.WriteLine(@"Usage: TransformXML.exe [Mandatory: files Path] [Mandatory: xslt file path]");
                Console.WriteLine("Example: TransformXML.exe \"C:\\FilesToMerge\" \"XSLT/XSLTIndent.xslt\"");
                Console.WriteLine();
                return 0;
            }

            // Test if input arguments were supplied:
            if (args.Length < 2)
            {
                Console.WriteLine();
                Console.WriteLine(@"Please enter the required parameters:");
                Console.WriteLine(@"Usage: TransformXML.exe [Mandatory: files Path] [Mandatory: xslt file path]");
                Console.WriteLine("Example: TransformXML.exe \"C:\\FilesToMerge\" \"XSLT/XSLTIndent.xslt\"");
                Console.WriteLine();
                return 1;
            }

            // Assign input parameter values
            XmlFilePath = args[0];
            if (!Directory.Exists(XmlFilePath))
            {
                Print("The provided file path does not exists..!", 'e');
                return 1;
            }

            XsltFileName = args[1];

            if (!File.Exists(XsltFileName))
            {
                Print("The provided XSLT file path does not exists..!", 'e');
                return 1;
            }

            // Remove the slash at the end of the path string if there where any
            var fullPath = XmlFilePath.TrimEnd(Path.DirectorySeparatorChar);

            try
            {
                // Get the file name list
                var xmlFiles = Directory.EnumerateFiles(fullPath, "*.xml");

                // Check if there were files to merege in the provided path
                var files = xmlFiles as string[] ?? xmlFiles.ToArray();
                if (!files.Any())
                {
                    Print("No files to be merged found..!", 'e');
                    return 1;
                }

                Print(files.Count() + " files to transform found..!", 's');

                // Loop through the other files in folder to merge
                foreach (var currentFile in files)
                {
                    Print("Cleaning " + currentFile + " File...", 'i');

                    // Cleanup the file before merging it.
                    CleanupXmlFile(currentFile);
                    Print(currentFile + " File Cleaned...", 'o');

                    // Transform the current file if XSLT file has been provided
                    if (!string.IsNullOrEmpty(XsltFileName))
                    {
                        XsltTransformXml(currentFile);
                        Print(currentFile + " File Transformed...", 'o');
                    }
                }
            }
            catch (Exception e)
            {
                Print(e.Message, 'e');
                return 1;
            }

            Print("Done!", 's');
            return 0;
        }

        //re-create the file making sure that xml declaration is the first char in file.
        public static void CleanupXmlFile(string filePath)
        {
            Console.WriteLine("Cleaning " + filePath + " File...");
            var lines = new List<string>();

            // Read the file line by line into list
            using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (TextReader reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    lines.Add(line);
            }

            // Find the line containing the XML Definition
            var i = lines.FindIndex(s => s.StartsWith("<?xml"));

            // Move the xml definition line to top of the file
            var xmlLine = lines[i].Trim();
            lines.RemoveAt(i);
            lines.Insert(0, xmlLine);

            // Write the file back to disk
            using (var fileStream = File.Open(filePath, FileMode.Truncate, FileAccess.Write))
            using (TextWriter writer = new StreamWriter(fileStream))
            {
                foreach (var line in lines)

                    writer.Write(line.Trim());

                writer.Flush();
            }
        }

        // Transform XML file using the provided XSLT file.
        public static void XsltTransformXml(string filePath)
        {
            Console.WriteLine("Transforming " + filePath + " File...");
            var myXslTransform = new XslTransform();

            // Load the XSLT File
            myXslTransform.Load(XsltFileName);

            // Transform the XML file
            myXslTransform.Transform(filePath, filePath);
        }

        // Custom console text printer
        public static void Print(string text, char type)
        {
            // i: Information: Magenta
            // e: Error: Red
            // s: Success: Green

            switch (type)
            {
                case 'i':
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(text);
                    Console.ResetColor();
                    break;
                case 'e':
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(text);
                    Console.ResetColor();
                    break;
                case 's':
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(text);
                    Console.ResetColor();
                    break;
                default:
                    Console.WriteLine(text);
                    break;
            }
        }
    }
}
