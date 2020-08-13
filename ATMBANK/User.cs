using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace ATMBANK
{
    class User : TheTu
    {
        // fields
        private string _name;
        private int _balance;
        private string _currency;

        // properties
        public string Name { get => _name; set => _name = value; }
        public int Balance { get => _balance; set => _balance = value; }
        public string Currency { get => _currency; set => _currency = value; }

        // constructor
        public User() : base()
        {
            _name = "";
            _balance = 0;
            _currency = "";
        }

        public User(long id, string name, int balance, string currency) : base(id)
        {
            _name = name;
            _balance = balance;
            _currency = currency;
        }

        // methods
        public static void SaveFile(User user)
        {
            // save file
            string path = $"{user.Id}.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine($"{user.Id}#{user.Name}#{user.Balance}#{user.Currency}");
            }
        }

        public static User GetFile(long id)
        {
            string path = $"{id}.txt";
            try
            {
                using (StreamReader rd = new StreamReader(path))
                {
                    string line = rd.ReadLine();
                    string[] seperator = new string[] { "#" };
                    string[] arr = line.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                    User user = new User(id, arr[1], Convert.ToInt32(arr[2]), arr[3]);
                    return user;
                }
            }
            catch (Exception)
            {
                LinkedList<TheTu> ListTheTu = TheTu.GetFile();
                for (LinkedListNode<TheTu> p = ListTheTu.First; p != null; p = p.Next)
                    if (p.Value.Id == id)
                    {
                        ListTheTu.Remove(p.Value);
                        SaveFile(ListTheTu);
                    }
                File.Delete($"D:/LichSu{id}.txt");
                return new User();
                throw;
            }
        }

        public static void ShowInfo(User user)
        {
            Console.WriteLine($"\t- ID: {user.Id}");
            Console.WriteLine($"\t- Chu tai khoan: {user.Name}");
            Console.WriteLine($"\t- So du: {user.Balance}");
            Console.WriteLine($"\t- Loai tien te: {user.Currency}");
        }

        public static void RutTien(User user)
        {
            Console.Write("\t- Nhap so tien can rut: ");
            int money; int.TryParse(Console.ReadLine(), out money);
            if (money >= 50000)
            {
                if (user.Balance - money >= 50000)
                {
                    if (money % 50 == 0)
                    {
                        Console.Write($"\nBan chac chan muon rut {money} {user.Currency}? (Y/N)");
                        string check = Console.ReadLine();

                        if (check == "Y" || check == "y" || check == "Yes")
                        {
                            user.Balance -= money;
                            User.SaveFile(user);
                            GiaoDich.SaveFile(user, "Rut Tien", money, 0);
                            BoTro.Waiting(true, "Rut tien thanh cong!", "");
                        }
                        else BoTro.Waiting(false, "", "Rut tien that bai!");
                    }
                    else BoTro.Waiting(false, "", "So tien nhap phai la boi so cua 50!");
                }
                else BoTro.Waiting(false, "", "Ban khong du tien!");
            }
            else BoTro.Waiting(false, "", "Ban khong the rut duoi 50.000!");
        }

        public static void ChuyenTien(User user)
        {
            Console.Write("\t- Nhap id tai khoan muon chuyen: ");
            long id; long.TryParse(Console.ReadLine(), out id);
            Console.Write("\t- Nhap so tien can chuyen: ");
            int money; int.TryParse(Console.ReadLine(), out money);

            if (money >= 50000)
            {
                if (money >= 50000 && user.Balance - money >= 50000)
                {
                    if (money % 50 == 0)
                    {
                        User userReceive = User.GetFile(id);
                        if (userReceive.Id != 0)
                        {
                            Console.Write($"\nBan chac chan muon chuyen {money} {user.Currency} cho so tai khoan " +
                                $"{userReceive.Id} - {userReceive.Name}? (Y/N)");
                            string check = Console.ReadLine();

                            if (check == "Y" || check == "y" || check == "Yes")
                            {
                                user.Balance -= money;
                                userReceive.Balance += money;
                                User.SaveFile(userReceive);
                                User.SaveFile(user);
                                GiaoDich.SaveFile(userReceive, "Nhan Tien", money, user.Id);
                                GiaoDich.SaveFile(user, "Chuyen Tien", money, userReceive.Id);
                                BoTro.Waiting(true, "Chuyen tien thanh cong!", "");
                            }
                            else
                            {
                                BoTro.Waiting(false, "", "Chuyen tien that bai!");
                            }

                        }
                        else
                        {
                            BoTro.Waiting(false, "", "Khong tim thay nguoi dung nay!");
                        }

                    }
                    else
                    {
                        BoTro.Waiting(false, "", "So tien nhap phai la boi so cua 50!");
                    }

                }
                else
                {
                    BoTro.Waiting(false, "", "Ban khong du tien!");
                }

            }
            else
            {
                BoTro.Waiting(false, "", "Ban khong the rut duoi 50.000!");
            }

        }

        public static void GiaoDien(User user)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"\tGiao Dich");
            Console.Write($"\tSo Tien");
            Console.WriteLine($"\t\tThoi Gian");
            Console.ResetColor();
            LinkedList<GiaoDich> ListGiaoDich = GiaoDich.GetFile(user.Id);
            for (LinkedListNode<GiaoDich> p = ListGiaoDich.First; p != null; p = p.Next)
            {
                string IdTf = p.Value.IdTf != 0 ? $"\t({p.Value.IdTf})\n" : "";
                Console.Write($"\t{p.Value.Type}");
                Console.Write($"\t{p.Value.Amount}");
                Console.WriteLine($"\t\t{p.Value.Time}");
                Console.Write($"{IdTf}");
            }
        }

        public static void DoiMaPin(LinkedList<TheTu> ListTheTu, User user)
        {
            for (LinkedListNode<TheTu> p = ListTheTu.First; p != null; p = p.Next)
            {
                if (p.Value.Id == user.Id)
                {
                    Console.Write("\t- Nhap ma pin cu: ");
                    int oldPin; int.TryParse(BoTro.HidePass(), out oldPin);
                    Console.Write("\n\t- Nhap ma pin moi: ");
                    int newPin; int.TryParse(BoTro.HidePass(), out newPin);
                    Console.Write("\n\t- Nhap lai ma pin moi: ");
                    int reNewPin; int.TryParse(BoTro.HidePass(), out reNewPin);
                    if (oldPin == p.Value.Pin)
                    {
                        if (newPin == reNewPin)
                        {
                            p.Value.Pin = newPin;
                            User.SaveFile(ListTheTu);
                            BoTro.Waiting(true, "Doi ma pin thanh cong!", "");
                        }
                        else BoTro.Waiting(false, "", "Nhap ma pin khong khop!");
                    }
                    else BoTro.Waiting(false, "", "Nhap sai ma pin!");
                    break;
                }
            }
        }
    }
}
