using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sails.Utils
{
    public static partial class AssemblyHelper
    {
        public static void LoadAssemblies()
        {
            string[] assembliyFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            foreach (string filename in assembliyFiles)
            {
                if (filename.Contains("NativeCaller.dll")) continue;
                try
                {
                    Assembly assembly = Assembly.LoadFile(filename);
                    AppDomain.CurrentDomain.Load(assembly.FullName);
                }
                catch
                {
                    //throw new Exception(string.Format("加载程序集{0}时发生错误", filename), ex);
                }
            }
        }

        /// <summary>
        /// 读取给定代理类型所代理的方法信息
        /// </summary>
        /// <param name="delegateType"></param>
        /// <returns></returns>
        public static MethodInfo GetDelegateMethoInfo(Type delegateType)
        {
            if (delegateType.BaseType != typeof(MulticastDelegate)) throw new InvalidOperationException("无效的委托类型!");
            MethodInfo invoke = delegateType.GetMethod("Invoke");
            if (invoke == null) throw new InvalidOperationException("无效的委托类型!");
            return invoke;
            //Type[] typeParameters = new Type[parameters.Length];
            //for(int i = 0; i < parameters.Length; i++)
            //{
            //    typeParameters[i] = parameters[i].ParameterType;
            //}
            //return typeParameters;
        }

        public static bool IsAnonymous(MethodInfo info)
        {
            return info.Name.Contains("<");
        }

        #region CreateInstance

        /// <summary>
        /// 从程序集创建指定类型的实例
        /// </summary>
        /// <typeparam name="T">需要创建的类型</typeparam>
        /// <param name="assembly">程序集</param>
        /// <param name="args">类型参数，传入null表示无参数</param>
        /// <param name="typeName">类型全名，如果传入null或者string.Empty视为所有类型</param>
        /// <returns></returns>
        public static T[] CreateInstance<T>(Assembly assembly, object[] args, string typeName)
        {
            Type t = typeof(T);
            Type[] types = assembly.GetTypes();
            List<T> result = new List<T>();

#pragma warning disable 618

            //new AppDomainSetup().PrivateBinPath = Path.GetDirectoryName(assembly.Location);

            AppDomain.CurrentDomain.AppendPrivatePath(Path.GetDirectoryName(assembly.Location));
#pragma warning restore 618

            foreach (Type item in types)
            {
                if (item != t && !item.IsAbstract && !item.IsInterface && t.IsAssignableFrom(item))
                {
                    if (typeName != null && item.FullName != typeName) continue;
                    if (!HasEmptyParamConstructor(item)) continue;
                    result.Add((T)assembly.CreateInstance(item.FullName));
                }
            }
            T[] res = result.ToArray();
            result.Clear();
            result = null;
            return res;
        }

        /// <summary>
        /// 尝试从指定的程序集文件中创建实例，如果失败返回null
        /// </summary>
        public static T[] CreateInstance<T>(Assembly assembly, object[] args)
        {
            return CreateInstance<T>(assembly, args, null);
        }

        /// <summary>
        /// 尝试从指定的程序集文件中创建实例，如果失败返回null
        /// </summary>
        public static T[] CreateInstance<T>(Assembly assembly)
        {
            return CreateInstance<T>(assembly, null, null);
        }

        /// <summary>
        /// 尝试从指定的程序集文件中创建实例，如果失败返回null
        /// </summary>
        public static object CreateInstance(string[] assembly, params object[] args)
        {
            return CreateInstance(assembly[0], assembly[1], args);
        }
        /// <summary>
        /// 尝试从指定的程序集文件中创建实例，如果失败返回null
        /// </summary>
        /// <param name="assemblyFile">程序集文件，支持相对路径</param>
        /// <param name="typeName">类型名，不要求全名，但如果不是全名，则创建第一个最相近类型名称的对象</param>
        /// <returns></returns>
        public static object CreateInstance(string assemblyFile, string typeName, params object[] args)
        {
            Assembly ass = null;

            if (assemblyFile == null || assemblyFile == string.Empty)
                ass = Assembly.GetCallingAssembly();
            else
            {
                if (assemblyFile.Length < 2 || assemblyFile[1] != ':')
                    assemblyFile = FindAssemblyFile(assemblyFile);
                if (!File.Exists(assemblyFile))
                {
                    ass = Assembly.GetCallingAssembly();
                }
                else
                {
                    assemblyFile = assemblyFile.ToLower();
                    Assembly[] allAssemblys = AppDomain.CurrentDomain.GetAssemblies();
                    for (int i = 0; i < allAssemblys.Length; i++)
                    {
                        //if(allAssemblys[i].IsDynamic) continue;
                        if (allAssemblys[i].Location.ToLower() == assemblyFile)
                        {
                            ass = allAssemblys[i];
                            break;
                        }
                    }
                    if (ass == null) ass = Assembly.LoadFile(assemblyFile);
                }
            }
            Type t = FindType(ass, typeName, typeName.Contains("."));
            if (t != null)
                return ass.CreateInstance(t.FullName, false, BindingFlags.CreateInstance, null, args, null, null);
            return null;
        }

        /// <summary>
        /// 从程序集目录下搜索指定的文件
        /// </summary>
        /// <param name="fileName">需要搜索的文件名，如果是全路径则直接返回</param>
        /// <returns></returns>
        public static string FindAssemblyFile(string fileName)
        {
            if (fileName == null || fileName == string.Empty) return null;
            if (fileName.IndexOf(":") > 0) return fileName;

            string domainPath = AppDomain.CurrentDomain.BaseDirectory;
            using (EnumDirectoryArg res = new EnumDirectoryArg())
            {
                res.Tag = fileName;
                if (EnumDirectory(ref domainPath, new EnumDirectoryMethodHandler(EnumDir), null, res))
                {
                    return Path.Combine(res.TargetPath, fileName);
                }
            }

            string holePath;
            string[] pathes = Environment.GetEnvironmentVariable("Path").Split(';');
            for (int i = 0; i < pathes.Length; i++)
            {
                pathes[i] = GetPath(pathes[i]);
                holePath = Path.Combine(pathes[i], fileName);
                if (File.Exists(holePath))
                {
                    return holePath;
                }
            }

            return null;
        }

        /// <summary>
        /// 取得当前应用程序目录下的子目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string GetPath(string path, params string[] paths)
        {
            string ret = path;
            if (paths != null && paths.Length > 0)
            {
                foreach (var item in paths)
                {
                    ret = Path.Combine(ret, item);
                }
            }
            if (ret == null)
                ret = AppDomain.CurrentDomain.BaseDirectory;
            else
            {
                if (ret.Length < 2 || ret[1] != ':')
                    ret = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ret);
            }
            return ret;
        }
        #endregion

        public static void SetValue(MemberInfo memberInfo, object target, object value)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Property:
                {
                    PropertyInfo prop = ((PropertyInfo)memberInfo);
                    var indexer = prop.GetIndexParameters();
                    if (prop.CanWrite && (indexer == null || indexer.Length == 0))
                        prop.SetValue(target, TurnValue(prop.PropertyType, value), null);
                    break;
                }
                case MemberTypes.Field:
                {
                    FieldInfo field = (FieldInfo)(memberInfo);
                    ((FieldInfo)memberInfo).SetValue(target, TurnValue(field.FieldType, value));
                    break;
                }
            }
        }

        private static object TurnValue(Type type, object value)
        {
            if (value == null) return null;
            Type valueType = value.GetType();
            if (valueType == type) return value;
            if (type == SystemTypes.String) return value.ToString();
            if (type.IsEnum)
            {
                var name = value.ToString();
                if (valueType.IsEnum) name = ((int)value).ToString();
                if (RegexHelper.IsNum(name))
                    return Enum.ToObject(type, Oct.ToInt32(name));
                else
                {
                    return Enum.Parse(type, name);
                }
            }
            return value;
        }

        /// <summary>
        /// 加载引用的程序集
        /// </summary>
        /// <param name="assemblies"></param>
        public static void LoadReferences(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
            {
                assemblies = new Assembly[]
                {
                    Assembly.GetExecutingAssembly(),
                    Assembly.GetEntryAssembly(),
                    Assembly.GetCallingAssembly(),
                };
            };

            foreach (var assembly in assemblies)
            {
                if (assembly == null) continue;
                var assNames = assembly.GetReferencedAssemblies();
                foreach (var item in assNames)
                {
                    try
                    {
                        Assembly.Load(item);
                    }
                    catch { }
                }
            }
        }

        static string startPath;
        public static string StartupPath
        {
            get
            {
                var ass = Assembly.GetEntryAssembly();
                if (ass == null) ass = Assembly.GetCallingAssembly();
                if (ass == null) ass = Assembly.GetExecutingAssembly();
                if (startPath == null) startPath = Path.GetDirectoryName(ass.Location);
                return startPath;
            }
        }

        static string _EntryPath;
        public static string EntryPath
        {
            get
            {
                var ass = Assembly.GetEntryAssembly();
                if (ass == null) ass = Assembly.GetCallingAssembly();
                if (ass == null) ass = Assembly.GetExecutingAssembly();
                if (_EntryPath == null) _EntryPath = ass.Location;
                return _EntryPath;
            }
        }

        static string _EntryName;
        public static string EntryName
        {
            get
            {
                if (_EntryName != null) return _EntryName;
                var ass = Assembly.GetEntryAssembly();
                if (ass == null) ass = Assembly.GetCallingAssembly();
                if (ass == null) ass = Assembly.GetExecutingAssembly();
                if (ass == null || ass.Location == null)
                {
                    _EntryName = "UnknownEntry";
                    return _EntryName;
                }
                _EntryName = Path.GetFileNameWithoutExtension(ass.Location);
                return _EntryName;
            }
        }

        public static object CreateInstance(Type type, params object[] args)
        {
            return type.Assembly.CreateInstance(type.FullName, false, BindingFlags.CreateInstance, null, args, null, null);
        }

        /// <summary>
        /// 获取当前正在运行的程序集入口点
        /// </summary>
        /// <returns></returns>
        public static string GetEntryFileName()
        {
            var ass = Assembly.GetEntryAssembly();
            if (ass != null) return ass.Location;
            var proc = System.Diagnostics.Process.GetCurrentProcess();
            if (proc != null && proc.MainModule != null)
                return proc.MainModule.ModuleName;
            var mode = Assembly.GetExecutingAssembly();
            if (mode != null) return mode.Location;
            return null;
        }

        public static object GetValue(MemberInfo memberInfo, object target)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Property:
                {
                    PropertyInfo prop = ((PropertyInfo)memberInfo);
                    var indexer = prop.GetIndexParameters();
                    if (prop.CanRead && (indexer == null || indexer.Length == 0))
                        return prop.GetValue(target, null);
                    return null;
                }
                case MemberTypes.Field:
                return ((FieldInfo)memberInfo).GetValue(target);
            }
            return null;
        }
        /// <summary>
        /// 在搜索程序集路径的时候所使用的方法
        /// </summary>
        /// <param name="enumValue"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        private static bool EnumDir(ref string enumValue, EnumDirectoryArg arg)
        {
            string totalPath = Path.Combine(enumValue, arg.Tag.ToString());
            if (File.Exists(totalPath)) return true;
            return false;
        }

        /// <summary>
        /// 遍历文件夹所要执行的方法委托
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        delegate bool EnumDirectoryMethodHandler(ref string enumValue, EnumDirectoryArg arg);
        /// <summary>
        /// 遍历指定文件夹的所有目录
        /// </summary>
        /// <param name="dir">给定的文件夹</param>
        /// <param name="method">搜索时所要执行的方法</param>
        /// <param name="filter">文件名过滤器</param>
        /// <returns></returns>
        static bool EnumDirectory(ref string dir, EnumDirectoryMethodHandler method, string filter, EnumDirectoryArg result)
        {
            if (result != null)
                result.Success = false;
            if (dir == null || dir == string.Empty) return false;
            if (method(ref dir, result))
            {
                if (result != null)
                {
                    result.Success = true;
                    result.TargetPath = dir;
                }
                return true;
            }
            string[] dirs = null;
            if (filter != null && filter != string.Empty)
            {
                dirs = Directory.GetDirectories(dir, filter);
            }
            else
            {
                dirs = Directory.GetDirectories(dir);
            }
            for (int i = 0; i < dirs.Length; i++)
            {
                if (method(ref dirs[i], result))
                {
                    if (result != null)
                    {
                        result.Success = true;
                        result.TargetPath = dirs[i];
                    }
                    return true;
                }
            }
            for (int i = 0; i < dirs.Length; i++)
            {
                if (EnumDirectory(ref dirs[i], method, filter, result)) return true;
            }
            return false;
        }

        /// <summary>
        /// 获取可以从指定类型派生的所有类型
        /// </summary>
        /// <param name="assembly">需要获取类型的程序集</param>
        /// <param name="type">基类型</param>
        /// <param name="canInitialize">true=必须可以是被实例化的类型</param>
        /// <param name="emptyConstructor">true=必须包含空构造函数</param>
        /// <returns></returns>
        public static Type[] FindTypes(Assembly assembly, Type type, bool emptyConstructor, bool canInitialize)
        {
            Type[] types = assembly.GetTypes();
            List<Type> result = new List<Type>();
            foreach (Type item in types)
            {
                //if (item == type) continue;
                if (canInitialize)
                    if (item.IsInterface || item.IsAbstract) continue;

                if (type.IsAssignableFrom(item))
                {
                    if (emptyConstructor && !HasEmptyParamConstructor(item))
                        continue;
                    result.Add(item);
                }
            }
            Type[] res = result.ToArray();
            result.Clear();
            result = null;
            return res;
        }

        /// <summary>
        /// 获取可以从指定类型派生的所有类型
        /// </summary>
        /// <param name="assembly">需要获取类型的程序集</param>
        /// <param name="type">基类型</param>
        /// <param name="canInitialize">true=必须可以是被实例化的类型</param>
        /// <param name="emptyConstructor">true=必须包含空构造函数</param>
        /// <returns></returns>
        public static bool HasType(Assembly assembly, Type type, bool emptyConstructor, bool canInitialize)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type item in types)
            {
                //if (item == type) continue;
                if (canInitialize)
                    if (item.IsInterface || item.IsAbstract) continue;

                if (type.IsAssignableFrom(item))
                {
                    if (emptyConstructor && !HasEmptyParamConstructor(item))
                        continue;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断一个类型是否包含空构造函数
        /// </summary>
        /// <param name="constructorInfo"></param>
        /// <returns></returns>
        public static bool HasEmptyParamConstructor(Type type)
        {
            ConstructorInfo[] constructorInfo = type.GetConstructors();
            for (int i = 0; i < constructorInfo.Length; i++)
            {
                if (constructorInfo[i].GetParameters().Length == 0) return true;
            }
            return false;
        }

        /// <summary>
        /// 获取目标类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Type GetType<T>()
        {
            return typeof(T);
        }

        /// <summary>
        /// 从文件加载程序集，如果失败则返回null
        /// </summary>
        /// <param name="assemblyFile">需要加载的程序集文件</param>
        /// <returns></returns>
        public static Assembly Load(string assemblyFile)
        {
            if (!File.Exists(assemblyFile)) return null;
            try
            {
                return Assembly.LoadFrom(assemblyFile);
            }
            catch
            {
                return null;
            }
        }

#if !SILVERLIGHT
        /// <summary>
        /// 加载所有程序集
        /// </summary>
        public static void LoadAllAssemblies()
        {
            LoadReferenceAssembly(Assembly.GetExecutingAssembly());
            LoadReferenceAssembly(Assembly.GetEntryAssembly());
        }

        static Dictionary<string, Assembly> loadedAssDic = new Dictionary<string, Assembly>();
        public static void LoadReferenceAssembly(Assembly ass)
        {
            if (ass == null || ass.Location == null) return;
            var name = ass.Location.ToLower();
            if (name == null) return;
            if (loadedAssDic.ContainsKey(name)) return;
            loadedAssDic[name] = ass;
            var references = ass.GetReferencedAssemblies();
            var models = ass.GetModules(true);
            foreach (var asname in references)
            {
                try
                {
                    var loadedAss = Assembly.Load(asname);
                    LoadReferenceAssembly(loadedAss);
                }
                catch { }
            }
        }

        public static AssemblyName[] GetReferenceAssemblies(int deep = 0)
        {
            var ass = Assembly.GetEntryAssembly();
            if (ass == null)
                ass = Assembly.GetCallingAssembly();
            if (ass == null) ass = typeof(AssemblyHelper).Assembly;
            List<AssemblyName> list = new List<AssemblyName>();
            Dictionary<string, Assembly> loaded = new Dictionary<string, Assembly>();
            GetReferenceAssemblies(ass, list, loaded, deep);
            return list.ToArray();
        }

        public static AssemblyName[] GetReferenceAssemblies(Assembly ass, int deep = 0)
        {
            List<AssemblyName> list = new List<AssemblyName>();
            Dictionary<string, Assembly> loaded = new Dictionary<string, Assembly>();
            GetReferenceAssemblies(ass, list, loaded, deep);
            return list.ToArray();
        }

        private static void GetReferenceAssemblies(Assembly ass, List<AssemblyName> list, Dictionary<string, Assembly> loaded, int deep = 0)
        {
            var fullName = ass.GetName().FullName;
            if (loaded.ContainsKey(fullName)) return;
            loaded[fullName] = ass;
            var assNames = ass.GetReferencedAssemblies();
            list.AddRange(assNames);
            if (deep-- > 0)
            {
                foreach (var an in assNames)
                {
                    if (loaded.ContainsKey(an.FullName)) continue;
                    var subAss = AppDomain.CurrentDomain.Load(an);
                    GetReferenceAssemblies(subAss, list, loaded, deep);
                }
            }
        }
#endif
        /// <summary>
        /// 找到给定程序集中最匹配的的类型
        /// </summary>
        /// <param name="idBuilder"></param>
        /// <param name="exact">true=精确查找，如果不进行精确查找，那么可能找到的会是一个错误的类型</param>
        /// <returns></returns>
        public static Type FindType(Assembly ass, string type, bool exact = true)
        {
            if (type == null) return null;
            Type result = null;
#if !SILVERLIGHT
            result = ass.GetType(type, false, true);
#else
			result = ass.GetType(type,false);
#endif
            if (exact) return result;
            if (result != null) return result;
            Type[] types = ass.GetTypes();
            Type equalType = null;
            Type likeType = null;
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i].Name == type)
                    equalType = types[i];
                else if (types[i].FullName.Contains(type))
                    likeType = types[i];
            }
            if (equalType != null)
                return equalType;

            return likeType;
        }

        /// <summary>
        /// 找到给定程序集中最匹配的的类型
        /// </summary>
        /// <param name="idBuilder"></param>
        /// <returns></returns>
        public static Type FindType(string type)
        {
            return FindType(Assembly.GetCallingAssembly(), type);
        }

        public static bool SetFieldValue(object target, string fieldName, object value)
        {
            if (target == null) return false;
            Type t = target.GetType();
            FieldInfo fieldInfo = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField);
            if (fieldInfo == null) return false;
            if (DBNull.Value == value)
            {
                fieldInfo.SetValue(target, null);
            }
            else
                fieldInfo.SetValue(target, value);
            return true;
        }

        public static bool SetPropertyValue(object target, string fieldName, object value)
        {
            if (target == null) return false;
            Type t = target.GetType();
            PropertyInfo fieldInfo = t.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField);
            if (fieldInfo == null) return false;
            if (DBNull.Value == value)
            {
                fieldInfo.SetValue(target, null, null);
            }
            else
                fieldInfo.SetValue(target, value, null);
            return true;
        }

        public static object GetFieldValue(object target, string fieldName)
        {
            if (target == null) return null;
            Type t = target.GetType();
            FieldInfo field = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField);
            if (field == null) return null;
            return field.GetValue(target);
        }

        public static object GetFieldValue(Type targetType, string fieldName)
        {
            Type t = targetType;
            FieldInfo field = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField);
            if (field == null) return null;
            return field.GetValue(null);
        }

        public static object GetPropertyValue(object target, string fieldName)
        {
            if (target == null) return null;
            Type t = target.GetType();
            PropertyInfo field = t.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField);
            if (field == null) return null;
            return field.GetValue(target, null);
        }

        public static FieldInfo GetField(Type type, string fieldName)
        {
            FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField);
            return field;
        }

        public static MethodInfo GetMethod(Type type, string methodName)
        {
            MethodInfo field = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Public);
            return field;
        }

        public static MethodInfo GetMethod(Type type, string methodName, Type[] parmTypes)
        {
            MethodInfo field = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod | BindingFlags.Public, null, parmTypes, null);
            return field;
        }


        public static bool IsValueEquals(object a, object b)
        {
            if (a == b) return true;
            if (a == null && b != null) return false;
            if (a != null && b == null) return false;
            Type type = a.GetType();
            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                if (!field.FieldType.IsPrimitive) continue;
                object v1 = field.GetValue(a);
                object v2 = field.GetValue(b);
                if (v1 == v2) continue;
                if (v1 == null && v2 != null) return false;
                if (v1 != null && v2 == null) return false;
                if (!v1.Equals(v2)) return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="types"></param>
        /// <param name="readAndWrite">该值只有在MemberTypes包含属性时才有效</param>
        /// <returns></returns>
        public static MemberInfo[] GetMembers(this Type type, MemberTypes types, bool readAndWrite = true, params Type[] bindingAttribs)
        {
            List<MemberInfo> lst = new List<MemberInfo>();

            var members = type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var item in members)
            {
                if ((item.MemberType & types) == 0) continue;
                if (item.MemberType == MemberTypes.Property && readAndWrite)
                {
                    var prop = (PropertyInfo)item;
                    if (!prop.CanRead || !prop.CanWrite) continue;
                }

                if (bindingAttribs != null && bindingAttribs.Length > 0)
                {
                    for (int i = 0; i < bindingAttribs.Length; i++)
                    {
                        var customAtt = item.GetCustomAttributes(bindingAttribs[i], true);
                        if (customAtt != null && customAtt.Length > 0)
                        {
                            lst.Add(item);
                            break;
                        }
                    }
                }
                else
                    lst.Add(item);
            }
            return lst.ToArray();
        }

        public static T GetAttribute<T>(this MemberInfo info) where T : Attribute
        {
            if (info == null) return null;
            Type type = typeof(T);
            var attribs = info.GetCustomAttributes(type, true);
            if (attribs.Length == 0) return null;
            return (T)attribs[0];
        }

        public static T GetAttribute<T>(object target, string memberName) where T : Attribute
        {
            if (memberName == null || target == null) return null;
            Type targetType = target.GetType();
            var members = targetType.GetMember(memberName);
            if (members.Length == 0) return null;
            return GetAttribute<T>(members[0]);
        }

        public static string GetFieldString(object data, string equalSymbol = "=", string splitSymbol = ",")
        {
            if (data == null) return string.Empty;
            var type = data.GetType();
            var fields = type.GetFields();
            string ret = string.Empty;
            foreach (var field in fields)
            {
                ret += field.Name + equalSymbol + field.GetValue(data) + splitSymbol;
            }

            var props = type.GetProperties();
            foreach (var field in props)
            {
                ret += field.Name + equalSymbol + field.GetValue(data, null) + splitSymbol;
            }

            return ret;
        }

        public static Assembly GetAssemblyByName(string assemblyName, AppDomain domain = null)
        {
            if (domain == null) domain = AppDomain.CurrentDomain;
            var assemblies = domain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.FullName == assemblyName) return assembly;
            }
            return null;
        }

        public static Assembly CompileAssemblyFromSource(string source)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameter = new CompilerParameters();
            parameter.GenerateInMemory = true;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var ass in assemblies)
            {
                if (ass.IsDynamic) continue;
                parameter.ReferencedAssemblies.Add(ass.Location);
            }

            CompilerResults result = provider.CompileAssemblyFromSource(parameter, source);
            if (result.Errors.Count > 0)
            {
                string error = string.Empty;
                foreach (var errItem in result.Errors)
                    error += errItem.ToString() + "\r\n";
                throw new Exception(string.Format("在生成程序集时发生错误!\r\n{0}", error));
            }
            else
            {
                return result.CompiledAssembly;
            }
        }

        /// <summary>
        /// 将给定的类型转化成字符串描述信息，例如 System.Int32 会被转化成 int
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string ToTs(this Type type)
        {
            return SystemTypes.GetTS(type);
        }
    }

    public class EnumDirectoryArg : IDisposable
    {
        /// <summary>
        /// 当枚举方法返回true时该值会被设置为true
        /// </summary>
        public bool Success = false;
        /// <summary>
        /// 当枚举方法返回true时该值会被设置为枚举方法的参数
        /// </summary>
        public string TargetPath;
        /// <summary>
        /// 参数存储位
        /// </summary>
        public object Tag;

        //===
        private bool isDisposed = false;
        /// <summary>
        /// 释放所占用的资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            // Take yourself off the Finalization queue 
            // to prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                Tag = null;
                TargetPath = null;
            }
            isDisposed = true;
        }

        /// <summary>
        /// 获取该对象是否已经被释放
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return isDisposed;
            }
        }
    }
}
