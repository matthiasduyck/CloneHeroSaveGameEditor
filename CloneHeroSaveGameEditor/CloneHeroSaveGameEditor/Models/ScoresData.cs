﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneHeroSaveGameEditor.Models
{
    public class ScoresData
    {
        public string Weirdheadertodo { get; set; }//not sure what this should be or represents
        internal List<ScoreEntry> ScoreEntries { get; set; }

        private Logger Logger;

        public ScoresData(IEnumerable<byte> header, List<List<byte>> listOfLines, Logger logger)
        {
            Logger = logger;
            ScoreEntries = new List<ScoreEntry>();
            Weirdheadertodo = Encoding.Default.GetString(header.ToArray());
            

            foreach(var line in listOfLines)
            {
                ScoreEntries.Add(new ScoreEntry(line));
            }
        }

        public byte[] GenerateByteData()
        {
            Logger.Log("Generating bytes");
            byte delimiter = 32;//todo put somewhere shared
            var byteDataList = new List<byte>();
            byteDataList.AddRange(Encoding.Default.GetBytes(Weirdheadertodo).ToList());
            foreach(var scoreEntry in ScoreEntries)
            {
                byteDataList.Add(delimiter);
                byteDataList.AddRange(scoreEntry.ConvertToBytesArray());
            }
            Logger.Log("Bytes generated");
            return byteDataList.ToArray();
        }
    }
}
