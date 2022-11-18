using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using static System.Collections.Specialized.BitVector32;

namespace SimpleLauncher
{

    public class Logger
    {
        public static Logger Instance = new Logger();
        private RichTextBox rtb;
        Paragraph paragraph;

        private static Brush DefaultColor = Brushes.White;
        public static void Init(RichTextBox tb = null)
        {
            Logger.Instance.rtb = tb;
            Logger.Instance.paragraph = new Paragraph();
            Logger.Instance.rtb.Document = new FlowDocument();
            Logger.Instance.rtb.Document.Blocks.Add(Logger.Instance.paragraph);
        }

        private static void Write(string msg, Brush color)
        {

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {

                Run item = new Run(msg);
                item.Foreground = color;
                Instance.paragraph.Inlines.Add(item);
            }));

        }
        private static void WriteLine(string msg, Brush color)
        {

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {

                Run item = new Run(msg);
                item.Foreground = color;
                Instance.paragraph.Inlines.Add(item);

                Instance.paragraph.Inlines.Add(new LineBreak());
                Instance.rtb.ScrollToEnd();
            }));
        }

        public static void Error(string txt,StackTrace trace=null)
        {
            Write("[错误]", Brushes.Red);

            WriteLine(txt, DefaultColor);
            if (trace!=null)
            {
                WriteLine(trace.ToString(), Brushes.OrangeRed);
            }

        }
        public static void Info(string txt)
        {

            Write("[信息]", Brushes.Green);

            WriteLine(txt, DefaultColor);
        }
        public static void Warn(string txt)
        {
            Write("[警告]", Brushes.Yellow);

            WriteLine(txt, DefaultColor);

        }
        public static void Debug(string txt, [CallerFilePath] string filePath = "",
            [CallerLineNumber] int line = 0, [CallerMemberName] string member = "")
        {
#if DEBUG
            Write("[调试]", Brushes.White);

            Write($"[{Path.GetFileName(filePath)}.{member}:{line}]", Brushes.Gray);

            WriteLine(txt, DefaultColor);
#endif
        }




    }
}
