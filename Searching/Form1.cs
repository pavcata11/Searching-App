
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using WinSCP;


namespace Searching
{
    public partial class Form1 : Form
    {
        List<Person> persons = new List<Person>();
       
        public Form1()
        {
            InitializeComponent();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
           createPersonAddInList();
           
        }
        private void createPersonAddInList()
        {
            var person = new Person()
            {
                ClientId = clientId(),
                FirstName = firstNameTxtb.Text,
                SecondName = secondNametxtb.Text,
                LastName = lastNameTxtb.Text,
                Phone = phoneTxtb.Text,
                City = cityTxtb.Text,
                Product = productComboBox.Text,
                Street = streetTxtb.Text,
                Email = emailTxtb.Text,
            };
            persons.Add(person);
            clearFields();
            addPersonInView();
        }
        private int clientId()
        {
            Random rand = new Random();
                return  rand.Next(100000000);
        }
        private void clearFields()
        {
          
            firstNameTxtb.Text = "";
            secondNametxtb.Text = "";
            lastNameTxtb.Text = "";
            phoneTxtb.Text = "";
            emailTxtb.Text = "";
            cityTxtb.Text = "";
            productComboBox.Text = "";
            streetTxtb.Text = "";

        }
        private void addPersonInView()
        {
            dataGridView1.Rows.Clear();
            createGridView();
           
            foreach (var person in persons)
            {
               
                string[] row = new string[] { person.ClientId.ToString(), person.FirstName, person.SecondName, person.LastName, person.City, person.Street
                ,person.Phone,person.Email,person.Product};
                dataGridView1.Rows.Add(row);
            }
        }
        private void createGridView()
        {
            dataGridView1.ColumnCount = 9;
            dataGridView1.Columns[0].Name = "ClientID";
            dataGridView1.Columns[1].Name = "First Name";
            dataGridView1.Columns[2].Name = "Second Name";
            dataGridView1.Columns[3].Name = "Last Name";
            dataGridView1.Columns[4].Name = "City";
            dataGridView1.Columns[5].Name = "Street";
            dataGridView1.Columns[6].Name = "Phone";
            dataGridView1.Columns[7].Name = "Email";
            dataGridView1.Columns[8].Name = "Product";

        }
        private void updateBtn_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            string searchValue = searchTxtb.Text;
            int rowIndex = -1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["First Name"].Value != null) 
                {
                    string name = searchingBySelectedItemFromMenu();
                    if (row.Cells[name].Value.ToString().Contains(searchValue))
                    {
                        rowIndex = row.Index;
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(255, 224, 192);
                    }
                    else
                    {
                        rowIndex = row.Index;
                        dataGridView1.Rows[rowIndex].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }
        private string searchingBySelectedItemFromMenu()
        {
            if (clientIdToolStripMenuItem.Checked)
            {
                return "ClientID";
            }
            else if (firstNameToolStripMenuItem.Checked)
            {
                return "First Name";
            }
            else if (lastNameToolStripMenuItem.Checked)
            {
                return "Last Name";
            }
           
                return "Product";
        }
        private void firstNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clientIdToolStripMenuItem.Checked = false;
            firstNameToolStripMenuItem.Checked = true;
            lastNameToolStripMenuItem.Checked = false;
            productToolStripMenuItem.Checked = false;

        }

        private void clientIdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clientIdToolStripMenuItem.Checked = true;
            firstNameToolStripMenuItem.Checked = false;
            lastNameToolStripMenuItem.Checked = false;
            productToolStripMenuItem.Checked = false;
        }

        private void lastNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clientIdToolStripMenuItem.Checked = false;
            firstNameToolStripMenuItem.Checked = false;
            lastNameToolStripMenuItem.Checked = true;
            productToolStripMenuItem.Checked = false;
        }

        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clientIdToolStripMenuItem.Checked = false;
            firstNameToolStripMenuItem.Checked = false;
            lastNameToolStripMenuItem.Checked = false;
            productToolStripMenuItem.Checked = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string serializationFile = Path.Combine("myFile.txt");
            using (Stream stream = File.Open(serializationFile, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream,persons);
            }
            SFTPFileTransfer.Send("myFile.txt");

        }
   
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SFTPFileTransfer.downloadFile();
            loadDeserialization();
            addPersonInView();
        }
        private void loadDeserialization()
        {
            using (Stream stream = File.Open("myFile.txt", FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                persons = (List<Person>)bformatter.Deserialize(stream);
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
  }

