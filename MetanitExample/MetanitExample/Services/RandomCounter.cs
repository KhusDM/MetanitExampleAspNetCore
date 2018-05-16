using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetanitExample.Services
{
    public class RandomCounter : ICounter
    {
        static Random random = new Random();
        private int value;

        public RandomCounter()
        {
            this.value = random.Next(0, 1000000);
        }

        public int Value { get { return this.value; } }
    }
}
