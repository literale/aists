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

        public static string Get_bd_name()
        {
            return bd_name;
        }

        public static List<string> get_bd()
        {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            //Encrypt_File("info.xml", "info1.xml", desCrypto);
            Decrypt_File("info.xml", "info2.xml", desCrypto);
            List<string> bd = new List<string>();
            string[] temp_info = File.ReadAllLines("info.xml");
            foreach (string s in temp_info)
            {
                bd.Add(s);
            }
            Encrypt_File("info.xml", "info1.xml", desCrypto);
            return bd;
        }
        public static void Set_bd(string host, string name)
        {
            desCrypto.Key = key;
            desCrypto.IV = IV;
            Decrypt_File("info.xml", "info2.xml", desCrypto);
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

            Encrypt_File("info.xml", "info1.xml", desCrypto);
            //Decrypt_File("info.xml", "info2.xml", desCrypto);
        }
        static void Encrypt_File(string sInputFilename, string sOutputFilename, DESCryptoServiceProvider sKey)
        {
            FileStream fsInput = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypted = new FileStream(sOutputFilename, FileMode.Create, FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = sKey.Key;
            DES.IV = sKey.IV;
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();

            File.Delete(sInputFilename);
            File.Move(sOutputFilename, sInputFilename);


        }
        static void Decrypt_File(string sInputFilename, string sOutputFilename, DESCryptoServiceProvider sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = sKey.Key;
            //Set initialization vector.
            DES.IV = sKey.IV;

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename, FileMode.Open, FileAccess.Read);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
            //Print the contents of the decrypted file.
            FileStream fsDecrypted = new FileStream(sOutputFilename, FileMode.Create);
            byte[] bytearrayinput = new byte[1024];
            int length;
            do
            {
                length = cryptostreamDecr.Read(bytearrayinput, 0, 1024);
                fsDecrypted.Write(bytearrayinput, 0, length);
            } while (length == 1024);
            fsDecrypted.Flush();
            fsread.Close();
            cryptostreamDecr.Close();
            fsDecrypted.Close();
            File.Delete(sInputFilename);
            File.Move(sOutputFilename, sInputFilename);
        }

    }
}
