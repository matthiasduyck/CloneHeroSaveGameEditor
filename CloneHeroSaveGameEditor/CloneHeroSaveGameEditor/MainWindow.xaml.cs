using CloneHeroSaveGameEditor.Models;
using Microsoft.Win32;
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
        private string scoresbinFilePath;


        public MainWindow()
        {
            InitializeComponent();

            //todo add manual button loading options


            


            
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
            var header = scoresBinFile.Take(8);
            scoresBinFile = scoresBinFile.Skip(9).ToArray();

            while (scoresBinFile.Length > 0)
            {

                List<byte> line = new List<byte>();
                while (scoresBinFile.Length > 0 && !(scoresBinFile.First().Equals(delimiter) && (line.Count.Equals(49) || line.Count.Equals(62) || line.Count.Equals(75))))
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

            return new ScoresData(header, listOfLines);//load into data structures
        }

        private void BtnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\LocalLow\\srylain Inc_\\Clone Hero\\",
                Title = "Browse for scores.bin file",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "bin",
                Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*",
                FilterIndex = 0,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == true)
            {
                txtScoresFile.Text = openFileDialog1.FileName;
                scoresbinFilePath = openFileDialog1.FileName;
            }

        }

        private void BtnWrite_Click(object sender, RoutedEventArgs e)
        {
            var originalDirectoryPath = System.IO.Path.GetDirectoryName(scoresbinFilePath);
            var filepath = originalDirectoryPath + "\\scores_modified_"+ DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".bin";
            SaveBinaryFile(scoresData.GenerateByteData(), filepath);//todo replace with scoresbinfilepath
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            scoresData = ReadInScoresBinFile(scoresbinFilePath);
            //todo put in gridview
            grdScores.ItemsSource = scoresData.ScoreEntries;
        }
    }
}
