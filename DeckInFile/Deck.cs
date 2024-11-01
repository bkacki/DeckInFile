using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace DeckInFile
{
    class Deck : ObservableCollection<Card>
    {
        private static Random random = new Random();
        public Deck()
        {
            Reset();
        }

        public Deck(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    var nextCard = reader.ReadLine();
                    var cardParts = nextCard.Split(new char[] { ' ' });
                    var suit = cardParts[1] switch 
                    { 
                        "karo" => Suits.karo,
                        "trefl" => Suits.trefl,
                        "kier" => Suits.kier,
                        "pik" => Suits.pik,
                        _ => throw new InvalidDataException($"No suit for {cardParts[1]}")
                    };
                    var value = cardParts[0] switch
                    {
                        "As" => Values.As,
                        "Dwójka" => Values.Dwójka,
                        "Trójka" => Values.Trójka,
                        "Czwórka" => Values.Czwórka,
                        "Piątka" => Values.Piątka,
                        "Szóstka" => Values.Szóstka,
                        "Siódemka" => Values.Siódemka,
                        "Ósemka" => Values.Ósemka,
                        "Dziewiątka" => Values.Dziewiątka,
                        "Dziesiątka" => Values.Dziewiątka,
                        "Walet" => Values.Walet,
                        "Dama" => Values.Dama,
                        "Król" => Values.Król,
                        _ => throw new InvalidDataException($"No value for {cardParts[0]}")
                    };
                    Add(new Card(value, suit));
                }
            }
        }

        public void Reset()
        {
            /* Call Clear() to remove all cards from the deck, then use two for loops to add
            * all combinations of suit and value, calling Add(new Card(...)) to add each card */
            Clear();

            for(int suit = 0; suit <= 3; suit++)
            {
                for(int value = 1; value <= 13; value++)
                    Add(new Card((Values)value, (Suits)suit));
            }
        }
        public Card Deal(int index)
        {
            Card cardToDeal = base[index];
            RemoveAt(index);
            return cardToDeal;
        }
        public void Shuffle()
        {
            List<Card> copy = new List<Card>(this);
            Clear();
            while (copy.Count > 0)
            {
                int index = random.Next(copy.Count);
                Card card = copy[index];
                copy.RemoveAt(index);
                Add(card);
            }
        }
        public void Sort()
        {
            List<Card> sortedCards = new List<Card>(this);
            sortedCards.Sort(new CardComparerByValue());
            Clear();
            foreach (Card card in sortedCards)
            {
                Add(card);
            }
        }

        public void WriteCards(string filename)
        {
            using(var writer = new StreamWriter(filename, false))
            {
                List<Card> copy = new List<Card>(this);
                foreach(var card in copy)
                    writer.WriteLine(card.Name);
            }
        }
    }
}
