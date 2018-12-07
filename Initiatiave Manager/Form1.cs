using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Initiatiave_Manager
{
    public partial class Form1 : Form
    {
        static Random rnd = new Random();
        List<Player> playerList;
        List<Player> dmList;
        List<string> namelist;
        IEnumerable<Player> paixtoures;
        int roundCount = 1;
        
        class Player
        {
            private int initBonus;
            private int hasAdvantage;
            private string name;
            private int finalInit;

            public Player(string name, int initBonus)
            {
                this.name = name;
                this.initBonus = initBonus;
                this.hasAdvantage = 0;
            }
            public string Name
            {
                get => name;
                set => name = value;
            }

            public int FinalInit
            {
                get => finalInit;
                set => finalInit = value;
            }
            public int InitiativeBonus
            {
                get => initBonus;
                set => initBonus = value;
            }

            public int Advantage
            {
                get
                {
                    if (hasAdvantage == 0)
                    {
                        return 0;
                    }
                    else if (hasAdvantage == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                set
                {
                    if (value < -1 || value > 1)
                    {
                        throw new ArgumentOutOfRangeException($"{nameof(value)} must be -1, 0 or 1");
                    }
                    else
                        hasAdvantage = value;
                }
            }

            public int rollInitiative()
            {
                if (hasAdvantage == 0)
                {
                    return rnd.Next(1, 20) + this.initBonus;
                }
                else if (hasAdvantage == 1)
                {
                    int init1, init2;
                    init1 = rnd.Next(1, 20);
                    init2 = rnd.Next(1, 20);
                    return Math.Max(init1, init2) + this.initBonus;
                }
                else
                {
                    int init1, init2;
                    init1 = rnd.Next(1, 20);
                    init2 = rnd.Next(1, 20);
                    return Math.Min(init1, init2) + this.initBonus;
                }
            }
        }
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            playerList = new List<Player>();
            dmList = new List<Player>();
            namelist = new List<string>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Player paixtis;
            int temp;
            int.TryParse(textBox2.Text, out temp);
            playerList.Add(paixtis = new Player(textBox1.Text, temp));
            namelist.Add(paixtis.Name);
            richTextBox1.Text += paixtis.Name + " " + paixtis.InitiativeBonus+"\n";
            comboBox1.Items.Add(paixtis.Name);
            comboBox2.Items.Add(paixtis.Name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(button2.Text == "End Encounter")
            {
                button2.Text = "Roll Initiative";
                roundCount = 1;
                richTextBox2.Text = "";
                label7.Text = "";
                label9.Text = "";
                button9.Text = "Start Encounter";
                button9.Visible = false;
                return;
             
            }
            button9.Visible = true;
            roundCount = 1;
            richTextBox2.Text = "";
            //var newList = a.Concat(b);
            foreach (Player item in playerList)
            {
                item.FinalInit = item.rollInitiative();
            }
            var newList = playerList.Concat(dmList);
            paixtoures =  newList.OrderByDescending(x => x.FinalInit);
            foreach (Player item in paixtoures)
            {
                richTextBox2.Text += item.Name + " " + item.FinalInit + "\n";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Player creature;
            int temp;
            int.TryParse(textBox4.Text, out temp);
            dmList.Add(creature = new Player(textBox3.Text, 0));
            creature.FinalInit = temp;
            richTextBox3.Text += creature.Name + " " + creature.FinalInit + "\n";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox3.Text = "";
            dmList.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() != "")
            {
                String name = comboBox1.SelectedItem.ToString();
                Player paixtis = playerList.Find(i => i.Name == name);
                paixtis.Advantage = 1;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() != "")
            {
                String name = comboBox1.SelectedItem.ToString();
                Player paixtis = playerList.Find(i => i.Name == name);
                paixtis.Advantage = 0;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() != "")
            {
                String name = comboBox1.SelectedItem.ToString();
                Player paixtis = playerList.Find(i => i.Name == name);
                paixtis.Advantage = -1;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() != "")
            {
                String name = comboBox2.SelectedItem.ToString();
                Player paixtis = playerList.Find(i => i.Name == name);
                playerList.Remove(paixtis);
                richTextBox1.Text = "";
                foreach (Player item in playerList)
                {
                    richTextBox1.Text += item.Name + " " + item.InitiativeBonus + "\n";
                }
                comboBox1.Items.Remove(paixtis.Name);
                comboBox2.Items.Remove(paixtis.Name);
            }
            else
                return;
        }
        int index = 0;
        private void button9_Click(object sender, EventArgs e)
        {
            button9.Text = "Next...";
            List<Player> tempList = paixtoures.ToList<Player>();
            label7.Text = tempList.ElementAt(index).Name.ToString();
            button2.Text = "End Encounter";
            
            index++;
            
            label9.Text = roundCount.ToString();
            if (index == tempList.Count)
            {
                index = 0;
                roundCount++;
            }
        }
    }
}
