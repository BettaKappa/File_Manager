using System.Diagnostics;
using WindowsInput.Native;
using WindowsInput;
using static System.Console;
using static System.ConsoleKey;
using Process = System.Diagnostics.Process;

namespace Norton_Commander
{
    internal class Program
    {
        static int position = 2;
        static void Main()
        {
            BackgroundColor = ConsoleColor.DarkBlue;
            ForegroundColor = ConsoleColor.Gray;
            CursorVisible = false;
            ImitateSpasebarPress();
            ShowDriveInfo();
        }

        private static void ShowDriveInfo()
        {
            position = 2;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            var key = ReadKey();
            while (key.Key != Enter)
            {
                Clear();
                WriteLine("   Drive\tFile system\tTotal size\tAvailable space\n" +
                          "   ------------------------------------------------------------");
                foreach (DriveInfo disk in allDrives)
                {
                    WriteLine("   " + disk.Name +
                              "\t\t" + disk.DriveFormat +
                              "\t\t" + disk.TotalSize / 1073741824 + " GB" +
                              "\t\t" + disk.TotalFreeSpace / 1073741824 + " GB");
                }

                key = Arrow(key, ref position);


            }

            Clear();
            ImitateSpasebarPress();
            ShowDirectoryContent(allDrives[position - 2].Name);
        }

        static void ShowDirectoryContent(string Path)    
        {
            position = 2;
            WriteLine("   " + "File Name" + "\t\t\t   " + "Last Write Time");
            WriteLine("   " + "---------------------------------------------------");

            string[] allDirectories = Directory.GetDirectories(Path);
            int b = 2;
            foreach (string directoryPath in allDirectories)
            {
                string dirName = new DirectoryInfo(directoryPath).Name;
                DateTime dateTime = File.GetLastWriteTime(directoryPath);

                Write("   " + dirName);
                SetCursorPosition(35, b);
                b++;
                WriteLine(dateTime);
            }

            string[] allFiles = Directory.GetFiles(Path);
            foreach (string filePath in allFiles)
            {
                string fileName = new FileInfo(filePath).Name;
                DateTime dateTime = File.GetLastWriteTime(filePath);
                
                Write("   " + fileName);
                SetCursorPosition(35, b);
                b++;
                WriteLine(dateTime);
            }

            var key = ReadKey(true);
            while (key.Key != Enter)
            {
                for (int i = 0; i < (allFiles.Length + allDirectories.Length + 2); i++)
                {
                    SetCursorPosition(0, i);
                    Write("   ");
                }

                key = Arrow(key, ref position);
            }

            Clear();
            ImitateSpasebarPress();
            if (position < allDirectories.Length + 2)
            {
                ShowDirectoryContent(allDirectories[position - 2]);
            }
            else
            {
                Process.Start(new ProcessStartInfo {FileName = allFiles[position - (allDirectories.Length + 2)], UseShellExecute = true });
            }
        }

        private static ConsoleKeyInfo Arrow(ConsoleKeyInfo key, ref int position)
        {
            switch (key.Key)
            {
                case UpArrow:
                    position--;
                    break;
                case DownArrow:
                    position++;
                    break;
                case LeftArrow:
                    ImitateSpasebarPress();
                    ShowDriveInfo();
                    break;
                case Escape:
                    Clear();
                    WriteLine("Вы вышли" + "\n" + "САМ ТЫ Ы");
                    Environment.Exit(Environment.ExitCode);
                    break;
            }

            SetCursorPosition(0, position);
            WriteLine(" ► ");
            key = ReadKey();
            return key;
        }

        static void ImitateSpasebarPress()
        {
            InputSimulator spacebar = new();
            spacebar.Keyboard.KeyPress(VirtualKeyCode.SPACE);
        }
    } 
}