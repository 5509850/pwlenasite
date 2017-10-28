using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MLDBUtils
{
    public class WindowWrapper : System.Windows.Forms.IWin32Window
    {
        public WindowWrapper(IntPtr handle)
        {
            _hwnd = handle;
        }

        public IntPtr Handle
        {
            get { return _hwnd; }
        }

        private IntPtr _hwnd;
    }

    public class MLMenuUtils
    {
        delegate void ExecFunc(object obj, EventArgs e); // делегат или указатель на функцию

        public static void AddMainMenuItem(System.Windows.Forms.MenuStrip mainMenu, string testMenu, string menuAction)
        {
            //string testMenu = "Файл/Выход";
            string[] menus = testMenu.Split('/');
            string curMenu = "";
            //string menuAction = "toolStripMenuItem2_Click";

            for (int i = 0; i < menus.Length; i++)
            {
                if (mainMenu.Items[menus[i]] == null)
                {
                    System.Windows.Forms.ToolStripMenuItem newMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                    newMenuItem.Text = menus[i];
                    newMenuItem.Name = menus[i];
                    if (i == menus.Length - 1)
                    {

                        newMenuItem.Tag = menuAction;
                        newMenuItem.Click += new System.EventHandler(MLDBUtils.MLMenuUtils.ExecMenuAction);

                    }


                    if (curMenu == "")
                    {
                        mainMenu.Items.Add(newMenuItem);
                        curMenu = menus[i];
                    }
                    else
                    {
                        System.Windows.Forms.ToolStripMenuItem oldMenuItem = mainMenu.Items[curMenu] as System.Windows.Forms.ToolStripMenuItem;
                        oldMenuItem.DropDownItems.Add(newMenuItem);

                    }
                }
                else curMenu = menus[i];

            }

            
        }

        static protected void ExecMenuAction(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem mi = sender as System.Windows.Forms.ToolStripMenuItem;
            //MessageBox.Show(mi.Tag.ToString());
            ExecFunc exf = (ExecFunc)Delegate.CreateDelegate(   // Динамическое создание делегата из MethodInfo
               typeof(ExecFunc),   // Тип создаваемого делегата
               sender,                 // У кого вызывать метод
               mi.Tag.ToString());                // Какой метод вызывать

            exf(sender,e);
        }
        /// <summary>
        /// Вызов функции из заданной сборки
        /// </summary>
        /// <param name="dllName">Имя сборки</param>
        /// <param name="className">Имя класса</param>
        /// <param name="methodName">Имя метода(функции)</param>
        /// <param name="methodTypes">Типы параметров метода или new Type[0] без параметров</param>
        /// <param name="methodPar">Параметры метода в object[]</param>
        /// <param name="isCreateObj">Нужно ли создавать объект</param>
        /// <param name="cPar">Праметры конструктора Dictionary<string,object></param>
        /// В настоящее время не используется.
        
        static public void ExecAssemblyFunction(string dllName,string className,string methodName,Type[] methodTypes,object[] methodPar,bool isCreateObj,Dictionary<string,object> cPar)
        {
            dllName = dllName.Replace("[MLDLL]", MLDBUtils.Properties.Settings.Default.MLDLLPath);
            
            Assembly ass = Assembly.LoadFile(dllName);
            Type TestType = ass.GetType(className, false, true);
            object Obj=null;
            
            /*Type TestType =
                           Type.GetType(selectMethod, false, true);*/
            // Если класс найден
            if (TestType != null)
            {
                //если нужно создать объект
                if (isCreateObj)
                {
                    // Пробуем найти конструтор с параметрами
                    if (cPar != null)
                    {
                        ConstructorInfo ci = TestType.GetConstructor(
                                       new Type[] { typeof(Dictionary<string, object>) });

                        // Вызываем конструктор c параметрами
                        Obj = ci.Invoke(new object[1] { cPar });
                    }
                    else
                    {
                        ConstructorInfo ci =
                                TestType.GetConstructor(new Type[] { });
                        // Вызываем конструктор без параметров
                        Obj = ci.Invoke(new object[] { });
                        
                    }
                }

                //MethodInfo mi = TestType.GetMethod(methodName, new Type[0]);
                if (Obj != null)
                {
                    /*
                    MethodInfo mi = TestType.GetMethod(methodName, methodTypes);
                    mi.Invoke(Obj, methodPar);
                    */
                    TestType.InvokeMember(methodName, BindingFlags.Public|BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, Obj, methodPar);
                }
                else
                {
                    throw new Exception(" функция MLDBUtils.ExecAssemblyFunction не смог создать класс");
                    return;
                }
            }
            else
            {
                throw new Exception(" функция MLDBUtils.ExecAssemblyFunction не смог найти класс");
                return;
            }
        }

        public static void ExecAssemblyFunction(string pString,Dictionary<string,object> pValues)
        {
            /*
            pString format
            (0)имя_сборки(путь к сборке полный);(1)имя_класса;(2)имя_метода;(3)параметры_значения_метода;
            (4)создавать_объект_(true,false);(5)параметры_конструктора;(6)тип_документа;(7)конструктор вида Constr(Dictionary<string,object>)
            
            Типы, параметры и значения метода  в виде
            Type1-ParamName0=DefValue,...,TypeN-ParamNameN=DefValueN либо {}
            
            Параметры конструктора в виде Type1-ParamName0=DefValue,...,TypeN-ParamNameN=DefValueN либо {}
            */

            string[] par = pString.Split(';');
            Type[] MethodTypes=new Type[0];
            object[] MethodParamsValues=new object[0];

            // разбираем метод
            if (par[3] != "{}")
            {
                string[] pMethod = par[3].Split(',');
                for (int i = 0; i < pMethod.Length; i++)
                {
                    string[] method=pMethod[i].Split('-');
                    Array.Resize(ref MethodTypes, MethodTypes.Length + 1);
                    MethodTypes[MethodTypes.Length-1]=Type.GetType(method[0]);
                    Array.Resize(ref MethodParamsValues, MethodParamsValues.Length + 1);
                    if (method[1].IndexOf('=') > 0)
                    {
                        method = method[1].Split('=');
                        //MethodParamsValues[MethodParamsValues.Length - 1] = method[1] as object;
                        object[] mp = new object[1]; mp[0] = method[1];
                        
                        if (MethodTypes[MethodTypes.Length - 1].ToString() != "System.String")
                            MethodParamsValues[MethodParamsValues.Length - 1] = MethodTypes[MethodTypes.Length - 1].InvokeMember("Parse",
                                BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static,
                               null, null, mp);
                        else MethodParamsValues[MethodParamsValues.Length - 1] = mp[0].ToString();
                    }
                    else MethodParamsValues[MethodParamsValues.Length - 1] = pValues[method[1]];
                }
            }
            
            //разбираем конструктор
            Type[] ConstructorTypes = new Type[0];
            object[] ConstructorParamsValues = new object[0];

            if(bool.Parse(par[7]))
            {
                Array.Resize(ref ConstructorTypes, ConstructorTypes.Length + 1);
                ConstructorTypes[ConstructorTypes.Length - 1] = typeof(Dictionary<string,object>);
                Array.Resize(ref ConstructorParamsValues, ConstructorParamsValues.Length + 1);

                if (par[5].ToString() != "{}")
                {
                    string[] pMethod = par[5].Split(',');
                    for (int i = 0; i < pMethod.Length; i++)
                    {
                        string[] method = pMethod[i].Split('=');
                        if(pValues.ContainsKey(method[0])) pValues.Remove(method[0]);
                        pValues.Add(method[0], method[1]);
                    }
                }

                ConstructorParamsValues[ConstructorParamsValues.Length - 1] = pValues;
            }
            else 
                if (par[5] != "{}")
                {
                  string[] pMethod = par[5].Split(',');
                  for (int i = 0; i < pMethod.Length; i++)
                  { 
                    string[] method = pMethod[i].Split('-');
                                       
                    Array.Resize(ref ConstructorTypes, ConstructorTypes.Length + 1);
                    ConstructorTypes[ConstructorTypes.Length - 1] = Type.GetType(method[0]);
                    Array.Resize(ref ConstructorParamsValues, ConstructorParamsValues.Length + 1);
                    if (method[1].IndexOf('=') > 0)
                    {
                        method = method[1].Split('=');
                        /*
                        ConstructorInfo ci = ConstructorTypes[ConstructorTypes.Length - 1].GetConstructor();
                        object obj = ci.Invoke();
                        */
                        object[] mp=new object[1]; mp[0]=method[1];
                        
                        ConstructorParamsValues[ConstructorParamsValues.Length - 1] = ConstructorTypes[ConstructorTypes.Length - 1].InvokeMember("Parse", BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, null, null, mp);
                        //ConstructorParamsValues[ConstructorParamsValues.Length - 1] = method[1] as object;
                    }
                    else ConstructorParamsValues[ConstructorParamsValues.Length - 1] = pValues[method[1]];
                  }                
                }
            
            ExecAssemblyFunction(par[0], par[1], par[2], 
                MethodTypes, MethodParamsValues, 
                bool.Parse(par[4]), ConstructorTypes, ConstructorParamsValues);

            

        }

        static public void ExecAssemblyFunction(string dllName, string className, string methodName, Type[] methodTypes, object[] methodPar, bool isCreateObj,Type[] ConstructorTypes,object[] ConstructorPar)
        {
            dllName = dllName.Replace("[MLDLL]", MLDBUtils.Properties.Settings.Default.MLDLLPath);
                //.Properties.Settings.Default.MLDLLPath);
 //            System.Windows.Forms.MessageBox.Show(dllName);
            Assembly ass = Assembly.LoadFile(dllName);
            Type TestType = ass.GetType(className, false, true);
            //MyOBJ Obj = null;
            object Obj = null;
            
            /*Type TestType =
                           Type.GetType(selectMethod, false, true);*/
            // Если класс найден
            if (TestType != null)
            {
                //если нужно создать объект
                if (isCreateObj)
                {
                   
                        ConstructorInfo ci = TestType.GetConstructor(ConstructorTypes);

                        // Вызываем конструктор c параметрами
                        
                        Obj = ci.Invoke(ConstructorPar);
                        
                   
                }
                
                //MethodInfo mi = TestType.GetMethod(methodName, new Type[0]);
                if (Obj != null)
                {
                    /*
                    MethodInfo mi = TestType.GetMethod(methodName, methodTypes);
                    mi.Invoke(Obj, methodPar);
                    */
                    TestType.InvokeMember(methodName, BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic, null, Obj, methodPar);
                }
                else
                {
                    MethodInfo mi=TestType.GetMethod(methodName);

                    //System.Windows.Forms.MessageBox.Show(mi.ToString());
                    //foreach (object j in methodPar) System.Windows.Forms.MessageBox.Show(j.ToString());

                    mi.Invoke(null, methodPar);

                    /*
                    if (!isCreateObj) TestType.InvokeMember(methodName, BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static, null, null, methodPar);
                    else
                    {
                        throw new Exception(" функция MLDBUtils.ExecAssemblyFunction не смог создать класс");
                        return;
                    }
                     */
                }
            }
            else
            {
                throw new Exception(" функция MLDBUtils.ExecAssemblyFunction не смог найти класс");
                return;
            }
        }

        public static string _AboutMes()
        {
            string[] st = System.IO.Directory.GetFiles(MLDBUtils.Properties.Settings.Default.MLDLLPath, "*.*");
            string mes="";

            foreach (string s in st)
            {
                if (System.Diagnostics.FileVersionInfo.GetVersionInfo(s).CompanyName == "JSC SOMBelBank" |
                    System.Diagnostics.FileVersionInfo.GetVersionInfo(s).CompanyName == "Developer Express Inc.")
                    mes+=System.Diagnostics.FileVersionInfo.GetVersionInfo(s).FileName + " " + System.Diagnostics.FileVersionInfo.GetVersionInfo(s).ProductVersion+"\n";
            }

            return mes;
        }

    }
}
