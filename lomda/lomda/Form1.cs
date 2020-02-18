using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lomda
{
    public partial class Form1 : Form
    {
        const string PUSHo = "PUSH";
        const string PULLo = "PULL";
        const string LOOPo = "LOOP";
        const string ELOOPo = "ELOOP";
        const string INCo = "INC";
        const string DECo = "DEC";
        const string MOVABo = "MOV A B";
        const string MOVBAo = "MOV B A";
        const string MOVACo = "MOV A C";
        const string MOVCAo = "MOV C A";
        const string ADDo = "ADD";
        const string SUBo = "SUB";
        const string MULo = "MUL";
        const string DIVo = "DIV";
        const string FINISHo = "FINISH";

        string DirPath = AppDomain.CurrentDomain.BaseDirectory, question = "";

        Stack<int> S = new Stack<int>();
        int A = 0;
        int B = 0;
        int C = 0;
        int ip = 0;
        int question_num = 1;
        int question_num_max = 1;

        Stack<string> past = new Stack<string>();
        string present;
        Stack<string> future = new Stack<string>();

        public Form1()
        {
            InitializeComponent();
            config_num_of_questions();
            uploadQuestion();
        }

        void config_num_of_questions()
        {
            const Int32 BufferSize = 128;

            if (File.Exists(DirPath + "questions.dat"))
            {
                using (var fileStream = File.OpenRead(DirPath + "questions.dat"))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line = streamReader.ReadLine();
                    if (line != null)
                        question_num_max = Int32.Parse(line.Substring(21));
                }
            }
            else
            {
                MessageBox.Show("Could not find questions.dat");
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            string message = "";
            const Int32 BufferSize = 128;

            if (File.Exists(DirPath + "help.txt"))
            {
                using (var fileStream = File.OpenRead(DirPath + "help.txt"))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        message += line + "\n";
                    }
                }
            }
            else
            {
                message = "Could not find help.txt";
            }
            message += "\n-made by roy arama ©";
            MessageBox.Show(message);

        }
        private void PUSH_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + PUSHo);
        }

        private void PULL_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + PULLo);
        }

        private void LOOP_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + LOOPo);
        }

        private void ELOOP_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + ELOOPo);
        }

        private void INC_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + INCo);
        }

        private void DEC_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + DECo);
        }

        private void MOVAB_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + MOVABo);
        }

        private void MOVEBA_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + MOVBAo);
        }

        private void MOVAC_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + MOVACo);
        }

        private void MOVCA_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + MOVCAo);
        }

        private void ADD_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + ADDo);
        }

        private void SUB_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + SUBo);
        }

        private void MUL_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + MULo);
        }

        private void DIV_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + DIVo);
        }

        private void FINISH_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(listBox1.Items.Count.ToString("D4") + " " + FINISHo);
        }

        private void DELETE_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBox1.SelectedItems.Count; i++)
                listBox1.Items.Remove(listBox1.SelectedItems[i]);
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.Items[i] = i.ToString("D4") + listBox1.Items[i].ToString().Substring(4);
            }
        }
        //START LINE UP/DOWN
        private void LINEUP_Click(object sender, EventArgs e)
        {
            MoveUp();
        }
        private void LINEDOWN_Click(object sender, EventArgs e)
        {
            MoveDown();
        }

        public void MoveUp()
        {
            MoveItem(-1);
        }

        public void MoveDown()
        {
            MoveItem(1);
        }

        public void MoveItem(int direction)
        {
            // Checking selected item
            if (listBox1.SelectedItem == null || listBox1.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int newIndex = listBox1.SelectedIndex + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= listBox1.Items.Count)
                return; // Index out of range - nothing to do

            object selected = listBox1.SelectedItem;

            // Removing removable element
            listBox1.Items.Remove(selected);
            // Insert it in new position
            listBox1.Items.Insert(newIndex, selected);
            // Restore selection
            listBox1.SetSelected(newIndex, true);

            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                listBox1.Items[i] = i.ToString("D4") + listBox1.Items[i].ToString().Substring(4);
            }
        }
        //END LINE UP/DOWN

        void uploadQuestion()
        {
            label1.Text = label1.Text.Substring(0, label1.Text.Length - 2) + question_num.ToString("D2");
            while (S.Any())
                S.Pop();
            A = 0; B = 0; C = 0;
            ip = 0;
            bool in_line = false;
            const Int32 BufferSize = 128;

            if (File.Exists(DirPath + "questions.dat"))
            {
                using (var fileStream = File.OpenRead(DirPath + "questions.dat"))
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
                {
                    String line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (line == "START: " + question_num.ToString())
                            in_line = true;
                        else if (line == "END: " + question_num.ToString())
                            in_line = false;
                        else if (in_line)
                        {
                            switch (line.Substring(0, 2))
                            {
                                case "S ":
                                    S.Push(Int32.Parse(line[2].ToString()));
                                    break;
                                case "A ":
                                    A = Int32.Parse(line[2].ToString());
                                    break;
                                case "B ":
                                    B = Int32.Parse(line[2].ToString());
                                    break;
                                case "C ":
                                    C = Int32.Parse(line[2].ToString());
                                    break;
                                case "Q ":
                                    richTextBox1.Text = line.Substring(2);
                                    break;
                                case "W ":
                                    question = line.Substring(2);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Could not find help.txt");
            }

        }
        bool checkQuestion()
        {
            bool suc = true;
            try
            {
                for (int i = 0; i < question.Length; i+=5)
                {
                    switch (question.Substring(i, 2))
                    {
                        case "S=":
                            if (S.Count() > 0 && S.Pop() == Int32.Parse(question[i + 2].ToString() + question[i + 3].ToString()))
                                suc = suc & true;
                            else
                                suc = false;
                            break;
                        case "A=":
                            if (A == Int32.Parse(question[i + 2].ToString() + question[i + 3].ToString()))
                                suc = suc & true;
                            else
                                suc = false;
                            break;
                        case "B=":
                            if (B == Int32.Parse(question[i + 2].ToString() + question[i + 3].ToString()))
                                suc = suc & true;
                            else
                                suc = false;
                            break;
                        case "C=":
                            if (C == Int32.Parse(question[i + 2].ToString() + question[i + 3].ToString()))
                                suc = suc & true;
                            else
                                suc = false;
                            break;
                        default:
                            suc = false;
                            break;
                    }
                }

            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("probably you tried to pass a var without D2 format\n" +
                    "for example: do- A=00 instead of A=0 after W");
                suc = false;
            }
            return suc;

        }
        void compileQuestions()
        {
            bool FINISH = false;
            int loop = 0, len = listBox1.Items.Count;
            while (!FINISH)
            {
                if (ip >= len)
                {
                    uploadQuestion();
                    break;
                }
                switch (listBox1.Items[ip].ToString().Substring(5))
                {
                    case (PUSHo):
                        S.Push(B);
                        break;
                    case (PULLo):
                        if (S.Count() == 0)
                        {
                            uploadQuestion();
                            FINISH = true;
                        }
                        else
                            B = S.Pop();
                        break;
                    case (LOOPo):
                        loop = ip;
                        break;
                    case (ELOOPo):
                        if (C > 1)
                        {
                            ip = loop;
                            C--;
                        }
                        break;
                    case (INCo):
                        ++C;
                        break;
                    case (DECo):
                        --C;
                        break;
                    case (MOVABo):
                        A = B;
                        break;
                    case (MOVBAo):
                        B = A;
                        break;
                    case (MOVACo):
                        A = C;
                        break;
                    case (MOVCAo):
                        C = A;
                        break;
                    case (ADDo):
                        A += B;
                        break;
                    case (SUBo):
                        A -= B;
                        break;
                    case (MULo):
                        A *= B;
                        break;
                    case (DIVo):
                        if (B != 0)
                        {
                            A /= B;
                        }
                        break;
                    case (FINISHo):
                        FINISH = true;
                        break;
                    default:
                        break;
                }
                ip++;
            }
        }

        private void check_please_Click(object sender, EventArgs e)
        {
            uploadQuestion();
            compileQuestions();
            if (checkQuestion())
            {
                MessageBox.Show("sucsess");
            }
            else
            {
                MessageBox.Show("fail");
            }
        }

        void take_current()
        {
            present = "";
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                present += listBox1.Items[i].ToString() + '\n';
            }
        }
        void return_current()
        {
            listBox1.Items.Clear();
            if (present != null)
            {
                string[] to_list = present.Split('\n');
                for (int i = 0; i < to_list.Length; i++)
                {
                    if (to_list[i]!="")
                        listBox1.Items.Add(to_list[i]);
                }
            }
        }
        private void prevbutton_Click(object sender, EventArgs e)
        {
            if (question_num > 1)
            {
                --question_num;
                uploadQuestion();

                take_current();
                future.Push(present);
                if (past.Any())
                {
                    present = past.Pop();
                    return_current();
                }
                else
                {
                    listBox1.Items.Clear();
                }
            }
        }

        private void nextbutton_Click(object sender, EventArgs e)
        {
            if (question_num_max > question_num)
            {
                ++question_num;
                uploadQuestion();


                take_current();
                past.Push(present);
                if (future.Any())
                {
                    present = future.Pop();
                    return_current();
                }
                else
                {
                    listBox1.Items.Clear();
                }
            }
        }
    }
}
