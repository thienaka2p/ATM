using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ATMBANK
{
    class Admin
    {
        // fields
        private string _user;
        private string _pass;

        // properties
        public string Username { get => _user; set => _user = value; }
        public string Pass { get => _pass; set => _pass = value; }

        // constructor
        public Admin()
        {
            _user = "";
            _pass = "";
        }

        public Admin(string user, string pass)
        {
            _user = user;
            _pass = pass;
        }

        // methods
        public static LinkedList<Admin> GetFile()
        {
        back:
            try
            {
                string path = "Admin.txt";
                LinkedList<Admin> ListAdmin = new LinkedList<Admin>();
                using (StreamReader rd = new StreamReader(path))
                {
                    int spt = Convert.ToInt32(rd.ReadLine());
                    for (int i = 0; i < spt; i++)
                    {
                        string line = rd.ReadLine();
                        string[] seperator = new string[] { "#" };
                        string[] arr = line.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                        Admin admin = new Admin(arr[0], arr[1]);
                        ListAdmin.AddLast(admin);
                    }
                }
                return ListAdmin;
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.ResetColor();
                Console.Write("Nhap username: ");
                string username = Console.ReadLine();
                Console.Write("Nhap password: ");
                string password = Console.ReadLine();
                BoTro.Waiting(true, "Tao tai khoan thanh cong!", "");

                string path = "Admin.txt";
                LinkedList<Admin> ListAdmin = new LinkedList<Admin>();
                ListAdmin.AddLast(new Admin(username, password));
                ListAdmin.AddLast(new Admin($"{username}2", password));
                ListAdmin.AddLast(new Admin($"{username}3", password));
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine(ListAdmin.Count);
                    for (LinkedListNode<Admin> p = ListAdmin.First; p != null; p = p.Next)
                        sw.WriteLine($"{p.Value.Username}#{p.Value.Pass}");
                };
                goto back;
                throw;
            }
        }

        public static void RenderAccount(LinkedList<TheTu> ListTheTu, string status)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t{0,-20}{1,-25}{2,-20}{3,-20}{4,-20}", "ID", "Name", "So Du", "Loai Tien", "Trang Thai");
            Console.ResetColor();
            for (LinkedListNode<TheTu> p = ListTheTu.First; p != null; p = p.Next)
            {
                User user = User.GetFile(p.Value.Id);
                string locked = p.Value.Locked ? "Bi Khoa" : "Su Dung";
                if (user.Id != 0)
                {
                    if (status == "locked")
                    {
                        if (p.Value.Locked)
                            Console.WriteLine("\t{0,-20}{1,-25}{2,-20}{3,-20}{4,-20}", user.Id, user.Name, user.Balance, user.Currency, locked);
                    }
                    else Console.WriteLine("\t{0,-20}{1,-25}{2,-20}{3,-20}{4,-20}", user.Id, user.Name, user.Balance, user.Currency, locked);
                }
            }
        }

        public static void CreateAccount(LinkedList<TheTu> ListTheTu)
        {
            int pin = 123456;
            User user = CreateUser(ListTheTu);
            TheTu card = new TheTu(user.Id, pin, false);
            ListTheTu.AddLast(card);

            TheTu.SaveFile(ListTheTu);
            BoTro.Waiting(true, $"Tao tai khoan {user.Id} - " +
                $"{user.Name} - " +
                $"{user.Balance} {user.Currency} thanh cong!", "");
        }

        public static void XoaTaiKhoan(LinkedList<TheTu> ListTheTu)
        {
            RenderAccount(ListTheTu, "");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nNhap ID can xoa: ");
            Console.ResetColor();
            long id; long.TryParse(Console.ReadLine(), out id);
            bool status = false;

            // delete
            for (LinkedListNode<TheTu> p = ListTheTu.First; p != null; p = p.Next)
            {
                if (p.Value.Id == id)
                {
                    status = true;
                    ListTheTu.Remove(p);
                    // delete file
                    File.Delete($"{p.Value.Id}.txt");
                    File.Delete($"LichSu{p.Value.Id}.txt");
                    TheTu.SaveFile(ListTheTu);
                    break;
                }
            }
            BoTro.Waiting(status, "Xoa Thanh Cong!", "Khong tim thay ID nay!");
        }

        public static void UnLockAccount(LinkedList<TheTu> ListTheTu)
        {
            RenderAccount(ListTheTu, "locked");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nNhap ID can unlock: ");
            Console.ResetColor();
            long id; long.TryParse(Console.ReadLine(), out id);
            bool status = false;

            // unlock
            for (LinkedListNode<TheTu> p = ListTheTu.First; p != null; p = p.Next)
            {
                if (p.Value.Id == id)
                {
                    status = true;
                    p.Value.Locked = false;
                    TheTu.SaveFile(ListTheTu);
                    break;
                }
            }
            BoTro.Waiting(status, "Mo khoa Thanh Cong!", "Khong tim thay ID nay!");
        }

        static User CreateUser(LinkedList<TheTu> ListTheTu)
        {
            int balance;
            string name, currency;
            long id = BoTro.RandomID(ListTheTu, 14);
            do
            {
                Console.Write("\t- Ten khach hang: ");
                name = Console.ReadLine();
            } while (name.Length == 0);
            do
            {
                Console.Write("\t- So du: ");
                int.TryParse(Console.ReadLine(), out balance);
            } while (balance == 0);
            do
            {
                Console.Write("\t- Loai tien te: ");
                currency = Console.ReadLine();
            } while (currency.Length == 0);

            User user = new User(id, name, balance, currency);

            // create file
            string userPath = $"{id}.txt";
            string historyUserPath = $"LichSu{id}.txt";
            using (StreamWriter sw = new StreamWriter(historyUserPath)) sw.WriteLine(0);
            using (StreamWriter sw = new StreamWriter(userPath))
                sw.WriteLine($"{user.Id}#{user.Name}#{user.Balance}#{user.Currency}");
            return user;
        }
    }
}
