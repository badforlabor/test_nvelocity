using NVelocity;
using NVelocity.App;
using NVelocity.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace test_nvelocity
{
    class ClassInfo
    {
        public string Name { get; set; }
        public List<FieldInfo> Fields { get; set; }
    }
    class Util
    {
        public string Show(string a)
        {
            return "util:" + a;
        }
    }
    class TestClass
    {
        public int a;
        private int aa;
        public int ab;
        public int b { get; set; }
        public int func1()
        {
            return 0;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //创建一个模板引擎
            VelocityEngine ve = new VelocityEngine();
            ve.Init();

            // dict用法
            {
                VelocityContext vc = new VelocityContext();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic["dudu"] = "博客园";
                dic["Jimmy"] = "softcomz";
                //传入模板所需要的参数
                vc.Put("dic", dic); //设置参数为对象，在模板中可以通过$dic.dudu 来引用

                StringWriter sw = new StringWriter();
                string toEvaluate = "测试, dict value: $dic.dudu, $dic.Jimmy";
                ve.Evaluate(vc, sw, "eval1", toEvaluate);

                Console.WriteLine(sw.GetStringBuilder().ToString());
            }

            // foreach
            {
                var t1 = new TestClass().GetType();
                var fields = t1.GetFields();
                ClassInfo cls = new ClassInfo();
                cls.Name = t1.Name;
                cls.Fields = new List<FieldInfo>();
                foreach (var f in fields)
                {
                    Console.WriteLine("字段类型：" + f.MemberType + "，名字:" + f.Name);                    
                    cls.Fields.Add(f);
                }
                
                VelocityContext vc = new VelocityContext();
                //传入模板所需要的参数
                vc.Put("cls", cls); //设置参数为对象，在模板中可以通过$dic.dudu 来引用

                StringWriter sw = new StringWriter();
                // 注意cls.Name必须是属性，而不是普通的成员变量，要不会无效！
                string toEvaluate = "测试, 类名: $cls.Name。遍历出所有变量： #foreach($f in $cls.Fields) 变量名字： $f.FieldType.Name $f.Name #end";
                ve.Evaluate(vc, sw, "eval1", toEvaluate);

                Console.WriteLine(sw.GetStringBuilder().ToString());
            }

            // 函数/方法
            {
                Util util = new Util();

                VelocityContext vc = new VelocityContext();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic["dudu"] = "博客园";
                dic["Jimmy"] = "softcomz";
                //传入模板所需要的参数
                vc.Put("dic", dic); //设置参数为对象，在模板中可以通过$dic.dudu 来引用
                vc.Put("util", util);

                StringWriter sw = new StringWriter();
                string toEvaluate = "测试, dict value: $dic.dudu, $util.Show($dic.Jimmy)";
                ve.Evaluate(vc, sw, "eval1", toEvaluate);

                Console.WriteLine(sw.GetStringBuilder().ToString());
            }

            Console.WriteLine("按任意键结束...");
            Console.ReadKey();
        }
    }
}
