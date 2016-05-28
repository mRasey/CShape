using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Record
{
    class Program
    {
        string path = @"Provider=Microsoft.ACE.OLEDB.12.0;Data source=MyData.accdb;Persist Security Info=False";
        static string SELECT_DATA = "select * from 学生信息";
        private OleDbConnection connect;//链接数据库对象
        private OleDbCommandBuilder command;
        private OleDbDataAdapter dataAdapter;//数据库结果链接对象
        private DataTable dataTable = new DataTable();//数据表

        public Program()
        {
            connect = new OleDbConnection(path);
            try
            {
                connect.Open();
                Console.WriteLine("数据库链接成功");                   
            }
            catch (Exception)
            {
                Console.WriteLine("数据库链接失败");
                return;             
            }
            dataAdapter = new OleDbDataAdapter(SELECT_DATA, connect);
            command = new OleDbCommandBuilder(dataAdapter);
            dataAdapter.Fill(dataTable);//填充数据表
        }

        public void insertInfo(string input)//插入信息
        {
            dataAdapter.InsertCommand = command.GetInsertCommand();
            dataAdapter.UpdateCommand = command.GetUpdateCommand();
            string[] inputs = Regex.Split(input, " ");
            if (inputs.Length != 8)
                Console.WriteLine("错误的输入格式");
            else
            {
                DataRow dr = dataTable.NewRow();
                dr["学号"] = int.Parse(inputs[0]);
                dr["姓名"] = inputs[1];
                dr["性别"] = inputs[2];
                dr["班级"] = int.Parse(inputs[3]);
                dr["邮箱"] = inputs[4];
                dr["成绩1"] = int.Parse(inputs[5]);
                dr["成绩2"] = int.Parse(inputs[6]);
                dr["成绩3"] = int.Parse(inputs[7]);
                dataTable.Rows.Add(dr);
                dataAdapter.Update(dataTable);
                Console.WriteLine("输入成功");
            }
        }

        public void getInfoByID(int ID)//通过ID获取信息
        {
            DataRow[] dr = dataTable.Select("学号=" + ID);
            if (dr.Length == 0)
                Console.WriteLine("没有这个ID的学生");
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    Console.Write(dr[0][i] + " ");
                }
                Console.WriteLine();
            }
        }

        public void getInfoByName(string name)//通过姓名获取信息
        {
            name = "'" + name + "'";
            DataRow[] dr = dataTable.Select("姓名=" + name);
            if (dr.Length == 0)
                Console.WriteLine("没有这个姓名的学生");
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Console.Write(dr[i][j] + " ");
                    }
                    Console.WriteLine();
                }
            }
        }
        
        public void getInfoByEmail(string email)//通过邮箱获取信息
        {
            email = "'" + email + "'";
            DataRow[] dr = dataTable.Select("邮箱=" + email);
            if (dr.Length == 0)
                Console.WriteLine("没有这个邮箱的学生");
            else
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Console.Write(dr[i][j] + " ");
                    }
                    Console.WriteLine();
                }
            }
        } 

        static void Main(string[] args)
        {
            Program p = new Program();
            while (true)
            {
                try
                {
                    string input = Console.ReadLine();
                    if (input.Equals("exit"))//按exit退出程序
                        break;
                    else if (input.Equals("insert"))//insert插入信息
                    {
                        p.insertInfo(Console.ReadLine());
                    }
                    else//读取信息
                    {
                        string[] inputs = Regex.Split(input, " ");
                        if (inputs[0].Equals("read"))
                        {
                            if (inputs[1].Equals("id"))
                                p.getInfoByID(int.Parse(inputs[2]));
                            else if (inputs[1].Equals("email"))
                                p.getInfoByEmail(inputs[2]);
                            else if (inputs[1].Equals("name"))
                                p.getInfoByName(inputs[2]);
                            else
                                Console.WriteLine("错误的输入格式");
                        }
                        else
                            Console.WriteLine("错误的输入格式");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("错误的输入格式");
                    continue;
                }
            }
            Console.WriteLine("程序结束");
            p.connect.Close();
        }
    }
}