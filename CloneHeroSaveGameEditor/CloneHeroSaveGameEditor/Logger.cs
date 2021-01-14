using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CloneHeroSaveGameEditor
{
    public class Logger
    {
        private TextBox TextBox;
        public Logger(TextBox textBox)
        {
            TextBox = textBox;
        }
        public void Log(string text)
        {
            TextBox.Text += Environment.NewLine + text;
            TextBox.ScrollToEnd();
        }
    }
}
