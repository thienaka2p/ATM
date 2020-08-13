using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMBANK
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedList<Admin> ListAdmin = Admin.GetFile();
            LinkedList<TheTu> ListTheTu = TheTu.GetFile();

            
            mainMenu:
            switch (Menu.MainMenu())
            {
                case 1:
                loginAdminMenu:
                    bool adminLogin = Menu.LoginAdminMenu(ListAdmin);
                adminMenu:
                    if (adminLogin)
                        switch (Menu.AdminMenu())
                        {
                            case 1:
                                Admin.RenderAccount(ListTheTu, "");
                                BoTro.PressKeyToExit();
                                goto adminMenu;
                            case 2:
                                Admin.CreateAccount(ListTheTu);
                                BoTro.PressKeyToExit();
                                goto adminMenu;
                            case 3:
                                Admin.XoaTaiKhoan(ListTheTu);
                                BoTro.PressKeyToExit();
                                goto adminMenu;
                            case 4:
                                Admin.UnLockAccount(ListTheTu);
                                BoTro.PressKeyToExit();
                                goto adminMenu;
                            default:
                                BoTro.Waiting(true, "Dang Xuat Thanh Cong!", "");
                                goto mainMenu;
                        }
                    else
                        goto loginAdminMenu;
                case 2:
                    User userLogin = Menu.LoginUserMenu(ListTheTu);
                userMenu:
                    switch (Menu.UserMenu())
                    {
                        case 1:
                            User.ShowInfo(userLogin);
                            BoTro.PressKeyToExit();
                            goto userMenu;
                        case 2:
                            User.RutTien(userLogin);
                            BoTro.PressKeyToExit();
                            goto userMenu;
                        case 3:
                            User.ChuyenTien(userLogin);
                            BoTro.PressKeyToExit();
                            goto userMenu;
                        case 4:
                            User.GiaoDien(userLogin);
                            BoTro.PressKeyToExit();
                            goto userMenu;
                        case 5:
                            User.DoiMaPin(ListTheTu, userLogin);
                            BoTro.PressKeyToExit();
                            goto userMenu;
                        default:
                            BoTro.Waiting(true, "Dang Xuat Thanh Cong!", "");
                            goto mainMenu;
                    }
                case 3:
                    BoTro.PressKeyToExit();
                    goto mainMenu;
                default:
                    break;
            }
            
        }
    }
}
