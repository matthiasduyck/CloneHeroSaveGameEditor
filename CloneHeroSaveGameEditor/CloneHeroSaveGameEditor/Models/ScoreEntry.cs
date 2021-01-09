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
        bool Score; //hex number from 36 to 39, 4 long, contains score in little endian int32


        public byte[] ConvertToBytesArray()
        {
            
            return null;//todo
        }

        public ScoreEntry(byte[] fromBytes)
        {
            //todo decode frombytes to all relevant data
        }
    }

    
}