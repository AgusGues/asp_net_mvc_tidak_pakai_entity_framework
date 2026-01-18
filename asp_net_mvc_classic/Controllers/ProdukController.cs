using Microsoft.AspNetCore.Mvc;
using asp_net_mvc_classic.Data;
using asp_net_mvc_classic.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace asp_net_mvc_classic.Controllers
{
    public class ProdukController : Controller
    {
        private readonly DbHelper _db;

        public ProdukController(DbHelper db)
        {
            _db = db;
        }

        //read
        public IActionResult Index()
        {
            List<Produk>list = new List<Produk>();
            
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "select * from produk";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    list.Add(new Produk
                    {
                        Id = (int)reader["id"],
                        NamaProduk = reader["NamaProduk"].ToString(),
                        Harga = (decimal)reader["Harga"],
                        Stok = (int)reader["Stok"]
                    });
                }

                return View(list);
            }
        }

        //Create
        [HttpPost]
        public IActionResult Create(Produk produk)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "insert into produk(NamaProduk,Harga,Stok)values(@Nama,@Harga,@Stok)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nama", produk.NamaProduk);
                cmd.Parameters.AddWithValue("@Harga", produk.Harga);
                cmd.Parameters.AddWithValue("@Stok", produk.Stok);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        //Edit untuk Id
        public IActionResult Edit(int Id)
        {
            Produk produk = new Produk();

            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "select * from produk where id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", Id);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read()) 
                {
                    produk.Id = (int)reader["id"];
                    produk.NamaProduk = reader["NamaProduk"].ToString();
                    produk.Harga = (decimal)reader["Harga"];
                    produk.Stok = (int)reader["Stok"];
                }
            }

            return View(produk);
        }

        [HttpPost]
        public IActionResult Edit(Produk produk)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = @"update produk set NamaProduk = @Nama, Harga=@Harga, @Stok=Stok where @Id=Id";
                SqlCommand cmd = new SqlCommand(query,conn);
                cmd.Parameters.AddWithValue("@Id", produk.Id);
                cmd.Parameters.AddWithValue("@Nama",produk.NamaProduk);
                cmd.Parameters.AddWithValue("@Harga", produk.Harga);
                cmd.Parameters.AddWithValue("@Stok",produk.Stok);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        //Delete
        public IActionResult Delete(int Id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                string query = "Delete from produk where id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", Id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }


    }
}
