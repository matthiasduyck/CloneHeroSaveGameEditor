using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloneHeroSaveGameEditor.Models
{
    public class ScoreEntry
    {
        public string SongFolderName { get; set; }
        public string SongIdentifier { get; set; }// first part of the score entry, 20 character long hex string, can be linked in the songcache.bin file to the actual file // blok(h)9F-BE length (h)20
        public byte unknown29 { get; set; }//todo some unknown field at 29 after identifier
        public int PlayCount { get; set; }//simple hex number at 2A
        //todo 4 hex fields I don't understand from 2B to 2E, maybe just padding???
        public byte unknown2B { get; set; }
        public byte unknown2C { get; set; }
        //public byte ScoreType { get; set; } //represents score type(guitar/bass/...) 
        public string ScoreType { get; set; }

        public byte unknown2E { get; set; }
        public string Difficulty { get; set; }//hex number at 2F representing difficulty, 00=expert, 01 = hard, 02=medium, 03=easy, 
        public int Percentage { get; set; }//hex number at 30 representing percentage score
        public bool HasCrown { get; set; }//hex value at 31, looks like a bool indicating the 'crown'???
        //todo 4 hex fields I don't understand yet from 32-35, probably contains some useful data, maybe modifiers?
        public int NoteSpeed { get; set; }
        public byte unknown33 { get; set; }
        public byte unknown34 { get; set; }
        public byte unknown35 { get; set; }
        public int Score { get; set; } //hex number from 36 to 39, 4 long, contains score in little endian int32

        //second score
        public bool hasSecondScore { get; set; }
        
        public string ScoreType2 { get; set; }
        public byte unknown3B { get; set; }
        public string Difficulty2 { get; set; }
        public int Percentage2 { get; set; }
        public bool HasCrown2 { get; set; }
        public int NoteSpeed2 { get; set; }
        public byte unknown40 { get; set; }
        public byte unknown41 { get; set; }
        public byte unknown42 { get; set; }
        public int Score2 { get; set; }

        //third score?
        public bool hasThirdScore { get; set; }

        public string ScoreType3 { get; set; }
        public byte unknown3_2 { get; set; }
        public string Difficulty3 { get; set; }
        public int Percentage3 { get; set; }
        public bool HasCrown3 { get; set; }
        public int NoteSpeed3 { get; set; }
        public byte unknown3_4 { get; set; }
        public byte unknown3_5 { get; set; }
        public byte unknown3_6 { get; set; }
        public int Score3 { get; set; }

        public byte[] ConvertToBytesArray()
        {
            var byteDataList = new List<byte>();
            byteDataList.AddRange(Encoding.ASCII.GetBytes(SongIdentifier).ToList());
            byteDataList.Add(unknown29);//todo unknown
            byteDataList.Add(Convert.ToByte(PlayCount));
            byteDataList.Add(unknown2B);//todo unknown
            byteDataList.Add(unknown2C);//todo unknown
            byteDataList.Add(ScoreStringToBytes(ScoreType));//todo unknown
            byteDataList.Add(unknown2E);//todo unknown
            byteDataList.Add(DifficultyStringToBytes(Difficulty));
            byteDataList.Add(Convert.ToByte(Percentage));
            byteDataList.Add(Convert.ToByte(HasCrown));
            byteDataList.Add(Convert.ToByte(NoteSpeed));
            byteDataList.Add(unknown33);//todo unknown
            byteDataList.Add(unknown34);//todo unknown
            byteDataList.Add(unknown35);//todo unknown
            byteDataList.AddRange(BitConverter.GetBytes(Score));

            if (hasSecondScore)
            {
                byteDataList.Add(ScoreStringToBytes(ScoreType2));//todo unknown
                byteDataList.Add(unknown3B);//todo unknown
                byteDataList.Add(DifficultyStringToBytes(Difficulty2));
                byteDataList.Add(Convert.ToByte(Percentage2));
                byteDataList.Add(Convert.ToByte(HasCrown2));
                byteDataList.Add(Convert.ToByte(NoteSpeed2));
                byteDataList.Add(unknown40);//todo unknown
                byteDataList.Add(unknown41);//todo unknown
                byteDataList.Add(unknown42);//todo unknown
                byteDataList.AddRange(BitConverter.GetBytes(Score2));
            }
            if (hasThirdScore)
            {
                byteDataList.Add(ScoreStringToBytes(ScoreType3));//todo unknown
                byteDataList.Add(unknown3_2);//todo unknown
                byteDataList.Add(DifficultyStringToBytes(Difficulty3));
                byteDataList.Add(Convert.ToByte(Percentage3));
                byteDataList.Add(Convert.ToByte(HasCrown3));
                byteDataList.Add(Convert.ToByte(NoteSpeed3));
                byteDataList.Add(unknown3_4);//todo unknown
                byteDataList.Add(unknown3_5);//todo unknown
                byteDataList.Add(unknown3_6);//todo unknown
                byteDataList.AddRange(BitConverter.GetBytes(Score3));
            }
            

            return byteDataList.ToArray();
        }

        public ScoreEntry(List<byte> fromBytes)
        {
            //todo decode frombytes to all relevant data
            SongIdentifier = Encoding.Default.GetString(fromBytes.Take(32).ToArray());
            unknown29 = fromBytes[32];
            PlayCount = fromBytes[33];
            unknown2B = fromBytes[34];
            unknown2C = fromBytes[35];
            ScoreType =  ScoreTypeBytesToString(fromBytes[36]);
            unknown2E = fromBytes[37];
            Difficulty = DifficultyBytesToString(fromBytes[38]);
            Percentage = fromBytes[39];
            HasCrown = fromBytes[40].Equals(1);
            NoteSpeed = fromBytes[41];
            unknown33 = fromBytes[42];
            unknown34 = fromBytes[43];
            unknown35 = fromBytes[44];

            //if (fromBytes.Count > 45)
            //{
                Score = BitConverter.ToInt32(fromBytes.Skip(45).Take(4).ToArray(), 0);
            //}
            

            //looks like we have more than just one score
            if (fromBytes.Count > 49)
            {
                hasSecondScore = true;

                ScoreType2 = ScoreTypeBytesToString(fromBytes[49]);//score type (guitar/bass/etc)
                unknown3B = fromBytes[50];
                Difficulty2 = DifficultyBytesToString(fromBytes[51]);
                Percentage2 = fromBytes[52];
                HasCrown2 = fromBytes[53].Equals(1);
                NoteSpeed2 = fromBytes[54];//note speed
                unknown40 = fromBytes[55];
                unknown41 = fromBytes[56];
                unknown42 = fromBytes[57];
                Score2 = BitConverter.ToInt32(fromBytes.Skip(58).Take(4).ToArray(), 0);
            }

            //looks like we have even more scores
            if (fromBytes.Count > 62)
            {
                hasThirdScore = true;

                ScoreType3 = ScoreTypeBytesToString(fromBytes[62]);
                unknown3_2 = fromBytes[63];
                Difficulty3 = DifficultyBytesToString(fromBytes[64]);
                Percentage3 = fromBytes[65];
                HasCrown3 = fromBytes[66].Equals(1);
                NoteSpeed3 = fromBytes[67];
                unknown3_4 = fromBytes[68];
                unknown3_5 = fromBytes[69];
                unknown3_6 = fromBytes[70];
                Score3 = BitConverter.ToInt32(fromBytes.Skip(71).Take(4).ToArray(), 0);
            }
        }

        public byte[] GetSongIdentifierAsBytes()
        {
            int NumberChars = SongIdentifier.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(SongIdentifier.Substring(i, 2), 16);
            return bytes;
        }

        //0 = guitar, 8 = bass?, 7 = keys?, 2 = ???
        public static string ScoreTypeBytesToString(byte score)
        {
            var result = Convert.ToString(score);
            switch (score)
            {
                case 0:
                    result = "guitar";
                    break;
                case 8:
                    result = "bass";
                    break;
                case 7:
                    result = "keys";
                    break;
                default:
                    break;
            }

            return result;
        }

        public static byte ScoreStringToBytes(string score)
        {
            byte result;
            switch (score)
            {
                case "guitar":
                    result = 0;
                    break;
                case "bass":
                    result = 8;
                    break;
                case "keys":
                    result = 7;
                    break;
                default:
                    result = Convert.ToByte(score);
                    break;
            }

            return result;
        }

        //00=expert, 01 = hard, 02=medium, 03=easy
        public static string DifficultyBytesToString(byte score)
        {
            var result = Convert.ToString(score);
            switch (score)
            {
                case 0:
                    result = "expert";
                    break;
                case 1:
                    result = "hard";
                    break;
                case 2:
                    result = "medium";
                    break;
                case 3:
                    result = "easy";
                    break;
                default:
                    break;
            }

            return result;
        }

        public static byte DifficultyStringToBytes(string difficulty)
        {
            byte result;
            switch (difficulty)
            {
                case "expert":
                    result = 0;
                    break;
                case "hard":
                    result = 1;
                    break;
                case "medium":
                    result = 2;
                    break;
                case "easy":
                    result = 3;
                    break;
                default:
                    result = Convert.ToByte(difficulty);
                    break;
            }

            return result;
        }
    }
}