using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balatro_Deck_Maker_Prototype_1.Classes
{
    public class Card
    {
        // this is setting up the card object, and setting default values for the computer to not hate itself and implode 
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public SuitTypes SuitType { get; set; }
        public EffectTypes EffectType { get; set; }
        public string Rarity { get; set; } = string.Empty;
        public ActivationTypes ActivationType { get; set; }
        public HandTypes HandType { get; set; }
        public Card()
        {
            Name = string.Empty;
        }
        public Card(SQLiteDataReader reader)
        {
            try
            {
                Id = reader.GetInt32(0);
                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                SuitType = (SuitTypes) reader.GetInt16(2);
                EffectType = (EffectTypes)reader.GetInt16(3);
                Rarity = reader.IsDBNull(4) ? string.Empty : reader.GetString(4);
                ActivationType = (ActivationTypes)reader.GetInt16(5);
                HandType = (HandTypes)reader.GetInt16(6);
            }
            catch(SqliteException sqlLiteException)
            {
                Console.WriteLine("Something is wrong with the Database", sqlLiteException);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Your shits fucked.  Error code: {ex.Message}");
            }

        }

    }
}
