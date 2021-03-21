using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace DinhXuanGiang_5951071019
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            GetStudentsRecord();
        }
        //Kết nối DB
        SqlConnection con = new SqlConnection(@"Data Source=OGA;Initial Catalog=DemoCRUD;Integrated Security=True");

        private void GetStudentsRecord()
        {
            //Truy vấn DB
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM StudentsTb", con);
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            StudentRecordData.DataSource = dt;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }

        private bool IsValidData()
        {
            if(txtHName.Text == string.Empty 
                || txtNName.Text == string.Empty 
                || txtAddress.Text == string.Empty 
                || string.IsNullOrEmpty(txtPhone.Text) 
                || string.IsNullOrEmpty(txtRoll.Text))
            {
                MessageBox.Show("Có chỗ chưa nhập dữ liệu !!!",
                    "Lỗi dữ liệu", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            return true;
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            if (IsValidData())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO StudentsTb VALUES " +
                    "(@Name, @FatherName, @RollNumber, @Address, @Mobile)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", txtHName.Text);
                cmd.Parameters.AddWithValue("@Fathername", txtNName.Text);
                cmd.Parameters.AddWithValue("@RollNumber", txtRoll.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtPhone.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                GetStudentsRecord();
            }
        }

        public int StudentID;
        private void StudentRecordData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentID = Convert.ToInt32(StudentRecordData.Rows[0].Cells[0].Value);
            txtHName.Text = StudentRecordData.SelectedRows[0].Cells[1].Value.ToString();
            txtNName.Text = StudentRecordData.SelectedRows[0].Cells[2].Value.ToString();
            txtRoll.Text = StudentRecordData.SelectedRows[0].Cells[3].Value.ToString();
            txtAddress.Text = StudentRecordData.SelectedRows[0].Cells[4].Value.ToString();
            txtPhone.Text = StudentRecordData.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE StudentsTb SET " +
                    "Name = @Name, Fathername = @Fathername," +
                    "RollNumber = @RollNumber, Address = @Address," +
                    "Mobile = @Mobile WHERE StudentID = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Name", txtHName.Text);
                cmd.Parameters.AddWithValue("@Fathername", txtNName.Text);
                cmd.Parameters.AddWithValue("@RollNumber", txtRoll.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Mobile", txtPhone.Text);
                cmd.Parameters.AddWithValue("@ID", this.StudentID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                GetStudentsRecord();
                ResetData();
            } 
            else
            {
                MessageBox.Show("Cập nhật bị lỗi !!!", "Lỗi !",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void ResetData()
        {
            throw new NotImplementedException();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM StudentsTb WHERE StudentID = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", this.StudentID);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                GetStudentsRecord();
                ResetData();
            } 
            else
            {
                MessageBox.Show("Xóa thành công!", "Thông báo?", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát!", "Thông báo?", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();
        }
    }
}
