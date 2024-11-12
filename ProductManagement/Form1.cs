using System.Data;
using System.Data.SqlClient;

namespace ProductManagement
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        public Form1()
        {
            InitializeComponent();
            connection = new SqlConnection("Server=CNTT1\\SQLEXPRESS;Database=product_management;Integrated Security = true;");
        }
        public Form1(string username)
        {
            InitializeComponent();
            connection = new SqlConnection("Server=CNTT1\\SQLEXPRESS;Database=product_management;Integrated Security = true;");
            lbUser.Text = username;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            connection.Open();
            MessageBox.Show(this, "Successful connection!", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
            FillData();
            GetCategories();
        }

        public void FillData()
        {
            string query = "select * from product";
            DataTable tbl = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            adapter.Fill(tbl);
            dgvProduct.DataSource = tbl;
            connection.Close();
        }

        public void GetCategories()
        {
            string query = "select category_id, category_name from category";
            DataTable table = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            adapter.Fill(table);
            cbCategory.DataSource = table;
            cbCategory.DisplayMember = "category_name";
            cbCategory.ValueMember = "category_id";
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int error = 0;
            string id = txtID.Text;
            if (id.Equals(""))
            {
                error = error + 1;
                lbIDError.Text = "ID can't be blank";
            }
            else
                lbIDError.Text = "";

            string name = txtName.Text;
            if (name.Equals(""))
            {
                error = error + 1;
                lbNameError.Text = "Name can't be blank";
            }
            else
                lbNameError.Text = "";

            string quantity = txtQuantity.Text;
            if (quantity.Equals(""))
            {
                error = error + 1;
                lbQuantityError.Text = "Quantity can't be blank";
            }
            else
            {
                string query = "select * from product where product_id = @id";
                connection.Open();
                SqlCommand cmdCheck = new SqlCommand(query, connection);
                cmdCheck.Parameters.Add("@id", SqlDbType.Int);
                cmdCheck.Parameters["@id"].Value = id;
                SqlDataReader reader = cmdCheck.ExecuteReader();
                if (reader.Read())
                {
                    error++;
                    lbIDError.Text = "This ID is existing, please choose another";
                }
                else
                {
                    lbQuantityError.Text = "";
                }
                connection.Close();
            }
            string catid = cbCategory.SelectedValue.ToString();
            if (error == 0)
            {
                string insert = "insert into product values (@id, @name, @quantity, @catid)";
                connection.Open();
                SqlCommand cmd = new SqlCommand(insert, connection);
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = id;
                cmd.Parameters.Add("@name", SqlDbType.VarChar);
                cmd.Parameters["@name"].Value = name;
                cmd.Parameters.Add("@quantity", SqlDbType.Int);
                cmd.Parameters["@quantity"].Value = quantity;
                cmd.Parameters.Add("@catid", SqlDbType.Int);
                cmd.Parameters["@catid"].Value = catid;
                cmd.ExecuteNonQuery();
                FillData();
                MessageBox.Show(this, "Inserted successfully", "Result", MessageBoxButtons.OK, MessageBoxIcon.None);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtID.Text = "";
            txtName.Text = "";
            txtQuantity.Text = "";
            cbCategory.Text = "";
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Dispose();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show(this, "Do you want to edit?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                string update = "update product set product_name = @name, quantity = @quantity where product_id = @productid";
                connection.Open();
                SqlCommand cmd = new SqlCommand(update, connection);
                cmd.Parameters.Add("@name", SqlDbType.VarChar);
                cmd.Parameters["@name"].Value = txtName.Text;
                cmd.Parameters.Add("@quantity", SqlDbType.Int);
                cmd.Parameters["@quantity"].Value = txtQuantity.Text;
                cmd.Parameters.Add("@productid", SqlDbType.Int);
                cmd.Parameters["@productid"].Value = txtID.Text;
                int i = cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    FillData();
                    MessageBox.Show(this, "Updated successfully", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dgvProduct.Rows[e.RowIndex];
                txtID.Text = row.Cells["ProductID"].Value.ToString();
                txtName.Text = row.Cells["ProductName"].Value.ToString();
                txtQuantity.Text = row.Cells["quantity"].Value.ToString();
                cbCategory.SelectedValue = row.Cells["CategoryID"].Value.ToString();
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if ((MessageBox.Show(this, "Do you want to delete?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                connection.Open();
                string delete = "delete from product where product_id = @productid";
                SqlCommand cmd = new SqlCommand(delete, connection);
                cmd.Parameters.Add("@productid", SqlDbType.Int);
                cmd.Parameters["@productid"].Value = txtID.Text;
                cmd.ExecuteNonQuery();
                FillData();
                MessageBox.Show(this, "Deleted successfully", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void InitializeComponent()
        {
            dgvProduct = new DataGridView();
            ProductID = new DataGridViewTextBoxColumn();
            ProductName = new DataGridViewTextBoxColumn();
            quantity = new DataGridViewTextBoxColumn();
            CategoryID = new DataGridViewTextBoxColumn();
            label1 = new Label();
            label2 = new Label();
            btnLogout = new Button();
            label3 = new Label();
            labelID = new Label();
            labelName = new Label();
            labelQuantity = new Label();
            labelCategory = new Label();
            txtID = new TextBox();
            txtName = new TextBox();
            txtQuantity = new TextBox();
            cbCategory = new ComboBox();
            btnInsert = new Button();
            btnCancel = new Button();
            lbIDError = new Label();
            lbQuantityError = new Label();
            lbNameError = new Label();
            lbUser = new Label();
            btnUpdate = new Button();
            btnDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvProduct).BeginInit();
            SuspendLayout();
            // 
            // dgvProduct
            // 
            dgvProduct.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProduct.Columns.AddRange(new DataGridViewColumn[] { ProductID, ProductName, quantity, CategoryID });
            dgvProduct.Location = new Point(209, 271);
            dgvProduct.Name = "dgvProduct";
            dgvProduct.Size = new Size(443, 150);
            dgvProduct.TabIndex = 0;
            dgvProduct.CellClick += dgvProduct_CellClick;
            // 
            // ProductID
            // 
            ProductID.DataPropertyName = "product_id";
            ProductID.HeaderText = "ProductID";
            ProductID.Name = "ProductID";
            // 
            // ProductName
            // 
            ProductName.DataPropertyName = "product_name";
            ProductName.HeaderText = "Name";
            ProductName.Name = "ProductName";
            // 
            // quantity
            // 
            quantity.DataPropertyName = "quantity";
            quantity.HeaderText = "Quantity";
            quantity.Name = "quantity";
            // 
            // CategoryID
            // 
            CategoryID.DataPropertyName = "category_id";
            CategoryID.HeaderText = "CategoryID";
            CategoryID.Name = "CategoryID";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(355, 236);
            label1.Name = "label1";
            label1.Size = new Size(136, 23);
            label1.TabIndex = 1;
            label1.Text = "List of products";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(321, 25);
            label2.Name = "label2";
            label2.Size = new Size(227, 30);
            label2.TabIndex = 2;
            label2.Text = "Product Management";
            // 
            // btnLogout
            // 
            btnLogout.BackColor = SystemColors.ScrollBar;
            btnLogout.Location = new Point(752, 25);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(81, 40);
            btnLogout.TabIndex = 3;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(34, 25);
            label3.Name = "label3";
            label3.Size = new Size(45, 20);
            label3.TabIndex = 4;
            label3.Text = "User: ";
            // 
            // labelID
            // 
            labelID.AutoSize = true;
            labelID.Location = new Point(135, 85);
            labelID.Name = "labelID";
            labelID.Size = new Size(63, 15);
            labelID.TabIndex = 5;
            labelID.Text = "Product ID";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new Point(135, 136);
            labelName.Name = "labelName";
            labelName.Size = new Size(84, 15);
            labelName.TabIndex = 6;
            labelName.Text = "Product Name";
            // 
            // labelQuantity
            // 
            labelQuantity.AutoSize = true;
            labelQuantity.Location = new Point(487, 85);
            labelQuantity.Name = "labelQuantity";
            labelQuantity.Size = new Size(53, 15);
            labelQuantity.TabIndex = 7;
            labelQuantity.Text = "Quantity";
            // 
            // labelCategory
            // 
            labelCategory.AutoSize = true;
            labelCategory.Location = new Point(485, 141);
            labelCategory.Name = "labelCategory";
            labelCategory.Size = new Size(55, 15);
            labelCategory.TabIndex = 8;
            labelCategory.Text = "Category";
            // 
            // txtID
            // 
            txtID.Location = new Point(229, 77);
            txtID.Name = "txtID";
            txtID.Size = new Size(129, 23);
            txtID.TabIndex = 9;
            // 
            // txtName
            // 
            txtName.Location = new Point(229, 133);
            txtName.Name = "txtName";
            txtName.Size = new Size(129, 23);
            txtName.TabIndex = 10;
            // 
            // txtQuantity
            // 
            txtQuantity.Location = new Point(562, 77);
            txtQuantity.Name = "txtQuantity";
            txtQuantity.Size = new Size(121, 23);
            txtQuantity.TabIndex = 11;
            // 
            // cbCategory
            // 
            cbCategory.FormattingEnabled = true;
            cbCategory.Location = new Point(562, 133);
            cbCategory.Name = "cbCategory";
            cbCategory.Size = new Size(121, 23);
            cbCategory.TabIndex = 12;
            // 
            // btnInsert
            // 
            btnInsert.BackColor = SystemColors.ScrollBar;
            btnInsert.Location = new Point(183, 186);
            btnInsert.Name = "btnInsert";
            btnInsert.Size = new Size(84, 39);
            btnInsert.TabIndex = 13;
            btnInsert.Text = "Insert";
            btnInsert.UseVisualStyleBackColor = false;
            btnInsert.Click += btnInsert_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = SystemColors.ScrollBar;
            btnCancel.Location = new Point(606, 186);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(77, 39);
            btnCancel.TabIndex = 14;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // lbIDError
            // 
            lbIDError.AutoSize = true;
            lbIDError.ForeColor = Color.Red;
            lbIDError.Location = new Point(229, 103);
            lbIDError.Name = "lbIDError";
            lbIDError.Size = new Size(0, 15);
            lbIDError.TabIndex = 15;
            // 
            // lbQuantityError
            // 
            lbQuantityError.AutoSize = true;
            lbQuantityError.ForeColor = Color.Red;
            lbQuantityError.ImageAlign = ContentAlignment.TopLeft;
            lbQuantityError.Location = new Point(562, 103);
            lbQuantityError.Name = "lbQuantityError";
            lbQuantityError.Size = new Size(0, 15);
            lbQuantityError.TabIndex = 16;
            // 
            // lbNameError
            // 
            lbNameError.AutoSize = true;
            lbNameError.ForeColor = Color.Red;
            lbNameError.Location = new Point(229, 159);
            lbNameError.Name = "lbNameError";
            lbNameError.Size = new Size(0, 15);
            lbNameError.TabIndex = 17;
            // 
            // lbUser
            // 
            lbUser.AutoSize = true;
            lbUser.Location = new Point(85, 29);
            lbUser.Name = "lbUser";
            lbUser.Size = new Size(0, 15);
            lbUser.TabIndex = 18;
            // 
            // btnUpdate
            // 
            btnUpdate.BackColor = Color.LightGray;
            btnUpdate.Location = new Point(321, 186);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(75, 39);
            btnUpdate.TabIndex = 19;
            btnUpdate.Text = "Update";
            btnUpdate.UseVisualStyleBackColor = false;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.BackColor = Color.LightGray;
            btnDelete.Location = new Point(465, 186);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 39);
            btnDelete.TabIndex = 20;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(905, 519);
            Controls.Add(btnDelete);
            Controls.Add(btnUpdate);
            Controls.Add(lbUser);
            Controls.Add(lbNameError);
            Controls.Add(lbQuantityError);
            Controls.Add(lbIDError);
            Controls.Add(btnCancel);
            Controls.Add(btnInsert);
            Controls.Add(cbCategory);
            Controls.Add(txtQuantity);
            Controls.Add(txtName);
            Controls.Add(txtID);
            Controls.Add(labelCategory);
            Controls.Add(labelQuantity);
            Controls.Add(labelName);
            Controls.Add(labelID);
            Controls.Add(label3);
            Controls.Add(btnLogout);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dgvProduct);
            Name = "Form1";
            Load += Form1_Load_1;
            ((System.ComponentModel.ISupportInitialize)dgvProduct).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dgvProduct;
        private Label label1;
        private Label label2;
        private Button btnLogout;
        private Label label3;
        private Label labelID;
        private Label labelName;
        private Label labelQuantity;
        private Label labelCategory;
        private TextBox txtID;
        private TextBox txtName;
        private TextBox txtQuantity;
        private ComboBox cbCategory;
        private Button btnInsert;
        private Button btnCancel;
        private Label lbIDError;
        private Label lbQuantityError;
        private Label lbNameError;
        private Label lbUser;
        private Button btnUpdate;
        private DataGridViewTextBoxColumn ProductID;
        private DataGridViewTextBoxColumn ProductName;
        private DataGridViewTextBoxColumn quantity;
        private DataGridViewTextBoxColumn CategoryID;
        private Button btnDelete;
    }
}
