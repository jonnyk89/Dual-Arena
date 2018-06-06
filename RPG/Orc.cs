using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG
{
    public class Orc : Person
    {
        // Specials
        private int _critStrike;
        private int _critChance = 15;
        public bool crit = false;

        public bool can_attack = true;

        public Orc()
        {
            health = 150;
            attack = 20;
            mana = 0;
        }

        // Abilities
        public void Attack(Player player)
        {
            if (can_attack == true)
            {
                Random r = new Random();
                if (r.Next(0, 100) <= _critChance && r.Next(0, 100) > 0)
                {
                    _critStrike = attack * 2;
                    crit = true;

                    player.TakeDamage(_critStrike);
                    Console.WriteLine("Orc used Attack and did {0} *CRITICAL* Damage to {1}.", _critStrike, player.name);
                    Console.WriteLine(new string('-', 25));
                }
                else
                {
                    player.TakeDamage(attack);
                    Console.WriteLine("Orc used Attack and did {0} Damage to {1}.", attack, player.name);
                    Console.WriteLine(new string('-', 25));
                }
            }
            else
            {
                Console.WriteLine("Orc is stunned.");
                Console.WriteLine(new string('-', 25));
            }
        }
    }
}
