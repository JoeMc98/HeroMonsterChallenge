using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HeroMonsterChallenge
{
    public partial class index : System.Web.UI.Page
    {
        private Random random;
        Character jedi = new Character();
        Character sith = new Character();
        Dice jediDice = new Dice();
        Dice sithDice = new Dice();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                setInfo();
                saveHealth(jedi.Health, sith.Health);
                saveInfo();
            }
        }

        protected void btnAttack_Click(object sender, EventArgs e)
        {
            random = new Random();
            lblResult.Text += displayResult();
        }

        private void saveHealth(int jediHealth, int sithHealth)
        {
            ViewState.Add("jediHealth", jediHealth);
            ViewState.Add("sithHealth", sithHealth);
        }

        private void setInfo()
        {
            sithSetInfo();
            jediSetInfo();
        }

        private void jediSetInfo()
        {
            jedi.Name = "Luke Skywalker";
            jedi.Health = 100;
            jedi.DamageMaximum = 25;
            jedi.AttackBonus = true;
        }

        private void sithSetInfo()
        {
            sith.Name = "Darth Vader";
            sith.Health = 100;
            sith.DamageMaximum = 25;
            sith.AttackBonus = false;
        }

        private void saveInfo()
        {
            jediSaveInfo();
            sithSaveInfo();
        }

        private void jediSaveInfo()
        {
            ViewState.Add("jediName", jedi.Name);
            ViewState.Add("jediDamageMax", jedi.DamageMaximum);
            ViewState.Add("jediDice", jedi.DamageMaximum);
            ViewState.Add("jediBonus", jedi.AttackBonus);
        }

        private void sithSaveInfo()
        {
            ViewState.Add("sithName", sith.Name);
            ViewState.Add("sithDamageMax", sith.DamageMaximum);
            ViewState.Add("sithDice", sith.DamageMaximum);
            ViewState.Add("sithBonus", sith.AttackBonus);
        }

        private void retrieveInfo()
        {
            jediRetrieveInfo();
            sithRetrieveInfo();
        }

        private void jediRetrieveInfo()
        {
            jedi.Name = ViewState["jediName"].ToString();
            jedi.Health = Convert.ToInt32(ViewState["jediHealth"]);
            jediDice.Sides = Convert.ToInt32(ViewState["jediDice"]);
            jedi.DamageMaximum = Convert.ToInt32(ViewState["jediDice"]);
            jedi.AttackBonus = Convert.ToBoolean(ViewState["jediBonus"]);
        }

        private void sithRetrieveInfo()
        {
            sith.Name = ViewState["sithName"].ToString();
            sith.Health = Convert.ToInt32(ViewState["sithHealth"]);
            sithDice.Sides = Convert.ToInt32(ViewState["sithDice"]);
            sith.DamageMaximum = Convert.ToInt32(ViewState["sithDice"]);
            sith.AttackBonus = Convert.ToBoolean(ViewState["sithBonus"]);
        }

        private string displayResult()
        {
            do
            {
                retrieveInfo();

                string bonus = attackBonus(jedi, sith);

                retrieveInfo();

                int lukeAttack = jedi.Attack(jediDice, random);
                int vaderDefend = sith.Defend(lukeAttack);
                int vaderAttack = sith.Attack(sithDice, random);
                int lukeDefend = jedi.Defend(vaderAttack);

                saveHealth(lukeDefend, vaderDefend);

                retrieveInfo();

                if (jedi.Health <= 0 && sith.Health <= 0)
                    return "Both characters died...Nobody won.";
                else if (jedi.Health <= 0 || sith.Health <= 0)
                    return defeat(jedi, sith);
                else
                    return String.Format("{6} <br><br>{0} attacked {1} and inflicted {2}% damage. {1}'s Health: {3}%.<br>" +
                        "{1} attacked {0} and inflicted {4}% damage. {0}'s Health: {5}%.",
                        jedi.Name, sith.Name, lukeAttack, vaderDefend, vaderAttack, lukeDefend, bonus);
            }
            while (jedi.Health > 0 && sith.Health > 0);
        }

        private string defeat(Character jedi, Character sith)
        {
            if (jedi.Health <= 0)
                return String.Format("<br><br>{0} attacked {1} and killed him. {0} WINS !!!", sith.Name, jedi.Name);
            else if(sith.Health <= 0)
                return String.Format("<br><br>{0} attacked {1} and killed him. {0} WINS !!!", jedi.Name, sith.Name);
            else
                return "";
        }

        private string attackBonus(Character jedi, Character sith)
        {
            if (jedi.AttackBonus == true)
            {
                int lukeAttack = jedi.Attack(jediDice, random);
                int vaderDefend = sith.Defend(lukeAttack);
                jedi.AttackBonus = false;
                ViewState.Add("sithHealth", vaderDefend);
                ViewState.Add("jediBonus", jedi.AttackBonus);
                return String.Format("Bonus Attack -- {0} attacked {1} and inflicted " +
                    "{2}% damage. {1}'s Health: {3}%.",
                    jedi.Name, sith.Name, lukeAttack, vaderDefend);
            }
            else if (sith.AttackBonus == true)
            {
                int vaderAttack = sith.Attack(sithDice, random);
                int lukeDefend = jedi.Defend(vaderAttack);
                sith.AttackBonus = false;
                ViewState.Add("jediHealth", lukeDefend);
                ViewState.Add("sithBonus", sith.AttackBonus);
                return String.Format("Bonus Attack -- {0} attacked {1} and inflicted " +
                    "{2}% damage. {1}'s Health: {3}%.",
                    sith.Name, jedi.Name, vaderAttack, lukeDefend);
            }
            else
                return "";
        }
    }
    
    class Character
    {
        
        public string Name { get; set; }
        public int Health { get; set; }
        public int DamageMaximum { get; set; }
        public bool AttackBonus { get; set; }

        public int Attack(Dice dice, Random random)
        {
            dice.Sides = this.DamageMaximum;
            return dice.Roll(random);
        }

        public int Defend(int damage)
        {
            return this.Health - damage;
        }
    }

    class Dice
    {
        private Random _random;
        public int Sides { get; set; }

        public int Roll(Random random)
        {
            _random = random;
            return _random.Next(this.Sides);
        }
    }
}