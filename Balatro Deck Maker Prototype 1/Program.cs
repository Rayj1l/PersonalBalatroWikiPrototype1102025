using Balatro_Deck_Maker_Prototype_1.Classes;
using System;
using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;



class DatabaseConnector
{
    private string connectionString;

    public DatabaseConnector(string dbPath)
    {
        connectionString = $"Data Source = {dbPath} ; Version = 3;";
    }
    
    public List<Card> ExecuteQuery(string sortMethod, string? selectedValue = null)
    {
        List<Card> cards = new List<Card>();

        try
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT Number, Name, Suit, Effect_Type, Rarity, Activation_Type, Hand_Type FROM Jokers {sortMethod}";

                using SQLiteCommand command = new SQLiteCommand (query, connection);
                {
                    command.Parameters.AddWithValue("@selectedValue", $"%{selectedValue}%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var newCard = new Card(reader);
                            cards.Add(newCard);
                        }
                    }
                }
            }
        } catch (Exception ex)
        {
            Console.WriteLine("Your shits fucked.  Error code: " + ex.Message);
        }

        return cards;
    }
    //specific sort methods
}

class program
{
    static void Main()
    {
        Console.WriteLine("Enter database path");
        var dbpath = Console.ReadLine();

        Console.WriteLine("Choose information type:\nA. All Card  \nB. Effect Type  \nC. Suit  \nD. Hand Type  \nE. Activation Type");
        var infoType = Console.ReadLine();

        string? selectedValue = null;

        if (!string.IsNullOrEmpty(infoType))
        {
            //switch statement to determine sort method (if any
            string sortMethod = infoType.ToLower() switch
            {
                "b" => "WHERE Effect_Type LIKE @selectedValue",
                "c" => "WHERE Suit LIKE @selectedValue",
                "d" => "WHERE Hand_Type LIKE @selectedValue",
                "e" => "WHERE Activation_Type LIKE @selectedValue",
                _ => string.Empty
            };
        }

        if (infoType == "B" || infoType == "b")
        {
            Console.WriteLine("Enter Effect Type (Write as seen) \n+$. \n+C, \n+M, \n*M, \nEffect, \nRetrigger");
            selectedValue = Console.ReadLine();
        } else if (infoType == "C" || infoType == "c")
        {
            Console.WriteLine("Enter Suit (Write as seen) \nDiamonds, \nHearts, \nClubs, \nSpades");
            selectedValue = Console.ReadLine();
        } else if (infoType == "D" || infoType == "d")
        {
            Console.WriteLine("Enter Hand Type");
            selectedValue = Console.ReadLine();
        } else if (infoType == "E" || infoType == "e")
        {
            Console.WriteLine("Enter Activation Type (Write as seen) \nIndependent, \nOn Scored, \nOn Purchase, \nOn Held");
            selectedValue = Console.ReadLine();
        }

        DatabaseConnector dbConnector = new DatabaseConnector(dbpath);
        List<Cards> cards = dbConnector.ExecuteQuery(sortMethod, selectedValue);

        if (cards.Count > 0)
        {
            Console.WriteLine("\nCards that meet the Criteria: \n");
            foreach (var Cards in cards)
            {
                Console.WriteLine($"ID: {Cards.Id} , Name: {Cards.Name}  ,  Suit: {Cards.Suit}  ,  Effect Type: {Cards.EffectType}  ,  Rarity: {Cards.Rarity}  ,  How to activate: {Cards.ActivType}  ,  Applies to this hand: {Cards.HandType} \n  ");
            }
        }
    }
}