using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using АИСТ.Class.algoritms;
using АИСТ.Class.Setttings;
using АИСТ.Class.SQL.Tab;
using АИСТ.Forms;

namespace АИСТ.Class
{
    static class Info
    {

        private static string bd_name = "";
        private static byte[] key = new byte[] { 7, 45, 146, 89, 255, 38, 95, 17 };
        private static byte[] IV = new byte[] { 17, 58, 246, 93, 235, 83, 129, 175 };
        private static DESCryptoServiceProvider desCrypto = new DESCryptoServiceProvider();
        private static string bd_con = "Не подключено";
        private static bool admin = false;
        private static Algoritm a = new Algoritm();
        private static Dictionary<string, List<Promo>> promos;
        private static Generate_Setttings gs;
        private static bool test = false;
        private static Dictionary<string, Tabs> tabs = new Dictionary<string, Tabs>();
        private static string email = "ESGdiplom2020shop@yandex.ru";
        private static string test_email = "ESGdiplom2020@yandex.ru";
        public static Collect_settings temp_settings = new Collect_settings();
        public static void set_email()
        {
            email = "ESGdiplom2020shop@yandex.ru";
        }
        public static Algoritm Get_alg()
        {
            return a;
        }
        public static Dictionary<string, List<Promo>> Get_prom()
        {
            return promos;
        }
        public static Dictionary<string, Tabs> Get_tabs()
        {
            return tabs;
        }

        public static void Set_tabs(Dictionary<string, Tabs> t)
        {
            tabs = t;
        }

        public static bool Is_test()
        {
            return test;
        }
        public static void Set_test(bool tests)
        {
            test = tests;
        }

        public static Generate_Setttings Get_settings()
        {
            return gs;
        }
        public static void Set_promo(Dictionary<string, List<Promo>> promo, Algoritm alg, Generate_Setttings settings)
        {
            a = alg;
            promos = promo;
            gs = settings;

        }
        public static bool Is_admin()
        {
            return admin;
        }
        public static void Set_admin(bool admin_v)
        {
            admin = admin_v;
        }

        /// <summary>
        /// просто возвращаем имя бд, косметика
        /// </summary>
        /// <returns></returns>
        public static string Get_bd_name()
        {
            return bd_name;
        }

        public static void Give_me_file()
        {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            Decrypt_File("info.txt", desCrypto);
        }

        public static void Take_you_file()
        {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            Encrypt_File("info.txt", desCrypto);
        }


        public static List<string> get_bd(bool already_encript)
        {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            //Encrypt_File("info.txt", desCrypto);
          //  Decrypt_File("info.txt", desCrypto);
            List<string> bd = new List<string>();
            string[] temp_info = File.ReadAllLines("info.txt");
            foreach (string s in temp_info)
            {
                bd.Add(s);
            }
            Encrypt_File("info.txt", desCrypto);
            return bd;
        }

        /// <summary>
        /// автозаполнение форм
        /// </summary>
        /// <returns></returns>
        public static List<string> get_bd()
        {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            //Encrypt_File("info.txt", desCrypto);
            Decrypt_File("info.txt", desCrypto);
            List<string> bd = new List<string>();
            string[] temp_info = File.ReadAllLines("info.txt");
            foreach (string s in temp_info)
            {
                bd.Add(s);
            }
            Encrypt_File("info.txt", desCrypto);
            return bd;
        }

        /// <summary>
        /// сохранения форм
        /// </summary>
        /// <param name="host"></param>
        /// <param name="name"></param>
        public static void Set_bd(string host, string name)
        {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            Decrypt_File("info.txt", desCrypto);
            List<string> bd = new List<string>();
            string[] temp_info = File.ReadAllLines("info.txt");
            bd_name = name;
            foreach (string s in temp_info)
            {
                if (s.Contains("server"))
                {
                    string tmp = "server = " + host;
                    bd.Add(tmp);
                }
                else if (s.Contains("database"))
                {
                    string tmp = "database = " + name;
                    bd.Add(tmp);
                }
                else bd.Add(s);

            }
            File.Delete("info.txt");
            File.WriteAllLines("info.txt", bd.ToArray());
            Encrypt_File("info.txt", desCrypto);
        }
        public static void Set_bd()
        {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            Encrypt_File("info.txt", desCrypto);
        }

