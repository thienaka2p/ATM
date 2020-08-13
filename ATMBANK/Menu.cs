using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ATMBANK
{
    class Menu
    {
        public static int MainMenu()
        {
            int select;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("*********************************");
                Console.Write("*");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\tBank ATM\t");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t*");
                Console.WriteLine("*********************************");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\t1. Dang Nhap Admin");
                Console.WriteLine("\t2. Dang Nhap User");
                Console.WriteLine("\t3. Thoat");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("*********************************");
                select = InputSelect();
            } while (select <= 0 || select > 4);
            return select;
        }

        public static int AdminMenu()
        {
            int select;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("*********************************");
                Console.Write("*");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\tDA DN - ADMIN\t");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t*");
                Console.WriteLine("*********************************");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\t1. Xem danh sach tai khoan");
                Console.WriteLine("\t2. Them tai khoan");
                Console.WriteLine("\t3. Xoa tai khoan");
                Console.WriteLine("\t4. Mo khoa tai khoan");
                Console.WriteLine("\t5. -> Dang Xuat");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("*********************************");
                select = InputSelect();
            } while (select <= 0 || select > 5);
            return select;
        }

        public static int UserMenu()
        {
            int select;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("*********************************");
                Console.Write("*");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\tDA DN - USER\t");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t*");
                Console.WriteLine("*********************************");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\t1. Xem thong tin tai khoan");
                Console.WriteLine("\t2. Rut tien");
                Console.WriteLine("\t3. Chuyen Tien");
                Console.WriteLine("\t4. Xem noi dung giao dich");
                Console.WriteLine("\t5. Doi ma pin");
                Console.WriteLine("\t6. -> Dang Xuat");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("*********************************");
                select = InputSelect();
            } while (select <= 0 || select > 6);
            return select;
        }

        
        public static bool LoginAdminMenu(LinkedList<Admin> ListAdmin)
        {
            string user, pass;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*********************************");
            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\tDANG NHAP ADMIN\t");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t*");
            Console.WriteLine("*********************************");
            // user
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("User:\t");
            Console.ResetColor();
            user = Console.ReadLine();
            // pass
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Pass:\t");
            Console.ResetColor();
            pass = BoTro.HidePass();
            Console.WriteLine();

            bool status = false;
            // checkpass
            for (LinkedListNode<Admin> p = ListAdmin.First; p != null; p = p.Next)
            {
                if (user == p.Value.Username && pass == p.Value.Pass)
                {
                    status = true; break;
                }
            }
            BoTro.Waiting(status, "Dang nhap thanh cong!", "Tai khoan hoac mat khau khong chinh xac!");
            return status;
        }

        public static User LoginUserMenu(LinkedList<TheTu> ListTheTu)
        {
        back:
            long id; int pin;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("*********************************");
            Console.Write("*");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\tDANG NHAP USER\t");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t*");
            Console.WriteLine("*********************************");
            // user
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("ID:\t");
            Console.ResetColor();
            long.TryParse(Console.ReadLine(), out id);
            // pass
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Pin:\t");
            Console.ResetColor();
            int.TryParse(BoTro.HidePass(), out pin);
            Console.WriteLine();

            bool status = false;
            // checkpass
            for (LinkedListNode<TheTu> p = ListTheTu.First; p != null; p = p.Next)
            {
                if (id == p.Value.Id)
                {
                    if (p.Value.Locked)
                    {
                        BoTro.Waiting(false, "", "Tai khoan nay da bi khoa!");
                        break;
                    }
                    else if (p.Value.Wrong < 2)
                    {
                        if (pin == p.Value.Pin)
                        {
                            status = true;
                            BoTro.Waiting(true, "Dang nhap thanh cong!", "");
                        }
                        else
                        {
                            p.Value.Wrong++;
                            BoTro.Waiting(false, "", "Tai khoan hoac mat khau khong chinh xac!");
                        }
                    }
                    else
                    {
                        p.Value.Locked = true;
                        TheTu.SaveFile(ListTheTu);
                        BoTro.Waiting(false, "", "Tai khoan bi khoa do nhap sai qua 3 lan!");
                        Thread.Sleep(1000);
                    }
                    break;
                }
            }
            if (status)
                return User.GetFile(id);
            else goto back;
        }

        static int InputSelect()
        {
            int select;
            // select
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Enter key: ");
            Console.ResetColor();
            int.TryParse(Console.ReadLine(), out select);
            return select;
        }
    }
}
