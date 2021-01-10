using CloneHeroSaveGameEditor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CloneHeroSaveGameEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private byte[] scoresBinFile;
        private ScoresData scoresData;
        public MainWindow()
        {
            InitializeComponent();

            //todo add manual button loading options


            scoresData = ReadInScoresBinFile(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\LocalLow\\srylain Inc_\\Clone Hero\\scores.bin");

            var output = scoresData.GenerateByteData();

            SaveBinaryFile(output, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\LocalLow\\srylain Inc_\\Clone Hero\\scoresOUTPUT.bin");
        }

        private byte[] LoadFile(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        private void SaveBinaryFile(byte[] data, string fileName)
        {
            File.WriteAllBytes(fileName, data); // Requires System.IO
        }

        private ScoresData ReadInScoresBinFile(string filename){
            //todo make more robust
            scoresBinFile = LoadFile(filename);

            //load in vars
            byte delimiter = 32;//todo put somewhere shared


            List<List<byte>> listOfLines = new List<List<byte>>();

            while (scoresBinFile.Length > 0)
            {

                List<byte> line = new List<byte>();
                while (scoresBinFile.Length > 0 && !scoresBinFile.First().Equals(delimiter))
                {
                    line.Add(scoresBinFile.First());
                    scoresBinFile = scoresBinFile.Skip(1).ToArray();
                }
                if (scoresBinFile.Length > 0)
                {
                    scoresBinFile = scoresBinFile.Skip(1).ToArray();//remove the delimiter
                }

                listOfLines.Add(line);

            }

            return new ScoresData(listOfLines);//load into data structures
        }
    }
}
