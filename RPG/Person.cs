using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class Person
    {
        public int health;
        public int attack;
        public int mana;

        public void TakeDamage(int dmg)
        {
            health -= dmg;
        }
    }
}
