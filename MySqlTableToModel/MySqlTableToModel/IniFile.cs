using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace MySqlTableToModel
{
    public class IniFile
    {
        #region static 

        //[DllImport("kernel32", CharSet = CharSet.Unicode)]
        //static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        //[DllImport("kernel32", CharSet = CharSet.Unicode)]
        //static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(byte[] section, byte[] key, byte[] val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(byte[] section, byte[] key, byte[] def, byte[] retVal, int size, string filePath);


        #endregion

        #region Fields

        string _path;
        string _exe = Assembly.GetExecutingAssembly().GetName().Name;

        #endregion

        #region Constructors
        public IniFile(string IniPath = null)
        {
            _path = new FileInfo(IniPath ?? _exe + ".ini").FullName;
        }

        #endregion

        #region Methods

        private static byte[] getBytes(string s, string encodingName)
        {
            return null == s ? null : Encoding.GetEncoding(encodingName).GetBytes(s);
        }

        //public string Read(string Key, string Section = null)
        //{
        //    var stringBuilder = new StringBuilder(255);
        //    GetPrivateProfileString(Section ?? _exe, Key, "", stringBuilder, 255, _path);
        //    return stringBuilder.ToString();
        //}

        //public void Write(string Key, string Value, string Section = null)
        //{
        //    WritePrivateProfileString(Section ?? _exe, Key, Value, _path);
        //}

        public string Read(string key, string section, string encodingName = "utf-8", int size = 1024)
        {
            byte[] buffer = new byte[size];
            int count = GetPrivateProfileString(getBytes(section ?? _exe, encodingName), getBytes(key, encodingName), null, buffer, size, _path);
            return Encoding.GetEncoding(encodingName).GetString(buffer, 0, count).Trim();
        }

        public bool Write(string key, string value, string section = null, string encodingName = "utf-8")
        {
            return WritePrivateProfileString(getBytes(section ?? _exe, encodingName), getBytes(key, encodingName), getBytes(value, encodingName), _path);
        }

        public void DeleteKey(string Key, string Section = null)
        {
            Write(Key, null, Section ?? _exe);
        }

        public void DeleteSection(string Section = null)
        {
            Write(null, null, Section ?? _exe);
        }

        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Key, Section).Length > 0;
        }

        #endregion
    }
}
