using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;

namespace injecc
{
    public partial class injecc
    {

        #region kernel32

        [DllImport("kernel32")]
        public static extern IntPtr CreateRemoteThread(
        IntPtr hProcess,
        IntPtr lpThreadAttributes,
        uint dwStackSize,
        UIntPtr lpStartAddress, // raw Pointer into remote process
        IntPtr lpParameter,
        uint dwCreationFlags,
        out IntPtr lpThreadId
        );

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(
            UInt32 dwDesiredAccess,
            Int32 bInheritHandle,
            Int32 dwProcessId
            );

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(
        IntPtr hObject
        );

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            UIntPtr dwSize,
            uint dwFreeType
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
        public static extern UIntPtr GetProcAddress(
            IntPtr hModule,
            string procName
            );

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect
            );

        [DllImport("kernel32.dll")]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            string lpBuffer,
            UIntPtr nSize,
            out IntPtr lpNumberOfBytesWritten
        );

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(
            string lpModuleName
            );

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        internal static extern Int32 WaitForSingleObject(
            IntPtr handle,
            Int32 milliseconds
            );

        public static Int32 GetProcessId(String proc)
        {
            Process[] ProcList;
            ProcList = Process.GetProcessesByName(proc);
            return ProcList[0].Id;
        }

        public static void InjectDLL(IntPtr hProcess, String strDLLName)
        {
            IntPtr bytesout;

            // Length of string containing the DLL file name +1 byte padding
            Int32 LenWrite = strDLLName.Length + 1;
            // Allocate memory within the virtual address space of the target process
            IntPtr AllocMem = (IntPtr)VirtualAllocEx(hProcess, (IntPtr)null, (uint)LenWrite, 0x1000, 0x40); //allocation pour WriteProcessMemory

            // Write DLL file name to allocated memory in target process
            WriteProcessMemory(hProcess, AllocMem, strDLLName, (UIntPtr)LenWrite, out bytesout);
            // Function pointer "Injector"
            UIntPtr Injector = (UIntPtr)GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            if (Injector == null)
            {
                Console.WriteLine("Injector Error!");
                // return failed
                return;
            }

            // Create thread in target process, and store handle in hThread
            IntPtr hThread = (IntPtr)CreateRemoteThread(hProcess, (IntPtr)null, 0, Injector, AllocMem, 0, out bytesout);
            // Make sure thread handle is valid
            if (hThread == null)
            {
                //incorrect thread handle ... return failed
                Console.WriteLine("Failed to create thread...");
                return;
            }
            // // Time-out is 10 seconds...
            // int Result = WaitForSingleObject(hThread, 10 * 1000);
            // // Check whether thread timed out...
            // if (Result == 0x00000080L || Result == 0x00000102L || Result == 0xFFFFFFFF)
            // {
            //     /* Thread timed out... */
            //     Console.WriteLine("hThread [2] Error!");
            //     // Make sure thread handle is valid before closing... prevents crashes.
            //     if (hThread != null)
            //     {
            //         //Close thread in target process
            //         CloseHandle(hThread);
            //     }
            //     return;
            // }
            // // Sleep thread for 1 second
            // Thread.Sleep(1000);
            // // Clear up allocated space ( Allocmem )
            // VirtualFreeEx(hProcess, AllocMem, (UIntPtr)0, 0x8000);
            // // Make sure thread handle is valid before closing... prevents crashes.
            // if (hThread != null)
            // {
            //     //Close thread in target process
            //     CloseHandle(hThread);
            // }

            // return succeeded
            Console.WriteLine("Injection successful!");
            return;
        }

        #endregion

        private static void worker(object sender, DoWorkEventArgs e)
        {
            Process.Start(exe.FullName);
        }

        private static FileInfo exe = null;

        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("USAGE: injecc <programName(NoExtension)/ProcID> <lib1.dll,lib2.dll,...>");
                return 1;
            }
            else if (args.Length == 1)
            {
                if (args[0] == "--help")
                {
                    Console.WriteLine("USAGE: injecc <programName(NoExtension)/ProcID> <lib1.dll,lib2.dll,...>");
                    return 0;
                }
            }

            Int32 ProcID = 0;

            if (!File.Exists(args[0]))
            {
                if (int.TryParse(args[0], out ProcID) == false)
                {
                    try
                    {
                        
                        ProcID = GetProcessId(args[0]);
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Console.WriteLine("process not found...");
                        Console.WriteLine("exiting...");
                        return 1;
                    }
                }
            }
            else
            {
                //start application
                exe = new FileInfo(args[0]);
                Console.WriteLine("starting application... " + exe.FullName);
                BackgroundWorker bgwrkr = new BackgroundWorker();
                bgwrkr.DoWork += new DoWorkEventHandler(worker);
                bgwrkr.RunWorkerAsync();
                Console.WriteLine("application started");
                Console.WriteLine("sleep 1000ms");
                Thread.Sleep(1000);
                ProcID = GetProcessId(exe.Name.Replace(exe.Extension, ""));
            }
            
            Console.WriteLine("process ID: " + ProcID);
            
            if (ProcID >= 0)
            {
                Console.WriteLine("getting process handle...");
                IntPtr hProcess = (IntPtr)OpenProcess(0x1F0FFF, 1, ProcID); //PROCESS_ALL_ACCESS:=0x1F0FFF

                if (hProcess == null)
                {
                    Console.WriteLine("no process found...");
                    Console.WriteLine("exiting...");
                    return 1;
                }
                else
                {
                    string dll = args[1];

                    if (dll.Contains(","))
                    {
                        string[] ent = dll.Split(',');

                        foreach (string a in ent)
                        {
                            FileInfo new_dll = new FileInfo(a);

                            Console.WriteLine("Injecting... " + new_dll.FullName);
                            InjectDLL(hProcess, new_dll.FullName);
                        }
                    }
                    else
                    {
                        FileInfo new_dll = new FileInfo(dll);

                        Console.WriteLine("Injecting... " + new_dll.FullName);
                        InjectDLL(hProcess, new_dll.FullName);
                    }
                }
            }

            Console.WriteLine("exiting...");
            return 0;
        }
    }
}
