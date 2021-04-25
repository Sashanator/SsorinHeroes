using System;
using System.Windows;

namespace SsorinHeroes
{
    class Mob : Being
    {
        private const int ActionPoints_Attack = 2; //Константа, которая определяет сколько отнимается очков действий при атаке

        public bool isEnemy;
        public int ap; //Action Points
        public GridPosition gr;

        public Mob(string name_in, int hp_in, int attack_in, int defense_in, int luck_in, int move_in, int accuracy_in, int leader_in) : base(name_in, hp_in, attack_in, defense_in, luck_in, move_in, accuracy_in, leader_in)
        {
            this.name = name_in;
            this.hp = hp_in;
            this.attack = attack_in;
            this.defense = defense_in;
            this.luck = luck_in;
            this.move = move_in;
            this.accuracy = accuracy_in;
            this.leader = leader_in;
        }
        public bool isDead()
        {
            return (this.hp <= 0);
        }
        public void setLeader(int leader_in)
        {
            this.leader = leader_in;
        }
        public int getLeader()
        {
            return this.leader;
        }
        public void setAP(int in_ap)
        {
            this.ap = in_ap;
        }
        public int getAP()
        {
            return this.ap;
        }
        public int getHP()
        {
            return this.hp;
        }
        public void setHP(int hp_in)
        {
            this.hp = hp_in;
        }
        public int getAttack()
        {
            return this.attack;
        }
        public int getDefense()
        {
            return this.defense;
        }
        public int getLuck()
        {
            return this.luck;
        }
        public int getMove()
        {
            return this.move;
        }
        public int getAccuracy()
        {
            return this.accuracy;
        }

        public void SetPosition(GridPosition pos)
        {
            this.gr = pos;
        }
        public void setEnemy()
        {
            this.isEnemy = true;
        }
        public void setFriend()
        {
            this.isEnemy = false;
        }
        public void AttackMob(Mob mob)
        {
            this.ap -= ActionPoints_Attack;
            Random rng = new Random();
            bool isCrit = (rng.Next(0, 100) <= this.luck) ? true : false;
            bool isHit = (rng.Next(0, 100) <= this.accuracy) ? true : false;
            if (isHit)
            {
                if (isCrit)
                {
                    mob.hp -= (this.attack - mob.defense > 0 ? this.attack - mob.defense : 0);
                }
                else
                {
                    mob.hp -= (this.attack * 2 - mob.defense > 0 ? this.attack * 2 - mob.defense : 0);
                }
            }
            else
            {
                MessageBox.Show("Miss!");
            }
        }

    }
}
