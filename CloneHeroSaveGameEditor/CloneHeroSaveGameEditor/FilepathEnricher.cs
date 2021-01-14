using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneHeroSaveGameEditor
{
    static class FilepathEnricher
    {
        public static int Locate(this byte[] self, byte[] candidate)
        {
            if (IsEmptyLocate(self, candidate))
                return -1;

            var list = new List<int>();

            for (int i = 0; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate))
                    continue;

                return i;
            }

            return -1;
        }

        static bool IsMatch(byte[] array, int position, byte[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;

            return true;
        }

        static bool IsEmptyLocate(byte[] array, byte[] candidate)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length;
        }


        public static string FindAndBuildUpPath(byte[] songcachefile, int startingIndex)
        {
            List<byte> folderPath = new List<byte>();
            while (startingIndex>0 && !songcachefile[startingIndex].Equals(92) && folderPath.Count<261)//probably no path longer than 260 chars
            {
                folderPath.Add(songcachefile[startingIndex]);
                startingIndex--;
            }

            if (folderPath.Count>0)
            {
                //we have the full path, convert to text now
                folderPath.Reverse();
                folderPath = folderPath.TakeWhile(x=>!x.Equals(0)).ToList();

                return Encoding.Default.GetString(folderPath.ToArray());
            }

            return "";
        }

        public static string Enrich(byte[] identifier, byte[] songcachefile)
        {
            if (identifier.Length > 0 && songcachefile.Length>0)
            {
                //var location = SinglePatternAt(songcachefile, identifier);
                var location = songcachefile.Locate(identifier);
                if (location != -1)
                {
                    return FindAndBuildUpPath(songcachefile, location - 89);
                }
            }

            return "";
        }
    }
}
