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
        private byte[] scoresBinFileOrig;
        private byte[] songCacheBinFile;
        private ScoresData scoresData;
        private string scoresbinFilePath;
        private string songcachebinFilePath;
        private static Thickness grdMaximisedMargin = new Thickness(10, 91, 10, 35);
        private static Thickness grdMinimisedMargin = new Thickness(10, 91, 10, 115);

        internal Logger logger;

        public MainWindow()
        {
            InitializeComponent();
            logger = new Logger(txtConsole);
            //todo add manual button loading options

            grdScores.Margin = grdMinimisedMargin;
        }

        private byte[] LoadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                logger.Log("ERROR - File path is empty");
                return null;
            }
            logger.Log("Reading: " + fileName);
            var bytes = File.ReadAllBytes(fileName);
            logger.Log("Finished reading: " + fileName);
            return bytes;
        }

        private void SaveBinaryFile(byte[] data, string fileName)
        {
            logger.Log("Writing binary file: " + fileName);
            File.WriteAllBytes(fileName, data);
            logger.Log("File written: " + fileName);
        }

        private ScoresData ReadInScoresBinFile(string filename){
            //todo make more robust
            scoresBinFile = LoadFile(filename);
            scoresBinFileOrig = LoadFile(filename);
            if (scoresBinFile == null)
            {
                logger.Log("ERROR - Could not read scores file");
                return null;
            }

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

            return new ScoresData(header, listOfLines, logger);//load into data structures
        }

        private bool IsFileModelCompatible(byte[] input, byte[] output)
        {
            if (!input.Length.Equals(output.Length))
            {
                return false;
            }
            if (!input.SequenceEqual(output))
            {
                return false;
            }
            return true;
        }

        private void ReadInSongCacheBinFile(string filename)
        {
            //todo make more robust
            songCacheBinFile = LoadFile(filename);
            if (songCacheBinFile == null)
            {
                logger.Log("ERROR - Could not read song cache file");
            }
        }

        private void BtnSelectScoresFile_Click(object sender, RoutedEventArgs e)
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
            if (scoresData == null)
            {
                logger.Log("ERROR - Scores Data is empty, read it in from a file first");
            }
            else
            {
                try
                {
                    var originalDirectoryPath = System.IO.Path.GetDirectoryName(scoresbinFilePath);
                    var filepath = originalDirectoryPath + "\\scores_modified_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".bin";
                    var backupFilepath = originalDirectoryPath + "\\scores_BACKUP_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".bin";
                    logger.Log("Backing up original file to: " + backupFilepath);

                    File.Copy(scoresbinFilePath, backupFilepath, true);

                    logger.Log("Writing out to: " + scoresbinFilePath);
                    SaveBinaryFile(scoresData.GenerateByteData(), scoresbinFilePath);
                }
                catch (IOException iox)
                {
                    Console.WriteLine(iox.Message);
                }
                
            }
        }

        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            scoresData = ReadInScoresBinFile(scoresbinFilePath);
            if (scoresData == null)
            {
                logger.Log("ERROR - Scores Data is empty");
            }
            else
            {
                //verify here
                if (IsFileModelCompatible(scoresBinFileOrig, scoresData.GenerateByteData())){
                    if (!string.IsNullOrEmpty(songcachebinFilePath))
                    {
                        ReadInSongCacheBinFile(songcachebinFilePath);
                        logger.Log("Enriching song entries with folder path names");
                        scoresData.ScoreEntries.ForEach(x => x.SongFolderName = FilepathEnricher.Enrich(x.GetSongIdentifierAsBytes(), songCacheBinFile));
                        logger.Log("Finished enriching song entries with folder path names");
                    }

                    grdScores.ItemsSource = scoresData.ScoreEntries;
                }
                else
                {
                    //provide error message and contact info
                    logger.Log("Error, the scores file is not compatible with the program, will cause data loss or worse, not loading. Please contact the developer to resolve this mismatch.");
                }
            }            
        }

        private void BtnSelectFileSongCache_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialogSelectFileSongCache = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\LocalLow\\srylain Inc_\\Clone Hero\\",
                Title = "Browse for songcache.bin file",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "bin",
                Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*",
                FilterIndex = 0,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialogSelectFileSongCache.ShowDialog() == true)
            {
                txtSongCache.Text = openFileDialogSelectFileSongCache.FileName;
                songcachebinFilePath = openFileDialogSelectFileSongCache.FileName;
            }

        }

        private void BtnToggleConsole_Click(object sender, RoutedEventArgs e)
        {
            if (grdScores.Margin.Equals(grdMaximisedMargin))
            {
                grdScores.Margin = grdMinimisedMargin;
            }
            else
            {
                grdScores.Margin = grdMaximisedMargin;
            }
        }
    }
}
