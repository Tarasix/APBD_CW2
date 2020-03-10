using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CW2
{



    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Study { get; set; }
        public string Tryb { get; set; }
        public int Index { get; set; }
        public string Urodzenia { get; set; }
        public string Email { get; set; }
        public string MotherName { get; set; }
        public string FatherName { get; set; }
    }

    class StudentComparer : IEqualityComparer<Student>
    {
        public bool Equals(Student x, Student y)
        {
            if (x.Index != y.Index) return false;
            if (!x.Name.Equals(y.Name)) return false;
            if (!x.Surname.Equals(y.Surname)) return false;
            return true;
        }
        public int GetHashCode(Student obj)
        {
            return obj.Index;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {




            /*
            if (args[0] is null || args[1] is null )
            {
                args[0] = @"Data\dane.csv";
                args[1] = "result.xml";
                args[2] = "xml";
            }
            */

            string path = @"Data\dane.csv";


            var students = new HashSet<Student>(new StudentComparer());
            // var students = new List<Student>();

            var plik = new FileInfo(path);
            using (var stream = new StreamReader(plik.OpenRead()))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] info = line.Split(',');

                    var student = new Student
                    {
                        Name = info[0],
                        Surname = info[1],
                        Study = info[2],
                        Tryb = info[3],
                        Index = int.Parse(info[4]),
                        Urodzenia = info[5],
                        Email = info[6],
                        MotherName = info[7],
                        FatherName = info[8]
                    };

                    students.Add(student);
                }
            }



            FileStream writer = new FileStream("C:\\Users\\Taras\\Desktop\\dama2.xml", FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(List<Student>));

            var listStudents = new List<Student>();

            foreach (var stud in students)
            {
                listStudents.Add(stud);
            }
            serializer.Serialize(writer, listStudents);

            //Console.WriteLine(line);

        }
    }


}







