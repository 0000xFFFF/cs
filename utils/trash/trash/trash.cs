using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace trash
{
    class trash
    {
        private static int Trash(string item)
        {
            Console.Write(item + " -- ");

            if (Directory.Exists(item))
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory
                    (
                        item,
                        Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin
                    );
                }
                catch (FileNotFoundException e)             {  Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (ArgumentNullException e)             {  Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (ArgumentException e)                 {  Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (PathTooLongException e)              {  Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (NotSupportedException e)             {  Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (System.Security.SecurityException e) {  Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (UnauthorizedAccessException e)       {  Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (IOException e)                       {  Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (Exception e)                         {  Console.WriteLine("ERROR: " + e.Message); return 1; }
            }
            else if (File.Exists(item))
            {
                try
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile
                    (
                        item,
                        Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                        Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin
                    );
                }
                catch (FileNotFoundException e)             { Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (ArgumentNullException e)             { Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (ArgumentException e)                 { Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (PathTooLongException e)              { Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (NotSupportedException e)             { Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (System.Security.SecurityException e) { Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (UnauthorizedAccessException e)       { Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (IOException e)                       { Console.WriteLine("ERROR: " + e.Message); return 1; }
                catch (Exception e)                         { Console.WriteLine("ERROR: " + e.Message); return 1; }
            }
            else
            {
                Console.WriteLine("not found");
                return 1;
            }
            
            Console.WriteLine("trashed");
            return 0;
        }
        
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("USAGE: trash <dir/file>");
                return 1;
            }

            int errors = 0;
            foreach(string item in args)
            {
                errors += Trash(item);
            }

            return errors;
        }
    }
}
