using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloneHeroSaveGameEditor.Models
{
    class ScoreEntry
    {
        string SongIdentifier;// first part of the score entry, 20 character long hex string, can be linked in the songcache.bin file to the actual file // blok(h)9F-BE length (h)20
        byte unknown29;//todo some unknown field at 29 after identifier
        int PlayCount;//simple hex number at 2A
        //todo 4 hex fields I don't understand from 2B to 2E, maybe just padding???
        byte unknown2B;
        byte unknown2C;
        byte unknown2D;
        byte unknown2E;
        int Difficulty;//hex number at 2F representing difficulty, 02=easy 03=medium
        int Percentage;//hex number at 30 representing percentage score
        bool HasCrown;//hex value at 31, looks like a bool indicating the 'crown'???
        //todo 4 hex fields I don't understand yet from 32-35, probably contains some useful data, maybe modifiers?
        byte unknown32;
        byte unknown33;
        byte unknown34;
        byte unknown35;
        int Score; //hex number from 36 to 39, 4 long, contains score in little endian int32

        //bass guitar
        bool hasBass = false;
        byte unknown3A;
        byte unknown3B;
        int Difficulty2Maybe;
        int Percentage2;
        bool HasCrown2Maybe;
        byte unknown3F;
        byte unknown40;
        byte unknown41;
        byte unknown42;
        int Score2;

        public byte[] ConvertToBytesArray()
        {
            var byteDataList = new List<byte>();
            byteDataList.AddRange(Encoding.ASCII.GetBytes(SongIdentifier).ToList());
            byteDataList.Add(unknown29);//todo unknown
            byteDataList.Add(Convert.ToByte(PlayCount));
            byteDataList.Add(unknown2B);//todo unknown
            byteDataList.Add(unknown2C);//todo unknown
            byteDataList.Add(unknown2D);//todo unknown
            byteDataList.Add(unknown2E);//todo unknown
            byteDataList.Add(Convert.ToByte(Difficulty));
            byteDataList.Add(Convert.ToByte(Percentage));
            byteDataList.Add(Convert.ToByte(HasCrown));
            byteDataList.Add(unknown32);//todo unknown
            byteDataList.Add(unknown33);//todo unknown
            byteDataList.Add(unknown34);//todo unknown
            byteDataList.Add(unknown35);//todo unknown
            byteDataList.AddRange(BitConverter.GetBytes(Score));

            if (hasBass)
            {
                byteDataList.Add(unknown3A);//todo unknown
                byteDataList.Add(unknown3B);//todo unknown
                byteDataList.Add(Convert.ToByte(Difficulty2Maybe));
                byteDataList.Add(Convert.ToByte(Percentage2));
                byteDataList.Add(Convert.ToByte(HasCrown2Maybe));
                byteDataList.Add(unknown3F);//todo unknown
                byteDataList.Add(unknown40);//todo unknown
                byteDataList.Add(unknown41);//todo unknown
                byteDataList.Add(unknown42);//todo unknown
                byteDataList.AddRange(BitConverter.GetBytes(Score2));
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
            unknown2D = fromBytes[36];
            unknown2E = fromBytes[37];
            Difficulty = fromBytes[38];
            Percentage = fromBytes[39];
            HasCrown = fromBytes[40].Equals(1);
            unknown32 = fromBytes[41];
            unknown33 = fromBytes[42];
            unknown34 = fromBytes[43];
            unknown35 = fromBytes[44];

            //if (fromBytes.Count > 45)
            //{
                Score = BitConverter.ToInt32(fromBytes.Skip(45).Take(4).ToArray(), 0);
            //}
            

            //looks like we have more than just the lead
            if (fromBytes.Count > 49)
            {
                hasBass = true;

                unknown3A = fromBytes[49];
                unknown3B = fromBytes[50];
                Difficulty2Maybe = fromBytes[51];
                Percentage2 = fromBytes[52];
                HasCrown2Maybe = fromBytes[53].Equals(1);
                unknown3F = fromBytes[54];
                unknown40 = fromBytes[55];
                unknown41 = fromBytes[56];
                unknown42 = fromBytes[57];
                Score2 = BitConverter.ToInt32(fromBytes.Skip(58).Take(4).ToArray(), 0);
            }

            //looks like we have even more scores
            if (fromBytes.Count > 62)
            {

            }
        }
    }

    
}