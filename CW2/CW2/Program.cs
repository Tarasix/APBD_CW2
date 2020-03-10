using System;
using System.IO;

namespace CW2
{
    class Program
    {
        static void Main(string[] args)
        {

            // if(args[0] == null )
            //{
            //  args[0] = "data.csv";
            //args[1] = "result.xml";
            //args[2] = "xml";
            //}
            string path = @"Data\dane.csv";
            Console.WriteLine("Hello World");

            var plik = new FileInfo(path);
            using (var stream = new StreamReader(plik.OpenRead()))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] info = line.Split(',');
                    Console.WriteLine(line);
                    
                }
            }

            //if (File.Exists(args[1]))
            //{
            //    FileStream create = File.Open(args[1],FileMode.Create);
            //}


           // if(!Directory.Exists(args[0]))




        }
    }
}
