using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pw.lena.slave.winpc
{
    public class utils
    {
        public static string GetMacAdres()
        {
            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {

                    if (nic.OperationalStatus == OperationalStatus.Up &&
                        nic.GetPhysicalAddress().ToString().Length == 12)
                    {
                        return ConvertMacToDefis(nic.GetPhysicalAddress().ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        private static String ConvertMacToDefis(string mac)
        {
            if (mac.Length != 12)
                return mac;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mac.Length; i++)
            {
                if (i == 2 || i == 4 || i == 6 || i == 8 || i == 10)
                    sb.Append("-");
                sb.Append(mac[i]);
            }
            //002522CD0E00
            //00-25-22-CD-0E-00
            return sb.ToString();
        }

        private static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string GetMinSecFromInt(int time)
        {
            int min = time / 60;
            int sec = time - (min * 60);
            string result = string.Empty;
            if (sec < 10)
            {
                result = string.Format("{0}.0{1}", min, sec);
            }
            else
            {
                result = string.Format("{0}.{1}", min, sec);
            }
            return result;
        }

        public static bool GetAuturunValue()
        {
            bool result = false;
            string ExePath = Application.ExecutablePath;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            String vol = String.Empty;
            object obj;

            try
            {
                if (reg != null)
                {
                    obj = reg.GetValue("pw.lena.slave.winpc");


                    if (obj == null)
                        result = false;
                    else
                        result = true;

                    if (reg != null)
                        reg.Close();
                }


            }
            catch
            {
                return false;
            }
            return result;
        }


        public static bool SetAutorunValue(bool autorun)
        {
            try
            {

                string ExePath = Application.ExecutablePath;
                RegistryKey reg;
                reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
                if (autorun)
                {
                    //string app = getAppName() + ".exe";
                    //if (reg != null) reg.SetValue(app, ExePath + " k");
                    if (reg != null) reg.SetValue("pw.lena.slave.winpc.exe", ExePath + " k");
                }
                else if (reg != null)
                {
                    //string app = getAppName() + ".exe";
                    //if (reg != null) reg.SetValue(app, ExePath + " k");
                    reg.DeleteValue("pw.lena.slave.winpc.exe");
                }
                

                if (reg != null) reg.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        private static string getAppName()
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            int id = p.Id;
            return p.ProcessName;
        }

        #region hide tab app show
        private const int WS_EX_TOOLWINDOW = 0x00000080;
        private const int GWL_EXSTYLE = -20;
        public static void HideFromAltTab(IntPtr Handle)
        {
            SetWindowLong(Handle, GWL_EXSTYLE, GetWindowLong(Handle,
                GWL_EXSTYLE) | WS_EX_TOOLWINDOW);
        }

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr window, int index, int value);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr window, int index);
        #endregion





        public static class ScreenShot
        {


            //static public void MakeScreenShot(string ftpPath, float x, float y)
            //{
            //    if (x == 0 || y == 0)
            //    {
            //        MakeScreenShot(ftpPath);
            //        return;
            //    }

            //    Screen currentScreen = Screen.PrimaryScreen;//Выбираем, какой именно экран скриншотить 
            //    using (Bitmap bmpScreenShot = new Bitmap(currentScreen.Bounds.Width, currentScreen.Bounds.Height, PixelFormat.Format32bppRgb))
            //    {
            //        Graphics gScreenShot = Graphics.FromImage(bmpScreenShot);
            //        // Делаем переменную для хранения скрина 
            //        gScreenShot.CopyFromScreen(currentScreen.Bounds.X, currentScreen.Bounds.Y, 0, 0, currentScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            //        // Делаем скриншот 
            //        //bmpScreenShot.Save(@"D:\screen.jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);// Сохраняем по пути D:\screen.jpeg 
            //        using (Bitmap outImage = new Bitmap((int)x, (int)y))
            //        {
            //            Graphics g = Graphics.FromImage(outImage);
            //            g.DrawImage(bmpScreenShot, new RectangleF(0, 0, x, y), new RectangleF(0, 0, bmpScreenShot.Width, bmpScreenShot.Height), GraphicsUnit.Pixel);
            //            g.Dispose();
            //            DeleteFile(Path.Combine(ftpPath, "screen.jpg"));
            //            outImage.Save(Path.Combine(ftpPath, "screen.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
            //        }
            //    }// Сохраняем по пути D:\screen.jpeg 




            //}

            static public void MakeScreenShot(string path)
            {
                Screen currentScreen = Screen.PrimaryScreen;//Выбираем, какой именно экран скриншотить  !! add other monitors HERE!!!!!
                using (Bitmap bmpScreenShot = new Bitmap(currentScreen.Bounds.Width, currentScreen.Bounds.Height, PixelFormat.Format32bppRgb))
                {
                    Graphics gScreenShot = Graphics.FromImage(bmpScreenShot);
                    // Делаем переменную для хранения скрина 
                    gScreenShot.CopyFromScreen(currentScreen.Bounds.X, currentScreen.Bounds.Y, 0, 0, currentScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    //Удаляем предыдущий скриншот
                    DeleteFile(Path.Combine(path, "screen.jpg"));
                    // Делаем скриншот 
                    bmpScreenShot.Save(Path.Combine(path, "screen.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                }// Сохраняем по пути D:\screen.jpeg 

            }

            static public byte[] MakeScreenShot()
            {
                Screen currentScreen = Screen.PrimaryScreen;//Выбираем, какой именно экран скриншотить  !! add other monitors HERE!!!!!
                using (Bitmap bmpScreenShot = new Bitmap(currentScreen.Bounds.Width, currentScreen.Bounds.Height, PixelFormat.Format32bppRgb))
                {
                    Graphics gScreenShot = Graphics.FromImage(bmpScreenShot);
                    // Делаем переменную для хранения скрина 
                    gScreenShot.CopyFromScreen(currentScreen.Bounds.X, currentScreen.Bounds.Y, 0, 0, currentScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    //Удаляем предыдущий скриншот
                    //   DeleteFile(Path.Combine(path, "screen.jpg"));
                    // Делаем скриншот 
                    // bmpScreenShot.Save(Path.Combine(path, "screen.jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                    using (var stream = new MemoryStream())
                    {
                        bmpScreenShot.Save(stream, ImageFormat.Jpeg);
                        return stream.ToArray();
                    }

                }// Сохраняем по пути D:\screen.jpeg 

            }

            private static byte[] ReadFully(Stream input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }

            static private void DeleteFile(String file)
            {
                //MessageBox.Show(file);//
                //var tt = Path.Combine(ftpPath, file);
                if (File.Exists(file))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }



        }


        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

    }
}
