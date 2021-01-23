using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace YMG
{
    public static class ramveri
    {
       
            //const int PROCESS_WM_READ = 0x0010;
            //[DllImport("kernel32.dll")]
            //public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

            //[DllImport("kernel32.dll")]
            //public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

           public static string veri() {

            //Process notePad = Process.Start("notepad");

            //// Get Process objects for all running instances on notepad.

            //IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, notePad.Id);

            //int bytesRead = 0;
            //byte[] buffer = new byte[5]; //'Hello World!' takes 12*2 bytes because of Unicode 

            //// 0x0046A3B8 is the address where I found the string, replace it with what you found
            //ReadProcessMemory((int)processHandle, 0X59000, buffer, buffer.Length, ref bytesRead);


            //string deger = processHandle.ToString(); /*Encoding.Unicode.GetString(buffer).ToString();*/
            //byte[] bytes = Encoding.Unicode.GetBytes(deger);
            //SHA256Managed hashstring = new SHA256Managed();
            //byte[] hash = hashstring.ComputeHash(bytes);
            //string hashString = string.Empty;
            //foreach (byte x in hash)
            //{
            //    hashString += String.Format("{0:x}", x);
            //}
            //notePad.Kill();
            //return hashString;

            
            unsafe
            {
                int x = 100;//Ramden 
                int* ptr = &x;
                IntPtr veri = (IntPtr)(int)ptr;
                long s = Marshal.ReadInt64(veri);
                string deger = s.ToString();
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(deger));

                      
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }

                    return builder.ToString();
                }
            }
           
            
        }


        }
}