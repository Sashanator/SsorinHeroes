namespace SsorinHeroes
{
    class Being
    {
        public string name;
        protected int hp;
        public int move;
        protected int attack;
        protected int defense;
        protected int luck;
        protected int accuracy;
        protected int leader; //Очки лидерства

        public Being(string name_in, int hp_in, int attack_in, int defense_in, int luck_in, int move_in, int accuracy_in, 
            int leader_in)
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
    }
}
