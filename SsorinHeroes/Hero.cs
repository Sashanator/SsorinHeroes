using System;
using System.Windows;

namespace SsorinHeroes
{
    struct Spell
    {
        public string spellName;
        public int damage;
        public int distance;
        public int mana;
        public int magicError; //Шанс маг. ошибки
    }

    class Hero : Mob
    {
        private const int BonusStat = 2;
        public Spell heroSpell;
        public Hero(string name_in, int hp_in, int attack_in, int defense_in, int luck_in, int move_in, int accuracy_in, int leader_in) : base(name_in, hp_in, attack_in, defense_in, luck_in, move_in, accuracy_in, leader_in)
        {
            this.name = name_in;
            this.hp = hp_in * BonusStat;
            this.attack = attack_in * BonusStat;
            this.defense = defense_in * BonusStat;
            this.luck = (luck_in * BonusStat > 100) ? 100 : luck_in * BonusStat;
            this.move = move_in;
            this.accuracy = (accuracy_in * BonusStat > 100) ? 100 : accuracy_in * BonusStat;
            this.leader = (leader_in * BonusStat > 1000) ? 1000 : leader_in * BonusStat;
        }
        public void SetSpell(int damage_in, int distance_in,
            int mana_in, int magicError_in, string name_in)
        {
            this.heroSpell.spellName = name_in;
            this.heroSpell.damage = damage_in;
            this.heroSpell.distance = distance_in;
            this.heroSpell.mana = mana_in;
            this.heroSpell.magicError = magicError_in;
        }
        public void CastSpell(Mob mob)
        {
            this.ap -= this.heroSpell.mana;
            Random rng = new Random();
            bool isHit = (rng.Next(0, 100) <= this.heroSpell.magicError) ? false : true;
            if (isHit)
            {
                mob.setHP(mob.getHP() - this.heroSpell.damage);
            }
            else
            {
                MessageBox.Show("Magic error!"); //log
            }
        }
        public void CastSpell(Hero hero)
        {
            this.ap -= this.heroSpell.mana;
            Random rng = new Random();
            bool isHit = (rng.Next(0, 100) <= this.heroSpell.magicError) ? false : true;
            if (isHit)
            {
                hero.setHP(hero.getHP() - this.heroSpell.damage);
            }
            else
            {
                MessageBox.Show("Magic error!"); //log
            }
        }
    }
}
