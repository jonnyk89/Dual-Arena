using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class Player : Person
    {
        // Characteristic:
        public string name;

        // Potions
        public int healthPotions = 1;
        public int manaPotions = 1;

        // Specials:

        // Fireball
        public int fireball = 30;
        public int costF = 50;

        // Might
        public int boost = 10;
        public int costM = 100;

        // Stun
        public int durationS;
        public int costS = 50;

        // Debuff
        public int durationD;
        public int debuff = 5;
        public int costD = 50;

        public Player(string name)
        {
            this.name = name;
            health = 100;
            attack = 10;
            mana = 200 ;
        }

        // Abilities
        public void Attack(Orc orc)
        {
            orc.TakeDamage(attack);
            Console.WriteLine("{0} used Attack and did {1} damage to Orc.", name, attack);
        }

        public void Fireball(Orc orc)
        {
            mana -= costF;
            orc.TakeDamage(fireball);
            Console.WriteLine("{0} used Fireball for {1} mana and did {2} damage to Orc.", name, costF, fireball);
        }

        public void Might()
        {
            mana -= costM;
            attack += boost;
            Console.WriteLine("{0} used Might for {1} mana and added {2} damage to his Attack.", name, costM, boost);
        }

        public void Stun(Orc orc)
        {
            mana -= costS;
            durationS = 3;
            if (durationS > 0)
            {
                orc.can_attack = false;
            }
            Console.WriteLine("{0} used Stun for {1} mana and disabled the Orc for {2} rounds.", name, costS, durationS);
        }

        public void Debuff(Orc orc)
        {
            mana -= costD;
            durationD = 2;
            if (durationD > 0)
            {
                orc.attack -= debuff;
            }
            Console.WriteLine("{0} used Debuff for {1} mana and debuffed the Orc for {2} rounds.", name, costD, durationD);
        }

        public void HealthPotion()
        {
            health += 50;
            if (health > 100)
            {
                health = 100;
            }
            Console.WriteLine("{0} drank a health potion, restoring 50 health.", name);
        }

        public void ManaPotion()
        {
            mana += 100;
            if (mana > 200)
            {
                mana = 200;
            }
            Console.WriteLine("{0} drank a mana potion, restoring 100 mana.", name);
        }
    }
}
