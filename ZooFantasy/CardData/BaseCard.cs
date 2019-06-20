using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZooFantasy.CardData
{
    public class BaseCard
    {
        private int cost;
        public int Cost { get { return cost; } }
        private string name;
        public string Name { get { return name; } }
        private string description;
        public string Description { get { return description; } }



        public BaseCard()
        { }
        public BaseCard(int CardCost, string CardName,string CardDescription)
        {
            cost = CardCost;
            name = CardName;
            description = CardDescription;
        }

    }
}
