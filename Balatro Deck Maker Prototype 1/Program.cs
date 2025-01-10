using System;
using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

class Cards
{
    // this is setting up the card object, and setting default values for the computer to not hate itself and implode 
    public int Id { get; set; } = 0;
    public string Name { get; set; } = string.Empty;
    public string? Suit { get; set; } = string.Empty;
    public string EffectType { get; set; } = string.Empty;
    public string Rarity { get; set; } = string.Empty;
    public string ActivType { get; set; } = string.Empty;
    public string? HandType { get; set; } = string.Empty;

}

class DatabaseConnector
{
    private string connectionString;

    public DatabaseConnector(string dbPath)
    {
        connectionString = $"Data Source = {dbPath} ; Version = 3;";
    }

    private Cards CreateNewCard(SQLiteDataReader reader)
    {
        return new Cards
        {
            Id = reader.GetInt32(0),
            Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
            Suit = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
            EffectType = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            Rarity = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
            ActivType = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
            HandType = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
        };
    }

    public List<Cards> ExecuteQuery(string sortMethod, string? selectedValue = null)
    {
        List<Cards> cards = new List<Cards>();

        try
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT Number, Name, Suit, Effect_Type, Rarity, Activation_Type, Hand_Type FROM Jokers {sortMethod}";

                using SQLiteCommand command = new SQLiteCommand (query, connection);
                {
                    command.Parameters.AddWithValue("@selectedValue", "%" + selectedValue + "%");

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cards.Add(CreateNewCard(reader));
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
        string? dbpath = Console.ReadLine();

        Console.WriteLine("Choose information type:\nA. All Cards  \nB. Effect Type  \nC. Suit  \nD. Hand Type  \nE. Activation Type");
        string? infoType = Console.ReadLine();

        string? selectedValue = null;
        string sortMethod = infoType switch
        {
            "A" or "a" => string.Empty,
            "B" or "b" => "WHERE Effect_Type LIKE @selectedValue",
            "C" or "c" => "WHERE Suit LIKE @selectedValue",
            "D" or "d" => "WHERE Hand_Type LIKE @selectedValue",
            "E" or "e" => "WHERE Activation_Type LIKE @selectedValue",
            _ => string.Empty
        };

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