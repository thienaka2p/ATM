using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ATMBANK
{
    class BoTro
    {
        
            public static void Waiting(bool status, string alertTrue, string alertFalse)
            {
                Console.Write("\nVui long cho mot chut");
                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(300);
                    Console.Write(".");
                }
                Thread.Sleep(500);
                Console.WriteLine();
                if (status)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(alertTrue);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(alertFalse);
                    Console.ResetColor();
                }
                Thread.Sleep(800);
            }

            public static string HidePass()
            {
                string pass = "";
                do
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                    {
                        pass += key.KeyChar;
                        Console.Write("*");
                    }
                    else
                    {
                        if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                        {
                            pass = pass.Substring(0, (pass.Length - 1));
                            Console.Write("\b \b");
                        }
                        else if (key.Key == ConsoleKey.Enter)
                        {
                            break;
                        }
                    }
                } while (true);
                return pass;
            }

            public static long RandomID(LinkedList<TheTu> ListTheTu, int length)
            {
                long id; bool status;
                do
                {
                    status = true;
                    id = RandomNumber(length);
                    for (LinkedListNode<TheTu> p = ListTheTu.First; p != null; p = p.Next)
                    {
                        if (p.Value.Id == id)
                        {
                            status = false; break;
                        }
                    }
                } while (!status);
                return id;
            }

            public static long RandomNumber(int length)
            {
                string sNumber = "";
                long lNumber;
                Random rd = new Random();
                for (int i = 0; i < length; i++)
                {
                    sNumber += rd.Next(1, 9);
                }
                long.TryParse(sNumber, out lNumber);
                return lNumber;
            }

            public static void PressKeyToExit()
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("\nNhan phim Enter de thoat. ");
                Console.ResetColor();
                Console.ReadLine();
            }
        
    }
}