        /// <summary>
        /// Шифрование
        /// </summary>
        /// <param name="sInputFilename"></param> имя файла, который шифруем
        /// <param name="sKey"></param> ключ
        static void Encrypt_File(string sInputFilename, DESCryptoServiceProvider sKey)
        {
            string sOutputFilename = "temp" + sInputFilename;
            FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);//читаем информацию
            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);//подготовливаем поток
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();//далем шифровальщик
            DES.Key = sKey.Key;
            DES.IV = sKey.IV;
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);//шифруем
            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);//записывем
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
            File.Delete(sInputFilename);//пересохраняем
            File.Move(sOutputFilename, sInputFilename);


        }
        static void Encrypt_File(string sInputFilename, DESCryptoServiceProvider sKey, string sOutputFilename)
        {
            FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);//читаем информацию
            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);//подготовливаем поток
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();//далем шифровальщик
            DES.Key = sKey.Key;
            DES.IV = sKey.IV;
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);//шифруем
            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);//записывем
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
            File.Delete(sInputFilename);//пересохраняем
            File.Move(sOutputFilename, sInputFilename);


        }

        /// <summary>
        /// Дешифрование
        /// </summary>
        /// <param name="sInputFilename"></param> имя файла, который дешифруем
        /// <param name="sKey"></param>ключ
        static void Decrypt_File(string sInputFilename, DESCryptoServiceProvider sKey)
        {
            FileStream fsread = null;
            CryptoStream cryptostreamDecr = null;
            FileStream fsDecrypted = null;
            try
            {
                string sOutputFilename = "temp" + sInputFilename;
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = sKey.Key;
                DES.IV = sKey.IV;
                fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);//поток для чтения зашифрованного файла
                ICryptoTransform desdecrypt = DES.CreateDecryptor();//создание расшифровщика
                cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read); //Поток для расшифрования
                fsDecrypted = new FileStream(sOutputFilename, FileMode.Create);//Поток для расшифрования
                byte[] bytearrayinput = new byte[1024];
                int length;
                do//расшифровываем
                {
                    length = cryptostreamDecr.Read(bytearrayinput, 0, 1024);
                    fsDecrypted.Write(bytearrayinput, 0, length);
                } while (length == 1024);
                fsDecrypted.Flush();//заливаем, пересохраняем
                fsread.Close();
                cryptostreamDecr.Close();
                fsDecrypted.Close();
                File.Delete(sInputFilename);
                File.Move(sOutputFilename, sInputFilename);
            }
            catch
            {
                try
                {
                    fsread.Close();
                }
                catch
                {
                    File.Create("info.txt");
                }
                fsDecrypted.Close();
                DialogResult result = MessageBox.Show(
                 "Сбросить настройки?",
                 "Ошибка импорта настроек",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                 MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                    Info.Set_defolt_file();
            }
        }
        public static void Set_defolt_file()
        {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            File.Delete("info.txt");
            File.Copy("F:\\учеба\\Вода(диплом)\\aists\\АИСТ\\Class\\Infos\\defolt_settings.txt", "info.txt");
            //  Encrypt_File("info.txt", desCrypto);
            //  Encrypt_File("F:\\учеба\\Вода(диплом)\\aists\\АИСТ\\Class\\Infos\\defolt_settings.txt", desCrypto, "info.txt");
        }

        public static void Update()
            {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            Decrypt_File("info.txt", desCrypto);
            List<string> bd = new List<string>();
            string[] temp_info = File.ReadAllLines("info.txt");
            Encrypt_File("info.txt", desCrypto);
            Dictionary<string, Tab> tabs = Tab_Settings.tabs;
            for (int i = 0; i < temp_info.Length; i++)
            {
                string s = temp_info[i];
                if (s.Contains("Tstart"))
                {
                    bd.Add(s);
                    i++;
                    string name_tab = temp_info[i].Split(' ')[0];
                    string s2 = name_tab + " = " + tabs[name_tab].tab_name;
                    bd.Add(s2);
                    i ++;
                    bd.Add(temp_info[i]);
                    i++;
                    while (!temp_info[i].Contains("Tend"))
                    {
                       string[] name = temp_info[i].Split(' ');
                        s2 = name[0] + " = " + tabs[name_tab].fields[name[0]];                
                        bd.Add(s2);
                        i++;
                    }
                    bd.Add(temp_info[i]);
                }
                else bd.Add(s);
            }
            Tab_Settings.Put_in_class();
            Decrypt_File("info.txt", desCrypto);
            File.Delete("info.txt");
            File.WriteAllLines("info.txt", bd.ToArray());
            Encrypt_File("info.txt", desCrypto);

        }


    }
}
