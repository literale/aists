using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace АИСТ.Class
{
    static class Info
    {

        private static string bd_name = "";
        private static byte[] key = new byte[] { 7, 45, 146, 89, 255, 38, 95, 17 };
        private static byte[] IV = new byte[] { 17, 58, 246, 93, 235, 83, 129, 175 };
        private static DESCryptoServiceProvider desCrypto = new DESCryptoServiceProvider();


        /// <summary>
        /// просто возвращаем имя бд, косметика
        /// </summary>
        /// <returns></returns>
        public static string Get_bd_name()
        {
            return bd_name;
        }


        /// <summary>
        /// автозаполнение форм
        /// </summary>
        /// <returns></returns>
        public static List<string> get_bd()
        {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            Encrypt_File("info.xml", desCrypto);
            Decrypt_File("info.xml", desCrypto);
            List<string> bd = new List<string>();
            string[] temp_info = File.ReadAllLines("info.xml");
            foreach (string s in temp_info)
            {
                bd.Add(s);
            }
            Encrypt_File("info.xml", desCrypto);
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
            Decrypt_File("info.xml", desCrypto);
            List<string> bd = new List<string>();
            string[] temp_info = File.ReadAllLines("info.xml");
            bd_name = name;
            foreach (string s in temp_info)
            {
                if (s.Contains("server"))
                {
                    string tmp = "server = " + host;
                    bd.Add(tmp);
                }
                if (s.Contains("database"))
                {
                    string tmp = "database = " + name;
                    bd.Add(tmp);
                }
            }
            File.Delete("info.xml");
            File.WriteAllLines("info.xml", bd.ToArray());
            Encrypt_File("info.xml", desCrypto);
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


        /// <summary>
        /// Дешифрование
        /// </summary>
        /// <param name="sInputFilename"></param> имя файла, который дешифруем
        /// <param name="sKey"></param>ключ
        static void Decrypt_File(string sInputFilename,  DESCryptoServiceProvider sKey)
        {
            string sOutputFilename = "temp" + sInputFilename;
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = sKey.Key;
            DES.IV = sKey.IV;
            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);//поток для чтения зашифрованного файла
            ICryptoTransform desdecrypt = DES.CreateDecryptor();//создание расшифровщика
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read); //Поток для расшифрования
            FileStream fsDecrypted = new FileStream(sOutputFilename, FileMode.Create);//Поток для расшифрования
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

    }
}
