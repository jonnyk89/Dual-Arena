using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;
using System.Threading;

namespace RPG
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Player Name: ");
            Player player = new Player(Console.ReadLine());
            Orc orc = new Orc();
            bool end = false;

            // Preparations:
            bool stun_active = false;
            bool debuff_active = false;

            while (end == false)
            {
                // Player Move:
            Fail:
                Console.Write(">");
                string command = Console.ReadLine();

                if (command.ToLower() == "attack")
                {
                    player.Attack(orc);
                    PlayerAttackSound();
                    Thread.Sleep(2000);
                }
                else if (command.ToLower() == "fireball")
                {
                    if (player.mana - player.costF >= 0)
                    {
                        player.Fireball(orc);
                        PlayerFireballSound();
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("{0} failed Fireball spell due to lack of mana.", player.name);
                        Thread.Sleep(2000);
                    }
                }
                else if (command.ToLower() == "might")
                {
                    if (player.mana - player.costM >= 0)
                    {
                        player.Might();
                        PlayerMightSound();
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("{0} failed Might spell due to lack of mana.", player.name);
                        Thread.Sleep(2000);
                    }
                }
                else if (command.ToLower() == "stun")
                {
                    if (player.mana - player.costS >= 0)
                    {
                        player.Stun(orc);
                        stun_active = true;
                        PlayerStunSound();
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("{0} failed Stun spell due to lack of mana.", player.name);
                        Thread.Sleep(2000);
                    }
                }
                else if (command.ToLower() == "debuff")
                {
                    if (player.mana - player.costD >= 0)
                    {
                        player.Debuff(orc);
                        debuff_active = true;
                        PlayerDebuffSound();
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("{0} failed Debuff spell due to lack of mana.", player.name);
                        Thread.Sleep(2000);
                    }
                }
                else if (command.ToLower() == "health potion")
                {
                    if(player.healthPotions > 0)
                    {
                        player.HealthPotion();
                        player.healthPotions--;
                        PlayerPotionSound();
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("{0} failed to drink health potion due to lack of such an item.", player.name);
                        Thread.Sleep(2000);
                    }
                }
                else if (command.ToLower() == "mana potion")
                {
                    if (player.manaPotions > 0)
                    {
                        player.ManaPotion();
                        player.manaPotions--;
                        PlayerPotionSound();
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("{0} failed to drink mana potion due to lack of such an item.", player.name);
                        Thread.Sleep(2000);
                    }
                }
                else if (command.ToLower() == "stats")
                {
                    // Stats:
                    Console.WriteLine("{0}: HP = {1}, ATK = {2}, MP = {3}", player.name, player.health, player.attack, player.mana);
                    Console.WriteLine("Orc:    HP = {0}, ATK = {1}, MP = {2}", orc.health, orc.attack, orc.mana);
                    Console.WriteLine(new string('-', 25));
                    goto Fail;
                }
                else if (command.ToLower() == "rules")
                {
                    // Open "Game Rules.txt" file: 
                    ReadRules();
                    PlayRulesSound();
                    Console.WriteLine(new string('-', 25));
                    goto Fail;
                }
                else
                {
                    Console.WriteLine("Unknown command, try again!");
                    Console.WriteLine(new string('-', 25));
                    goto Fail;
                }

                // Extra effects from spells:
                if (debuff_active == true && player.durationD > 0)
                {
                    player.durationD--;
                }
                else
                {
                    debuff_active = false;
                    orc.attack = 20;
                }

                if (stun_active == true && player.durationS > 0)
                {
                    player.durationS--;
                }
                else
                {
                    stun_active = false;
                    orc.can_attack = true;
                }

                // Orc Move:
                orc.Attack(player);
                if (orc.can_attack != false)
                {
                    if (orc.crit == true)
                    {
                        OrcAttackCritSound();
                        System.Threading.Thread.Sleep(2000);
                        orc.crit = false;
                    }
                    else
                    {
                        OrcAttackSound();
                        System.Threading.Thread.Sleep(2000);
                    }
                }
                
                // End
                if (orc.health <= 0)
                {
                    Console.WriteLine("WINNER: " + player.name);
                    PlayVictorySound();
                    Thread.Sleep(10000);
                    end = true;
                }
                else if (player.health <= 0)
                {
                    Console.WriteLine("WINNER: Orc");
                    PlayDefeatSound();
                    Thread.Sleep(1000);
                    end = true;
                }
            }
        }

        // Create, write and read Game Rules.txt file
        public static void ReadRules()
        {
            string fileName = "Game Rules.txt";
            string projectPath = System.IO.Directory.GetCurrentDirectory();

            if (File.Exists(projectPath + projectPath + "\\" + fileName))
            {
                File.Delete(projectPath + projectPath + "\\" + fileName);
            }

            List<string> Rules = new List<string>()
                {
                    "Game rules: ",
                    "This is a round-based game.",
                    "Every time a command needs to be given, it is pointed out with this \'>\'.",
                    "The commands \"Stats\" and \"Rules\" (not case sensitive) do not consume a round.",
                    "If a command does not exist, you will be asked to give another command.",
                    "If a command can not be completed due to lack of mana, a round will be wasted on your part.",
                    "The game ends once one of the characters drops at or below 0 health.",
                    "----------------------------------------------------",
                    "Player stats: ",
                    "Health = 100",
                    "Attack = 10 (can be increased)",
                    "Mana = 200",
                    "Health Potions = 1",
                    "Mana Potions = 1",
                    "----------------------------------------------------",
                    "Player abilities: ",
                    "Attack - deals 10 damage unless buffed",
                    "Fireball - deals 30 damage, costs 50 mana",
                    "Might - increases Player attack by 10, costs 100 mana",
                    "Stun - makes Orc unable to attack, costs 50 mana, 3 round duration",
                    "Debuff - decreases Orc attack by 5, costs 50 mana, 2 round duration",
                    "Health Potion - gives player 50 health, can not go over 100 health",
                    "Mana Potion - gives player 100 mana, can not go over 200 mana",
                    "----------------------------------------------------",
                    "Orc stats: ",
                    "Health = 150",
                    "Attack = 20 (can be buffed)",
                    "Mana = 0",
                    "----------------------------------------------------",
                    "Orc Abilities: ",
                    "Attack - deals 20 damage unless a critical strike is done",
                    "*  The orc has a 15% chance to land a critical strike with each attack",
                    "   If he does manage to land a critican strike, he will attack twice ",
                    "   as hard that round (instead of 20 damage, he will do 40 damage).",
                    "----------------------------------------------------",
                    "Extra commands: ",
                    "Status - shows the current stats of each character",
                    "Rules - opens the file than contains the Rules for the game",
                    "*  These commands do not consume a round.",
                };

            File.WriteAllLines(projectPath + "\\" + fileName, Rules);
            System.Diagnostics.Process.Start(projectPath + "\\" + fileName);
        }

        // Player sounds: 
        public static void PlayerAttackSound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\PlayerAttack.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }

        public static void PlayerFireballSound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\PlayerFireball.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }

        public static void PlayerMightSound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\PlayerMight.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }

        public static void PlayerStunSound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\PlayerStun.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }

        public static void PlayerDebuffSound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\PlayerDebuff.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }

        public static void PlayerPotionSound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\PlayerPotion.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }

        // Orc sounds:
        public static void OrcAttackSound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\OrcAttack.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }

        public static void OrcAttackCritSound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\OrcAttackCrit.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }

        // Game sounds:
        public static void PlayVictorySound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\victory.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }

        public static void PlayDefeatSound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\defeat.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }

        public static void PlayRulesSound()
        {
            string soundPath = System.IO.Directory.GetCurrentDirectory() + "\\Sounds" + "\\rules.wav";

            SoundPlayer victorySound = new SoundPlayer(soundPath);
            victorySound.Play();
        }
    }
}