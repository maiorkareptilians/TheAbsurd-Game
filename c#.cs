using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonsAndNoobCo
{
    // Базовый класс для всех персонажей
    public abstract class Character
    {
        public string Name { get; protected set; }
        public int Strength { get; protected set; }
        public int Dexterity { get; protected set; }
        public int Vitality { get; protected set; }
        public int Intelligence { get; protected set; }
        public int Health { get; protected set; }
        public int MaxHealth { get; protected set; }
        public int Mana { get; protected set; }
        public int MaxMana { get; protected set; }
        public int Armor { get; protected set; }
        public int MagicArmor { get; protected set; }
        public bool IsAlive => Health > 0;
        public Weapon Weapon { get; protected set; }

        protected Character(string name, int strength, int dexterity, int vitality, int intelligence)
        {
            Name = name;
            Strength = strength;
            Dexterity = dexterity;
            Vitality = vitality;
            Intelligence = intelligence;
            
            MaxHealth = Vitality * 4;
            Health = MaxHealth;
            MaxMana = Intelligence * 4;
            Mana = MaxMana;
            Armor = Dexterity / 2;
            MagicArmor = Intelligence / 2;
        }

        public abstract void Attack(List<Character> enemies);
        public abstract int CalculatePhysicalDamage();
        public abstract int CalculateSpellDamage();
        
        public virtual void TakePhysicalDamage(int damage)
        {
            int actualDamage = damage - Armor - Dexterity;
            if (actualDamage < 0) actualDamage = 0;
            Health -= actualDamage;
            if (Health < 0) Health = 0;
        }

        public virtual void TakeSpellDamage(int damage)
        {
            int actualDamage = damage - MagicArmor - Intelligence;
            if (actualDamage < 0) actualDamage = 0;
            Health -= actualDamage;
            if (Health < 0) Health = 0;
        }

        public Character SelectTarget(List<Character> enemies)
        {
            return enemies.Where(e => e.IsAlive)
                         .OrderBy(e => e.Health + e.Armor)
                         .FirstOrDefault();
        }
    }

    // Класс Рыцаря
    public class Knight : Character
    {
        public Knight(string name, int strength, int dexterity, int vitality, int intelligence) 
            : base(name, strength, dexterity, vitality, intelligence)
        {
            Health += 15;
            MaxHealth += 15;
            Strength += 2;
            Armor += 2;
            Weapon = new Sword();
        }

        public override void Attack(List<Character> enemies)
        {
            var target = SelectTarget(enemies);
            if (target == null) return;

            int damage = CalculatePhysicalDamage();
            Console.WriteLine($"{GetType().Name} {Name} attacking {target.GetType().Name} {target.Name} with {Weapon.Name}.");
            target.TakePhysicalDamage(damage);
            Console.WriteLine($"{target.GetType().Name} {target.Name} get hit for {damage} hp and have {target.Health} hp left!");
            
            if (!target.IsAlive)
            {
                Console.WriteLine($"{target.GetType().Name} {target.Name} is defeated!");
            }
        }

        public override int CalculatePhysicalDamage()
        {
            return Weapon.Damage + Strength;
        }

        public override int CalculateSpellDamage()
        {
            return 0; // Рыцарь не использует заклинания
        }
    }

    // Класс Вора
    public class Thief : Character
    {
        public Thief(string name, int strength, int dexterity, int vitality, int intelligence) 
            : base(name, strength, dexterity, vitality, intelligence)
        {
            Dexterity += 3;
            Weapon = new Dagger();
        }

        public override void Attack(List<Character> enemies)
        {
            var target = SelectTarget(enemies);
            if (target == null) return;

            int damage = CalculatePhysicalDamage();
            Console.WriteLine($"{GetType().Name} {Name} attacking {target.GetType().Name} {target.Name} with {Weapon.Name}.");
            target.TakePhysicalDamage(damage);
            Console.WriteLine($"{target.GetType().Name} {target.Name} get hit for {damage} hp and have {target.Health} hp left!");
            
            if (!target.IsAlive)
            {
                Console.WriteLine($"{target.GetType().Name} {target.Name} is defeated!");
            }
        }

        public override int CalculatePhysicalDamage()
        {
            return Weapon.Damage + Dexterity;
        }

        public override int CalculateSpellDamage()
        {
            return 0; // Вор не использует заклинания
        }
    }

    // Класс Волшебника
    public class Mage : Character
    {
        private const int CHAIN_LIGHTNING_COST = 40;
        private const int STAFF_SPELL_BONUS = 10;

        public Mage(string name, int strength, int dexterity, int vitality, int intelligence) 
            : base(name, strength, dexterity, vitality, intelligence)
        {
            Intelligence += 5;
            MaxMana = Intelligence * 4;
            Mana = MaxMana;
            Mana += 25;
            MaxMana += 25;
            MagicArmor += 2;
            Weapon = new Staff();
        }

        public override void Attack(List<Character> enemies)
        {
            var target = SelectTarget(enemies);
            if (target == null) return;

            // Используем заклинание, если хватает маны, иначе бьём посохом
            if (Mana >= CHAIN_LIGHTNING_COST)
            {
                int damage = CalculateSpellDamage();
                Console.WriteLine($"{GetType().Name} {Name} attacking {target.GetType().Name} {target.Name} with chain lightning.");
                target.TakeSpellDamage(damage);
                Console.WriteLine($"{target.GetType().Name} {target.Name} get hit for {damage} hp and have {target.Health} hp left!");
                Mana -= CHAIN_LIGHTNING_COST;
            }
            else
            {
                int damage = CalculatePhysicalDamage();
                Console.WriteLine($"{GetType().Name} {Name} attacking {target.GetType().Name} {target.Name} with {Weapon.Name}.");
                target.TakePhysicalDamage(damage);
                Console.WriteLine($"{target.GetType().Name} {target.Name} get hit for {damage} hp and have {target.Health} hp left!");
            }
            
            if (!target.IsAlive)
            {
                Console.WriteLine($"{target.GetType().Name} {target.Name} is defeated!");
            }
        }

        public override int CalculatePhysicalDamage()
        {
            return Weapon.Damage + Strength;
        }

        public override int CalculateSpellDamage()
        {
            return ((Staff)Weapon).SpellDamageBonus + Intelligence;
        }
    }

    // Базовый класс оружия
    public abstract class Weapon
    {
        public string Name { get; protected set; }
        public int Damage { get; protected set; }
        public string DamageType { get; protected set; }
    }

    public class Sword : Weapon
    {
        public Sword()
        {
            Name = "sword";
            Damage = 5;
            DamageType = "physical";
        }
    }

    public class Dagger : Weapon
    {
        public Dagger()
        {
            Name = "dagger";
            Damage = 4;
            DamageType = "physical";
        }
    }

    public class Staff : Weapon
    {
        public int SpellDamageBonus { get; private set; }

        public Staff()
        {
            Name = "staff";
            Damage = 15;
            DamageType = "physical";
            SpellDamageBonus = 10;
        }
    }

    // Основной класс игры
    public class Game
    {
        private List<Character> heroes = new List<Character>();
        private List<Character> enemies = new List<Character>();

        public void ProcessInput(List<string> inputLines)
        {
            string currentSection = "";
            
            foreach (string line in inputLines)
            {
                if (line.Trim() == "hero" || line.Trim() == "enemy" || line.Trim() == "end")
                {
                    currentSection = line.Trim();
                    continue;
                }

                if (string.IsNullOrEmpty(line.Trim())) continue;

                string[] parts = line.Trim().Split(' ');
                if (parts.Length < 5) continue;

                string className = parts[0];
                int strength = int.Parse(parts[1]);
                int dexterity = int.Parse(parts[2]);
                int vitality = int.Parse(parts[3]);
                int intelligence = int.Parse(parts[4]);
                string name = string.Join(" ", parts.Skip(5));

                Character character = CreateCharacter(className, name, strength, dexterity, vitality, intelligence);
                
                if (currentSection == "hero")
                {
                    heroes.Add(character);
                }
                else if (currentSection == "enemy")
                {
                    enemies.Add(character);
                }
            }
        }

        private Character CreateCharacter(string className, string name, int strength, int dexterity, int vitality, int intelligence)
        {
            return className.ToLower() switch
            {
                "knight" => new Knight(name, strength, dexterity, vitality, intelligence),
                "thief" => new Thief(name, strength, dexterity, vitality, intelligence),
                "mage" => new Mage(name, strength, dexterity, vitality, intelligence),
                _ => throw new ArgumentException($"Unknown character class: {className}")
            };
        }

        public void RunBattle()
        {
            // Вступление
            Console.WriteLine("Stay a while and listen, and I will tell you a story. A story of Dungeons and Dragons, of Orcs and Goblins, of Ghouls and Ghosts, of Kings and Quests, but most importantly -- of Heroes and NoobCo -- Well... A story of Heroes.");
            Console.WriteLine();

            // Завязка
            if (heroes.Count == 1)
            {
                Console.WriteLine($"So here starts the journey of our hero {heroes[0].GetType().Name} {heroes[0].Name} got order to eliminate the local bandit known as {enemies[0].GetType().Name} {enemies[0].Name}.");
            }
            else
            {
                Console.WriteLine($"So here starts the journey of our heroes got order to eliminate the local bandit known as {enemies[0].GetType().Name} {enemies[0].Name}.");
            }
            Console.WriteLine();

            // Битва
            while (heroes.Any(h => h.IsAlive) && enemies.Any(e => e.IsAlive))
            {
                // Ход героев
                foreach (var hero in heroes.Where(h => h.IsAlive))
                {
                    if (!enemies.Any(e => e.IsAlive)) break;
                    hero.Attack(enemies);
                }

                // Ход врагов
                foreach (var enemy in enemies.Where(e => e.IsAlive))
                {
                    if (!heroes.Any(h => h.IsAlive)) break;
                    enemy.Attack(heroes);
                }
            }

            Console.WriteLine();

            // Финал
            if (enemies.All(e => !e.IsAlive))
            {
                Console.WriteLine("Congratulations!");
            }
            else
            {
                Console.WriteLine("Unfortunately our hero was brave, yet not enough skilled, or just lack of luck.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var game = new Game();
            var inputLines = new List<string>();

            // Чтение входных данных
            string line;
            while ((line = Console.ReadLine()) != "end")
            {
                inputLines.Add(line);
            }
            inputLines.Add("end");

            game.ProcessInput(inputLines);
            game.RunBattle();
        }
    }
}