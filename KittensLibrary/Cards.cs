﻿using KittensLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KittensLibrary
{
    public static class Cards
    {
        public readonly static Dictionary<string, Card> cards = new Dictionary<string, Card>()
           {
                 { "Exploding Kitten", new Card("Exploding Kitten", CardType.ExplodingKitten, "exploding_kitten.png")},
                 { "Defuse", new Card("Defuse", CardType.Defuse, "defuse.jpeg")},
                 { "Attack", new Card("Attack", CardType.Attack, "attack.jpeg")},
                 { "Skip", new Card("Skip", CardType.Skip, "skip.jpeg")},
                 { "Shuffle", new Card("Shuffle", CardType.Shuffle, "shuffle.jpeg")},
                 { "Steal", new Card("Steal", CardType.Steal, "steal.jpeg")},
                 { "See the future", new Card("See the future", CardType.SeeTheFuture, "see_future.jpeg")},
                 { "", new Card("", CardType.None, "")}
             };

        public readonly static Dictionary<CardType, Card> typeCards = new()
             {
                 { CardType.ExplodingKitten, new Card("Exploding Kitten", CardType.ExplodingKitten, "exploding_kitten.png")},
                 { CardType.Defuse, new Card("Defuse", CardType.Defuse, "defuse.jpeg")},
                 { CardType.Attack, new Card("Attack", CardType.Attack, "attack.jpeg")},
                 { CardType.Skip, new Card("Skip", CardType.Skip, "skip.jpeg")},
                 { CardType.Shuffle, new Card("Shuffle", CardType.Shuffle, "shuffle.jpeg")},
                 { CardType.Steal, new Card("Steal", CardType.Steal, "steal.jpeg")},
                 { CardType.SeeTheFuture, new Card("See the future", CardType.SeeTheFuture, "see_future.jpeg")},
                 {CardType.None, new Card("", CardType.None, "") }
             };

    }
}
