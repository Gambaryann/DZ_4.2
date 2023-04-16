using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DZ_4_Zadachi
{
    public partial class Form1 : Form
    {

        DB_Connection DataBase = new DB_Connection();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            createcolumns2();
            refreshdatagrid2();

        }

        private void createcolumns2()
        {
            dataGridView1.Columns.Add("ID", "id");
            dataGridView1.Columns.Add("Name", "Название");
            var checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.HeaderText = "Выполнено?";
            dataGridView1.Columns.Add(checkColumn);
            dataGridView1.Columns[0].Visible = false;
        }

        private void readsinglerow2(IDataRecord rec)
        {
            dataGridView1.Rows.Add(rec.GetInt32(0), rec.GetString(1), rec.GetBoolean(2));
        }

        private void refreshdatagrid2()
        {
            dataGridView1.Rows.Clear();

            String query = $"select * from Task";

            SqlCommand com = new SqlCommand(query, DataBase.GetConnection());

            DataBase.OpenConnection();

            SqlDataReader read = com.ExecuteReader();

            while (read.Read())
            {
                readsinglerow2(read);
            }

            read.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add_Task ap = new Add_Task();
            ap.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataBase.OpenConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                var title = dataGridView1.Rows[index].Cells[1].Value.ToString();
                var isDone = dataGridView1.Rows[index].Cells[2].Value.ToString();

                var changequery = $"update Task set Complete = '{isDone}', Name = '{title}' where ID = '{id}'";

                var command = new SqlCommand(changequery, DataBase.GetConnection());
                command.ExecuteNonQuery();
            }

            DataBase.CloseConnection();

            refreshdatagrid2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DataBase.OpenConnection();

            var selectedrowindex = dataGridView1.CurrentCell.RowIndex;

            var id = Convert.ToInt32(dataGridView1.Rows[selectedrowindex].Cells[0].Value);
            var deletequery = $"delete from Task where ID = '{id}'";

            var command = new SqlCommand(deletequery, DataBase.GetConnection());
            command.ExecuteNonQuery();

            DataBase.CloseConnection();

            refreshdatagrid2();
        }
    }
}
