using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloneHeroSaveGameEditor.Models
{
    class ScoreEntry
    {
        string SongIdentifier;// first part of the score entry, 20 character long hex string, can be linked in the songcache.bin file to the actual file // blok(h)9F-BE length (h)20
        //todo some unknown field at 29 after identifier
        int PlayCount;//simple hex number at 2A
        //todo 4 hex fields I don't understand from 2B to 2E, maybe just padding???
        int Difficulty;//hex number at 2F representing difficulty, 02=easy 03=medium
        int Percentage;//hex number at 30 representing percentage score
        bool HasCrown;//hex value at 31, looks like a bool indicating the 'crown'???
        //todo 4 hex fields I don't understand yet from 32-35, probably contains some useful data, maybe modifiers?
        int Score; //hex number from 36 to 39, 4 long, contains score in little endian int32


        public byte[] ConvertToBytesArray()
        {
            var byteDataList = new List<byte>();
            byteDataList.AddRange(Encoding.ASCII.GetBytes(SongIdentifier).ToList());
            byteDataList.Add(1);//todo unknown
            byteDataList.Add(Convert.ToByte(PlayCount));
            byteDataList.Add(0);//todo unknown
            byteDataList.Add(0);//todo unknown
            byteDataList.Add(0);//todo unknown
            byteDataList.Add(0);//todo unknown
            byteDataList.Add(Convert.ToByte(Difficulty));
            byteDataList.Add(Convert.ToByte(Percentage));
            byteDataList.Add(Convert.ToByte(HasCrown));
            byteDataList.Add(0);//todo unknown
            byteDataList.Add(0);//todo unknown
            byteDataList.Add(0);//todo unknown
            byteDataList.Add(0);//todo unknown
            byteDataList.AddRange(BitConverter.GetBytes(Score));
            return byteDataList.ToArray();
        }

        public ScoreEntry(List<byte> fromBytes)
        {
            //todo decode frombytes to all relevant data
            SongIdentifier = Encoding.Default.GetString(fromBytes.Take(32).ToArray());
            PlayCount = fromBytes[33];
            Difficulty = fromBytes[38];
            Percentage = fromBytes[39];
            HasCrown = fromBytes[40].Equals(1);
            Score = BitConverter.ToInt32(fromBytes.Skip(45).Take(4).ToArray(), 0);
        }
    }

    
}